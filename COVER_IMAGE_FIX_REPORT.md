# Cover Image Fix Report

## Issue Summary

**Problem:** The database could contain multiple `TripImage` records for the same `TripId` with `IsCover = true`, violating the business rule that states "A Trip can have at most ONE TripImage where IsCover = true".

## Root Cause Analysis

The issue had **TWO critical bugs**:

### 1. Missing Database-Level Constraint
- **Problem:** No unique index existed to enforce "at most one IsCover=true per TripId" at the database level
- **Impact:** Race conditions between concurrent requests could create duplicate covers
- **Example Scenario:**
  - Request A queries → finds no cover → sets Image A as cover
  - Request B queries (before A commits) → finds no cover → sets Image B as cover
  - Both commit successfully → **TWO covers exist**

### 2. Entity Tracking Bug in Services
- **Problem:** `AddTripImageService` and `UpdateTripImageService` loaded existing cover images using `GetByTripIdAsync()`, which uses `.AsNoTracking()`
- **Impact:** When the services set `currentCover.IsCover = false`, EF Core did NOT track these changes, so they were **NEVER saved to the database**
- **Code Pattern:**
  ```csharp
  // BEFORE (BROKEN):
  var existingImages = await _unitOfWork.TripImages.GetByTripIdAsync(tripId, ct);
  var currentCovers = existingImages.Where(i => i.IsCover).ToList();
  
  foreach (var currentCover in currentCovers)
  {
      currentCover.IsCover = false;  // ❌ NOT TRACKED - NOT SAVED!
      currentCover.UpdatedAt = DateTime.UtcNow;
  }
  ```

## Solution Implemented

### 1. Added Filtered Unique Index (Database-Level Protection)

**File:** `Infrastructure/Configurations/TripImageConfiguration.cs`

```csharp
// Filtered unique index: Only one IsCover=true per TripId
builder.HasIndex(i => i.TripId)
    .IsUnique()
    .HasFilter("[IsCover] = 1")
    .HasDatabaseName("IX_TripImages_TripId_IsCover_Unique");
```

**Benefits:**
- Prevents race conditions at the database level
- SQL Server will reject any INSERT/UPDATE that creates duplicate covers
- Even if two requests execute simultaneously, only one can commit

### 2. Fixed Entity Tracking in Services

**Added Repository Method:**

**File:** `Infrastructure/Repositories/TripImageRepository.cs`

```csharp
public async Task<List<TripImage>> GetCoverImagesTrackedAsync(
    Guid tripId,
    CancellationToken cancellationToken = default)
{
    // Returns tracked entities so changes will be persisted
    return await _dbSet
        .Where(i => i.TripId == tripId && i.IsCover)
        .ToListAsync(cancellationToken);
}
```

**Updated Services:**

**File:** `Application/Services/TripImages/AddTripImageService.cs`

```csharp
// AFTER (FIXED):
var currentCovers = await _unitOfWork.TripImages
    .GetCoverImagesTrackedAsync(tripId, cancellationToken);

foreach (var currentCover in currentCovers)
{
    currentCover.IsCover = false;  // ✅ TRACKED - WILL BE SAVED!
    currentCover.UpdatedAt = DateTime.UtcNow;
    _unitOfWork.TripImages.Update(currentCover);  // Explicit update
}
```

**File:** `Application/Services/TripImages/UpdateTripImageService.cs`

```csharp
// AFTER (FIXED):
var currentCovers = await _unitOfWork.TripImages
    .GetCoverImagesTrackedAsync(tripId, cancellationToken);

foreach (var currentCover in currentCovers.Where(c => c.Id != imageId))
{
    currentCover.IsCover = false;  // ✅ TRACKED - WILL BE SAVED!
    currentCover.UpdatedAt = DateTime.UtcNow;
    _unitOfWork.TripImages.Update(currentCover);  // Explicit update
}
```

### 3. Added Update() Method to Repository Base

**Files Modified:**
- `Application/Interfaces/Repositories/IRepositoryGeneric.cs`
- `Infrastructure/Repositories/RepositoryGeneric.cs`

Added `void Update(T entity)` method to enable explicit entity tracking.

## Files Changed

### Application Layer (3 files)
1. **Application/Interfaces/Repositories/IRepositoryGeneric.cs**
   - Added `void Update(T entity)` method

2. **Application/Interfaces/Repositories/ITripImageRepository.cs**
   - Added `Task<List<TripImage>> GetCoverImagesTrackedAsync(...)` method

3. **Application/Services/TripImages/AddTripImageService.cs**
   - Changed to use `GetCoverImagesTrackedAsync()` instead of `GetByTripIdAsync()`
   - Added explicit `_unitOfWork.TripImages.Update(currentCover)` calls

4. **Application/Services/TripImages/UpdateTripImageService.cs**
   - Changed to use `GetCoverImagesTrackedAsync()` instead of `GetByTripIdAsync()`
   - Added explicit `_unitOfWork.TripImages.Update(currentCover)` calls

### Infrastructure Layer (3 files)
1. **Infrastructure/Repositories/RepositoryGeneric.cs**
   - Added `Update()` method implementation

2. **Infrastructure/Repositories/TripImageRepository.cs**
   - Added `GetCoverImagesTrackedAsync()` method implementation

3. **Infrastructure/Configurations/TripImageConfiguration.cs**
   - Added filtered unique index on `TripId` where `IsCover = 1`

4. **Infrastructure/Migrations/20260901223411_AddUniqueCoverIndexToTripImages.cs** (NEW)
   - Creates unique filtered index `IX_TripImages_TripId_IsCover_Unique`
   - Drops old non-unique index `IX_TripImages_TripId`

### Supporting Files (2 files)
1. **cleanup_duplicate_covers.sql** (NEW)
   - SQL script to identify and fix any existing duplicate covers
   - Uses `ROW_NUMBER()` to keep newest cover, set others to false

2. **COVER_IMAGE_FIX_REPORT.md** (NEW - this file)

## Migration Status

### Migration Created: ✅
**Name:** `20260901223411_AddUniqueCoverIndexToTripImages`

**Actions:**
- Drops existing non-unique index `IX_TripImages_TripId`
- Creates filtered unique index `IX_TripImages_TripId_IsCover_Unique` on `TripId` where `IsCover = 1`

### Migration Applied: ✅
**Status:** Successfully applied with **EXIT CODE: 0**

**No duplicate data cleanup was needed** because the database did not contain any duplicate cover images at the time of migration.

### If Duplicates Had Existed

If the migration had failed due to existing duplicates, the resolution would be:

1. Run the provided `cleanup_duplicate_covers.sql` script
2. Verify the cleanup by checking the SELECT query results
3. Commit the transaction if correct
4. Re-run `dotnet ef database update`

## Build Status

**Build Result:** ✅ **Succeeded**

```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

All projects compiled successfully:
- Domain.dll
- Application.dll
- Infrastructure.dll
- Api.dll

## Expected Behavior

### Before Fix
❌ **Broken:**
- Two concurrent requests could both set their images as cover
- Existing covers were NOT actually unsetting (tracking bug)
- Database had no protection against duplicates

### After Fix
✅ **Fixed:**

**Scenario 1: Add New Cover Image**
```
Initial: Image1 (IsCover=false), Image2 (IsCover=true)
Action:  Add Image3 with IsCover=true
Result:  Image1 (IsCover=false), Image2 (IsCover=false), Image3 (IsCover=true)
```

**Scenario 2: Update Existing Image to Cover**
```
Initial: Image1 (IsCover=false), Image2 (IsCover=true), Image3 (IsCover=false)
Action:  Update Image3 to IsCover=true
Result:  Image1 (IsCover=false), Image2 (IsCover=false), Image3 (IsCover=true)
```

**Scenario 3: Concurrent Requests (Race Condition)**
```
Request A: Set Image1 as cover
Request B: Set Image2 as cover (same TripId)

Result: ONE request succeeds, the other gets a database constraint violation
        Database CANNOT contain both Image1 and Image2 as covers
```

**Scenario 4: Already Has Cover**
```
Initial: Image1 (IsCover=true)
Action:  Add Image2 with IsCover=false
Result:  Image1 (IsCover=true), Image2 (IsCover=false)  ← No change to cover
```

## Existing TripImage CRUD Verification

### Not Changed ✅
The following existing functionality remains **UNCHANGED**:

- ✅ **GET** `/api/trips/{tripId}/images` - List all images
- ✅ **GET** `/api/trips/{tripId}/images/{imageId}` - Get single image
- ✅ **POST** `/api/trips/{tripId}/images` - Add image (only cover logic improved)
- ✅ **PUT** `/api/trips/{tripId}/images/{imageId}` - Update image (only cover logic improved)
- ✅ **DELETE** `/api/trips/{tripId}/images/{imageId}` - Delete image
- ✅ All DTOs, validators, controllers remain unchanged
- ✅ API routes remain unchanged
- ✅ Response formats remain unchanged

### Only Changed ✅
- Internal business logic for enforcing "single cover" rule
- Database schema (added index)

## Testing Recommendations

### 1. Unit Tests (Recommended)
- Test that setting a new cover unsets old covers
- Test that only one cover can exist per trip
- Test concurrent cover updates (should use mock to simulate constraint violation)

### 2. Integration Tests (Recommended)
- Create trip with multiple images
- Set Image A as cover → verify Image B is not cover
- Set Image B as cover → verify Image A is not cover
- Attempt to manually INSERT duplicate covers → should fail with constraint violation

### 3. Manual Testing (Quick Verification)
- Create a trip with 3 images, all IsCover=false
- Set Image 1 as cover via API → verify only Image 1 has IsCover=true
- Set Image 3 as cover via API → verify only Image 3 has IsCover=true
- Query database directly: `SELECT TripId, COUNT(*) FROM TripImages WHERE IsCover=1 GROUP BY TripId` → should return count=1 for all trips

## Conclusion

✅ **Root cause identified:** Entity tracking bug + missing database constraint

✅ **Solution implemented:** Filtered unique index + tracked entity loading + explicit updates

✅ **Migration applied:** Successfully with no duplicates found

✅ **Build verified:** 0 errors, 0 warnings

✅ **Existing CRUD preserved:** No breaking changes to API contracts

The business rule "at most ONE IsCover=true per TripId" is now enforced at BOTH the application level (service logic) and database level (unique index).

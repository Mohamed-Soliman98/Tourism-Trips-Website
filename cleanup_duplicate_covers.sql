-- ============================================================================
-- Data Cleanup Script: Remove Duplicate Cover Images
-- ============================================================================
-- This script identifies and fixes TripImages where multiple records have
-- IsCover = 1 for the same TripId.
--
-- Strategy:
-- For each Trip with multiple covers, keep the NEWEST cover (by CreatedAt DESC)
-- and set all others to IsCover = 0.
--
-- IMPORTANT: Review the SELECT query results before running the UPDATE!
-- ============================================================================

-- Step 1: View all trips with multiple cover images
SELECT 
    TripId,
    COUNT(*) AS CoverCount,
    STRING_AGG(CAST(Id AS VARCHAR(50)), ', ') AS CoverImageIds
FROM TripImages
WHERE IsCover = 1
GROUP BY TripId
HAVING COUNT(*) > 1
ORDER BY TripId;

-- Step 2: View detailed information about duplicate covers
SELECT 
    ti.TripId,
    ti.Id AS ImageId,
    ti.ImageUrl,
    ti.CreatedAt,
    ti.DisplayOrder,
    ROW_NUMBER() OVER (PARTITION BY ti.TripId ORDER BY ti.CreatedAt DESC) AS RowNum
FROM TripImages ti
WHERE ti.IsCover = 1
    AND ti.TripId IN (
        SELECT TripId 
        FROM TripImages 
        WHERE IsCover = 1 
        GROUP BY TripId 
        HAVING COUNT(*) > 1
    )
ORDER BY ti.TripId, ti.CreatedAt DESC;

-- Step 3: UPDATE - Keep only the newest cover per Trip
-- (RowNum = 1 means newest, others become false)
BEGIN TRANSACTION;

UPDATE TripImages
SET 
    IsCover = 0,
    UpdatedAt = GETUTCDATE()
WHERE Id IN (
    SELECT Id
    FROM (
        SELECT 
            Id,
            TripId,
            ROW_NUMBER() OVER (PARTITION BY TripId ORDER BY CreatedAt DESC) AS RowNum
        FROM TripImages
        WHERE IsCover = 1
    ) AS Ranked
    WHERE RowNum > 1
);

-- Verify the fix - should return 0 rows
SELECT 
    TripId,
    COUNT(*) AS CoverCount
FROM TripImages
WHERE IsCover = 1
GROUP BY TripId
HAVING COUNT(*) > 1;

-- If everything looks good, COMMIT. Otherwise, ROLLBACK.
-- COMMIT;
-- ROLLBACK;

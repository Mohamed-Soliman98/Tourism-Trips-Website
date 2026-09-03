SELECT 
    TripId,
    COUNT(*) AS CoverCount,
    STRING_AGG(CAST(Id AS VARCHAR(50)), ', ') AS CoverImageIds
FROM TripImages
WHERE IsCover = 1
GROUP BY TripId
HAVING COUNT(*) > 1
ORDER BY TripId;

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

SELECT 
    TripId,
    COUNT(*) AS CoverCount
FROM TripImages
WHERE IsCover = 1
GROUP BY TripId
HAVING COUNT(*) > 1;

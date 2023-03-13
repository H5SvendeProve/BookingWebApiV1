IF NOT EXISTS (SELECT *FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tr_ElectricityPrices_Insert]'))
BEGIN
EXEC('
CREATE TRIGGER [tr_ElectricityPrices_Insert]
ON electricityprices
AFTER INSERT
AS
BEGIN
    INSERT INTO DepartmentElectricityPrices (DepartmentName, Exr, TimeStart, TimeEnd, Location)
    SELECT d.DepartmentName, ep.Exr, ep.TimeStart, ep.TimeEnd, 
           CASE WHEN d.ZipCode > 4999 THEN ''WestDenmark''
                ELSE ''EastDenmark''
           END AS Location
    FROM Departments d
    INNER JOIN ElectricityPrices ep ON 
        (CASE WHEN d.ZipCode > 4999 THEN ''WestDenmark'' ELSE ''EastDenmark'' END) = ep.Location
    WHERE NOT EXISTS (
        SELECT 1 FROM DepartmentElectricityPrices dep
        WHERE dep.DepartmentName = d.DepartmentName
            AND dep.Exr = ep.Exr
            AND dep.TimeStart = ep.TimeStart
            AND dep.TimeEnd = ep.TimeEnd
            AND dep.Location = (CASE WHEN d.ZipCode > 4999 THEN ''WestDenmark'' ELSE ''EastDenmark'' END)
    )
END;')
end
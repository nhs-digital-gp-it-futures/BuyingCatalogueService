CREATE FUNCTION import.GetSupplierId(@catalogueItemId varchar(14))
RETURNS varchar(6) AS
BEGIN
    RETURN SUBSTRING(@catalogueItemId, 1, CHARINDEX('-', @catalogueItemId) - 1);
END;

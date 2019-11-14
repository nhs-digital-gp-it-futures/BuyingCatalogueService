DELETE FROM [FrameworkCapabilities]
DELETE FROM [FrameworkSolutions]
UPDATE [Solution] SET SolutionDetailId = NULL
DELETE FROM [SolutionDetail]
DELETE FROM [SolutionCapability]
DELETE FROM [Solution]
DELETE FROM [Capability]
DELETE FROM [Organisation]

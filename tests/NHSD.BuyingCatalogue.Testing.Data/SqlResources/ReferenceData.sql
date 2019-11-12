INSERT [dbo].[Framework] ([Id], [Name], [Description], [Owner], [ActiveDate], [ExpiryDate]) VALUES (N'NHSDGP001', N'NHS Digital GP Futures Framework 1', NULL, NULL, NULL, NULL)

INSERT INTO [dbo].[CapabilityStatus] ([Id] ,[Name]) VALUES (1,'Effective')

INSERT INTO [dbo].[CapabilityCategory] ([Id] ,[Name]) VALUES (0,'Undefined')

INSERT INTO [dbo].[SolutionCapabilityStatus] ([Id], [Name], [Pass]) VALUES (1, 'Passed', 0);

INSERT INTO [dbo].[SolutionSupplierStatus] ([Id], [Name]) VALUES (1, 'Draft');
INSERT INTO [dbo].[SolutionSupplierStatus] ([Id], [Name]) VALUES (2, 'Authority Review');

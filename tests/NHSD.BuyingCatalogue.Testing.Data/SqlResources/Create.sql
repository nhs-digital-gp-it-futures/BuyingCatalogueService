SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*-----------------------------------------------------------------------
--
-- CompliancyLevel
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[CompliancyLevel](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,
 CONSTRAINT [PK_CompliancyLevel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_CompliancyLevelName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/*-----------------------------------------------------------------------
--
-- CapabilityCategory
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[CapabilityCategory](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CapabilityCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_CapabilityCategoryName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/*-----------------------------------------------------------------------
--
-- CapabilityStatus
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[CapabilityStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,
 CONSTRAINT [PK_CapabilityStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_CapabilityStatusName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

 
/*-----------------------------------------------------------------------
--
-- Capability
-- Note: Clustering turned off on Id as uniqueidentifer, but enabled on 
-- CapabilityRef as Azure SQL requires one and only one per table
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[Capability](
	[Id] [uniqueidentifier] NOT NULL,
	[CapabilityRef] [varchar](10) NOT NULL,
	[Version] [varchar](10) NOT NULL,
	[PreviousVersion] [varchar](10) NULL,
	[StatusId] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[SourceUrl] [varchar](1000) NULL,
	[EffectiveDate] [date] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_Capability] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_CapabilityCapabilityRef] ON [dbo].[Capability]
(
	[CapabilityRef] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Capability]  WITH CHECK ADD  CONSTRAINT [FK_Capability_CapabilityStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[CapabilityStatus] ([Id])
GO

ALTER TABLE [dbo].[Capability] CHECK CONSTRAINT [FK_Capability_CapabilityStatus]
GO

ALTER TABLE [dbo].[Capability]  WITH CHECK ADD  CONSTRAINT [FK_Capability_CapabilityCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[CapabilityCategory] ([Id])
GO

ALTER TABLE [dbo].[Capability] CHECK CONSTRAINT [FK_Capability_CapabilityCategory]
GO

/*-----------------------------------------------------------------------
--
-- EpicCategory
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[EpicCategory](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,
 CONSTRAINT [PK_EpicCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_EpicCategoryStatusName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/*-----------------------------------------------------------------------
--
-- Epic
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[Epic](
	[Id] [uniqueidentifier] NOT NULL,
	[EpicRef] [varchar](16) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CapabilityId] [uniqueidentifier] NOT NULL,
	[SourceUrl] [varchar](max) NULL,
	[CategoryId] [int],
	[CompliancyLevelId] [int],
 CONSTRAINT [PK_Epic] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_EpicEpicRef] ON [dbo].[Epic]
(
	[EpicRef] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Epic]  WITH CHECK ADD  CONSTRAINT [FK_Epic_Capability] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[Capability] ([Id])
GO

ALTER TABLE [dbo].[Epic] CHECK CONSTRAINT [FK_Epic_Capability]
GO

ALTER TABLE [dbo].[Epic]  WITH CHECK ADD  CONSTRAINT [FK_Epic_EpicCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[EpicCategory] ([Id])
GO

ALTER TABLE [dbo].[Epic] CHECK CONSTRAINT [FK_Epic_EpicCategory]
GO

ALTER TABLE [dbo].[Epic]  WITH CHECK ADD  CONSTRAINT [FK_Epic_CompliancyLevel] FOREIGN KEY([CompliancyLevelId])
REFERENCES [dbo].[CompliancyLevel] ([Id])
GO

ALTER TABLE [dbo].[Epic] CHECK CONSTRAINT [FK_Epic_CompliancyLevel]
GO

/*-----------------------------------------------------------------------
--
-- StatandardStatus
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[StandardStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,	
 CONSTRAINT [PK_StandardStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_StandardStatusName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/*-----------------------------------------------------------------------
--
-- StandardCategory
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[StandardCategory](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,
 CONSTRAINT [PK_StandardCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_StandardCategoryName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/*-----------------------------------------------------------------------
--
-- Standard
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[Standard](
	[Id] [uniqueidentifier] NOT NULL,
	[StandardRef] [varchar](10) NOT NULL,
	[Version] [varchar](10) NOT NULL,
	[PreviousId] [int] NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[SourceUrl] [varchar](1000) NULL,
	[StatusId] [int] NOT NULL,
	[EffectiveDate] [date] NULL,
 CONSTRAINT [PK_Standard] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_StandardStandardRef] ON [dbo].[Standard]
(
	[StandardRef] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Standard]  WITH CHECK ADD  CONSTRAINT [FK_Standard_StandardCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[StandardCategory] ([Id])
GO

ALTER TABLE [dbo].[Standard] CHECK CONSTRAINT [FK_Standard_StandardCategory]
GO

ALTER TABLE [dbo].[Standard]  WITH CHECK ADD  CONSTRAINT [FK_Standard_StandardStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[StandardStatus] ([Id])
GO

ALTER TABLE [dbo].[Standard] CHECK CONSTRAINT [FK_Standard_StandardStatus]
GO

/*-----------------------------------------------------------------------
--
-- CapabilityStandards
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[CapabilityStandards](
	[CapabilityId] [uniqueidentifier] NOT NULL,
	[StandardId] [uniqueidentifier] NOT NULL,	
	[IsOptional] [bit] NOT NULL CONSTRAINT [DF_CapabilityStandards_IsOptional] DEFAULT 0,
 CONSTRAINT [PK_CapabilityStandards] PRIMARY KEY CLUSTERED 
(
	[CapabilityId] ASC,	
	[StandardId] ASC	
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CapabilityStandards]  WITH CHECK ADD  CONSTRAINT [FK_CapabilityStandards_Capability] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[Capability] ([Id])
GO

ALTER TABLE [dbo].[CapabilityStandards] CHECK CONSTRAINT [FK_CapabilityStandards_Capability]
GO

ALTER TABLE [dbo].[CapabilityStandards]  WITH CHECK ADD  CONSTRAINT [FK_CapabilityStandards_Standard] FOREIGN KEY([StandardId])
REFERENCES [dbo].[Standard] ([Id])
GO

ALTER TABLE [dbo].[CapabilityStandards] CHECK CONSTRAINT [FK_CapabilityStandards_Standard]
GO

/*-----------------------------------------------------------------------
--
-- Framework
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[Framework](
	[Id] [varchar](10) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NULL,
	[Owner] [varchar](100) NULL,
	[ActiveDate] [date] NULL,
	[ExpiryDate] [date] NULL,
 CONSTRAINT [PK_Framework] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/*-----------------------------------------------------------------------
--
-- FramworkCapabilities
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[FrameworkCapabilities](
	[FrameworkId] [varchar](10) NOT NULL,
	[CapabilityId] [uniqueidentifier] NOT NULL,	
	[IsFoundation] [bit] NOT NULL CONSTRAINT [DF_FrameworkCapabilities_IsFoundation] DEFAULT 0,
 CONSTRAINT [PK_FrameworkCapabilities] PRIMARY KEY CLUSTERED 
(
	[FrameworkId] ASC,
	[CapabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FrameworkCapabilities]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkCapabilities_Capability] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[Capability] ([Id])
GO

ALTER TABLE [dbo].[FrameworkCapabilities] CHECK CONSTRAINT [FK_FrameworkCapabilities_Capability]
GO

ALTER TABLE [dbo].[FrameworkCapabilities]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkCapabilities_Framework] FOREIGN KEY([FrameworkId])
REFERENCES [dbo].[Framework] ([Id])
GO

ALTER TABLE [dbo].[FrameworkCapabilities] CHECK CONSTRAINT [FK_FrameworkCapabilities_Framework]
GO


/*-----------------------------------------------------------------------
--
-- FramworkStandards
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[FrameworkStandards](
	[FrameworkId] [varchar](10) NOT NULL,
	[StandardId] [uniqueidentifier] NOT NULL,	
 CONSTRAINT [PK_FrameworkStandards] PRIMARY KEY CLUSTERED 
(
	[FrameworkId] ASC,
	[StandardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FrameworkStandards]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkStandards_Framework] FOREIGN KEY([FrameworkId])
REFERENCES [dbo].[Framework] ([Id])
GO

ALTER TABLE [dbo].[FrameworkStandards] CHECK CONSTRAINT [FK_FrameworkStandards_Framework]
GO

ALTER TABLE [dbo].[FrameworkStandards]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkStandards_Standard] FOREIGN KEY([StandardId])
REFERENCES [dbo].[Standard] ([Id])
GO

ALTER TABLE [dbo].[FrameworkStandards] CHECK CONSTRAINT [FK_FrameworkStandards_Standard]
GO

/*-----------------------------------------------------------------------
--
-- Organisation
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[Organisation](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[OdsCode] [varchar](8) NULL,
	[PrimaryRoleId] [varchar](8) NULL,
	[CrmRef] [uniqueidentifier] NULL,
	[Address] [nvarchar](500) NULL,
	[CatalogueAgreementSigned] [bit] NOT NULL CONSTRAINT [DF_Organisation_CatalogueAgreement] DEFAULT 0,
	[Deleted] bit NOT NULL CONSTRAINT [DF_Organisation_Deleted] DEFAULT 0,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Organisation] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_OrganisationName] ON [dbo].[Organisation]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


/*-----------------------------------------------------------------------
--
-- Supplier
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[Supplier](
	[Id] [varchar](6) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[LegalName] [varchar](255) NOT NULL,
	[Summary] [varchar](1000) NULL,
	[SupplierUrl] [varchar](1000) NULL,
	[Address] [nvarchar](500) NULL,
	[OdsCode] [varchar](8) NULL,
	[CrmRef] [uniqueidentifier] NULL,	
	[Deleted] bit NOT NULL CONSTRAINT [DF_Supplier_Deleted] DEFAULT 0,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/*-----------------------------------------------------------------------
--
-- SupplierContact
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SupplierContact](
	[Id] [uniqueidentifier] NOT NULL,	
	[SupplierId] [varchar](6) NOT NULL,
	[FirstName] [varchar](35) NOT NULL,
	[LastName] [varchar](35) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[PhoneNumber] [varchar](35) NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SupplierContact] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_SupplierContactSupplierId] ON [dbo].[SupplierContact]
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SupplierContact]  WITH CHECK ADD  CONSTRAINT [FK_SupplierContact_Supplier] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Supplier] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SupplierContact] CHECK CONSTRAINT [FK_SupplierContact_Supplier]
GO

/*-----------------------------------------------------------------------
--
-- PublicationStatus
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[PublicationStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,	
 CONSTRAINT [PK_PublicationStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_PublicationStatusName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/*-----------------------------------------------------------------------
--
-- SolutionAuthorityStatus
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionAuthorityStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,	
 CONSTRAINT [PK_SolutionAuthorityStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_SolutionAuthorityStatusName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/*-----------------------------------------------------------------------
--
-- SolutionSupplierStatus
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionSupplierStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,	
 CONSTRAINT [PK_SolutionSupplierStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_SolutionSupplierStatusName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/*-----------------------------------------------------------------------
--
-- Solution
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[Solution](
	[Id] [varchar](14) NOT NULL,
	[ParentId] [varchar](14) NULL,	
	[SupplierId] [varchar](6) NOT NULL,	
	[SolutionDetailId] [uniqueidentifier] NULL,	
	[Name] [varchar](255) NOT NULL,
	[Version] [varchar](10) NULL,
	[PublishedStatusId] [int] NOT NULL CONSTRAINT [DF_Solution_PublishedStatus] DEFAULT 1,
	[AuthorityStatusId] [int] NOT NULL CONSTRAINT [DF_Solution_AuthorityStatus] DEFAULT 1,
	[SupplierStatusId] [int] NOT NULL CONSTRAINT [DF_Solution_SupplierStatus] DEFAULT 1,	
	[OnCatalogueVersion] [int] NOT NULL CONSTRAINT [DF_Solution_OnCatalogueVersion] DEFAULT 0,	
	[ServiceLevelAgreement] [nvarchar](1000) NULL,
	[WorkOfPlan] [nvarchar](max) NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Solution] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Solution]  WITH CHECK ADD  CONSTRAINT [FK_Solution_Supplier] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Supplier] ([Id])
GO

ALTER TABLE [dbo].[Solution] CHECK CONSTRAINT [FK_Solution_Supplier]
GO

ALTER TABLE [dbo].[Solution]  WITH CHECK ADD  CONSTRAINT [FK_Solution_Parent] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Solution] ([Id])
GO

ALTER TABLE [dbo].[Solution] CHECK CONSTRAINT [FK_Solution_Parent]
GO

ALTER TABLE [dbo].[Solution]  WITH CHECK ADD  CONSTRAINT [FK_Solution_PublicationStatus] FOREIGN KEY([PublishedStatusId])
REFERENCES [dbo].[PublicationStatus] ([Id])
GO

ALTER TABLE [dbo].[Solution] CHECK CONSTRAINT [FK_Solution_PublicationStatus]
GO

ALTER TABLE [dbo].[Solution]  WITH CHECK ADD  CONSTRAINT [FK_Solution_AuthorityStatus] FOREIGN KEY([AuthorityStatusId])
REFERENCES [dbo].[SolutionAuthorityStatus] ([Id])
GO

ALTER TABLE [dbo].[Solution] CHECK CONSTRAINT [FK_Solution_AuthorityStatus]
GO

ALTER TABLE [dbo].[Solution]  WITH CHECK ADD  CONSTRAINT [FK_Solution_SupplierStatus] FOREIGN KEY([SupplierStatusId])
REFERENCES [dbo].[SolutionSupplierStatus] ([Id])
GO

ALTER TABLE [dbo].[Solution] CHECK CONSTRAINT [FK_Solution_SupplierStatus]
GO


/*-----------------------------------------------------------------------
--
-- SolutionDetail
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[SolutionId] [varchar](14) NOT NULL,
	[PublishedStatusId] [int] NOT NULL CONSTRAINT [DF_SolutionDetail_PublishedStatus] DEFAULT 1,
	[Features] [nvarchar](max) NULL,
	[ClientApplication] [nvarchar](max) NULL,
	[Hosting] [nvarchar](max) NULL,
	[ImplementationDetail] [nvarchar](max) NULL,
	[RoadMap] [varchar](1000) NULL,
	[RoadMapImageUrl] [varchar](1000) NULL,	
	[AboutUrl] [varchar](1000) NULL,	
	[Summary] [varchar](300) NULL,
	[FullDescription] [varchar](3000) NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SolutionDetail] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_SolutionDetail] ON [dbo].[SolutionDetail]
(
	[SolutionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolutionDetail]  WITH CHECK ADD  CONSTRAINT [FK_SolutionDetail_Solution] FOREIGN KEY([SolutionId])
REFERENCES [dbo].[Solution] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SolutionDetail] CHECK CONSTRAINT [FK_SolutionDetail_Solution]
GO

ALTER TABLE [dbo].[SolutionDetail]  WITH CHECK ADD  CONSTRAINT [FK_SolutionDetail_PublicationStatus] FOREIGN KEY([PublishedStatusId])
REFERENCES [dbo].[PublicationStatus] ([Id])
GO

ALTER TABLE [dbo].[SolutionDetail] CHECK CONSTRAINT [FK_SolutionDetail_PublicationStatus]
GO

ALTER TABLE [dbo].[Solution]  WITH CHECK ADD  CONSTRAINT [FK_Solution_SolutionDetail] FOREIGN KEY([SolutionDetailId])
REFERENCES [dbo].[SolutionDetail] ([Id])
GO

ALTER TABLE [dbo].[Solution] CHECK CONSTRAINT [FK_Solution_SolutionDetail]
GO

/*-----------------------------------------------------------------------
--
-- AdditionalServiceDetail
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[AdditionalServiceDetail](
	[Id] [varchar](14) NOT NULL,
	[FullDescription] [varchar](3000) NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AdditionalServiceDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AdditionalServiceDetail]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalServiceDetail_Solution_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Solution] ([Id])
GO

ALTER TABLE [dbo].[AdditionalServiceDetail] CHECK CONSTRAINT [FK_AdditionalServiceDetail_Solution_Id]
GO

/*-----------------------------------------------------------------------
--
-- SolutionCapabilityStatus
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionCapabilityStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,
	[Pass] [bit] NOT NULL,
 CONSTRAINT [PK_SolutionCapabilityStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_SolutionCapabilityStatusName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/*-----------------------------------------------------------------------
--
-- SolutionCapability
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionCapability](
	[SolutionId] [varchar](14) NOT NULL,
	[CapabilityId] [uniqueidentifier] NOT NULL,	
	[StatusId] [int] NOT NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SolutionCapability] PRIMARY KEY CLUSTERED 
(
	[SolutionId] ASC,
	[CapabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolutionCapability]  WITH CHECK ADD  CONSTRAINT [FK_SolutionCapability_Capability] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[Capability] ([Id])
GO

ALTER TABLE [dbo].[SolutionCapability] CHECK CONSTRAINT [FK_SolutionCapability_Capability]
GO

ALTER TABLE [dbo].[SolutionCapability]  WITH CHECK ADD  CONSTRAINT [FK_SolutionCapability_Solution] FOREIGN KEY([SolutionId])
REFERENCES [dbo].[Solution] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SolutionCapability] CHECK CONSTRAINT [FK_SolutionCapability_Solution]
GO

ALTER TABLE [dbo].[SolutionCapability]  WITH CHECK ADD  CONSTRAINT [FK_SolutionCapability_SolutionCapabilityStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[SolutionCapabilityStatus] ([Id])
GO

ALTER TABLE [dbo].[SolutionCapability] CHECK CONSTRAINT [FK_SolutionCapability_SolutionCapabilityStatus]
GO


/*-----------------------------------------------------------------------
--
-- SolutionEpicStatus
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionEpicStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,	
 CONSTRAINT [PK_EpicStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_EpicStatusName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/*-----------------------------------------------------------------------
--
-- SolutionEpic
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionEpic](
	[SolutionId] [varchar](14) NOT NULL,
	[CapabilityId] [uniqueidentifier] NOT NULL,
	[EpicId] [uniqueidentifier] NOT NULL,
	[StatusId] [int] NOT NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SolutionEpic] PRIMARY KEY CLUSTERED 
(
	[SolutionId] ASC,
	[CapabilityId],
	[EpicId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolutionEpic]  WITH CHECK ADD  CONSTRAINT [FK_SolutionEpic_Solution] FOREIGN KEY([SolutionId])
REFERENCES [dbo].[Solution] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SolutionEpic] CHECK CONSTRAINT [FK_SolutionEpic_Solution]
GO

ALTER TABLE [dbo].[SolutionEpic]  WITH CHECK ADD  CONSTRAINT [FK_SolutionEpic_SolutionEpicStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[SolutionEpicStatus] ([Id])
GO

ALTER TABLE [dbo].[SolutionEpic] CHECK CONSTRAINT [FK_SolutionEpic_SolutionEpicStatus]
GO

ALTER TABLE [dbo].[SolutionEpic]  WITH CHECK ADD  CONSTRAINT [FK_SolutionEpicStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[SolutionEpicStatus] ([Id])
GO

ALTER TABLE [dbo].[SolutionEpic] CHECK CONSTRAINT [FK_SolutionEpic_SolutionEpicStatus]
GO

ALTER TABLE [dbo].[SolutionEpic]  WITH CHECK ADD  CONSTRAINT [FK_SolutionEpic_Epic] FOREIGN KEY([EpicId])
REFERENCES [dbo].[Epic] ([Id])
GO

ALTER TABLE [dbo].[SolutionEpic] CHECK CONSTRAINT [FK_SolutionEpic_Epic]
GO

ALTER TABLE [dbo].[SolutionEpic]  WITH CHECK ADD  CONSTRAINT [FK_SolutionEpic_Capability] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[Capability] ([Id])
GO

ALTER TABLE [dbo].[SolutionEpic] CHECK CONSTRAINT [FK_SolutionEpic_Capability]
GO

/*-----------------------------------------------------------------------
--
-- SolutionStandardStatus
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionStandardStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,	
 CONSTRAINT [PK_SolutionStandardStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_SolutionStandardStatusName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/*-----------------------------------------------------------------------
--
-- SolutionStandard
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionStandard](
	[SolutionId] [varchar](14) NOT NULL,
	[StandardId] [uniqueidentifier] NOT NULL,		
	[StatusId] [int] NOT NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SolutionStandard_1] PRIMARY KEY CLUSTERED 
(	
	[SolutionId] ASC,
	[StandardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolutionStandard]  WITH CHECK ADD  CONSTRAINT [FK_SolutionStandard_Solution] FOREIGN KEY([SolutionId])
REFERENCES [dbo].[Solution] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SolutionStandard] CHECK CONSTRAINT [FK_SolutionStandard_Solution]
GO

ALTER TABLE [dbo].[SolutionStandard]  WITH CHECK ADD  CONSTRAINT [FK_SolutionStandard_SolutionStandardStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[SolutionStandardStatus] ([Id])
GO

ALTER TABLE [dbo].[SolutionStandard] CHECK CONSTRAINT [FK_SolutionStandard_SolutionStandardStatus]
GO

ALTER TABLE [dbo].[SolutionStandard]  WITH CHECK ADD  CONSTRAINT [FK_SolutionStandard_Standard] FOREIGN KEY([StandardId])
REFERENCES [dbo].[Standard] ([Id])
GO

ALTER TABLE [dbo].[SolutionStandard] CHECK CONSTRAINT [FK_SolutionStandard_Standard]
GO


/*-----------------------------------------------------------------------
--
-- SolutionDefinedCapability
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionDefinedCapability](	
	[Id] [varchar](18) NOT NULL,	
	[SolutionId] [varchar](14) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[StatusId] [int] NOT NULL,
	[Description] [varchar](1000) NOT NULL,
	[Tag] [varchar](100) NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SolutionDefinedCapability] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolutionDefinedCapability]  WITH CHECK ADD  CONSTRAINT [FK_SolutionDefinedCapability_CapabilityStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[SolutionCapabilityStatus] ([Id])
GO

ALTER TABLE [dbo].[SolutionDefinedCapability] CHECK CONSTRAINT [FK_SolutionDefinedCapability_CapabilityStatus]
GO

ALTER TABLE [dbo].[SolutionDefinedCapability]  WITH CHECK ADD  CONSTRAINT [FK_SolutionDefinedCapability_Solution] FOREIGN KEY([SolutionId])
REFERENCES [dbo].[Solution] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SolutionDefinedCapability] CHECK CONSTRAINT [FK_SolutionDefinedCapability_Solution]
GO


/*-----------------------------------------------------------------------
--
-- SolutionDefinedEpic
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionDefinedEpic](
	[Id] [uniqueidentifier] NOT NULL,
	[SolutionDefinedCapabilityId] [varchar](18) NOT NULL,	
	[EpicRef] [varchar](16) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](3000) NOT NULL,
	[StatusId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SolutionDefinedEpic] PRIMARY KEY NONCLUSTERED 
(		
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_SolutionDefinedEpicEpicRef] ON [dbo].[SolutionDefinedEpic]
(
	[EpicRef] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolutionDefinedEpic]  WITH CHECK ADD  CONSTRAINT [FK_SolutionDefinedEpic_SolutionEpicStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[SolutionEpicStatus] ([Id])
GO

ALTER TABLE [dbo].[SolutionDefinedEpic] CHECK CONSTRAINT [FK_SolutionDefinedEpic_SolutionEpicStatus]
GO

ALTER TABLE [dbo].[SolutionDefinedEpic]  WITH CHECK ADD  CONSTRAINT [FK_SolutionDefinedEpic_EpicCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[EpicCategory] ([Id])
GO

ALTER TABLE [dbo].[SolutionDefinedEpic] CHECK CONSTRAINT [FK_SolutionDefinedEpic_EpicCategory]
GO

ALTER TABLE [dbo].[SolutionDefinedEpic]  WITH CHECK ADD  CONSTRAINT [FK_SolutionDefinedEpic_SolutionDefinedCapability] FOREIGN KEY([SolutionDefinedCapabilityId])
REFERENCES [dbo].[SolutionDefinedCapability] ([Id])
GO

ALTER TABLE [dbo].[SolutionDefinedEpic] CHECK CONSTRAINT [FK_SolutionDefinedEpic_SolutionDefinedCapability]
GO

/*-----------------------------------------------------------------------
--
-- SolutionDefinedEpicAcceptanceCriteria
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionDefinedEpicAcceptanceCriteria](
	[Id] [uniqueidentifier] NOT NULL,
	[EpicId] [uniqueidentifier] NOT NULL,
	[EpicRef] [varchar](16) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SolutionDefinedEpicAcceptanceCriteria] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_SolutionDefinedEpicAcceptanceCriteriaEpicRef] ON [dbo].[SolutionDefinedEpicAcceptanceCriteria]
(
	[EpicRef] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolutionDefinedEpicAcceptanceCriteria]  WITH CHECK ADD  CONSTRAINT [FK_SolutionDefinedEpicAcceptanceCriteria_SolutionDefinedEpic] FOREIGN KEY([EpicId])
REFERENCES [dbo].[SolutionDefinedEpic] ([Id])
GO

ALTER TABLE [dbo].[SolutionDefinedEpicAcceptanceCriteria] CHECK CONSTRAINT [FK_SolutionDefinedEpicAcceptanceCriteria_SolutionDefinedEpic]
GO


/*-----------------------------------------------------------------------
--
-- PricingUnit
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[PricingUnit](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](500) NOT NULL,	
 CONSTRAINT [PK_PricingUnit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/*-----------------------------------------------------------------------
--
-- PriceType
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[PriceType](
	[Id] [int] NOT NULL,
	[Name] [varchar](35) NOT NULL,	
 CONSTRAINT [PK_PricingType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/*-----------------------------------------------------------------------
--
-- PurchasingModel
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[PurchasingModel](
	[Id] [uniqueidentifier] NOT NULL,
	[FrameworkId] [varchar](10) NOT NULL,
	[SolutionId] [varchar](14) NOT NULL,
	[StatusId] [int] NOT NULL CONSTRAINT [DF_PurchasingModel_Status] DEFAULT 1,
	[AuthorityStatusId] [int] NOT NULL CONSTRAINT [DF_PurchasingModel_AuthorityStatus] DEFAULT 1,
	[Deleted] [bit] NOT NULL CONSTRAINT [DF_PurchaseModel_Deleted] DEFAULT 0,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PurchasingModel] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_PurchasingModelFrameworkId] ON [dbo].[PurchasingModel]
(
	[FrameworkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PurchasingModel]  WITH CHECK ADD  CONSTRAINT [FK_PurchasingModel_PublicationStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[PublicationStatus] ([Id])
GO

ALTER TABLE [dbo].[PurchasingModel] CHECK CONSTRAINT [FK_PurchasingModel_PublicationStatus]
GO

ALTER TABLE [dbo].[PurchasingModel]  WITH CHECK ADD  CONSTRAINT [FK_PurchasingModel_AuthorityStatus] FOREIGN KEY([AuthorityStatusId])
REFERENCES [dbo].[SolutionAuthorityStatus] ([Id])
GO

ALTER TABLE [dbo].[PurchasingModel] CHECK CONSTRAINT [FK_PurchasingModel_AuthorityStatus]
GO

ALTER TABLE [dbo].[PurchasingModel]  WITH CHECK ADD  CONSTRAINT [FK_PurchasingModel_SolutionId] FOREIGN KEY([SolutionId])
REFERENCES [dbo].[Solution] ([Id])
GO

ALTER TABLE [dbo].[PurchasingModel] CHECK CONSTRAINT [FK_PurchasingModel_SolutionId]
GO

ALTER TABLE [dbo].[PurchasingModel]  WITH CHECK ADD  CONSTRAINT [FK_PurchasingModel_FrameworkId] FOREIGN KEY([FrameworkId])
REFERENCES [dbo].[Framework] ([Id])
GO

ALTER TABLE [dbo].[PurchasingModel] CHECK CONSTRAINT [FK_PurchasingModel_FrameworkId]
GO

/*-----------------------------------------------------------------------
--
-- SolutionPrice
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[SolutionPrice](
	[Id] [uniqueidentifier] NOT NULL,
	[PurchasingModelId] [uniqueidentifier] NOT NULL,
	[UnitId] [int] NOT NULL,
	[PriceTypeId] [int] NOT NULL,
	[ConsumptionPrice] [bit] NOT NULL DEFAULT (0),
	[Description] [varchar](30) NULL,
	[Price] [decimal](18, 4) NOT NULL,
	[BandStart] [int] NOT NULL,
	[BandEnd] [int] NULL,	
	[Created] [datetime2](7) NOT NULL CONSTRAINT [DF_SolutionPrice_Created] DEFAULT GetUtcDate(),
 CONSTRAINT [PK_SolutionPrice] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_SolutionPriceLastUpdated] ON [dbo].[SolutionPrice]
(
	[Created] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SolutionPrice]  WITH CHECK ADD  CONSTRAINT [FK_SolutionPrice_PurchasingModelId] FOREIGN KEY([PurchasingModelId])
REFERENCES [dbo].[PurchasingModel] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SolutionPrice] CHECK CONSTRAINT [FK_SolutionPrice_PurchasingModelId]
GO

ALTER TABLE [dbo].[SolutionPrice]  WITH CHECK ADD  CONSTRAINT [FK_SolutionPrice_PricingUnit] FOREIGN KEY([UnitId])
REFERENCES [dbo].[PricingUnit] ([Id])
GO

ALTER TABLE [dbo].[SolutionPrice] CHECK CONSTRAINT [FK_SolutionPrice_PricingUnit]
GO

ALTER TABLE [dbo].[SolutionPrice]  WITH CHECK ADD  CONSTRAINT [FK_SolutionPrice_PriceType] FOREIGN KEY([PriceTypeId])
REFERENCES [dbo].[PricingUnit] ([Id])
GO

ALTER TABLE [dbo].[SolutionPrice] CHECK CONSTRAINT [FK_SolutionPrice_PriceType]
GO

/*-----------------------------------------------------------------------
--
-- AdditionalServicePrice
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[AdditionalServicePrice](
	[Id] [uniqueidentifier] NOT NULL,
	[PurchasingModelId] [uniqueidentifier] NOT NULL,
	[AdditionalServiceId] [varchar](14) NOT NULL,
	[UnitId] [int] NOT NULL,
	[PriceTypeId] [int] NOT NULL,
	[ConsumptionPrice] [bit] NOT NULL DEFAULT (0),
	[Description] [varchar](30) NULL,
	[Price] [decimal](18, 4) NOT NULL,
	[BandStart] [int] NOT NULL,
	[BandEnd] [int] NULL,	
 CONSTRAINT [PK_AdditionalServicePrice] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_AdditionalServicePriceAdditionalServiceId] ON [dbo].[AdditionalServicePrice]
(
	[AdditionalServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AdditionalServicePrice]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalServicePrice_PurchasingModelId] FOREIGN KEY([PurchasingModelId])
REFERENCES [dbo].[PurchasingModel] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AdditionalServicePrice] CHECK CONSTRAINT [FK_AdditionalServicePrice_PurchasingModelId]
GO

ALTER TABLE [dbo].[AdditionalServicePrice]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalServicePrice_PricingUnit] FOREIGN KEY([UnitId])
REFERENCES [dbo].[PricingUnit] ([Id])
GO

ALTER TABLE [dbo].[AdditionalServicePrice] CHECK CONSTRAINT [FK_AdditionalServicePrice_PricingUnit]
GO

ALTER TABLE [dbo].[AdditionalServicePrice]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalServicePrice_PriceType] FOREIGN KEY([PriceTypeId])
REFERENCES [dbo].[PriceType] ([Id])
GO

ALTER TABLE [dbo].[AdditionalServicePrice] CHECK CONSTRAINT [FK_AdditionalServicePrice_PriceType]
GO

ALTER TABLE [dbo].[AdditionalServicePrice]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalServicePrice_Soution] FOREIGN KEY([AdditionalServiceId])
REFERENCES [dbo].[Solution] ([Id])
GO

ALTER TABLE [dbo].[AdditionalServicePrice] CHECK CONSTRAINT [FK_AdditionalServicePrice_Soution]
GO


/*-----------------------------------------------------------------------
--
-- AssociatedService
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[AssociatedService](
	[Id] [varchar](18) NOT NULL,
	[SolutionId] [varchar](14) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
	[OrderGuidance] [varchar](1000) NULL,	
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AssociatedService] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AssociatedService]  WITH CHECK ADD  CONSTRAINT [FK_AssociatedService_Solution] FOREIGN KEY([SolutionId])
REFERENCES [dbo].[Solution] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AssociatedService] CHECK CONSTRAINT [FK_AssociatedService_Solution]
GO

/*-----------------------------------------------------------------------
--
-- AssociatedServicePrice
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[AssociatedServicePrice](
	[Id] [uniqueidentifier] NOT NULL,
	[PurchasingModelId] [uniqueidentifier] NOT NULL,
	[AssociatedServiceId] [varchar](18) NOT NULL,
	[UnitId] [int] NOT NULL,
	[PriceTypeId] [int] NOT NULL,
	[ConsumptionPrice] [bit] NOT NULL DEFAULT (0),
	[Description] [varchar](30) NULL,
	[Price] [decimal](18, 4) NOT NULL,
	[BandStart] [int] NOT NULL,
	[BandEnd] [int] NULL,	
	[Created] [datetime2](7) NOT NULL CONSTRAINT [DF_AssociatedServicePrice_Created] DEFAULT GetUtcDate(),
CONSTRAINT [PK_AssociatedServicePrice] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_AssociatedServicePriceLastUpdated] ON [dbo].[AssociatedServicePrice]
(
	[Created] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AssociatedServicePrice]  WITH CHECK ADD  CONSTRAINT [FK_AssociatedServicePrice_PurchasingModelId] FOREIGN KEY([PurchasingModelId])
REFERENCES [dbo].[PurchasingModel] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AssociatedServicePrice] CHECK CONSTRAINT [FK_AssociatedServicePrice_PurchasingModelId]
GO

ALTER TABLE [dbo].[AssociatedServicePrice]  WITH CHECK ADD  CONSTRAINT [FK_AssociateServicePrice_PricingUnit] FOREIGN KEY([UnitId])
REFERENCES [dbo].[PricingUnit] ([Id])
GO

ALTER TABLE [dbo].[AssociatedServicePrice] CHECK CONSTRAINT [FK_AssociateServicePrice_PricingUnit]
GO

ALTER TABLE [dbo].[AssociatedServicePrice]  WITH CHECK ADD  CONSTRAINT [FK_AssociatedServicePrice_PriceType] FOREIGN KEY([PriceTypeId])
REFERENCES [dbo].[PriceType] ([Id])
GO

ALTER TABLE [dbo].[AssociatedServicePrice] CHECK CONSTRAINT [FK_AssociatedServicePrice_PriceType]
GO

/*-----------------------------------------------------------------------
--
-- MarketingContact
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[MarketingContact](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SolutionId] [varchar](14) NOT NULL,
	[FirstName] [varchar](35) NULL,
	[LastName] [varchar](35) NULL,
	[Email] [varchar](255) NULL,
	[PhoneNumber] [varchar](35) NULL,
	[Department] [varchar](50) NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_MarketingContact] PRIMARY KEY CLUSTERED 
(
	[SolutionId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MarketingContact]  WITH CHECK ADD  CONSTRAINT [FK_MarketingContact_Solution] FOREIGN KEY([SolutionId])
REFERENCES [dbo].[Solution] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[MarketingContact] CHECK CONSTRAINT [FK_MarketingContact_Solution]
GO

/*-----------------------------------------------------------------------
--
-- FrameworkSolutions
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[FrameworkSolutions](
	[FrameworkId] [varchar](10) NOT NULL,
	[SolutionId] [varchar](14) NOT NULL,
	[IsFoundation] [bit] NOT NULL CONSTRAINT [DF_FrameworkSolutions_IsFoundation] DEFAULT 0,
	[LastUpdated] [datetime2](7) NOT NULL,
	[LastUpdatedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_FrameworkSolutions] PRIMARY KEY CLUSTERED
( 
	[FrameworkId] ASC,
	[SolutionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FrameworkSolutions]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkSolutions_Framework] FOREIGN KEY([FrameworkId])
REFERENCES [dbo].[Framework] ([Id])
GO

ALTER TABLE [dbo].[FrameworkSolutions] CHECK CONSTRAINT [FK_FrameworkSolutions_Framework]
GO

ALTER TABLE [dbo].[FrameworkSolutions]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkSolutions_Solution] FOREIGN KEY([SolutionId])
REFERENCES [dbo].[Solution] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[FrameworkSolutions] CHECK CONSTRAINT [FK_FrameworkSolutions_Solution]
GO


/*-----------------------------------------------------------------------
--
-- Audit
--
------------------------------------------------------------------------*/
CREATE TABLE [dbo].[Audit](
	[Id] [uniqueidentifier] NOT NULL,
	[DataType] [varchar](50) NOT NULL,
	[DataItemId] [varchar](50) NOT NULL,
	[AuditType] [varchar](30) NOT NULL,
	[PerformedAt] [datetime2](7) NOT NULL,
	[PerformedBy] [uniqueidentifier] NOT NULL,
CONSTRAINT [PK_Audit] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE CLUSTERED INDEX [IX_AuditPerformedAt] ON [dbo].[Audit]
(
	[PerformedAt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

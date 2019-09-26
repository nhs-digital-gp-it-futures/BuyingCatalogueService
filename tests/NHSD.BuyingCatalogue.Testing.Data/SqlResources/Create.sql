
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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]



CREATE TABLE [dbo].[CapabilityCategory](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,
 CONSTRAINT [PK_CapabilityCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_CapabilityCategoryName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]



CREATE TABLE [dbo].[CapabilityStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,
 CONSTRAINT [PK_CapabilityStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_CapabilityStatusName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


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
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[Capability]  WITH CHECK ADD  CONSTRAINT [FK_Capability_CapabilityCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[CapabilityCategory] ([Id])


ALTER TABLE [dbo].[Capability] CHECK CONSTRAINT [FK_Capability_CapabilityCategory]


ALTER TABLE [dbo].[Capability]  WITH CHECK ADD  CONSTRAINT [FK_Capability_CapabilityStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[CapabilityStatus] ([Id])


ALTER TABLE [dbo].[Capability] CHECK CONSTRAINT [FK_Capability_CapabilityStatus]


CREATE TABLE [dbo].[FrameworkCapabilities](
	[FrameworkId] [varchar](10) NOT NULL,
	[CapabilityId] [uniqueidentifier] NOT NULL,
	[IsFoundation] [bit] NOT NULL,
 CONSTRAINT [PK_FrameworkCapabilities] PRIMARY KEY CLUSTERED 
(
	[FrameworkId] ASC,
	[CapabilityId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[FrameworkCapabilities] ADD  CONSTRAINT [DF_FrameworkCapabilities_IsFoundation]  DEFAULT ((0)) FOR [IsFoundation]


ALTER TABLE [dbo].[FrameworkCapabilities]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkCapabilities_Capability] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[Capability] ([Id])


ALTER TABLE [dbo].[FrameworkCapabilities] CHECK CONSTRAINT [FK_FrameworkCapabilities_Capability]


ALTER TABLE [dbo].[FrameworkCapabilities]  WITH CHECK ADD  CONSTRAINT [FK_FrameworkCapabilities_Framework] FOREIGN KEY([FrameworkId])
REFERENCES [dbo].[Framework] ([Id])


ALTER TABLE [dbo].[FrameworkCapabilities] CHECK CONSTRAINT [FK_FrameworkCapabilities_Framework]


CREATE TABLE [dbo].[SolutionCapabilityStatus](
	[Id] [int] NOT NULL,
	[Name] [varchar](16) NOT NULL,
 CONSTRAINT [PK_SolutionCapabilityStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_SolutionCapabilityStatusName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[Solution](
	[Id] [varchar](14) NOT NULL,
	[OrganisationId] [varchar](8) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Version] [varchar](10) NOT NULL,
	[PublishedStatusId] [int] NOT NULL CONSTRAINT [DF_Solution_PublishedStatus] DEFAULT 1,
	[AuthorityStatusId] [int] NOT NULL CONSTRAINT [DF_Solution_AuthorityStatus] DEFAULT 1,
	[SupplierStatusId] [int] NOT NULL CONSTRAINT [DF_Solution_SupplierStatus] DEFAULT 1,
	[ParentId] [varchar](14) NULL,	
	[OnCatalogueVersion] [int] NOT NULL CONSTRAINT [DF_Solution_OnCatalogueVersion] DEFAULT 0,
	[Summary] [varchar](300) NULL,
	[FullDescription] [varchar](3000) NULL,
 CONSTRAINT [PK_Solution] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[SolutionCapability](
	[SolutionId] [varchar](14) NOT NULL,
	[CapabilityId] [uniqueidentifier] NOT NULL,	
	[StatusId] [int] NOT NULL,
 CONSTRAINT [PK_SolutionCapability] PRIMARY KEY CLUSTERED 
(
	[SolutionId] ASC,
	[CapabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[SolutionCapability]  WITH CHECK ADD  CONSTRAINT [FK_SolutionCapability_Capability] FOREIGN KEY([CapabilityId])
REFERENCES [dbo].[Capability] ([Id])

ALTER TABLE [dbo].[SolutionCapability] CHECK CONSTRAINT [FK_SolutionCapability_Capability]

ALTER TABLE [dbo].[SolutionCapability]  WITH CHECK ADD  CONSTRAINT [FK_SolutionCapability_Solution] FOREIGN KEY([SolutionId])
REFERENCES [dbo].[Solution] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[SolutionCapability] CHECK CONSTRAINT [FK_SolutionCapability_Solution]

ALTER TABLE [dbo].[SolutionCapability]  WITH CHECK ADD  CONSTRAINT [FK_SolutionCapability_SolutionCapabilityStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[SolutionCapabilityStatus] ([Id])

ALTER TABLE [dbo].[SolutionCapability] CHECK CONSTRAINT [FK_SolutionCapability_SolutionCapabilityStatus]

CREATE TABLE [dbo].[Organisation](
	[Id] [varchar](8) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Summary] [varchar](1000) NULL,
	[OrganisationUrl] [varchar](1000) NULL,
	[OdsCode] [varchar](8) NULL,
	[PrimaryRoleId] [varchar](8) NULL,
	[CrmRef] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Organisation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


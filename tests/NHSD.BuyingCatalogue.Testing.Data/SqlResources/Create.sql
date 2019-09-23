
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


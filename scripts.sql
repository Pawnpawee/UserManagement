USE [UserManagementDb]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 2/6/2568 23:26:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[PermissionId] [uniqueidentifier] NOT NULL,
	[PermissionName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 2/6/2568 23:26:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](max) NOT NULL,
	[PermissionId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Roles]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Permissions_PermissionId] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permissions] ([PermissionId])
GO
ALTER TABLE [dbo].[Roles] CHECK CONSTRAINT [FK_Roles_Permissions_PermissionId]
GO

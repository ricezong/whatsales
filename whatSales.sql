USE [whatSales]
GO
/****** Object:  Table [dbo].[ws_category]    Script Date: 2021/8/26 9:17:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ws_category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ws_order]    Script Date: 2021/8/26 9:17:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ws_order](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Price] [float] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[User_id] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ws_order_product]    Script Date: 2021/8/26 9:17:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ws_order_product](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Order_id] [int] NULL,
	[User_id] [int] NOT NULL,
	[Product_id] [int] NOT NULL,
	[Count] [int] NULL,
	[Price] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ws_product]    Script Date: 2021/8/26 9:17:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ws_product](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](200) NOT NULL,
	[Price] [float] NOT NULL,
	[Property] [varchar](max) NULL,
	[CategoryId] [int] NULL,
	[Active] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ws_user]    Script Date: 2021/8/26 9:17:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ws_user](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[PassWord] [varchar](50) NOT NULL,
	[Active] [int] NOT NULL,
	[Salt] [varchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NULL,
	[DeleteTime] [datetime] NULL,
	[Role] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ws_userDetails]    Script Date: 2021/8/26 9:17:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ws_userDetails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[NickName] [varchar](50) NOT NULL,
	[Sex] [varchar](4) NULL,
	[Phone] [varbinary](50) NULL,
	[Email] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ws_userInfo]    Script Date: 2021/8/26 9:17:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ws_userInfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[LoginTime] [datetime] NOT NULL,
	[Ip] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ws_order] ADD  CONSTRAINT [DF_ws_order_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[ws_user] ADD  CONSTRAINT [DF_user_disable]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[ws_user] ADD  CONSTRAINT [DF_user_createtime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[ws_userInfo] ADD  CONSTRAINT [DF_user_message_logintime]  DEFAULT (getdate()) FOR [LoginTime]
GO

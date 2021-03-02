

/****** Object:  Table [dbo].[Tb_User]    Script Date: 2021/3/2 14:31:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tb_User]') AND type in (N'U'))
DROP TABLE [dbo].[Tb_User]
GO

/****** Object:  Table [dbo].[Tb_User]    Script Date: 2021/3/2 14:31:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tb_User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Account] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[RoleCode] [nvarchar](10) NOT NULL,
	[Age] [int] NULL,
	[Sex] [int] NOT NULL,
	[Email] [nvarchar](100) NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [nchar](10) NULL,
	[CreateUser] [int] NOT NULL,
 CONSTRAINT [PK_Tb_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



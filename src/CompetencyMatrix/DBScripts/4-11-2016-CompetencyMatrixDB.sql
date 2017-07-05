USE [master]
GO
/****** Object:  Database [CompetencyMatrix]    Script Date: 11/6/2016 11:56:12 PM ******/
CREATE DATABASE [CompetencyMatrix]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CompetencyMatrix', FILENAME = N'D:\SQLDB\CompetencyMatrix.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CompetencyMatrix_log', FILENAME = N'D:\SQLDB\CompetencyMatrix_log.ldf' , SIZE = 1536KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [CompetencyMatrix] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CompetencyMatrix].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CompetencyMatrix] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET ARITHABORT OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [CompetencyMatrix] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CompetencyMatrix] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CompetencyMatrix] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CompetencyMatrix] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CompetencyMatrix] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CompetencyMatrix] SET  MULTI_USER 
GO
ALTER DATABASE [CompetencyMatrix] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CompetencyMatrix] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CompetencyMatrix] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CompetencyMatrix] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'CompetencyMatrix', N'ON'
GO
USE [CompetencyMatrix]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[EmployeeId] [int] NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ChangeLog]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChangeLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MatrixId] [int] NOT NULL,
	[SkillId] [int] NOT NULL,
	[SkillLevelId] [int] NOT NULL,
	[Action] [int] NOT NULL,
	[OldSkillLevelId] [int] NULL,
	[When] [datetime] NOT NULL,
	[ByWhom] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_ChangeLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Employee]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Employee](
	[Id] [int] NOT NULL,
	[MatrixId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Photo] [image] NULL,
	[ProfileStatus] [nvarchar](max) NOT NULL,
	[EMail] [varchar](320) NULL,
	[Manager] [int] NULL,
	[Skype] [nvarchar](64) NULL,
	[Cell] [varchar](15) NULL,
	[Office] [nvarchar](50) NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmployeeMatrix]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeMatrix](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [ntext] NULL,
 CONSTRAINT [PK_EmployeeMatrix] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmployeeMatrixApproval]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeMatrixApproval](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ByWhom] [nvarchar](450) NOT NULL,
	[When] [datetime] NOT NULL,
	[MatrixId] [int] NOT NULL,
 CONSTRAINT [PK_EmployeeMatrixApproval] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmployeeMatrixSkill]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeMatrixSkill](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MatrixId] [int] NOT NULL,
	[SkillId] [int] NOT NULL,
	[SkillLevelId] [int] NOT NULL,
 CONSTRAINT [PK_EmployeeMatrixSkills] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmployeePastProject]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeePastProject](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Company] [nvarchar](50) NOT NULL,
	[WorkPeriodStart] [datetime] NOT NULL,
	[WorkPeriodEnd] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Role] [nvarchar](50) NOT NULL,
	[Team] [nvarchar](50) NULL,
	[Project] [nvarchar](50) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[Tool] [nvarchar](max) NULL,
	[Technology] [nvarchar](max) NULL,
 CONSTRAINT [PK_EmployeePastProject] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Permission]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Controller] [nvarchar](50) NULL,
	[Action] [nvarchar](50) NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PermissionOnRole]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionOnRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[PermissionId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Controller] [nvarchar](32) NULL,
	[Action] [nvarchar](32) NULL,
 CONSTRAINT [PK_PermissionOnRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PositionMatrix]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PositionMatrix](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_PositionMatrix] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PositionMatrixInheritance]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PositionMatrixInheritance](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MatrixId] [int] NOT NULL,
	[ParentMatrixId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PositionMatrixSkill]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PositionMatrixSkill](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MatrixId] [int] NOT NULL,
	[SkillId] [int] NOT NULL,
	[SkillLevelId] [int] NOT NULL,
	[SkillGroupId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PositionMatrixSkillGroup]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PositionMatrixSkillGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[GroupTypeId] [int] NOT NULL,
 CONSTRAINT [PK_PositionMatrixSkillGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RoleInProject]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleInProject](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_RoleInProject] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Skill]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Skill](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CategoryId] [int] NOT NULL,
	[TrainingMaterials] [nvarchar](max) NULL,
	[Questionarie] [nvarchar](max) NULL,
	[EvaluationModelId] [int] NULL,
 CONSTRAINT [PK_Skill] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SkillCategory]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkillCategory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [ntext] NULL,
	[ParentId] [int] NULL,
 CONSTRAINT [PK_SkillCategory] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SkillCriteria]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkillCriteria](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SkillId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [ntext] NULL,
 CONSTRAINT [PK_SkillCriteria] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SkillEvaluationModel]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkillEvaluationModel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_SkillEvaluationModel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SkillEvaluationModelLevel]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkillEvaluationModelLevel](
	[_id] [int] IDENTITY(1,1) NOT NULL,
	[SkillEvaluationModelId] [int] NOT NULL,
	[SkillLevelModelId] [int] NOT NULL,
 CONSTRAINT [PK_SkillEvaluationModelLevel] PRIMARY KEY CLUSTERED 
(
	[_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SkillGroupType]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkillGroupType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SkillGroupTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SkillLevelCriteria]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkillLevelCriteria](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[SkillLevelModelId] [int] NOT NULL,
	[SkillCriteriaId] [int] NOT NULL,
 CONSTRAINT [PK__SkillLev__3213E83F23105D50] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SkillLevelModel]    Script Date: 11/6/2016 11:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkillLevelModel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [ntext] NULL,
	[Quality] [int] NOT NULL,
 CONSTRAINT [PK_SkillLevelModel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 11/6/2016 11:56:12 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [RoleNameIndex]    Script Date: 11/6/2016 11:56:12 PM ******/
CREATE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 11/6/2016 11:56:12 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 11/6/2016 11:56:12 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 11/6/2016 11:56:12 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetUserRoles_UserId]    Script Date: 11/6/2016 11:56:12 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_UserId] ON [dbo].[AspNetUserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmailIndex]    Script Date: 11/6/2016 11:56:12 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UserNameIndex]    Script Date: 11/6/2016 11:56:12 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PermissionOnRole] ADD  CONSTRAINT [DF_PermissionOnRole_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[ChangeLog]  WITH CHECK ADD  CONSTRAINT [FK_ChangeLog_AspNetUsers] FOREIGN KEY([ByWhom])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[ChangeLog] CHECK CONSTRAINT [FK_ChangeLog_AspNetUsers]
GO
ALTER TABLE [dbo].[ChangeLog]  WITH CHECK ADD  CONSTRAINT [FK_ChangeLog_EmployeeMatrix] FOREIGN KEY([MatrixId])
REFERENCES [dbo].[EmployeeMatrix] ([Id])
GO
ALTER TABLE [dbo].[ChangeLog] CHECK CONSTRAINT [FK_ChangeLog_EmployeeMatrix]
GO
ALTER TABLE [dbo].[ChangeLog]  WITH CHECK ADD  CONSTRAINT [FK_ChangeLog_Skill] FOREIGN KEY([SkillId])
REFERENCES [dbo].[Skill] ([id])
GO
ALTER TABLE [dbo].[ChangeLog] CHECK CONSTRAINT [FK_ChangeLog_Skill]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Employee] FOREIGN KEY([Manager])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Employee]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_EmployeeMatrix] FOREIGN KEY([MatrixId])
REFERENCES [dbo].[EmployeeMatrix] ([Id])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_EmployeeMatrix]
GO
ALTER TABLE [dbo].[EmployeeMatrixApproval]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeMatrixApproval_AspNetUsers] FOREIGN KEY([ByWhom])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[EmployeeMatrixApproval] CHECK CONSTRAINT [FK_EmployeeMatrixApproval_AspNetUsers]
GO
ALTER TABLE [dbo].[EmployeeMatrixSkill]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeMatrixSkills_AvailableSkillLevels] FOREIGN KEY([SkillLevelId])
REFERENCES [dbo].[SkillLevelModel] ([Id])
GO
ALTER TABLE [dbo].[EmployeeMatrixSkill] CHECK CONSTRAINT [FK_EmployeeMatrixSkills_AvailableSkillLevels]
GO
ALTER TABLE [dbo].[EmployeeMatrixSkill]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeMatrixSkills_EmployeeMatrix] FOREIGN KEY([MatrixId])
REFERENCES [dbo].[EmployeeMatrix] ([Id])
GO
ALTER TABLE [dbo].[EmployeeMatrixSkill] CHECK CONSTRAINT [FK_EmployeeMatrixSkills_EmployeeMatrix]
GO
ALTER TABLE [dbo].[EmployeeMatrixSkill]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeMatrixSkills_Skills] FOREIGN KEY([SkillId])
REFERENCES [dbo].[Skill] ([id])
GO
ALTER TABLE [dbo].[EmployeeMatrixSkill] CHECK CONSTRAINT [FK_EmployeeMatrixSkills_Skills]
GO
ALTER TABLE [dbo].[EmployeePastProject]  WITH CHECK ADD  CONSTRAINT [FK_EmployeePastProject_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[EmployeePastProject] CHECK CONSTRAINT [FK_EmployeePastProject_Employee]
GO
ALTER TABLE [dbo].[PermissionOnRole]  WITH CHECK ADD  CONSTRAINT [FK_PermissionOnRole_AspNetRoles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[PermissionOnRole] CHECK CONSTRAINT [FK_PermissionOnRole_AspNetRoles]
GO
ALTER TABLE [dbo].[PermissionOnRole]  WITH CHECK ADD  CONSTRAINT [FK_PermissionOnRole_Permission] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permission] ([Id])
GO
ALTER TABLE [dbo].[PermissionOnRole] CHECK CONSTRAINT [FK_PermissionOnRole_Permission]
GO
ALTER TABLE [dbo].[PositionMatrixInheritance]  WITH CHECK ADD  CONSTRAINT [FK_PositionMatrixInheritance_PositionMatrix] FOREIGN KEY([MatrixId])
REFERENCES [dbo].[PositionMatrix] ([Id])
GO
ALTER TABLE [dbo].[PositionMatrixInheritance] CHECK CONSTRAINT [FK_PositionMatrixInheritance_PositionMatrix]
GO
ALTER TABLE [dbo].[PositionMatrixInheritance]  WITH CHECK ADD  CONSTRAINT [FK_PositionMatrixInheritance_PositionMatrix1] FOREIGN KEY([ParentMatrixId])
REFERENCES [dbo].[PositionMatrix] ([Id])
GO
ALTER TABLE [dbo].[PositionMatrixInheritance] CHECK CONSTRAINT [FK_PositionMatrixInheritance_PositionMatrix1]
GO
ALTER TABLE [dbo].[PositionMatrixSkill]  WITH CHECK ADD  CONSTRAINT [FK_PositionMatrixSkills_AvailableSkillLevels] FOREIGN KEY([SkillLevelId])
REFERENCES [dbo].[SkillLevelModel] ([Id])
GO
ALTER TABLE [dbo].[PositionMatrixSkill] CHECK CONSTRAINT [FK_PositionMatrixSkills_AvailableSkillLevels]
GO
ALTER TABLE [dbo].[PositionMatrixSkill]  WITH CHECK ADD  CONSTRAINT [FK_PositionMatrixSkills_PositionMatrix] FOREIGN KEY([MatrixId])
REFERENCES [dbo].[PositionMatrix] ([Id])
GO
ALTER TABLE [dbo].[PositionMatrixSkill] CHECK CONSTRAINT [FK_PositionMatrixSkills_PositionMatrix]
GO
ALTER TABLE [dbo].[PositionMatrixSkill]  WITH CHECK ADD  CONSTRAINT [FK_PositionMatrixSkills_PositionMatrixSkillGroups] FOREIGN KEY([SkillGroupId])
REFERENCES [dbo].[PositionMatrixSkillGroup] ([Id])
GO
ALTER TABLE [dbo].[PositionMatrixSkill] CHECK CONSTRAINT [FK_PositionMatrixSkills_PositionMatrixSkillGroups]
GO
ALTER TABLE [dbo].[PositionMatrixSkill]  WITH CHECK ADD  CONSTRAINT [FK_PositionMatrixSkills_Skills] FOREIGN KEY([SkillId])
REFERENCES [dbo].[Skill] ([id])
GO
ALTER TABLE [dbo].[PositionMatrixSkill] CHECK CONSTRAINT [FK_PositionMatrixSkills_Skills]
GO
ALTER TABLE [dbo].[PositionMatrixSkillGroup]  WITH CHECK ADD  CONSTRAINT [FK_PositionMatrixSkillGroups_SkillGroupTypes] FOREIGN KEY([GroupTypeId])
REFERENCES [dbo].[SkillGroupType] ([Id])
GO
ALTER TABLE [dbo].[PositionMatrixSkillGroup] CHECK CONSTRAINT [FK_PositionMatrixSkillGroups_SkillGroupTypes]
GO
ALTER TABLE [dbo].[Skill]  WITH CHECK ADD  CONSTRAINT [FK_Skill_SkillCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[SkillCategory] ([id])
GO
ALTER TABLE [dbo].[Skill] CHECK CONSTRAINT [FK_Skill_SkillCategory]
GO
ALTER TABLE [dbo].[Skill]  WITH CHECK ADD  CONSTRAINT [FK_Skill_SkillEvaluationModel] FOREIGN KEY([EvaluationModelId])
REFERENCES [dbo].[SkillEvaluationModel] ([Id])
GO
ALTER TABLE [dbo].[Skill] CHECK CONSTRAINT [FK_Skill_SkillEvaluationModel]
GO
ALTER TABLE [dbo].[SkillCategory]  WITH CHECK ADD  CONSTRAINT [FK_SkillCategory_SkillCategory] FOREIGN KEY([ParentId])
REFERENCES [dbo].[SkillCategory] ([id])
GO
ALTER TABLE [dbo].[SkillCategory] CHECK CONSTRAINT [FK_SkillCategory_SkillCategory]
GO
ALTER TABLE [dbo].[SkillCriteria]  WITH CHECK ADD  CONSTRAINT [FK_SkillCriteria_Skills1] FOREIGN KEY([SkillId])
REFERENCES [dbo].[Skill] ([id])
GO
ALTER TABLE [dbo].[SkillCriteria] CHECK CONSTRAINT [FK_SkillCriteria_Skills1]
GO
ALTER TABLE [dbo].[SkillEvaluationModelLevel]  WITH CHECK ADD  CONSTRAINT [FK_SkillEvaluationModelLevel_SkillEvaluationModel] FOREIGN KEY([SkillEvaluationModelId])
REFERENCES [dbo].[SkillEvaluationModel] ([Id])
GO
ALTER TABLE [dbo].[SkillEvaluationModelLevel] CHECK CONSTRAINT [FK_SkillEvaluationModelLevel_SkillEvaluationModel]
GO
ALTER TABLE [dbo].[SkillEvaluationModelLevel]  WITH CHECK ADD  CONSTRAINT [FK_SkillEvaluationModelLevel_SkillLevelModel] FOREIGN KEY([SkillLevelModelId])
REFERENCES [dbo].[SkillLevelModel] ([Id])
GO
ALTER TABLE [dbo].[SkillEvaluationModelLevel] CHECK CONSTRAINT [FK_SkillEvaluationModelLevel_SkillLevelModel]
GO
ALTER TABLE [dbo].[SkillLevelCriteria]  WITH CHECK ADD  CONSTRAINT [FK_SkillLevelCriteria_AvailableSkillLevels] FOREIGN KEY([SkillLevelModelId])
REFERENCES [dbo].[SkillLevelModel] ([Id])
GO
ALTER TABLE [dbo].[SkillLevelCriteria] CHECK CONSTRAINT [FK_SkillLevelCriteria_AvailableSkillLevels]
GO
ALTER TABLE [dbo].[SkillLevelCriteria]  WITH CHECK ADD  CONSTRAINT [FK_SkillLevelCriteria_SkillCriteria] FOREIGN KEY([SkillCriteriaId])
REFERENCES [dbo].[SkillCriteria] ([Id])
GO
ALTER TABLE [dbo].[SkillLevelCriteria] CHECK CONSTRAINT [FK_SkillLevelCriteria_SkillCriteria]
GO
USE [master]
GO
ALTER DATABASE [CompetencyMatrix] SET  READ_WRITE 
GO

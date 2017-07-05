USE [CompetencyMatrix]
GO
INSERT [dbo].[AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName]) VALUES (N'80f004dd-34da-484a-91ac-3ea8cb659979', N'25801739-ef8d-45f5-a8b1-008c1d18c81b', N'admin', N'ADMIN')
GO
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName], [EmployeeId]) VALUES (N'1c5ed7d9-4448-42d9-a708-19050a8d366d', 0, N'c2aedf7a-a8b0-42e0-9486-93957b453f77', N'bodomusa@gmail.com', 0, 1, NULL, N'BODOMUSA@GMAIL.COM', N'BODOMUSA@GMAIL.COM', N'AQAAAAEAACcQAAAAEGSDPQl+CLmGNWdhKDv7qNx7yO2v+GqXsUbmMrf5YPCA/CJcqoTdOBwJJoueLHAqCw==', NULL, 0, N'b8a137bb-60bd-4ccc-8057-003590a3836b', 0, N'bodomusa@gmail.com', 11)
GO
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName], [EmployeeId]) VALUES (N'3d72ae60-87b6-45e6-8159-18d3e2c6031f', 0, N'e5c0e166-248d-44ce-aa5d-f2d567af9d2e', N'bodomus@gmail.com', 0, 1, NULL, N'ADMIN@WORK.COM', N'ADMIN', N'AQAAAAEAACcQAAAAEKpmCvjK3XcIBtLnxq/MSUygrh1sysWHzppPpwm5WoKziRzbkS8Rj9iZt4jymhyYKA==', NULL, 0, N'e6312ca3-21ca-4d44-b4d0-f56480e4d114', 0, N'bodomus@gmail.com', NULL)
GO
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName], [EmployeeId]) VALUES (N'59380a8f-e7c6-4a1c-b4de-fd21879f2467', 0, N'21418012-9d58-47f5-b441-c6d55de0adec', N'bodomus@gmail.com', 0, 1, NULL, N'DEV@DEV.COM', N'DEVUSER', N'AQAAAAEAACcQAAAAEF/rGBCPOL7ZQxAUElSzC4q8w0Rtkzt/nvxT/GDRTeJkS/4dMcibmXbqorNhmTRljA==', NULL, 0, N'dbccd49e-ffae-472c-97a2-d9b7eb665c50', 0, N'bodomus@gmail.com', NULL)
GO
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName], [EmployeeId]) VALUES (N'97aa8e4e-db32-4b8e-8611-a90a69f36fe1', 0, N'32e1341a-609e-48bf-ace5-8871db3669a4', N'bodomus@gmail.com', 0, 1, NULL, N'WORK@WORK.COM', N'WORKUSER', N'AQAAAAEAACcQAAAAEDd+PIpLY98yLgi7eaOexut3vxR95b4+6BzRooRAId+DC5qqa1uFb41RHPpxVgdZ+g==', NULL, 0, N'fbabd1f4-b840-4070-b49c-3183797e8899', 0, N'bodomus@gmail.com', NULL)
GO
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName], [EmployeeId]) VALUES (N'f98a87c0-e9c4-420c-8a1c-1fc667b12392', 0, N'92cae8ed-81ad-4f2e-8c62-fe3ad5ceea91', N'bodomus@gmail.com', 0, 1, NULL, N'bodomus@gmail.com', N'bodomus@gmail.com', N'AQAAAAEAACcQAAAAEBUtRa554H9pIVXYJAFA6WE25AuRDrR1ho3ZT2S0a5h2171jxNQa3X9czNoSdQGwqg==', NULL, 0, N'2e016eed-f05f-4f95-a2d2-e1bf381454d7', 0, N'bodomus@gmail.com', NULL)
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'1c5ed7d9-4448-42d9-a708-19050a8d366d', N'80f004dd-34da-484a-91ac-3ea8cb659979')
GO
SET IDENTITY_INSERT [dbo].[Permission] ON 


SET IDENTITY_INSERT [dbo].[Permission] ON 

GO
INSERT [dbo].[Permission] ([Id], [Name], [Controller], [Action]) VALUES (1, N'Add/edit/remove matrix data', NULL, NULL)
GO
INSERT [dbo].[Permission] ([Id], [Name], [Controller], [Action]) VALUES (2, N'Review profile of subordinates
', NULL, NULL)
GO
INSERT [dbo].[Permission] ([Id], [Name], [Controller], [Action]) VALUES (3, N'Review all', NULL, NULL)
GO
INSERT [dbo].[Permission] ([Id], [Name], [Controller], [Action]) VALUES (4, N'Review matrix', NULL, NULL)
GO
INSERT [dbo].[Permission] ([Id], [Name], [Controller], [Action]) VALUES (5, N'Review own matrix data', NULL, NULL)
GO
INSERT [dbo].[Permission] ([Id], [Name], [Controller], [Action]) VALUES (6, N'Edit skills of subordinates', NULL, NULL)
GO
INSERT [dbo].[Permission] ([Id], [Name], [Controller], [Action]) VALUES (7, N'Edit all info', NULL, NULL)
GO
INSERT [dbo].[Permission] ([Id], [Name], [Controller], [Action]) VALUES (8, N'Edit own skills', NULL, NULL)
GO
INSERT [dbo].[Permission] ([Id], [Name], [Controller], [Action]) VALUES (9, N'Edit skills of employees of own location', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Permission] OFF
GO
SET IDENTITY_INSERT [dbo].[PermissionOnRole] ON 

GO
INSERT [dbo].[PermissionOnRole] ([Id], [RoleId], [PermissionId], [IsActive], [Controller], [Action]) VALUES (1, N'80f004dd-34da-484a-91ac-3ea8cb659979', 1, 1, NULL, NULL)
GO
INSERT [dbo].[PermissionOnRole] ([Id], [RoleId], [PermissionId], [IsActive], [Controller], [Action]) VALUES (2, N'80f004dd-34da-484a-91ac-3ea8cb659979', 2, 1, NULL, NULL)
GO
INSERT [dbo].[PermissionOnRole] ([Id], [RoleId], [PermissionId], [IsActive], [Controller], [Action]) VALUES (3, N'80f004dd-34da-484a-91ac-3ea8cb659979', 3, 1, NULL, NULL)
GO
INSERT [dbo].[PermissionOnRole] ([Id], [RoleId], [PermissionId], [IsActive], [Controller], [Action]) VALUES (4, N'80f004dd-34da-484a-91ac-3ea8cb659979', 4, 1, NULL, NULL)
GO
INSERT [dbo].[PermissionOnRole] ([Id], [RoleId], [PermissionId], [IsActive], [Controller], [Action]) VALUES (5, N'80f004dd-34da-484a-91ac-3ea8cb659979', 5, 0, NULL, NULL)
GO
INSERT [dbo].[PermissionOnRole] ([Id], [RoleId], [PermissionId], [IsActive], [Controller], [Action]) VALUES (6, N'80f004dd-34da-484a-91ac-3ea8cb659979', 6, 0, NULL, NULL)
GO
INSERT [dbo].[PermissionOnRole] ([Id], [RoleId], [PermissionId], [IsActive], [Controller], [Action]) VALUES (7, N'80f004dd-34da-484a-91ac-3ea8cb659979', 7, 0, NULL, NULL)
GO
INSERT [dbo].[PermissionOnRole] ([Id], [RoleId], [PermissionId], [IsActive], [Controller], [Action]) VALUES (8, N'80f004dd-34da-484a-91ac-3ea8cb659979', 8, 0, NULL, NULL)
GO
INSERT [dbo].[PermissionOnRole] ([Id], [RoleId], [PermissionId], [IsActive], [Controller], [Action]) VALUES (9, N'80f004dd-34da-484a-91ac-3ea8cb659979', 9, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[PermissionOnRole] OFF
GO
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON 

GO
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (17, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Manager10', N'1c5ed7d9-4448-42d9-a708-19050a8d366d')
GO
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (18, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Manager10', N'1c5ed7d9-4448-42d9-a708-19050a8d366d')
GO
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF
GO


SET IDENTITY_INSERT [dbo].[Company] ON 

GO
INSERT [dbo].[Company] ([Id], [Name]) VALUES (1, N'Company 1')
GO
SET IDENTITY_INSERT [dbo].[Company] OFF
GO
SET IDENTITY_INSERT [dbo].[RoleInProject] ON 

GO
INSERT [dbo].[RoleInProject] ([Id], [Name]) VALUES (1, N'Team lead')
GO
INSERT [dbo].[RoleInProject] ([Id], [Name]) VALUES (2, N'Developer')
GO
SET IDENTITY_INSERT [dbo].[RoleInProject] OFF
GO
SET IDENTITY_INSERT [dbo].[Project] ON 

GO
INSERT [dbo].[Project] ([Id], [Name]) VALUES (1, N'Project 1')
GO
SET IDENTITY_INSERT [dbo].[Project] OFF
GO
SET IDENTITY_INSERT [dbo].[EmployeePastProject] ON 

GO
INSERT [dbo].[EmployeePastProject] ([Id], [CompanyId], [WorkPeriod], [Description], [RoleId], [Team], [ProjectId], [EmployeeId]) VALUES (1, 1, CAST(0x0000A2AF00000000 AS DateTime), N'Lorem ipsum dolor sit amet, consectetuer sadipscing elitr, sed
          diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
          sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum.
          Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit
          amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam
          nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
          sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum.
          Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.
          Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod
          tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.
          At vero eos et accusam et justo duo dolores et ea rebum.
          Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.', 1, N'Team 7 ', 1, 11)
GO
INSERT [dbo].[EmployeePastProject] ([Id], [CompanyId], [WorkPeriod], [Description], [RoleId], [Team], [ProjectId], [EmployeeId]) VALUES (3, 1, CAST(0x0000A3C200000000 AS DateTime), N'New description test for me 2222', 2, N'Team 8', 1, 11)
GO
SET IDENTITY_INSERT [dbo].[EmployeePastProject] OFF
GO
SET IDENTITY_INSERT [dbo].[Technology] ON 

GO
INSERT [dbo].[Technology] ([Id], [Name]) VALUES (1, N'Java VM')
GO
INSERT [dbo].[Technology] ([Id], [Name]) VALUES (2, N'Java card')
GO
INSERT [dbo].[Technology] ([Id], [Name]) VALUES (3, N'Java 7')
GO
INSERT [dbo].[Technology] ([Id], [Name]) VALUES (4, N'Java 8')
GO
SET IDENTITY_INSERT [dbo].[Technology] OFF
GO
SET IDENTITY_INSERT [dbo].[EmployeePastProjectTechnology] ON 

GO
INSERT [dbo].[EmployeePastProjectTechnology] ([Id], [TechnologyId], [EmployeePastProjectId]) VALUES (1, 1, 1)
GO
INSERT [dbo].[EmployeePastProjectTechnology] ([Id], [TechnologyId], [EmployeePastProjectId]) VALUES (2, 2, 1)
GO
INSERT [dbo].[EmployeePastProjectTechnology] ([Id], [TechnologyId], [EmployeePastProjectId]) VALUES (3, 3, 1)
GO
INSERT [dbo].[EmployeePastProjectTechnology] ([Id], [TechnologyId], [EmployeePastProjectId]) VALUES (83, 1, 3)
GO
INSERT [dbo].[EmployeePastProjectTechnology] ([Id], [TechnologyId], [EmployeePastProjectId]) VALUES (84, 2, 3)
GO
INSERT [dbo].[EmployeePastProjectTechnology] ([Id], [TechnologyId], [EmployeePastProjectId]) VALUES (85, 3, 3)
GO
INSERT [dbo].[EmployeePastProjectTechnology] ([Id], [TechnologyId], [EmployeePastProjectId]) VALUES (86, 4, 3)
GO
SET IDENTITY_INSERT [dbo].[EmployeePastProjectTechnology] OFF
GO
SET IDENTITY_INSERT [dbo].[Tool] ON 

GO
INSERT [dbo].[Tool] ([Id], [Name]) VALUES (1, N'Manen')
GO
INSERT [dbo].[Tool] ([Id], [Name]) VALUES (2, N'Gradle')
GO
INSERT [dbo].[Tool] ([Id], [Name]) VALUES (3, N'Sbt')
GO
SET IDENTITY_INSERT [dbo].[Tool] OFF
GO
SET IDENTITY_INSERT [dbo].[EmployeePastProjectTool] ON 

GO
INSERT [dbo].[EmployeePastProjectTool] ([Id], [ToolId], [EmployeePastProjectId]) VALUES (1, 1, 1)
GO
INSERT [dbo].[EmployeePastProjectTool] ([Id], [ToolId], [EmployeePastProjectId]) VALUES (2, 2, 1)
GO
INSERT [dbo].[EmployeePastProjectTool] ([Id], [ToolId], [EmployeePastProjectId]) VALUES (3, 3, 1)
GO
INSERT [dbo].[EmployeePastProjectTool] ([Id], [ToolId], [EmployeePastProjectId]) VALUES (72, 1, 3)
GO
INSERT [dbo].[EmployeePastProjectTool] ([Id], [ToolId], [EmployeePastProjectId]) VALUES (73, 2, 3)
GO
INSERT [dbo].[EmployeePastProjectTool] ([Id], [ToolId], [EmployeePastProjectId]) VALUES (74, 3, 3)
GO
SET IDENTITY_INSERT [dbo].[EmployeePastProjectTool] OFF
GO

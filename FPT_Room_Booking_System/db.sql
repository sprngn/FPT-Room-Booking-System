USE [FPT_Room_Booking_System]
GO

-- Drop all existing tables in the correct dependency order
IF OBJECT_ID('[dbo].[booking]', 'U') IS NOT NULL DROP TABLE [dbo].[booking];
IF OBJECT_ID('[dbo].[users]', 'U') IS NOT NULL DROP TABLE [dbo].[users];
IF OBJECT_ID('[dbo].[slot]', 'U') IS NOT NULL DROP TABLE [dbo].[slot];
IF OBJECT_ID('[dbo].[role]', 'U') IS NOT NULL DROP TABLE [dbo].[role];
IF OBJECT_ID('[dbo].[room]', 'U') IS NOT NULL DROP TABLE [dbo].[room];
IF OBJECT_ID('[dbo].[department]', 'U') IS NOT NULL DROP TABLE [dbo].[department];
IF OBJECT_ID('[dbo].[room_type]', 'U') IS NOT NULL DROP TABLE [dbo].[room_type];
GO

-- Recreate the tables with IDENTITY(1,1) for primary keys

-- Role table
CREATE TABLE [dbo].[role] (
    [role_id] INT IDENTITY(1,1) PRIMARY KEY,
    [role_name] NVARCHAR(100) NOT NULL UNIQUE
);

-- Users table
CREATE TABLE [dbo].[users] (
    [user_id] INT IDENTITY(1,1) PRIMARY KEY,
    [user_name] NVARCHAR(100) NOT NULL,
    [email] NVARCHAR(255) NOT NULL UNIQUE,
    [dob] DATE NULL,
    [role] INT NOT NULL,
    [created_at] DATETIME DEFAULT GETDATE(),
    [is_active] BIT DEFAULT 1,
    [email_key] NVARCHAR(255) NULL,
    [password] NVARCHAR(255) NOT NULL,
    CONSTRAINT FK_Users_Role FOREIGN KEY ([role]) REFERENCES [dbo].[role]([role_id])
);

-- Department table
CREATE TABLE [dbo].[department] (
    [department_id] INT IDENTITY(1,1) PRIMARY KEY,
    [department_name] NVARCHAR(100) NOT NULL,
    [is_active] BIT DEFAULT 1
);

-- Room Type table
CREATE TABLE [dbo].[room_type] (
    [type_id] INT IDENTITY(1,1) PRIMARY KEY,
    [type_name] NVARCHAR(100) NOT NULL,
    [capacity] INT NOT NULL,
    [projector] BIT NOT NULL,
    [is_active] BIT DEFAULT 1
);

-- Slot table
CREATE TABLE [dbo].[slot] (
    [slot_id] INT IDENTITY(1,1) PRIMARY KEY,
    [slot_name] NVARCHAR(50) NOT NULL,
    [start_time] TIME NOT NULL,
    [end_time] TIME NOT NULL
);

-- Room table
CREATE TABLE [dbo].[room] (
    [room_id] INT IDENTITY(1,1) PRIMARY KEY,
    [department_id] INT NOT NULL,
    [room_name] NVARCHAR(100) NOT NULL,
    [type_id] INT NOT NULL,
    [is_active] BIT DEFAULT 1,
    CONSTRAINT FK_Room_Department FOREIGN KEY ([department_id]) REFERENCES [dbo].[department]([department_id]),
    CONSTRAINT FK_Room_Type FOREIGN KEY ([type_id]) REFERENCES [dbo].[room_type]([type_id])
);

-- Booking table
CREATE TABLE [dbo].[booking] (
    [booking_id] INT IDENTITY(1,1) PRIMARY KEY,
    [room_id] INT NOT NULL,
    [booking_user] INT NOT NULL,
    [booking_date] DATE NOT NULL,
    [slot] INT NOT NULL,
    [status] NVARCHAR(50) NOT NULL,
    [is_active] BIT DEFAULT 1,
    CONSTRAINT FK_Booking_Room FOREIGN KEY ([room_id]) REFERENCES [dbo].[room]([room_id]),
    CONSTRAINT FK_Booking_Slot FOREIGN KEY ([slot]) REFERENCES [dbo].[slot]([slot_id]),
    CONSTRAINT FK_Booking_User FOREIGN KEY ([booking_user]) REFERENCES [dbo].[users]([user_id])
);
GO

SET IDENTITY_INSERT [dbo].[booking] ON 

INSERT [dbo].[booking] ([booking_id], [room_id], [booking_user], [booking_date], [slot], [status], [is_active]) VALUES (1, 36, 1, CAST(N'2024-12-10' AS Date), 1, N'Pending', 1)
INSERT [dbo].[booking] ([booking_id], [room_id], [booking_user], [booking_date], [slot], [status], [is_active]) VALUES (2, 35, 1, CAST(N'2024-12-10' AS Date), 1, N'Pending', 1)
INSERT [dbo].[booking] ([booking_id], [room_id], [booking_user], [booking_date], [slot], [status], [is_active]) VALUES (3, 37, 1, CAST(N'2024-12-10' AS Date), 2, N'Pending', 1)
INSERT [dbo].[booking] ([booking_id], [room_id], [booking_user], [booking_date], [slot], [status], [is_active]) VALUES (1003, 35, 1, CAST(N'2024-12-12' AS Date), 5, N'Pending', 1)
INSERT [dbo].[booking] ([booking_id], [room_id], [booking_user], [booking_date], [slot], [status], [is_active]) VALUES (1004, 35, 1, CAST(N'2024-12-11' AS Date), 5, N'Pending', 1)
INSERT [dbo].[booking] ([booking_id], [room_id], [booking_user], [booking_date], [slot], [status], [is_active]) VALUES (1005, 35, 1, CAST(N'2024-12-11' AS Date), 2, N'Pending', 1)
INSERT [dbo].[booking] ([booking_id], [room_id], [booking_user], [booking_date], [slot], [status], [is_active]) VALUES (1006, 35, 1, CAST(N'2024-12-11' AS Date), 1, N'Pending', 1)
INSERT [dbo].[booking] ([booking_id], [room_id], [booking_user], [booking_date], [slot], [status], [is_active]) VALUES (1007, 31, 1, CAST(N'2024-12-10' AS Date), 1, N'Pending', 1)
SET IDENTITY_INSERT [dbo].[booking] OFF
GO
SET IDENTITY_INSERT [dbo].[department] ON 

INSERT [dbo].[department] ([department_id], [department_name], [is_active]) VALUES (1, N'Alpha', 1)
INSERT [dbo].[department] ([department_id], [department_name], [is_active]) VALUES (2, N'Beta', 1)
INSERT [dbo].[department] ([department_id], [department_name], [is_active]) VALUES (3, N'Gamma', 1)
INSERT [dbo].[department] ([department_id], [department_name], [is_active]) VALUES (4, N'Delta', 1)
SET IDENTITY_INSERT [dbo].[department] OFF
GO
INSERT [dbo].[role] ([role_id], [role_name]) VALUES (1, N'admin')
INSERT [dbo].[role] ([role_id], [role_name]) VALUES (2, N'manager')
INSERT [dbo].[role] ([role_id], [role_name]) VALUES (3, N'teacher')
INSERT [dbo].[role] ([role_id], [role_name]) VALUES (4, N'user')
GO
SET IDENTITY_INSERT [dbo].[room] ON 

INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (1, 1, N'A201', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (2, 1, N'A202', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (3, 1, N'A203', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (4, 1, N'A204', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (5, 1, N'A205', 3, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (6, 1, N'A206', 3, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (7, 1, N'A207', 3, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (8, 1, N'A208', 3, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (9, 1, N'A209', 3, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (10, 1, N'A210', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (11, 1, N'A301', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (12, 1, N'A302', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (13, 1, N'A303', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (14, 1, N'A304', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (15, 1, N'A305', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (16, 1, N'A306', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (17, 1, N'A307', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (18, 1, N'A308', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (19, 1, N'A309', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (20, 1, N'A310', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (21, 1, N'A101L', 6, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (22, 1, N'A102L', 6, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (23, 1, N'A103L', 6, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (24, 2, N'B101L', 6, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (25, 2, N'B102L', 6, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (26, 2, N'B103L', 6, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (27, 2, N'B104', 6, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (28, 2, N'B105', 2, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (29, 2, N'B106', 2, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (30, 2, N'B201', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (31, 2, N'B202', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (32, 2, N'B203', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (33, 2, N'B204', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (34, 2, N'B205', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (35, 2, N'B206', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (36, 2, N'B207', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (37, 2, N'B208', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (38, 2, N'B209', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (39, 2, N'B210', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (40, 2, N'B301', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (41, 2, N'B302', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (42, 2, N'B303', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (43, 2, N'B304', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (44, 2, N'B305', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (45, 2, N'B306', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (46, 2, N'B307', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (47, 2, N'B308', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (48, 2, N'B309', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (49, 2, N'B310', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (50, 3, N'C301C', 1, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (51, 3, N'C302C', 1, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (52, 3, N'C303C', 1, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (53, 3, N'C304C', 2, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (54, 3, N'C305C', 2, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (55, 3, N'C306C', 2, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (56, 1, N'A401', 4, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (57, 1, N'A402', 4, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (58, 1, N'A403', 4, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (59, 1, N'A404', 4, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (60, 4, N'D101', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (61, 4, N'D102', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (62, 4, N'D103', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (63, 4, N'D104', 3, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (64, 4, N'D105', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (65, 4, N'D106', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (66, 4, N'D107', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (67, 4, N'D108', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (68, 4, N'D109', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (69, 4, N'D110', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (70, 4, N'D201', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (71, 4, N'D202', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (72, 4, N'D203', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (73, 4, N'D204', 3, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (74, 4, N'D205', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (75, 4, N'D206', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (76, 4, N'D207', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (77, 4, N'D208', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (78, 4, N'D209', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (79, 4, N'D210', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (80, 4, N'D301', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (81, 4, N'D302', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (82, 4, N'D303', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (83, 4, N'D304', 3, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (84, 4, N'D305', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (85, 4, N'D306', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (86, 4, N'D307', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (87, 4, N'D308', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (88, 4, N'D309', 5, 1)
INSERT [dbo].[room] ([room_id], [department_id], [room_name], [type_id], [is_active]) VALUES (89, 4, N'D310', 5, 1)
SET IDENTITY_INSERT [dbo].[room] OFF
GO
SET IDENTITY_INSERT [dbo].[room_type] ON 

INSERT [dbo].[room_type] ([type_id], [type_name], [capacity], [projector], [is_active]) VALUES (1, N'Conference Room', 100, 1, 1)
INSERT [dbo].[room_type] ([type_id], [type_name], [capacity], [projector], [is_active]) VALUES (2, N'Meeting Room', 20, 0, 1)
INSERT [dbo].[room_type] ([type_id], [type_name], [capacity], [projector], [is_active]) VALUES (3, N'Lab Room', 31, 0, 1)
INSERT [dbo].[room_type] ([type_id], [type_name], [capacity], [projector], [is_active]) VALUES (4, N'Lecture Hall', 80, 1, 1)
INSERT [dbo].[room_type] ([type_id], [type_name], [capacity], [projector], [is_active]) VALUES (5, N'Classroom', 31, 0, 1)
INSERT [dbo].[room_type] ([type_id], [type_name], [capacity], [projector], [is_active]) VALUES (6, N'Lobby', -1, 0, 1)
SET IDENTITY_INSERT [dbo].[room_type] OFF
GO
SET IDENTITY_INSERT [dbo].[slot] ON 

INSERT [dbo].[slot] ([slot_id], [slot_name], [start_time], [end_time]) VALUES (1, 1, CAST(N'07:30:00' AS Time), CAST(N'09:00:00' AS Time))
INSERT [dbo].[slot] ([slot_id], [slot_name], [start_time], [end_time]) VALUES (2, 2, CAST(N'09:10:00' AS Time), CAST(N'12:20:00' AS Time))
INSERT [dbo].[slot] ([slot_id], [slot_name], [start_time], [end_time]) VALUES (3, 3, CAST(N'12:50:00' AS Time), CAST(N'15:10:00' AS Time))
INSERT [dbo].[slot] ([slot_id], [slot_name], [start_time], [end_time]) VALUES (4, 4, CAST(N'15:20:00' AS Time), CAST(N'17:40:00' AS Time))
INSERT [dbo].[slot] ([slot_id], [slot_name], [start_time], [end_time]) VALUES (5, 5, CAST(N'17:50:00' AS Time), CAST(N'18:10:00' AS Time))
INSERT [dbo].[slot] ([slot_id], [slot_name], [start_time], [end_time]) VALUES (6, 6, CAST(N'18:20:00' AS Time), CAST(N'20:40:00' AS Time))
INSERT [dbo].[slot] ([slot_id], [slot_name], [start_time], [end_time]) VALUES (7, 7, CAST(N'20:50:00' AS Time), CAST(N'22:10:00' AS Time))
SET IDENTITY_INSERT [dbo].[slot] OFF
GO
SET IDENTITY_INSERT [dbo].[users] ON 

INSERT [dbo].[users] ([user_id], [user_name], [email], [dob], [role], [created_at], [is_active], [email_key], [password]) VALUES (1, N'admin', N'admin@gmail.com', CAST(N'2003-06-13' AS Date), N'1', CAST(N'2024-11-29T15:23:53.050' AS DateTime), 1, NULL, N'123456')
SET IDENTITY_INSERT [dbo].[users] OFF
GO
SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[booking] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[department] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[room] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[room_type] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT ((1)) FOR [is_active]
GO

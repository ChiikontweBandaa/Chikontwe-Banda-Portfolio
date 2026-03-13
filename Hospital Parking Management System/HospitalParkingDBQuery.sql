USE [HospitalParkingDB]
GO

-- Users table for login system
CREATE TABLE [dbo].[Users](
    [UserId] [int] IDENTITY(1,1) NOT NULL,
    [Username] [nvarchar](50) NOT NULL,
    [Password] [nvarchar](100) NOT NULL,
    [Role] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
)
GO

-- Parking sections
CREATE TABLE [dbo].[ParkingSections](
    [SectionId] [int] IDENTITY(1,1) NOT NULL,
    [SectionName] [nvarchar](50) NOT NULL,
    [TotalSpaces] [int] NOT NULL,
 CONSTRAINT [PK_ParkingSections] PRIMARY KEY CLUSTERED ([SectionId] ASC)
 )
GO

-- Parking spaces
CREATE TABLE [dbo].[ParkingSpaces](
    [SpaceId] [int] IDENTITY(1,1) NOT NULL,
    [SectionId] [int] NOT NULL,
    [SpaceNumber] [nvarchar](10) NOT NULL,
    [Status] [nvarchar](20) NOT NULL, -- Available, Occupied, Reserved
 CONSTRAINT [PK_ParkingSpaces] PRIMARY KEY CLUSTERED ([SpaceId] ASC)
)
GO

-- Vehicles
CREATE TABLE [dbo].[Vehicles](
    [VehicleId] [int] IDENTITY(1,1) NOT NULL,
    [LicensePlate] [nvarchar](20) NOT NULL,
    [Make] [nvarchar](50) NOT NULL,
    [Model] [nvarchar](50) NOT NULL,
    [Color] [nvarchar](30) NOT NULL,
    [OwnerName] [nvarchar](100) NOT NULL,
    [OwnerPhone] [nvarchar](20) NULL,
    [OwnerEmail] [nvarchar](100) NULL,
 CONSTRAINT [PK_Vehicles] PRIMARY KEY CLUSTERED ([VehicleId] ASC)
)
GO

-- Parking tickets
CREATE TABLE [dbo].[ParkingTickets](
    [TicketId] [int] IDENTITY(1,1) NOT NULL,
    [VehicleId] [int] NOT NULL,
    [SpaceId] [int] NOT NULL,
    [EntryTime] [datetime] NOT NULL,
    [ExitTime] [datetime] NULL,
    [TotalHours] [decimal](10, 2) NULL,
    [TotalAmount] [decimal](10, 2) NULL,
    [Status] [nvarchar](20) NOT NULL, -- Active, Paid, Reserved
 CONSTRAINT [PK_ParkingTickets] PRIMARY KEY CLUSTERED ([TicketId] ASC)
 )
GO

-- Payments
CREATE TABLE [dbo].[Payments](
    [PaymentId] [int] IDENTITY(1,1) NOT NULL,
    [TicketId] [int] NOT NULL,
    [Amount] [decimal](10, 2) NOT NULL,
    [PaymentMethod] [nvarchar](20) NOT NULL, -- Cash, MobileMoney, Card
    [PaymentTime] [datetime] NOT NULL,
    [TransactionReference] [nvarchar](100) NULL,
    [Status] [nvarchar](20) NOT NULL, -- Pending, Completed, Failed
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED ([PaymentId] ASC)
)
GO

-- Add foreign key constraints
ALTER TABLE [dbo].[ParkingSpaces] ADD CONSTRAINT [FK_ParkingSpaces_ParkingSections] FOREIGN KEY([SectionId])
REFERENCES [dbo].[ParkingSections] ([SectionId])
GO

ALTER TABLE [dbo].[ParkingTickets] ADD CONSTRAINT [FK_ParkingTickets_Vehicles] FOREIGN KEY([VehicleId])
REFERENCES [dbo].[Vehicles] ([VehicleId])
GO

ALTER TABLE [dbo].[ParkingTickets] ADD CONSTRAINT [FK_ParkingTickets_ParkingSpaces] FOREIGN KEY([SpaceId])
REFERENCES [dbo].[ParkingSpaces] ([SpaceId])
GO

ALTER TABLE [dbo].[Payments] ADD CONSTRAINT [FK_Payments_ParkingTickets] FOREIGN KEY([TicketId])
REFERENCES [dbo].[ParkingTickets] ([TicketId])
GO

-- Insert initial data
INSERT INTO [dbo].[ParkingSections] ([SectionName], [TotalSpaces]) VALUES 
('Visitors', 50),
('Patients', 30),
('Hospital Staff', 20),
('Emergency Vehicles', 20),
('Disabled or Infirm', 20)
GO

-- Insert parking spaces for Visitors section (50 spaces)
DECLARE @i int = 1
WHILE @i <= 50
BEGIN
    INSERT INTO [dbo].[ParkingSpaces] ([SectionId], [SpaceNumber], [Status]) VALUES (1, 'V' + RIGHT('00' + CAST(@i AS VARCHAR(2)), 2), 'Available')
    SET @i = @i + 1
END
GO

-- Insert parking spaces for Patients section (30 spaces)
DECLARE @i int = 1
WHILE @i <= 30
BEGIN
    INSERT INTO [dbo].[ParkingSpaces] ([SectionId], [SpaceNumber], [Status]) VALUES (2, 'P' + RIGHT('00' + CAST(@i AS VARCHAR(2)), 2), 'Available')
    SET @i = @i + 1
END
GO

-- Insert parking spaces for Hospital Staff section (20 spaces)
DECLARE @i int = 1
WHILE @i <= 20
BEGIN
    INSERT INTO [dbo].[ParkingSpaces] ([SectionId], [SpaceNumber], [Status]) VALUES (3, 'H' + RIGHT('00' + CAST(@i AS VARCHAR(2)), 2), 'Available')
    SET @i = @i + 1
END
GO

-- Insert parking spaces for Emergency vehicles sections
DECLARE @i int = 1
WHILE @i <= 20
BEGIN
    INSERT INTO [dbo].[ParkingSpaces] ([SectionId], [SpaceNumber], [Status]) VALUES (4, 'E' + RIGHT('00' + CAST(@i AS VARCHAR(2)), 2), 'Available')
    SET @i = @i + 1
END
GO
-- Insert parking spaces for disabled and infirm
DECLARE @i int = 1
WHILE @i <= 20
BEGIN
    INSERT INTO [dbo].[ParkingSpaces] ([SectionId], [SpaceNumber], [Status]) VALUES (5, 'D' + RIGHT('00' + CAST(@i AS VARCHAR(2)), 2), 'Available')
    SET @i = @i + 1
END
GO

-- Create a test user
INSERT INTO [dbo].[Users] ([Username], [Password], [Role]) VALUES 
('admin', 'Admin@123', 'Administrator')
GO

INSERT INTO [dbo].[Users] ([Username], [Password], [Role]) VALUES 
('admin', 'Admin@123', 'Administrator')
GO

SELECT * FROM Vehicles;
SELECT * FROM ParkingSpaces;
SELECT * FROM ParkingTickets;




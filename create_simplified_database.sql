-- Create simplified Boatify database for LocalDB
-- Run this script in SQL Server Management Studio or Azure Data Studio connected to (localdb)\MSSQLLocalDB

-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'db_BoatifySimple')
BEGIN
    ALTER DATABASE db_BoatifySimple SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE db_BoatifySimple;
END
GO

-- Create new database
CREATE DATABASE db_BoatifySimple;
GO

USE db_BoatifySimple;
GO

-- Create Users table
CREATE TABLE Users (
    UserId int IDENTITY(1,1) PRIMARY KEY,
    Email nvarchar(255) NOT NULL UNIQUE,
    Password nvarchar(255) NOT NULL,
    FullName nvarchar(255) NOT NULL,
    Phone nvarchar(20) NULL,
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive bit NOT NULL DEFAULT 1
);

-- Create Orders table
CREATE TABLE Orders (
    OrderId int IDENTITY(1,1) PRIMARY KEY,
    UserId int NOT NULL,
    OrderCode nvarchar(50) NOT NULL,
    Departure nvarchar(255) NOT NULL,
    Destination nvarchar(255) NOT NULL,
    DepartureDate datetime2 NOT NULL,
    DepartureTime nvarchar(10) NULL,
    TotalAmount decimal(18,2) NOT NULL,
    Status nvarchar(50) NOT NULL DEFAULT 'Pending',
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    ExpiresAt datetime2 NULL,
    ContactName nvarchar(255) NULL,
    ContactEmail nvarchar(255) NULL,
    ContactPhone nvarchar(20) NULL,
    ExternalVoyageId int NULL,
    ExternalScheduleId int NULL,
    ExternalRouteId int NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);

-- Create Tickets table
CREATE TABLE Tickets (
    TicketId int IDENTITY(1,1) PRIMARY KEY,
    OrderId int NOT NULL,
    SeatCode nvarchar(10) NOT NULL,
    PassengerName nvarchar(255) NOT NULL,
    PassengerPhone nvarchar(20) NULL,
    PassengerIdCard nvarchar(20) NULL,
    PassengerDateOfBirth datetime2 NULL,
    Price decimal(18,2) NOT NULL,
    TicketType nvarchar(50) NOT NULL DEFAULT 'Adult',
    TicketClass nvarchar(50) NOT NULL DEFAULT 'Economy',
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    ExternalSeatId int NULL,
    ExternalTicketPriceId int NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE
);

-- Create Payments table
CREATE TABLE Payments (
    PaymentId int IDENTITY(1,1) PRIMARY KEY,
    OrderId int NOT NULL,
    PaymentMethod nvarchar(50) NOT NULL,
    Amount decimal(18,2) NOT NULL,
    Status nvarchar(50) NOT NULL DEFAULT 'Pending',
    TransactionId nvarchar(255) NULL,
    PaymentGatewayResponse nvarchar(max) NULL,
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    PaidAt datetime2 NULL,
    FailureReason nvarchar(500) NULL,
    GatewayTransactionId nvarchar(255) NULL,
    GatewayOrderId nvarchar(255) NULL,
    CallbackData nvarchar(max) NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE
);

-- Create indexes for better performance
CREATE INDEX IX_Orders_UserId ON Orders(UserId);
CREATE INDEX IX_Orders_Status ON Orders(Status);
CREATE INDEX IX_Orders_CreatedAt ON Orders(CreatedAt);
CREATE INDEX IX_Tickets_OrderId ON Tickets(OrderId);
CREATE INDEX IX_Payments_OrderId ON Payments(OrderId);
CREATE INDEX IX_Payments_Status ON Payments(Status);

-- Insert sample data for testing
INSERT INTO Users (Email, Password, FullName, Phone) VALUES 
('test@example.com', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 'Test User', '0123456789'),
('admin@boatify.com', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 'Admin User', '0987654321');

PRINT 'Simplified database created successfully!';
PRINT 'Test accounts:';
PRINT 'Email: test@example.com, Password: secret';
PRINT 'Email: admin@boatify.com, Password: secret';
GO

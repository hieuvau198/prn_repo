-- Create the database
CREATE DATABASE MyStore;
GO

-- Use the newly created database
USE MyStore;
GO

-- Create AccountMember table
CREATE TABLE AccountMember (
    MemberID NVARCHAR(20) NOT NULL PRIMARY KEY,
    MemberPassword NVARCHAR(80) NOT NULL,
    FullName NVARCHAR(80) NOT NULL,
    EmailAddress NVARCHAR(100) NOT NULL,
    MemberRole INT NOT NULL
);
GO

-- Create Categories table
CREATE TABLE Categories (
    CategoryID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(15) NOT NULL
);
GO

-- Create Products table
CREATE TABLE Products (
    ProductID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(40) NOT NULL,
    CategoryID INT NOT NULL,
    UnitsInStock SMALLINT NULL,
    UnitPrice MONEY NULL,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);
GO



-- Use the MyStore database
USE MyStore;
GO

-- Insert sample data into AccountMember
INSERT INTO AccountMember (MemberID, MemberPassword, FullName, EmailAddress, MemberRole) VALUES
('U001', 'hashedpassword123', 'Alice Johnson', 'alice@example.com', 1),
('U002', 'hashedpassword456', 'Bob Smith', 'bob@example.com', 2),
('U003', 'hashedpassword789', 'Charlie Brown', 'charlie@example.com', 2),
('U004', 'hashedpassword101', 'David Miller', 'david@example.com', 1),
('U005', 'hashedpassword202', 'Eve Davis', 'eve@example.com', 3);
GO

-- Insert sample data into Categories (Shortened names to fit NVARCHAR(15))
INSERT INTO Categories (CategoryName) VALUES
('Electronics'),
('Clothing'),
('Home Decor'),
('Books'),
('Sports');
GO

-- Insert sample data into Products (Ensure valid CategoryID references)
INSERT INTO Products (ProductName, CategoryID, UnitsInStock, UnitPrice) VALUES
('Smartphone', 1, 50, 699.99),
('Laptop', 1, 30, 1199.99),
('Wireless Headphones', 1, 100, 199.99),
('Jeans', 2, 200, 49.99),
('T-Shirt', 2, 150, 19.99),
('Sofa', 3, 10, 599.99),
('Table Lamp', 3, 80, 39.99),
('Fiction Novel', 4, 500, 14.99),
('Cookbook', 4, 250, 24.99),
('Basketball', 5, 90, 29.99),
('Tennis Racket', 5, 40, 89.99);
GO








-- Delete existing data before inserting new seed data
DELETE FROM Products;
DELETE FROM Categories;
DELETE FROM AccountMember;
GO

-- Reset identity columns (if applicable, for auto-incrementing primary keys)
DBCC CHECKIDENT ('Products', RESEED, 0);
DBCC CHECKIDENT ('Categories', RESEED, 0);
DBCC CHECKIDENT ('AccountMember', RESEED, 0);
GO

-- Insert new sample data into AccountMember
INSERT INTO AccountMember (MemberID, MemberPassword, FullName, EmailAddress, MemberRole) VALUES
('U006', 'newhashedpass001', 'Sophia Martinez', 'sophia@example.com', 1),
('U007', 'newhashedpass002', 'Liam Thompson', 'liam@example.com', 2),
('U008', 'newhashedpass003', 'Olivia Walker', 'olivia@example.com', 2),
('U009', 'newhashedpass004', 'Noah White', 'noah@example.com', 1),
('U010', 'newhashedpass005', 'Emma Harris', 'emma@example.com', 3);
GO

-- Insert new sample data into Categories
INSERT INTO Categories (CategoryName) VALUES
('Furniture'),
('Beauty & Health'),
('Toys'),
('Kitchenware'),
('Automotive');
GO

-- Insert new sample data into Products (Ensure valid CategoryID references)
INSERT INTO Products (ProductName, CategoryID, UnitsInStock, UnitPrice) VALUES
('Wooden Dining Table', 1, 15, 799.99),
('Office Chair', 1, 25, 149.99),
('Face Serum', 2, 100, 39.99),
('Herbal Shampoo', 2, 80, 24.99),
('Building Blocks Set', 3, 120, 34.99),
('Remote Control Car', 3, 60, 59.99),
('Non-Stick Frying Pan', 4, 90, 27.99),
('Chef’s Knife Set', 4, 70, 89.99),
('Car Vacuum Cleaner', 5, 40, 69.99),
('Dashboard Camera', 5, 35, 129.99);
GO

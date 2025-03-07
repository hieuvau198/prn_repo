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

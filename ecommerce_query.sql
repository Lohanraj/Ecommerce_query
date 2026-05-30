create database ECOMMERCE_ASSIGNMENT_DB;
GO

use ECOMMERCE_ASSIGNMENT_DB;
GO
-- TABLE CREATION

drop table if exists OrderItem;
drop table if exists Orders;
drop table if exists Product;
drop table if exists Seller;
drop table if Exists Customer;

create table Customer
(
CustomerId int primary key identity,
CustomerName varchar(100) not null,
Email varchar(100) unique not null,
MobileNo bigint not null,
City varchar(100),
Address varchar(200),
IsActive bit default 1,
CreatedDate datetime default getdate()
);

create table Seller
(
SellerId int primary key identity,
SellerName varchar(100)not null,
Email varchar(100) unique not null,
MobileNo bigint not null,
City varchar(100),
Rating decimal(2,1),
IsActive bit default 1
);

create table Product
(
ProductId int primary key identity,
ProductName varchar(100) not null,
Category varchar(100),
Price money check(Price > 0),
StockQuantity int check(StockQuantity >= 0),
SellerId int,
CreatedDate datetime default getdate(),

constraint fk_product_seller
foreign key(SellerId)
references Seller(SellerId)
);

create table Orders
(
OrderId int primary key identity,
CustomerId int,
OrderDate Datetime default getdate(),
OrderStatus varchar(50) default 'Pending',
PaymentMode varchar(50),
DeliveryCity varchar(100),

constraint fk_orders_customer
foreign key(CustomerId)
references Customer(CustomerId)
);

create table OrderItem
(
OrderItemId int primary key identity,
OrderId int,
ProductId int,
quantity int check(Quantity > 0),
UnitPrice money,

constraint fk_orderitem_order
foreign key(OrderId)
references Orders(orderId),

CONSTRAINT fk_orderitem_product
FOREIGN KEY(ProductId)
REFERENCES Product(ProductId)
);

--- INSERT RECORDS

INSERT INTO Customer
(CustomerName,Email,MobileNo,City,Address)
VALUES
('Arun Kumar','arun@gmail.com',9876543210,'Chennai','Anna Nagar'),
('Priya Sharma','priya@gmail.com',9876543211,'Bangalore','MG Road'),
('Rahul Verma','rahul@gmail.com',9876543212,'Hyderabad','Banjara Hills'),
('Sneha Reddy','sneha@gmail.com',9876543213,'Chennai','Velachery'),
('Akash Singh','akash@gmail.com',9876543214,'Mumbai','Andheri');


INSERT INTO Seller
(SellerName,Email,MobileNo,City,Rating)
VALUES
('Tech World','techworld@gmail.com',9000000001,'Chennai',4.5),
('Mobile Hub','mobilehub@gmail.com',9000000002,'Bangalore',4.2),
('Laptop Store','laptop@gmail.com',9000000003,'Hyderabad',4.7),
('Gadget Zone','gadget@gmail.com',9000000004,'Mumbai',4.1);


INSERT INTO Product
(ProductName,Category,Price,StockQuantity,SellerId)
VALUES
('iPhone 15','Mobile',80000,10,2),
('Samsung S24','Mobile',75000,8,2),
('Dell XPS','Laptop',95000,5,3),
('HP Pavilion','Laptop',65000,6,3),
('Boat Headset','Accessories',3000,20,1),
('Smart Watch','Accessories',12000,15,1),
('Lenovo Legion','Laptop',110000,4,3),
('OnePlus 12','Mobile',60000,7,2);

INSERT INTO Orders
(CustomerId,PaymentMode,DeliveryCity)
VALUES
(1,'UPI','Chennai'),
(2,'Card','Bangalore'),
(3,'Cash On Delivery','Hyderabad'),
(4,'UPI','Chennai'),
(5,'Card','Mumbai');


INSERT INTO OrderItem
(OrderId,ProductId,Quantity,UnitPrice)
VALUES
(1,1,1,80000),
(1,5,2,3000),
(2,3,1,95000),
(2,6,1,12000),
(3,2,1,75000),
(3,8,1,60000),
(4,4,1,65000),
(4,5,1,3000),
(5,7,1,110000),
(5,6,2,12000);

-- WHERE CLAUSE 

SELECT * FROM Customer WHERE City='Chennai';

SELECT * FROM Customer WHERE City<>'Chennai';

SELECT * FROM Product WHERE Price > 50000;

SELECT * FROM Product WHERE Price BETWEEN 10000 AND 60000;

SELECT * FROM Product WHERE Category IN ('Mobile','Laptop');

SELECT * FROM Customer WHERE CustomerName LIKE 'A%';

SELECT * FROM Customer WHERE Email LIKE '%gmail%';

SELECT * FROM Product WHERE ProductName LIKE '%Phone%';

SELECT * FROM Orders WHERE OrderStatus='Delivered';

SELECT * FROM Product WHERE StockQuantity < 10;

SELECT * FROM Customer WHERE MobileNo IS NOT NULL;

SELECT * FROM Product WHERE Price NOT BETWEEN 10000 AND 50000;

SELECT * FROM Customer WHERE City IN ('Chennai','Bangalore');

SELECT * FROM Customer WHERE City='Chennai' AND IsActive=1;

SELECT * FROM Customer WHERE City<>'Hyderabad';

--- GROUP BY QUESTIONS

SELECT City,COUNT(CustomerId) AS TotalCustomers FROM Customer GROUP BY City;

SELECT Category,COUNT(ProductId) AS TotalProducts FROM Product GROUP BY Category;

SELECT Category,SUM(StockQuantity) AS TotalStock FROM Product GROUP BY Category;

SELECT Category,MAX(Price) AS MaximumPrice FROM Product GROUP BY Category;

SELECT Category,MIN(Price) AS MinimumPrice FROM Product GROUP BY Category;

SELECT Category,AVG(Price) AS AveragePrice FROM Product GROUP BY Category;

SELECT c.CustomerName,SUM(oi.Quantity*oi.UnitPrice) AS TotalOrderAmount FROM Customer c INNER JOIN Orders o ON c.CustomerId=o.CustomerId INNER JOIN OrderItem oi ON o.OrderId=oi.OrderId GROUP BY c.CustomerName;

SELECT p.ProductName,SUM(oi.Quantity*oi.UnitPrice) AS TotalSales FROM Product p INNER JOIN OrderItem oi ON p.ProductId=oi.ProductId GROUP BY p.ProductName;

SELECT p.ProductName,SUM(oi.Quantity) AS TotalQuantitySold FROM Product p INNER JOIN OrderItem oi ON p.ProductId=oi.ProductId GROUP BY p.ProductName;

SELECT Category,COUNT(ProductId) AS TotalProducts FROM Product GROUP BY Category HAVING COUNT(ProductId)>1;

--ORDER BY

SELECT * FROM Product ORDER BY Price ASC;

SELECT * FROM Product ORDER BY Price DESC;

SELECT * FROM Customer ORDER BY City ASC,CustomerName ASC;

SELECT * FROM Orders ORDER BY OrderDate DESC;

SELECT * FROM Product ORDER BY Category ASC,Price DESC;

SELECT TOP 3 * FROM Product ORDER BY Price DESC;

SELECT TOP 5 * FROM Orders ORDER BY OrderDate DESC;

SELECT * FROM Customer ORDER BY IsActive DESC,CustomerName ASC;

SELECT o.OrderId,c.CustomerName,c.City,o.OrderDate,o.OrderStatus,o.PaymentMode FROM Orders o INNER JOIN Customer c ON o.CustomerId=c.CustomerId;

SELECT p.ProductName,p.Category,p.Price,s.SellerName,s.City FROM Product p INNER JOIN Seller s ON p.SellerId=s.SellerId;

SELECT oi.OrderItemId,p.ProductName,oi.Quantity,oi.UnitPrice FROM OrderItem oi INNER JOIN Product p ON oi.ProductId=p.ProductId;

SELECT c.CustomerName,o.OrderId,p.ProductName,s.SellerName,oi.Quantity,oi.UnitPrice,(oi.Quantity*oi.UnitPrice) AS TotalAmount FROM Customer c INNER JOIN Orders o ON c.CustomerId=o.CustomerId INNER JOIN OrderItem oi ON o.OrderId=oi.OrderId INNER JOIN Product p ON oi.ProductId=p.ProductId INNER JOIN Seller s ON p.SellerId=s.SellerId;

SELECT c.CustomerName,o.OrderId,o.OrderStatus FROM Customer c LEFT JOIN Orders o ON c.CustomerId=o.CustomerId;

SELECT c.CustomerName,o.OrderId,o.OrderStatus FROM Customer c RIGHT JOIN Orders o ON c.CustomerId=o.CustomerId;

SELECT c.CustomerName,o.OrderId,o.OrderStatus FROM Customer c FULL OUTER JOIN Orders o ON c.CustomerId=o.CustomerId;

SELECT c.CustomerName,p.ProductName FROM Customer c CROSS JOIN Product p;

SELECT * FROM Customer WHERE CustomerId NOT IN(SELECT CustomerId FROM Orders);

SELECT * FROM Product WHERE ProductId NOT IN(SELECT ProductId FROM OrderItem);

SELECT s.SellerName,p.ProductName,p.Category,p.Price FROM Seller s INNER JOIN Product p ON s.SellerId=p.SellerId;

SELECT c.CustomerName,p.ProductName,oi.Quantity FROM Customer c INNER JOIN Orders o ON c.CustomerId=o.CustomerId INNER JOIN OrderItem oi ON o.OrderId=oi.OrderId INNER JOIN Product p ON oi.ProductId=p.ProductId;

SELECT o.OrderId,SUM(oi.Quantity*oi.UnitPrice) AS TotalAmount FROM Orders o INNER JOIN OrderItem oi ON o.OrderId=oi.OrderId GROUP BY o.OrderId;

SELECT s.SellerName,SUM(oi.Quantity*oi.UnitPrice) AS TotalSales FROM Seller s INNER JOIN Product p ON s.SellerId=p.SellerId INNER JOIN OrderItem oi ON p.ProductId=oi.ProductId GROUP BY s.SellerName;

SELECT p.ProductName,SUM(oi.Quantity) AS TotalQuantitySold FROM Product p INNER JOIN OrderItem oi ON p.ProductId=oi.ProductId GROUP BY p.ProductName;

--A.Basic subquery questions

SELECT *
FROM Product
WHERE Price >
(
SELECT AVG(Price)
FROM Product
);

SELECT *
FROM Product
WHERE StockQuantity <
(
SELECT AVG(StockQuantity)
FROM Product
);

SELECT *
FROM Customer
WHERE CustomerId IN
(
SELECT CustomerId
FROM Orders
);

SELECT *
FROM Customer
WHERE CustomerId NOT IN
(
SELECT CustomerId
FROM Orders
);

SELECT *
FROM Product
WHERE ProductId IN
(
SELECT ProductId
FROM OrderItem
);

SELECT *
FROM Product
WHERE ProductId NOT IN
(
SELECT ProductId
FROM OrderItem
);

SELECT *
FROM Seller
WHERE SellerId IN
(
SELECT SellerId
FROM Product
);

SELECT *
FROM Seller
WHERE SellerId NOT IN
(
SELECT SellerId
FROM Product
);

SELECT *
FROM Orders
WHERE CustomerId IN
(
SELECT CustomerId
FROM Customer
WHERE City='Chennai'
);

SELECT *
FROM Product
WHERE SellerId IN
(
SELECT SellerId
FROM Seller
WHERE City='Bangalore'
);

--Subquery with IN / NOT IN

SELECT *
FROM Customer
WHERE CustomerId IN
(
SELECT CustomerId
FROM Orders
);

SELECT *
FROM Customer
WHERE CustomerId NOT IN
(
SELECT CustomerId
FROM Orders
);

SELECT *
FROM Product
WHERE ProductId IN
(
SELECT ProductId
FROM OrderItem
);

SELECT *
FROM Product
WHERE ProductId NOT IN
(
SELECT ProductId
FROM OrderItem
);

SELECT *
FROM Seller
WHERE SellerId IN
(
SELECT SellerId
FROM Product
);

SELECT *
FROM Seller
WHERE SellerId NOT IN
(
SELECT SellerId
FROM Product
);

SELECT *
FROM Orders
WHERE OrderId IN
(
SELECT OrderId
FROM OrderItem
WHERE ProductId IN
(
SELECT ProductId
FROM Product
WHERE Category='Mobile'
)
);

SELECT *
FROM Orders
WHERE OrderId NOT IN
(
SELECT OrderId
FROM OrderItem
WHERE ProductId IN
(
SELECT ProductId
FROM Product
WHERE Category='Laptop'
)
);

--Subquery with Aggregate Functions

SELECT *
FROM Product
WHERE Price =
(
SELECT MAX(Price)
FROM Product
);

SELECT *
FROM Product
WHERE Price =
(
SELECT MIN(Price)
FROM Product
);

SELECT *
FROM Product
WHERE Price >
(
SELECT AVG(Price)
FROM Product
);

SELECT *
FROM Product
WHERE Price <
(
SELECT AVG(Price)
FROM Product
);

SELECT c.CustomerId,
c.CustomerName,
SUM(oi.Quantity * oi.UnitPrice) AS TotalAmount
FROM Customer c
INNER JOIN Orders o
ON c.CustomerId = o.CustomerId
INNER JOIN OrderItem oi
ON o.OrderId = oi.OrderId
GROUP BY c.CustomerId,c.CustomerName
HAVING SUM(oi.Quantity * oi.UnitPrice) >
(
SELECT AVG(OrderAmount)
FROM
(
SELECT SUM(oi.Quantity * oi.UnitPrice) AS OrderAmount
FROM Orders o
INNER JOIN OrderItem oi
ON o.OrderId = oi.OrderId
GROUP BY o.OrderId
) AS TempTable
);



SELECT s.SellerId,
s.SellerName,
SUM(oi.Quantity * oi.UnitPrice) AS TotalSales
FROM Seller s
INNER JOIN Product p
ON s.SellerId = p.SellerId
INNER JOIN OrderItem oi
ON p.ProductId = oi.ProductId
GROUP BY s.SellerId,s.SellerName
HAVING SUM(oi.Quantity * oi.UnitPrice) > 50000;


SELECT p.ProductId,
p.ProductName,
SUM(oi.Quantity) AS TotalSoldQuantity
FROM Product p
INNER JOIN OrderItem oi
ON p.ProductId = oi.ProductId
GROUP BY p.ProductId,p.ProductName
HAVING SUM(oi.Quantity) >
(
SELECT AVG(ProductQuantity)
FROM
(
SELECT SUM(Quantity) AS ProductQuantity
FROM OrderItem
GROUP BY ProductId
) AS TempTable
);



SELECT TOP 1
c.CustomerId,
c.CustomerName,
SUM(oi.Quantity * oi.UnitPrice) AS TotalSpent
FROM Customer c
INNER JOIN Orders o
ON c.CustomerId = o.CustomerId
INNER JOIN OrderItem oi
ON o.OrderId = oi.OrderId
GROUP BY c.CustomerId,c.CustomerName
ORDER BY TotalSpent DESC;


SELECT TOP 1
s.SellerId,
s.SellerName,
SUM(oi.Quantity * oi.UnitPrice) AS TotalSales
FROM Seller s
INNER JOIN Product p
ON s.SellerId = p.SellerId
INNER JOIN OrderItem oi
ON p.ProductId = oi.ProductId
GROUP BY s.SellerId,s.SellerName
ORDER BY TotalSales DESC;

-- Correlated Subquery Questions

SELECT *
FROM Product p1
WHERE Price >
(
SELECT AVG(Price)
FROM Product p2
WHERE p1.Category = p2.Category
);


SELECT *
FROM Product p1
WHERE Price <
(
SELECT AVG(Price)
FROM Product p2
WHERE p1.Category = p2.Category
);

SELECT *
FROM Product p1
WHERE Price <
(
SELECT AVG(Price)
FROM Product p2
WHERE p1.Category = p2.Category
);


SELECT *
FROM Customer c
WHERE
(
SELECT COUNT(*)
FROM Orders o
WHERE c.CustomerId = o.CustomerId
) > 1;


SELECT o.OrderId,
SUM(oi.Quantity * oi.UnitPrice) AS OrderAmount
FROM Orders o
INNER JOIN OrderItem oi
ON o.OrderId = oi.OrderId
GROUP BY o.OrderId
HAVING SUM(oi.Quantity * oi.UnitPrice) >
(
SELECT AVG(OrderTotal)
FROM
(
SELECT SUM(Quantity * UnitPrice) AS OrderTotal
FROM OrderItem
GROUP BY OrderId
) AS TempTable
);

SELECT *
FROM Product p1
WHERE StockQuantity >
(
SELECT AVG(StockQuantity)
FROM Product p2
WHERE p1.Category = p2.Category
);

SELECT *
FROM Seller s
WHERE
(
SELECT AVG(Price)
FROM Product p
WHERE s.SellerId = p.SellerId
)
>
(
SELECT AVG(Price)
FROM Product
);


--EXISTS / NOT R+EXISTS Questions

SELECT *
FROM Customer c
WHERE EXISTS
(
SELECT *
FROM Orders o
WHERE c.CustomerId = o.CustomerId
);

SELECT *
FROM Customer c
WHERE NOT EXISTS
(
SELECT *
FROM Orders o
WHERE c.CustomerId = o.CustomerId
);

SELECT *
FROM Customer c
WHERE NOT EXISTS
(
SELECT *
FROM Orders o
WHERE c.CustomerId = o.CustomerId
);

SELECT *
FROM Product p
WHERE NOT EXISTS
(
SELECT *
FROM OrderItem oi
WHERE p.ProductId = oi.ProductId
);


SELECT *
FROM Seller s
WHERE EXISTS
(
SELECT *
FROM Product p
WHERE s.SellerId = p.SellerId
);


SELECT *
FROM Seller s
WHERE NOT EXISTS
(
SELECT *
FROM Product p
WHERE s.SellerId = p.SellerId
);


SELECT *
FROM Customer c
WHERE EXISTS
(
SELECT *
FROM Orders o
INNER JOIN OrderItem oi
ON o.OrderId = oi.OrderId
INNER JOIN Product p
ON oi.ProductId = p.ProductId
WHERE c.CustomerId = o.CustomerId
AND p.Category = 'Mobile'
);


SELECT *
FROM Customer c
WHERE NOT EXISTS
(
SELECT *
FROM Orders o
INNER JOIN OrderItem oi
ON o.OrderId = oi.OrderId
INNER JOIN Product p
ON oi.ProductId = p.ProductId
WHERE c.CustomerId = o.CustomerId
AND p.Category = 'Laptop'
);


-- Stored Procedure Assignment Questions

CREATE PROC sp_GetAllCustomers
AS
BEGIN
SELECT * FROM Customer;
END;
EXEC sp_GetAllCustomers;

CREATE PROC sp_GetAllProducts
AS
BEGIN
SELECT * FROM Product;
END;
EXEC sp_GetAllProducts;

CREATE PROC sp_GetAllSellers
AS
BEGIN
SELECT * FROM Seller;
END;
EXEC sp_GetAllSellers;

CREATE PROC sp_GetAllOrders
AS
BEGIN
SELECT * FROM Orders;
END;
EXEC sp_GetAllOrders;


CREATE PROC sp_GetAllOrderItems
AS
BEGIN
SELECT * FROM OrderItem;
END;
EXEC sp_GetAllOrderItems;


-- stored Procedure with input Parameters

CREATE PROC sp_GetCustomerById
@CustomerId INT
AS
BEGIN
SELECT *
FROM Customer
WHERE CustomerId = @CustomerId;
END;
EXEC sp_GetCustomerById 1;

-- Stored Procedure with Input Parameter

CREATE PROC sp_GetCustomerById
@CustomerId INT
AS
BEGIN
SELECT * FROM Customer WHERE CustomerId=@CustomerId; 
END;


CREATE PROC sp_GetProductById
@ProductId INT
AS
BEGIN
SELECT * FROM Product WHERE ProductId=@ProductId;
END;

CREATE PROC sp_GetSellerById
@SellerId INT
AS
BEGIN
SELECT * FROM Seller WHERE SellerId=@SellerId;
END;

CREATE PROC sp_GetOrderById
@OrderId INT
AS
BEGIN
SELECT * FROM Orders WHERE OrderId=@OrderId;
END;

CREATE PROC sp_GetCustomersByCity
@City VARCHAR(100)
AS
BEGIN
SELECT * FROM Customer WHERE City=@City; 
END;

CREATE PROC sp_GetProductsByCategory 
@Category VARCHAR(100)
AS
BEGIN
SELECT * FROM Product WHERE Category=@Category;
END;

CREATE PROC sp_GetProductsBySellerId
@SellerId INT
AS
BEGIN
SELECT * FROM Product WHERE SellerId=@SellerId;
END;

CREATE PROC sp_GetOrdersByCustomerId
@CustomerId INT
AS
BEGIN
SELECT * FROM Orders WHERE CustomerId=@CustomerId;
END;

CREATE PROC sp_GetOrderItemsByOrderId
@OrderId INT
AS
BEGIN
SELECT * FROM OrderItem WHERE OrderId=@OrderId; 
END;

CREATE PROC sp_GetProductsGreaterThanPrice
@Price MONEY
AS 
BEGIN
SELECT * FROM Product WHERE Price>@Price;
END;


--Insert Stored Procedure Questions

CREATE PROC sp_InsertCustomer
@CustomerName VARCHAR(100),
@Email VARCHAR(100),
@MobileNo BIGINT,
@City VARCHAR(100),
@Address VARCHAR(200),
@IsActive BIT
AS
BEGIN
INSERT INTO Customer
(CustomerName,Email,MobileNo,City,Address,IsActive)
VALUES
(@CustomerName,@Email,@MobileNo,@City,@Address,@IsActive);
END;

CREATE PROC sp_InsertSeller
@SellerName VARCHAR(100),
@Email VARCHAR(100),
@MobileNo BIGINT,
@City VARCHAR(100),
@Rating DECIMAL(2,1),
@IsActive BIT
AS
BEGIN
INSERT INTO Seller
(SellerName,Email,MobileNo,City,Rating,IsActive)
VALUES
(@SellerName,@Email,@MobileNo,@City,@Rating,@IsActive);
END;


CREATE PROC sp_InsertProduct
@ProductName VARCHAR(100),
@Category VARCHAR(100),
@Price MONEY,
@StockQuantity INT,
@SellerId INT
AS
BEGIN
INSERT INTO Product
(ProductName,Category,Price,StockQuantity,SellerId)
VALUES
(@ProductName,@Category,@Price,@StockQuantity,@SellerId);
END;


CREATE PROC sp_InsertOrder
@CustomerId INT,
@OrderStatus VARCHAR(50),
@PaymentMode VARCHAR(50),
@DeliveryCity VARCHAR(100)
AS
BEGIN
INSERT INTO Orders
(CustomerId,OrderStatus,PaymentMode,DeliveryCity)
VALUES
(@CustomerId,@OrderStatus,@PaymentMode,@DeliveryCity);
END;

CREATE PROC sp_InsertOrderItem
@OrderId INT,
@ProductId INT,
@Quantity INT,
@UnitPrice MONEY
AS
BEGIN
INSERT INTO OrderItem
(OrderId,ProductId,Quantity,UnitPrice)
VALUES
(@OrderId,@ProductId,@Quantity,@UnitPrice);
END;


--Update Stored Procedure Questions

CREATE PROC sp_UpdateCustomerCity
@CustomerId INT,
@City VARCHAR(100)
AS
BEGIN
UPDATE Customer
SET City=@City
WHERE CustomerId=@CustomerId;
END;
EXEC sp_UpdateCustomerCity 1,'Mumbai';


CREATE PROC sp_UpdateCustomerMobile
@CustomerId INT,
@MobileNo BIGINT
AS
BEGIN
UPDATE Customer
SET MobileNo=@MobileNo
WHERE CustomerId=@CustomerId;
END;
EXEC sp_UpdateCustomerMobile 1,9876500000;

CREATE PROC sp_UpdateProductPrice
@ProductId INT,
@Price MONEY
AS
BEGIN
UPDATE Product
SET Price=@Price
WHERE ProductId=@ProductId;
END;
EXEC sp_UpdateProductPrice 1,85000;

CREATE PROC sp_UpdateProductStock
@ProductId INT,
@StockQuantity INT
AS
BEGIN
UPDATE Product
SET StockQuantity=@StockQuantity
WHERE ProductId=@ProductId;
END;
EXEC sp_UpdateProductStock 1,20;


CREATE PROC sp_UpdateOrderStatus
@OrderId INT,
@OrderStatus VARCHAR(50)
AS
BEGIN
UPDATE Orders
SET OrderStatus=@OrderStatus
WHERE OrderId=@OrderId;
END;
EXEC sp_UpdateOrderStatus 1,'Delivered';

-- 21. Create a stored procedure to update customer city based on customer id

CREATE PROC sp_UpdateCustomerCity
@CustomerId INT,
@City VARCHAR(100)
AS
BEGIN

UPDATE Customer
SET City=@City
WHERE CustomerId=@CustomerId;

END;

EXEC sp_UpdateCustomerCity 1,'Mumbai';



-- 22. Create a stored procedure to update customer mobile number based on customer id

CREATE PROC sp_UpdateCustomerMobile
@CustomerId INT,
@MobileNo BIGINT
AS
BEGIN

UPDATE Customer
SET MobileNo=@MobileNo
WHERE CustomerId=@CustomerId;

END;

EXEC sp_UpdateCustomerMobile 1,9876500000;



-- 23. Create a stored procedure to update product price based on product id

CREATE PROC sp_UpdateProductPrice
@ProductId INT,
@Price MONEY
AS
BEGIN

UPDATE Product
SET Price=@Price
WHERE ProductId=@ProductId;

END;

EXEC sp_UpdateProductPrice 1,85000;



-- 24. Create a stored procedure to update product stock quantity based on product id

CREATE PROC sp_UpdateProductStock
@ProductId INT,
@StockQuantity INT
AS
BEGIN

UPDATE Product
SET StockQuantity=@StockQuantity
WHERE ProductId=@ProductId;

END;

EXEC sp_UpdateProductStock 1,20;



-- 25. Create a stored procedure to update order status based on order id

CREATE PROC sp_UpdateOrderStatus
@OrderId INT,
@OrderStatus VARCHAR(50)
AS
BEGIN

UPDATE Orders
SET OrderStatus=@OrderStatus
WHERE OrderId=@OrderId;

END;

EXEC sp_UpdateOrderStatus 1,'Delivered';


CREATE PROC sp_UpdateSellerRating
@SellerId INT,
@Rating DECIMAL(2,1)
AS
BEGIN
UPDATE Seller
SET Rating=@Rating
WHERE SellerId=@SellerId;
END;
EXEC sp_UpdateSellerRating 1,4.8;


-- 21. Create a stored procedure to update customer city based on customer id

CREATE PROC sp_UpdateCustomerCity
@CustomerId INT,
@City VARCHAR(100)
AS
BEGIN

UPDATE Customer
SET City=@City
WHERE CustomerId=@CustomerId;

END;

EXEC sp_UpdateCustomerCity 1,'Mumbai';



-- 22. Create a stored procedure to update customer mobile number based on customer id

CREATE PROC sp_UpdateCustomerMobile
@CustomerId INT,
@MobileNo BIGINT
AS
BEGIN

UPDATE Customer
SET MobileNo=@MobileNo
WHERE CustomerId=@CustomerId;

END;

EXEC sp_UpdateCustomerMobile 1,9876500000;



-- 23. Create a stored procedure to update product price based on product id

CREATE PROC sp_UpdateProductPrice
@ProductId INT,
@Price MONEY
AS
BEGIN

UPDATE Product
SET Price=@Price
WHERE ProductId=@ProductId;

END;

EXEC sp_UpdateProductPrice 1,85000;



-- 24. Create a stored procedure to update product stock quantity based on product id

CREATE PROC sp_UpdateProductStock
@ProductId INT,
@StockQuantity INT
AS
BEGIN

UPDATE Product
SET StockQuantity=@StockQuantity
WHERE ProductId=@ProductId;

END;

EXEC sp_UpdateProductStock 1,20;



-- 25. Create a stored procedure to update order status based on order id

CREATE PROC sp_UpdateOrderStatus
@OrderId INT,
@OrderStatus VARCHAR(50)
AS
BEGIN

UPDATE Orders
SET OrderStatus=@OrderStatus
WHERE OrderId=@OrderId;

END;

EXEC sp_UpdateOrderStatus 1,'Delivered';


CREATE PROC sp_UpdateSellerRating
@SellerId INT,
@Rating DECIMAL(2,1)
AS
BEGIN
UPDATE Seller
SET Rating=@Rating
WHERE SellerId=@SellerId;
END;
EXEC sp_UpdateSellerRating 1,4.8;

-- 21. Create a stored procedure to update customer city based on customer id

CREATE PROC sp_UpdateCustomerCity
@CustomerId INT,
@City VARCHAR(100)
AS
BEGIN

UPDATE Customer
SET City=@City
WHERE CustomerId=@CustomerId;

END;

EXEC sp_UpdateCustomerCity 1,'Mumbai';



-- 22. Create a stored procedure to update customer mobile number based on customer id

CREATE PROC sp_UpdateCustomerMobile
@CustomerId INT,
@MobileNo BIGINT
AS
BEGIN

UPDATE Customer
SET MobileNo=@MobileNo
WHERE CustomerId=@CustomerId;

END;

EXEC sp_UpdateCustomerMobile 1,9876500000;



-- 23. Create a stored procedure to update product price based on product id

CREATE PROC sp_UpdateProductPrice
@ProductId INT,
@Price MONEY
AS
BEGIN

UPDATE Product
SET Price=@Price
WHERE ProductId=@ProductId;

END;

EXEC sp_UpdateProductPrice 1,85000;


CREATE PROC sp_UpdateProductStock
@ProductId INT,
@StockQuantity INT
AS
BEGIN
UPDATE Product
SET StockQuantity=@StockQuantity
WHERE ProductId=@ProductId;
END;
EXEC sp_UpdateProductStock 1,20;


CREATE PROC sp_UpdateOrderStatus
@OrderId INT,
@OrderStatus VARCHAR(50)
AS
BEGIN
UPDATE Orders
SET OrderStatus=@OrderStatus
WHERE OrderId=@OrderId;
END;
EXEC sp_UpdateOrderStatus 1,'Delivered';


CREATE PROC sp_UpdateSellerRating
@SellerId INT,
@Rating DECIMAL(2,1)
AS
BEGIN
UPDATE Seller
SET Rating=@Rating
WHERE SellerId=@SellerId;
END;
EXEC sp_UpdateSellerRating 1,4.8;

CREATE PROC sp_UpdateCustomerStatus
@CustomerId INT,
@IsActive BIT
AS
BEGIN
UPDATE Customer
SET IsActive=@IsActive
WHERE CustomerId=@CustomerId;
END;
EXEC sp_UpdateCustomerStatus 1,0;

CREATE PROC sp_UpdateSellerStatus
@SellerId INT,
@IsActive BIT
AS
BEGIN
UPDATE Seller
SET IsActive=@IsActive
WHERE SellerId=@SellerId;
END;
EXEC sp_UpdateSellerStatus 1,0;

--Delete stored Procedure 

CREATE PROC sp_DeleteCustomer
@CustomerId INT
AS
BEGIN
DELETE FROM Customer
WHERE CustomerId=@CustomerId;
END;
EXEC sp_DeleteCustomer 1;



CREATE PROC sp_DeleteSeller
@SellerId INT
AS
BEGIN
DELETE FROM Seller
WHERE SellerId=@SellerId;
END;
EXEC sp_DeleteSeller 1;

CREATE PROC sp_DeleteProduct
@ProductId INT
AS
BEGIN
DELETE FROM Product
WHERE ProductId=@ProductId;
END;
EXEC sp_DeleteProduct 1;

CREATE PROC sp_DeleteOrder
@OrderId INT
AS
BEGIN
DELETE FROM Orders
WHERE OrderId=@OrderId;
END;
EXEC sp_DeleteOrder 1;

CREATE PROC sp_DeleteOrderItem
@OrderItemId INT
AS
BEGIN
DELETE FROM OrderItem
WHERE OrderItemId=@OrderItemId;
END;
EXEC sp_DeleteOrderItem 1;

--Stored Procedure

CREATE PROC sp_CustomerWiseOrderDetails
AS
BEGIN
SELECT
c.CustomerName,
o.OrderId,
o.OrderDate,
o.OrderStatus,
o.PaymentMode
FROM Customer c
INNER JOIN Orders o
ON c.CustomerId=o.CustomerId;
END;
EXEC sp_CustomerWiseOrderDetails;


CREATE PROC sp_SellerWiseProductDetails
AS
BEGIN
SELECT
s.SellerName,
p.ProductName,
p.Category,
p.Price,
p.StockQuantity
FROM Seller s
INNER JOIN Product p
ON s.SellerId=p.SellerId;
END;
EXEC sp_SellerWiseProductDetails;


CREATE PROC sp_OrderWiseProductDetails
AS
BEGIN
SELECT
o.OrderId,
p.ProductName,
oi.Quantity,
oi.UnitPrice
FROM Orders o
INNER JOIN OrderItem oi
ON o.OrderId=oi.OrderId
INNER JOIN Product p
ON oi.ProductId=p.ProductId;
END;
EXEC sp_OrderWiseProductDetails;


CREATE PROC sp_CompleteOrderReport
AS
BEGIN

SELECT
c.CustomerName,
p.ProductName,
s.SellerName,
oi.Quantity,
oi.UnitPrice,
(oi.Quantity*oi.UnitPrice) AS TotalAmount
FROM Customer c
INNER JOIN Orders o
ON c.CustomerId=o.CustomerId
INNER JOIN OrderItem oi
ON o.OrderId=oi.OrderId
INNER JOIN Product p
ON oi.ProductId=p.ProductId
INNER JOIN Seller s
ON p.SellerId=s.SellerId;
END;
EXEC sp_CompleteOrderReport;


CREATE PROC sp_CustomerWiseTotalOrderAmount
AS
BEGIN
SELECT
c.CustomerName,
SUM(oi.Quantity*oi.UnitPrice) AS TotalOrderAmount
FROM Customer c
INNER JOIN Orders o
ON c.CustomerId=o.CustomerId
INNER JOIN OrderItem oi
ON o.OrderId=oi.OrderId
GROUP BY c.CustomerName;
END;
EXEC sp_CustomerWiseTotalOrderAmount;


CREATE PROC sp_SellerWiseTotalSales
AS
BEGIN
SELECT
s.SellerName,
SUM(oi.Quantity*oi.UnitPrice) AS TotalSalesAmount
FROM Seller s
INNER JOIN Product p
ON s.SellerId=p.SellerId
INNER JOIN OrderItem oi
ON p.ProductId=oi.ProductId
GROUP BY s.SellerName;
END;
EXEC sp_SellerWiseTotalSales;

CREATE PROC sp_ProductWiseSalesQuantity
AS
BEGIN

SELECT
p.ProductName,
SUM(oi.Quantity) AS TotalSalesQuantity
FROM Product p
INNER JOIN OrderItem oi
ON p.ProductId=oi.ProductId
GROUP BY p.ProductName;
END;
EXEC sp_ProductWiseSalesQuantity;

--Stored Procedure with o/p

CREATE PROC sp_TotalCustomers
@TotalCustomers INT OUTPUT
AS
BEGIN
SELECT @TotalCustomers=COUNT(*)
FROM Customer;
END;
DECLARE @Count INT;
EXEC sp_TotalCustomers @Count OUTPUT;
SELECT @Count AS TotalCustomers;


CREATE PROC sp_TotalProducts
@TotalProducts INT OUTPUT
AS
BEGIN
SELECT @TotalProducts=COUNT(*)
FROM Product;
END;
DECLARE @ProductCount INT;
EXEC sp_TotalProducts @ProductCount OUTPUT;
SELECT @ProductCount AS TotalProducts;

CREATE PROC sp_TotalOrders
@TotalOrders INT OUTPUT
AS
BEGIN
SELECT @TotalOrders=COUNT(*)
FROM Orders;
END;
DECLARE @OrderCount INT;
EXEC sp_TotalOrders @OrderCount OUTPUT;
SELECT @OrderCount AS TotalOrders;

CREATE PROC sp_ProductTotalSales
@ProductId INT,
@TotalSales MONEY OUTPUT
AS
BEGIN
SELECT @TotalSales=SUM(Quantity*UnitPrice)
FROM OrderItem
WHERE ProductId=@ProductId;
END;
DECLARE @Sales MONEY;
EXEC sp_ProductTotalSales 1,@Sales OUTPUT;
SELECT @Sales AS TotalSalesAmount;

CREATE PROC sp_CustomerTotalPurchase
@CustomerId INT,
@TotalPurchase MONEY OUTPUT
AS
BEGIN
SELECT @TotalPurchase=SUM(oi.Quantity*oi.UnitPrice)
FROM Orders o
INNER JOIN OrderItem oi
ON o.OrderId=oi.OrderId
WHERE o.CustomerId=@CustomerId;
END;
DECLARE @Purchase MONEY;
EXEC sp_CustomerTotalPurchase 1,@Purchase OUTPUT;
SELECT @Purchase AS TotalPurchaseAmount;
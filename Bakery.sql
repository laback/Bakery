create database Bakery
go

use Bakery

CREATE TABLE [Products] (
  [productId] int PRIMARY KEY IDENTITY(1, 1),
  [productName] nvarchar(100)
)
GO

CREATE TABLE [Raws] (
  [rawId] int PRIMARY KEY IDENTITY(1, 1),
  [rawName] nvarchar(100)
)
GO

CREATE TABLE [Materials] (
  [materialId] int PRIMARY KEY IDENTITY(1, 1),
  [materialName] nvarchar(100)
)
GO

CREATE TABLE [Norms] (
  [normId] int PRIMARY KEY IDENTITY(1, 1),
  [rowId] int,
  [productId] int,
  [quantity] float
)
GO

CREATE TABLE [ProductsMaterials] (
  [productMaterial] int PRIMARY KEY IDENTITY(1, 1),
  [materialId] int,
  [productId] int,
  [quantity] float
)
GO

CREATE TABLE [ProductsPlans] (
  [productPlan] int PRIMARY KEY IDENTITY(1, 1),
  [dayPlanId] int,
  [productId] int,
  [count] int
)
GO

CREATE TABLE [DayPlans] (
  [dayPlanId] int PRIMARY KEY IDENTITY(1, 1),
  [date] date
)
GO

CREATE TABLE [DayProductions] (
  [dayProductionId] int PRIMARY KEY IDENTITY(1, 1),
  [date] date
)
GO

CREATE TABLE [ProductsProductions] (
  [productProductionId] int PRIMARY KEY IDENTITY(1, 1),
  [dayProductionId] int,
  [productId] int,
  [count] int
)
GO

ALTER TABLE [Norms] ADD FOREIGN KEY ([rowId]) REFERENCES [Raws] ([rawId])
GO

ALTER TABLE [Norms] ADD FOREIGN KEY ([productId]) REFERENCES [Products] ([productId])
GO

ALTER TABLE [ProductsMaterials] ADD FOREIGN KEY ([materialId]) REFERENCES [Materials] ([materialId])
GO

ALTER TABLE [ProductsMaterials] ADD FOREIGN KEY ([productId]) REFERENCES [Products] ([productId])
GO

ALTER TABLE [ProductsPlans] ADD FOREIGN KEY ([dayPlanId]) REFERENCES [DayPlans] ([dayPlanId])
GO

ALTER TABLE [ProductsPlans] ADD FOREIGN KEY ([productId]) REFERENCES [Products] ([productId])
GO

ALTER TABLE [ProductsProductions] ADD FOREIGN KEY ([dayProductionId]) REFERENCES [DayProductions] ([dayProductionId])
GO

ALTER TABLE [ProductsProductions] ADD FOREIGN KEY ([productId]) REFERENCES [Products] ([productId])
GO

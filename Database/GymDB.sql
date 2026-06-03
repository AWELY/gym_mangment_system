/* ============================================================================
   Glory Gym Management System  -  MSSQL Server database setup script
   ----------------------------------------------------------------------------
   Run this script ONCE in SQL Server Management Studio (SSMS) before launching
   the application. It creates the GymDB database, all tables, indexes, views,
   CRUD + backup stored procedures, and seeds the default data.

   Design rules:
     * NO primary key uses IDENTITY / auto-increment. All keys are supplied by
       the application / persistence layer.
     * Fully normalized: no column stores multiple values. Plan features,
       diet-send history and store-sale line items each live in their own table.
   ============================================================================ */

/* ---------- 1. Database ---------------------------------------------------- */
IF DB_ID(N'GymDB') IS NULL
BEGIN
    CREATE DATABASE [GymDB];
END
GO

USE [GymDB];
GO

/* ---------- 2. Tables ------------------------------------------------------ */

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        UserId   INT            NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
        FullName NVARCHAR(150)  NULL,
        Username NVARCHAR(100)  NOT NULL,
        [Password] NVARCHAR(200) NULL,
        [Role]   INT            NOT NULL CONSTRAINT DF_Users_Role DEFAULT(1)  -- 0 = Admin, 1 = Receptionist
    );
END
GO

IF OBJECT_ID(N'dbo.SubscriptionPlans', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SubscriptionPlans
    (
        PlanId        INT           NOT NULL CONSTRAINT PK_SubscriptionPlans PRIMARY KEY,
        [Name]        NVARCHAR(150) NULL,
        Price         DECIMAL(18,2) NOT NULL CONSTRAINT DF_Plans_Price DEFAULT(0),
        DurationValue INT           NOT NULL CONSTRAINT DF_Plans_Dur   DEFAULT(1),
        DurationUnit  NVARCHAR(50)  NULL
    );
END
GO

IF OBJECT_ID(N'dbo.SubscriptionPlanFeatures', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SubscriptionPlanFeatures
    (
        PlanId    INT           NOT NULL,
        FeatureNo INT           NOT NULL,
        Feature   NVARCHAR(200) NOT NULL,
        CONSTRAINT PK_PlanFeatures PRIMARY KEY (PlanId, FeatureNo),
        CONSTRAINT FK_PlanFeatures_Plan FOREIGN KEY (PlanId)
            REFERENCES dbo.SubscriptionPlans (PlanId) ON DELETE CASCADE
    );
END
GO

IF OBJECT_ID(N'dbo.Members', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Members
    (
        MemberId     INT           NOT NULL CONSTRAINT PK_Members PRIMARY KEY,
        FullName     NVARCHAR(150) NULL,
        Phone        NVARCHAR(40)  NULL,
        Gender       NVARCHAR(20)  NULL,
        PlanId       INT           NULL,                 -- FK -> SubscriptionPlans(PlanId)
        PlanName     NVARCHAR(150) NULL,                 -- cached plan label for display
        PriceText    NVARCHAR(60)  NULL,
        DurationText NVARCHAR(60)  NULL,
        JoinDate     DATE          NULL,
        CONSTRAINT FK_Members_Plan FOREIGN KEY (PlanId)
            REFERENCES dbo.SubscriptionPlans (PlanId) ON DELETE SET NULL
    );
END
GO

/* Upgrade existing databases: add the Members.PlanId column + FK if missing. */
IF COL_LENGTH(N'dbo.Members', N'PlanId') IS NULL
    ALTER TABLE dbo.Members ADD PlanId INT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Members_Plan')
BEGIN
    -- Backfill PlanId from the matching plan name before enforcing the FK.
    UPDATE m
    SET    m.PlanId = p.PlanId
    FROM   dbo.Members m
    JOIN   dbo.SubscriptionPlans p ON p.[Name] = m.PlanName
    WHERE  m.PlanId IS NULL;

    ALTER TABLE dbo.Members WITH CHECK ADD CONSTRAINT FK_Members_Plan
        FOREIGN KEY (PlanId) REFERENCES dbo.SubscriptionPlans (PlanId) ON DELETE SET NULL;
END
GO

IF OBJECT_ID(N'dbo.Trainers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Trainers
    (
        TrainerId INT           NOT NULL CONSTRAINT PK_Trainers PRIMARY KEY,
        [Name]    NVARCHAR(150) NULL,
        Phone     NVARCHAR(40)  NULL,
        Specialty NVARCHAR(120) NULL,
        Salary    DECIMAL(18,2) NOT NULL CONSTRAINT DF_Trainers_Salary DEFAULT(0),
        JoinDate  DATE          NULL
    );
END
GO

IF OBJECT_ID(N'dbo.StoreProducts', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.StoreProducts
    (
        ProductId   INT            NOT NULL CONSTRAINT PK_StoreProducts PRIMARY KEY,
        [Name]      NVARCHAR(150)  NULL,
        Price       DECIMAL(18,2)  NOT NULL CONSTRAINT DF_Products_Price DEFAULT(0),
        Category    NVARCHAR(80)   NULL,
        Emoji       NVARCHAR(20)   NULL,
        StockQty    INT            NOT NULL CONSTRAINT DF_Products_Stock DEFAULT(0),
        Expiry      DATE           NULL,
        PhotoBase64 NVARCHAR(MAX)  NULL
    );
END
GO

IF OBJECT_ID(N'dbo.StoreSales', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.StoreSales
    (
        SaleId  INT           NOT NULL CONSTRAINT PK_StoreSales PRIMARY KEY,
        SoldAt  NVARCHAR(50)  NULL,                 -- ISO round-trip ("o") string
        Total   DECIMAL(18,2) NOT NULL CONSTRAINT DF_Sales_Total DEFAULT(0),
        Summary NVARCHAR(MAX) NULL                  -- human-readable receipt note
    );
END
GO

IF OBJECT_ID(N'dbo.StoreSaleItems', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.StoreSaleItems
    (
        SaleId      INT           NOT NULL,
        [LineNo]    INT           NOT NULL,
        ProductId   INT           NULL,                 -- FK -> StoreProducts(ProductId)
        ProductName NVARCHAR(200) NULL,                 -- cached product label for receipt history
        Price       DECIMAL(18,2) NOT NULL CONSTRAINT DF_SaleItems_Price DEFAULT(0),
        Qty         INT           NOT NULL CONSTRAINT DF_SaleItems_Qty   DEFAULT(0),
        CONSTRAINT PK_StoreSaleItems PRIMARY KEY (SaleId, [LineNo]),
        CONSTRAINT FK_SaleItems_Sale FOREIGN KEY (SaleId)
            REFERENCES dbo.StoreSales (SaleId) ON DELETE CASCADE,
        CONSTRAINT FK_SaleItems_Product FOREIGN KEY (ProductId)
            REFERENCES dbo.StoreProducts (ProductId) ON DELETE SET NULL
    );
END
GO

/* Upgrade existing databases: add the StoreSaleItems.ProductId column + FK if missing. */
IF COL_LENGTH(N'dbo.StoreSaleItems', N'ProductId') IS NULL
    ALTER TABLE dbo.StoreSaleItems ADD ProductId INT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_SaleItems_Product')
BEGIN
    -- Backfill ProductId from the matching product name before enforcing the FK.
    UPDATE i
    SET    i.ProductId = p.ProductId
    FROM   dbo.StoreSaleItems i
    JOIN   dbo.StoreProducts p ON p.[Name] = i.ProductName
    WHERE  i.ProductId IS NULL;

    ALTER TABLE dbo.StoreSaleItems WITH CHECK ADD CONSTRAINT FK_SaleItems_Product
        FOREIGN KEY (ProductId) REFERENCES dbo.StoreProducts (ProductId) ON DELETE SET NULL;
END
GO

IF OBJECT_ID(N'dbo.FeedingPlans', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.FeedingPlans
    (
        FeedingPlanId INT           NOT NULL CONSTRAINT PK_FeedingPlans PRIMARY KEY,
        [Name]        NVARCHAR(150) NULL,
        PdfPath       NVARCHAR(500) NULL
    );
END
GO

IF OBJECT_ID(N'dbo.DietSendHistory', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DietSendHistory
    (
        HistoryId INT           NOT NULL CONSTRAINT PK_DietSendHistory PRIMARY KEY,
        Entry     NVARCHAR(MAX) NULL,
        CreatedAt DATETIME      NOT NULL CONSTRAINT DF_DietHistory_Created DEFAULT(GETDATE())
    );
END
GO

/* ---------- 3. Indexes (the "good stuff") ---------------------------------- */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Users_Username' AND object_id = OBJECT_ID(N'dbo.Users'))
    CREATE UNIQUE INDEX UX_Users_Username ON dbo.Users(Username);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Members_JoinDate' AND object_id = OBJECT_ID(N'dbo.Members'))
    CREATE INDEX IX_Members_JoinDate ON dbo.Members(JoinDate);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Members_PlanName' AND object_id = OBJECT_ID(N'dbo.Members'))
    CREATE INDEX IX_Members_PlanName ON dbo.Members(PlanName);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_StoreProducts_Category' AND object_id = OBJECT_ID(N'dbo.StoreProducts'))
    CREATE INDEX IX_StoreProducts_Category ON dbo.StoreProducts(Category);
GO

/* ---------- 4. Views ------------------------------------------------------- */
IF OBJECT_ID(N'dbo.vw_MembersWithPlan', N'V') IS NOT NULL DROP VIEW dbo.vw_MembersWithPlan;
GO
CREATE VIEW dbo.vw_MembersWithPlan
AS
    SELECT  m.MemberId,
            m.FullName,
            m.Phone,
            m.Gender,
            m.PlanName,
            m.PriceText,
            m.DurationText,
            m.JoinDate,
            p.Price          AS PlanPrice,
            p.DurationValue,
            p.DurationUnit
    FROM    dbo.Members m
    LEFT JOIN dbo.SubscriptionPlans p ON p.PlanId = m.PlanId;
GO

IF OBJECT_ID(N'dbo.vw_MonthlyStoreRevenue', N'V') IS NOT NULL DROP VIEW dbo.vw_MonthlyStoreRevenue;
GO
CREATE VIEW dbo.vw_MonthlyStoreRevenue
AS
    SELECT  YEAR(TRY_CONVERT(datetimeoffset, SoldAt))  AS [Year],
            MONTH(TRY_CONVERT(datetimeoffset, SoldAt)) AS [Month],
            SUM(Total)                                 AS Revenue,
            COUNT(*)                                   AS SalesCount
    FROM    dbo.StoreSales
    WHERE   TRY_CONVERT(datetimeoffset, SoldAt) IS NOT NULL
    GROUP BY YEAR(TRY_CONVERT(datetimeoffset, SoldAt)),
             MONTH(TRY_CONVERT(datetimeoffset, SoldAt));
GO

/* ============================================================================
   5. Stored procedures  (CREATE / UPDATE / DELETE / SELECT per entity)
   ============================================================================ */

/* ----- Users -------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.usp_Users_SelectAll', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Users_SelectAll;
GO
CREATE PROCEDURE dbo.usp_Users_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT UserId, FullName, Username, [Password], [Role]
    FROM   dbo.Users
    ORDER BY UserId;
END
GO

IF OBJECT_ID(N'dbo.usp_Users_Insert', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Users_Insert;
GO
CREATE PROCEDURE dbo.usp_Users_Insert
    @UserId INT, @FullName NVARCHAR(150), @Username NVARCHAR(100),
    @Password NVARCHAR(200), @Role INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Users (UserId, FullName, Username, [Password], [Role])
    VALUES (@UserId, @FullName, @Username, @Password, @Role);
END
GO

IF OBJECT_ID(N'dbo.usp_Users_Update', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Users_Update;
GO
CREATE PROCEDURE dbo.usp_Users_Update
    @UserId INT, @FullName NVARCHAR(150), @Username NVARCHAR(100),
    @Password NVARCHAR(200), @Role INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Users
    SET FullName = @FullName, Username = @Username, [Password] = @Password, [Role] = @Role
    WHERE UserId = @UserId;
END
GO

IF OBJECT_ID(N'dbo.usp_Users_Delete', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Users_Delete;
GO
CREATE PROCEDURE dbo.usp_Users_Delete @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Users WHERE UserId = @UserId;
END
GO

/* ----- SubscriptionPlans + Features --------------------------------------- */
IF OBJECT_ID(N'dbo.usp_SubscriptionPlans_SelectAll', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_SubscriptionPlans_SelectAll;
GO
CREATE PROCEDURE dbo.usp_SubscriptionPlans_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PlanId, [Name], Price, DurationValue, DurationUnit
    FROM   dbo.SubscriptionPlans
    ORDER BY PlanId;
END
GO

IF OBJECT_ID(N'dbo.usp_SubscriptionPlans_Insert', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_SubscriptionPlans_Insert;
GO
CREATE PROCEDURE dbo.usp_SubscriptionPlans_Insert
    @PlanId INT, @Name NVARCHAR(150), @Price DECIMAL(18,2),
    @DurationValue INT, @DurationUnit NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.SubscriptionPlans (PlanId, [Name], Price, DurationValue, DurationUnit)
    VALUES (@PlanId, @Name, @Price, @DurationValue, @DurationUnit);
END
GO

IF OBJECT_ID(N'dbo.usp_SubscriptionPlans_Update', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_SubscriptionPlans_Update;
GO
CREATE PROCEDURE dbo.usp_SubscriptionPlans_Update
    @PlanId INT, @Name NVARCHAR(150), @Price DECIMAL(18,2),
    @DurationValue INT, @DurationUnit NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.SubscriptionPlans
    SET [Name] = @Name, Price = @Price, DurationValue = @DurationValue, DurationUnit = @DurationUnit
    WHERE PlanId = @PlanId;
END
GO

IF OBJECT_ID(N'dbo.usp_SubscriptionPlans_Delete', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_SubscriptionPlans_Delete;
GO
CREATE PROCEDURE dbo.usp_SubscriptionPlans_Delete @PlanId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.SubscriptionPlans WHERE PlanId = @PlanId;  -- features cascade
END
GO

IF OBJECT_ID(N'dbo.usp_PlanFeatures_SelectAll', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_PlanFeatures_SelectAll;
GO
CREATE PROCEDURE dbo.usp_PlanFeatures_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PlanId, FeatureNo, Feature
    FROM   dbo.SubscriptionPlanFeatures
    ORDER BY PlanId, FeatureNo;
END
GO

IF OBJECT_ID(N'dbo.usp_PlanFeatures_Insert', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_PlanFeatures_Insert;
GO
CREATE PROCEDURE dbo.usp_PlanFeatures_Insert
    @PlanId INT, @FeatureNo INT, @Feature NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.SubscriptionPlanFeatures (PlanId, FeatureNo, Feature)
    VALUES (@PlanId, @FeatureNo, @Feature);
END
GO

/* ----- Members ------------------------------------------------------------ */
IF OBJECT_ID(N'dbo.usp_Members_SelectAll', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Members_SelectAll;
GO
CREATE PROCEDURE dbo.usp_Members_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MemberId, FullName, Phone, Gender, PlanName, PriceText, DurationText,
           CONVERT(VARCHAR(10), JoinDate, 23) AS JoinDate, PlanId
    FROM   dbo.Members
    ORDER BY MemberId;
END
GO

IF OBJECT_ID(N'dbo.usp_Members_Insert', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Members_Insert;
GO
CREATE PROCEDURE dbo.usp_Members_Insert
    @MemberId INT, @FullName NVARCHAR(150), @Phone NVARCHAR(40), @Gender NVARCHAR(20),
    @PlanName NVARCHAR(150), @PriceText NVARCHAR(60), @DurationText NVARCHAR(60), @JoinDate DATE,
    @PlanId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Members (MemberId, FullName, Phone, Gender, PlanId, PlanName, PriceText, DurationText, JoinDate)
    VALUES (@MemberId, @FullName, @Phone, @Gender, @PlanId, @PlanName, @PriceText, @DurationText, @JoinDate);
END
GO

IF OBJECT_ID(N'dbo.usp_Members_Update', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Members_Update;
GO
CREATE PROCEDURE dbo.usp_Members_Update
    @MemberId INT, @FullName NVARCHAR(150), @Phone NVARCHAR(40), @Gender NVARCHAR(20),
    @PlanName NVARCHAR(150), @PriceText NVARCHAR(60), @DurationText NVARCHAR(60), @JoinDate DATE,
    @PlanId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Members
    SET FullName = @FullName, Phone = @Phone, Gender = @Gender, PlanId = @PlanId, PlanName = @PlanName,
        PriceText = @PriceText, DurationText = @DurationText, JoinDate = @JoinDate
    WHERE MemberId = @MemberId;
END
GO

IF OBJECT_ID(N'dbo.usp_Members_Delete', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Members_Delete;
GO
CREATE PROCEDURE dbo.usp_Members_Delete @MemberId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Members WHERE MemberId = @MemberId;
END
GO

/* ----- Trainers ----------------------------------------------------------- */
IF OBJECT_ID(N'dbo.usp_Trainers_SelectAll', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Trainers_SelectAll;
GO
CREATE PROCEDURE dbo.usp_Trainers_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TrainerId, [Name], Phone, Specialty, Salary,
           CONVERT(VARCHAR(10), JoinDate, 23) AS JoinDate
    FROM   dbo.Trainers
    ORDER BY TrainerId;
END
GO

IF OBJECT_ID(N'dbo.usp_Trainers_Insert', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Trainers_Insert;
GO
CREATE PROCEDURE dbo.usp_Trainers_Insert
    @TrainerId INT, @Name NVARCHAR(150), @Phone NVARCHAR(40),
    @Specialty NVARCHAR(120), @Salary DECIMAL(18,2), @JoinDate DATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Trainers (TrainerId, [Name], Phone, Specialty, Salary, JoinDate)
    VALUES (@TrainerId, @Name, @Phone, @Specialty, @Salary, @JoinDate);
END
GO

IF OBJECT_ID(N'dbo.usp_Trainers_Update', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Trainers_Update;
GO
CREATE PROCEDURE dbo.usp_Trainers_Update
    @TrainerId INT, @Name NVARCHAR(150), @Phone NVARCHAR(40),
    @Specialty NVARCHAR(120), @Salary DECIMAL(18,2), @JoinDate DATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Trainers
    SET [Name] = @Name, Phone = @Phone, Specialty = @Specialty, Salary = @Salary, JoinDate = @JoinDate
    WHERE TrainerId = @TrainerId;
END
GO

IF OBJECT_ID(N'dbo.usp_Trainers_Delete', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Trainers_Delete;
GO
CREATE PROCEDURE dbo.usp_Trainers_Delete @TrainerId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Trainers WHERE TrainerId = @TrainerId;
END
GO

/* ----- StoreProducts ------------------------------------------------------ */
IF OBJECT_ID(N'dbo.usp_StoreProducts_SelectAll', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_StoreProducts_SelectAll;
GO
CREATE PROCEDURE dbo.usp_StoreProducts_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProductId, [Name], Price, Category, Emoji, StockQty,
           CONVERT(VARCHAR(10), Expiry, 23) AS Expiry, PhotoBase64
    FROM   dbo.StoreProducts
    ORDER BY ProductId;
END
GO

IF OBJECT_ID(N'dbo.usp_StoreProducts_Insert', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_StoreProducts_Insert;
GO
CREATE PROCEDURE dbo.usp_StoreProducts_Insert
    @ProductId INT, @Name NVARCHAR(150), @Price DECIMAL(18,2), @Category NVARCHAR(80),
    @Emoji NVARCHAR(20), @StockQty INT, @Expiry DATE, @PhotoBase64 NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.StoreProducts (ProductId, [Name], Price, Category, Emoji, StockQty, Expiry, PhotoBase64)
    VALUES (@ProductId, @Name, @Price, @Category, @Emoji, @StockQty, @Expiry, @PhotoBase64);
END
GO

IF OBJECT_ID(N'dbo.usp_StoreProducts_Update', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_StoreProducts_Update;
GO
CREATE PROCEDURE dbo.usp_StoreProducts_Update
    @ProductId INT, @Name NVARCHAR(150), @Price DECIMAL(18,2), @Category NVARCHAR(80),
    @Emoji NVARCHAR(20), @StockQty INT, @Expiry DATE, @PhotoBase64 NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.StoreProducts
    SET [Name] = @Name, Price = @Price, Category = @Category, Emoji = @Emoji,
        StockQty = @StockQty, Expiry = @Expiry, PhotoBase64 = @PhotoBase64
    WHERE ProductId = @ProductId;
END
GO

IF OBJECT_ID(N'dbo.usp_StoreProducts_Delete', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_StoreProducts_Delete;
GO
CREATE PROCEDURE dbo.usp_StoreProducts_Delete @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.StoreProducts WHERE ProductId = @ProductId;
END
GO

/* ----- StoreSales + Items ------------------------------------------------- */
IF OBJECT_ID(N'dbo.usp_StoreSales_SelectAll', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_StoreSales_SelectAll;
GO
CREATE PROCEDURE dbo.usp_StoreSales_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT SaleId, SoldAt, Total, Summary
    FROM   dbo.StoreSales
    ORDER BY SaleId;
END
GO

IF OBJECT_ID(N'dbo.usp_StoreSales_Insert', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_StoreSales_Insert;
GO
CREATE PROCEDURE dbo.usp_StoreSales_Insert
    @SaleId INT, @SoldAt NVARCHAR(50), @Total DECIMAL(18,2), @Summary NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.StoreSales (SaleId, SoldAt, Total, Summary)
    VALUES (@SaleId, @SoldAt, @Total, @Summary);
END
GO

IF OBJECT_ID(N'dbo.usp_StoreSaleItems_SelectAll', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_StoreSaleItems_SelectAll;
GO
CREATE PROCEDURE dbo.usp_StoreSaleItems_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT SaleId, [LineNo], ProductName, Price, Qty, ProductId
    FROM   dbo.StoreSaleItems
    ORDER BY SaleId, [LineNo];
END
GO

IF OBJECT_ID(N'dbo.usp_StoreSaleItems_Insert', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_StoreSaleItems_Insert;
GO
CREATE PROCEDURE dbo.usp_StoreSaleItems_Insert
    @SaleId INT, @LineNo INT, @ProductName NVARCHAR(200), @Price DECIMAL(18,2), @Qty INT,
    @ProductId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.StoreSaleItems (SaleId, [LineNo], ProductId, ProductName, Price, Qty)
    VALUES (@SaleId, @LineNo, @ProductId, @ProductName, @Price, @Qty);
END
GO

/* ----- FeedingPlans ------------------------------------------------------- */
IF OBJECT_ID(N'dbo.usp_FeedingPlans_SelectAll', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_FeedingPlans_SelectAll;
GO
CREATE PROCEDURE dbo.usp_FeedingPlans_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT FeedingPlanId, [Name], PdfPath
    FROM   dbo.FeedingPlans
    ORDER BY FeedingPlanId;
END
GO

IF OBJECT_ID(N'dbo.usp_FeedingPlans_Insert', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_FeedingPlans_Insert;
GO
CREATE PROCEDURE dbo.usp_FeedingPlans_Insert
    @FeedingPlanId INT, @Name NVARCHAR(150), @PdfPath NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.FeedingPlans (FeedingPlanId, [Name], PdfPath)
    VALUES (@FeedingPlanId, @Name, @PdfPath);
END
GO

IF OBJECT_ID(N'dbo.usp_FeedingPlans_Update', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_FeedingPlans_Update;
GO
CREATE PROCEDURE dbo.usp_FeedingPlans_Update
    @FeedingPlanId INT, @Name NVARCHAR(150), @PdfPath NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.FeedingPlans
    SET [Name] = @Name, PdfPath = @PdfPath
    WHERE FeedingPlanId = @FeedingPlanId;
END
GO

IF OBJECT_ID(N'dbo.usp_FeedingPlans_Delete', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_FeedingPlans_Delete;
GO
CREATE PROCEDURE dbo.usp_FeedingPlans_Delete @FeedingPlanId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.FeedingPlans WHERE FeedingPlanId = @FeedingPlanId;
END
GO

/* ----- DietSendHistory ---------------------------------------------------- */
IF OBJECT_ID(N'dbo.usp_DietSendHistory_SelectAll', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_DietSendHistory_SelectAll;
GO
CREATE PROCEDURE dbo.usp_DietSendHistory_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT HistoryId, Entry
    FROM   dbo.DietSendHistory
    ORDER BY HistoryId;
END
GO

IF OBJECT_ID(N'dbo.usp_DietSendHistory_Insert', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_DietSendHistory_Insert;
GO
CREATE PROCEDURE dbo.usp_DietSendHistory_Insert
    @HistoryId INT, @Entry NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.DietSendHistory (HistoryId, Entry)
    VALUES (@HistoryId, @Entry);
END
GO

/* ----- Master clear (used by the full-snapshot resync in GymDataStore.Save) */
IF OBJECT_ID(N'dbo.usp_ClearAllData', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_ClearAllData;
GO
CREATE PROCEDURE dbo.usp_ClearAllData
AS
BEGIN
    SET NOCOUNT ON;
    -- Delete referencing (child) rows before the tables they point at, so the
    -- foreign keys (Members -> SubscriptionPlans, StoreSaleItems -> StoreProducts/StoreSales) hold.
    DELETE FROM dbo.StoreSaleItems;
    DELETE FROM dbo.StoreSales;
    DELETE FROM dbo.SubscriptionPlanFeatures;
    DELETE FROM dbo.Members;
    DELETE FROM dbo.SubscriptionPlans;
    DELETE FROM dbo.StoreProducts;
    DELETE FROM dbo.Trainers;
    DELETE FROM dbo.FeedingPlans;
    DELETE FROM dbo.DietSendHistory;
    DELETE FROM dbo.Users;
END
GO

/* ----- Backup ------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.usp_BackupDatabase', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_BackupDatabase;
GO
CREATE PROCEDURE dbo.usp_BackupDatabase
    @BackupFolder NVARCHAR(400),
    @BackupFile   NVARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF RIGHT(@BackupFolder, 1) NOT IN ('\', '/')
        SET @BackupFolder = @BackupFolder + '\';

    DECLARE @stamp NVARCHAR(20) =
        REPLACE(REPLACE(REPLACE(CONVERT(NVARCHAR(19), GETDATE(), 120), '-', ''), ':', ''), ' ', '_');

    SET @BackupFile = @BackupFolder + N'GymDB_' + @stamp + N'.bak';

    -- COMPRESSION is intentionally omitted: SQL Server Express does not support it.
    BACKUP DATABASE [GymDB]
        TO DISK = @BackupFile
        WITH INIT, FORMAT, NAME = N'GymDB Full Backup';
END
GO

/* ============================================================================
   6. Seed data (only when empty) - mirrors GymDataStore.SeedDefaults()
   ============================================================================ */
IF NOT EXISTS (SELECT 1 FROM dbo.Users)
BEGIN
    INSERT INTO dbo.Users (UserId, FullName, Username, [Password], [Role]) VALUES
        (1, N'المدير العام', N'admin',     N'admin', 0),
        (2, N'مستلم النظام', N'reception', N'1234',  1);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.SubscriptionPlans)
BEGIN
    INSERT INTO dbo.SubscriptionPlans (PlanId, [Name], Price, DurationValue, DurationUnit) VALUES
        (1, N'Basic Plan',  30.00,  1, N'شهر'),
        (2, N'Pro Plan',    50.00,  3, N'شهر'),
        (3, N'Annual Plan', 300.00, 1, N'سنة');

    INSERT INTO dbo.SubscriptionPlanFeatures (PlanId, FeatureNo, Feature) VALUES
        (1, 1, N'دخول غير محدود'),
        (1, 2, N'خزانة خاصة'),
        (2, 1, N'دخول غير محدود'),
        (2, 2, N'حصص جماعية'),
        (2, 3, N'خزانة خاصة'),
        (2, 4, N'خصم على المتجر'),
        (3, 1, N'دخول غير محدود'),
        (3, 2, N'حصص جماعية'),
        (3, 3, N'مدرب شخصي'),
        (3, 4, N'غرفة ساونا'),
        (3, 5, N'خزانة خاصة'),
        (3, 6, N'خصم على المتجر'),
        (3, 7, N'خطة تغذية');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Members)
BEGIN
    INSERT INTO dbo.Members (MemberId, FullName, Phone, Gender, PlanId, PlanName, PriceText, DurationText, JoinDate) VALUES
        (1,  N'أحمد محمد',    N'0501234567', N'ذكر',  1, N'Basic Plan',  N'30 د.ل',  N'1 شهر', '2026-01-15'),
        (2,  N'سارة علي',     N'0559876543', N'أنثى', 2, N'Pro Plan',    N'50 د.ل',  N'3 شهر', '2025-06-20'),
        (3,  N'خالد إبراهيم', N'0561112233', N'ذكر',  3, N'Annual Plan', N'300 د.ل', N'1 سنة', '2025-11-01'),
        (4,  N'نورة حسن',     N'0547778899', N'أنثى', 1, N'Basic Plan',  N'30 د.ل',  N'1 شهر', '2026-02-10'),
        (5,  N'عمر فاروق',    N'0533334455', N'ذكر',  2, N'Pro Plan',    N'50 د.ل',  N'3 شهر', '2025-09-05'),
        (6,  N'ليلى أحمد',    N'0522225566', N'أنثى', 1, N'Basic Plan',  N'30 د.ل',  N'1 شهر', '2026-03-01'),
        (7,  N'يوسف كمال',    N'0511116677', N'ذكر',  3, N'Annual Plan', N'300 د.ل', N'1 سنة', '2025-08-15'),
        (8,  N'فاطمة سعيد',   N'0588889900', N'أنثى', 1, N'Basic Plan',  N'30 د.ل',  N'1 شهر', '2026-04-01'),
        (9,  N'محمود عادل',   N'0577771122', N'ذكر',  2, N'Pro Plan',    N'50 د.ل',  N'3 شهر', '2025-12-20'),
        (10, N'هند محمود',    N'0566662233', N'أنثى', 3, N'Annual Plan', N'300 د.ل', N'1 سنة', '2026-01-05');
END
GO

/* Currency migration: convert any previously seeded "ريال" labels to "د.ل". */
UPDATE dbo.Members
SET    PriceText = REPLACE(PriceText, N'ريال', N'د.ل')
WHERE  PriceText LIKE N'%ريال%';
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Trainers)
BEGIN
    INSERT INTO dbo.Trainers (TrainerId, [Name], Phone, Specialty, Salary, JoinDate) VALUES
        (1, N'محمد السالم',  N'0501111222', N'رفع أثقال',     2500, '2023-01-10'),
        (2, N'أنس العتيبي',  N'0522223333', N'كروس فيت',      2200, '2023-03-15'),
        (3, N'ريم الزهراني', N'0533334444', N'يوغا ولياقة',   2000, '2023-06-01'),
        (4, N'فيصل الحربي',  N'0544445555', N'ملاكمة',        2800, '2022-11-20'),
        (5, N'نورا الشمري',  N'0555556666', N'تمارين نسائية', 1900, '2024-01-05');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.StoreProducts)
BEGIN
    INSERT INTO dbo.StoreProducts (ProductId, [Name], Price, Category, Emoji, StockQty, Expiry, PhotoBase64) VALUES
        (1,  N'واي بروتين',      50.0, N'بروتين',       N'💪', 20, CAST(DATEADD(MONTH, 8,  GETDATE()) AS DATE), NULL),
        (2,  N'كرياتين مونو',    25.0, N'كرياتين',      N'⚡', 15, CAST(DATEADD(MONTH, 12, GETDATE()) AS DATE), NULL),
        (3,  N'BCAA أحماض',      30.0, N'بروتين',       N'🧬', 10, CAST(DATEADD(MONTH, 6,  GETDATE()) AS DATE), NULL),
        (4,  N'فيتامين D3',      12.0, N'فيتامينات',    N'☀️', 30, CAST(DATEADD(MONTH, 18, GETDATE()) AS DATE), NULL),
        (5,  N'أوميغا 3',        18.0, N'فيتامينات',    N'🐟', 25, CAST(DATEADD(MONTH, 10, GETDATE()) AS DATE), NULL),
        (6,  N'مشروب طاقة',      5.0,  N'مشروبات طاقة', N'🥤', 50, CAST(DATEADD(MONTH, 4,  GETDATE()) AS DATE), NULL),
        (7,  N'حزام رفع أثقال',  35.0, N'معدات',        N'🏋️', 8,  CAST(DATEADD(YEAR,  3,  GETDATE()) AS DATE), NULL),
        (8,  N'قفازات تمرين',    15.0, N'معدات',        N'🧤', 12, CAST(DATEADD(YEAR,  3,  GETDATE()) AS DATE), NULL),
        (9,  N'شيكر بروتين',     8.0,  N'معدات',        N'🥤', 18, CAST(DATEADD(YEAR,  2,  GETDATE()) AS DATE), NULL),
        (10, N'بروتين بار',      3.5,  N'بروتين',       N'🍫', 40, CAST(DATEADD(MONTH, 3,  GETDATE()) AS DATE), NULL),
        (11, N'جلوتامين',        22.0, N'بروتين',       N'💊', 14, CAST(DATEADD(MONTH, 9,  GETDATE()) AS DATE), NULL),
        (12, N'ZMA مكمل',        16.0, N'فيتامينات',    N'💤', 11, CAST(DATEADD(MONTH, 15, GETDATE()) AS DATE), NULL);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.FeedingPlans)
BEGIN
    INSERT INTO dbo.FeedingPlans (FeedingPlanId, [Name], PdfPath) VALUES
        (1, N'خطة التنشيف', N'C:\Plans\cutting_plan.pdf'),
        (2, N'خطة التضخم',  N'C:\Plans\bulking_plan.pdf'),
        (3, N'خطة الحفاظ',  N'C:\Plans\maintenance_plan.pdf'),
        (4, N'خطة نباتية',  N'C:\Plans\vegan_plan.pdf'),
        (5, N'خطة كيتو',    N'C:\Plans\keto_plan.pdf');
END
GO

PRINT N'GymDB setup completed successfully.';
GO

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
        (1, N'Basic Plan', 30.00, 1, N'شهر'),
        (2, N'Pro Plan', 50.00, 3, N'شهر'),
        (3, N'Annual Plan', 300.00, 1, N'سنة'),
        (4, N'Student Plan', 20.00, 1, N'شهر'),
        (5, N'Couple Plan', 80.00, 1, N'شهر'),
        (6, N'VIP Plan', 500.00, 1, N'سنة');

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
        (3, 7, N'خطة تغذية'),
        (4, 1, N'دخول في الفترة الصباحية'),
        (4, 2, N'خزانة مشتركة'),
        (5, 1, N'اشتراك لشخصين'),
        (5, 2, N'دخول غير محدود'),
        (5, 3, N'حصص جماعية'),
        (6, 1, N'دخول VIP غير محدود'),
        (6, 2, N'مدرب شخصي'),
        (6, 3, N'غرفة ساونا'),
        (6, 4, N'جاكوزي'),
        (6, 5, N'خطة تغذية'),
        (6, 6, N'موقف خاص'),
        (6, 7, N'خصم 20% على المتجر');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Members)
BEGIN
    INSERT INTO dbo.Members (MemberId, FullName, Phone, Gender, PlanId, PlanName, PriceText, DurationText, JoinDate) VALUES
        (1, N'سارة علي', N'0922181960', N'أنثى', 4, N'Student Plan', N'20 د.ل', N'1 شهر', '2025-09-08'),
        (2, N'عبدالله الفيتوري', N'0928637940', N'ذكر', 1, N'Basic Plan', N'30 د.ل', N'1 شهر', '2025-01-28'),
        (3, N'ليلى أحمد', N'0932351161', N'أنثى', 4, N'Student Plan', N'20 د.ل', N'1 شهر', '2025-12-15'),
        (4, N'ناصر الترهوني', N'0948161849', N'ذكر', 3, N'Annual Plan', N'300 د.ل', N'1 سنة', '2025-02-14'),
        (5, N'ريم الزهراني', N'0913413164', N'أنثى', 3, N'Annual Plan', N'300 د.ل', N'1 سنة', '2025-03-13'),
        (6, N'ناصر الترهوني', N'0924192832', N'ذكر', 1, N'Basic Plan', N'30 د.ل', N'1 شهر', '2025-12-30'),
        (7, N'حسام الورفلي', N'0930305641', N'ذكر', 6, N'VIP Plan', N'500 د.ل', N'1 سنة', '2025-08-13'),
        (8, N'أيمن الزنتاني', N'0927672423', N'ذكر', 5, N'Couple Plan', N'80 د.ل', N'1 شهر', '2025-11-19'),
        (9, N'خديجة المبروك', N'0945328710', N'أنثى', 1, N'Basic Plan', N'30 د.ل', N'1 شهر', '2026-03-15'),
        (10, N'ليلى أحمد', N'0916697848', N'أنثى', 3, N'Annual Plan', N'300 د.ل', N'1 سنة', '2026-03-09'),
        (11, N'سارة علي', N'0935146270', N'أنثى', 4, N'Student Plan', N'20 د.ل', N'1 شهر', '2025-04-28'),
        (12, N'دعاء الزنتاني', N'0928148932', N'أنثى', 4, N'Student Plan', N'20 د.ل', N'1 شهر', '2026-05-28'),
        (13, N'طارق المبروك', N'0937015430', N'ذكر', 3, N'Annual Plan', N'300 د.ل', N'1 سنة', '2025-01-01'),
        (14, N'أيمن الزنتاني', N'0941822782', N'ذكر', 6, N'VIP Plan', N'500 د.ل', N'1 سنة', '2025-03-29'),
        (15, N'سامي العماري', N'0924657871', N'ذكر', 2, N'Pro Plan', N'50 د.ل', N'3 شهر', '2025-08-05'),
        (16, N'عمر فاروق', N'0920103105', N'ذكر', 2, N'Pro Plan', N'50 د.ل', N'3 شهر', '2025-08-24'),
        (17, N'مصطفى الزوي', N'0928299737', N'ذكر', 1, N'Basic Plan', N'30 د.ل', N'1 شهر', '2026-05-13'),
        (18, N'فاطمة سعيد', N'0936670106', N'أنثى', 1, N'Basic Plan', N'30 د.ل', N'1 شهر', '2026-03-18'),
        (19, N'رانيا القماطي', N'0923872624', N'أنثى', 5, N'Couple Plan', N'80 د.ل', N'1 شهر', '2025-09-12'),
        (20, N'عمر فاروق', N'0918013267', N'ذكر', 2, N'Pro Plan', N'50 د.ل', N'3 شهر', '2025-04-11'),
        (21, N'محمد العبيدي', N'0940647468', N'ذكر', 5, N'Couple Plan', N'80 د.ل', N'1 شهر', '2025-06-18'),
        (22, N'أسماء الفيتوري', N'0920980500', N'أنثى', 1, N'Basic Plan', N'30 د.ل', N'1 شهر', '2025-10-31'),
        (23, N'خديجة المبروك', N'0918121913', N'أنثى', 5, N'Couple Plan', N'80 د.ل', N'1 شهر', '2025-06-11'),
        (24, N'أيمن الزنتاني', N'0916998543', N'ذكر', 1, N'Basic Plan', N'30 د.ل', N'1 شهر', '2025-02-10'),
        (25, N'ريم الزهراني', N'0924751079', N'أنثى', 1, N'Basic Plan', N'30 د.ل', N'1 شهر', '2026-02-10'),
        (26, N'نورة حسن', N'0932513542', N'أنثى', 1, N'Basic Plan', N'30 د.ل', N'1 شهر', '2025-08-07'),
        (27, N'هيثم البركي', N'0931241182', N'ذكر', 4, N'Student Plan', N'20 د.ل', N'1 شهر', '2025-01-09'),
        (28, N'سامي العماري', N'0924874016', N'ذكر', 1, N'Basic Plan', N'30 د.ل', N'1 شهر', '2025-12-18'),
        (29, N'سارة علي', N'0932786801', N'أنثى', 1, N'Basic Plan', N'30 د.ل', N'1 شهر', '2025-05-14'),
        (30, N'محمود عادل', N'0926204505', N'ذكر', 3, N'Annual Plan', N'300 د.ل', N'1 سنة', '2026-01-14');
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
        (1, N'محمد السالم', N'0949232260', N'ملاكمة', 1800, '2022-12-26'),
        (2, N'أنس العتيبي', N'0943421607', N'تمارين وظيفية', 2500, '2025-05-22'),
        (3, N'فيصل الحربي', N'0933303654', N'سبينينج', 2900, '2022-12-18'),
        (4, N'سهى الفيتوري', N'0948501429', N'بيلاتس', 2300, '2022-12-21'),
        (5, N'وفاء الزنتاني', N'0945569816', N'تمارين نسائية', 1600, '2021-08-11'),
        (6, N'آية البركي', N'0940883561', N'ملاكمة', 2300, '2021-04-01'),
        (7, N'لمياء الدرسي', N'0931484656', N'تمارين وظيفية', 2500, '2024-06-30'),
        (8, N'شيماء الورفلي', N'0946299468', N'سباحة', 1900, '2022-01-28'),
        (9, N'رغد القماطي', N'0949957773', N'تمارين نسائية', 2400, '2022-03-07'),
        (10, N'كريم الزوي', N'0914895134', N'بيلاتس', 2000, '2024-09-11'),
        (11, N'زياد المبروك', N'0913791769', N'ملاكمة', 1900, '2021-02-20'),
        (12, N'عماد الورفلي', N'0943201632', N'بناء أجسام', 2700, '2023-10-10'),
        (13, N'ندى العماري', N'0918317278', N'بناء أجسام', 3100, '2023-08-10'),
        (14, N'روان الزوي', N'0949868727', N'فنون قتالية', 2500, '2025-03-27'),
        (15, N'ماهر القماطي', N'0938734714', N'بيلاتس', 2200, '2025-09-17'),
        (16, N'محمد السالم', N'0912236231', N'كارديو', 2500, '2024-01-12'),
        (17, N'أنس العتيبي', N'0940366909', N'كارديو', 3200, '2023-08-13'),
        (18, N'فيصل الحربي', N'0946889373', N'رفع أثقال', 2600, '2022-09-04'),
        (19, N'عادل المنفي', N'0936272980', N'لياقة عامة', 1500, '2023-03-08'),
        (20, N'وفاء الزنتاني', N'0911627204', N'فنون قتالية', 3300, '2024-09-19'),
        (21, N'منصور أبوشعالة', N'0936464170', N'ملاكمة', 2900, '2022-11-01'),
        (22, N'لمياء الدرسي', N'0910033092', N'رفع أثقال', 2600, '2022-04-05'),
        (23, N'رضا الفيتوري', N'0927452991', N'لياقة عامة', 1800, '2024-03-01'),
        (24, N'رغد القماطي', N'0914966319', N'يوغا ولياقة', 2400, '2021-08-10'),
        (25, N'تالة الشريف', N'0939190586', N'تأهيل إصابات', 2200, '2021-07-28'),
        (26, N'جنى العابد', N'0930671657', N'كروس فيت', 3100, '2024-08-19'),
        (27, N'ميار الساعدي', N'0939877694', N'تغذية رياضية', 2000, '2025-02-11'),
        (28, N'سفيان الدرسي', N'0943799650', N'ملاكمة', 1700, '2022-07-26'),
        (29, N'غسان الشريف', N'0925454948', N'كارديو', 2000, '2023-09-26'),
        (30, N'ماهر القماطي', N'0947837770', N'ملاكمة', 1700, '2022-05-10');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.StoreProducts)
BEGIN
    INSERT INTO dbo.StoreProducts (ProductId, [Name], Price, Category, Emoji, StockQty, Expiry, PhotoBase64) VALUES
        (1, N'واي بروتين', 50.0, N'بروتين', N'💪', 10, CAST(DATEADD(MONTH, 9, GETDATE()) AS DATE), NULL),
        (2, N'كرياتين مونو', 25.0, N'كرياتين', N'⚡', 19, CAST(DATEADD(MONTH, 12, GETDATE()) AS DATE), NULL),
        (3, N'BCAA أحماض', 30.0, N'بروتين', N'🧬', 49, CAST(DATEADD(MONTH, 8, GETDATE()) AS DATE), NULL),
        (4, N'فيتامين D3', 12.0, N'فيتامينات', N'☀️', 24, CAST(DATEADD(MONTH, 10, GETDATE()) AS DATE), NULL),
        (5, N'أوميغا 3', 18.0, N'فيتامينات', N'🐟', 35, CAST(DATEADD(MONTH, 18, GETDATE()) AS DATE), NULL),
        (6, N'مشروب طاقة', 5.0, N'مشروبات طاقة', N'🥤', 38, CAST(DATEADD(MONTH, 10, GETDATE()) AS DATE), NULL),
        (7, N'حزام رفع أثقال', 35.0, N'معدات', N'🏋️', 32, CAST(DATEADD(YEAR, 3, GETDATE()) AS DATE), NULL),
        (8, N'قفازات تمرين', 15.0, N'معدات', N'🧤', 27, CAST(DATEADD(YEAR, 3, GETDATE()) AS DATE), NULL),
        (9, N'شيكر بروتين', 8.0, N'معدات', N'🥤', 24, CAST(DATEADD(YEAR, 2, GETDATE()) AS DATE), NULL),
        (10, N'بروتين بار', 3.5, N'بروتين', N'🍫', 12, CAST(DATEADD(MONTH, 8, GETDATE()) AS DATE), NULL),
        (11, N'جلوتامين', 22.0, N'بروتين', N'💊', 25, CAST(DATEADD(MONTH, 4, GETDATE()) AS DATE), NULL),
        (12, N'ZMA مكمل', 16.0, N'فيتامينات', N'💤', 52, CAST(DATEADD(MONTH, 18, GETDATE()) AS DATE), NULL),
        (13, N'كازين بروتين', 55.0, N'بروتين', N'🌙', 53, CAST(DATEADD(MONTH, 6, GETDATE()) AS DATE), NULL),
        (14, N'مالتي فيتامين', 20.0, N'فيتامينات', N'💊', 17, CAST(DATEADD(MONTH, 8, GETDATE()) AS DATE), NULL),
        (15, N'بري وركاوت', 28.0, N'مشروبات طاقة', N'🔥', 52, CAST(DATEADD(MONTH, 15, GETDATE()) AS DATE), NULL),
        (16, N'ماء معدني', 1.0, N'مشروبات طاقة', N'💧', 22, CAST(DATEADD(MONTH, 18, GETDATE()) AS DATE), NULL),
        (17, N'حبل قفز', 10.0, N'معدات', N'🪢', 43, CAST(DATEADD(YEAR, 2, GETDATE()) AS DATE), NULL),
        (18, N'بساط يوغا', 24.0, N'معدات', N'🧘', 58, CAST(DATEADD(YEAR, 3, GETDATE()) AS DATE), NULL),
        (19, N'أساور مقاومة', 18.0, N'معدات', N'💪', 19, CAST(DATEADD(YEAR, 2, GETDATE()) AS DATE), NULL),
        (20, N'تيشيرت رياضي', 30.0, N'ملابس', N'👕', 24, CAST(DATEADD(YEAR, 5, GETDATE()) AS DATE), NULL),
        (21, N'شورت تمرين', 25.0, N'ملابس', N'🩳', 39, CAST(DATEADD(YEAR, 3, GETDATE()) AS DATE), NULL),
        (22, N'حذاء رياضي', 90.0, N'ملابس', N'👟', 7, CAST(DATEADD(YEAR, 5, GETDATE()) AS DATE), NULL),
        (23, N'حقيبة جيم', 45.0, N'إكسسوارات', N'🎒', 23, CAST(DATEADD(YEAR, 5, GETDATE()) AS DATE), NULL),
        (24, N'سماعات رياضية', 60.0, N'إكسسوارات', N'🎧', 60, CAST(DATEADD(YEAR, 2, GETDATE()) AS DATE), NULL),
        (25, N'منشفة قطنية', 7.0, N'إكسسوارات', N'🧻', 60, CAST(DATEADD(YEAR, 5, GETDATE()) AS DATE), NULL),
        (26, N'ل-كارنتين', 26.0, N'بروتين', N'🔥', 23, CAST(DATEADD(MONTH, 15, GETDATE()) AS DATE), NULL),
        (27, N'كرياتين HCL', 32.0, N'كرياتين', N'⚡', 35, CAST(DATEADD(MONTH, 15, GETDATE()) AS DATE), NULL),
        (28, N'فيتامين C', 9.0, N'فيتامينات', N'🍊', 26, CAST(DATEADD(MONTH, 6, GETDATE()) AS DATE), NULL),
        (29, N'مكمل مفاصل', 34.0, N'فيتامينات', N'🦴', 8, CAST(DATEADD(MONTH, 9, GETDATE()) AS DATE), NULL),
        (30, N'بروتين نباتي', 48.0, N'بروتين', N'🌱', 60, CAST(DATEADD(MONTH, 15, GETDATE()) AS DATE), NULL);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.StoreSales)
BEGIN
    INSERT INTO dbo.StoreSales (SaleId, SoldAt, Total, Summary) VALUES
        (1, N'2026-01-30T21:00:00', 76.0, N'بيع: ماء معدني ×3، BCAA أحماض ×1، أساور مقاومة ×1، شورت تمرين ×1'),
        (2, N'2026-05-25T12:00:00', 96.0, N'بيع: فيتامين D3 ×2، بساط يوغا ×3'),
        (3, N'2026-02-27T20:45:00', 234.0, N'بيع: بروتين نباتي ×3، بري وركاوت ×2، بروتين بار ×2، فيتامين C ×3'),
        (4, N'2026-01-16T17:00:00', 155.0, N'بيع: شورت تمرين ×2، حزام رفع أثقال ×3'),
        (5, N'2026-01-21T10:15:00', 54.0, N'بيع: بساط يوغا ×1، BCAA أحماض ×1'),
        (6, N'2026-04-15T15:45:00', 115.5, N'بيع: كرياتين مونو ×3، قفازات تمرين ×2، بروتين بار ×3'),
        (7, N'2026-04-27T09:15:00', 139.0, N'بيع: ل-كارنتين ×3، شورت تمرين ×1، أساور مقاومة ×2'),
        (8, N'2026-01-30T16:15:00', 56.0, N'بيع: بروتين نباتي ×1، شيكر بروتين ×1'),
        (9, N'2026-01-16T10:30:00', 182.0, N'بيع: بري وركاوت ×2، فيتامين D3 ×3، حقيبة جيم ×2'),
        (10, N'2026-03-11T16:45:00', 302.0, N'بيع: BCAA أحماض ×2، تيشيرت رياضي ×3، كرياتين مونو ×2، مكمل مفاصل ×3'),
        (11, N'2026-03-06T08:00:00', 366.0, N'بيع: حذاء رياضي ×3، كرياتين HCL ×3'),
        (12, N'2026-01-06T20:30:00', 7.0, N'بيع: منشفة قطنية ×1'),
        (13, N'2026-05-01T16:45:00', 71.0, N'بيع: مشروب طاقة ×3، أساور مقاومة ×2، مالتي فيتامين ×1'),
        (14, N'2026-05-01T13:45:00', 226.0, N'بيع: جلوتامين ×1، حذاء رياضي ×2، فيتامين D3 ×2'),
        (15, N'2026-05-07T12:45:00', 28.0, N'بيع: بري وركاوت ×1'),
        (16, N'2026-03-22T12:30:00', 14.0, N'بيع: منشفة قطنية ×2'),
        (17, N'2026-05-12T21:00:00', 215.0, N'بيع: مالتي فيتامين ×2، كرياتين مونو ×3، حزام رفع أثقال ×2، حبل قفز ×3'),
        (18, N'2026-04-24T20:00:00', 56.0, N'بيع: شيكر بروتين ×1، بساط يوغا ×2'),
        (19, N'2026-04-23T19:45:00', 150.0, N'بيع: واي بروتين ×3'),
        (20, N'2026-03-03T19:15:00', 94.0, N'بيع: بساط يوغا ×1، واي بروتين ×1، مالتي فيتامين ×1'),
        (21, N'2026-04-29T09:15:00', 261.0, N'بيع: بروتين نباتي ×3، حقيبة جيم ×2، بروتين بار ×2، حبل قفز ×2'),
        (22, N'2026-05-01T11:45:00', 270.0, N'بيع: كازين بروتين ×3، حزام رفع أثقال ×3'),
        (23, N'2026-02-04T21:00:00', 93.0, N'بيع: منشفة قطنية ×2، ل-كارنتين ×2، فيتامين C ×3'),
        (24, N'2026-03-10T21:00:00', 286.5, N'بيع: سماعات رياضية ×3، بروتين بار ×3، كرياتين HCL ×3'),
        (25, N'2026-05-06T21:15:00', 151.0, N'بيع: بساط يوغا ×3، ماء معدني ×3، ZMA مكمل ×2، جلوتامين ×2'),
        (26, N'2026-03-24T21:15:00', 128.0, N'بيع: أساور مقاومة ×1، كازين بروتين ×2'),
        (27, N'2026-01-12T13:45:00', 339.0, N'بيع: كازين بروتين ×3، حذاء رياضي ×1، ل-كارنتين ×2، كرياتين HCL ×1'),
        (28, N'2026-02-02T16:30:00', 18.0, N'بيع: فيتامين C ×2'),
        (29, N'2026-01-26T16:45:00', 60.0, N'بيع: سماعات رياضية ×1'),
        (30, N'2026-04-15T21:15:00', 2.0, N'بيع: ماء معدني ×2');

    INSERT INTO dbo.StoreSaleItems (SaleId, [LineNo], ProductId, ProductName, Price, Qty) VALUES
        (1, 1, 16, N'ماء معدني', 1.0, 3),
        (1, 2, 3, N'BCAA أحماض', 30.0, 1),
        (1, 3, 19, N'أساور مقاومة', 18.0, 1),
        (1, 4, 21, N'شورت تمرين', 25.0, 1),
        (2, 1, 4, N'فيتامين D3', 12.0, 2),
        (2, 2, 18, N'بساط يوغا', 24.0, 3),
        (3, 1, 30, N'بروتين نباتي', 48.0, 3),
        (3, 2, 15, N'بري وركاوت', 28.0, 2),
        (3, 3, 10, N'بروتين بار', 3.5, 2),
        (3, 4, 28, N'فيتامين C', 9.0, 3),
        (4, 1, 21, N'شورت تمرين', 25.0, 2),
        (4, 2, 7, N'حزام رفع أثقال', 35.0, 3),
        (5, 1, 18, N'بساط يوغا', 24.0, 1),
        (5, 2, 3, N'BCAA أحماض', 30.0, 1),
        (6, 1, 2, N'كرياتين مونو', 25.0, 3),
        (6, 2, 8, N'قفازات تمرين', 15.0, 2),
        (6, 3, 10, N'بروتين بار', 3.5, 3),
        (7, 1, 26, N'ل-كارنتين', 26.0, 3),
        (7, 2, 21, N'شورت تمرين', 25.0, 1),
        (7, 3, 19, N'أساور مقاومة', 18.0, 2),
        (8, 1, 30, N'بروتين نباتي', 48.0, 1),
        (8, 2, 9, N'شيكر بروتين', 8.0, 1),
        (9, 1, 15, N'بري وركاوت', 28.0, 2),
        (9, 2, 4, N'فيتامين D3', 12.0, 3),
        (9, 3, 23, N'حقيبة جيم', 45.0, 2),
        (10, 1, 3, N'BCAA أحماض', 30.0, 2),
        (10, 2, 20, N'تيشيرت رياضي', 30.0, 3),
        (10, 3, 2, N'كرياتين مونو', 25.0, 2),
        (10, 4, 29, N'مكمل مفاصل', 34.0, 3),
        (11, 1, 22, N'حذاء رياضي', 90.0, 3),
        (11, 2, 27, N'كرياتين HCL', 32.0, 3),
        (12, 1, 25, N'منشفة قطنية', 7.0, 1),
        (13, 1, 6, N'مشروب طاقة', 5.0, 3),
        (13, 2, 19, N'أساور مقاومة', 18.0, 2),
        (13, 3, 14, N'مالتي فيتامين', 20.0, 1),
        (14, 1, 11, N'جلوتامين', 22.0, 1),
        (14, 2, 22, N'حذاء رياضي', 90.0, 2),
        (14, 3, 4, N'فيتامين D3', 12.0, 2),
        (15, 1, 15, N'بري وركاوت', 28.0, 1),
        (16, 1, 25, N'منشفة قطنية', 7.0, 2),
        (17, 1, 14, N'مالتي فيتامين', 20.0, 2),
        (17, 2, 2, N'كرياتين مونو', 25.0, 3),
        (17, 3, 7, N'حزام رفع أثقال', 35.0, 2),
        (17, 4, 17, N'حبل قفز', 10.0, 3),
        (18, 1, 9, N'شيكر بروتين', 8.0, 1),
        (18, 2, 18, N'بساط يوغا', 24.0, 2),
        (19, 1, 1, N'واي بروتين', 50.0, 3),
        (20, 1, 18, N'بساط يوغا', 24.0, 1),
        (20, 2, 1, N'واي بروتين', 50.0, 1),
        (20, 3, 14, N'مالتي فيتامين', 20.0, 1),
        (21, 1, 30, N'بروتين نباتي', 48.0, 3),
        (21, 2, 23, N'حقيبة جيم', 45.0, 2),
        (21, 3, 10, N'بروتين بار', 3.5, 2),
        (21, 4, 17, N'حبل قفز', 10.0, 2),
        (22, 1, 13, N'كازين بروتين', 55.0, 3),
        (22, 2, 7, N'حزام رفع أثقال', 35.0, 3),
        (23, 1, 25, N'منشفة قطنية', 7.0, 2),
        (23, 2, 26, N'ل-كارنتين', 26.0, 2),
        (23, 3, 28, N'فيتامين C', 9.0, 3),
        (24, 1, 24, N'سماعات رياضية', 60.0, 3),
        (24, 2, 10, N'بروتين بار', 3.5, 3),
        (24, 3, 27, N'كرياتين HCL', 32.0, 3),
        (25, 1, 18, N'بساط يوغا', 24.0, 3),
        (25, 2, 16, N'ماء معدني', 1.0, 3),
        (25, 3, 12, N'ZMA مكمل', 16.0, 2),
        (25, 4, 11, N'جلوتامين', 22.0, 2),
        (26, 1, 19, N'أساور مقاومة', 18.0, 1),
        (26, 2, 13, N'كازين بروتين', 55.0, 2),
        (27, 1, 13, N'كازين بروتين', 55.0, 3),
        (27, 2, 22, N'حذاء رياضي', 90.0, 1),
        (27, 3, 26, N'ل-كارنتين', 26.0, 2),
        (27, 4, 27, N'كرياتين HCL', 32.0, 1),
        (28, 1, 28, N'فيتامين C', 9.0, 2),
        (29, 1, 24, N'سماعات رياضية', 60.0, 1),
        (30, 1, 16, N'ماء معدني', 1.0, 2);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.FeedingPlans)
BEGIN
    INSERT INTO dbo.FeedingPlans (FeedingPlanId, [Name], PdfPath) VALUES
        (1, N'خطة التنشيف',     N'C:\Plans\cutting_plan.pdf'),
        (2, N'خطة التضخم',      N'C:\Plans\bulking_plan.pdf'),
        (3, N'خطة الحفاظ',      N'C:\Plans\maintenance_plan.pdf'),
        (4, N'خطة نباتية',      N'C:\Plans\vegan_plan.pdf'),
        (5, N'خطة كيتو',        N'C:\Plans\keto_plan.pdf'),
        (6, N'خطة زيادة الكتلة', N'C:\Plans\mass_gain_plan.pdf'),
        (7, N'خطة لو كارب',     N'C:\Plans\low_carb_plan.pdf'),
        (8, N'خطة البحر المتوسط', N'C:\Plans\mediterranean_plan.pdf');
END
GO

PRINT N'GymDB setup completed successfully.';
GO

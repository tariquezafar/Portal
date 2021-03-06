
INSERT INTO UserInterface(InterfaceId,InterfaceName, InterfaceDescription, InterfaceType, InterfaceSubType, InterfaceURL, SequenceNo, Status, CompanyBranchId)
VALUES (12292,'Dispatch Plan','Dispatch Plan','SALE', 'MASTER','~/DispatchPlan/ListDispatchPlan',39, 1, 10004)

GO

INSERT INTO RoleUIActionMapping(RoleId, InterfaceId, AddAccess, EditAccess, ViewAccess, Status,CancelAccess, ReviseAccess, CompanyBranchId)
VALUES(2, 12292, 1, 1, 1, 1, 0, 0, 10004)

GO
INSERT INTO UserInterface(InterfaceId,InterfaceName, InterfaceDescription, InterfaceType, InterfaceSubType, InterfaceURL, SequenceNo, Status, CompanyBranchId)
VALUES (12293,'Appoved Dispatch Plan','Dispatch Plan','SALE', 'MASTER','~/DispatchPlan/ListApproveDP',40, 1, 10004)

GO
INSERT INTO RoleUIActionMapping(RoleId, InterfaceId, AddAccess, EditAccess, ViewAccess, Status,CancelAccess, ReviseAccess, CompanyBranchId)
VALUES(2, 12293, 1, 1, 1, 1, 0, 0, 10004)

GO
INSERT INTO UserInterface(InterfaceId,InterfaceName, InterfaceDescription, InterfaceType, InterfaceSubType, InterfaceURL, SequenceNo, Status, CompanyBranchId)
VALUES (12294,'Dispatch','Dispatch','SALE', 'MASTER','~/Dispatch/ListDispatch',41, 1, 10004)

GO
INSERT INTO RoleUIActionMapping(RoleId, InterfaceId, AddAccess, EditAccess, ViewAccess, Status,CancelAccess, ReviseAccess, CompanyBranchId)
VALUES(2, 12294, 1, 1, 1, 1, 0, 0, 10004)

GO


CREATE TYPE [dbo].[udt_DispatchPlanProductDetail] AS TABLE(
	[SOId] [int] NULL,
	[ProductId] [bigint] NULL,
	[Quantity] [decimal](18, 2) NULL,
	[Priority] [decimal](18, 2) NULL
)
GO

CREATE TYPE [dbo].[udt_DispatchProductDetail] AS TABLE(
	[SOId] [int] NULL,
	[ProductId] [bigint] NULL,
	[Quantity] [decimal](18, 2) NULL,
	[Priority] [decimal](18, 2) NULL
)

GO
CREATE TABLE [dbo].[Dispatch](
	[DispatchID] [int] IDENTITY(1,1) NOT NULL,
	[DispatchNo] [varchar](50) NULL,
	[DispatchDate] [date] NULL,
	[DispatchPlanID] [int] NOT NULL,
	[CompanyBranchID] [int] NULL,
	[DispatchSequence] [int] NULL,
	[ApprovalStatus] [varchar](20) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [date] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[DispatchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
CREATE TABLE [dbo].[DispatchPlan](
	[DispatchPlanID] [int] IDENTITY(1,1) NOT NULL,
	[DispatchPlanNo] [varchar](50) NULL,
	[DispatchPlanDate] [date] NULL,
	[CustomerID] [int] NULL,
	[CompanyBranchID] [int] NULL,
	[DispatchSequence] [int] NULL,
	[StatusID] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [date] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [date] NULL,
	[ApprovalStatus] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[DispatchPlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


GO
CREATE TABLE [dbo].[DispatchPlanProductDetail](
	[DPProductDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[DispatchPlanID] [int] NULL,
	[SOId] [int] NULL,
	[ProductId] [int] NULL,
	[Quantity] [decimal](18, 2) NULL,
	[Priority] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[DPProductDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[DispatchProductDetail](
	[DispatchDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[DispatchID] [int] NULL,
	[SOId] [int] NULL,
	[ProductId] [int] NULL,
	[Quantity] [decimal](18, 2) NULL,
	[Priority] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[DispatchDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Dispatch]  WITH CHECK ADD FOREIGN KEY([DispatchPlanID])
REFERENCES [dbo].[DispatchPlan] ([DispatchPlanID])
GO
ALTER TABLE [dbo].[DispatchPlan]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[DispatchPlanProductDetail]  WITH CHECK ADD FOREIGN KEY([DispatchPlanID])
REFERENCES [dbo].[DispatchPlan] ([DispatchPlanID])
GO
ALTER TABLE [dbo].[DispatchProductDetail]  WITH CHECK ADD FOREIGN KEY([DispatchID])
REFERENCES [dbo].[Dispatch] ([DispatchID])
GO



-- =============================================
-- Author		:		Dheeraj Kumar
-- Create date	:		26 Jun 2022
-- Description	:		This function is used to split string value.
-- ==============================================
CREATE FUNCTION [dbo].[Splitstr]
(
  @delimited nvarchar(max),
  @delimiter nvarchar(100)
) RETURNS @t TABLE
(
-- Id column can be commented out, not required for sql splitting string
  id int identity(1,1), -- I use this column for numbering splitted parts
  val nvarchar(max)
)
AS
BEGIN
  declare @xml xml
  set @xml = N'<root><r>' + replace(@delimited,@delimiter,'</r><r>') + '</r></root>'

  insert into @t(val)
  select
    r.value('.','varchar(max)') as item
  from @xml.nodes('//root/r') as records(r)

  RETURN
END

GO

-- =============================================
-- Author		:		Dheeraj Kumar
-- Create date	:		03 July 2022
-- Description	:		This sp is used to get sale order.
-- ==============================================
--EXECUTE proc_GetDispatchProductList 2
CREATE PROC [dbo].[proc_GetDispatchProductList]    
(                
@DispatchID INT
)                
AS                
BEGIN                
	SET NOCOUNT ON;     
	
		SELECT C.CustomerName, C.City, SO.SOId, SO.SONo, DPD.ProductId, P.ProductName, P.ProductCode, 
		ISNULL(DPD.Quantity, 0) AS Quantity,
		ISNULL(DPD.Priority, 0) AS Priority
		FROM DispatchProductDetail AS DPD
		INNER JOIN SO AS SO ON DPD.SOId = SO.SOId
		INNER JOIN Product AS P ON DPD.ProductId = P.Productid
		INNER JOIN Customer AS C ON SO.CustomerId = C.CustomerId
		WHERE DPD.DispatchID = @DispatchID
	     
	SET NOCOUNT OFF;                
END            
  
GO

  -- =============================================
-- Author		:		DHEERAJ KUMAR
-- Create date	:		27-Jun-2022
-- Description	:		This sp is used to get Dispatch Plan Detail
-- ==============================================
-- EXECUTE [dbo].[proc_GetDispatchPlanDetail] 1  
CREATE  proc [dbo].[proc_GetDispatchPlanDetail]            
(                      
	@DispatchPlanID INT                    
)                      
AS 
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date			Story					Developer			Description  
-- ----------- ----------------------- ------------------- -----------------------------------------------------------------------------------------
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN                      
	SET NOCOUNT ON;   

	SELECT DP.DispatchPlanID, DP.DispatchPlanDate, DP.DispatchPlanNo, DP.CustomerID, C.CustomerName, DP.CompanyBranchID,
	CB.BranchName, DP.ApprovalStatus
	FROM DispatchPlan AS DP
	INNER JOIN Customer AS C ON DP.CustomerID = C.CustomerId
	INNER JOIN ComapnyBranch AS CB ON DP.CompanyBranchID = CB.CompanyBranchId
	WHERE Dp.DispatchPlanID = @DispatchPlanID

	SET NOCOUNT OFF;                      
END 

GO

-- =============================================
-- Author		:		DHEERAJ KUMAR
-- Create date	:		27-Jun-2022
-- Description	:		This sp is used to get Dispatch Plan Detail
-- ==============================================
-- EXECUTE [dbo].[proc_GetDispatchPlan] '', '', 0 , '2017-01-01', '2022-06-30',''
CREATE PROC [dbo].[proc_GetDispatchPlan] 
( 
@DispatchPlanNo VARCHAR(50) = '',                                
@CustomerName VARCHAR(50) = '', 
@CompanyBranchId  INT = 0  ,
@FromDate DATE, 
@ToDate DATE,
@ApprovalStatus	VARCHAR(20) = ''
)                    
AS   
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date			Story					Developer			Description  
-- ----------- ----------------------- ------------------- -----------------------------------------------------------------------------------------
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN                    
	SET NOCOUNT ON;                    
	DECLARE @STRSQL AS NVARCHAR(4000);
	
	SET @STRSQL = 'SELECT DP.DispatchPlanID, DP.DispatchPlanNo , 
	REPLACE(CONVERT(VARCHAR, DP.DispatchPlanDate, 106),'''',''-'') AS DispatchPlanDate,
	C.CustomerId, C.CustomerName, CB.BranchName ,DP.ApprovalStatus
	FROM DispatchPlan AS DP
	INNER JOIN Customer AS C ON DP.CustomerID = C.CustomerId
	INNER JOIN ComapnyBranch AS CB ON DP.CompanyBranchID = CB.CompanyBranchId
	WHERE DP.ApprovalStatus != ''Approve'''        
	
	SET @STRSQL = @STRSQL + ' AND DP.DispatchPlanDate BETWEEN '''+ CAST(@FromDate AS VARCHAR) + ''' AND  '''+ CAST(@ToDate AS VARCHAR) + ''' ';     

          
	IF @DispatchPlanNo <> ''                    
	BEGIN                    
		SET @STRSQL = @STRSQL + ' AND DP.DispatchPlanNo LIKE ''%' + @DispatchPlanNo + '%''';                    
	END 

	IF @CustomerName <> ''                    
	BEGIN                    
		SET @STRSQL = @STRSQL + ' AND C.CustomerName LIKE ''%' + @CustomerName + '%''';                    
	END 
     
	IF @CompanyBranchId <> 0                  
	BEGIN                  
		SET @STRSQL = @STRSQL + ' AND DP.CompanyBranchID =' + CAST(@CompanyBranchId AS VARCHAR) + '';                  
	END  

	IF @ApprovalStatus <> '' AND @ApprovalStatus <> '0'                  
	BEGIN                  
		SET @STRSQL = @STRSQL + ' AND DP.ApprovalStatus =''' + @ApprovalStatus + '''';                  
	END  
            
	SET @STRSQL = @STRSQL + ' ORDER BY DP.DispatchPlanNo DESC '

	PRINT @STRSQL
	EXECUTE SP_EXECUTESQL @STRSQL    
                  
	SET NOCOUNT OFF;  
	
END 

GO

-- =============================================
-- Author		:		DHEERAJ KUMAR
-- Create date	:		03-July-2022
-- Description	:		This sp is used to get Dispatch Plan Detail
-- ==============================================
-- EXECUTE [dbo].[proc_GetDispatchList] '', '', 0 ,'0', '2012-01-01', '2023-06-30'
CREATE PROC [dbo].[proc_GetDispatchList] 
( 
@DispatchNo VARCHAR(50) = '',  
@DispatchPlanNo VARCHAR(50) = '',                                
@CompanyBranchId  INT = 0  ,
@ApprovalStatus	VARCHAR(20) = '',
@FromDate DATE, 
@ToDate DATE

)                    
AS   
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date			Story					Developer			Description  
-- ----------- ----------------------- ------------------- -----------------------------------------------------------------------------------------
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN                    
	SET NOCOUNT ON;                    
	DECLARE @STRSQL AS NVARCHAR(4000);
	
	SET @STRSQL = '	SELECT D.DispatchID, D.DispatchNo, DP.DispatchPlanNo,
					REPLACE(CONVERT(VARCHAR, D.DispatchDate, 106),'''',''-'') AS DispatchDate,
					CB.BranchName, D.ApprovalStatus
					FROM Dispatch AS D
					INNER JOIN DispatchPlan AS DP ON D.DispatchPlanID = DP.DispatchPlanID
					INNER JOIN ComapnyBranch AS CB ON D.CompanyBranchID = CB.CompanyBranchId
					WHERE 1 = 1 '        
	
	SET @STRSQL = @STRSQL + ' AND D.DispatchDate BETWEEN '''+ CAST(@FromDate AS VARCHAR) + ''' AND  '''+ CAST(@ToDate AS VARCHAR) + ''' ';     

	IF @DispatchNo <> ''                    
	BEGIN                    
		SET @STRSQL = @STRSQL + ' AND D.DispatchNo LIKE ''%' + @DispatchNo + '%''';                    
	END 
          
	IF @DispatchPlanNo <> ''                    
	BEGIN                    
		SET @STRSQL = @STRSQL + ' AND DP.DispatchPlanNo LIKE ''%' + @DispatchPlanNo + '%''';                    
	END 

	     
	IF @CompanyBranchId <> 0                  
	BEGIN                  
		SET @STRSQL = @STRSQL + ' AND D.CompanyBranchID =' + CAST(@CompanyBranchId AS VARCHAR) + '';                  
	END  

	IF @ApprovalStatus <> '' AND @ApprovalStatus <> '0'                  
	BEGIN                  
		SET @STRSQL = @STRSQL + ' AND D.ApprovalStatus =''' + @ApprovalStatus + '''';                  
	END  
            
	SET @STRSQL = @STRSQL + ' ORDER BY D.DispatchNo DESC '

	PRINT @STRSQL
	EXECUTE SP_EXECUTESQL @STRSQL    
                  
	SET NOCOUNT OFF;  
	
END 

GO

-- =============================================
-- Author		:		DHEERAJ KUMAR
-- Create date	:		05-Jun-2022
-- Description	:		This sp is used to get Dispatch Detail
-- ==============================================
-- EXECUTE [dbo].[proc_GetDispatchDetail] 2  
CREATE PROC [dbo].[proc_GetDispatchDetail]            
(                      
	@DispatchID INT                    
)                      
AS 
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date			Story					Developer			Description  
-- ----------- ----------------------- ------------------- -----------------------------------------------------------------------------------------
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN                      
	SET NOCOUNT ON;   

	SELECT D.DispatchID, D.DispatchNo, 
	REPLACE(CONVERT(VARCHAR, D.DispatchDate, 106),'','-') AS DispatchDate,
	DP.DispatchPlanID, 
	REPLACE(CONVERT(VARCHAR, DP.DispatchPlanDate, 106),'','-') AS DispatchPlanDate, 
	DP.DispatchPlanNo,  
	D.CompanyBranchID,
	CB.BranchName, D.ApprovalStatus
	FROM Dispatch AS D
	INNER JOIN DispatchPlan AS DP ON D.DispatchPlanID = DP.DispatchPlanID
	INNER JOIN ComapnyBranch AS CB ON D.CompanyBranchID = CB.CompanyBranchId 
	WHERE D.DispatchID = @DispatchID

	SET NOCOUNT OFF;                      
END 
GO
-- =============================================
-- Author		:		DHEERAJ KUMAR
-- Create date	:		27-Jun-2022
-- Description	:		This sp is used to get Dispatch Plan Detail
-- ==============================================
-- EXECUTE [dbo].[proc_GetApprovDispatchPlanList] '', '', 0 , '2017-01-01', '2022-06-30',''
CREATE PROC [dbo].[proc_GetApprovDispatchPlanList] 
( 
@DispatchPlanNo VARCHAR(50) = '',                                
@CustomerName VARCHAR(50) = '', 
@CompanyBranchId  INT = 0  ,
@FromDate DATE, 
@ToDate DATE,
@ApprovalStatus	VARCHAR(20) = ''
)                    
AS   
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date			Story					Developer			Description  
-- ----------- ----------------------- ------------------- -----------------------------------------------------------------------------------------
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN                    
	SET NOCOUNT ON;                    
	DECLARE @STRSQL AS NVARCHAR(4000);
	
	SET @STRSQL = 'SELECT DP.DispatchPlanID, DP.DispatchPlanNo , 
	REPLACE(CONVERT(VARCHAR, DP.DispatchPlanDate, 106),'''',''-'') AS DispatchPlanDate,
	C.CustomerId, C.CustomerName, CB.BranchName ,DP.ApprovalStatus
	FROM DispatchPlan AS DP
	INNER JOIN Customer AS C ON DP.CustomerID = C.CustomerId
	INNER JOIN ComapnyBranch AS CB ON DP.CompanyBranchID = CB.CompanyBranchId
	WHERE DP.ApprovalStatus != ''Draft'''        
	
	SET @STRSQL = @STRSQL + ' AND DP.DispatchPlanDate BETWEEN '''+ CAST(@FromDate AS VARCHAR) + ''' AND  '''+ CAST(@ToDate AS VARCHAR) + ''' ';     

          
	IF @DispatchPlanNo <> ''                    
	BEGIN                    
		SET @STRSQL = @STRSQL + ' AND DP.DispatchPlanNo LIKE ''%' + @DispatchPlanNo + '%''';                    
	END 

	IF @CustomerName <> ''                    
	BEGIN                    
		SET @STRSQL = @STRSQL + ' AND C.CustomerName LIKE ''%' + @CustomerName + '%''';                    
	END 
     
	IF @CompanyBranchId <> 0                  
	BEGIN                  
		SET @STRSQL = @STRSQL + ' AND DP.CompanyBranchID =' + CAST(@CompanyBranchId AS VARCHAR) + '';                  
	END  

	IF @ApprovalStatus <> '' AND @ApprovalStatus <> '0'                  
	BEGIN                  
		SET @STRSQL = @STRSQL + ' AND DP.ApprovalStatus =''' + @ApprovalStatus + '''';                  
	END  
            
	SET @STRSQL = @STRSQL + ' ORDER BY DP.DispatchPlanNo DESC '

	PRINT @STRSQL
	EXECUTE SP_EXECUTESQL @STRSQL    
                  
	SET NOCOUNT OFF;  
	
END 

GO
-- =============================================
-- Author		:		DHEERAJ KUMAR
-- Create date	:		27-Jun-2022
-- Description	:		This sp is used to Save Dispatch Plan Data in Table.
-- ==============================================
CREATE PROC [dbo].[proc_AddEditDispatchPlan]        
(        
	@DispatchPlanID INT = 0, 
	@DispatchPlanDate DATE,       
	@CustomerID INT,  
	@CompanyBranchID INT,
	@UserID INT,
	@StatusID INT,
	@ApprovalStatus VARCHAR(20),
	@DispatchPlanProductDetail udt_DispatchPlanProductDetail readonly,  
	@Status VARCHAR(50) OUT,        
	@Message VARCHAR(500) OUT,        
	@RetDispatchPlanId BIGINT OUT  
)        
AS
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date			Story					Developer			Description  
-- ----------- ----------------------- ------------------- -----------------------------------------------------------------------------------------
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN        
BEGIN TRY        
BEGIN TRANSACTION        
	
	DECLARE @CompanyBranchCode VARCHAR(10), @CompanyStateId INT, @DispatchPlnaNo VARCHAR(50), @MaxDispatchPlnaNo INT

	DECLARE @TempProduct AS TABLE
	(    
		RowId INT,  
		SOId INT, 
		ProductId BIGINT,    
		Quantity DECIMAL(18,2)    
	)
	
	SELECT @CompanyBranchCode = CB.CompanyBranchCode FROM ComapnyBranch CB WHERE CB.CompanyBranchId = @CompanyBranchId  

	--INSERT START 
	
	IF @DispatchPlanID = 0          
	BEGIN                  
		SELECT @MaxDispatchPlnaNo = MAX(DispatchSequence) FROM DispatchPlan WHERE CompanyBranchID = @CompanyBranchId
		
		IF ISNULL(@MaxDispatchPlnaNo , 0) <> 0        
		BEGIN        
			SET @MaxDispatchPlnaNo = @MaxDispatchPlnaNo + 1;        
		END        
		ELSE        
		BEGIN        
			SET @MaxDispatchPlnaNo = 1;        
		END        

		SET @DispatchPlnaNo = @CompanyBranchCode + '/DP/'+ FORMAT(@MaxDispatchPlnaNo , '000#')
		
		INSERT INTO DispatchPlan (DispatchPlanNo, DispatchPlanDate, CustomerID, CompanyBranchID, DispatchSequence, StatusID, CreatedBy , CreatedDate, ApprovalStatus ) 
		VALUES(@DispatchPlnaNo, @DispatchPlanDate, @CustomerID, @CompanyBranchID, @MaxDispatchPlnaNo, @UserID, @StatusID, GETDATE(), @ApprovalStatus)
        

		SET @RetDispatchPlanId = SCOPE_IDENTITY();         

		INSERT INTO DispatchPlanProductDetail(DispatchPlanID, SOId, ProductId, Quantity, Priority)        
		SELECT @RetDispatchPlanId, SOId, ProductId, Quantity, Priority from @DispatchPlanProductDetail 


		SET @Message = '';        
		SET @Status = 'SUCCESS';        
         
	END  
	
	ELSE      
	BEGIN    
		UPDATE DispatchPlan SET DispatchPlanDate = @DispatchPlanDate,
								CustomerID = @CustomerID,
								CompanyBranchID = @CompanyBranchID,
								StatusID = @StatusID,
								ModifiedBy = @UserID,
								ModifiedDate = GETDATE(),
								ApprovalStatus = @ApprovalStatus
								WHERE DispatchPlanID = @DispatchPlanID

		DELETE FROM DispatchPlanProductDetail WHERE DispatchPlanID = @DispatchPlanID  

		INSERT INTO DispatchPlanProductDetail(DispatchPlanID, SOId, ProductId, Quantity, Priority)        
		SELECT @DispatchPlanID, SOId, ProductId, Quantity, Priority from @DispatchPlanProductDetail

		SET @Message = '';        
		SET @Status = 'SUCCESS';        
		SET @RetDispatchPlanId = @DispatchPlanID; 
		
	END       
	
COMMIT TRANSACTION        
END TRY        
BEGIN CATCH        
	IF @@TRANCOUNT > 0        
	BEGIN        
		ROLLBACK TRANSACTION;        
	END        
		SET @Status = 'FAIL';        
		SET @Message = ERROR_MESSAGE();        
		SET @RetDispatchPlanId = 0;        
	END CATCH;        
END  

GO
-- =============================================
-- Author		:		DHEERAJ KUMAR
-- Create date	:		03-July-2022
-- Description	:		This sp is used to Save Dispatch Data in Table.
-- ==============================================
CREATE PROC [dbo].[proc_AddEditDispatch]        
(        
	@DispatchID INT = 0, 
	@DispatchDate DATE,       
	@DispatchPlanID INT,  
	@CompanyBranchID INT,
	@UserID INT,
	@ApprovalStatus VARCHAR(20),
	@DispatchProductDetail udt_DispatchProductDetail readonly,  
	@Status VARCHAR(50) OUT,        
	@Message VARCHAR(500) OUT,        
	@RetDispatchId BIGINT OUT  
)        
AS
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date			Story					Developer			Description  
-- ----------- ----------------------- ------------------- -----------------------------------------------------------------------------------------
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN        
BEGIN TRY        
BEGIN TRANSACTION        
	
	DECLARE @CompanyBranchCode VARCHAR(10), @CompanyStateId INT, @DispatchNo VARCHAR(50), @MaxDispatchNo INT

	DECLARE @TempProduct AS TABLE
	(    
		RowId INT,  
		SOId INT, 
		ProductId BIGINT,    
		Quantity DECIMAL(18,2)    
	)
	
	SELECT @CompanyBranchCode = CB.CompanyBranchCode FROM ComapnyBranch CB WHERE CB.CompanyBranchId = @CompanyBranchId  

	--INSERT START 
	
	IF @DispatchID = 0          
	BEGIN                  
		SELECT @MaxDispatchNo = MAX(DispatchSequence) FROM Dispatch WHERE CompanyBranchID = @CompanyBranchId
		
		IF ISNULL(@MaxDispatchNo , 0) <> 0        
		BEGIN        
			SET @MaxDispatchNo = @MaxDispatchNo + 1;        
		END        
		ELSE        
		BEGIN        
			SET @MaxDispatchNo = 1;        
		END        

		SET @DispatchNo = @CompanyBranchCode + '/DP/'+ FORMAT(@MaxDispatchNo , '000#')
		
		INSERT INTO Dispatch (DispatchNo, DispatchDate, DispatchPlanID, CompanyBranchID, DispatchSequence, CreatedBy , CreatedDate, ApprovalStatus ) 
		VALUES(@DispatchNo, @DispatchDate, @DispatchPlanID, @CompanyBranchID, @MaxDispatchNo, @UserID, GETDATE(), @ApprovalStatus)
        

		SET @RetDispatchId = SCOPE_IDENTITY();         

		INSERT INTO DispatchProductDetail(DispatchID, SOId, ProductId, Quantity, Priority)        
		SELECT @RetDispatchId, SOId, ProductId, Quantity, Priority from @DispatchProductDetail 


		SET @Message = '';        
		SET @Status = 'SUCCESS';        
         
	END  
	
	ELSE      
	BEGIN    
		UPDATE Dispatch SET DispatchDate = @DispatchDate,
								DispatchPlanID = @DispatchPlanID,
								CompanyBranchID = @CompanyBranchID,
								ModifiedBy = @UserID,
								ModifiedDate = GETDATE(),
								ApprovalStatus = @ApprovalStatus
								WHERE DispatchID = @DispatchID

		DELETE FROM DispatchProductDetail WHERE DispatchID = @DispatchID  
		
		INSERT INTO DispatchProductDetail(DispatchID, SOId, ProductId, Quantity, Priority)        
		SELECT @DispatchID, SOId, ProductId, Quantity, Priority from @DispatchProductDetail

		SET @Message = '';        
		SET @Status = 'SUCCESS';        
		SET @RetDispatchId = @DispatchID; 
		
	END       
	
COMMIT TRANSACTION        
END TRY        
BEGIN CATCH        
	IF @@TRANCOUNT > 0        
	BEGIN        
		ROLLBACK TRANSACTION;        
	END        
		SET @Status = 'FAIL';        
		SET @Message = ERROR_MESSAGE();        
		SET @RetDispatchId = 0;        
	END CATCH;        
END  


GO
-- =============================================
-- Author		:		Dheeraj Kumar
-- Create date	:		26 Jun 2022
-- Description	:		This sp is used to get sale order.
-- ==============================================
-- EXECUTE proc_GetSOList 1,'', '', '2017-01-01', '2022-06-30', 1
CREATE proc [dbo].[proc_GetSOList]
(
@CustomerID INT = 0, 
@SONO VARCHAR(50) = NULL,                     
@QuotationNo VARCHAR(50) = NULL,                        
@FromDate DATE, 
@ToDate DATE,    
@CompanyBranchId INT = 0                                      
)                      
AS     
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date		Story					Developer			Description  
-- ----------- ----------------------- ------------------- ----------------------------------------------------------------------------------------- 
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN                      
	SET NOCOUNT ON;                                        
	DECLARE @strSql AS NVARCHAR(4000);     
	
	SET @strSql = 'SELECT SO.SOId, SO.SONo, 
	REPLACE(CONVERT(VARCHAR, SO.SODate, 106),'''',''-'') SODate, 
	SO.QuotationNo FROM SO AS SO
	INNER JOIN Customer AS C ON SO.CustomerId = C.CustomerId
	INNER JOIN ComapnyBranch AS CB ON SO.CompanyBranchId = CB.CompanyBranchId
	WHERE SO.ApprovalStatus = ''Final''';    
                     
	SET @strSql = @strSql + ' AND  SO.SODate BETWEEN '''+ cast(@FromDate as varchar) + ''' AND  '''+ cast(@ToDate as varchar) + ''' ';                      
 
	IF @CustomerID <> '0' and @CustomerID <> ''                  
	BEGIN                  
		SET @strSql = @strSql + ' AND C.CustomerId =''' + CAST(@CustomerID AS VARCHAR) + '''';                  
	END 
 
	IF (ISNULL(@SONO,'') <> '' )                     
	BEGIN                      
		SET @strSql = @strSql + ' AND  SO.SONo like ''%' + @SONO + '%'' ';                       
	END

	IF (ISNULL(@QuotationNo,'') <> '' )                     
	BEGIN                      
		SET @strSql = @strSql + ' AND  SO.QuotationNo like ''%' + @QuotationNo + '%'' ';                       
	END
                          
	IF @CompanyBranchId <> '0' and @CompanyBranchId <> ''                  
	BEGIN                  
		SET @strSql = @strSql + ' AND SO.CompanyBranchId =''' + CAST(@CompanyBranchId AS VARCHAR) + '''';                  
	END          
                       
	SET @strSql = @strSql + ' ORDER BY SO.SONo ASC ';  
                 
	EXECUTE SP_EXECUTESQL @strSql                      
                      
	SET NOCOUNT OFF;  
	
END                  

GO
-- =============================================
-- Author		:		Dheeraj Kumar
-- Create date	:		26 Jun 2022
-- Description	:		This sp is used to get sale order.
-- ==============================================
--EXECUTE proc_GetCustomerSOProductList '1,2',0
CREATE PROC [dbo].[proc_GetCustomerSOProductList]    
(                
@SOIds VARCHAR(200),
@IsDispatchPlan BIT
)                
AS                
BEGIN                
	SET NOCOUNT ON;     
	
	IF(@IsDispatchPlan = 0)
	BEGIN
		SELECT C.CustomerName, C.City, SO.SOId, SO.SONo, SPD.ProductId, P.ProductName, P.ProductCode, 
		ISNULL(SPD.Quantity, 0) AS Quantity,
		CAST(0 AS DECIMAL(18, 2)) AS Priority
		FROM SO AS SO
		INNER JOIN SOProductDetail AS SPD ON SO.SOId = SPD.SOId
		INNER JOIN Product AS P ON SPD.ProductId = P.Productid
		INNER JOIN Customer AS C ON SO.CustomerId = C.CustomerId
		WHERE SO.SOId IN (SELECT Val FROM [dbo].Splitstr(@SOIds,','))
	END
	ELSE
	BEGIN
		SELECT C.CustomerName, C.City, SO.SOId, SO.SONo, DPPD.ProductId, P.ProductName, P.ProductCode, 
		ISNULL(DPPD.Quantity, 0) AS Quantity ,
		CAST(DPPD.Priority AS DECIMAL(18, 2)) AS Priority
		FROM DispatchPlanProductDetail AS DPPD
		INNER JOIN SO AS SO ON DPPD.SOId = SO.SOId
		INNER JOIN Product AS P ON DPPD.ProductId = P.Productid
		INNER JOIN Customer AS C ON SO.CustomerId = C.CustomerId
		WHERE DPPD.DispatchPlanID IN (SELECT Val FROM [dbo].Splitstr(@SOIds,',')) 
	END
	
	          
	SET NOCOUNT OFF;                
END            
 GO

 GO

-- =============================================
-- Author		:		DHEERAJ KUMAR
-- Create date	:		27-Jun-2022
-- Description	:		This sp is used to get Dispatch Plan Detail
-- ==============================================
-- EXECUTE [dbo].[proc_GetDispatchPlan] '', '', 0 , '2017-01-01', '2022-06-30',''
CREATE PROC [dbo].[proc_GetDispatchPlanForDispatch] 
( 
@DispatchPlanNo VARCHAR(50) = '',                                
@CustomerName VARCHAR(50) = '', 
@CompanyBranchId  INT = 0  ,
@FromDate DATE, 
@ToDate DATE,
@ApprovalStatus	VARCHAR(20) = ''
)                    
AS   
-- -------------------------------------------------------- History -------------------------------------------------------------------------------- 
--    Date			Story					Developer			Description  
-- ----------- ----------------------- ------------------- -----------------------------------------------------------------------------------------
-- /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
BEGIN                    
	SET NOCOUNT ON;                    
	DECLARE @STRSQL AS NVARCHAR(4000);
	
	SET @STRSQL = 'SELECT DP.DispatchPlanID, DP.DispatchPlanNo , 
	REPLACE(CONVERT(VARCHAR, DP.DispatchPlanDate, 106),'''',''-'') AS DispatchPlanDate,
	C.CustomerId, C.CustomerName, CB.BranchName ,DP.ApprovalStatus
	FROM DispatchPlan AS DP
	INNER JOIN Customer AS C ON DP.CustomerID = C.CustomerId
	INNER JOIN ComapnyBranch AS CB ON DP.CompanyBranchID = CB.CompanyBranchId
	WHERE 1 = 1'        
	
	SET @STRSQL = @STRSQL + ' AND DP.DispatchPlanDate BETWEEN '''+ CAST(@FromDate AS VARCHAR) + ''' AND  '''+ CAST(@ToDate AS VARCHAR) + ''' ';     

          
	IF @DispatchPlanNo <> ''                    
	BEGIN                    
		SET @STRSQL = @STRSQL + ' AND DP.DispatchPlanNo LIKE ''%' + @DispatchPlanNo + '%''';                    
	END 

	IF @CustomerName <> ''                    
	BEGIN                    
		SET @STRSQL = @STRSQL + ' AND C.CustomerName LIKE ''%' + @CustomerName + '%''';                    
	END 
     
	IF @CompanyBranchId <> 0                  
	BEGIN                  
		SET @STRSQL = @STRSQL + ' AND DP.CompanyBranchID =' + CAST(@CompanyBranchId AS VARCHAR) + '';                  
	END  

	IF @ApprovalStatus <> '' AND @ApprovalStatus <> '0'                  
	BEGIN                  
		SET @STRSQL = @STRSQL + ' AND DP.ApprovalStatus =''' + @ApprovalStatus + '''';                  
	END  
            
	SET @STRSQL = @STRSQL + ' ORDER BY DP.DispatchPlanNo DESC '

	PRINT @STRSQL
	EXECUTE SP_EXECUTESQL @STRSQL    
                  
	SET NOCOUNT OFF;  
	
END 




















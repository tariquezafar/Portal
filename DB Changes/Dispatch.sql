USE [Demo]
GO
/****** Object:  StoredProcedure [dbo].[proc_GetDispatchs]    Script Date: 6/29/2022 6:14:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- proc_GetDispatchs '','','2021-04-01','2022-03-31',1,0
  
ALTER proc [dbo].[proc_GetDispatchs]
(                      
@DispatchNo varchar(50),                     
@CustomerName varchar(50),                    
@FromDate date, 
@ToDate date,
@CompanyBranchId int=0,                                                            
@CustomerId int =0
)                      
as                      
begin                      
set nocount on;                      
                      
declare @strSql as nvarchar(4000);                   
set @strSql='select E.DispatchPlanID,                  
E.DispatchPlanNo,           
replace(convert(varchar, E.DispatchPlanDate,106),'' '',''-'') DispatchPlanDate,                          
E.CustomerId,            
c.CustomerCode,   
CB.BranchName,            
c.CustomerName,               
c.City,                   
St.StateName,                                                                   
E.CreatedBy,                                              
replace(convert(varchar, E.CreatedDate,106),'' '',''-'') CreatedDate,                      
E.ModifiedBy,                                         
case when E.ModifiedDate is null then '''' else replace(convert(varchar, E.ModifiedDate,106),'' '',''-'') end ModifiedDate,
isnull(E.CompanyBranchId,0)  CompanyBranchId,
isnull(cb.BranchName,'''')  CompanyBranchName
from [DispatchPlan] E                
inner join [Customer] c on E.CustomerId = c.CustomerId                       
inner join [State] St on c.StateId=St.StateId  
inner join [ComapnyBranch] cb on E.CompanyBranchId = cb.CompanyBranchId                   
where  1=1 ';                      
set @strSql=@strSql + ' and  E.DispatchPlanDate BETWEEN '''+ cast(@FromDate as varchar) + ''' AND  '''+ cast(@ToDate as varchar) + ''' ';                      
            
if @CustomerName<>''                      
begin                      
set @strSql=@strSql + ' and  c.CustomerName like ''%' + @CustomerName + '%'' ';                      
end   
                               
if @DispatchNo<>''                      
begin                      
set @strSql=@strSql + ' and E.DispatchPlanNo like ''%' + @DispatchNo + '%''';                      
end                            
                        
set @strSql=@strSql + ' order by E.DispatchPlanDate Desc, E.DispatchPlanNo Desc ';         
--print @strSql
exec sp_executesql @strSql 
--select @strSql                     
                      
set nocount off;                      
end                  




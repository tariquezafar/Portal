ALTER TABLE [dbo].[ComplaintServiceProductDetail]
ALTER COLUMN [Quantity] INT



INSERT INTO UserInterface(InterfaceId,InterfaceName, InterfaceDescription, InterfaceType, InterfaceSubType, InterfaceURL, SequenceNo, Status, CompanyBranchId)
VALUES (12290,'Appoved CS','Complaint Service','SALE', 'MASTER','~/ComplaintService/ListAPCS',5, 1, 10004)

INSERT INTO RoleUIActionMapping(RoleId, InterfaceId, AddAccess, EditAccess, ViewAccess, Status,CancelAccess, ReviseAccess, CompanyBranchId)
VALUES(2, 12290, 1, 1, 1, 1, 0, 0, 10004)



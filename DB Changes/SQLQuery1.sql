CREATE TABLE EmployeeSupportingDocument (
EmployeeDocId INT IDENTITY(1,1) PRIMARY KEY,
EmployeeId INT FOREIGN KEY REFERENCES Employee(EmployeeId),
DocumentTypeId int NULL,
DocumentName VARCHAR(100)  NULL,
DocumentPath VARCHAR(100)  NULL
);
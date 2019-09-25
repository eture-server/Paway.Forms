
-- ----------------------------
-- Table structure for Users
-- Date: 2015-11-11
-- ----------------------------
CREATE TABLE [Users](
"Id"  integer Primary Key AutoIncrement not null,
"ParentId" integer,
"Name"  nvarchar NULL,
"Pad"  nvarchar NULL,
"Statu"  bit,
"UserType"  int,
"Value" int,
"DateTime"  datetime,
"Image"  blob,
"CreateDate"  datetime,
 unique(Id asc)
);
GO
Create index main.Users_id on Users (Id ASC);
GO
-- ----------------------------
-- Table structure for Admins
-- Date: 2015-12-12
-- ----------------------------
CREATE TABLE [Admins](
"Id"  integer Primary Key AutoIncrement not null,
"Name"  nvarchar,
"Value"  nvarchar,
"DateTime"  datetime,
 unique(Id asc)
);
GO
Create index main.Admins_id on Admins (Id ASC);
GO
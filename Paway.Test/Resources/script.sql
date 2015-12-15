
-- ----------------------------
-- Table structure for Users
-- Date: 2015-11-11
-- ----------------------------
CREATE TABLE [Users](
"Id"  integer Primary Key AutoIncrement not null,
"Name"  nvarchar NULL,
"Pad"  nvarchar NULL,
"Status"  bit,
"IsAdmin"  bit,
"Money"  double,
"LastDate"  datetime,
"CreateDate"  datetime
);
GO
-- ----------------------------
-- Table structure for Admins
-- Date: 2015-12-12
-- ----------------------------
CREATE TABLE [Admins](
"Id"  integer Primary Key AutoIncrement not null,
"Name"  nvarchar,
"Value"  nvarchar,
"CreateDate"  datetime
);
GO
﻿USE MASTER
DROP DATABASE QLNH
CREATE DATABASE QLNH
USE QLNH

CREATE TABLE EMPLOYEE
(
	EMP_ID INT IDENTITY(1,1) CONSTRAINT pk_emp_id primary key,
	EMP_CODE char(5) NOT NULL,
	EMP_NAME nvarchar(50) NOT NULL,
	EMP_ADDR nvarchar(100),
	EMP_PHONE nvarchar(20) NOT NULL,
	EMP_CCCD nvarchar(12) NULL CONSTRAINT uq_emp_cccd UNIQUE,
	EMP_ROLE nvarchar(50),
	EMP_SALARY MONEY NOT NULL,
	ISDELETED BIT NOT NULL DEFAULT 0
);

CREATE TABLE CUSTOMER
(
	CUS_ID INT IDENTITY(1,1) CONSTRAINT pk_cus_id primary key,
	CUS_CODE char(6) NOT NULL,
	CUS_NAME nvarchar(50) NOT NULL, 
	CUS_ADDR nvarchar(100),
	CUS_PHONE nvarchar(20) CONSTRAINT uq_cus_phone UNIQUE,
	CUS_CCCD nvarchar(12),
	CUS_EMAIL nvarchar(50),
	ISDELETED BIT NOT NULL DEFAULT 0
);


CREATE TABLE DINING_TABLE
(
	TAB_ID INT IDENTITY(1,1) CONSTRAINT pk_table_id primary key,
	TAB_NUM TINYINT UNIQUE,
	TAB_STATUS BIT NOT NULL, -- BAN DA SAN SANG / CHUA SAN SANG
	ISDELETED BIT NOT NULL DEFAULT 0
);

CREATE TABLE INGREDIENTS
(
	INGRE_ID INT IDENTITY(1,1) CONSTRAINT pk_ingre_id primary key,
	INGRE_CODE char(5) NOT NULL,
	INGRE_NAME NVARCHAR(30) NOT NULL,
	INSTOCK_KG FLOAT NOT NULL,
	ISDELETED BIT NOT NULL
);


CREATE TABLE MENU_ITEMS
(
	ITEM_ID INT IDENTITY(1,1) CONSTRAINT pk_dish_id primary key,
	ITEM_CODE char(4) NOT NULL,
	ITEM_TYPE NVARCHAR(10) CHECK (ITEM_TYPE IN ('FOOD', 'DRINK', 'OTHER')) NOT NULL, -- them not null, them truong hop other
	ITEM_NAME NVARCHAR(50) NOT NULL,
	ITEM_IMG NVARCHAR(MAX),
	ITEM_CPRICE MONEY NOT NULL,
	ITEM_SPRICE MONEY NOT NULL,
	IS_AVAILABLE BIT NOT NULL,
	ISDELETED BIT NOT NULL DEFAULT 0,
);

CREATE TABLE RECIPES
(
	ITEM_ID INT,
	INGRE_ID INT,
	INGRE_QUANTITY_KG FLOAT NOT NULL,
	PRIMARY KEY (ITEM_ID, INGRE_ID),
	FOREIGN KEY (ITEM_ID) REFERENCES MENU_ITEMS(ITEM_ID) ON DELETE CASCADE,
	FOREIGN KEY (INGRE_ID) REFERENCES INGREDIENTS(INGRE_ID) ON UPDATE CASCADE
);

CREATE TABLE BOOKING
(
	BK_ID INT IDENTITY(1,1) CONSTRAINT pk_booking_id primary key,
	BK_CODE char(5) NOT NULL,
	EMP_ID INT FOREIGN KEY REFERENCES EMPLOYEE(EMP_ID) ON UPDATE CASCADE,
	CUS_ID INT FOREIGN KEY REFERENCES CUSTOMER(CUS_ID) ON DELETE CASCADE,
	TAB_ID INT FOREIGN KEY REFERENCES DINING_TABLE(TAB_ID) ON UPDATE CASCADE,
	BK_STIME DATETIME NOT NULL, -- BOOKING_START_TIME
	BK_OTIME DATETIME NOT NULL, -- BOOKING_OVER_TIME
	BK_STATUS TINYINT NOT NULL CHECK (BK_STATUS IN (0,1,2)), -- XAC NHAN / DANG CHO / BI HUY
	ISDELETED BIT NOT NULL DEFAULT 0
);

CREATE TABLE RECEIPT
(
	REC_ID INT IDENTITY(1,1) CONSTRAINT pk_receipt_id primary key,
	REC_CODE CHAR(8) NOT NULL,
	EMP_ID INT FOREIGN KEY REFERENCES EMPLOYEE(EMP_ID) ON UPDATE CASCADE,
	CUS_ID INT FOREIGN KEY REFERENCES CUSTOMER(CUS_ID) ON DELETE CASCADE,
	TAB_ID INT FOREIGN KEY REFERENCES DINING_TABLE(TAB_ID) ON UPDATE CASCADE,
	REC_TIME DATETIME NOT NULL,
	REC_PAY MONEY NOT NULL,
	ISDELETED BIT NOT NULL DEFAULT 0
);

CREATE TABLE RECEIPT_DETAILS
(
    REC_ID INT, 
    ITEM_ID INT,
	QUANTITY INT NOT NULL,
	PRICE MONEY NOT NULL,
	PRIMARY KEY(REC_ID, ITEM_ID),
	FOREIGN KEY (REC_ID) REFERENCES RECEIPT(REC_ID) ON DELETE CASCADE,
	FOREIGN KEY (ITEM_ID) REFERENCES MENU_ITEMS(ITEM_ID) ON UPDATE CASCADE
);

CREATE TABLE STOCKIN
(
	STO_ID INT IDENTITY(1,1) CONSTRAINT pk_stockin_id primary key,
	STO_CODE CHAR(8) NOT NULL,
	STO_DATE DATETIME NOT NULL,
	STO_PRICE MONEY NOT NULL
);

CREATE TABLE STOCKIN_DETAILS_INGRE
(
	STO_ID INT,
	INGRE_ID INT,
	QUANTITY_KG FLOAT NOT NULL,
	CPRICE MONEY NOT NULL,
	PRIMARY KEY (STO_ID, INGRE_ID),
	FOREIGN KEY (STO_ID) REFERENCES STOCKIN(STO_ID) ON DELETE CASCADE,
	FOREIGN KEY (INGRE_ID) REFERENCES INGREDIENTS(INGRE_ID) ON UPDATE CASCADE
);

CREATE TABLE STOCKIN_DETAILS_DRINK_OTHER
(
	STO_ID INT,
	ITEM_ID INT,
	QUANTITY_UNITS INT NOT NULL,
	CPRICE MONEY NOT NULL,
	PRIMARY KEY (STO_ID, ITEM_ID),
	FOREIGN KEY (STO_ID) REFERENCES STOCKIN(STO_ID) ON DELETE CASCADE,
	FOREIGN KEY (ITEM_ID) REFERENCES MENU_ITEMS(ITEM_ID) ON UPDATE CASCADE
);

CREATE TABLE FINANCIAL_HISTORY (
    FIN_ID INT IDENTITY(1,1) PRIMARY KEY,
    FIN_DATE DATETIME NOT NULL,
    DESCRIPTION NVARCHAR(255),
    TYPE NVARCHAR(10) CHECK (TYPE IN ('INCOME', 'EXPENSE', 'PROFIT')),
    AMOUNT MONEY NOT NULL,
    REFERENCE_ID INT NULL, -- ID liên kết với bảng RECEIPT hoặc STOCKIN nếu cần
    REFERENCE_TYPE NVARCHAR(20) NULL -- Loại liên kết ('RECEIPT', 'STOCKIN', 'OTHER')
);

CREATE TABLE ACCOUNT_ROLE
(
	ROLE_ID INT IDENTITY(1,1) CONSTRAINT pk_id primary key,
	ROLE_NAME NVARCHAR(MAX)
);

CREATE TABLE ACCOUNT
(
	ACC_USERNAME NVARCHAR(100) CONSTRAINT pk_username PRIMARY KEY,
	ACC_PASSWORD NVARCHAR(100) NOT NULL,
	ACC_EMAIL NVARCHAR(100) CONSTRAINT uq_acc_email UNIQUE,
	ACC_DISPLAYNAME NVARCHAR(100) NOT NULL,
	ACC_GENDER NVARCHAR(5) NOT NULL,
	ACC_BDAY DATE NOT NULL,
	ACC_ADDRESS NVARCHAR(100) NOT NULL,
	ACC_PHONE NVARCHAR(20) NOT NULL,
	ROLE_ID INT FOREIGN KEY REFERENCES ACCOUNT_ROLE(ROLE_ID) ON DELETE CASCADE,

	ISDELETED BIT NOT NULL DEFAULT 0
);


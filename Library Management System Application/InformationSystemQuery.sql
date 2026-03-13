CREATE TABLE BOOK(
BookID INT PRIMARY KEY IDENTITY(1,1),
Title VARCHAR(20) NOT NULL,
Author VARCHAR(20) NOT NULL,
ISBN VARCHAR(20) NOT NULL,
Publisher VARCHAR(20) NOT NULL,
Year DATE NOT NULL,
CopiesAvailable VARCHAR(20) NOT NULL,
);

CREATE TABLE MEMBER(
MemberID INT PRIMARY KEY IDENTITY(1,1),
FirstName Varchar(20) NOT NULL,
LastName Varchar(20) NOT NULL,
Email Varchar (30) NOT NULL,
PhoneNumber Varchar (15) NOT NULL,
MembershipDate DATE,
);

CREATE TABLE BORROWING(
BorrowingID INT PRIMARY KEY IDENTITY(1,1),
BookID INT,
MemberID INT,
BorrowDate DATE NOT NULL,
DueDate DATE NOT NULL,
ReturnDate DATE,
CONSTRAINT FK_BOKO FOREIGN KEY (BookID) References BOOK(BookID),
CONSTRAINT FK_MEMO FOREIGN KEY (MemberID) References MEMBER(MemberID),
);

CREATE TABLE ENDUSER(
UserID INT PRIMARY KEY IDENTITY(1,1),
Username VARCHAR(20) NOT NULL,
Password VARCHAR(20) NOT NULL,
Role VARCHAR(10) NOT NULL,
);

CREATE TABLE BOOKRETURN(
ReturnID INT PRIMARY KEY IDENTITY (1,1),
BookID INT,
MemberID INT,
Email VARCHAR(20) NOT NULL,
BorrowDate DATE,
DueDate DATE,
ReturnDate DATE,
CONSTRAINT FK_BOKOS FOREIGN KEY (BookID) References BOOK(BookID),
CONSTRAINT FK_MEMOS FOREIGN KEY (MemberID) References MEMBER(MemberID),

);

CREATE TABLE MemberBorrowing(
BorrowingID INT PRIMARY KEY IDENTITY (1,1),
BookID INT,
MemberID INT,
BorrowDate DATE NOT NULL,
DueDate DATE NOT NULL,
ReturnDate DATE,
CONSTRAINT FK_BOKOZ FOREIGN KEY (BookID) References BOOK(BookID),
CONSTRAINT FK_MEMOZ FOREIGN KEY (MemberID) References MEMBER(MemberID),
);

SELECT * FROM BOOKRETURN;

INSERT INTO ENDUSER (Username, Password, Role) VALUES ('Chikontwe', 'password123','Admin');
INSERT INTO ENDUSER (Username, Password, Role) VALUES ('Mike', 'password456','Member');
INSERT INTO ENDUSER(Username, Password, Role) VALUES ('James', 'password789','Member');
INSERT INTO ENDUSER(Username, Password, Role) VALUES ('Timothy', 'password117','Admin');
INSERT INTO ENDUSER(Username, Password, Role) VALUES ('Samuel', 'password111','Admin');
INSERT INTO ENDUSER(Username, Password, Role) VALUES ('Salifya', 'password112','Admin');

SELECT * FROM ENDUSER;
SELECT * FROM BOOK;
SELECT * FROM MEMBER;
SELECT * FROM BORROWING;
SELECT * FROM MemberBorrowing;
SELECT * FROM BOOKRETURN;



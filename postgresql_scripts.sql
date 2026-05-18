-- ENUM Types

CREATE TYPE membership_type_enum AS ENUM
(
    'Basic',
    'Student',
    'Premium'
);

CREATE TYPE book_copy_status_enum AS ENUM
(
    'Available',
    'Borrowed',
    'Damaged',
    'Lost',
    'Maintenance'
);

CREATE TYPE borrow_status_enum AS ENUM
(
    'Borrowed',
    'Returned',
    'Overdue',
    'Lost'
);




--Tabels

CREATE TABLE MembershipTypes
(
    MembershipTypeId SERIAL PRIMARY KEY,
    TypeName membership_type_enum UNIQUE NOT NULL,
    MaxBooks INT NOT NULL CHECK (MaxBooks>0),
    MaxBorrowDays INT NOT NULL CHECK (MaxBorrowDays>0),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


INSERT INTO MembershipTypes(TypeName, MaxBooks, MaxBorrowDays) VALUES ('Basic',2,7), ('Student',3,10), ('Premium',5,15);

SELECT * FROM MembershipTypes;


CREATE TABLE Members
(
    MemberId SERIAL PRIMARY KEY,
    FullName VARCHAR(150) NOT NULL,
    Email VARCHAR(150) UNIQUE NOT NULL,
    Phone VARCHAR(10) UNIQUE NOT NULL,
    Address TEXT,
    MembershipTypeId INT NOT NULL,
    IsActive BOOLEAN DEFAULT TRUE,
    JoinedDate DATE DEFAULT CURRENT_DATE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	
    CONSTRAINT FK_Members_MembershipTypes
    FOREIGN KEY(MembershipTypeId)
    REFERENCES MembershipTypes(MembershipTypeId)
);



CREATE TABLE BookCategories
(
    CategoryId SERIAL PRIMARY KEY,
    CategoryName VARCHAR(150) UNIQUE NOT NULL,
    Description TEXT
);

CREATE TABLE Authors
(
    AuthorId SERIAL PRIMARY KEY,
    AuthorName VARCHAR(150) NOT NULL
);


CREATE TABLE Books
(
    BookId SERIAL PRIMARY KEY,
    Title VARCHAR(150) NOT NULL,
    ISBN VARCHAR(30) UNIQUE NOT NULL,
    PublishedYear INT,
    CategoryId INT NOT NULL,
    Publisher VARCHAR(150),
    Description TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_Books_Categories
    FOREIGN KEY(CategoryId)
    REFERENCES BookCategories(CategoryId)
);


CREATE TABLE BookAuthors
(
    BookId INT NOT NULL,
    AuthorId INT NOT NULL,

    PRIMARY KEY(BookId, AuthorId),

    CONSTRAINT FK_BookAuthors_Books
    FOREIGN KEY(BookId)
    REFERENCES Books(BookId)
    ON DELETE CASCADE,

    CONSTRAINT FK_BookAuthors_Authors
    FOREIGN KEY(AuthorId)
    REFERENCES Authors(AuthorId)
    ON DELETE CASCADE
);

SELECT * FROM BookAuthors;


CREATE TABLE BookCopies
(
    CopyId SERIAL PRIMARY KEY,
    BookId INT NOT NULL,
    CopyCode VARCHAR(50) UNIQUE NOT NULL,
    Status book_copy_status_enum DEFAULT 'Available',
    ShelfLocation VARCHAR(50),
    IsAvailable BOOLEAN DEFAULT TRUE,
    IsDamaged BOOLEAN DEFAULT FALSE,
    AddedDate DATE DEFAULT CURRENT_DATE,
	

    CONSTRAINT FK_BookCopies_Books
    FOREIGN KEY(BookId)
    REFERENCES Books(BookId)
);



CREATE TABLE BorrowTransactions
(
    BorrowId SERIAL PRIMARY KEY,
    MemberId INT NOT NULL,
    CopyId INT NOT NULL,
    BorrowDate DATE DEFAULT CURRENT_DATE,
    DueDate DATE NOT NULL,
    ReturnDate DATE,
    Status borrow_status_enum DEFAULT 'Borrowed',
    Remarks TEXT,

    CONSTRAINT FK_Borrow_Members
    FOREIGN KEY(MemberId)
    REFERENCES Members(MemberId),

    CONSTRAINT FK_Borrow_BookCopies
    FOREIGN KEY(CopyId)
    REFERENCES BookCopies(CopyId),

    CONSTRAINT CHK_ReturnDate
    CHECK( ReturnDate IS NULL OR ReturnDate >= BorrowDate)
);


CREATE TABLE Fines
(
    FineId SERIAL PRIMARY KEY,
    BorrowId INT UNIQUE NOT NULL,
    MemberId INT NOT NULL,
    FineAmount DECIMAL(10,2) NOT NULL CHECK(FineAmount >= 0),
    PaidAmount DECIMAL(10,2) DEFAULT 0 CHECK(PaidAmount >= 0),
    PendingAmount DECIMAL(10,2) GENERATED ALWAYS AS (FineAmount-PaidAmount) STORED,
    FineReason TEXT,
    IsPaid BOOLEAN DEFAULT FALSE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_Fines_Borrow
    FOREIGN KEY(BorrowId)
    REFERENCES BorrowTransactions(BorrowId),

    CONSTRAINT FK_Fines_Members
    FOREIGN KEY(MemberId)
    REFERENCES Members(MemberId),

    CONSTRAINT CHK_PaidAmount
    CHECK (PaidAmount <= FineAmount)
);


CREATE TABLE FinePayments
(
    PaymentId SERIAL PRIMARY KEY,
    FineId INT NOT NULL,
    AmountPaid DECIMAL(10,2) NOT NULL CHECK(AmountPaid>0),
    PaymentDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PaymentMode VARCHAR(30),
    Remarks TEXT,

    CONSTRAINT FK_FinePayments_Fines
    FOREIGN KEY(FineId)
    REFERENCES Fines(FineId)
);



CREATE TABLE UserAccounts
(
    UserId SERIAL PRIMARY KEY,
    Username VARCHAR(100) UNIQUE NOT NULL,
    PasswordHash TEXT NOT NULL,
    Role VARCHAR(20) NOT NULL,
    MemberId INT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_UserAccounts_Members
    FOREIGN KEY(MemberId)
    REFERENCES Members(MemberId)
    ON DELETE SET NULL
);





SELECT * FROM  Members;


SELECT * FROM bookcategories;

SELECT * FROM books;






SELECT unnest(enum_range(NULL::book_copy_status_enum));


ALTER TABLE bookcopies
ALTER COLUMN status DROP DEFAULT;



ALTER TABLE bookcopies
ALTER COLUMN status TYPE text;


DROP TYPE book_copy_status_enum;


CREATE TYPE book_copy_status_enum AS ENUM
(
    'available',
    'borrowed',
    'damaged'
);


ALTER TABLE bookcopies
ALTER COLUMN status TYPE book_copy_status_enum
USING status::book_copy_status_enum;



SELECT * FROM bookcopies;





ALTER TABLE borrowtransactions
ALTER COLUMN status DROP DEFAULT;

ALTER TABLE borrowtransactions
ALTER COLUMN status TYPE text;

DROP TYPE borrow_status_enum;

CREATE TYPE borrow_status_enum AS ENUM
(
    'borrowed',
    'returned',
    'overdue',
    'lost'
);

ALTER TABLE borrowtransactions
ALTER COLUMN status TYPE borrow_status_enum
USING lower(status)::borrow_status_enum;

SELECT * FROM  books;


SELECT table_name
FROM information_schema.tables
WHERE table_schema = 'public'
ORDER BY table_name;

SELECT * FROM borrowtransactions;
SELECT * FROM bookcopies;












-- =========================================
-- BOOK CATEGORIES
-- =========================================

INSERT INTO BookCategories(CategoryName, Description)
VALUES
('Programming', 'Books related to coding and software development'),
('Database', 'Database management and SQL books'),
('Artificial Intelligence', 'AI and Machine Learning books'),
('Networking', 'Computer networking concepts'),
('Web Development', 'Frontend and backend development'),
('Data Science', 'Analytics and data processing'),
('Cyber Security', 'Security and ethical hacking'),
('Operating Systems', 'OS concepts and internals');



-- =========================================
-- AUTHORS
-- =========================================

INSERT INTO Authors(AuthorName)
VALUES
('Robert C. Martin'),
('Herbert Schildt'),
('Andrew S. Tanenbaum'),
('Thomas H. Cormen'),
('Ian Goodfellow'),
('Al Sweigart'),
('Bjarne Stroustrup'),
('Dennis Ritchie'),
('James Gosling'),
('Guido van Rossum');



-- =========================================
-- MEMBERS
-- =========================================

INSERT INTO Members
(
    FullName,
    Email,
    Phone,
    Address,
    MembershipTypeId
)
VALUES
('Aayush Sharma', 'aayush1@gmail.com', '9876543210', 'Guntur', 1),
('Renu Kumar', 'renu@gmail.com', '9876543211', 'Vijayawada', 2),
('Rahul Verma', 'rahul@gmail.com', '9876543212', 'Hyderabad', 3),
('Sneha Reddy', 'sneha@gmail.com', '9876543213', 'Chennai', 2),
('Kiran Rao', 'kiran@gmail.com', '9876543214', 'Bangalore', 1),
('Anjali Mehta', 'anjali@gmail.com', '9876543215', 'Delhi', 3),
('Varun Teja', 'varun@gmail.com', '9876543216', 'Mumbai', 2),
('Pooja Singh', 'pooja@gmail.com', '9876543217', 'Pune', 1);



-- =========================================
-- BOOKS
-- =========================================

INSERT INTO Books
(
    Title,
    ISBN,
    PublishedYear,
    CategoryId,
    Publisher,
    Description
)
VALUES
('Clean Code', 'ISBN1001', 2008, 1, 'Prentice Hall', 'Guide to writing clean code'),
('Java Complete Reference', 'ISBN1002', 2020, 1, 'McGraw Hill', 'Complete Java guide'),
('Computer Networks', 'ISBN1003', 2018, 4, 'Pearson', 'Networking fundamentals'),
('Introduction to Algorithms', 'ISBN1004', 2009, 1, 'MIT Press', 'Algorithm concepts'),
('Deep Learning', 'ISBN1005', 2016, 3, 'MIT Press', 'Deep learning fundamentals'),
('Automate the Boring Stuff', 'ISBN1006', 2019, 1, 'No Starch Press', 'Python automation'),
('The C Programming Language', 'ISBN1007', 1988, 1, 'Pearson', 'Classic C programming'),
('Operating System Concepts', 'ISBN1008', 2014, 8, 'Wiley', 'Operating system internals'),
('SQL for Beginners', 'ISBN1009', 2021, 2, 'Oracle Press', 'SQL basics'),
('Python for Data Analysis', 'ISBN1010', 2022, 6, 'OReilly', 'Data analysis using Python');



-- =========================================
-- BOOK AUTHORS
-- =========================================

INSERT INTO BookAuthors(BookId, AuthorId)
VALUES
(2,2),
(3,3),
(4,4),
(5,5),
(6,6),
(7,8),
(8,3),
(9,2),
(10,10);



-- =========================================
-- BOOK COPIES
-- =========================================

INSERT INTO BookCopies
(
    BookId,
    CopyCode,
    Status,
    ShelfLocation,
    IsAvailable,
    IsDamaged
)
VALUES
(2, 'JAVA001', 'available', 'A2', TRUE, FALSE),
(2, 'JAVA002', 'damaged', 'A2', FALSE, TRUE),

(3, 'NET001', 'available', 'B1', TRUE, FALSE),
(3, 'NET002', 'borrowed', 'B1', FALSE, FALSE),

(4, 'ALG001', 'available', 'B2', TRUE, FALSE),

(5, 'DL001', 'borrowed', 'C1', FALSE, FALSE),

(6, 'PY001', 'available', 'C2', TRUE, FALSE),

(7, 'C001', 'available', 'D1', TRUE, FALSE),

(8, 'OS001', 'borrowed', 'D2', FALSE, FALSE),

(9, 'SQL001', 'available', 'E1', TRUE, FALSE),

(10, 'DS001', 'available', 'E2', TRUE, FALSE);



-- =========================================
-- BORROW TRANSACTIONS
-- =========================================

INSERT INTO BorrowTransactions
(
    MemberId,
    CopyId,
    BorrowDate,
    DueDate,
    ReturnDate,
    Status,
    Remarks
)
VALUES

(5, 6, CURRENT_DATE - 5, CURRENT_DATE + 5, NULL, 'borrowed', 'Currently borrowed'),

(6, 8, CURRENT_DATE - 15, CURRENT_DATE - 5, CURRENT_DATE - 2, 'returned', 'Returned successfully'),

(7, 9, CURRENT_DATE - 20, CURRENT_DATE - 10, NULL, 'lost', 'Book lost by member');



-- =========================================
-- FINES
-- =========================================

INSERT INTO Fines
(
    BorrowId,
    MemberId,
    FineAmount,
    PaidAmount,
    FineReason,
    IsPaid
)
VALUES
(17, 5, 150.00, 50.00, 'Overdue fine', FALSE),

(19, 7, 500.00, 0.00, 'Lost book fine', FALSE);


-- =========================================
-- FINE PAYMENTS
-- =========================================

INSERT INTO FinePayments
(
    FineId,
    AmountPaid,
    PaymentMode,
    Remarks
)
VALUES
(1, 50.00, 'UPI', 'Partial payment');



-- =========================================
-- USER ACCOUNTS
-- =========================================

INSERT INTO UserAccounts
(
    Username,
    PasswordHash,
    Role,
    MemberId
)
VALUES
('admin', 'admin123', 'Admin', NULL),

('aayush', 'pass123', 'Member', 1),

('renu', 'pass123', 'Member', 5),

('rahul', 'pass123', 'Member', 6),

('sneha', 'pass123', 'Member', 7);


SELECT MemberId, FullName FROM Members;
-- =========================================
-- VERIFY DATA
-- =========================================

SELECT * FROM MembershipTypes;
SELECT * FROM Members;
SELECT * FROM BookCategories;
SELECT * FROM Authors;
SELECT * FROM Books;
SELECT * FROM BookAuthors;
SELECT * FROM BookCopies;
SELECT * FROM BorrowTransactions;
SELECT * FROM Fines;
SELECT * FROM FinePayments;
SELECT * FROM UserAccounts;


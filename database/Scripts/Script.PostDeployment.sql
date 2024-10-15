/*
CREATE TABLE Player(
    Id int PRIMARY KEY,
    OAuthId varchar(150) NOT NULL,
    Name varchar(150),
    Created datetime DEFAULT GETDATE(),
    _username varchar(150),
    _password varchar(150)
);

Seed the table with 10 records.

*/

IF NOT EXISTS(SELECT TOP 1 Id FROM Player)
INSERT INTO Player 
VALUES 
    (1, '123456', 'John Doe', '2021-01-01', 'JohnDoe', 'password'),
    (2, '123457', 'Jane Doe', '2021-01-01', 'JaneDoe', 'password'),
    (3, '123458', 'John Smith', '2021-01-01', 'JohnSmith', 'password'),
    (4, '123459', 'Jane Smith', '2021-01-01', 'JaneSmith', 'password'),
    (5, '123460', 'John Johnson', '2021-01-01', 'JohnJohnson', 'password'),
    (6, '123461', 'Jane Johnson', '2021-01-01', 'JaneJohnson', 'password'),
    (7, '123462', 'John Jackson', '2021-01-01', 'JohnJackson', 'password'),
    (8, '123463', 'Jane Jackson', '2021-01-01', 'JaneJackson', 'password'),
    (9, '123464', 'John Brown', '2021-01-01', 'JohnBrown', 'password'),
    (10, '123465', 'Jane Brown', '2021-01-01', 'JaneBrown', 'password');

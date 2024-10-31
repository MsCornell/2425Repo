IF NOT EXISTS(SELECT TOP 1 Id FROM Player)
INSERT INTO Player (OAuthId, Name, Created, _username, _password)
VALUES 
    ('123456', 'John Doe', '2021-01-01', 'JohnDoe', 'password'),
    ('123457', 'Jane Doe', '2021-01-01', 'JaneDoe', 'password'),
    ('123458', 'John Smith', '2021-01-01', 'JohnSmith', 'password'),
    ('123459', 'Jane Smith', '2021-01-01', 'JaneSmith', 'password'),
    ('123460', 'John Johnson', '2021-01-01', 'JohnJohnson', 'password'),
    ('123461', 'Jane Johnson', '2021-01-01', 'JaneJohnson', 'password'),
    ('123462', 'John Jackson', '2021-01-01', 'JohnJackson', 'password'),
    ('123463', 'Jane Jackson', '2021-01-01', 'JaneJackson', 'password'),
    ('123464', 'John Brown', '2021-01-01', 'JohnBrown', 'password'),
    ('123465', 'Jane Brown', '2021-01-01', 'JaneBrown', 'password');

CREATE TABLE Player(
    Id int PRIMARY KEY,
    OAuthId varchar(150) NOT NULL,
    Name varchar(150),
    Created datetime DEFAULT GETDATE(),
    _username varchar(150),
    _password varchar(150)
);

/*
GO

ALTER TABLE Player
	ALTER COLUMN _password 
		ADD MASKED 
			WITH (FUNCTION = 'default()');
*/
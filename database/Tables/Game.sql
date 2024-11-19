CREATE TABLE Game (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Started DATETIME2 NOT NULL,
    Ended DATETIME2,
    AiCharacter BIT NOT NULL CHECK (AiCharacter IN (0, 1)),
    PlayerId INT NOT NULL,
    PlayerCharacter VARCHAR(1) NOT NULL CHECK (PlayerCharacter IN ('X', 'O')),
    GameWinner CHAR(1) CHECK (GameWinner IN ('X', 'O', '-')),
    GameMode VARCHAR(50) NOT NULL,
    GameScore INT NOT NULL,
    FOREIGN KEY (PlayerCharacter) REFERENCES Character(CharacterName),
    FOREIGN KEY (PlayerId) REFERENCES Player(Id) ON DELETE CASCADE ON UPDATE CASCADE
);

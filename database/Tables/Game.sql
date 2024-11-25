CREATE TABLE Game (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Started DATETIME2 NOT NULL,
    Ended DATETIME2,
    AiCharacter BIT NOT NULL CONSTRAINT CHK_Game_AiCharacter CHECK (AiCharacter IN (0, 1)),
    PlayerId INT NOT NULL,
    PlayerCharacter VARCHAR(1) NOT NULL CONSTRAINT CHK_Game_PlayerCharacter CHECK (PlayerCharacter IN ('X', 'O')),
    GameWinner CHAR(1) CONSTRAINT CHK_Game_GameWinner CHECK (GameWinner IN ('X', 'O', '-')),
    GameMode VARCHAR(50) NOT NULL,
    GameScore INT NOT NULL,
    CONSTRAINT FK_Game_PlayerCharacter FOREIGN KEY (PlayerCharacter) REFERENCES Character(CharacterName),
    CONSTRAINT FK_Game_PlayerId FOREIGN KEY (PlayerId) REFERENCES Player(Id) ON DELETE CASCADE ON UPDATE CASCADE
);

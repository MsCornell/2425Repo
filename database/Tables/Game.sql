CREATE TABLE Game (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Started DATETIME,
    Ended DATETIME,
    NextBoard INT,
    NextPlayer INT,
    AiCharacter VARCHAR(255),
    PlayerId INT,
    PlayerCharacter VARCHAR(255),
    GameWinner VARCHAR(255),
    FOREIGN KEY (AiCharacter) REFERENCES Character(Character),
    FOREIGN KEY (PlayerId) REFERENCES Player(Id),
    FOREIGN KEY (PlayerCharacter) REFERENCES Character(Character)
);


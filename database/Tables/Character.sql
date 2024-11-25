CREATE TABLE Character (
    CharacterName VARCHAR(1) PRIMARY KEY NOT NULL,
    CHECK (CharacterName IN ('X', 'O'))
);


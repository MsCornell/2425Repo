CREATE TABLE Player_Audit (
    Date DATETIME,
    PlayerId INT,
    OperationId INT,
    FOREIGN KEY (PlayerId) REFERENCES Player(Id),
    FOREIGN KEY (OperationId) REFERENCES Audit_Operation(Id)
);
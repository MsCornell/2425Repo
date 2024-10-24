CREATE TABLE [dbo].[Score]
(
  [Id] INT IDENTITY(1, 1) PRIMARY KEY,
  [Date] DATETIME DEFAULT GETDATE(),
  [PlayerId] INT NOT NULL,
  [Score] INT NOT NULL
)

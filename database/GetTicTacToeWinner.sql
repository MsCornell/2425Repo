CREATE FUNCTION dbo.GetTicTacToeWinner( 
    @Cell1 CHAR(1), 
    @Cell2 CHAR(1), 
    @Cell3 CHAR(1), 
    @Cell4 CHAR(1), 
    @Cell5 CHAR(1), 
    @Cell6 CHAR(1), 
    @Cell7 CHAR(1), 
    @Cell8 CHAR(1), 
    @Cell9 CHAR(1)
)
RETURNS CHAR(1)
AS
BEGIN
    RETURN CASE
  
        -- Winning combinations for 'X'
        WHEN @Cell1 + @Cell2 + @Cell3 = 'XXX' THEN 'X'  -- Top row
        WHEN @Cell4 + @Cell5 + @Cell6 = 'XXX' THEN 'X'  -- Middle row
        WHEN @Cell7 + @Cell8 + @Cell9 = 'XXX' THEN 'X'  -- Bottom row
        WHEN @Cell1 + @Cell4 + @Cell7 = 'XXX' THEN 'X'  -- Left column
        WHEN @Cell2 + @Cell5 + @Cell8 = 'XXX' THEN 'X'  -- Middle column
        WHEN @Cell3 + @Cell6 + @Cell9 = 'XXX' THEN 'X'  -- Right column
        WHEN @Cell1 + @Cell5 + @Cell9 = 'XXX' THEN 'X'  -- Diagonal from top-left to bottom-right
        WHEN @Cell3 + @Cell5 + @Cell7 = 'XXX' THEN 'X'  -- Diagonal from top-right to bottom-left

        -- Winning combinations for 'O'
        WHEN @Cell1 + @Cell2 + @Cell3 = 'OOO' THEN 'O'  -- Top row
        WHEN @Cell4 + @Cell5 + @Cell6 = 'OOO' THEN 'O'  -- Middle row
        WHEN @Cell7 + @Cell8 + @Cell9 = 'OOO' THEN 'O'  -- Bottom row
        WHEN @Cell1 + @Cell4 + @Cell7 = 'OOO' THEN 'O'  -- Left column
        WHEN @Cell2 + @Cell5 + @Cell8 = 'OOO' THEN 'O'  -- Middle column
        WHEN @Cell3 + @Cell6 + @Cell9 = 'OOO' THEN 'O'  -- Right column
        WHEN @Cell1 + @Cell5 + @Cell9 = 'OOO' THEN 'O'  -- Diagonal from top-left to bottom-right
        WHEN @Cell3 + @Cell5 + @Cell7 = 'OOO' THEN 'O'  -- Diagonal from top-right to bottom-left

        -- Check for a draw (Cat's game)
        WHEN @Cell1 + @Cell2 + @Cell3 + @Cell4 + @Cell5 + @Cell6 + @Cell7 + @Cell8 + @Cell9 IS NOT NULL THEN 'C'
        
        -- Game is still in progress
        ELSE NULL  
  
    END;
END;

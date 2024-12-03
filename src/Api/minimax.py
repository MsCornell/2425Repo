import math

# Define the board size and players
EMPTY = 0
PLAYER_X = 1  # human player(minimizer)
PLAYER_O = -1 # AI player(maximizer)

class TicTacToe:
    def __init__(self, board_state=[[EMPTY for _ in range(3)] for _ in range(3)]):
        self.board = board_state
        self.current_player = PLAYER_X

    def is_winner(self, player):
        """
        This function checks rows, columns and diagonals for a win.
        """
        for row in self.board:
            if all(cell == player for cell in row):
                return True
        for col in range(3):
            if all(self.board[row][col] == player for row in range(3)):
                return True
        if all(self.board[i][i] == player for i in range(3)) or all(self.board[i][2-i] == player for i in range(3)):
            return True
        return False

    def is_full(self):
        return all(cell != EMPTY for row in self.board for cell in row)

    def minimax(self, maximizing_player):
        """
        This function rolls out the minimax algorithm.
        """
        if self.is_winner(PLAYER_X):
            return -1  # X is the human player (minimizer)
        if self.is_winner(PLAYER_O):
            return 1  # O is the AI player (maximizer)
        if self.is_full():
            return 0  # Draw

        if maximizing_player:
            max_eval = -math.inf
            for i in range(3):
                for j in range(3):
                    if self.board[i][j] == EMPTY:
                        self.board[i][j] = PLAYER_O
                        eval = self.minimax(False)
                        self.board[i][j] = EMPTY
                        max_eval = max(max_eval, eval)
            return max_eval
        else:
            min_eval = math.inf
            for i in range(3):
                for j in range(3):
                    if self.board[i][j] == EMPTY:
                        self.board[i][j] = PLAYER_X
                        eval = self.minimax(True)
                        self.board[i][j] = EMPTY
                        min_eval = min(min_eval, eval)
            return min_eval

    def best_move(self):
        best_eval = -math.inf
        move = (-1, -1)
        for i in range(3):
            for j in range(3):
                if self.board[i][j] == EMPTY:
                    self.board[i][j] = PLAYER_O
                    eval = self.minimax(False)
                    self.board[i][j] = EMPTY
                    if eval > best_eval:
                        best_eval = eval
                        move = (i, j)
        return move

    def play_game(self):
        while True:
            print("Current board:")
            for row in self.board:
                print(row)
            if self.is_winner(PLAYER_X):
                print("Player X wins!")
                break
            if self.is_winner(PLAYER_O):
                print("Player O wins!")
                break
            if self.is_full():
                print("It's a draw!")
                break

            # Player X move (input)
            x, y = map(int, input("Enter your move (row and column): ").split())
            if self.board[x][y] == EMPTY:
                self.board[x][y] = PLAYER_X
            else:
                print("Invalid move! Try again.")
                continue

            if self.is_winner(PLAYER_X):
                print("Player X wins!")
                break
            if self.is_full():
                print("It's a draw!")
                break

            # Player O move (AI)
            print("AI is making a move...")
            move = self.best_move()
            if move != (-1, -1):
                self.board[move[0]][move[1]] = PLAYER_O

# To play the game
if __name__ == "__main__":
    game = TicTacToe()
    game.play_game()
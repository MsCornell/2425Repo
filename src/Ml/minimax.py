import math

# Define the board size and players
EMPTY = 0
PLAYER_X = 1  # human player(minimizer)
PLAYER_O = -1 # AI player(maximizer)

class TicTacToe:
    def __init__(self, board):
        self.board = board
        self.current_player = PLAYER_X

    def minimax(self, maximizing_player):
        """
        This function rolls out the minimax algorithm.
        """
        if maximizing_player:
            max_eval = -math.inf
            for i in range(3):
                for j in range(3):
                    if self.board[i][j] == EMPTY:
                        self.board[i][j] = PLAYER_X
                        eval = self.minimax(False)
                        self.board[i][j] = EMPTY
                        max_eval = max(max_eval, eval)
            return max_eval
        else:
            min_eval = math.inf
            for i in range(3):
                for j in range(3):
                    if self.board[i][j] == EMPTY:
                        self.board[i][j] = PLAYER_O
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
                    eval = self.minimax(True)
                    self.board[i][j] = EMPTY
                    if eval > best_eval:
                        best_eval = eval
                        move = (i, j)
        return move

    # def play_game(self):
    #     while True:
    #         # Player X move (input)
    #         # x, y = map(int, input("Enter your move (row and column): ").split())
    #         # if self.board[x][y] == EMPTY:
    #         #     self.board[x][y] = PLAYER_X
    #         # else:
    #         #     print("Invalid move! Try again.")
    #         #     continue

    #         # if self.is_winner(PLAYER_X):
    #         #     print("Player X wins!")
    #         #     break
    #         # if self.is_full():
    #         #     print("It's a draw!")
    #         #     break

    #         # Player O move (AI)
    #         move = self.best_move()
    #         # if move != (-1, -1):
    #         #     self.board[move[0]][move[1]] = PLAYER_O
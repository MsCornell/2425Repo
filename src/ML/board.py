import numpy as np

class Board():
    """
    Board Class
    """
    def __init__(self):
        self.state = np.zeros((3, 3))
        self.next_turn = None

    
    def get_value():
        """
        Get the values(X:1/O:-1) across the board when called. AI will be the X player.
        """
        return self.__state
    

    def playout(self):
        pass
    
    def get_board_as_string(self):
        """
        This function prints the board.
        """
        rows, cols = self.state.shape
        board_as_string = "-------\n"
        for r in range(rows):
            for c in range(cols):
                move = get_symbol(self.__state[r, c])
                if c == 0:
                    board_as_string += f"|{move}|"
                elif c == 1:
                    board_as_string += f"{move}|"
                else:
                    board_as_string += f"{move}|\n"
        board_as_string += "-------\n"

        return board_as_string
    
    def get_symbol(self, cell):
        if cell == 1:
            return 'X'
        if cell == -1:
            return 'O'
        return '-'
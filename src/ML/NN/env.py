import numpy as np
import gym

from gym import Env, spaces
import random
import time

from tabulate import tabulate
# Global constants

# Game Board Values

EMPTY = 0
X = 1
O = -1


class TicTacToeEnv(Env):
    def __init__(self):
        super().__init__()
        """
        # Observation Space: Define a 2-D discrete obeservation space


        # Action Space: Define a 2-D discrete action space
        self.action_space = spaces.Discrete(9)

        ### Rewards: 
        - Win: +1
        ###

        """      

        self.observation_space = spaces.Discrete(9)
        # Create a canvas to render the environment images upon
        self.canvas = np.ones(self.observation_shape) * 1

        # Define elements present inside the environment
        self.elements = []

    
    def reset(self):
        """
        Reset the board state.
        """
        # Reset the reward
        self.ep_return = 0

        # Reset the board
        self.board


        # Reset the canvas
        self.canvas = np.ones(self.observation_shape) * 1

        # return the obervation
        return self.canvas
    
    def step(self, action):


        # Assert that it is a valid action
        assert self.action_space.contains(action), "Invalid Action"

        # Reward for executing a step
        reward = 1

        

    def render(self):
        """
        Render the board

         The following charachters are used to represent the fields,
            '-' no stone
            'O' for player 0
            'X' for player 1
         example:
            ╒═══╤═══╤═══╕
            │ O │ - │ - │
            ├───┼───┼───┤
            │ - │ X │ - │
            ├───┼───┼───┤
            │ - │ - │ - │
            ╘═══╧═══╧═══╛
        """

        board = np.zeros((3, 3), dtype=str)

        for i in range(3):
            for j in range(3):
                if self.state[i, j] == 0:
                    board[i, j] = "-"
                elif self.state[i, j] == 1:
                    board[i, j] = "X"
                elif self.state[ii, jj] == -1:
                    board[i, j] = "O"
        
        board = tabulate(board, tablefmt="fancy_grid")
        return board

env = TicTacToeEnv()
obs = env.reset()
plt.imshow(obs)


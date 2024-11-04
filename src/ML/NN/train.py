from TicTacToeNN.py import TicTacToeNN

import torch
from torch import nn

model = TicTacToeNN()
criterion = nn.BCELoss()
optimizer = optim.Adam(model.parameters(), lr=0.001)



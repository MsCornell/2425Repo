import azure.functions as func
import datetime
import json
import logging
import sys
import os
import random
#from ...src.ML.minimax import minimax
from minimax import TicTacToe

app = func.FunctionApp()

@app.route(route="test", auth_level=func.AuthLevel.ANONYMOUS, methods=['GET'])
def test(req: func.HttpRequest) -> func.HttpResponse:
    return func.HttpResponse("Hello")

@app.route(route="minimax", auth_level=func.AuthLevel.ANONYMOUS)
def minimax(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')
    logging.info(req.get_json())
    if req.get_json() is None:
        return func.HttpResponse(
                f"No input data",
                status_code=400
            )
    else:
        req_body = req.get_json()
        board = req_body['board_state']
        #next_player = req_body['next_player']
        difficulty_level = req_body['difficulty_level']
    
        # Check the board state contains 9 cells
        if len(board) != 9:
            return func.HttpResponse(
                "Invalid board state",
                status_code=400
            )

        # Check the next player is O (AI player)
        
        # if next_player != "O":
        #     return func.HttpResponse(
        #         "Invalid next player",
        #         status_code=400
        #     )

        # Check player O has 1 less move than player X (X plays first)

        # if board.count("X") != board.count("O") + 1 and board.count("X") != board.count("O"):
        #     return func.HttpResponse(
        #         "Invalid move numbers on the board.",
        #         status_code=400
        #     )

        board_state = []
        board_empty = []
        for row in range(3):
            board_row = []
            i = 0
            while i < 3:
                if board[3*row+i] == "X":
                    board_row.append(1)
                elif board[3*row+i] == "O":
                    board_row.append(-1)
                elif board[3*row+i] == "_":
                    board_row.append(0)
                    board_empty.append(3*row+i+1)
                elif board[3*row+i] == "C":
                    board_row.append(2)
                else:  # Check the cell information is valid
                    return func.HttpResponse(
                        "Invalid cell information",
                        status_code=400)
                i += 1
            board_state.append(board_row)
       
        # if(board.count("X") == board.count("O")):
        #     next_move = {
        #         "next_move": random.choice(board_empty)
        #     }
        #     json_object = json.dumps(next_move)
        
        #     return func.HttpResponse(
        #         f"{json_object}",
        #         status_code=200
        #     )
        

        # Check there is a win in the board state
        # Check the rows
        for row in board_state:
            if all(cell == -1 for cell in row) or all(cell == 1 for cell in row):
                return func.HttpResponse(
                    "There is a win in the board state. Invalid board state",
                    status_code=400
                )
    
        # Check the columns
        for col in range(3):
                if all(board_state[row][col] == -1 for row in range(3)) or all(board_state[row][col] == 1 for row in range(3)):
                    return func.HttpResponse(
                    "There is a win in the board state. Invalid board state",
                    status_code=400
                )

        # Check the diagonals
        if all(board_state[i][i] == -1 for i in range(3)) or all(board_state[i][2-i] == -1 for i in range(3)) or all(board_state[i][i] == 1 for i in range(3)) or all(board_state[i][i] == -1 for i in range(3)):
                return func.HttpResponse(
                    "There is a win in the board state. Invalid board state",
                    status_code=400
                )
        logging.info(board_state)
        
        if difficulty_level == "easy":
            # Use random to get the next move
            next_move = {
                "next_move": random.choice(board_empty)
            }
           
        elif difficulty_level == "medium":
            # Use random and best next move to get the next move
            best_move = 3 * TicTacToe(board_state).best_move()[0] + TicTacToe(board_state).best_move()[1] + 1
            if random.random() < 0.5:
                next_move = {
                    "next_move": random.choice(board_empty)
                }
            else:
                next_move = {
                    "next_move": best_move
                }
      
        elif difficulty_level == "hard":
            # Use minimax to get the next move
            next_move = {
                "next_move": 3 * TicTacToe(board_state).best_move()[0] + TicTacToe(board_state).best_move()[1] + 1
            }

        json_object = json.dumps(next_move)
        
        return func.HttpResponse(
                f"{json_object}",
                status_code=200
            )
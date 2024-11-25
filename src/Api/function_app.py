import azure.functions as func
import datetime
import json
import logging

app = func.FunctionApp()

@app.route(route="minimax", auth_level=func.AuthLevel.ANONYMOUS)
def minimax(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    logging.info(req)

    req_body = req.get_json()
    board = req_body['board']
    next_player = req_body['next_player']
    
    # Check the board state contains 9 cells
    if len(board) != 9:
        return func.HttpResponse(
             "Invalid board state",
             status_code=400
        )

    # Check the next player is O (AI player)
    if next_player != "O":
        return func.HttpResponse(
             "Invalid next player",
             status_code=400
        )

    board_state = []
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
            else:  # Check the cell information is valid
                return func.HttpResponse(
                    "Invalid cell information",
                    status_code=400)
            i += 1
        board_state.append(board_row)
    
    #next_move = TicTacToe(board_state).best_move()

            
    return func.HttpResponse(
             f"{board_state}",
             status_code=200
        )

    # name = req.params.get('name')
    # if not name:
    #     try:
    #         req_body = req.get_json()
    #     except ValueError:
    #         pass
    #     else:
    #         name = req_body.get('name')

    # if name:
    #     return func.HttpResponse(f"Hello, {name}. This HTTP triggered function executed successfully.")
    # else:
    #     return func.HttpResponse(
    #          board.best_move(),
    #          "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.",
    #          status_code=200
    #     )
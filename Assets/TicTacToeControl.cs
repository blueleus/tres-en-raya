using UnityEngine;
using System.Collections;

public class TicTacToeControl : MonoBehaviour {

	public SquareState[] board = new SquareState[9];
	public bool xTurn = true;
	public GameState gameState = GameState.Opening;
	public SquareState winner = SquareState.Clear;

	public GUISkin guiSkin;
	public Texture2D titleImage;

	public void setControl(int boardIndex) {
		if (boardIndex < 0 || boardIndex >= board.Length)
			return;
		board [boardIndex] = xTurn ? SquareState.XControl : SquareState.OControl;
		xTurn = !xTurn;
	}

	public void NewGame() {
		xTurn = true;
		board = new SquareState[9];
	}

	public void DrawGameBoard () {

		bool widthSmaller = Screen.width < Screen.height;
		float smallSide = widthSmaller ? Screen.width : Screen.height;
		float width = smallSide / 3;
		float height = width;
		
		for(int y = 0; y<3; y++) {
			for(int x = 0; x<3; x++) {
				int boardIndex = (y*3)+x;
				Rect square = new Rect(x * width, y * height, width, height);
				string owner = board[boardIndex] == SquareState.XControl ? "X" : board[boardIndex] == SquareState.OControl ? "O" : "";
				if(board[boardIndex] == SquareState.Clear) {
					if(GUI.Button(square, owner)) {
						setControl(boardIndex);
					}
				}
				else {
					GUI.Label(square, owner, owner + "Square");
				}
			}
		}

		Rect turnRect = new Rect (300, 0, 100, 100);
		turnRect.x = widthSmaller ? 0 : smallSide;
		turnRect.y = widthSmaller ? smallSide : 0;
		turnRect.width = widthSmaller ? Screen.width : Screen.width - Screen.height;
		turnRect.height = widthSmaller ? Screen.height - Screen.width : Screen.height;
		GUIStyle turnStyle = new GUIStyle(GUI.skin.GetStyle("label"));
		turnStyle.fontSize *= (int)((Screen.width + Screen.height - (smallSide * 2)) / 100);
		string turnTitle = xTurn ? "X turn" : "O turn";
		GUI.Label (turnRect, turnTitle, turnStyle);
	}

	public void DrawOpening() {

		Rect groupRect = new Rect ((Screen.width /2) - (titleImage.width/2 ),
			(Screen.height/2 ) - ((titleImage.height + 75)/2), titleImage.width, titleImage.height + 75 );

		GUI.BeginGroup (groupRect);
		Rect titleRect = new Rect (0, 0, titleImage.width, titleImage.height);
		GUI.DrawTexture (titleRect, titleImage);
		Rect multiButtonRect = new Rect (titleRect.x, titleRect.y + titleRect.height, titleRect.width, 75);
		if(GUI.Button(multiButtonRect, "New Game")) {
			NewGame();
			gameState = GameState.MultiPlayer;
		}
		GUI.EndGroup ();
	}

	public void DrawGameOver() {
		Rect groupRect = new Rect ((Screen.width / 2) - 150, (Screen.height / 2) - 75, 300, 150 );	
		GUI.BeginGroup (groupRect);
		Rect winnerRect = new Rect (0, 0, groupRect.width, groupRect.height / 2);
		string winnerTitle = winner == SquareState.XControl ? "X Wins" : winner == SquareState.OControl ? "O Wins" : "There is no winner";
		GUI.Label (winnerRect, winnerTitle);
		winnerRect.y += winnerRect.height;
		if (GUI.Button (winnerRect, "Main menu"))
			gameState = GameState.Opening;
		GUI.EndGroup ();
	}

	public void OnGUI() {
		if (guiSkin != null)
			GUI.skin = guiSkin;

		switch (gameState) {
		case GameState.Opening :
			DrawOpening();
			break;
		case GameState.MultiPlayer :
			DrawGameBoard();
			break;
		case GameState.GameOver :
			DrawGameOver();
			break;
		}
	}

	public void setWinner(SquareState toWin) {
		winner = toWin;
		gameState = GameState.GameOver;
	}

	public void LateUpdate() {	
		if (gameState != GameState.MultiPlayer)
						return;

		for (int i = 0; i<3; i++) {
			if ( board[i] != SquareState.Clear && board[i] == board[i + 3] && board[i] == board[i + 6]) {
				setWinner(board[i]);
				return;
			}
			else if ( board[i*3] != SquareState.Clear && board[i*3] == board[i*3 + 1] && board[i*3] == board[i*3 + 2]) {
				setWinner(board[i*3]);
				return;
			}
		}

		if ( board[0] != SquareState.Clear && board[0] == board[4] && board[0] == board[8]) {
			setWinner(board[0]);
			return;
		} else if ( board[2] != SquareState.Clear && board[2] == board[4] && board[2] == board[6]) {
			setWinner(board[2]);
			return;
		}

		for (int i = 0; i<board.Length; i++) {
			if(board[i] == SquareState.Clear)
				return;
		}

		setWinner (SquareState.Clear);
	}
}

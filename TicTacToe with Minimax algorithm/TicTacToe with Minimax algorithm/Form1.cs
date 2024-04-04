using BoardLogic;   // import the BoardLogic namespace
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe_with_Minimax_algorithm
{
    public partial class Form1 : Form
    {
        Board game = new Board();                           // create a new instance of the Board class
        Button[] buttons = new Button[9];                   // create an array to hold references to the buttons on the form

        int humanScore = 0;                                 // create variables to keep track of the human and AI scores, and the number of draws
        int AIScore = 0;
        int drawCount = 0;

        public Form1()
        {
            InitializeComponent();
            game = new Board();                            // initialize the game board

            buttons[0] = button1;                          // initialize the button array with references to the buttons on the form
            buttons[1] = button2;
            buttons[2] = button3;
            buttons[3] = button4;
            buttons[4] = button5;
            buttons[5] = button6;
            buttons[6] = button7;
            buttons[7] = button8;
            buttons[8] = button9;

            
            for (int i=0;i < buttons.Length; i++)         // add a common click event to each button, and set the Tag property to the index of the button in the array
            {
                buttons[i].Click += handleButtonclick;
                buttons[i].Tag = i;
            }
            
        }

        private void handleButtonclick(object sender, EventArgs e)
        {
           Button clickedButton= (Button)sender;                         // get a reference to the button that was clicked

            int gameSquareNumber = (int)clickedButton.Tag;              // get the index of the button from its Tag property

            game.Grid[gameSquareNumber] = 1;                            // update the game board with the human player's move

            UpdateBoard();                                              // update the buttons on the form to reflect the current state of the game board

            if (game.isBoardFull() == true)                             // check to see if the game is over
            {
                disableAllButtons();                                    // if the board is full and there is no winner, it's a draw
                drawCount++;
                MessageBox.Show("No winner, it's a draw!");
                lblDraws.Text = "Draws: " + drawCount.ToString();
            }
            else if(game.CheckForWinner() ==1)                             // if the human player has won, update the score and display a message
            {
                humanScore++;
                lblHumanScore.Text = "Human: " + humanScore.ToString();
                MessageBox.Show("Player human wins!");
                disableAllButtons();
            }
            else
            {                                                        
                ComputerChoose();                                       // if the game is not over, it's the AI's turn
            }

        }

        private void disableAllButtons()                                  // disable all the buttons on the form
        {
            foreach (var item in buttons)
            {
                item.Enabled = false;
            }
        }

        private void ComputerChoose()                                     
        {
            int bestMove = int.MinValue;                               // initialize variables to hold the best move and its score
            int bestMoveScore = int.MinValue;

            for (int i = 0; i < game.Grid.Length; i++)                // loop through each square on the game board
            {
                if (game.Grid[i] == 0)                                 // if the square is empty, try making a move there
                {
                    game.Grid[i] = 2;
                    int score = MiniMax(game.Grid, 0, false);
                    game.Grid[i] = 0;

                    if (score > bestMoveScore)
                    {
                        bestMove = i;
                        bestMoveScore = score;
                    }
                }
            }

            game.Grid[bestMove] = 2;
            UpdateBoard();

            //check for winner
            //check to see if the board is full

            if (game.isBoardFull() == true)
            {
                disableAllButtons();
            }
            if (game.CheckForWinner() == 2)
            {
                AIScore++;
                lblAIScore.Text = "Compute_AI: " + AIScore.ToString();
                MessageBox.Show("Computer wins!");
                disableAllButtons();
            }
    }
        private int MiniMax(int[] grid, int depth, bool isMaximizing)
        {
            int winner = game.CheckForWinner();

            if (winner == 1) // Player human wins
            {
                return -1;
            }
            else if (winner == 2) // Player computer wins
            {
                return 1;
            }
            else if (game.isBoardFull()) // Tie game
            {
                return 0;
            }

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int i = 0; i < grid.Length; i++)
                {
                    if (grid[i] == 0)
                    {
                        grid[i] = 2;
                        int score = MiniMax(grid, depth + 1, false);
                        grid[i] = 0;
                        bestScore = Math.Max(score, bestScore);
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int i = 0; i < grid.Length; i++)
                {
                    if (grid[i] == 0)
                    {
                        grid[i] = 1;
                        int score = MiniMax(grid, depth + 1, true);
                        grid[i] = 0;
                        bestScore = Math.Min(score, bestScore);
                    }
                }
                return bestScore;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateBoard();
        }

        private void UpdateBoard()
        {
            //assign an X or O to the text of each button based on the values in the Board
            for (int i = 0; i < game.Grid.Length; i++)
            {

                if (game.Grid[i] ==0)
                {
                    buttons[i].Text = "";
                    buttons[i].Enabled = true;
                    
                }
                else if (game.Grid[i] == 1)
                {
                    buttons[i].Text = "X";
                    buttons[i].Enabled = false;
                    
                }
                else if (game.Grid[i] == 2)
                {
                    buttons[i].Text = "O";
                    buttons[i].Enabled = false;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            game = new Board();
            enableAllButtons();
        }

        private void enableAllButtons()
        {
            foreach (var item in buttons)
            {
                item.Enabled = true;
            }

            UpdateBoard();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

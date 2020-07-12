using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Microsoft.VisualBasic;
using System.Text;
using System;

namespace TicTacToe
{
    class Program
    {
        // Creates the layout for the game board
        private static string[][] board = new string[][] { 
            new string[] { "", "", "" }, 
            new string[] { "", "", "" },
            new string[] { "", "", "" }
        };

        // Players
        private static List<GamePlayer> players = new List<GamePlayer>(); // List of players
        private static GamePlayer playerTurn; // The current players turn

        static void Main(string[] args)
        {
            // Adding the players into the game
            players.Add(new GamePlayer("Player 1", "x"));
            players.Add(new GamePlayer("Player 2", "o"));

            // Sets the current turn to a random player
            playerTurn = players[new Random().Next(players.Count - 1)];

            // Starts the game loop
            while (true)
            {
                // Informs the current player that it's their turn
                if (!playerTurn.hasInformedPlay())
                {
                    playerTurn.setInformedPlayer(true); // We do this so the player isn't spammed
                    Console.WriteLine("Hey " + playerTurn.getName() + ", it's your turn! Pick from the following positions:");
                    
                    // Prints the available positions the player can use
                    StringBuilder positions = new StringBuilder();
                    foreach (int position in getAvailablePositions())
                    {
                        positions.Append(position + ", ");
                    }
                    Console.WriteLine(positions.ToString().Substring(0, positions.ToString().Length - 2));
                }

                string typed = Console.ReadLine(); // Gets what the user typed
                
                // Handles game closing
                if (typed.ToLower().Equals("exit"))
                {
                    Console.WriteLine("Exited game, thanks for playing!");
                    break;
                }
                
                // Handles player selection
                int selected = -1;
                try {
                    selected = int.Parse(typed); // Try to parse the users input as an integer
                } catch (Exception ignored) {}
                if (selected == -1 || (!getAvailablePositions().Contains(selected))) // If the parse failed or the position selected is not valid, give an error
                {
                    Console.WriteLine("Invalid position, try again!");
                } else {
                    if (select(selected)) // Selects the position and updates the game board (returns true if the position is a winning position)
                    {
                        printBoard(); // Prints the board so the participants can see what the winning move was
                        Console.WriteLine(playerTurn.getName() + " has won! Thanks for playing!");
                        break;
                    }
                    printBoard(); // Prints the new game board to the user
                    Console.WriteLine(playerTurn.getName() + " selected position " + selected + "!"); // Notifys the user of the position they selected
                
                    // Rotates the player turn (this will select the next player to play)
                    int index = players.IndexOf(playerTurn) + 1;
                    if (index >= players.Count) // If the index is more than the players size, set the new player index to 0\
                    {
                        index = 0;
                    }
                    playerTurn = players[index];
                    playerTurn.setInformedPlayer(false);
                }
            }
        }

        private static bool select(int position)
        {
            // Updates the board
            int column = 0;
            int row = 0;
            for (int i = 1; i <= board.Length * 3; i++)
            {
                if (i == position)
                {
                    board[column][row] = playerTurn.getIcon();
                    break;
                }
                if (++row >= 3)
                {
                    column++;
                    row = 0;
                }
            }

            // Checks horizontal winnings
            for (int i = 0; i < board.Length; i++)
            {
                bool win = true;
                for (int j = 0; j < board[i].Length; j++)
                {
                    if (!board[i][j].Equals(playerTurn.getIcon()))
                    {
                        win = false;
                    }
                }
                if (win)
                {
                    return true;
                }
            }

            // Checks vertical winnings
            for (int i = 0; i < 3; i++)
            {
                string positionIcon = board[1][i];
                if (positionIcon.Length == 0) // If the icon is empty (blank), continue
                {
                    continue;
                }
                bool above = board[0][i].ToUpper().Equals(positionIcon.ToUpper());
                bool below = board[2][i].ToUpper().Equals(positionIcon.ToUpper());

                if (above && below)
                {
                    return true;
                }
            }

            // Checks diagonal winnings
            string center = board[1][1].ToUpper();
            string icon = playerTurn.getIcon().ToUpper();
            if (center.Length > 0 && center.Equals(icon)) // If the center icon is set and it's the same as the player's icon
            {
                string topLeft = board[0][0].ToUpper();
                string bottomRight = board[2][2].ToUpper();
                string bottomLeft = board[2][0].ToUpper();
                string topRight = board[0][2].ToUpper();
            
                // Checks if the top left and bottom right positions are the same as the center
                if (topLeft.Equals(icon) && bottomRight.Equals(icon))
                {
                    return true;
                }

                // Checks if the bottom left and top right positions are the same as the center
                if (bottomLeft.Equals(icon) && topRight.Equals(icon))
                {
                    return true;
                }
            }
            
            return false;
        }

        private static List<int> getAvailablePositions()
        {
            List<int> positions = new List<int>();
            foreach(Tuple<int, string> position in getPositions())
            {
                if (position.Item2.Length == 0)
                {
                    positions.Add(position.Item1);
                }
            }
            return positions;
        }

        private static List<Tuple<int, string>> getPositions()
        {
            List<Tuple<int, string>> positions = new List<Tuple<int, string>>();
            int column = 0;
            int row = 0;
            for (int i = 1; i <= board.Length * 3; i++)
            {
                positions.Add(new Tuple<int, string>(i, board[column][row++].ToString()));
                if (row >= 3)
                {
                    column++;
                    row = 0;
                }
            }
            return positions;
        }

        // The game board would be shown as follows:
        /*
            -------------
            | x | x | x |
            | o | o | o |
            | x | x | x |
            -------------
        */
        private static void printBoard()
        {
            StringBuilder builder = new StringBuilder();
            Console.WriteLine("-------------");
            for (int i = 0; i < board.Length; i++)
            {
                builder.Append("| ");
                for (int j = 0; j < board[i].Length; j++)
                {
                    string icon = board[i][j];
                    builder.Append((icon.Length == 0 ? "-" : icon) + " | ");
                }
                builder.Append("\n");
            }
            Console.WriteLine(builder.ToString().Substring(0, builder.ToString().Length - 2));
            Console.WriteLine("-------------");
        }
    }
}

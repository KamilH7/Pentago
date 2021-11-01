using System;

namespace PentagoMinMax
{

    class Pentago
    {
        Field[,] board;
        Random rnd;

        public Pentago(Field[,] board)
        {
            rnd = new Random();
            this.board = board;
        }

        public Pentago()
        {
            rnd = new Random();

            //x is columns, y is rows
            board = new Field[6, 6];

            //initialize the board
            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    board[i, j] = new Field();
                }
            }
        }

        public void RotateSegment(int segmentX, int segmentY, bool rotateClockWise)
        {
            /*
               each segment is a 3x3 piece of the board placed like so:

               segment [0,0] | segment [0,1]
              ______________________________
               segment [1,0] | segment [1,1]

             */

            //rotate the designated 3x3 piece of the board and save it to a new array
            Field[,] temp = new Field[3, 3];

            //offset is determined by the segment chosen
            int offsetX = segmentX * 3;
            int offsetY = segmentY * 3;

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (rotateClockWise)
                    {
                        temp[i, j] = board[3 - j - 1 + offsetX, i + offsetY];
                    }
                    else
                    {
                        temp[i, j] = board[j + offsetX, 3 - i - 1 + offsetY];
                    }
                }
            }

            //overwrite the board
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    board[i + offsetX, j + offsetY] = temp[i, j];
                }
            }
        }

        public bool CheckIfPossiblePlacement(int rockX, int rockY)
        {
            if (board[rockX, rockY].player == Player.None)
            {
                return true;
            }

            return false;
        }

        public void PlaceRock(Player player, int rockX, int rockY)
        {
            //place a stone
            board[rockX, rockY].player = player;
        }

        public Field[,] getBoard()
        {
            return board;
        }

        public Field[,] copyBoard()
        {
            Field[,] deepCopy = new Field[6, 6];

            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    deepCopy[i, j] = new Field();
                    deepCopy[i, j].player = board[i, j].player;
                }
            }

            return deepCopy;
        }

        public WinType CheckWinType()
        {
            int player1WinningRows = 0;
            int player2WinningRows = 0;

            //calculate the amount of winning rows for each player
            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    Field currentField = board[i, j];

                    if (currentField.player == Player.None)
                        continue;

                    //vertical
                    if (!currentField.checkedVertically)
                    {
                        for (int offset = 0; offset < 5; ++offset)
                        {
                            //for vertical x stays the same while y increases
                            int xPosition = i;
                            int yPosition = j + offset;

                            //stop
                            if (StopCondition(xPosition, yPosition + 1, currentField.player))
                            {
                                //set flags
                                for (int n = 0; n <= offset; n++)
                                {
                                    board[i, j + n].checkedVertically = true;
                                }

                                CheckWinningRow(currentField.player, offset);
                                break;
                            }
                        }
                    }

                    //horizontal
                    if (!currentField.checkedHorizontally)
                    {
                        for (int offset = 0; offset < 5; ++offset)
                        {
                            //for horizontall y stays the same while x increases
                            int xPosition = i + offset;
                            int yPosition = j;

                            //stop
                            if (StopCondition(xPosition + 1, yPosition, currentField.player))
                            {
                                //set flags
                                for (int n = 0; n <= offset; n++)
                                {
                                    board[i + n, j].checkedHorizontally = true;
                                }

                                CheckWinningRow(currentField.player, offset);
                                break;
                            }
                        }
                    }

                    //diagonal right
                    if (!currentField.checkedDiagonalRight)
                    {
                        for (int offset = 0; offset < 5; ++offset)
                        {
                            //for diagonall right x and y increase
                            int xPosition = i + offset;
                            int yPosition = j + offset;

                            //stop
                            if (StopCondition(xPosition + 1, yPosition + 1, currentField.player))
                            {
                                //set flags
                                for (int n = 0; n <= offset; n++)
                                {
                                    board[i + n, j + n].checkedDiagonalRight = true;
                                }

                                CheckWinningRow(currentField.player, offset);
                                break;
                            }
                        }
                    }

                    //diagonal left
                    if (!currentField.checkedDiagonalLeft)
                    {
                        for (int offset = 0; offset < 5; ++offset)
                        {
                            //for vertical x decreases while y increases
                            int xPosition = i - offset;
                            int yPosition = j + offset;

                            //stop
                            if (StopCondition(xPosition - 1, yPosition + 1, currentField.player))
                            {
                                //set flags
                                for (int n = 0; n <= offset; n++)
                                {
                                    board[i - n, j + n].checkedDiagonalLeft = true;
                                }

                                CheckWinningRow(currentField.player, offset);
                                break;
                            }
                        }
                    }
                }
            }

            void CheckWinningRow(Player player, int InARow)
            {
                if (InARow == 4)
                {
                    if (player == Player.Player1)
                    {
                        player1WinningRows++;
                    }
                    else
                    {
                        player2WinningRows++;
                    }
                }
            }

            bool StopCondition(int x, int y, Player checkedPlayer)
            {
                if (x < 0 || x > 5 || y < 0 || y > 5 || board[x, y].player != checkedPlayer)
                {
                    return true;
                }
                return false;
            }

            //reset the checks used for optimization
            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    board[i, j].resetChecks();
                }
            }

            //if there is at least one winning row compare the amount of winning rows for each player and return according result
            if (player1WinningRows != 0 || player2WinningRows != 0)
            {
                if (player1WinningRows == player2WinningRows)
                {
                    return WinType.Draw;
                }
                else if (player1WinningRows > player2WinningRows)
                {
                    return WinType.Player1Win;
                }
                else
                {
                    return WinType.Player2Win;
                }
            }

            //finally check if every spot is occupied in which case the game ends in a draw
            bool draw = true;
            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    if (board[i, j].player == 0)
                    {
                        draw = false;
                        break;
                    }
                }
            }

            if (draw)
            {
                return WinType.Draw;
            }

            //if no test returned true then the win condition is not met.
            return WinType.None;
        }

        //DEBUG
        public void PrintBoard()
        {
            Console.WriteLine("");
            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    Console.ForegroundColor = board[i, j].player == Player.Player1 ? ConsoleColor.Blue : ConsoleColor.Green;

                    Console.Write("[" + board[i, j].player.ToString().PadRight(7, ' ') + "]", Console.ForegroundColor);
                }
                Console.WriteLine("");
            }

            Console.WriteLine();
        }
    }
}

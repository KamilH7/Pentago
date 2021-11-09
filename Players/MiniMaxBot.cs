using System;

namespace PentagoMinMax
{
    class MiniMaxBot : PentagoPlayer
    {
        Random rnd = new Random();
        int searchDepth;

        public MiniMaxBot(int searchDepth) : base(PlayerType.MiniMax)
        {
            this.searchDepth = searchDepth;
        }

        public override void PlaceRock()
        {
            int chosenX = 0;
            int chosenY = 0;

            double bestValue = double.NegativeInfinity;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (pentago.CheckIfPossiblePlacement(i, j))
                    {
                        Pentago newBoard = new Pentago(pentago.copyBoard());
                        newBoard.PlaceRock(assignedPlayer, i, j);
                        double newValue = MiniMax(newBoard, searchDepth, true, true);
                        if (bestValue < newValue)
                        {
                            bestValue = newValue;
                            chosenX = i;
                            chosenY = j;
                        }
                    }
                }
            }

            pentago.PlaceRock(assignedPlayer, chosenX, chosenY);
        }

        public override void RotateSegment()
        {
            int chosenX = 0;
            int chosenY = 0;
            bool rotateClockwise = true;

            double bestValue = double.NegativeInfinity;

            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    for (int k = 0; k < 2; ++k)
                    {
                        Pentago newBoard = new Pentago(pentago.copyBoard());
                        newBoard.RotateSegment(i, j, k == 1 ? true : false);
                        double newValue = MiniMax(newBoard, searchDepth, false, false);
                        if (bestValue < newValue)
                        {
                            bestValue = newValue;
                            chosenX = i;
                            chosenY = j;
                            rotateClockwise = k == 1 ? true : false;
                        }
                    }
                }
            }

            pentago.RotateSegment(chosenX, chosenY, rotateClockwise);
        }

        public double MiniMax(Pentago pentago, int depth, bool maximizing, bool rotating)
        {
            if (depth == 0 || pentago.CheckWinType() != WinType.None)
            {
                return CalculateBoardValue(pentago);
            }

            if (maximizing)
            {
                double bestValue = Double.NegativeInfinity;

                if (!rotating)
                {
                    for (int i = 0; i < 6; ++i)
                    {
                        //possible y placement
                        for (int j = 0; j < 6; ++j)
                        {
                            if (!pentago.CheckIfPossiblePlacement(i, j))
                                continue;

                            Pentago newPosition = new Pentago(pentago.copyBoard());
                            newPosition.PlaceRock(assignedPlayer, i, j);
                            Double newValue = MiniMax(newPosition, depth, true, true);
                            bestValue = Math.Max(bestValue, newValue);
                        }
                    }
                    return bestValue;
                }
                else
                {
                    for (int i = 0; i < 2; ++i)
                    {
                        for (int j = 0; j < 2; ++j)
                        {
                            for (int k = 0; k < 2; ++k)
                            {
                                Pentago newPosition = new Pentago(pentago.copyBoard());
                                newPosition.RotateSegment(i, j, k == 1 ? true : false);
                                double newValue = MiniMax(newPosition, depth, false, false);
                                bestValue = Math.Max(bestValue, newValue);
                            }
                        }
                    }
                    return bestValue;
                }
            }
            else
            {
                double bestValue = Double.PositiveInfinity;

                if (!rotating)
                {
                    for (int i = 0; i < 6; ++i)
                    {
                        for (int j = 0; j < 6; ++j)
                        {
                            if (!pentago.CheckIfPossiblePlacement(i, j))
                                continue;
                            Pentago newPosition = new Pentago(pentago.copyBoard());
                            newPosition.PlaceRock(assignedPlayer == Player.Player1 ? Player.Player2 : Player.Player1, i, j);
                            double newValue = MiniMax(newPosition, depth - 1, false, true);
                            bestValue = Math.Min(bestValue, newValue);
                        }
                    }
                    return bestValue;
                }
                else
                {
                    for (int i = 0; i < 2; ++i)
                    {
                        for (int j = 0; j < 2; ++j)
                        {
                            for (int k = 0; k < 2; ++k)
                            {
                                Pentago newPosition = new Pentago(pentago.copyBoard());
                                newPosition.RotateSegment(i, j, k == 1 ? true : false);
                                double newValue = MiniMax(newPosition, depth - 1, true, false);
                                bestValue = Math.Min(bestValue, newValue);
                            }
                        }
                    }

                    return bestValue;
                }
            }
        }

        public double CalculateBoardValue(Pentago pentago)
        {
            Field[,] board = pentago.getBoard();
            //pentago.PrintBoard();
            double opponentCost = 0;
            double playerCost = 0;

            //for every field
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

                                AddCost(currentField.player, offset);
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

                                AddCost(currentField.player, offset);
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

                                AddCost(currentField.player, offset);
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

                                AddCost(currentField.player, offset);
                                break;
                            }
                        }
                    }
                }
            }

            void AddCost(Player player, int offset)
            {
                if (player == assignedPlayer)
                {
                    if (offset == 0)
                    {
                        playerCost += 0.25;
                    }
                    else if (offset == 4)
                    {
                        playerCost += Double.PositiveInfinity;
                    }
                    else
                    {
                        playerCost += Math.Pow(2, offset * 2);
                    }
                }
                else if (player != assignedPlayer)
                {
                    if (offset == 0)
                    {
                        opponentCost += 0.25 -0.1;
                    }
                    else if (offset == 4)
                    {
                        opponentCost += Double.PositiveInfinity;
                    }
                    else
                    {
                        opponentCost += Math.Pow(2, offset * 2) - 1;
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

            //reset the checks

            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    board[i, j].resetChecks();
                }
            }

            double cost = (double)playerCost - (double)opponentCost;

            return cost;
        }
    }
}

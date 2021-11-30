using System;

namespace Pentago
{
    class StandardMiniMax : MiniMaxBot
    {
        public StandardMiniMax(Player assignedPlayer, int searchDepth) : base(assignedPlayer, searchDepth, PlayerType.MiniMax)
        {

        }

        protected override double GetBoardValueAfterRockPlacement(Pentago board)
        {
            return MiniMaxAlgorithm(board, searchDepth, true, true);
        }

        protected override double GetBoardValueAfterSegmentRotation(Pentago board)
        {
            return MiniMaxAlgorithm(board, searchDepth, false, false);
        }

        private double MiniMaxAlgorithm(Pentago pentago, int depth, bool maximizing, bool rotating)
        {
            treeIterations++;

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
                            Double newValue = MiniMaxAlgorithm(newPosition, depth -1, true, true);
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
                                double newValue = MiniMaxAlgorithm(newPosition, depth-1, false, false);
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
                            double newValue = MiniMaxAlgorithm(newPosition, depth - 1, false, true);
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
                                double newValue = MiniMaxAlgorithm(newPosition, depth - 1, true, false);
                                bestValue = Math.Min(bestValue, newValue);
                            }
                        }
                    }

                    return bestValue;
                }
            }
        }
    }
}

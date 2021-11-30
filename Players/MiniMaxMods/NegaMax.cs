using System;

namespace Pentago
{
    class NegaMax : MiniMaxBot
    {

        public NegaMax(Player assignedPlayer, int searchDepth) : base(assignedPlayer, searchDepth, PlayerType.NegaMax)
        {

        }

        protected override double GetBoardValueAfterRockPlacement(Pentago board)
        {
            return NegaMaxAlgorithm(board, searchDepth, true);
        }

        protected override double GetBoardValueAfterSegmentRotation(Pentago board)
        {
            return NegaMaxAlgorithm(board, searchDepth, false);
        }

        private double NegaMaxAlgorithm(Pentago pentago, int depth, bool rotating, int sign = 1)
        {
            treeIterations++;

            if (depth == 0 || pentago.CheckWinType() != WinType.None)
            {
                return sign * CalculateBoardValue(pentago);
            }

            double bestValue = Double.NegativeInfinity;

            if (!rotating)
            {
                for (int i = 0; i < 6; ++i)
                {
                    for (int j = 0; j < 6; ++j)
                    {
                        if (!pentago.CheckIfPossiblePlacement(i, j))
                            continue;

                        Pentago newBoard = new Pentago(pentago.copyBoard());
                        newBoard.PlaceRock(assignedPlayer, i, j);
                        Double newValue = NegaMaxAlgorithm(newBoard, depth - 1, true, sign);
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
                            Pentago newBoard = new Pentago(pentago.copyBoard());
                            newBoard.RotateSegment(i, j, k == 1 ? true : false);
                            double newValue = -NegaMaxAlgorithm(newBoard, depth - 1, false, -sign);
                            bestValue = Math.Max(bestValue, newValue);
                        }
                    }
                }
                return bestValue;
            }
        }
    }
}

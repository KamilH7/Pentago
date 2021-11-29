using System;

namespace Pentago
{
    class ABNegaMax : MiniMaxBot
    {
        public ABNegaMax(Player assignedPlayer, int searchDepth) : base(assignedPlayer, searchDepth, PlayerType.ABNegaMax)
        {

        }

        protected override double GetBoardValueAfterRockPlacement(Pentago board)
        {
            return ABNegaMaxAlogirthm(board, searchDepth, 1, true);
        }

        protected override double GetBoardValueAfterSegmentRotation(Pentago board)
        {
            return ABNegaMaxAlogirthm(board, searchDepth, 1, false);
        }

        private double ABNegaMaxAlogirthm(Pentago pentago, int depth, int sign, bool rotating, double alpha = Double.NegativeInfinity, double beta = Double.PositiveInfinity)
        {
            if (depth == 0 || pentago.CheckWinType() != WinType.None)
            {
                return sign * CalculateBoardValue(pentago);
            }


            if (!rotating)
            {
                for (int i = 0; i < 6; ++i)
                {
                    for (int j = 0; j < 6; ++j)
                    {
                        if (!pentago.CheckIfPossiblePlacement(i, j))
                            continue;

                        Pentago newPosition = new Pentago(pentago.copyBoard());
                        newPosition.PlaceRock(assignedPlayer, i, j);
                        alpha = Math.Max(alpha, ABNegaMaxAlogirthm(newPosition, depth - 1, sign, true));

                        if (alpha >= beta)
                            return beta;
                    }
                }

                return alpha;
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
                            alpha = Math.Max(alpha, -ABNegaMaxAlogirthm(newPosition, depth - 1, -sign, false));

                            if (alpha >= beta)
                                return beta;
                        }
                    }
                }

                return alpha;

            }
        }
    }
}

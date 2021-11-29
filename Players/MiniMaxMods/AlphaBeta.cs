using System;

namespace Pentago
{
    class AlphaBeta : MiniMaxBot
    {
        public AlphaBeta(Player assignedPlayer, int searchDepth) : base(assignedPlayer, searchDepth, PlayerType.AlphaBeta)
        {

        }

        protected override double GetBoardValueAfterRockPlacement(Pentago board)
        {
            return AlphaBetaAlgorithm(board, searchDepth, true, true);
        }

        protected override double GetBoardValueAfterSegmentRotation(Pentago board)
        {
            return AlphaBetaAlgorithm(board, searchDepth, false, false);
        }

        private double AlphaBetaAlgorithm(Pentago pentago, int depth, bool maximizing, bool rotating, double alpha = Double.NegativeInfinity, double beta = Double.PositiveInfinity)
        {
            if (depth == 0 || pentago.CheckWinType() != WinType.None)
            {
                return CalculateBoardValue(pentago);
            }

            if (maximizing)
            {
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
                            alpha = Math.Max(alpha, AlphaBetaAlgorithm(newPosition, depth - 1, true, true));

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
                                alpha = Math.Max(alpha, AlphaBetaAlgorithm(newPosition, depth - 1, false, false));

                                if (alpha >= beta)
                                    return beta;
                            }
                        }
                    }

                    return alpha;
                }
            }
            else
            {
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
                            beta = Math.Min(beta,AlphaBetaAlgorithm(newPosition, depth - 1, false, true));

                            if (alpha >= beta)
                                return alpha;
                        }
                    }

                    return beta;
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
                                beta = Math.Min(beta, AlphaBetaAlgorithm(newPosition, depth - 1, true, false));

                                if (alpha >= beta)
                                    return alpha;
                            }
                        }
                    }
                    return beta;
                }
            }
        }
    }
}

using System;

namespace PentagoMinMax
{
    class NegaMaxBot : MiniMaxBot
    {
        public NegaMaxBot(Player assignedPlayer, int searchDepth) : base(assignedPlayer, searchDepth, PlayerType.NegaMax)
        {

        }

        public override double MiniMax(Pentago pentago, int depth, bool maximizing, bool rotating)
        {
            return NegaMax(pentago,depth,rotating);
        }

        public double NegaMax(Pentago pentago, int depth, bool rotating, int sign=1)
        {
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

                        Pentago newPosition = new Pentago(pentago.copyBoard());
                        newPosition.PlaceRock(assignedPlayer, i, j);
                        Double newValue = -NegaMax(newPosition, depth -1, true, -sign);
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
                            double newValue = -NegaMax(newPosition, depth -1, false, -sign);
                            bestValue = Math.Max(bestValue, newValue);
                        }
                    }
                }
                return bestValue;
            }

        }
    }
}


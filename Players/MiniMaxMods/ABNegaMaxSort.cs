using System;
using System.Collections.Generic;
namespace Pentago
{
    class ABNegaMaxSort : MiniMaxBot
    {
        public ABNegaMaxSort(Player assignedPlayer, int searchDepth) : base(assignedPlayer, searchDepth, PlayerType.ABNegaMaxSort)
        {

        }

        protected override double GetBoardValueAfterRockPlacement(Pentago board)
        {
            return ABNegaMaxSortAlogirthm(board, searchDepth, 1, true);
        }

        protected override double GetBoardValueAfterSegmentRotation(Pentago board)
        {
            return ABNegaMaxSortAlogirthm(board, searchDepth, 1, false);
        }

        private double ABNegaMaxSortAlogirthm(Pentago pentago, int depth, int sign, bool rotating, double alpha = Double.NegativeInfinity, double beta = Double.PositiveInfinity)
        {
            if (depth == 0 || pentago.CheckWinType() != WinType.None)
            {
                return sign * CalculateBoardValue(pentago);
            }


            if (!rotating)
            {
                List<RockPlacement> rockPlacements = GenerateRockPlacements(pentago);
                QuickSort(rockPlacements.ConvertAll(x => (Move)x), 0, rockPlacements.Count - 1);

                foreach (RockPlacement placement in rockPlacements)
                {
                    Pentago newPosition = new Pentago(pentago.copyBoard());
                    newPosition.PlaceRock(assignedPlayer, placement.x, placement.y);
                    alpha = Math.Max(alpha, ABNegaMaxSortAlogirthm(newPosition, depth - 1, sign, true));

                    if (alpha >= beta)
                        return beta;
                }

                return alpha;
            }
            else
            {
                List<SegmentRotation> segmentRotation = GenerateSegmentRotations(pentago);
                QuickSort(segmentRotation.ConvertAll(x => (Move)x), 0, segmentRotation.Count - 1);

                foreach (SegmentRotation rotation in segmentRotation)
                {

                    Pentago newPosition = new Pentago(pentago.copyBoard());
                    newPosition.RotateSegment(rotation.x, rotation.y, rotation.clockwise);
                    alpha = Math.Max(alpha, -ABNegaMaxSortAlogirthm(newPosition, depth - 1, -sign, false));

                    if (alpha >= beta)
                        return beta;
                }

                return alpha;

            }
        }

        public static void QuickSort(List<Move> array, int left, int right)
        {
            var i = left;
            var j = right;
            var pivot = array[(left + right) / 2].heuristicValue;

            while (i < j)
            {
                while (array[i].heuristicValue < pivot) i++;
                while (array[j].heuristicValue > pivot) j--;

                if (i <= j)
                {
                    var tmp = array[i];
                    array[i++] = array[j];
                    array[j--] = tmp;
                }
            }

            if (left < j) QuickSort(array, left, j);
            if (i < right) QuickSort(array, i, right);
        }

        List<RockPlacement> GenerateRockPlacements(Pentago pentago)
        {
            List<RockPlacement> possibleMoves = new List<RockPlacement>();

            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    if (!pentago.CheckIfPossiblePlacement(i, j))
                        continue;

                    possibleMoves.Add(new RockPlacement(i, j, CalculateBoardValue(pentago)));
                }
            }

            return possibleMoves;
        }

        List<SegmentRotation> GenerateSegmentRotations(Pentago pentago)
        {
            List<SegmentRotation> possibleMoves = new List<SegmentRotation>();

            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    for (int k = 0; k < 2; ++k)
                    {
                        possibleMoves.Add(new SegmentRotation(i, j, k == 1 ? true : false, CalculateBoardValue(pentago)));
                    }
                }
            }

            return possibleMoves;
        }
    }

    interface Move
    {
        public double heuristicValue { get; set; }
    }

    class RockPlacement : Move
    {
        public double heuristicValue { get; set; }
        public int x;
        public int y;


        public RockPlacement(int x, int y, double heuristicValue)
        {
            this.x = x;
            this.y = y;
            this.heuristicValue = heuristicValue;
        }
    }


    class SegmentRotation : Move
    {
        public double heuristicValue { get; set; }

        public int x;
        public int y;
        public bool clockwise;


        public SegmentRotation(int x, int y, bool clockwise, double heuristicValue)
        {
            this.x = x;
            this.y = y;
            this.clockwise = clockwise;
            this.heuristicValue = heuristicValue;
        }
    }
}

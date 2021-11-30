using System;
using System.Collections.Generic;
using System.Linq;

namespace Pentago
{
    class AlphaBetaSort : MiniMaxBot
    {
        public int treeIterations = 0;

        public AlphaBetaSort(Player assignedPlayer, int searchDepth) : base(assignedPlayer, searchDepth, PlayerType.AlphaBetaSort)
        {

        }

        protected override double GetBoardValueAfterRockPlacement(Pentago board)
        {
            return AlphaBetaAlgorithm(board, searchDepth, true, true, Double.NegativeInfinity, Double.PositiveInfinity);
        }

        protected override double GetBoardValueAfterSegmentRotation(Pentago board)
        {
            return AlphaBetaAlgorithm(board, searchDepth, false, false, Double.NegativeInfinity, Double.PositiveInfinity);
        }

        private double AlphaBetaAlgorithm(Pentago pentago, int depth, bool maximizing, bool rotating, double alpha, double beta)
        {
            treeIterations++;

            if (depth == 0 || pentago.CheckWinType() != WinType.None)
            {
                return CalculateBoardValue(pentago);
            }

            if (maximizing)
            {
                if (!rotating)
                {
                    List<RockPlacement> rockPlacements = GenerateRockPlacements(pentago);
                    rockPlacements = rockPlacements.OrderByDescending(o => o.heuristicValue).ToList();

                    foreach (RockPlacement placement in rockPlacements)
                    {
                        Pentago newPosition = new Pentago(pentago.copyBoard());
                        newPosition.PlaceRock(assignedPlayer, placement.x, placement.y);
                        alpha = Math.Max(alpha, AlphaBetaAlgorithm(newPosition, depth - 1, true, true, alpha, beta));

                        if (alpha >= beta)
                            return beta;

                    }
                    return alpha;
                }
                else
                {
                    List<SegmentRotation> segmentRotation = GenerateSegmentRotations(pentago);
                    segmentRotation = segmentRotation.OrderByDescending(o => o.heuristicValue).ToList();

                    foreach (SegmentRotation rotation in segmentRotation)
                    {
                        Pentago newPosition = new Pentago(pentago.copyBoard());
                        newPosition.RotateSegment(rotation.x, rotation.y, rotation.clockwise);
                        alpha = Math.Max(alpha, AlphaBetaAlgorithm(newPosition, depth - 1, false, false, alpha, beta));

                        if (alpha >= beta)
                            return beta;
                    }

                    return alpha;
                }
            }
            else
            {
                if (!rotating)
                {
                    List<RockPlacement> rockPlacements = GenerateRockPlacements(pentago);
                    rockPlacements = rockPlacements.OrderByDescending(o => o.heuristicValue).ToList();

                    foreach (RockPlacement placement in rockPlacements)
                    {
                        Pentago newPosition = new Pentago(pentago.copyBoard());
                        newPosition.PlaceRock(assignedPlayer == Player.Player1 ? Player.Player2 : Player.Player1, placement.x, placement.y);
                        beta = Math.Min(beta, AlphaBetaAlgorithm(newPosition, depth - 1, false, true, alpha, beta));

                        if (alpha >= beta)
                            return alpha;
                    }

                    return beta;
                }
                else
                {
                    List<SegmentRotation> segmentRotation = GenerateSegmentRotations(pentago);
                    segmentRotation = segmentRotation.OrderByDescending(o => o.heuristicValue).ToList();

                    foreach (SegmentRotation rotation in segmentRotation)
                    {
                        Pentago newPosition = new Pentago(pentago.copyBoard());
                        newPosition.RotateSegment(rotation.x, rotation.y, rotation.clockwise);
                        beta = Math.Min(beta, AlphaBetaAlgorithm(newPosition, depth - 1, true, false, alpha, beta));

                        if (alpha >= beta)
                            return alpha;
                    }
                    return beta;
                }
            }
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

                    Pentago newBoard = new Pentago(pentago.copyBoard());
                    newBoard.PlaceRock(assignedPlayer, i, j);
                    double value = CalculateBoardValue(newBoard);
                    possibleMoves.Add(new RockPlacement(i, j, value));
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
                        Pentago newBoard = new Pentago(pentago.copyBoard());
                        newBoard.PlaceRock(assignedPlayer, i, j);
                        double value = CalculateBoardValue(newBoard);
                        possibleMoves.Add(new SegmentRotation(i, j, k == 1 ? true : false, value));
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

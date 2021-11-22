using System;

namespace PentagoMinMax
{
    class PentagoSimulation
    {
        private Pentago pentago = null;
        private PentagoPlayer player1 = null;
        private PentagoPlayer player2 = null;

        public double player1AvgMoveTime;
        public double player2AvgMoveTime;

        public PentagoSimulation(Pentago pentago, PentagoPlayer player1, PentagoPlayer player2)
        {
            this.pentago = pentago;

            player1.SetBoard(pentago);
            player1.assignedPlayer = Player.Player1;
            player2.SetBoard(pentago);
            player2.assignedPlayer = Player.Player2;

            this.player1 = player1;
            this.player2 = player2;
        }

        int player1MoveAmount;
        double player1MoveTime;
        int player2MoveAmount;
        double player2MoveTime;

        public WinType StartSimulation()
        {
            Random rnd = new Random();
            DateTime beforeMoving;

            //decide who moves first
            PentagoPlayer currentPlayer = rnd.Next(0, 2) == 1 ? currentPlayer = player1 : currentPlayer = player2;

            do
            {
                beforeMoving = DateTime.Now;

                currentPlayer.PlaceRock();

                AddMoveTime(beforeMoving, currentPlayer.assignedPlayer);

                if (IsGameOver())
                {
                    CalculateAvgMoveTime();
                    break;
                }

                beforeMoving = DateTime.Now;

                currentPlayer.RotateSegment();

                AddMoveTime(beforeMoving,currentPlayer.assignedPlayer);

                if (IsGameOver())
                {
                    CalculateAvgMoveTime();
                    break;
                }

                //switch player
                if (currentPlayer == player1)
                    currentPlayer = player2;
                else
                    currentPlayer = player1;

            } while (true);

            return pentago.CheckWinType();
        }

        private bool IsGameOver()
        {
            return pentago.CheckWinType() != WinType.None ? true : false;
        }

        void AddMoveTime(DateTime then, Player player)
        {

            TimeSpan span = DateTime.Now - then;

            if (player == Player.Player1)
            {
                player1MoveAmount++;
                player1MoveTime += span.TotalMilliseconds;
            }
            else
            {
                player2MoveAmount++;
                player2MoveTime += span.TotalMilliseconds;
            }
        }

        void CalculateAvgMoveTime()
        {
            player1AvgMoveTime = Math.Round(player1MoveTime / (double) player1MoveAmount,2);
            player2AvgMoveTime = Math.Round(player2MoveTime / (double) player2MoveAmount,2);
        }
    }
}

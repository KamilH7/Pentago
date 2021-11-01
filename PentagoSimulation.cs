using System;

namespace PentagoMinMax
{
    class PentagoSimulation
    {
        Pentago pentago = null;
        PentagoPlayer player1 = null;
        PentagoPlayer player2 = null;
        bool automatic = true;

        public PentagoSimulation(Pentago pentago, PentagoPlayer player1, PentagoPlayer player2, bool automatic)
        {
            if ((player1.assignedPlayer == Player.Player1 && player2.assignedPlayer == Player.Player2) || (player1.assignedPlayer == Player.Player2 && player2.assignedPlayer == Player.Player1))
            {
                this.automatic = automatic;
                this.pentago = pentago;
                player1.SetBoard(pentago);
                player2.SetBoard(pentago);
                this.player1 = player1;
                this.player2 = player2;
            }
            else
            {
                Console.WriteLine("Bots have to be on opposing sides");
            }
        }

        public WinType StartSimulation()
        {
            if (pentago != null)
            {
                WinType winType = WinType.None;

                //decide who moves first
                Random rnd = new Random();
                PentagoPlayer currentPlayer = rnd.Next(0, 2) == 1 ? currentPlayer = player1 : currentPlayer = player2;

                if (!automatic)
                {
                    Console.WriteLine("Player1 is " + player1.playerType.ToString());
                    Console.WriteLine("Player2 is " + player2.playerType.ToString());

                    pentago.PrintBoard();

                    Console.WriteLine(currentPlayer.assignedPlayer.ToString() + " starts");
                }

                do
                {
                    if (!automatic)
                    {
                        Console.ReadLine();
                        Console.WriteLine(currentPlayer.assignedPlayer.ToString() + " Places a rock...");
                    }

                    currentPlayer.PlaceRock();
                    winType = pentago.CheckWinType();


                    if (winType != WinType.None)
                        break;

                    if (!automatic)
                    {
                        pentago.PrintBoard();
                        Console.ReadLine();
                        Console.WriteLine(currentPlayer.assignedPlayer.ToString() + " Rotates a segment...");
                    }

                    currentPlayer.RotateSegment();
                    winType = pentago.CheckWinType();

                    if (winType != WinType.None)
                        break;

                    if (!automatic)
                    {
                        pentago.PrintBoard();
                        Console.ReadLine();
                    }

                    //switch player
                    if (currentPlayer == player1)
                        currentPlayer = player2;
                    else
                        currentPlayer = player1;

                } while (true);

                if (!automatic)
                {
                    Console.WriteLine(winType.ToString());
                    pentago.PrintBoard();
                }

                return winType;
            }
            else
            {
                Console.WriteLine("Wrong settings");
                return WinType.None;
            }
        }
    }
}

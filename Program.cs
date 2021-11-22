using System;

namespace PentagoMinMax
{
    class Program
    {
        static void Main(string[] args)
        {
            //for debugging purposes - can be put into the constructor of a pentago object to perform tests         
            /*
            Field[,] customBoard = new Field[,] {
            {new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None)},
            {new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None)},
            {new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None)},
            {new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None)},
            {new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None)},
            {new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None), new Field(Player.None)},
            };
            Pentago pentago = new Pentago(customBoard);
            */


            NegaMax player1 = new NegaMax(Player.Player1, 5);
            //MiniMaxBot player1 = new MiniMaxBot(Player.Player1, 2);
            AlphaBeta player2 = new AlphaBeta(Player.Player2, 5);
            //RandomBot player2 = new RandomBot(Player.Player2);

            const double numOfSimulations = 1;
            double numWonByPlayer1 = 0;
            double numWonByPlayer2 = 0;
            double numofDraws = 0;
            double player1MoveTime = 0;
            double player2MoveTime = 0;

            for (int i = 0; i < numOfSimulations; i++)
            {      
                Console.WriteLine("Simulating... " + Math.Round((numWonByPlayer1 + numWonByPlayer2 + numofDraws) / numOfSimulations * 100, 2) + "% complete");
                PentagoSimulation simulation = new PentagoSimulation(new Pentago(), player1, player2);

                WinType outcome = simulation.StartSimulation();

                if (outcome == WinType.Player2Win)
                    numWonByPlayer2 += 1;
                if (outcome == WinType.Player1Win)
                    numWonByPlayer1 += 1;
                if (outcome == WinType.Draw)
                    numofDraws += 1;

                player1MoveTime += simulation.player1AvgMoveTime;
                player2MoveTime += simulation.player2AvgMoveTime;
            }
            Console.Clear();

            Console.WriteLine("% won by Player1(" + player1.playerType.ToString() + "):" + 100 * numWonByPlayer1 / numOfSimulations);
            Console.WriteLine("% won by Player2(" + player2.playerType.ToString() + "):" + 100 * numWonByPlayer2 / numOfSimulations);
            Console.WriteLine("% drawed:" + 100 * numofDraws / numOfSimulations);

            Console.WriteLine();

            player1MoveTime = Math.Round(player1MoveTime, 2);
            player2MoveTime = Math.Round(player2MoveTime, 2);

            Console.WriteLine("Avg movetime of Player1(" + player1.playerType.ToString() + "):" + player1MoveTime / numOfSimulations + "ms");
            Console.WriteLine("Avg movetime of Player2(" + player2.playerType.ToString() + "):" + player2MoveTime / numOfSimulations + "ms");
            Console.ReadLine();

        }
    }
}

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


            RandomBot player1 = new RandomBot(Player.Player1);
            //MiniMaxBot player1 = new MiniMaxBot(Player.Player1, 2);
            MiniMaxBot player2 = new MiniMaxBot(Player.Player2, 1);

            const double numOfSimulations = 1000;
            double numWonByPlayer1 = 0;
            double numWonByPlayer2 = 0;
            double numofDraws = 0;


            for (int i = 0; i < numOfSimulations; i++)
            {
                //Console.Clear();
                Console.WriteLine("Simulating... " + Math.Round((numWonByPlayer1 + numWonByPlayer2 + numofDraws) / numOfSimulations * 100, 2) + "% complete");
                PentagoSimulation simulation = new PentagoSimulation(new Pentago(), player1, player2, true);

                WinType outcome = simulation.StartSimulation();

                if (outcome == WinType.Player2Win)
                    numWonByPlayer2 += 1;
                if (outcome == WinType.Player1Win)
                    numWonByPlayer1 += 1;
                if (outcome == WinType.Draw)
                    numofDraws += 1;
            }

            Console.WriteLine("% won by Player1(" + player1.playerType.ToString() + "):" + 100 * numWonByPlayer1 / numOfSimulations); ;
            Console.WriteLine("% won by Player2(" + player2.playerType.ToString() + "):" + 100 * numWonByPlayer2 / numOfSimulations);
            Console.WriteLine("% drawed:" + 100 * numofDraws / numOfSimulations);
            Console.ReadLine();

        }
    }
}

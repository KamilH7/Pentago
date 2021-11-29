using System;

namespace Pentago
{
    class RandomBot : PentagoPlayer
    {
        Random rnd = new Random();

        public RandomBot() : base(PlayerType.Random)
        {

        }

        public override void PlaceRock()
        {
            int chosenX = 0;
            int chosenY = 0;

            do
            {
                chosenX = rnd.Next(0, 6);
                chosenY = rnd.Next(0, 6);

            } while (!pentago.CheckIfPossiblePlacement(chosenX, chosenY));

            pentago.PlaceRock(assignedPlayer, chosenX, chosenY);
        }

        public override void RotateSegment()
        {
            int chosenX = 0;
            int chosenY = 0;
            bool rotateClockwise = true;

            chosenX = rnd.Next(0, 2);
            chosenY = rnd.Next(0, 2);
            rotateClockwise = rnd.Next(0, 2) == 0 ? true : false;


            pentago.RotateSegment(chosenX, chosenY, rotateClockwise);
        }

    }
}

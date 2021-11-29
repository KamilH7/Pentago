namespace Pentago
{
    class Field
    {
        /* player = 0 - empty field
         * player = 1 - field belongs to player 1
         * player = 0 - field belongs to player 2
         */

        public Player player;

        public Field()
        {
            player = 0;
        }

        public Field(Player player)
        {
            this.player = player;
        }

        //for optimisation
        //verticall check
        public bool checkedVertically = false;
        //horizontall check
        public bool checkedHorizontally = false;
        //diagonall right check
        public bool checkedDiagonalRight = false;
        //verticall  leftcheck
        public bool checkedDiagonalLeft = false;

        public void resetChecks()
        {
            checkedVertically = false;
            checkedHorizontally = false;
            checkedDiagonalRight = false;
            checkedDiagonalLeft = false;
        }
    }
}

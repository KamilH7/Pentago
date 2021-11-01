namespace PentagoMinMax
{

    class PentagoPlayer
    {
        public Pentago pentago;
        public PlayerType playerType;
        public Player assignedPlayer;

        public PentagoPlayer(Player assignedPlayer, PlayerType type)
        {
            this.assignedPlayer = assignedPlayer;
            this.playerType = type;
        }

        public virtual void PlaceRock()
        {

        }

        public virtual void RotateSegment()
        {

        }

        public void SetBoard(Pentago pentago)
        {
            this.pentago = pentago;
        }
    }
}

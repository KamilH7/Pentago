namespace Pentago
{

    class PentagoPlayer
    {
        public int treeIterations = 0;
        public Pentago pentago;
        public PlayerType playerType;
        public Player assignedPlayer;

        public PentagoPlayer(PlayerType type)
        {
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

        public void SetPlayer(Player player)
        {
            this.assignedPlayer = player;
        }
    }
}

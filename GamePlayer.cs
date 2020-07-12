namespace TicTacToe 
{
    class GamePlayer
    {
        private string name, icon;
        private bool informedPlay;

        public GamePlayer(string name, string icon) 
        {
            this.name = name;
            this.icon = icon;
        }

        public string getName() 
        {
            return name;
        }

        public string getIcon() 
        {
            return icon;
        }

        public void setInformedPlayer(bool informedPlay)
        {
            this.informedPlay = informedPlay;
        }

        public bool hasInformedPlay()
        {
            return informedPlay;
        }
    }
}

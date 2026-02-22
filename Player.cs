namespace PBS6
{
    internal class Player
    {
        public string Name { get; set; } = "";
        public int HP { get; set; } = 100;
        public int Damage { get; set; }
        public int BashUsesLeft { get; set; } = 3;
        public int Kills { get; set; }
        public int Deaths { get; set; }
    }
}

namespace ClanBattleGame.Model.Units
{
    public sealed class ClanLeader
    {
        private static ClanLeader _instance;
        public string Name { get; private set; }

        private ClanLeader(string name)
        {
            Name = name;
        }

        public static ClanLeader GetInstance(string name)
        {
            if (_instance == null)
                _instance = new ClanLeader(name);

            return _instance;
        }
    }
}

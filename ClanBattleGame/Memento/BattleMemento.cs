using ClanBattleGame.Model;
using ClanBattleGame.Service;

namespace ClanBattleGame.Memento
{
    public class BattleMemento
    {
        public Clan SavedClanA { get; }
        public Clan SavedClanB { get; }
        public BattleState SavedState { get; }

        public string LeaderAName { get; }
        public string LeaderBName { get; }

        public BattleMemento(Clan clanA, Clan clanB, BattleState state, string leaderAName, string leaderBName)
        {
            SavedClanA = clanA.DeepCopy();
            SavedClanB = clanB.DeepCopy();
            //SavedState = state.DeepCopy();

            LeaderAName = leaderAName;
            LeaderBName = leaderBName;
        }
    }
}

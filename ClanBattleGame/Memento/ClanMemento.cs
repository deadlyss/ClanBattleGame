using ClanBattleGame.Model;

namespace ClanBattleGame.Memento
{
    public class ClanMemento
    {
        public Clan SavedState { get; }

        public ClanMemento(Clan clan)
        {
            SavedState = clan.DeepCopy(); // повна копія
        }
    }
}

using ClanBattleGame.Core;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Service;
using ClanBattleGame.View.Controls;
using System;

namespace ClanBattleGame.ViewModel
{
    public class BattleVM : ObservableObject
    {
        private Squad _selectedSquad;
        public Clan PlayerClan { get; }
        public Clan EnemyClan { get; }

        public HexFieldVM HexField { get; }

        public Squad SelectedSquad
        {
            get => _selectedSquad;
            set
            {
                _selectedSquad = value;
                HexField.SelectedPlayerSquad = value; // ← важливо
                OnPropertyChanged();
            }
        }

        private string _battleLog;
        public string BattleLog
        {
            get => _battleLog;
            set
            {
                _battleLog = value;
                OnPropertyChanged();
            }
        }

        public BattleVM(NavigationStore navStore, Clan player, Clan enemy)
        {
            PlayerClan = player.DeepCopy();
            EnemyClan = enemy.DeepCopy();

            BattleEngine engine = new BattleEngine();

            HexField = new HexFieldVM();
            HexField.OwnerVM = this;
            HexField.BattleEngine = engine;
            HexField.PlayerClan = PlayerClan;
            HexField.EnemyClan = EnemyClan;

            HexField.PlaceInitialUnits(PlayerClan, EnemyClan);
        }
    }
}

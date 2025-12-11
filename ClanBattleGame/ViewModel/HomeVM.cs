using ClanBattleGame.Core;
using ClanBattleGame.Interface;
using ClanBattleGame.Model;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Service;
using System.Collections.ObjectModel;

namespace ClanBattleGame.ViewModel
{
    public class HomeVM : ObservableObject
    {
        //public string ClanInfoLog
        //{
        //    get => _clanInfoLog;
        //    private set => Set(ref _clanInfoLog, value);
        //}
        //private string _clanInfoLog;

        public ObservableCollection<Squad> Squads { get; }
        private Squad _selectedSquad;

        public Squad SelectedSquad
        {
            get => _selectedSquad;
            set
            {
                if (Set(ref _selectedSquad, value))
                {
                    UpdateUnits();
                }
            }
        }

        public ObservableCollection<IUnit> Units { get; }

        private IUnit _selectedUnit;
        public IUnit SelectedUnit
        {
            get => _selectedUnit;
            set => Set(ref _selectedUnit, value);
        }
        //Команди
        public RelayCommand GoBattleCommand { get; }

        public HomeVM(NavigationStore navStore)
        {
            //ClanInfoLog = ClanInfoPrinter.PrintInfo(GameState.Instance.CurrentClan);
            //Відображення та вибір Sqads
            var playerClan = GameState.Instance.CurrentClan;
            Squads = new ObservableCollection<Squad>(playerClan.Squads);
            //Відображення та вибір Unit
            Units = new ObservableCollection<IUnit>();

            GoBattleCommand = new RelayCommand(o =>
            {
                new NavigationService(navStore,
                    () => new BattlePageVM(navStore, playerClan)).Navigate();
            });
        }

        private void UpdateUnits()
        {
            Units.Clear();

            if (SelectedSquad == null)
                return;

            foreach (var unit in SelectedSquad.Units)
                Units.Add(unit);

            OnPropertyChanged(nameof(Units));
        }
    }
}

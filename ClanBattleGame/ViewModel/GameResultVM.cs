using ClanBattleGame.Core;
using ClanBattleGame.Model.Etc;
using ClanBattleGame.Service;
using System.Windows;

namespace ClanBattleGame.ViewModel
{
    public class GameResultVM : ObservableObject
    {
        private readonly NavigationStore _navStore;
        private readonly BattleResultInfo _result;

        public GameResultVM(NavigationStore navStore, BattleResultInfo result)
        {
            _navStore = navStore;
            _result = result;

            ReturnCommand = new RelayCommand(_ =>
            {
                if (_ is Window w)
                    w.Close();
                if (result.PlayerWon)
                    new NavigationService(_navStore, () => new HomeVM(_navStore)).Navigate();
                else
                    new NavigationService(_navStore, () => new MainMenuVM(_navStore)).Navigate();
            });
        }

        public string ResultText => _result.PlayerWon ? "Вітаємо з перемогою!" : "Ви програли...";
        public int UnitsKilled => _result.UnitsKilled;
        public int UnitsLost => _result.UnitsLost;
        public int Turns => _result.Turns;
        public int MoneyEarned => _result.MoneyEarned;

        public RelayCommand ReturnCommand { get; }
    }
}

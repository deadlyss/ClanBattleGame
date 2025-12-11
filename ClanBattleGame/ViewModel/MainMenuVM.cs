using ClanBattleGame.Core;
using ClanBattleGame.Service;
using System.Windows;

namespace ClanBattleGame.ViewModel
{
    public class MainMenuVM : ObservableObject
    {
        public RelayCommand NewGame { get; }
        public RelayCommand Exit { get; }


        public MainMenuVM(NavigationStore navStore)
        {
            var state = GameState.Instance;

            NewGame = new RelayCommand(_ =>
            {
                new NavigationService(navStore, () => new NewGameVM(navStore)).Navigate();
            });

            Exit = new RelayCommand(_ =>
            {
                Application.Current.Shutdown();
            });
        }
    }
}

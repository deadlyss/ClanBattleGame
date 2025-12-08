using ClanBattleGame.Core;

namespace ClanBattleGame.Service
{
    public class NavigationStore : ObservableObject
    {
        private ObservableObject _currentViewModel;

        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }
    }
}

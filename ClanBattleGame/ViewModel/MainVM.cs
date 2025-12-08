using ClanBattleGame.Core;
using ClanBattleGame.Service;

namespace ClanBattleGame.ViewModel
{
    public class MainVM : ObservableObject
    {
        public NavigationStore NavigationStore { get; }

        public ObservableObject CurrentViewModel => NavigationStore.CurrentViewModel;

        public MainVM(NavigationStore store)
        {
            NavigationStore = store;

            store.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(NavigationStore.CurrentViewModel))
                    OnPropertyChanged(nameof(CurrentViewModel));
            };
        }
    }
}

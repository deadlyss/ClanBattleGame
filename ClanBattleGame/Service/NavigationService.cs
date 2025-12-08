using ClanBattleGame.Core;
using System;

namespace ClanBattleGame.Service
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<ObservableObject> _createViewModel;

        public NavigationService(NavigationStore store, Func<ObservableObject> createVM)
        {
            _navigationStore = store;
            _createViewModel = createVM;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}

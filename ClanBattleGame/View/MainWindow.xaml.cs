using ClanBattleGame.Service;
using ClanBattleGame.View.Pages;
using ClanBattleGame.ViewModel;
using System.Windows;

namespace ClanBattleGame.View
{
    public partial class MainWindow : Window
    {
        private readonly NavigationStore _navStore = new NavigationStore();

        public MainWindow()
        {
            InitializeComponent();

            _navStore.CurrentViewModel = new MainMenuVM(_navStore); // ← ОБОВʼЯЗКОВО!

            DataContext = new MainVM(_navStore);
        }
    }
}

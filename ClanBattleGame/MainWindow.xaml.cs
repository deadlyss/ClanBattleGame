using ClanBattleGame.ViewModel;
using System.Windows;

namespace ClanBattleGame
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new GameVM();
        }
    }
}

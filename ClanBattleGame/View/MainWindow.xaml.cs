using ClanBattleGame.ViewModel;
using System.Windows;

namespace ClanBattleGame.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new BattlePage();
        }
    }
}

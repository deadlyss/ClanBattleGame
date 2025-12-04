using ClanBattleGame.ViewModel;
using System.Windows.Controls;

namespace ClanBattleGame.View
{
    public partial class ArmyBuilderPage : Page
    {
        public ArmyBuilderPage()
        {
            InitializeComponent();
            DataContext = new ArmyBuilderVM();
        }
    }
}

using ClanBattleGame.ViewModel;
using System.Windows.Controls;

namespace ClanBattleGame.View
{
    public partial class BattlePage : Page
    {
        public BattlePage()
        {
            InitializeComponent();
            RenderHexGrid();
        }
        private HexFieldVM _hexVm = new HexFieldVM();

        private void RenderHexGrid()
        {
            HexCanvas.Children.Clear();

            foreach (var pos in _hexVm.Cells)
            {
                var cell = new HexCell();

                Canvas.SetLeft(cell, pos.x);
                Canvas.SetTop(cell, pos.y);

                HexCanvas.Children.Add(cell);
            }
        }
    }
}

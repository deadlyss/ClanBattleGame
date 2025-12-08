using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClanBattleGame.View.Cards
{
    public partial class UnitCard : UserControl
    {
        public UnitCard()
        {
            InitializeComponent();
        }

        public Brush BorderColor
        {
            get => (Brush)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(Brush), typeof(UnitCard), new PropertyMetadata(Brushes.Gray));
    }
}

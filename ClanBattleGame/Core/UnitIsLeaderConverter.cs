using ClanBattleGame.Interface;
using ClanBattleGame.Model.Units;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ClanBattleGame.Core
{
    public class UnitIsLeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IUnit unit = value as IUnit;

            if (unit == null || Leader.Instance == null || Leader.Instance.Unit == null)
                return false;

            // Порівнюємо той самий ОБ’ЄКТ юніта (не ім’я!)
            return ReferenceEquals(Leader.Instance.Unit, unit);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
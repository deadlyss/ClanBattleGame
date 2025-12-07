using ClanBattleGame.Interface;
using ClanBattleGame.Model.Units;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Linq;

namespace ClanBattleGame.Core
{
    public class UnitIsLeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IUnit unit = value as IUnit;
            if (unit == null)
                return false;

            // Перевіряємо: чи є юніт лідером будь-якого клану
            return Leader
                .GetAll()   // отримуємо всіх Multiton-лідерів
                .Any(l => ReferenceEquals(l.Unit, unit));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
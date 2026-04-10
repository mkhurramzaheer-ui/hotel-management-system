using System.Globalization;

namespace HotelMobileApp.NetMAUI.Helpers;

/// <summary>
/// Returns true when an integer value equals zero.
/// Use IsVisible="{Binding Collection.Count, Converter={StaticResource IntIsZeroConverter}}"
/// </summary>
public sealed class IntIsZeroConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is int count && count == 0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
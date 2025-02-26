using PollingApp.Models;
using System.Globalization;

namespace PollingApp.Converters
{
    public class TotalVotesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<PollOptionDto> options)
            {
                return options.Sum(o => o.VoteCount);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

namespace PollingApp.Models
{
    public class ChartDataPoint
    {
        public string Option { get; set; }
        public int Votes { get; set; }

        public ChartDataPoint(string option, int votes)
        {
            Option = option;
            Votes = votes;
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PollingApp.Models;
using PollingApp.Services;
using System.Collections.ObjectModel;

namespace PollingApp.PageViewModels
{
    public partial class PollResultViewModel : ObservableObject
    {
        private readonly IPollApiService _apiService;

        [ObservableProperty]
        private PollDto pollResult;

        [ObservableProperty]
        private ObservableCollection<ChartDataPoint> chartData;

        [ObservableProperty]
        private bool isBusy;

        public PollResultViewModel(IPollApiService apiService)
        {
            _apiService = apiService;
            ChartData = new ObservableCollection<ChartDataPoint>();
        }

        public async Task LoadPollResultAsync(int pollId)
        {
            IsBusy = true;
            try
            {
                // Retrieve the PollId from the query parameters
                PollResult = await _apiService.GetPollResultAsync(pollId);
                if (PollResult != null)
                {
                    // Prepare data for the Pie Chart
                    foreach (var option in PollResult.Options)
                    {
                        ChartData.Add(new ChartDataPoint(option.OptionText, option.VoteCount));
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }


        }

        [RelayCommand]
        private async Task GoToHome()
        {
            await Shell.Current.GoToAsync("///PollListPage");
        }
    }
}

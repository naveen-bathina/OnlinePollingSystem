using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PollingApp.Models;
using PollingApp.Pages;
using PollingApp.Services;
using System.Collections.ObjectModel;


namespace PollingApp.PageViewModels
{
    public partial class PollListViewModel : ObservableObject
    {
        private readonly IPollApiService _apiService;


        [ObservableProperty]
        private ObservableCollection<PollDto> polls;

        [ObservableProperty]
        private bool isBusy;

        public PollListViewModel(IPollApiService apiService)
        {
            _apiService = apiService;
            LoadPolls();
        }


        public async void LoadPolls()
        {
            IsBusy = true;
            try
            {
                var polls = await _apiService.GetPollsAsync();
                Polls = new ObservableCollection<PollDto>(polls);
            }
            finally
            {
                IsBusy = false; // Hide progress indicator
            }
        }

        [RelayCommand]
        private async Task NavigateToPollDetail(PollDto poll)
        {
            await Shell.Current.GoToAsync($"{nameof(PollDetailPage)}?PollId={poll.Id}");
        }

        [RelayCommand]
        private async Task NavigateToPollResult(PollDto poll)
        {
            await Shell.Current.GoToAsync($"{nameof(PollResultPage)}?PollId={poll.Id}");
        }
    }
}
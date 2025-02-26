using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PollingApp.Models;
using PollingApp.Pages;
using PollingApp.Services;

namespace PollingApp.PageViewModels
{
    public partial class PollDetailViewModel : ObservableObject
    {
        private readonly IPollApiService _apiService;

        [ObservableProperty]
        private PollDto selectedPoll;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedPoll))]
        private int pollId;

        [ObservableProperty]
        private PollOptionDto selectedOption;

        [ObservableProperty]
        private bool isBusy;

        public PollDetailViewModel(IPollApiService apiService)
        {
            _apiService = apiService;
        }

        partial void OnPollIdChanged(int value)
        {
            IsBusy = true;
            try
            {
                // Load the poll when the PollId changes
                LoadPollAsync(value).ConfigureAwait(false);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadPollAsync(int pollId)
        {
            IsBusy = true;
            try
            {
                // Retrieve the PollId from the query parameters
                SelectedPoll = await _apiService.GetPollAsync(pollId);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SubmitVote()
        {
            IsBusy = true;
            try
            {
                var vote = new CastVoteModel { OptionId = SelectedOption.Id };
                await _apiService.SubmitVoteAsync(SelectedPoll.Id, vote);
                await Shell.Current.GoToAsync($"{nameof(PollResultPage)}?PollId={SelectedPoll.Id}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

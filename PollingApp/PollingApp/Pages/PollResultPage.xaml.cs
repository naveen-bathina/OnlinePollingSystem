using PollingApp.PageViewModels;

namespace PollingApp.Pages;

[QueryProperty(nameof(PollId), "PollId")]
public partial class PollResultPage : ContentPage
{
    public int PollId
    {
        set
        {
            if (BindingContext is PollResultViewModel viewModel)
            {
                viewModel.LoadPollResultAsync(value).ConfigureAwait(false);
            }
        }
    }

    public PollResultPage(PollResultViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("///PollListPage").ConfigureAwait(false);
        return false;
    }
}
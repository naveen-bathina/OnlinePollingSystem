using PollingApp.PageViewModels;

namespace PollingApp.Pages;

public partial class PollListPage : ContentPage
{
    public PollListPage(PollListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as PollListViewModel)?.LoadPolls();
    }
}
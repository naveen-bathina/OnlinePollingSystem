using PollingApp.Models;
using PollingApp.PageViewModels;

namespace PollingApp.Pages;

[QueryProperty(nameof(PollId), "PollId")]
public partial class PollDetailPage : ContentPage
{
    public int PollId
    {
        set
        {
            if (BindingContext is PollDetailViewModel viewModel)
            {
                viewModel.PollId = value;
            }
        }
    }
    public PollDetailPage(PollDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value && sender is RadioButton radioButton && radioButton.BindingContext is PollOptionDto selectedOption)
        {
            var viewModel = BindingContext as PollDetailViewModel;
            if (viewModel != null)
            {
                viewModel.SelectedOption = selectedOption;
            }
        }
    }
}
using ElectionAppMAUI.Pages;
using ElectionAppMAUI.ViewModels;

namespace ElectionAppMAUI.Views;

public partial class CandidatesPage : ContentPage
{
    private readonly CandidatesViewModel vm;

    public CandidatesPage()
    {
        InitializeComponent();

        vm = new CandidatesViewModel();
        BindingContext = vm;

        vm.LoadCommand.Execute(null);
    }

    private void OpenDetails_Clicked(object sender, EventArgs e)
    {
        int id = (int)((Button)sender).CommandParameter;
        Navigation.PushAsync(new CandidateDetailsPage(id));
    }

    private void OnAddCandidate(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddCandidatePage());
    }

    private void OnOpenDetails(object sender, EventArgs e)
    {
        int id = (int)((Button)sender).CommandParameter;
        Navigation.PushAsync(new CandidateDetailsPage(id));
    }

    private void OnEditCandidate(object sender, EventArgs e)
    {
        int id = (int)((Button)sender).CommandParameter;
        Navigation.PushAsync(new UpdateCandidatePage(id));
    }

    private void OnDeleteCandidate(object sender, EventArgs e)
    {
        int id = (int)((Button)sender).CommandParameter;
        vm.DeleteCommand.Execute(id);
    }
}

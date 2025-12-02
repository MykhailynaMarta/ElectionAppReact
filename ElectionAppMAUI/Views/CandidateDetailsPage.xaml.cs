using ElectionAppMAUI.ViewModels;

namespace ElectionAppMAUI.Pages;

public partial class CandidateDetailsPage : ContentPage
{
    public CandidateDetailsPage(int candidateId)
    {
        InitializeComponent();
        BindingContext = new CandidateDetailsViewModel(candidateId);
    }
}

namespace ElectionAppMAUI.Views;

public partial class UpdateCandidatePage : ContentPage
{
    public UpdateCandidatePage(int id)
    {
        InitializeComponent();
        BindingContext = new UpdateCandidateViewModel(id);

        Loaded += (s, e) =>
        {
            (BindingContext as UpdateCandidateViewModel)?.LoadCommand.Execute(null);
        };
    }
}

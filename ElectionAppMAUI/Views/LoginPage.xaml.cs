using ElectionAppMAUI.ViewModels;

namespace ElectionAppMAUI.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnPickFile(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Âטבונ³עü JSON פאיכ",
            //FileTypes = FilePickerFileType.All
        });

        if (result == null) return;

        using var stream = await result.OpenReadAsync();
        using var reader = new StreamReader(stream);
        var text = await reader.ReadToEndAsync();

        (BindingContext as LoginViewModel)?.ProcessJson(text);
    }
}

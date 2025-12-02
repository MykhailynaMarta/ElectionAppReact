using System.Text.Json;
using System.Net.Http.Json;
using System.Windows.Input;
using ElectionAppMAUI.Views;

namespace ElectionAppMAUI.ViewModels;

public class LoginViewModel : BindableObject
{
    private const string API = "http://localhost:5209/api/authorization";

    public string SelectedBank { get; set; }
    public string Password { get; set; }

    public List<string> Banks { get; } =
        new() { "privat24", "monobank", "oschad", "universal", "admin" };

    public string FileName { get; set; }
    public string Error { get; set; }
    public bool Loading { get; set; }

    private LoginFileModel _fileData;

    public ICommand LoginCommand => new Command(async () => await Login());

    public void ProcessJson(string json)
    {
        try
        {
            _fileData = JsonSerializer.Deserialize<LoginFileModel>(json);
            FileName = "Файл завантажено ✔";
            OnPropertyChanged(nameof(FileName));
        }
        catch
        {
            Error = "JSON файл некоректний";
            OnPropertyChanged(nameof(Error));
        }
    }

    private async Task Login()
    {
        Error = "";
        Loading = true;
        OnPropertyChanged(nameof(Loading));

        if (_fileData == null || string.IsNullOrEmpty(Password) || SelectedBank == null)
        {
            Error = "Заповніть всі поля";
            Loading = false;
            OnPropertyChanged(nameof(Loading));
            OnPropertyChanged(nameof(Error));
            return;
        }

        var payload = new Dictionary<string, object>
        {
            ["bank"] = SelectedBank,
            ["password"] = Password,
            ["fullname"] = _fileData.fullname
        };

        var handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };

        using var client = new HttpClient(handler);

        var resp = await client.PostAsJsonAsync($"{API}/check", payload);

        if (!resp.IsSuccessStatusCode)
        {
            Error = "Невірні дані!";
            Loading = false;
            OnPropertyChanged(nameof(Error));
            OnPropertyChanged(nameof(Loading));
            return;
        }

        var json = await resp.Content.ReadAsStringAsync();
        System.Diagnostics.Debug.WriteLine("SERVER JSON: " + json);

        var obj = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>();

        string role = obj.ContainsKey("status") ? obj["status"] : "";

        await SecureStorage.SetAsync("role", role);

        AppState.IsAdmin = (role == "admin-valid" || role == "admin");

        Loading = false;
        OnPropertyChanged(nameof(Loading));

        await Application.Current.MainPage.Navigation.PushAsync(new CandidatesPage());
    }
}

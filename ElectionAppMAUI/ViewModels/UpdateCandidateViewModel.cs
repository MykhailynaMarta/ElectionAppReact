using System.ComponentModel;
using System.Net.Http.Json;
using System.Windows.Input;
using ElectionAppMAUI;

public class UpdateCandidateViewModel : INotifyPropertyChanged
{
    private const string API_BASE = "http://localhost:5209/api/candidates";

    public Candidate Candidate { get; set; } = new Candidate();

    public ICommand LoadCommand { get; }
    public ICommand SaveCommand { get; }

    private readonly int _id;

    public UpdateCandidateViewModel(int id)
    {
        _id = id;

        LoadCommand = new Command(async () => await Load());
        SaveCommand = new Command(async () => await Save());
    }

    private async Task Load()
    {
        using var client = new HttpClient();
        Candidate data = await client.GetFromJsonAsync<Candidate>($"{API_BASE}/{_id}");

        Candidate = data;
        OnPropertyChanged(nameof(Candidate));
    }

    private async Task Save()
    {
        using var client = new HttpClient();

        var response = await client.PutAsJsonAsync($"{API_BASE}/{_id}", Candidate);

        if (response.IsSuccessStatusCode)
        {
            await App.Current.MainPage.DisplayAlert("Успіх", "Кандидата оновлено!", "OK");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Помилка", "Не вдалося оновити", "OK");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

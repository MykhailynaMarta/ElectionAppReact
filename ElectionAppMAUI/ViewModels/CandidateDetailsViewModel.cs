using System.Net.Http.Json;
using System.ComponentModel;

public class CandidateDetailsViewModel : INotifyPropertyChanged
{
    private const string API_BASE = "http://localhost:5209/api/candidates";

    private Candidate _candidate;
    public Candidate Candidate
    {
        get => _candidate;
        set
        {
            _candidate = value;
            OnPropertyChanged(nameof(Candidate));
        }
    }

    private bool _isLoading = true;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    public int Id { get; set; }

    public CandidateDetailsViewModel(int id)
    {
        Id = id;            // ❗ MUST SET ID
        LoadCandidate();    // ❗ call the correct method
    }

    private async void LoadCandidate()
    {
        try
        {
            using var client = new HttpClient();
            Candidate = await client.GetFromJsonAsync<Candidate>($"{API_BASE}/{Id}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Windows.Input;

namespace ElectionAppMAUI.ViewModels
{
    public class CandidatesViewModel : INotifyPropertyChanged
    {
        private const string API_BASE = "http://localhost:5209/api";

        public ObservableCollection<Candidate> Candidates { get; set; }
            = new ObservableCollection<Candidate>();

        public ObservableCollection<Candidate> Filtered { get; set; }
            = new ObservableCollection<Candidate>();

        public bool IsAdmin => AppState.IsAdmin;

        private bool _loading;
        public bool Loading
        {
            get => _loading;
            set { _loading = value; OnPropertyChanged(nameof(Loading)); }
        }

        private string _search = "";
        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                OnPropertyChanged(nameof(Search));
                FilterCandidates();
            }
        }

        public ICommand LoadCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand VoteCommand { get; }

        public CandidatesViewModel()
        {
            LoadCommand = new Command(async () => await LoadCandidates());
            DeleteCommand = new Command<int>(async (id) => await DeleteCandidate(id));
            VoteCommand = new Command<int>(async (id) => await Vote(id));

            OnPropertyChanged(nameof(IsAdmin));
        }

        private async Task LoadCandidates()
        {
            try
            {
                Loading = true;

                using var client = new HttpClient();
                var list = await client.GetFromJsonAsync<List<Candidate>>($"{API_BASE}/candidates");

                Candidates.Clear();
                foreach (var c in list)
                    Candidates.Add(c);

                FilterCandidates();
            }
            finally
            {
                Loading = false;
            }
        }

        private async Task DeleteCandidate(int id)
        {
            if (!IsAdmin) return;

            using var client = new HttpClient();
            await client.DeleteAsync($"{API_BASE}/candidates/{id}");

            var item = Candidates.FirstOrDefault(x => x.Id == id);
            if (item != null)
                Candidates.Remove(item);

            FilterCandidates();
        }

        private void FilterCandidates()
        {
            Filtered.Clear();
            string s = Search.ToLower();

            foreach (var c in Candidates)
            {
                if (c.Name.ToLower().Contains(s) ||
                    c.Party.ToLower().Contains(s))
                {
                    Filtered.Add(c);
                }
            }
        }

        // ---------------------
        //      ГОЛОСУВАННЯ
        // ---------------------
        private async Task Vote(int candidateId)
        {
            try
            {
                var payload = new
                {
                    CandidateId = candidateId
                };

                using var client = new HttpClient();
                var response = await client.PostAsJsonAsync($"{API_BASE}/voting/vote", payload);

                if (!response.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Помилка", "Не вдалося проголосувати", "OK");
                    return;
                }

                var data = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                string hash = data["hash"];

                await App.Current.MainPage.DisplayAlert("Успіх!", $"Голос прийнято.\nХеш: {hash}", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Помилка", ex.Message, "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

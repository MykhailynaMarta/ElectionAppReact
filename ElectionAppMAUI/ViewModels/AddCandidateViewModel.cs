using System.ComponentModel;
using System.Net.Http.Json;
using System.Windows.Input;
using ElectionAppMAUI;

namespace ElectionAppMAUI.ViewModels
{
    public class AddCandidateViewModel : INotifyPropertyChanged
    {
        private const string API_CANDIDATES = "http://localhost:5209/api/candidates";
        private const string API_UPLOAD = "http://localhost:5209/api/upload";

        public Candidate Candidate { get; set; } = new Candidate();

        public FileResult SelectedFile { get; set; }

        public ICommand PickFileCommand { get; }
        public ICommand SubmitCommand { get; }

        private string _photoPreview;
        public string PhotoPreview
        {
            get => _photoPreview;
            set
            {
                _photoPreview = value;
                OnPropertyChanged(nameof(PhotoPreview));
            }
        }

        public AddCandidateViewModel()
        {
            PickFileCommand = new Command(async () => await PickFile());
            SubmitCommand = new Command(async () => await Submit());
        }

        private async Task PickFile()
        {
            try
            {
                var file = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Виберіть фото кандидата",
                    FileTypes = FilePickerFileType.Images
                });

                if (file != null)
                {
                    SelectedFile = file;

                    // Показуємо фото в інтерфейсі
                    var stream = await file.OpenReadAsync();
                    PhotoPreview = file.FullPath;
                }
            }
            catch { }
        }

        private async Task Submit()
        {
            if (string.IsNullOrWhiteSpace(Candidate.Name) ||
                string.IsNullOrWhiteSpace(Candidate.Party) ||
                string.IsNullOrWhiteSpace(Candidate.Description) ||
                Candidate.Birthdate == default)
            {
                await App.Current.MainPage.DisplayAlert("Помилка", "Заповніть всі поля!", "OK");
                return;
            }

            string uploadedUrl = "";

            // ----------------- 1. Upload photo -----------------
            if (SelectedFile != null)
            {
                var content = new MultipartFormDataContent();
                var stream = await SelectedFile.OpenReadAsync();
                var fileContent = new StreamContent(stream);

                content.Add(fileContent, "file", SelectedFile.FileName);

                using var client = new HttpClient();
                var uploadRes = await client.PostAsync(API_UPLOAD, content);

                if (!uploadRes.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Помилка", "Фото не завантажено!", "OK");
                    return;
                }

                var result = await uploadRes.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                uploadedUrl = result["url"];
            }

            // ----------------- 2. Create candidate -----------------
            Candidate.PhotoUrl = uploadedUrl;

            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(API_CANDIDATES, Candidate);

                if (response.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Успіх", "Кандидата додано!", "OK");
                    Candidate = new Candidate();
                    PhotoPreview = null;
                    OnPropertyChanged(nameof(Candidate));
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Помилка", "Не вдалося створити кандидата.", "OK");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

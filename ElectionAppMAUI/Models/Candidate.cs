public class Candidate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Party { get; set; }
    public DateTime Birthdate { get; set; }
    public string PhotoUrl { get; set; }

    public ImageSource PhotoSource
    {
        get
        {
            if (PhotoUrl != null && PhotoUrl.StartsWith("data:image"))
            {
                var base64 = PhotoUrl.Split(',')[1];
                byte[] bytes = Convert.FromBase64String(base64);
                return ImageSource.FromStream(() => new MemoryStream(bytes));
            }

            return ImageSource.FromUri(new Uri(PhotoUrl));
        }
    }

    public string Description { get; set; }
}

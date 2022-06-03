
namespace FileUpload.Models
{
    public class FilmImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPoster { get; set; }
        public int FilmId { get; set; }
        public Film Film { get; set; }
    }
}

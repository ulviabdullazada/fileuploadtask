using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileUpload.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<FilmImage> Images{ get; set; }
        [NotMapped,Required]
        public IFormFile CoverImage { get; set; }
        [NotMapped]
        public List<IFormFile> Files { get; set; }
    }
}

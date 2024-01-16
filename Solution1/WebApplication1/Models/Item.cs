using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Item: BaseModel
    {
        [Required,MaxLength(32)]
        public string Title { get; set; }
        [Required,MaxLength(256)]
        public string Description { get; set; }
        [Required, MaxLength(64)]
        public string ImagePath { get; set; }
    }
}

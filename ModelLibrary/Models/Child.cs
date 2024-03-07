using System.ComponentModel.DataAnnotations;

namespace ModelLibrary.Models
{
    public class Child
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Use { get; set; }
        [Required]
        public string Family { get; set; }
        public string[] Given { get; set; }        
    }
}

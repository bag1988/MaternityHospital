
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ModelLibrary.Models
{
    public class Patient
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [ForeignKey("Id")]
        public Child Name { get; set; }
        public string Gender { get; set; }
        [Required]
        public DateTimeOffset BirthDate { get; set; }
        public bool Active { get; set; }
        [JsonIgnore]
        [ForeignKey("Gender")]
        public Gender? GenderTab { get; set; }
        [JsonIgnore]
        [ForeignKey("Active")]
        public Active? ActiveTab { get; set; }
    }
}

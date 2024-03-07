using System.ComponentModel.DataAnnotations;

namespace ModelLibrary.Models
{
    public class Gender
    {
        [Key]
        public string Value { get; set; }
        public string Name { get; set; }
    }   
}

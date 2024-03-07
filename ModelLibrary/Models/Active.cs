using System.ComponentModel.DataAnnotations;

namespace ModelLibrary.Models
{
    public class Active
    {
        [Key]
        public bool Value { get; set; }
        public string Name { get; set; }
    }
}

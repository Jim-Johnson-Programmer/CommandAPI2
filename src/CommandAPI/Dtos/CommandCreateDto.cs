using System.ComponentModel.DataAnnotations;

namespace CommandAPI.Dtos
{
    public class CommandCreateDto
    {
        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }
        
        [MaxLength(250)]
        public string Platform { get;set; }
        
        [MaxLength(250)]
        public string CommandLine { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace VirtualLibrary.Models;

public partial class PublisherDTO
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
}

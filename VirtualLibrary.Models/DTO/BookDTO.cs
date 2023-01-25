using System.ComponentModel.DataAnnotations;

namespace VirtualLibrary.Models;

public partial class BookDTO
{
    [Required]
    public int publisherId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    public int Isbn { get; set; }

    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = null!;

}

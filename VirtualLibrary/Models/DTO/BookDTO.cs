using System.ComponentModel.DataAnnotations;

namespace VirtualLibrary.Models;

public partial class BookDTO
{
    [Required]
    public int Isbn { get; set; }
    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = null!;
    [Required]
    public int ItemId { get; set; }
}

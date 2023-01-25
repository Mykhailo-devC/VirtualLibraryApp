using System.ComponentModel.DataAnnotations;

namespace VirtualLibrary.Models;

public partial class ArticleDTO
{
    [Required]
    public int publisherId { get; set; }

    [Required]
    public int Version { get; set; }

    [Required]
    [MaxLength(100)]
    public string Author { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    public int MagazineId { get; set; } = -1;
}

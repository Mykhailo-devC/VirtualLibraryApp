using System.ComponentModel.DataAnnotations;

namespace VirtualLibrary.Models;

public partial class ArticleDTO
{
    [Required]
    public int Version { get; set; }
    [Required]
    [MaxLength(100)]
    public string Author { get; set; }
    [Required]
    public int ItemId { get; set; }
    [Required]
    public int MagazineId { get; set; }
}

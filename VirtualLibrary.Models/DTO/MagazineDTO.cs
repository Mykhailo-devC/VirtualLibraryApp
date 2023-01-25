using System.ComponentModel.DataAnnotations;

namespace VirtualLibrary.Models;

public partial class MagazineDTO
{
    [Required]
    public int publisherId { get; set; }

    [Required]
    public int IssueNumber { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    public int ArticleId { get; set; } = -1;
}

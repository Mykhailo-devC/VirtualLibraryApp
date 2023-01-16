using System.ComponentModel.DataAnnotations;

namespace VirtualLibrary.Models;

public partial class MagazineDTO
{
    [Required]
    public int IssueNumber { get; set; }
    [Required]
    public int ItemId { get; set; }
}

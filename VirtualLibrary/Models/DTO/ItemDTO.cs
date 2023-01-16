using System.ComponentModel.DataAnnotations;

namespace VirtualLibrary.Models;

public partial class ItemDTO
{
    [Required]
    [MaxLength(50)]
    public string ItemName { get; set; }
    [Required]
    public int PublisherId { get; set; }
}

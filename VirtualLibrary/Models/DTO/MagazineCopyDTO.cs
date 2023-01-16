using System.ComponentModel.DataAnnotations;

namespace VirtualLibrary.Models;

public partial class MagazineCopyDTO
{
    [Required]
    public int MagazineId { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace VirtualLibrary.Models;

public partial class BookCopyDTO
{
    [Required]
    public int BookId { get; set; }
}

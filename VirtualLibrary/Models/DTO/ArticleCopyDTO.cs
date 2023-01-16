using System.ComponentModel.DataAnnotations;

namespace VirtualLibrary.Models;

public partial class ArticleCopyDTO
{
    [Required]
    public int ArticleId { get; set; }
}

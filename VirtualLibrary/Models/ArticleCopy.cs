
namespace VirtualLibrary.Models;

public partial class ArticleCopy
{
    public int CopyId { get; set; }

    public int ArticleId { get; set; }

    public virtual Article Article { get; set; }
}

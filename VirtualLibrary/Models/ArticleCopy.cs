namespace VirtualLibrary.Models;

public partial class ArticleCopy
{
    public int CopyId { get; set; }

    public int Version { get; set; }

    public int ArticleId { get; set; }

    public int ItemId { get; set; }

    public virtual Article Article { get; set; }

    public virtual Item Item { get; set; }
}

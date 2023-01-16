namespace VirtualLibrary.Models;

public partial class MagazineArticle
{
    public int Id { get; set; }

    public int MagazineId { get; set; }

    public int ArticleId { get; set; }

    public virtual Article Article { get; set; }

    public virtual Magazine Magazine { get; set; }
}

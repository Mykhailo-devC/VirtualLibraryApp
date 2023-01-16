
namespace VirtualLibrary.Models;

public partial class Article
{
    public Article()
    {

    }
    public int Id { get; set; }

    public int Version { get; set; }

    public string Author { get; set; } = null!;

    public int ItemId { get; set; }

    public virtual ICollection<ArticleCopy> ArticleCopies { get; } = new List<ArticleCopy>();

    public virtual Item Item { get; set; }

    public virtual ICollection<MagazineArticle> MagazineArticles { get; } = new List<MagazineArticle>();
}

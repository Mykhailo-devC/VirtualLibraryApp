namespace VirtualLibrary.Models;

public partial class Magazine
{
    public int Id { get; set; }

    public int IssueNumber { get; set; }

    public int ItemId { get; set; }

    public virtual Item Item { get; set; }

    public virtual ICollection<MagazineArticle> MagazineArticles { get; } = new List<MagazineArticle>();

    public virtual ICollection<MagazineCopy> MagazineCopies { get; } = new List<MagazineCopy>();
}

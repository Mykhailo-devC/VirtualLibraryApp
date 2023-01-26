namespace VirtualLibrary.Models;

public partial class Item
{
    public int Id { get; set; }

    public DateTime PublishDate { get; set; } 

    public int PublisherId { get; set; }

    public ArticleCopy ArticleCopy { get; set; }

    public BookCopy BookCopy { get; set; }

    public MagazineCopy MagazineCopy { get; set; }

    public Publisher Publisher { get; set; } = null!;
}

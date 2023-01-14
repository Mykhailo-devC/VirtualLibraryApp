using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models;

public partial class Item
{
    public int Id { get; set; }

    public string ItemName { get; set; } = null!;

    public DateTime PublishDate { get; set; }

    public int PublisherId { get; set; }

    public virtual ICollection<Article> Articles { get; } = new List<Article>();

    public virtual ICollection<Book> Books { get; } = new List<Book>();

    public virtual ICollection<Magazine> Magazines { get; } = new List<Magazine>();

    public virtual Publisher Publisher { get; set; }
}

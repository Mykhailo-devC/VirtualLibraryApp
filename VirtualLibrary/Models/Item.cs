using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models;

public partial class Item
{
    public int Id { get; set; }

    public DateTime PublishDate { get; set; }

    public int PublisherId { get; set; }

    public virtual ICollection<ArticleCopy> ArticleCopies { get; } = new List<ArticleCopy>();

    public virtual ICollection<BookCopy> BookCopies { get; } = new List<BookCopy>();

    public virtual ICollection<MagazineCopy> MagazineCopies { get; } = new List<MagazineCopy>();

    public virtual Publisher Publisher { get; set; }
}

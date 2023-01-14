using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models;

public partial class Book
{
    public int Id { get; set; }

    public int Isbn { get; set; }

    public string Author { get; set; } = null!;

    public int ItemId { get; set; }

    public virtual ICollection<BookCopy> BookCopies { get; } = new List<BookCopy>();

    public virtual Item Item { get; set; }
}

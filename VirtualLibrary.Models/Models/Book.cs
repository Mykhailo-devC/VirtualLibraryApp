using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Author { get; set; } = null!;

    public virtual ICollection<BookCopy> BookCopies { get; } = new List<BookCopy>();
}

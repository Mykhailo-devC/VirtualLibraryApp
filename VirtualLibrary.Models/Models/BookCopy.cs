using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models;

public partial class BookCopy
{
    public int CopyId { get; set; }

    public int Isbn { get; set; }

    public int BookId { get; set; }

    public int ItemId { get; set; }

    public Book Book { get; set; } = null!;

    public Item Item { get; set; } = null!;
}

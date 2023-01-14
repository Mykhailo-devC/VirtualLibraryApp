using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models;

public partial class BookCopy
{
    public int CopyId { get; set; }

    public int BookId { get; set; }

    public virtual Book Book { get; set; }
}

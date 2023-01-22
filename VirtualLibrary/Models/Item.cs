using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models;

public partial class Item
{
    public int Id { get; set; }

    public DateTime PublishDate { get; set; } 

    public int PublisherId { get; set; }

    public virtual ArticleCopy ArticleCopy { get; set; }

    public virtual BookCopy BookCopy { get; set; }

    public virtual MagazineCopy MagazineCopy { get; set; }

    public virtual Publisher Publisher { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models;

public partial class Magazine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MagazineArticle> MagazineArticles { get; } = new List<MagazineArticle>();

    public virtual ICollection<MagazineCopy> MagazineCopies { get; } = new List<MagazineCopy>();
}

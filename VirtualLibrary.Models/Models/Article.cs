using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models;

public partial class Article
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Author { get; set; } = null!;

    public virtual ICollection<ArticleCopy> ArticleCopies { get; } = new List<ArticleCopy>();

    public virtual ICollection<MagazineArticle> MagazineArticles { get; } = new List<MagazineArticle>();
}

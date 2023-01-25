using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models;

public partial class ArticleCopy
{
    public int CopyId { get; set; }

    public int Version { get; set; }

    public int ArticleId { get; set; }

    public int ItemId { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}

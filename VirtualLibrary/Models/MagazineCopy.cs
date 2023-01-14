using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models;

public partial class MagazineCopy
{
    public int CopyId { get; set; }

    public int MagazineId { get; set; }

    public virtual Magazine Magazine { get; set; }
}

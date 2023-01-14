using System;
using System.Collections.Generic;

namespace VirtualLibrary.Models;

public partial class Publisher
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Item> Items { get; } = new List<Item>();
}

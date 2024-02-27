using System;
using System.Collections.Generic;

namespace H5ServersideProgrammering.Data;

public partial class TodoItem
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? ItemText { get; set; }
}

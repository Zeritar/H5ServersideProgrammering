using System;
using System.Collections.Generic;

namespace H5ServersideProgrammering.Data;

public partial class UserCpr
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Cpr { get; set; } = null!;
}

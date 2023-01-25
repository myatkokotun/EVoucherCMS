using System;
using System.Collections.Generic;

namespace EVMSWeb.Models;

public class TbCmsuser
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public DateTime? Accesstime { get; set; }
}

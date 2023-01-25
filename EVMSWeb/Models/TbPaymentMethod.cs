using System;
using System.Collections.Generic;

namespace EVMSWeb.Models;

public class TbPaymentMethod
{
    public int Id { get; set; }

    public string? MethodName { get; set; }

    public int? Discount { get; set; }
}

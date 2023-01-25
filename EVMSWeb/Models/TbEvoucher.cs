using System;
using System.Collections.Generic;

namespace EVMSWeb.Models;

public class TbEvoucher
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public byte[]? Image { get; set; }

    public decimal? Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public int? Quantity { get; set; }
    public Boolean? IsPaid { get; set; }

    public string? BuyType { get; set; }

    public string? MyselfName { get; set; }

    public string? MyselfPhone { get; set; }

    public int? MyselfMaxLimit { get; set; }

    public string? OtherName { get; set; }

    public string? OtherPhone { get; set; }

    public int? OtherGiftLimit { get; set; }

    public int? OtherBuyLimit { get; set; }

    public DateTime? Accesstime { get; set; }
}

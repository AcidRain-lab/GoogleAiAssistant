using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class RegularPayment
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public string PaymentDetails { get; set; } = null!;

    public string PaymentType { get; set; } = null!;

    public string Frequency { get; set; } = null!;

    public DateOnly NextPaymentDate { get; set; }

    public virtual Client Client { get; set; } = null!;
}

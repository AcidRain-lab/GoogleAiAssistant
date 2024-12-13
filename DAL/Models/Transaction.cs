using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public double Quantity { get; set; }

    public int ActionType { get; set; }

    public double Balance { get; set; }
}

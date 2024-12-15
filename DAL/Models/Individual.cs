using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Individual
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? PassportData { get; set; }

    public string? TaxId { get; set; }
}

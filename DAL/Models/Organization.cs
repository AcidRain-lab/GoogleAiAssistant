using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Organization
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string RegistrationNumber { get; set; } = null!;

    public string? TaxId { get; set; }

    public string? Industry { get; set; }

    public int? NumberOfEmployees { get; set; }

    public decimal? AnnualTurnover { get; set; }

    public Guid? ContactPersonId { get; set; }
}

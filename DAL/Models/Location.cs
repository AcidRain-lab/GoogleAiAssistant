using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Location
{
    public Guid Id { get; set; }

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string? SuiteAptUnit { get; set; }

    public int? Zip { get; set; }

    public int StateId { get; set; }

    public int CountryId { get; set; }

    public string? GoogleMapsPlaceId { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public virtual ICollection<Contact> ContactBilllingAddresses { get; set; } = new List<Contact>();

    public virtual ICollection<Contact> ContactMailingAddresses { get; set; } = new List<Contact>();

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Lead> LeadBillingAddresses { get; set; } = new List<Lead>();

    public virtual ICollection<Lead> LeadLocationAddresses { get; set; } = new List<Lead>();

    public virtual ICollection<Lead> LeadMailingAddresses { get; set; } = new List<Lead>();

    public virtual State State { get; set; } = null!;
}

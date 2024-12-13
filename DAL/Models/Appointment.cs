using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Appointment
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ContactId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Subject { get; set; } = null!;

    public string? Description { get; set; }

    public string? GoogleMapsPlaceId { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? EventUrl { get; set; }

    public string? EventLocation { get; set; }

    public string? EventLabel { get; set; }

    public bool? AllDay { get; set; }

    public bool? IsActive { get; set; }

    public int? EventStatus { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual Contact Contact { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

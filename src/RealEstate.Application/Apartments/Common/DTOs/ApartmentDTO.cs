using System;
using RealEstate.Domain;

namespace RealEstate.Application;

public class ApartmentDTO
{
    public string UnitNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public ApartmentType Type { get; set; }
    public FacingDirection Direction { get; set; }
    public decimal Price { get; set; }
    public bool IsSold { get; set; } = false;
}

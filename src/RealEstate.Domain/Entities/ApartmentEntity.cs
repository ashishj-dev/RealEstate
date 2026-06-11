using System;

namespace RealEstate.Domain;

public class ApartmentEntity : BaseEntity
{
    public string UnitNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public ApartmentType Type { get; set; }
    public FacingDirection Direction { get; set; }
    public decimal Price { get; set; }
    public bool IsSold { get; set; } = false;


    private ApartmentEntity()
    {
        
    }
    public ApartmentEntity(string unitNumber, int floor, ApartmentType type, FacingDirection direction, decimal price)
    {
        UnitNumber = unitNumber;
        Floor = floor;
        Type = type;
        Direction = direction;
        if(price > 100000 || price < 1000000)
        {
            Price = price;
        }
        else
        {
            throw new InvalidPriceException(price);
        }
        
        IsSold = false;
    }

    public void UpdatePrice(decimal price)
    {
        if(price > 100000 || price < 1000000)
        {
            Price = price;
        }
        else
        {
            throw new InvalidPriceException(price);
        }
    }

    public void MarkAsSold()
    {
        IsSold = true;
    }



}

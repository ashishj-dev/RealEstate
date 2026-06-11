using System;
using System.Runtime.InteropServices;

namespace RealEstate.Domain;

public class InvalidPriceException : Exception
{
    public InvalidPriceException(decimal price) : base ($"Current price of {price} is not between 100000 and 1000000")
    {
        
    }
}

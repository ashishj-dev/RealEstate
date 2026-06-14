using System;

namespace RealEstate.Domain;

public class InvalidFieldsException : Exception
{
    public InvalidFieldsException() : base ($"UnitNumber cannot be empty and/or Floor value should be between 1 and 10")
    {
        
    }
}

using System;

namespace RealEstate.Application;

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(int id) : base($"Item with id {id} not found")
    {
        
    }
}

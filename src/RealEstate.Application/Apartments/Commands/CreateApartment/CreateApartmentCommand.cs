using System;
using RealEstate.Domain;
using MediatR;
using System.Windows.Input;

namespace RealEstate.Application;

public record CreateApartmentCommand(
    string unitNumber, 
    int floor, 
    ApartmentType type, 
    FacingDirection direction, 
    decimal price) : IRequest<int>;


using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public record UpdateApartmentPriceCommand(int id, decimal price) : IRequest<ApartmentEntity?>;



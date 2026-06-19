using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public record UpdateApartmentSaleStatusCommand(int id, bool status) : IRequest<int?>;


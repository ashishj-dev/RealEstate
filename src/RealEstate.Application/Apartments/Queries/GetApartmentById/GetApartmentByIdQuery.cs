using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public record GetApartmentByIdQuery(int id) : IRequest<ApartmentDTO?>;


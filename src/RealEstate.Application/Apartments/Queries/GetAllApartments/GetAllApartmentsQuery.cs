using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public record GetAllApartmentsQuery : IRequest<IReadOnlyList<ApartmentDTO>>;


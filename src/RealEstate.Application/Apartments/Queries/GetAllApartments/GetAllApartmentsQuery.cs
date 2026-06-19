using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public record GetAllApartmentsQuery(int pageNumber = 1, int pageSize = 20) : IRequest<IReadOnlyList<ApartmentDTO>>;


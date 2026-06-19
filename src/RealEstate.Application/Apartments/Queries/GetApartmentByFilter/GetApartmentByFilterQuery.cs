using System;
using MediatR;
using RealEstate.Domain;

namespace RealEstate.Application;

public record GetApartmentByFilterQuery(int? floor, ApartmentType? Type, FacingDirection? Direction) : IRequest<List<ApartmentDTO>>;


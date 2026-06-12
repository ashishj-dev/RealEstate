using System;
using System.Windows.Input;
using MediatR;

namespace RealEstate.Application;

public record DeleteApartmentByIdCommand(int id) : IRequest<int>;


using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Identity.Application.Contracts;

namespace GymStore.Modules.Identity.Application.Features.Register;

public sealed record RegisterCommand(RegisterRequest Request) : ICommand<AuthResult>;

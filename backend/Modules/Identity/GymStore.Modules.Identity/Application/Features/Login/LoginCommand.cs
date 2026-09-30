using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Identity.Application.Contracts;

namespace GymStore.Modules.Identity.Application.Features.Login;

public sealed record LoginCommand(LoginRequest Request) : ICommand<AuthResult>;

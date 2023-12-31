﻿using MediatR;

namespace OpenCommerce.Application.Features.UserFeatures.CreateUser;

public sealed record CreateUserRequest(string Email, string Name) : IRequest<CreateUserResponse>;
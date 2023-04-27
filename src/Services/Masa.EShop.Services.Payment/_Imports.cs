﻿global using Dapr;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using Masa.BuildingBlocks.Data.UoW;
global using Masa.BuildingBlocks.Ddd.Domain.Entities.Full;
global using Masa.BuildingBlocks.Ddd.Domain.Events;
global using Masa.BuildingBlocks.Ddd.Domain.Repositories;
global using Masa.Contrib.Ddd.Domain.Repository.EFCore;
global using Masa.BuildingBlocks.Ddd.Domain.Services;
global using Masa.BuildingBlocks.Dispatcher.Events;
global using Masa.Contrib.Data.EFCore;
global using Masa.Contrib.Data.UoW.EFCore;
global using Masa.Contrib.Ddd.Domain;
global using Masa.Contrib.Ddd.Domain.Options;
global using Masa.Contrib.Dispatcher.Events;
global using Masa.Contrib.Dispatcher.IntegrationEvents;
global using Masa.Contrib.Dispatcher.IntegrationEvents.Dapr;
global using Masa.Contrib.Dispatcher.IntegrationEvents.EventLogs.EFCore;
global using Masa.Contrib.Service.MinimalAPIs;
global using Masa.EShop.Contracts.Ordering.Event;
global using Masa.EShop.Contracts.Payment;
global using Masa.EShop.Services.Payment.Application.Middleware;
global using Masa.EShop.Services.Payment.Application.Payments.Commands;
global using Masa.EShop.Services.Payment.Domain.Repositories;
global using Masa.EShop.Services.Payment.Domain.Services;
global using Masa.EShop.Services.Payment.Infrastructure;
global using Masa.EShop.Services.Payment.Infrastructure.Extensions;
global using Masa.EShop.Services.Payment.Service;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Models;
global using Masa.BuildingBlocks.Dispatcher.IntegrationEvents;

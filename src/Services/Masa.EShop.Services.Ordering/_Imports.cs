﻿global using Dapr;
global using Dapr.Actors;
global using Dapr.Actors.Client;
global using Dapr.Actors.Runtime;
global using Masa.BuildingBlocks.Dispatcher.Events;
global using Masa.BuildingBlocks.Dispatcher.IntegrationEvents;
global using Masa.BuildingBlocks.ReadWriteSpliting.Cqrs.Commands;
global using Masa.BuildingBlocks.ReadWriteSpliting.Cqrs.Queries;
global using Masa.Contrib.Data.EntityFrameworkCore;
global using Masa.Contrib.Dispatcher.Events;
global using Masa.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF;
global using Masa.Contrib.Service.MinimalAPIs;
global using Masa.EShop.Contracts.Basket;
global using Masa.EShop.Contracts.Basket.Model.BFF;
global using Masa.EShop.Contracts.Catalog.Event;
global using Masa.EShop.Contracts.Ordering.Event;
global using Masa.EShop.Contracts.Payment;
global using Masa.EShop.Services.Ordering;
global using Masa.EShop.Services.Ordering.Actors;
global using Masa.EShop.Services.Ordering.Application.CardTypes.Queries;
global using Masa.EShop.Services.Ordering.Application.Orders.Commands;
global using Masa.EShop.Services.Ordering.Application.Orders.Queries;
global using Masa.EShop.Services.Ordering.Domain.Events;
global using Masa.EShop.Services.Ordering.Domain.Repositories;
global using Masa.EShop.Services.Ordering.Dto;
global using Masa.EShop.Services.Ordering.Entities;
global using Masa.EShop.Services.Ordering.Infrastructure;
global using Masa.EShop.Services.Ordering.Infrastructure.EntityConfigurations;
global using Masa.EShop.Services.Ordering.Infrastructure.Extensions;
global using Masa.EShop.Services.Ordering.Infrastructure.Repositories;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http.Connections;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.ResponseCompression;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.Data.SqlClient;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.OpenApi.Models;
global using System;
global using System.Text;
global using System.Text.Json;

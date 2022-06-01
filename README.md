[中](README.zh-CN.md) | EN

# <center>Masa.EShop</center>

# Introduction

A sample `.NET Core` distributed application based on eShopOnDapr, powered by [Masa.BuildingBlocks](https://github.com/masastack/Masa.BuildingBlocks), [Masa.Contrib](https://github.com/masastack/Masa.Contrib), [Masa.Utils](https://github.com/masastack/Masa.Utils),[Dapr](https://github.com/dapr/dapr).

## Directory Structure

```
Masa.EShop
├── dapr
│   ├── components                           dapr local components directory
│   │   ├── pubsub.yaml                      pub/sub config file
│   │   └── statestore.yaml                  state management config file
├── src
│   ├── Api
│   │   ├── Masa.EShop.Api.Caller            Caller package
│   │   └── Masa.EShop.Api.Open              BFF Layer, provide API to Web.Client
│   ├── Contracts                            Common contracts，like Event Class
│   │   ├── Masa.EShop.Contracts.Basket
│   │   ├── Masa.EShop.Contracts.Catalog
│   │   ├── Masa.EShop.Contracts.Ordering
│   │   └── Masa.EShop.Contracts.Payment
│   ├── Services
│   │   ├── Masa.EShop.Services.Basket
│   │   ├── Masa.EShop.Services.Catalog
│   │   ├── Masa.EShop.Services.Ordering
│   │   └── Masa.EShop.Services.Payment
│   ├── Web
│   │   ├── Masa.EShop.Web.Admin
│   │   └── Masa.EShop.Web.Client
├── test
|   └── Masa.EShop.Services.Catalog.Tests
├── docker-compose
│   ├── Masa.EShop.Web.Admin
│   └── Masa.EShop.Web.Client
├── .gitignore
├── LICENSE
├── .dockerignore
└── README.md
```

## Project Structure

![Project Structure](img/eshop.png)

## Project Architecture

![架构图](img/eshop-architectureks.png)

## Getting started

- Preparation

  - Docker
  - VS 2022
  - .Net 6.0
  - Dapr

- Startup

  - VS 2022(Recommended)

    Set docker-compose as start project, press `Ctrl + F5` to start.

    ![vs-run](img/vs_run.png)

    After startup, you can see the container view.

    ![vs-result](img/vs_result.png)

  - CLI

    Run the command in the project root directory.

    ```
    docker-compose build
    docker-compose up
    ```

    After startup, the output is as follows.

    ![cli-result](img/cli_result.png)

  - VS Code (Todo)

- Display after startup(Update later)

  Baseket Service: http://localhost:8081/swagger/index.html  
  Catalog Service: http://localhost:8082/swagger/index.html  
  Ordering Service: http://localhost:8083/swagger/index.html  
  Payment Service: http://localhost:8084/swagger/index.html  
  Admin Web: empty  
  Client Web: http://localhost:8090/catalog

## Features

#### MinimalAPI

The service in the project uses the `Minimal API` added in .NET 6 instead of the Web API.

> For more Minimal API content reference [mvc-to-minimal-apis-aspnet-6](https://benfoster.io/blog/mvc-to-minimal-apis-aspnet-6/)

```C#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/api/v1/helloworld", ()=>"Hello World");
app.Run();
```

`Masa.Contrib.Service.MinimalAPIs` based on `Masa.BuildingBlocks`:

Program.cs

```C#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Services.AddServices(builder);
app.Run();
```

HelloService.cs

```C#
public class HelloService : ServiceBase
{
    public HelloService(IServiceCollection services): base(services) =>
        App.MapGet("/api/v1/helloworld", ()=>"Hello World"));
}
```

> The `ServiceBase` class (like ControllerBase) provided by `Masa.BuildingBlocks` is used to define Service class (like Controller), maintains the route registry in the constructor. The `AddServices(builder)` method will auto register all the service classes to DI. Service inherited from ServiceBase is `similar to singleton pattern`. Such as `Repostory`, should be injected with the `FromService`.

#### Dapr

The official Dapr implementation, Masa.Contrib references the Event section.

More Dapr content reference: https://docs.microsoft.com/zh-cn/dotnet/architecture/dapr-for-net-developers/

1. Add Dapr

```C#
builder.Services.AddDaprClient();
...
app.UseRouting();
app.UseCloudEvents();
app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();
});
```

2. Publish event

```C#
var @event = new OrderStatusChangedToValidatedIntegrationEvent();
await _daprClient.PublishEventAsync
(
    "pubsub",
    nameof(OrderStatusChangedToValidatedIntegrationEvent),
    @event
);
```

3. Sub event

```C#
 [Topic("pubsub", nameof(OrderStatusChangedToValidatedIntegrationEvent)]
 public async Task OrderStatusChangedToValidatedAsync(
     OrderStatusChangedToValidatedIntegrationEvent integrationEvent,
     [FromServices] ILogger<IntegrationEventService> logger)
 {
     logger.LogInformation("----- integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", integrationEvent.Id, Program.AppName, integrationEvent);
 }
```

> `Topic` first parameter `pubsub` is the `name` field in the `pubsub.yaml` file.

#### Actor

1. Add Actor

```C#
app.UseEndpoints(endpoint =>
{
    ...
    endpoint.MapActorsHandlers();
});
```

2. Define actor interface and inherit IActor.

```C#
public interface IOrderingProcessActor : IActor
{
```

3. Implement `IOrderingProcessActor` and inherit the `Actor` class. The sample project also implements the `IRemindable` interface, and 'RegisterReminderAsync' method.

```C#
public class OrderingProcessActor : Actor, IOrderingProcessActor, IRemindable
{
    //todo
}
```

4. Register Actor

```C#
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<OrderingProcessActor>();
});
```

5. Invoke actor

```C#
var actorId = new ActorId(order.Id.ToString());
var actor = ActorProxy.Create<IOrderingProcessActor>(actorId, nameof(OrderingProcessActor));
```

#### EventBus

Only In-Process events.

1. Add EventBus

```C#
builder.Services.AddEventBus();
```

2. Define Event

```C#
public class DemoEvent : Event
{
    //todo 自定义属性事件参数
}
```

3. Send Event

```C#
IEventBus eventBus;
await eventBus.PublishAsync(new DemoEvent());
```

4. Hanle Event

```C#
[EventHandler]
public async Task DemoHandleAsync(DemoEvent @event)
{
    //todo
}
```

#### IntegrationEventBus

Cross-Process event, In-Process event also supported when `EventBus` is added.

1. Add IntegrationEventBus

```C#
builder.Services
    .AddDaprEventBus<IntegrationEventLogService>();
//   .AddDaprEventBus<IntegrationEventLogService>(options=>{
//        //todo
//       options.UseEventBus();//Add EventBus
//    });
```

2. Define Event

```C#
public class DemoIntegrationEvent : IntegrationEvent
{
    public override string Topic { get; set; } = nameof(DemoIntegrationEvent);
    //todo
}
```

> `Topic` property is the value of the dapr `TopicAttribute` second parameter.

3. Send Event

```C#
public class DemoService
{
    private readonly IIntegrationEventBus _eventBus;

    public DemoService(IIntegrationEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    //todo

    public async Task DemoPublish()
    {
        //todo
        await _eventBus.PublishAsync(new DemoIntegrationEvent());
    }
}
```

4. Handle Event

```C#
[Topic("pubsub", nameof(DemoIntegrationEvent))]
public async Task DemoIntegrationEventHandleAsync(DemoIntegrationEvent @event)
{
    //todo
}
```

#### CQRS

More CQRS content reference：https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs

##### Query

1. Define Query

```c#
public class CatalogItemQuery : Query<List<CatalogItem>>
{
    public string Name { get; set; } = default!;

    public override List<CatalogItem> Result { get; set; } = default!;
}
```

2. Add QueryHandler:

```c#
public class CatalogQueryHandler
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogQueryHandler(ICatalogItemRepository catalogItemRepository) => _catalogItemRepository = catalogItemRepository;

    [EventHandler]
    public async Task ItemsWithNameAsync(CatalogItemQuery query)
    {
        query.Result = await _catalogItemRepository.GetListAsync(query.Name);
    }
}
```

3. Send Query

```C#
IEventBus eventBus;// DI is recommended
await eventBus.PublishAsync(new CatalogItemQuery(){
    Name = "Rolex"
});
```

##### Command

1. Define Command

```c#
public class CreateCatalogItemCommand : Command
{
    public string Name { get; set; } = default!;

    //todo
}
```

2. Add CommandHandler：

```c#
public class CatalogCommandHandler
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogCommandHandler(ICatalogItemRepository catalogItemRepository) => _catalogItemRepository = catalogItemRepository;

    [EventHandler]
    public async Task CreateCatalogItemAsync(CreateCatalogItemCommand command)
    {
        //todo
    }
}
```

3. 发送 Command

```C#
IEventBus eventBus;
await eventBus.PublishAsync(new CreateCatalogItemCommand());
```

#### DDD

More DDD content reference:https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice

Both In-Process and Cross-Process events are supported.

1. Add DomainEventBus

```c#
.AddDomainEventBus(options =>
{
    options.UseEventBus()
        .UseUow<PaymentDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=payment"))
        .UseDaprEventBus<IntegrationEventLogService>()
        .UseEventLog<PaymentDbContext>()
        .UseRepository<PaymentDbContext>();//使用Repository的EF版实现
})
```

2. Define DomainCommand(In-Process)

To verify payment command, you need to inherit DomainCommand or DomainQuery<>

```C#
public class OrderStatusChangedToValidatedCommand : DomainCommand
{
    public Guid OrderId { get; set; }
}
```

3. Send DomainCommand

```C#
IDomainEventBus domainEventBus;
await domainEventBus.PublishAsync(new OrderStatusChangedToValidatedCommand()
{
    OrderId = "OrderId"
});
```

4. Add Handler

```C#
[EventHandler]
public async Task ValidatedHandleAsync(OrderStatusChangedToValidatedCommand command)
{
    //todo
}
```

5. Define DomainEvent(Cross-Process))

```c#
public class OrderPaymentSucceededDomainEvent : IntegrationDomainEvent
{
     public Guid OrderId { get; init; }

    public override string Topic { get; set; } = nameof(OrderPaymentSucceededIntegrationEvent);

    private OrderPaymentSucceededDomainEvent()
    {
    }

    public OrderPaymentSucceededDomainEvent(Guid orderId) => OrderId = orderId;
}

public class OrderPaymentFailedDomainEvent : IntegrationDomainEvent
{
    public Guid OrderId { get; init; }

    public override string Topic { get; set; } = nameof(OrderPaymentFailedIntegrationEvent);

    private OrderPaymentFailedDomainEvent()
    {
    }

    public OrderPaymentFailedDomainEvent(Guid orderId) => OrderId = orderId;
}
```

6. Define domain service and send IntegrationDomainEvent(Cross-Process)

```c#
public class PaymentDomainService : DomainService
{
    private readonly ILogger<PaymentDomainService> _logger;

    public PaymentDomainService(IDomainEventBus eventBus, ILogger<PaymentDomainService> logger) : base(eventBus)
        => _logger = logger;

    public async Task StatusChangedAsync(Aggregate.Payment payment)
    {
        IIntegrationDomainEvent orderPaymentDomainEvent;
        if (payment.Succeeded)
        {
            orderPaymentDomainEvent = new OrderPaymentSucceededDomainEvent(payment.OrderId);
        }
        else
        {
            orderPaymentDomainEvent = new OrderPaymentFailedDomainEvent(payment.OrderId);
        }
        _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", orderPaymentDomainEvent.Id, Program.AppName, orderPaymentDomainEvent);
        await EventBus.PublishAsync(orderPaymentDomainEvent);
    }
}
```

## Service Description

#### Masa.EShop.Services.Basket

1. Add [MinimalAPI](####MinimalAPI)
2. Add and use [Dapr](####Dapr)

#### Masa.EShop.Services.Catalog

1. Add [MinimalAPI](####MinimalAPI)
2. Add [DaprEventBus](####IntegrationEventBus)

```c#
builder.Services
.AddDaprEventBus<IntegrationEventLogService>(options =>
{
    options.UseEventBus()
           .UseUow<CatalogDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=catalog"))
           .UseEventLog<CatalogDbContext>();
})
```

3. Use [CQRS](####CQRS)

#### Masa.EShop.Services.Ordering

1. Add [MinimalAPI](####MinimalAPI)
2. Add [DaprEventBus](####IntegrationEventBus)

```C#
builder.Services
    .AddMasaDbContext<OrderingContext>(dbOptions => dbOptions.UseSqlServer("Data Source=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=order"))
    .AddDaprEventBus<IntegrationEventLogService>(options =>
    {
        options.UseEventBus().UseEventLog<OrderingContext>();
    })
```

3. Use [CQRS](####CQRS)
4. Add [Actor](####Actor)
5. Modify docker-compse file

`docker-compose.yml` add `dapr` service;

```yaml
dapr-placement:
  image: "daprio/dapr:1.4.0"
```

`docker-compose.override.yml` add command and port mapping.

```yaml
dapr-placement:
  command: ["./placement", "-port", "50000", "-log-level", "debug"]
  ports:
    - "50000:50000"
```

`ordering.dapr` service add command

```yaml
"-placement-host-address", "dapr-placement:50000"
```

#### Masa.EShop.Services.Payment

1. Add [MinimalAPI](####MinimalAPI)
2. Add [DomainEventBus](####DDD)

```C#
builder.Services
.AddDomainEventBus(options =>
{
    options.UseEventBus()
        .UseUow<PaymentDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=payment"))
        .UseDaprEventBus<IntegrationEventLogService>()
        .UseEventLog<PaymentDbContext>()
        .UseRepository<PaymentDbContext>();
})
```

3. Use [CQRS](####CQRS)

4. Use [DDD](####DDD)

# Function Introduction

Update later

# Nuget Package Introduction

```c#
Install-Package Masa.Contrib.Service.MinimalAPIs //MinimalAPI
```

```c#
Install-Package Masa.Contrib.Dispatcher.Events //In-Process event
```

```c#
Install-Package Masa.Contrib.Dispatcher.IntegrationEvents.Dapr //Cross-Process event
Install-Package Masa.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF //Local message table
```

```c#
Install-Package Masa.Contrib.Data.UoW.EF //EF UoW
```

```c#
Install-Package Masa.Contrib.ReadWriteSpliting.Cqrs //CQRS
```

```c#
Install-Package Masa.BuildingBlocks.Ddd.Domain //DDD相关实现
Install-Package Masa.Contrib.Ddd.Domain.Repository.EF //Repository实现
```



## Interactive

| QQ group                                        | WX public account                                            | WX Customer Service                                          |
| ----------------------------------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| ![masa.blazor-qq](img/masa.blazor-qq-group.png) | ![masa.blazor-weixin](img/masa.blazor-wechat-public-account.png) | ![masa.blazor-weixin](img/masa.blazor-wechat-customer-service.png) |

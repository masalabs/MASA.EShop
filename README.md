# MASA.EShop

## 简介

### 项目目录

```
- dapr
    - components  dapr配置文件目录
        - pubsub.yaml  发布订阅配置文件
        - statestore.yaml   状态管理配置文件
- src
    - Api
        - MASA.EShop.Api.Open  BFF层，提供接口给Web.Client
    - Contracts  公用元素提取，如服务间通信的Event Class。
        - MASA.EShop.Contracts.Basket
        - MASA.EShop.Contracts.Catalog
        - MASA.EShop.Contracts.Ordering
        - MASA.EShop.Services.Payment
    - Services  服务拆分
        - MASA.EShop.Services.Basket  购物车模块服务
        - MASA.EShop.Services.Catalog  商品模块服务
        - MASA.EShop.Services.Ordering  订单模块服务
        - MASA.EShop.Services.Payment  支付模块服务
    - Web
        - MASA.EShop.Web.Admin
        - MASA.EShop.Web.Client
- test
    - MASA.EShop.Services.Catalog.Tests  单元测试
- docker-compose    docker-compose 服务配置
    - docker-compose.yml
      docker-compose.override.yml
```

### 依赖框架

.Net Core 6.0  
Dapr.Actors.AspNetCore 1.4.0  
EntityFrameworkCore 6.0  
MASA.Contrib.XXXX 技术栈

### 服务介绍

#### MASA.EShop.Services.Basket

项目结构为最简单的 Minimal API + Dapr(StateManage + Pub/Sub) 模式,其中 pub/sub 使用的是封装好的`MASA.Contrib.Dispatcher.IntegrationEvents.Dapr`,Minimal API 由库`MASA.Contrib.Service.MinimalAPIs`提供支持。

Minimal API 相关代码

```
var builder = WebApplication.CreateBuilder(args);

var app = builder.Services
    .AddServices(builder);

app.Run();
```

增加 BasketService 类（相当于 Controller）继承 ServiceBase（相当于 ControllerBase）。构造函数中添加路由注册。

```
App.MapGet("/api/v1/payment/HelloWorld", ()=> "Hello World");
```

> 继承 ServiceBase 类为单例模式,构造函数注入只可以注入单例，如 ILogger,仓储类 Repostory 等应该借助 FromService 实现方法注入。

添加 Dapr 相关支持，`app.Run()`前增加如下代码,官方文档提到的`services.AddControllers().AddDapr();`无需再添加，`MASA.Contrib.Dispatcher.IntegrationEvents.Dapr` 提供的`AddDaprEventBus`方法内会自动添加 Dapr 注册必要的服务。

```
app.UseRouting();
app.UseCloudEvents();
app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();
});
```

增加 MASA.Contrib.Dispatcher.IntegrationEvents.Dapr 相关代码

```
builder.Services //todo
.AddMasaDbContext<IntegrationEventLogContext>(options => options.UseSqlServer("Data Source=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=order"))
//增加EF上下文，IntegrationEventLogContext需要添加包
//MASA.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF
    .AddDaprEventBus<IntegrationEventLogService>(options =>
    {
        options.Assemblies = new Assembly[] { typeof(UserCheckoutAcceptedIntegrationEvent).Assembly, Assembly.GetExecutingAssembly() };  //指定IntegrationEvent所在的程序集
    })
```

增加跨进程事件类`UserCheckoutAcceptedIntegrationEvent`，继承`IntegrationEvent`。

```
public class UserCheckoutAcceptedIntegrationEvent:IntegrationEvent
{
    public override string Topic { get; set; } = nameof(UserCheckoutAcceptedIntegrationEvent);
    //todo
}
```

> Topic 属性值为 Dapr pub/sub 相关特性 TopicAttribute 第二个参数的值,第一个参数为配置文件 pubsub.yaml 中指定的 name

在事件对应的处理方法加上特性

```
[Topic("pubsub", nameof(UserCheckoutAcceptedIntegrationEvent))]
```

至此 Basket 服务相关的的 Minimal API + Dapr(StateManage) + MASA.Contrib.Dispatcher.IntegrationEvents.Dapr 全部编码已经完成。

更多 Minimal API 内容参考[mvc-to-minimal-apis-aspnet-6](https://benfoster.io/blog/mvc-to-minimal-apis-aspnet-6/)



#### MASA.EShop.Services.Catalog

项目结构：MinimalAPI+CQRS

MinimalAPI参考[Basket](####MASA.EShop.Services.Basket). 

引用包情况: 

```c#
Install-Package MASA.Contrib.Service.MinimalAPIs //MinimalAPI使用

Install-Package MASA.Contrib.Dispatcher.Events //发送进程内消息
Install-Package MASA.Contrib.Dispatcher.IntegrationEvents.Dapr //发送进程外消息使用
Install-Package MASA.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF //记录进程外消息日志
Install-Package MASA.Contrib.Data.Uow.EF //工作单元，确保事务的一致性
Install-Package MASA.Contrib.ReadWriteSpliting.CQRS //CQRS实现
```

在Program.cs

```c#
.AddDaprEventBus<IntegrationEventLogService>(options =>
{
    options.UseEventBus()//使用进程内事件
           .UseUow<CatalogDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=catalog"))//使用工作单元
           .UseEventLog<CatalogDbContext>();//将CatalogDbContext上下文交于事件日志使用, CatalogDbContext需要继承IntegrationEventLogContext
})   
```

CQRS写法：

更多关于CQRS文档请参考：https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs

根据操作定义对应的Command或Query, 例如：

	//校验库存的Command, 需要继承Command, 如果是查询, 则需要继承Query<>
	public class StockValidationCommand : Command
	{
	   	public Guid OrderId { get; set; }
	   	
	   	public IEnumerable<OrderStockItem> OrderStockItems { get; set; }
	   	
	   	public StockValidationCommand(Guid orderId, IEnumerable<OrderStockItem> orderStockItems)
		{
	    	OrderId = orderId;
	    	OrderStockItems = orderStockItems;
		}
	}
然后添加Handler, 例如：

```c#
public class ProductCommandHandler
{
    private readonly ICatalogItemRepository _repository;
    private readonly IIntegrationEventBus _integrationEventBus;
	
	public ProductCommandHandler(ICatalogItemRepository repository, IIntegrationEventBus integrationEventBus)
	{
    	_repository = repository;
	    _integrationEventBus = integrationEventBus;
	}
	
	// 在对应的方法上添加EventHandler, 此处为库存校验的Handler
	[EventHandler]
    public async Task StockValidationHandlerAsync(StockValidationCommand command)
    {
		------此处写库存校验逻辑------
        IntegrationEvent integrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
            ? new OrderStockRejectedIntegrationEvent(command.OrderId, confirmedOrderStockItems)
            : new OrderStockConfirmedIntegrationEvent(command.OrderId);

        await _integrationEventBus.PublishAsync(integrationEvent);//此处需要发布进程外的事件, 因此使用IIntegrationEventBus
    }
}
```



#### MASA.EShop.Services.Ordering

项目结构为 Minimal API + CQRS + Dapr(Actor + Pub/Sub),CQRS 相关介绍参考[Catalog](####MASA.EShop.Services.Catalog)。
项目中增加 Actor 支持

```
app.UseRouting();
app.UseCloudEvents();
app.UseEndpoints(endpoint =>
{
    endpoint.MapSubscribeHandler();
    endpoint.MapActorsHandlers(); //Actor 支持
});
```

docker-compose.yml 中增加 dapr 服务;

```
dapr-placement:
    image: "daprio/dapr:1.4.0"
```

docker-compose.override.yml 中增加具体命令和端口映射

```
  dapr-placement:
    command: ["./placement", "-port", "50000", "-log-level", "debug"]
    ports:
      - "50000:50000"
```

对应的 ordering.dapr 服务上增加命令

```
"-placement-host-address", "dapr-placement:50000"
```

定义自己的 Actor 接口，继承 IActor。

```
public interface IOrderingProcessActor : IActor
{
```

实现`IOrderingProcessActor`，并继承`Actor`类。示例项目还实现了`IRemindable`接口，实现该接口后通过方法`RegisterReminderAsync`完成注册提醒。

增加 Actor 注册相关代码,一定要在`AddServices`方法前

```
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<OrderingProcessActor>();
});
```

编写 Actor 调用代码

```
var actorId = new ActorId(order.Id.ToString());
var actor = ActorProxy.Create<IOrderingProcessActor>(actorId, nameof(OrderingProcessActor));
```

#### MASA.EShop.Services.Payment

项目结构：MinimalAPI+CQRS+DDD

MinimalAPI参考[Basket](####MASA.EShop.Services.Basket). 

CQRS参考[Catalog](####MASA.EShop.Services.Catalog). 

引用包情况：

```c#
Install-Package MASA.Contrib.Service.MinimalAPIs //MinimalAPI使用
Install-Package MASA.Contrib.Dispatcher.Events //发送进程内消息
Install-Package MASA.Contrib.Dispatcher.IntegrationEvents.Dapr //发送进程外消息使用
Install-Package MASA.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF //记录进程外消息日志
Install-Package MASA.Contrib.Data.Uow.EF //工作单元，确保事务的一致性
Install-Package MASA.Contrib.ReadWriteSpliting.CQRS //CQRS实现

Install-Package MASA.BuildingBlocks.DDD.Domain //DDD相关实现
Install-Package MASA.Contribs.DDD.Domain.Repository.EF //Repository实现
```

在Program.cs

```c#
.AddDomainEventBus(options =>
{
    options.UseEventBus()//使用进程内事件
        .UseUow<PaymentDbContext>(dbOptions => dbOptions.UseSqlServer("server=masa.eshop.services.eshop.database;uid=sa;pwd=P@ssw0rd;database=payment"))
        .UseDaprEventBus<IntegrationEventLogService>()///使用进程外事件
        .UseEventLog<PaymentDbContext>()
        .UseRepository<PaymentDbContext>();//使用Repository的EF版实现
})
```

根据操作定义对应的DomainCommand或DomainQuery, 例如：

	//校验支付的Command, 需要继承DomainCommand, 如果是查询, 则需要继承DomainQuery<>
	public class OrderStatusChangedToValidatedCommand : DomainCommand
	{
	    public Guid OrderId { get; set; }
	}

然后添加对应的DomainService, 使其继承DomainService，在其中添加对应的方法发送DomainEventBus，并使用EventBus发送，如：

    public async Task StatusChangedAsync(Aggregate.Payment payment)
    {
    	IIntegrationDomainEvent orderPaymentDomainEvent;
        if (payment.Succeeded)
        {
            orderPaymentDomainEvent = new OrderPaymentSucceededDomainEvent(payment.OrderId);
        }
        else
        {
            //TscLogger.Warning(nameof(PaymentDomainService), "----- Payment rejected for order {OrderId}", command.OrderId);
            orderPaymentDomainEvent = new OrderPaymentFailedDomainEvent(payment.OrderId);
        }
    
        _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", orderPaymentDomainEvent.Id, Program.AppName, orderPaymentDomainEvent);
    
        await EventBus.PublishAsync(orderPaymentDomainEvent);//EventBus类型是IDomainEventBus，用于发送领域事件
    }



## 功能介绍

using FluentValidation;
using Masa.BuildingBlocks.Dispatcher.Events;
using Masa.EShop.Services.Catalog.Application.CatalogTypes.Commands.CreateCatalogType;
using Masa.EShop.Services.Catalog.Domain.Repositories;

namespace Masa.EShop.Services.Catalog.Tests;
[TestClass]
public class CatalogTypeUnitTest
{
    [TestMethod]
    public void TestCreate()
    {
        //Arrange
        Mock<IUnitOfWork> uow = new();
        uow.Setup(u => u.CommitAsync(default)).Verifiable();

        Mock<ICatalogTypeRepository> catalogTypeRepository = new();
        catalogTypeRepository.Setup(r => r.AddAsync(It.IsAny<CatalogType>())).Verifiable();

        CreateCatalogTypeCommand createCatalogTypeCommand = new() { Type = DateTime.Now.ToString("yyyyMMddHHmmss") };

        Mock<IEventBus> eventBus = new();
        eventBus
            .Setup(e => e.PublishAsync(It.IsAny<CreateCatalogTypeCommand>()))
            .Callback<CreateCatalogTypeCommand>(async cmd =>
            {
                var result = new CreateCatalogTypeCommandValidator().Validate(createCatalogTypeCommand);

                await new CatalogTypeCommandHandler(catalogTypeRepository.Object).CreateHandleAsync(createCatalogTypeCommand);
                await uow.Object.CommitAsync();
            });

        //Act
        eventBus.Object.PublishAsync(createCatalogTypeCommand);

        //Assert
        Assert.ThrowsException<ValidationException>(() => new CreateCatalogTypeCommandValidator().ValidateAndThrow(new CreateCatalogTypeCommand() { Type = "1" }));

        catalogTypeRepository.Verify(
            repo => repo.AddAsync(It.Is<CatalogType>(catalogType => catalogType.Type == createCatalogTypeCommand.Type)),
            Times.Once,
            "CreateAsync must be called only once");
        uow.Verify(u => u.CommitAsync(default), Times.Once, "CommitAsync must be called only once");
    }
}

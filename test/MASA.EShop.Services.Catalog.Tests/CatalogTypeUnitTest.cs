using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MASA.EShop.Services.Catalog.Tests;
[TestClass]
public class CatalogTypeUnitTest
{
    [TestMethod]
    public void TestCreate()
    {
        //Arrange
        Mock<IEventBus> eventBus = new();
        Mock<IUnitOfWork> uow = new();

        Mock<ICatalogTypeRepository> catalogTypeRepository = new();

        CreateCatalogTypeCommand createCatalogTypeCommand = new() { Type = DateTime.Now.ToString("yyyyMMddHHmmss") };

        //Act
        eventBus.Object.PublishAsync(createCatalogTypeCommand);

        //Assert
        Mock<CatalogType> data = new();
        catalogTypeRepository.Verify(repo => repo.CreateAsync(data.Object), Times.Once, "CreateAsync must be called only once");
        uow.Verify(u => u.CommitAsync(default), Times.Once, "CommitAsync must be called only once");
    }
}
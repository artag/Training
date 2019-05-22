using System.Collections.Generic;
using System.Linq;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        private Mock<IProductRepository> _mock;

        [Fact]
        public void Can_Paginate()
        {
            // Arrange
            var controller = new ProductController(Mock.Object);
            controller.PageSize = 3;

            // Act
            var result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            // Assert
            var prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            var controller = new ProductController(Mock.Object);
            controller.PageSize = 3;

            // Act
            var result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            // Assert
            var pagingInfo = result.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            // Arrange
            var controller = new ProductController(Mock.Object);
            controller.PageSize = 3;

            // Act
            var result = 

            // Assert
        }

        private Product[] Products { get; } = new Product[]
        {
            new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
            new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
            new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
            new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
            new Product { ProductID = 5, Name = "P5", Category = "Cat3" },
        };

        private Mock<IProductRepository> Mock
        {
            get
            {
                if (_mock == null)
                {
                    _mock = new Mock<IProductRepository>();
                    _mock.Setup(m => m.Products).Returns(Products.AsQueryable<Product>());
                }

                return _mock;
            }
        }
    }
}

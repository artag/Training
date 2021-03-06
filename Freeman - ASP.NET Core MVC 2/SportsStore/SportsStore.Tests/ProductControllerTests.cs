﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
            var result = controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel;
            var products = result.Products.ToArray();

            // Assert
            Assert.Equal(2, products.Length);
            Assert.True(products[0].Name == "P2" && products[0].Category == "Cat2");
            Assert.True(products[1].Name == "P4" && products[1].Category == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            // Arrange
            var controller = new ProductController(Mock.Object);
            controller.PageSize = 3;

            Func<ViewResult, ProductsListViewModel> getModel =
                result => result?.ViewData?.Model as ProductsListViewModel;

            // Act
            int? res1 = getModel(controller.List("Cat1"))?.PagingInfo.TotalItems;
            int? res2 = getModel(controller.List("Cat2"))?.PagingInfo.TotalItems;
            int? res3 = getModel(controller.List("Cat3"))?.PagingInfo.TotalItems;
            int? resAll = getModel(controller.List(null))?.PagingInfo.TotalItems;

            // Assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
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

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GestionProduits.Controllers;
using GestionProduits.Models;
using GestionProduits.Application.Products.Queries;
using GestionProduits.Application.Products.Commands.CreateProduct;
using GestionProduits.Application.Products.Commands.DeleteProduct;
using GestionProduits.Application.Products.Commands;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using GestionProduits.Data;

namespace GestionProduits.Tests
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ApplicationDbContext _context;

        public ProductsControllerTests()
        {
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);

            
            _mediatorMock = new Mock<IMediator>();

            
            _controller = new ProductsController(_context, _mediatorMock.Object);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOkResult_WithListOfProducts()
        {
            
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Description = "Description1", Price = 10.99m, Stock = 100 },
                new Product { Id = 2, Name = "Product2", Description = "Description2", Price = 20.99m, Stock = 200 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(products);

            
            var result = await _controller.GetProducts();

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count);
        }

       [Fact]
        public async Task GetProduct_ShouldReturnOkResult_WithProduct()
        {
            
            var mockMediator = new Mock<IMediator>();
            var testProduct = new Product { Id = 1, Name = "Test Product", Description = "Test Description", Price = 10.0m, Stock = 5 };
            mockMediator.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(testProduct);
                        
            var controller = new ProductsController(null, mockMediator.Object);

            
            var result = await controller.GetProduct(1);

            
            Assert.IsType<OkObjectResult>(result.Result); 
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(testProduct, okResult.Value);
        }




        [Fact]
        public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult<Product>(null)); 

            
            var result = await _controller.GetProduct(999);

            
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task PostProduct_ShouldReturnCreatedAtActionResult_WithProduct()
        {
            
            var product = new Product { Name = "Product1", Description = "Description1", Price = 10.99m, Stock = 100 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1);

            
            var result = await _controller.PostProduct(product);

            
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedProduct = Assert.IsType<Product>(createdAtActionResult.Value);
            Assert.Equal(product.Name, returnedProduct.Name);
            Assert.Equal(product.Description, returnedProduct.Description);
            Assert.Equal(product.Price, returnedProduct.Price);
            Assert.Equal(product.Stock, returnedProduct.Stock);
        }

        [Fact]
        public async Task PutProduct_ShouldReturnNoContent_WhenProductIsUpdated()
        {
            
            var product = new Product { Id = 1, Name = "Updated Product", Description = "Updated Description", Price = 15.99m, Stock = 50 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            
            var result = await _controller.PutProduct(1, product);

            
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutProduct_ShouldReturnBadRequest_WhenProductIdMismatch()
        {
            
            var product = new Product { Id = 2, Name = "Updated Product", Description = "Updated Description", Price = 15.99m, Stock = 50 };

            
            var result = await _controller.PutProduct(1, product);

            
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNoContent_WhenProductIsDeleted()
        {
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            
            var result = await _controller.DeleteProduct(1);

            
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

           
            var result = await _controller.DeleteProduct(999);

            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}

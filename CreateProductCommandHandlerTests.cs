using System.Threading;
using System.Threading.Tasks;
using GestionProduits.Application.Products.Commands.CreateProduct;
using GestionProduits.Data;
using GestionProduits.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GestionProduits.Tests
{
    public class CreateProductCommandHandlerTests
    {
        private readonly CreateProductCommandHandler _handler;
        private readonly ApplicationDbContext _context;

        public CreateProductCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _handler = new CreateProductCommandHandler(_context);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldCreateProduct()
        {
            
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.99m,
                Stock = 100
            };

            
            var result = await _handler.Handle(command, CancellationToken.None);

            
            var product = await _context.Products.FindAsync(result);
            Assert.NotNull(product);
            Assert.Equal(command.Name, product.Name);
            Assert.Equal(command.Description, product.Description);
            Assert.Equal(command.Price, product.Price);
            Assert.Equal(command.Stock, product.Stock);
        }
    }
}

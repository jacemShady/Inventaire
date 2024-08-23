using System.Threading;
using System.Threading.Tasks;
using GestionProduits.Application.Products.Commands;
using GestionProduits.Data;
using GestionProduits.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestionProduits.Tests
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly UpdateProductCommandHandler _handler;
        private readonly ApplicationDbContext _context;

        public UpdateProductCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _handler = new UpdateProductCommandHandler(_context);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldUpdateProduct()
        {
            
            var product = new Product { Name = "Old Product", Price = 10.99m, Stock = 100 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var command = new UpdateProductCommand
            {
                Id = product.Id,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 15.99m,
                Stock = 50
            };

            
            var result = await _handler.Handle(command, CancellationToken.None);

            
            Assert.True(result);
            var updatedProduct = await _context.Products.FindAsync(product.Id);
            Assert.NotNull(updatedProduct);
            Assert.Equal(command.Name, updatedProduct.Name);
            Assert.Equal(command.Description, updatedProduct.Description);
            Assert.Equal(command.Price, updatedProduct.Price);
            Assert.Equal(command.Stock, updatedProduct.Stock);
        }
    }
}

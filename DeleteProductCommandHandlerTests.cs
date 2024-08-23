using System.Threading;
using System.Threading.Tasks;
using GestionProduits.Application.Products.Commands.DeleteProduct;
using GestionProduits.Data;
using GestionProduits.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestionProduits.Tests
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly DeleteProductCommandHandler _handler;
        private readonly ApplicationDbContext _context;

        public DeleteProductCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _handler = new DeleteProductCommandHandler(_context);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldDeleteProduct()
        {
            
            var product = new Product { Name = "Test Product", Price = 10.99m, Stock = 100 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var command = new DeleteProductCommand(product.Id);

            
            var result = await _handler.Handle(command, CancellationToken.None);

           
            Assert.True(result);
            var deletedProduct = await _context.Products.FindAsync(product.Id);
            Assert.Null(deletedProduct);
        }
    }
}

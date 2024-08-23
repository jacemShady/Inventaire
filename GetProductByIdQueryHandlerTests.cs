using System.Threading;
using System.Threading.Tasks;
using GestionProduits.Application.Products.Queries;
using GestionProduits.Data;
using GestionProduits.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestionProduits.Tests
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly GetProductByIdQueryHandler _handler;
        private readonly ApplicationDbContext _context;

        public GetProductByIdQueryHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _handler = new GetProductByIdQueryHandler(_context);
        }

        [Fact]
        public async Task Handle_GivenValidId_ShouldReturnProduct()
        {
           
            var product = new Product { Name = "Test Product", Price = 10.99m, Stock = 100 };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var query = new GetProductByIdQuery(product.Id);

            
            var result = await _handler.Handle(query, CancellationToken.None);

           
            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
        }
    }
}

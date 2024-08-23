using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GestionProduits.Application.Products.Queries;
using GestionProduits.Data;
using GestionProduits.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GestionProduits.Tests
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly GetAllProductsQueryHandler _handler;
        private readonly ApplicationDbContext _context;

        public GetAllProductsQueryHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _handler = new GetAllProductsQueryHandler(_context);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllProducts()
        {
           
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;
                            
            using (var context = new ApplicationDbContext(options))
            {
                context.Products.AddRange(
                    new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.0m, Stock = 100 },
                    new Product { Id = 2, Name = "Product 2", Description = "Description 2", Price = 20.0m, Stock = 200 }
                );
                await context.SaveChangesAsync();
            }
            
            using (var context = new ApplicationDbContext(options))
            {
                var handler = new GetAllProductsQueryHandler(context);
                
              
                var result = await handler.Handle(new GetAllProductsQuery(), CancellationToken.None);
                
                
                Assert.Equal(2, result.Count); 
            }
        }



    }
}


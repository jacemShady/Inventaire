using GestionProduits.Models;
using MediatR;

namespace GestionProduits.Application.Products.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}

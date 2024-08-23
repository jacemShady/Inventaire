using GestionProduits.Models;
using MediatR;
using System.Collections.Generic;

namespace GestionProduits.Application.Products.Queries
{
    public class GetAllProductsQuery : IRequest<List<Product>>
    {
    }
}

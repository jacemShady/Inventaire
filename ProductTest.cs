using GestionProduits.Models;
using Xunit;
public class ProductTests
{
    [Fact]
    public void Product_Properties_ShouldHaveGettersAndSetters()
    {
        var product = new Product();

        product.Name = "Test Product";
        product.Description = "This is a test product.";
        product.Price = 10.99m;
        product.Stock = 5;

        Assert.Equal("Test Product", product.Name);
        Assert.Equal("This is a test product.", product.Description);
        Assert.Equal(10.99m, product.Price);
        Assert.Equal(5, product.Stock);
    }
}
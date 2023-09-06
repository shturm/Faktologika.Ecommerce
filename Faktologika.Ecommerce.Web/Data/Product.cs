// namespace Faktologika.Ecommerce.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public double Price { get; set; }
    public string CustomJson { get; set; } = "{}";

    // public List<ProductCategory> Category { get; set; } = new List<ProductCategory>();
}
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public double Price { get; set; } = 0;
    public JObject CustomJson { get; set; } = new JObject();

}
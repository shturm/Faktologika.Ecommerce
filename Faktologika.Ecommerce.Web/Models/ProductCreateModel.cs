using Newtonsoft.Json.Linq;

public class ProductCreateModel
{
    public string Name { get; set; } = "";
    public double Price { get; set; } = 0;
    public JObject CustomJson { get; set; } = new JObject();
}
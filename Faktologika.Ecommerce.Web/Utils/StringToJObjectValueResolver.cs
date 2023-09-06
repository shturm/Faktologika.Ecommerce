using AutoMapper;
using Newtonsoft.Json.Linq;

public class StringToJObjectValueResolver : IValueResolver<Product, ProductDto, JObject>
{
    public JObject Resolve(Product source, ProductDto destination, JObject destMember, ResolutionContext context)
    {
        return JObject.Parse(source.CustomJson);
    }
}
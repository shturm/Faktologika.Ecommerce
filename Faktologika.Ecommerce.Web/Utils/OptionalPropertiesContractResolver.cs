using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class OptionalPropertiesContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        // Allow properties to be optional (skip them if missing in JSON data)
        property.Required = Required.Default;

        return property;
    }
}
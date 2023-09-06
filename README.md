# Scaffold from data entity
```dotnet aspnet-codegenerator controller -name ProductsController -m Product -dc Faktologika.Ecommerce.Web.EcommerceDbContext --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite```

# Generate API controller for model
```dotnet aspnet-codegenerator controller -name ProductsController -m Product -api -dc CatalogDbContext --databaseProvider sqlite```

# Generate default overriding razor pages for ASP.NET Core Identity
```dotnet aspnet-codegenerator identity -dc UsersDbContext --databaseProvider sqlite```
Does not generate service registration for `UserManager<TUser>` or related


# Add swagger

```dotnet add TodoApi.csproj package Swashbuckle.AspNetCore -v 6.2.3```

# Add a user secret that will be replaced within `appsettings.json`

```dotnet user-secrets set "Jwt:Key" "my secret"```

# [#1 Configuration for prod/dev](https://github.com/shturm/Faktologika.Ecommerce/issues/1)    

_NOTE_: Env variables are not in the `app.Configuration` in `Program.cs` for some reason, but when requested from inside a service scope (e.g. by a Controller) - they are correctly set.

Env variables with `FEW_` (Faktologika.Ecommerce.Web) prefix are included in configuration.

# Default seeded admin 

User: `admin@faktologika.bg`
Pass: `qqz#123QWE`

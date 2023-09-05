# Scaffold from data entity
```
dotnet aspnet-codegenerator controller -name ProductsController -m Product -dc Faktologika.Ecommerce.Web.EcommerceDbContext --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite
```

# Generate default overriding razor pages for ASP.NET Core Identity
```dotnet aspnet-codegenerator identity -dc UsersDbContext --databaseProvider sqlite```
Does not generate service registration for `UserManager<TUser>` or related

# Default seeded admin 
User: `admin@faktologika.bg`
Pass: `qqz#123QWE`
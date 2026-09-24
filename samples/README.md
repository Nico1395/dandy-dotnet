# Entity Framework Core Migrations
Adding migrations is done using:

```shell
dotnet ef migrations add InitialCreate \
    --project ./DandyDotnet.Samples.OnlineShop.Api.SharedKernel/DandyDotnet.Samples.OnlineShop.Api.SharedKernel.csproj \
    --startup-project ./DandyDotnet.Samples.OnlineShop.Api/DandyDotnet.Samples.OnlineShop.Api.csproj \
    --context ApiDbContext \
    --output-dir Infrastructure/Persistence/EntityFrameworkCore/Migrations
```

Updating the database is done using:

```shell
dotnet ef database update \
    --project ./DandyDotnet.Samples.OnlineShop.Api.SharedKernel \
    --startup-project ./DandyDotnet.Samples.OnlineShop.Api \
    --context ApiDbContext
```

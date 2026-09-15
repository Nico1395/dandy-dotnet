# DandyDotnet
- [API reference](https://nico1395.github.io/dandy-dotnet/)
- [What is currently being worked on?](https://github.com/Nico1395/dandy-dotnet/tree/main#what-is-currently-being-worked-on)
- [What is DandyDotnet?](https://github.com/Nico1395/dandy-dotnet/tree/main#what-is-dandydotnet)
- [Packages](https://github.com/Nico1395/dandy-dotnet/tree/main#packages)
- [Package overview](https://github.com/Nico1395/dandy-dotnet/tree/main#package-overview)
- [Is AI being used and if yes, how?](https://github.com/Nico1395/dandy-dotnet/tree/main#how-can-i-use-packages)

## What is currently being worked on?
For an overview over the current work and state of releases, issues, features and bugs see:
- [Releases and milestone progress](https://github.com/users/Nico1395/projects/43/views/6)
- [Release and milestone priorities](https://github.com/users/Nico1395/projects/43/views/2)
- [Current work](https://github.com/users/Nico1395/projects/43/views/1)
- [Label/category overview](https://github.com/users/Nico1395/projects/43/views/7)

---

## What is DandyDotnet?
DandyDotnet is a family of opinionated packages that build on top of [.NET Core](https://github.com/dotnet/runtime) and [ASP.NET Core](https://github.com/dotnet/aspnetcore). Their goal is to implement design patterns or architectural paradigms and make the use of tech such as RabbitMQ easier. Some packages are more opinionated than others. The individual goals of each package differ. Consult their documentation for further information.

Aside from being available for everyone, this framework is a bundle of solutions that are commonly used by me (Nico1395).

## Packages
DandyDotnet differentiates between _feature packages_ and _internal packages_ that are used to provide common abstractions, utilites and functionalities for feature packages. Internal packages should better not be used on their own.

Feature packages are production-ready frameworks. The following packages are feature packages for DandyDotnet:

- Mediator: `DandyDotnet.Patterns.Mediator.*`
- Strategies: `DandyDotnet.Patterns.Strategies.*`
- Event sourcing: `DandyDotnet.Patterns.EventSourcing.*`
- RabbitMQ: `DandyDotnet.EventDrivenArchitecture.RabbitMQ.*`

All packages are currently using .NET 10. The .NET version will be kept up to date. New features, bug fixes and alike will only be added to the newest packages for the newest supported .NET release.

Packages are always pushed to NuGet together, which is why they should be updated together. Version numbers of all packages will stay in sync.

## Package overview
This section offers a brief overview of all packages available on [nuget.org](https://www.nuget.org/). It contains links to the detailed documentation in the repositories wiki. For a detailed API reference see [here](https://nico1395.github.io/dandy-dotnet/).

|Name|Type|Docs|
|-|-|-|
|`DandyDotnet.Encoding.Abstractions`|Internal|Incoming|
|`DandyDotnet.Encoding`|Internal|Incoming|
|`DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions`|Feature|Incoming|
|`DandyDotnet.EventDrivenArchitecture.RabbitMQ`|Feature|Incoming|
|`DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions`|Feature|Incoming|
|`DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer`|Feature|Incoming|
|`DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions`|Feature|Incoming|
|`DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer`|Feature|Incoming|
|`DandyDotnet.Patterns.EventSourcing.Abstractions`|Feature|Incoming|
|`DandyDotnet.Patterns.EventSourcing`|Feature|Incoming|
|`DandyDotnet.Patterns.EventSourcing.Persistence.Sql`|Feature|Incoming|
|`DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres`|Feature|Incoming|
|`DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite`|Feature|Incoming|
|`DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer`|Feature|Incoming|
|`DandyDotnet.Patterns.Mediator.Abstractions`|Feature|Incoming|
|`DandyDotnet.Patterns.Mediator`|Feature|Incoming|
|`DandyDotnet.Patterns.Mediator.Queries.Abstractions`|Feature|Incoming|
|`DandyDotnet.Patterns.Mediator.Queries`|Feature|Incoming|
|`DandyDotnet.Patterns.Mediator.Commands.Abstractions`|Feature|Incoming|
|`DandyDotnet.Patterns.Mediator.Commands`|Feature|Incoming|
|`DandyDotnet.Patterns.Mediator.Validation`|Feature|Incoming|
|`DandyDotnet.Patterns.Strategies.Abstractions`|Feature|Incoming|
|`DandyDotnet.Patterns.Strategies`|Feature|Incoming|
|`DandyDotnet.Persistence.Sql.Abstractions`|Internal|Incoming|
|`DandyDotnet.Persistence.Sql.Postgres`|Internal|Incoming|
|`DandyDotnet.Persistence.Sql.Sqlite`|Internal|Incoming|
|`DandyDotnet.Persistence.Sql.SqlServer`|Internal|Incoming|
|`DandyDotnet.Persistence.Sql.Migrations.Abstractions`|Internal|Incoming|
|`DandyDotnet.Persistence.Sql.Migrations`|Internal|Incoming|
|`DandyDotnet.Persistence.Sql.Migrations.Postgres`|Internal|Incoming|
|`DandyDotnet.Persistence.Sql.Migrations.Sqlite`|Internal|Incoming|
|`DandyDotnet.Persistence.Sql.Migrations.SqlServer`|Internal|Incoming|
|`DandyDotnet.Serialization.Abstractions`|Internal|Incoming|
|`DandyDotnet.Serialization`|Internal|Incoming|
|`DandyDotnet.Serialization.NewtonsoftJson`|Internal|Incoming|
|`DandyDotnet.Serialization.SystemTextJson`|Internal|Incoming|
|`DandyDotnet.DependencyInjection.Abstractions`|Internal|Incoming|
|`DandyDotnet.DependencyInjection`|Internal|Incoming|
|`DandyDotnet.Http.StaticEndpoints`|Internal|Incoming|

## Is AI being used and if yes, how?
Yes, AI is being used. I am not a very heavy AI-user. However there are tasks that I use and consult AI for. This section briefly summarizes how AI is used and why.

Unit tests are easily writable for AI. Especially if the tests are designed manually and then fed as an instruction to an agent. At least from my experience, frameworks such as this are a bit more easy to understand for an AI because the scope of the project is more clearly lined out. Real-world systems have more open ends as they have to deal with the uncertainty and endless possibilities of the real world.

SQL strings can sometimes differ from one database system to another. I am no SQL expert by any means. However AI is great at SQL and can write it much, much more efficiently than me.

Writing documentation, especially for a growing framework, is exhausting and quite boring. AI can help a lot in this department. However the formatting is usually all over the place, which is why documentation has to be iterated.

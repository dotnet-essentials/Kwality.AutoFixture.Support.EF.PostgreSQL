# PostgreSQL support for AutoFixture

[![Continuous Integration](https://github.com/dotnet-essentials/Kwality.AutoFixture.Support.EF.PostgreSQL/actions/workflows/CI.yml/badge.svg)](https://github.com/dotnet-essentials/Kwality.AutoFixture.Support.EF.PostgreSQL/actions/workflows/CI.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=dotnet-essentials_Kwality.AutoFixture.Support.EF.PostgreSQL&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=dotnet-essentials_Kwality.AutoFixture.Support.EF.PostgreSQL)
[![CodeQL](https://github.com/dotnet-essentials/Kwality.AutoFixture.Support.EF.PostgreSQL/actions/workflows/CodeQL.yml/badge.svg)](https://github.com/dotnet-essentials/Kwality.AutoFixture.Support.EF.PostgreSQL/actions/workflows/CodeQL.yml)
[![Publish on NuGet](https://github.com/dotnet-essentials/Kwality.AutoFixture.Support.EF.PostgreSQL/actions/workflows/Publish.yml/badge.svg)](https://github.com/dotnet-essentials/Kwality.AutoFixture.Support.EF.PostgreSQL/actions/workflows/Publish.yml)
[![latest version](https://img.shields.io/nuget/v/Kwality.AutoFixture.Support.EF.PostgreSQL)](https://www.nuget.org/packages/Kwality.AutoFixture.Support.EF.PostgreSQL)

### Installation

The latest stable version is available on [NuGet](https://www.nuget.org/packages/Kwality.AutoFixture.Support.EF.PostgreSQL).

```sh
dotnet add package Kwality.AutoFixture.Support.EF.PostgreSQL
```

### Basic usage

This library adds support for using the PostgreSQL driver when AutoFixture generates a `DbContext` instance.

```cs
IFixture? fixture = new Fixture().Customize(new PostgreSqlCustomization(connectionString));

using var context = fixture.Create<TestDbContext>();

// TODO: Perform operations on your DbContext.

context.SaveChanges();
```

It's also possible to configure the `DbContext` instance with specific options, for example `NodaTime` support.

```cs
IFixture? fixture = new Fixture().Customize(new NodaTimeCustomization())
                                 .Customize(new PostgreSqlCustomization(connectionString, static options => options.UseNodaTime()));

using var context = fixture.Create<TestDbContext>();

// TODO: Perform operations on your DbContext.

context.SaveChanges();
```


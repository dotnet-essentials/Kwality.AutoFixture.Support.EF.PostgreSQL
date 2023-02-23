// =====================================================================================================================
// = LICENSE:       Copyright (c) 2022 Kevin De Coninck
// =
// =                Permission is hereby granted, free of charge, to any person
// =                obtaining a copy of this software and associated documentation
// =                files (the "Software"), to deal in the Software without
// =                restriction, including without limitation the rights to use,
// =                copy, modify, merge, publish, distribute, sublicense, and/or sell
// =                copies of the Software, and to permit persons to whom the
// =                Software is furnished to do so, subject to the following
// =                conditions:
// =
// =                The above copyright notice and this permission notice shall be
// =                included in all copies or substantial portions of the Software.
// =
// =                THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// =                EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// =                OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// =                NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// =                HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// =                WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// =                FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// =                OTHER DEALINGS IN THE SOFTWARE.
// =====================================================================================================================
namespace Kwality.AutoFixture.Support.EF.PostgreSQL.Tests.Customizations;

using System.ComponentModel.DataAnnotations;

using FluentAssertions;

using global::AutoFixture;

using JetBrains.Annotations;

using Kwality.AutoFixture.Support.EF.PostgreSQL.Customizations;

using MicroElements.AutoFixture.NodaTime;

using Microsoft.EntityFrameworkCore;

using NodaTime;

using Xunit;

[Collection("Sequential")]
[Trait("Type", "Integration Test")]
public sealed class PostgreSqlWithOptionsCustomizationTests
{
    private const string connectionString = "User ID=postgres;Password=PostgreSQL;Host=localhost;Port=5432;";

    [Fact(DisplayName = "Create a `DbContext` (with options) instance succeeds.")]
    internal void Create_DbContext_with_options_succeeds()
    {
        // ARRANGE.
        IFixture? fixture = new Fixture().Customize(new NodaTimeCustomization())
                                         .Customize(
                                             new PostgreSqlCustomization(
                                                 connectionString, static options => options.UseNodaTime()));

        // ACT.
        Action act = () =>
        {
            using var context = fixture.Create<TestDbContextWithOptions>();

            // Insert data.
            context.Subscriptions.Add(fixture.Create<Subscription>());

            // Persist data.
            context.SaveChanges();
        };

        // ASSERT.
        act.Should()
           .NotThrow();
    }

#pragma warning disable CA1812 // "Avoid uninstantiated internal classes" - Used implicitly.
    internal sealed class TestDbContextWithOptions : DbContext
    {
        public TestDbContextWithOptions(DbContextOptions<TestDbContextWithOptions> options)
            : base(options)
        {
        }

        [UsedImplicitly]
        public DbSet<Subscription> Subscriptions => this.Set<Subscription>();
    }
#pragma warning restore CA1812
#pragma warning disable CA1812 // "Avoid uninstantiated internal classes" - Used implicitly.
    internal sealed class Subscription
    {
        [Key]
        public int Id { get; set; }

        public LocalDate DateCreated { get; set; }
    }
#pragma warning restore CA1812
}

[Collection("Sequential")]
[Trait("Type", "Integration Test")]
public sealed class PostgreSqlCustomizationTests
{
    private const string connectionString = "User ID=postgres;Password=PostgreSQL;Host=localhost;Port=5432;";

    [Fact(DisplayName = "Create a `DbContext` instance succeeds.")]
    internal void Create_DbContext_succeeds()
    {
        // ARRANGE.
        IFixture? fixture = new Fixture().Customize(new PostgreSqlCustomization(connectionString));

        // ACT.
        Action act = () =>
        {
            using var context = fixture.Create<TestDbContext>();

            // Insert data.
            context.Users.Add(fixture.Create<User>());

            // Persist data.
            context.SaveChanges();
        };

        // ASSERT.
        act.Should()
           .NotThrow();
    }

    [Fact(DisplayName = "The created `DbContext` instance, uses the `PostgreSql` driver.")]
    internal void Created_DbContext_uses_the_postgresql_driver()
    {
        // ARRANGE.
        IFixture? fixture = new Fixture().Customize(new PostgreSqlCustomization(connectionString));

        // ACT.
        using var context = fixture.Create<TestDbContext>();

        // ASSERT.
        context.Database.IsNpgsql()
               .Should()
               .BeTrue();
    }

    [Fact(DisplayName = "The created `DbContext` instance, uses the specified connection string.")]
    internal void Created_DbContext_uses_the_specified_connection_string()
    {
        // ARRANGE.
        IFixture? fixture = new Fixture().Customize(new PostgreSqlCustomization(connectionString));

        // ACT.
        using var context = fixture.Create<TestDbContext>();

        // ASSERT.
        context.Database.GetDbConnection()
               .ConnectionString.Should()
               .Be("Username=postgres;Host=localhost;Port=5432");
    }

#pragma warning disable CA1812 // "Avoid uninstantiated internal classes" - Used implicitly.
    internal sealed class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        [UsedImplicitly]
        public DbSet<User> Users => this.Set<User>();
    }
#pragma warning restore CA1812
#pragma warning disable CA1812 // "Avoid uninstantiated internal classes" - Used implicitly.
    internal sealed class User
    {
        [Key]
        public int Id { get; set; }
    }
#pragma warning restore CA1812
}

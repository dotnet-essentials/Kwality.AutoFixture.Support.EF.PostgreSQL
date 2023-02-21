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
namespace Kwality.AutoFixture.Support.EF.PostgreSQL.Customizations;

using global::AutoFixture;
using global::AutoFixture.Kernel;

using JetBrains.Annotations;

using Kwality.AutoFixture.Support.EF.PostgreSQL.Core.Builders;
using Kwality.AutoFixture.Support.EF.PostgreSQL.Core.Command;
using Kwality.AutoFixture.Support.EF.PostgreSQL.Core.Specifications;

using Microsoft.EntityFrameworkCore;

using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

[PublicAPI]
public sealed class PostgreSqlCustomization : ICustomization
{
    private readonly string connectionString;
    private readonly Action<NpgsqlDbContextOptionsBuilder>? dbContextOptionsBuilder;

    public PostgreSqlCustomization(
        string connectionString, Action<NpgsqlDbContextOptionsBuilder>? dbContextOptionsBuilder = null)
    {
        this.connectionString = connectionString;
        this.dbContextOptionsBuilder = dbContextOptionsBuilder;
    }

    public void Customize(IFixture fixture)
    {
#pragma warning disable CA1062 // "Validate arguments of public methods" - Not null.
        fixture.Customizations.Add(
            new FilteringSpecimenBuilder(
                new GenericDbContextOptionsBuilder(), new ExactTypeSpecification(typeof(DbContextOptions<>))));

        fixture.Customizations.Add(
            new FilteringSpecimenBuilder(
                new Postprocessor(new MethodInvoker(new GreedyConstructorQuery()), new EnsureCreatedCommand()),
                new AndRequestSpecification(
                    new BaseTypeSpecification(typeof(DbContext)),
                    new InverseRequestSpecification(new AbstractTypeSpecification()))));

        fixture.Customizations.Add(
            new FilteringSpecimenBuilder(
                new DbContextOptionsBuilderBuilder(
                    new MethodInvoker(new ModestConstructorQuery()), this.connectionString,
                    this.dbContextOptionsBuilder), new ExactTypeSpecification(typeof(DbContextOptionsBuilder<>))));
#pragma warning restore CA1062
    }
}

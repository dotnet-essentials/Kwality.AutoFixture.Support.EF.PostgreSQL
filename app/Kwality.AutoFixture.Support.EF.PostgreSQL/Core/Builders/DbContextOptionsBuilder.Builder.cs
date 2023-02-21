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
namespace Kwality.AutoFixture.Support.EF.PostgreSQL.Core.Builders;

using global::AutoFixture.Kernel;

using Microsoft.EntityFrameworkCore;

using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

internal sealed class DbContextOptionsBuilderBuilder : ISpecimenBuilder
{
    private readonly ISpecimenBuilder builder;
    private readonly string connectionString;
    private readonly Action<NpgsqlDbContextOptionsBuilder>? optionsBuilder;

    public DbContextOptionsBuilderBuilder(
        ISpecimenBuilder builder, string connectionString,
        Action<NpgsqlDbContextOptionsBuilder>? optionsBuilder = default)
    {
        this.builder = builder;
        this.connectionString = connectionString;
        this.optionsBuilder = optionsBuilder;
    }

    public object Create(object request, ISpecimenContext context)
    {
        object? result = this.builder.Create(request, context);

        return ((DbContextOptionsBuilder)result).UseNpgsql(this.connectionString, this.optionsBuilder);
    }
}

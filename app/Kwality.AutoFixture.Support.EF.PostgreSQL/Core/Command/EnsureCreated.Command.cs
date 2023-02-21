// =====================================================================================================================
// = LICENSE:       Copyright (c) 2022 Kevin De Coninck\r
// =\r
// =                Permission is hereby granted, free of charge, to any person\r
// =                obtaining a copy of this software and associated documentation\r
// =                files (the "Software"), to deal in the Software without\r
// =                restriction, including without limitation the rights to use,\r
// =                copy, modify, merge, publish, distribute, sublicense, and/or sell\r
// =                copies of the Software, and to permit persons to whom the\r
// =                Software is furnished to do so, subject to the following\r
// =                conditions:\r
// =\r
// =                The above copyright notice and this permission notice shall be\r
// =                included in all copies or substantial portions of the Software.\r
// =\r
// =                THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,\r
// =                EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES\r
// =                OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND\r
// =                NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT\r
// =                HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,\r
// =                WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING\r
// =                FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR\r
// =                OTHER DEALINGS IN THE SOFTWARE.\r
// =====================================================================================================================
namespace Kwality.AutoFixture.Support.EF.PostgreSQL.Core.Command;

using global::AutoFixture.Kernel;

using Microsoft.EntityFrameworkCore;

internal sealed class EnsureCreatedCommand : ISpecimenCommand
{
    public void Execute(object specimen, ISpecimenContext context)
    {
        ((DbContext)specimen).Database.ExecuteSqlRaw("DROP SCHEMA public CASCADE;");
        ((DbContext)specimen).Database.ExecuteSqlRaw("CREATE SCHEMA public;");
        ((DbContext)specimen).Database.ExecuteSqlRaw("GRANT ALL ON SCHEMA public TO postgres;");
        ((DbContext)specimen).Database.ExecuteSqlRaw("GRANT ALL ON SCHEMA public TO public;");
        ((DbContext)specimen).Database.EnsureCreated();
    }
}

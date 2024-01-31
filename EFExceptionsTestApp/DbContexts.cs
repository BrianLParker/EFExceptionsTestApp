using Microsoft.EntityFrameworkCore;

namespace EFExceptionsTestApp;

[MySqlExceptions]
internal partial class MySqlContext(DbContextOptions<MySqlContext> options) :
    DbContext(options), IStudentContext<MySqlContext>
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Category> Categories { get; set; }
}

[SqlExceptions]
internal partial class SqlContext(DbContextOptions<SqlContext> options) :
    DbContext(options), IStudentContext<SqlContext>
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Category> Categories { get; set; }

}

[SQLiteExceptions]
internal partial class SQLLiteContext(DbContextOptions<SQLLiteContext> options) :
    DbContext(options), IStudentContext<SQLLiteContext>
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Category> Categories { get; set; }

}

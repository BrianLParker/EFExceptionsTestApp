using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFExceptionsTestApp;

internal class Program
{
    private const string SqlConnectionString = "Server=(localdb)\\mssqllocaldb;Database=TestAppData;Trusted_Connection=True;MultipleActiveResultSets=true";
    private const string SQLiteConnectionString = "Filename=TestAppData.db";
    private const string MySqlConnectionString = "server=localhost;database=TestAppData;user=Testuser;password=Pass1234!";

    private static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddDbContext<SqlContext>(options => options.UseSqlServer(SqlConnectionString));
        services.AddDbContext<SQLLiteContext>(options => options.UseSqlite(SQLiteConnectionString));
        services.AddDbContext<MySqlContext>(options => options.UseMySQL(MySqlConnectionString));
        var serviceProvider = services.BuildServiceProvider();
        List<IStudentContext> contextList = [];
        contextList.Add(serviceProvider.GetRequiredService<SqlContext>());
        contextList.Add(serviceProvider.GetRequiredService<SQLLiteContext>());
        contextList.Add(serviceProvider.GetRequiredService<MySqlContext>());
        InitialiseDatabases(contextList);
        Console.WriteLine("\r\n\r\nDuplicate Unique Index Item Test:");
        foreach (var context in contextList)
        {
            Console.WriteLine("  " + context.GetType().Name);
            TestDuplicateIndex(context);
        }
        RemoveDatabases(contextList);
    }

    private static void RemoveDatabases(List<IStudentContext> contextList)
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.Write("\r\n\r\nDeleting Databases");
        foreach (var context in contextList)
        {
            context.DbContext.Database.EnsureDeleted();
        }
        Console.WriteLine(". done.");
    }
    private static void InitialiseDatabases(List<IStudentContext> contextList)
    {
        RemoveDatabases(contextList);
        Console.WriteLine();
        Console.WriteLine("Creating Databases");
        foreach (var context in contextList)
        {
            Console.WriteLine("  " + context.GetType().Name);
            context.DbContext.Database.EnsureCreated();
            context.DbContext.Set<Category>().Add(new Category { Id = "01", Name = "Student" });
            context.SaveChanges();
        }
    }
    private static void TestDuplicateIndex(IStudentContext db)
    {
        List<Student> students = [
            new Student { Id = "01", Name = "Foo", CategoryId = "01" },
            new Student { Id = "01", Name = "Bar", CategoryId = "01" }, // SameIdex
            new Student { Id = "02", Name = "Foo", CategoryId = "01" }, // Same Index Key 
            new Student { Id = "03", Name = "Mar", CategoryId = "02" }, // Bad Foreign Key
        ];

        foreach (var student in students)
        {
            AddAndCatchStudentError(db, student);
        }
    }
    private static void AddAndCatchStudentError(IStudentContext db, Student student)
    {
        try
        {
            db.Students.Add(student);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    {ex.GetType().Name} {ex.Message}");
        }

        db.DbContext.Entry(student).State = EntityState.Detached;
    }
}

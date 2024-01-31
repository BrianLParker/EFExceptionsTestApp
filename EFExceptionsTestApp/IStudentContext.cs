using Microsoft.EntityFrameworkCore;

namespace EFExceptionsTestApp;

public interface IStudentContext
{
    DbContext DbContext { get; }
    int SaveChanges();
    DbSet<Student> Students { get; set; }
}

public interface IStudentContext<T> : IStudentContext
    where T : DbContext, IStudentContext<T>
{
    DbContext IStudentContext.DbContext => (this as DbContext)!;
}

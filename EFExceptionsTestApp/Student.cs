using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFExceptionsTestApp;

[Index(nameof(Name), IsUnique = true)]
public class Student
{
    [Key, StringLength(450)]
    public string Id { get; set; } = default(string)!;

    [Required, StringLength(450)]
    public string CategoryId { get; set; } = default(string)!;

    [Required, ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = default!;

    [Required, StringLength(450)]
    public string Name { get; set; } = default(string)!;
}

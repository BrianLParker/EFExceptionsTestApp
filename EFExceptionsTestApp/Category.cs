using System.ComponentModel.DataAnnotations;

namespace EFExceptionsTestApp;

public class Category
{
    [Key, StringLength(450)]
    public string Id { get; set; } = default(string)!;

    [Required, StringLength(450)]
    public string Name { get; set; } = default(string)!;
}

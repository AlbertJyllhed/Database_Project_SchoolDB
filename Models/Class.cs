namespace Database_Project_SchoolDB.Models;

public partial class Class
{
    public int Id { get; set; }

    public string? ClassName { get; set; }

    public int? TeacherId { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual Employee? Teacher { get; set; }
}

namespace Database_Project_SchoolDB.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string? SubjectName { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}

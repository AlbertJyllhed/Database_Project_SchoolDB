namespace Database_Project_SchoolDB.Models;

public partial class Grade
{
    public int? StudentId { get; set; }

    public int? SubjectId { get; set; }

    public int? TeacherId { get; set; }

    public string? Score { get; set; }

    public DateOnly? GradingDate { get; set; }

    public virtual Student? Student { get; set; }

    public virtual Subject? Subject { get; set; }

    public virtual Employee? Teacher { get; set; }
}

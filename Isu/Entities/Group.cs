using Isu.Entities;
using Isu.Models;
using Isu.Tools;

namespace Isu.Entities;

public class Group
{
    public const int MaximumAmountOfStudents = 25;
    private readonly GroupName _groupName;

    public Group(GroupName groupName, Schedule schedule)
    {
        if (groupName == null)
        {
            throw new ArgumentNullException(nameof(groupName));
        }

        _groupName = groupName;
        Schedule = schedule ?? throw new IsuException("Schedule is null");
        Faculty = new Faculty(groupName.Name[0]);
        MegaFaculty = MegaFaculty.MegaFaculties.First(megaFaculty => megaFaculty.Faculties.Any(faculty => faculty.FacultyLetter == Faculty.FacultyLetter));
        Students = new List<Student>();
    }

    public GroupName GroupName => _groupName;
    public List<Student> Students { get; }
    public Schedule Schedule { get; private set; }
    public Faculty Faculty { get; }
    public MegaFaculty MegaFaculty { get; }

    public void AddStudent(Student student)
    {
        if (Students.Count == MaximumAmountOfStudents)
        {
            throw new IsuException("You've reached maximum amount of students in group.");
        }

        Students.Add(student);
    }

    public void ChangeSchedule(Schedule schedule)
    {
        Schedule = schedule ?? throw new IsuException("Schedule is null");
    }

    public void AddLesson(Lesson lesson)
    {
        if (lesson == null)
        {
            throw new IsuException("Lesson is null");
        }

        if (!Schedule.Lessons.Contains(lesson))
        {
            Schedule.Lessons.Add(lesson);
        }
    }
}
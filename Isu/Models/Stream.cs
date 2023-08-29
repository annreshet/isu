using Isu.Entities;
using Isu.Tools;

namespace Isu.Models;

public class Stream
{
    private const int MaximumAmountOfStudents = 25;
    public Stream(Schedule schedule)
    {
        Schedule = schedule;
        Students = new List<Student>();
    }

    public Schedule Schedule { get; }
    public List<Student> Students { get; }

    public bool TryAddStudent(Student student)
    {
        if (Students.Count == MaximumAmountOfStudents)
        {
            throw new IsuException("Maximum amount of students was reached in OGNP stream");
        }

        Schedule studentSchedule = student.Schedule;
        return Schedule.Lessons.All(streamLesson => !studentSchedule.Lessons.Any(studentLesson => streamLesson.LessonTime.Day == studentLesson.LessonTime.Day && streamLesson.LessonTime.Time == studentLesson.LessonTime.Time));
    }
}
using Isu.Entities;
using Isu.Tools;

namespace Isu.Models;

public class Schedule
{
    public Schedule(List<Lesson> lessons)
    {
        Lessons = lessons;
    }

    public List<Lesson> Lessons { get; }

    public void AddLesson(Lesson lesson)
    {
        if (lesson == null)
        {
            throw new IsuException("Lesson is null");
        }

        Lessons.Add(lesson);
    }
}
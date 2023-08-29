using Isu.Models;
using Isu.Tools;
using Stream = Isu.Models.Stream;

namespace Isu.Entities;

public class Student
{
    private string _name;
    private int _id;

    public Student(string name, Group group, int id)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new IsuException("Name of a student cannot be empty.");
        }

        _name = name;
        Group = group;
        _id = id;
        Schedule = group.Schedule;
        Ognp = null;
    }

    public string Name => _name;
    public int Id => _id;
    public Group Group { get; private set; }
    public Schedule Schedule { get; }
    public Ognp? Ognp { get; private set; }

    public void ChangeGroup(Group newGroup)
    {
        newGroup.AddStudent(this);
        Group.Students.Remove(this);
        Group = newGroup;
    }
    
    public void SignUpForOgnp(Ognp ognp)
    {
        if (ognp.MegaFaculty.Name == Group.MegaFaculty.Name)
        {
            throw new IsuException("MegaFaculty of OGNP is the same as student's megaFaculty");
        }

        IReadOnlyList<Stream> streams = ognp.GetStreams();
        Stream? stream = streams.FirstOrDefault(stream => stream.TryAddStudent(this));

        if (stream == null)
        {
            throw new IsuException("Schedules overlap in every stream");
        }

        stream.Students.Add(this);
        Schedule.Lessons.AddRange(stream.Schedule.Lessons);

        Ognp = ognp;
    }
    
    public void LeaveOgnp()
    {
        if (Ognp == null)
        {
            throw new IsuException("Student is not signed up for OGNP yet");
        }

        IReadOnlyList<Stream> streams = Ognp.GetStreams();
        Stream stream = streams.First(stream => stream.Students.Contains(this));
        stream.Students.Remove(this);
        foreach (Lesson lesson in stream.Schedule.Lessons)
        {
            Schedule.Lessons.Remove(lesson);
        }

        Ognp = null;
    }
}

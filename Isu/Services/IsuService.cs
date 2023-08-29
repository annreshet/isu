using Isu.Entities;
using Isu.Models;
using Isu.Tools;
using Stream = Isu.Models.Stream;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private readonly List<Group> _allGroups = new ();
    private readonly List<Student> _allStudents = new ();
    private readonly List<Ognp> _allOgnps = new ();
    private int _id = 310000;
    public Group AddGroup(GroupName name, Schedule schedule)
    {
        if (FindGroup(name) != null)
        {
            throw new IsuException("Group with that name already exists");
        }
        
        var group = new Group(name, schedule);
        _allGroups.Add(group);
        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        _id++;
        var student = new Student(name, group, _id);
        group.AddStudent(student);
        _allStudents.Add(student);
        return student;
    }

    public Student GetStudent(int id)
    {
        Student? student = _allStudents.SingleOrDefault(student => student.Id == id);
        if (student == null)
        {
            throw new IsuException("There is no student with that id");
        }

        return student;
    }

    public Student? FindStudent(int id) => _allStudents.SingleOrDefault(student => student.Id == id);

    public IReadOnlyList<Student> FindStudents(GroupName groupName) => _allStudents.FindAll(student => student.Group.GroupName == groupName);

    public IReadOnlyList<Student> FindStudents(CourseNumber courseNumber) => _allStudents.FindAll(student => student.Group.GroupName.Course.Number == courseNumber.Number);

    public Group? FindGroup(GroupName groupName) => _allGroups.SingleOrDefault(group => group.GroupName == groupName);

    public List<Group> FindGroups(CourseNumber courseNumber) => _allGroups.FindAll(group => group.GroupName.Course.Number == courseNumber.Number);

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        if (!_allStudents.Contains(student))
        {
            throw new IsuException("The student doesn't exist.");
        }

        if (!_allGroups.Contains(newGroup))
        {
            throw new IsuException("The new group doesn't exist.");
        }

        student.ChangeGroup(newGroup);
    }
    
    public Ognp AddOgnp(MegaFaculty megaFaculty, List<Stream> streams)
    {
        Ognp? ognp = _allOgnps.FirstOrDefault(ognp => ognp.MegaFaculty == megaFaculty);
        if (ognp == null)
        {
            ognp = new Ognp(megaFaculty, streams);
            _allOgnps.Add(ognp);
        }
        else
        {
            foreach (Stream stream in streams)
            {
                ognp.AddStream(stream);
            }
        }

        return ognp;
    }

    public void SignUpForOgnp(Student student, Ognp ognp)
    {
        student.SignUpForOgnp(ognp);
    }

    public IReadOnlyList<Stream> GetStreams(Ognp ognp) => ognp.Streams;

    public IReadOnlyList<Student> GetStudentsFromStream(Ognp ognp, Stream stream)
    {
        if (!ognp.Streams.Contains(stream))
        {
            throw new IsuException("This stream doesn't belong to OGNP");
        }

        return stream.Students.AsReadOnly();
    }

    public IReadOnlyList<Student> GetStudentsWithoutOgnp() => _allStudents.FindAll(student => student.Ognp == null);

    public IReadOnlyList<Student> GetStudentsWithOgnp(Ognp ognp) => _allStudents.FindAll(student => student.Ognp == ognp);

    public void LeaveOgnp(Student student)
    {
        student.LeaveOgnp();
    }
}
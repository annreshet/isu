using Isu.Entities;
using Isu.Models;
using Stream = Isu.Models.Stream;

namespace Isu.Services;
public interface IIsuService
{
    Group AddGroup(GroupName name, Schedule schedule);
    Student AddStudent(Group group, string name);

    Student GetStudent(int id);
    Student? FindStudent(int id);
    IReadOnlyList<Student> FindStudents(GroupName groupName);
    IReadOnlyList<Student> FindStudents(CourseNumber courseNumber);

    Group? FindGroup(GroupName groupName);
    List<Group> FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(Student student, Group newGroup);
    
    Ognp AddOgnp(MegaFaculty megaFaculty, List<Stream> streams);
    void SignUpForOgnp(Student student, Ognp ognp);
    IReadOnlyList<Stream> GetStreams(Ognp ognp);
    IReadOnlyList<Student> GetStudentsFromStream(Ognp ognp, Stream stream);
    IReadOnlyList<Student> GetStudentsWithoutOgnp();
    IReadOnlyList<Student> GetStudentsWithOgnp(Ognp ognp);
    void LeaveOgnp(Student student);
}
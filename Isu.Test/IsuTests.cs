using Isu.Entities;
using Isu.Models;
using Isu.Services;
using Isu.Tools;
using Xunit;
using Stream = Isu.Models.Stream;

namespace Isu.Test;

public class IsuTests
{
    private readonly IIsuService _isuExtraService = new IsuService();
    
    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        var lessonTime1 = new LessonTime(Day.Monday, Time.FirstLesson);
        var lessonTime2 = new LessonTime(Day.Monday, Time.ThirdLesson);
        var lessonTime3 = new LessonTime(Day.Monday, Time.FifthLesson);
        var lesson1 = new Lesson(lessonTime1, 1234, "Ivanov");
        var lesson2 = new Lesson(lessonTime2, 1234, "Ivanov");
        var lesson3 = new Lesson(lessonTime3, 1234, "Ivanov");
        var schedule = new Schedule(new List<Lesson>() { lesson1, lesson2, lesson3 });
        var groupName = new GroupName("M32041");
        Group group = _isuExtraService.AddGroup(groupName, schedule);
        Student student = _isuExtraService.AddStudent(group, "help");
        Assert.Same(student.Group, group);
        Assert.Same(group.Students[0], student);
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        var lessonTime1 = new LessonTime(Day.Monday, Time.FirstLesson);
        var lessonTime2 = new LessonTime(Day.Monday, Time.ThirdLesson);
        var lessonTime3 = new LessonTime(Day.Monday, Time.FifthLesson);
        var lesson1 = new Lesson(lessonTime1, 1234, "Ivanov");
        var lesson2 = new Lesson(lessonTime2, 1234, "Ivanov");
        var lesson3 = new Lesson(lessonTime3, 1234, "Ivanov");
        var schedule = new Schedule(new List<Lesson>() { lesson1, lesson2, lesson3 });
        var groupName = new GroupName("M32041");
        Assert.Throws<IsuException>(() =>
        {
            Group group = _isuExtraService.AddGroup(groupName, schedule);
            for (int i = 0; i <= Group.MaximumAmountOfStudents; i++)
            {
                _isuExtraService.AddStudent(group, i.ToString());
            }
        });
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        var lessonTime1 = new LessonTime(Day.Monday, Time.FirstLesson);
        var lessonTime2 = new LessonTime(Day.Monday, Time.ThirdLesson);
        var lessonTime3 = new LessonTime(Day.Monday, Time.FifthLesson);
        var lesson1 = new Lesson(lessonTime1, 1234, "Ivanov");
        var lesson2 = new Lesson(lessonTime2, 1234, "Ivanov");
        var lesson3 = new Lesson(lessonTime3, 1234, "Ivanov");
        var schedule = new Schedule(new List<Lesson>() { lesson1, lesson2, lesson3 });
        Assert.Throws<IsuException>(() =>
        {
            _isuExtraService.AddGroup(new GroupName("12345"), schedule);
        });
        Assert.Throws<IsuException>(() =>
        {
            _isuExtraService.AddGroup(new GroupName("M2345"), schedule);
        });
        Assert.Throws<IsuException>(() =>
        {
            _isuExtraService.AddGroup(new GroupName("M3845"), schedule);
        });
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        var groupName = new GroupName("M32041");
        var newGroupName = new GroupName("M32031");
        var lessonTime1 = new LessonTime(Day.Monday, Time.FirstLesson);
        var lessonTime2 = new LessonTime(Day.Monday, Time.ThirdLesson);
        var lessonTime3 = new LessonTime(Day.Monday, Time.FifthLesson);
        var lesson1 = new Lesson(lessonTime1, 1234, "Ivanov");
        var lesson2 = new Lesson(lessonTime2, 1234, "Ivanov");
        var lesson3 = new Lesson(lessonTime3, 1234, "Ivanov");
        var schedule = new Schedule(new List<Lesson>() { lesson1, lesson2, lesson3 });
        Group group = _isuExtraService.AddGroup(groupName, schedule);
        Group newGroup = _isuExtraService.AddGroup(newGroupName, schedule);
        Student student = _isuExtraService.AddStudent(group, "Alex");
        _isuExtraService.ChangeStudentGroup(student, newGroup);
        Assert.Same(student.Group, newGroup);
        Assert.Empty(_isuExtraService.FindStudents(groupName));
    }

    [Fact]
    public void FindAndGetFunctionsReturnRightObjects()
    {
        var groupName = new GroupName("M32041");
        var groupName2 = new GroupName("M3104");
        var groupName3 = new GroupName("M32051");
        var lessonTime1 = new LessonTime(Day.Monday, Time.FirstLesson);
        var lessonTime2 = new LessonTime(Day.Monday, Time.ThirdLesson);
        var lessonTime3 = new LessonTime(Day.Monday, Time.FifthLesson);
        var lesson1 = new Lesson(lessonTime1, 1234, "Ivanov");
        var lesson2 = new Lesson(lessonTime2, 1234, "Ivanov");
        var lesson3 = new Lesson(lessonTime3, 1234, "Ivanov");
        var schedule = new Schedule(new List<Lesson>() { lesson1, lesson2, lesson3 });
        Group group = _isuExtraService.AddGroup(groupName, schedule);
        Group group2 = _isuExtraService.AddGroup(groupName2, schedule);
        Group group3 = _isuExtraService.AddGroup(groupName3, schedule);
        Student student = _isuExtraService.AddStudent(group, "Anna");
        Student student2 = _isuExtraService.AddStudent(group2, "Alex");
        Student student3 = _isuExtraService.AddStudent(group3, "Somebody");

        Assert.Same(_isuExtraService.FindStudent(310001), student);
        Assert.Same(_isuExtraService.GetStudent(310002), student2);
        Assert.Null(_isuExtraService.FindStudent(310004));
        Assert.Throws<IsuException>(() =>
        {
            _isuExtraService.GetStudent(310004);
        });

        Assert.Equal(_isuExtraService.FindStudents(groupName), new List<Student>() { student });
        Assert.Equal(_isuExtraService.FindStudents(groupName.Course), new List<Student>() { student, student3 });

        Assert.Same(_isuExtraService.FindGroup(groupName), group);
        Assert.Null(_isuExtraService.FindGroup(new GroupName("M33041")));

        Assert.Equal(_isuExtraService.FindGroups(groupName.Course), new List<Group>() { group, group3 });
    }

    [Fact]
    public void SignUpStudentForOgnp_OgnpStreamHasStudent()
    {
        var lessonTime1 = new LessonTime(Day.Monday, Time.FirstLesson);
        var lessonTime2 = new LessonTime(Day.Monday, Time.ThirdLesson);
        var lessonTime3 = new LessonTime(Day.Monday, Time.FifthLesson);
        var lesson1 = new Lesson(lessonTime1, 1234, "Ivanov");
        var lesson2 = new Lesson(lessonTime2, 1234, "Ivanov");
        var lesson3 = new Lesson(lessonTime3, 1234, "Ivanov");
        var schedule = new Schedule(new List<Lesson>() { lesson1, lesson2, lesson3 });
        var groupName = new GroupName("M32041");
        Group group = _isuExtraService.AddGroup(groupName, schedule);
        Student student = _isuExtraService.AddStudent(group, "help");

        lessonTime1 = new LessonTime(Day.Tuesday, Time.FirstLesson);
        lessonTime2 = new LessonTime(Day.Tuesday, Time.ThirdLesson);
        lessonTime3 = new LessonTime(Day.Tuesday, Time.FifthLesson);
        lesson1 = new Lesson(lessonTime1, 1234, "Ivanov");
        lesson2 = new Lesson(lessonTime2, 1234, "Ivanov");
        lesson3 = new Lesson(lessonTime3, 1234, "Ivanov");
        schedule = new Schedule(new List<Lesson>() { lesson1, lesson2, lesson3 });
        var megaFaculty = new MegaFaculty("NOZH");

        Assert.Throws<IsuException>(() =>
        {
            megaFaculty = new MegaFaculty("help");
        });

        Stream stream = new Stream(schedule);
        Ognp ognp = _isuExtraService.AddOgnp(megaFaculty, new List<Stream>() { stream });
        _isuExtraService.SignUpForOgnp(student, ognp);
        Assert.Same(ognp, student.Ognp);
        Assert.Same(student, _isuExtraService.GetStudentsFromStream(ognp, stream)[0]);
        Assert.Same(student, _isuExtraService.GetStudentsWithOgnp(ognp)[0]);

        _isuExtraService.LeaveOgnp(student);
        Assert.Null(student.Ognp);
        Assert.Empty(_isuExtraService.GetStreams(ognp)[0].Students);
        Assert.Empty(_isuExtraService.GetStudentsFromStream(ognp, stream));
        Assert.Same(student, _isuExtraService.GetStudentsWithoutOgnp()[0]);
    }

    [Fact]
    public void TestExceptionsOgnp()
    {
        var lessonTime1 = new LessonTime(Day.Monday, Time.FirstLesson);
        var lessonTime2 = new LessonTime(Day.Monday, Time.ThirdLesson);
        var lessonTime3 = new LessonTime(Day.Monday, Time.FifthLesson);
        var lesson1 = new Lesson(lessonTime1, 1234, "Ivanov");
        var lesson2 = new Lesson(lessonTime2, 1234, "Ivanov");
        var lesson3 = new Lesson(lessonTime3, 1234, "Ivanov");
        var schedule = new Schedule(new List<Lesson>() { lesson1, lesson2, lesson3 });
        var groupName = new GroupName("M32041");
        Group group = _isuExtraService.AddGroup(groupName, schedule);
        Student student = _isuExtraService.AddStudent(group, "help");

        var megaFaculty = new MegaFaculty("NOZH");
        lessonTime2 = new LessonTime(Day.Tuesday, Time.ThirdLesson);
        lessonTime3 = new LessonTime(Day.Tuesday, Time.FifthLesson);
        lesson2 = new Lesson(lessonTime2, 1234, "Ivanov");
        lesson3 = new Lesson(lessonTime3, 1234, "Ivanov");
        schedule = new Schedule(new List<Lesson>() { lesson1, lesson2, lesson3 });
        var stream = new Stream(schedule);
        Ognp ognp = _isuExtraService.AddOgnp(megaFaculty, new List<Stream>() { stream });
        Assert.Throws<IsuException>(() =>
        {
            _isuExtraService.SignUpForOgnp(student, ognp);
        });

        lessonTime1 = new LessonTime(Day.Tuesday, Time.FirstLesson);
        lesson1 = new Lesson(lessonTime1, 1234, "Ivanov");
        schedule = new Schedule(new List<Lesson>() { lesson1, lesson2, lesson3 });
        stream = new Stream(schedule);
        megaFaculty = new MegaFaculty("TINT");
        ognp = _isuExtraService.AddOgnp(megaFaculty, new List<Stream>() { stream });
        Assert.Throws<IsuException>(() =>
        {
            _isuExtraService.SignUpForOgnp(student, ognp);
        });
    }
}
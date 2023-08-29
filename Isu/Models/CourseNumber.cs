using Isu.Tools;

namespace Isu.Models;

public class CourseNumber
{
    private const int MinimalCourseNumber = 1;
    private const int MaximalCourseNumber = 4;
    public CourseNumber(int courseNumber)
    {
        if (courseNumber < MinimalCourseNumber || courseNumber > MaximalCourseNumber)
        {
            throw new IsuException("Wrong course number.");
        }

        Number = courseNumber;
    }

    public int Number { get; }
}
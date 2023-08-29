using Isu.Tools;

namespace Isu.Models;

public class GroupName
{
    private const int MinimalLength = 5;
    private const char BachelorIndicator = '3';
    private readonly string _groupName;
    private readonly CourseNumber _courseNumber;

    public GroupName(string groupName)
    {
        if (string.IsNullOrEmpty(groupName))
        {
            throw new IsuException("Name of a group cannot be empty.");
        }

        char firstSymbol = groupName[0];
        char secondSymbol = groupName[1];
        char thirdSymbol = groupName[2];
        if (groupName.Length < MinimalLength || !char.IsLetter(firstSymbol) || secondSymbol != BachelorIndicator)
        {
            throw new IsuException("Wrong name of the group.");
        }

        _courseNumber = new CourseNumber(Convert.ToInt32(thirdSymbol) - '0');
        _groupName = groupName;
    }

    public string Name => _groupName;
    public CourseNumber Course => _courseNumber;
}
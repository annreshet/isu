using Isu.Models;
using Isu.Tools;

namespace Isu.Entities;

public class Lesson
{
    private const int MinimalRoomNumber = 1000;
    private const int MinimalBuildingNumber = 1;
    private const int MaximumBuildingNumber = 2;
    private const int MinimalFloor = 1;
    private const int MaximumFloor = 5;
    public Lesson(LessonTime lessonTime, int room, string teacher)
    {
        if (room < MinimalRoomNumber)
        {
            throw new IsuException("Wrong format of a room.");
        }

        int buildingNumber = room / 1000;
        if (buildingNumber is < MinimalBuildingNumber or > MaximumBuildingNumber)
        {
            throw new IsuException("Wrong building number in lesson's room");
        }

        int floor = (room / 100) % 10;
        if (floor is < MinimalFloor or > MaximumFloor)
        {
            throw new IsuException("Wrong floor number in lesson's room");
        }

        if (string.IsNullOrEmpty(teacher))
        {
            throw new IsuException("Name of a teacher cannot be empty");
        }

        LessonTime = lessonTime;
        Room = room;
        Teacher = teacher;
    }

    public LessonTime LessonTime { get; }
    public int Room { get; }
    public string Teacher { get; }
}
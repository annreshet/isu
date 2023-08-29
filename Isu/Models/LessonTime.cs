namespace Isu.Models;

public class LessonTime
{
    public LessonTime(Day day, Time time)
    {
        Day = day;
        Time = time;
    }

    public Day Day { get; }

    public Time Time { get; }
}
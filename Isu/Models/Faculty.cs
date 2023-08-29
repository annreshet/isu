using System.Collections.ObjectModel;
using Isu.Tools;

namespace Isu.Models;

public class Faculty
{
    private readonly IReadOnlyCollection<char> _facultyLetters =
        new Collection<char>() { 'B', 'D', 'G', 'H', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'T', 'U', 'V', 'W', 'Z' };

    public Faculty(char letter)
    {
        if (!_facultyLetters.Contains(letter))
        {
            throw new IsuException("Wrong faculty letter.");
        }

        FacultyLetter = letter;
    }

    public char FacultyLetter { get; }
}
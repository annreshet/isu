using Isu.Models;
using Isu.Tools;

namespace Isu.Entities;

public class MegaFaculty
{
    public static readonly List<MegaFaculty> MegaFaculties = new List<MegaFaculty>()
    {
        new ("KTU"), new ("NOZH"),
        new ("FTMI"), new ("TINT"),
        new ("PHYS"),
    };
    public MegaFaculty(string name)
    {
        switch (name)
        {
            case "KTU":
                Faculties = new List<Faculty>()
                {
                    new Faculty('R'), new Faculty('N'),
                    new Faculty('H'), new Faculty('P'),
                }.AsReadOnly();
                break;
            case "NOZH":
                Faculties = new List<Faculty>()
                {
                    new Faculty('G'), new Faculty('O'),
                    new Faculty('T'), new Faculty('W'),
                }.AsReadOnly();
                break;
            case "FTMI":
                Faculties = new List<Faculty>()
                {
                    new Faculty('D'), new Faculty('U'),
                }.AsReadOnly();
                break;
            case "TINT":
                Faculties = new List<Faculty>()
                {
                    new Faculty('K'), new Faculty('M'),
                }.AsReadOnly();
                break;
            case "PHYS":
                Faculties = new List<Faculty>()
                {
                    new Faculty('B'), new Faculty('L'),
                    new Faculty('V'), new Faculty('Z'),
                }.AsReadOnly();
                break;
            default:
                throw new IsuException("Name of the megafaculty is not valid");
        }

        Name = name;
    }

    public string Name { get; }
    public IReadOnlyCollection<Faculty> Faculties { get; }
}
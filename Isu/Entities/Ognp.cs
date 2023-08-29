using Isu.Entities;
using Isu.Tools;
using Stream = Isu.Models.Stream;

namespace Isu.Entities;

public class Ognp
{
    public Ognp(MegaFaculty megaFaculty, List<Stream> streams)
    {
        if (megaFaculty == null)
        {
            throw new IsuException("MegaFaculty is null");
        }

        MegaFaculty = megaFaculty;
        OgnpStreams = streams;
        Name = megaFaculty.Name + "_OGNP";
    }

    public MegaFaculty MegaFaculty { get; }
    public IReadOnlyList<Stream> Streams => OgnpStreams.AsReadOnly();
    public string Name { get; }
    private List<Stream> OgnpStreams { get; }

    public void AddStream(Stream stream)
    {
        if (stream == null)
        {
            throw new IsuException("Stream is null");
        }

        OgnpStreams.Add(stream);
    }

    public IReadOnlyList<Stream> GetStreams()
    {
        return Streams;
    }
}
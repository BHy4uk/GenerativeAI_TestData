using Bogus;
using CsvHelper;

public class Title
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ReleaseYear { get; set; }
    public string AgeCertification { get; set; }
    public int Runtime { get; set; }
    public string Genres { get; set; }
    public string ProductionCountry { get; set; }
    public string Seasons { get; set; }
}

public class Credit
{
    public int Id { get; set; }
    public int TitleId { get; set; }
    public string RealName { get; set; }
    public string CharacterName { get; set; }
    public string Role { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        var ageCertifications = new[] { "G", "PG", "PG-13", "R", "NC-17", "U", "U/A", "A", "S", "AL", "6", "9", "12", "12A", "15", "18", "18R", "R18", "R21", "M", "MA15+", "R16", "R18+", "X18", "T", "E", "E10+", "EC", "C", "CA", "GP", "M/PG", "TV-Y", "TV-Y7", "TV-G", "TV-PG", "TV-14", "TV-MA" };
        var roles = new[] { "Director", "Producer", "Screenwriter", "Actor", "Actress", "Cinematographer", "Film Editor", "Production Designer", "Costume Designer", "Music Composer" };

        var titleFaker = new Faker<Title>()
            .RuleFor(t => t.Id, f => f.IndexFaker)
            .RuleFor(t => t.Name, f => f.Lorem.Word())
            .RuleFor(t => t.Description, f => f.Lorem.Sentence())
            .RuleFor(t => t.ReleaseYear, f => f.Random.Int(1900, DateTime.Now.Year))
            .RuleFor(t => t.AgeCertification, f => f.PickRandom(ageCertifications))
            .RuleFor(t => t.Runtime, f => f.Random.Int(60, 180))
            .RuleFor(t => t.Genres, f => f.Lorem.Words(f.Random.Int(1, 3)).Aggregate((a, b) => a + ", " + b))
            .RuleFor(t => t.ProductionCountry, f => f.Address.CountryCode())
            .RuleFor(t => t.Seasons, f => f.Random.Bool() ? "" : f.Random.Int(1, 10).ToString());

        var creditFaker = new Faker<Credit>()
            .RuleFor(c => c.Id, f => f.IndexFaker)
            .RuleFor(c => c.TitleId, f => f.Random.Int(0, 99))
            .RuleFor(c => c.RealName, f => f.Person.FullName)
            .RuleFor(c => c.CharacterName, f => f.Lorem.Word())
            .RuleFor(c => c.Role, f => f.PickRandom(roles));

        var titles = titleFaker.Generate(150);
        var credits = creditFaker.Generate(150);

        WriteToCsv(titles, "titles.csv");
        WriteToCsv(credits, "credits.csv");
    }

    public static void WriteToCsv<T>(List<T> records, string path)
    {
        using (var writer = new StreamWriter(path))
        using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
    }
}
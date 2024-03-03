namespace BierFroh;

internal class NameGenerator
{
    private readonly IReadOnlyList<string> prefixes =
    [
        "Bier",
        "Gier",
        "Pier",
        "Stier",
        "Tier",
        "Zier",
        "Hier",
        "Schier",
        "Schmier",
        "Zier"
    ];

    private readonly IReadOnlyList<string> suffixes =
    [
        "Abo",
        "Afro",
        "Bosco",
        "Echo",
        "Ego",
        "Floh",
        "Go",
        "Ivo",
        "Klo",
        "Mo",
        "Odo",
        "Ordo",
        "Po",
        "Spacko",
        "Stroh",
        "Ufo",
        "Uno",
        "Froh",
        "Oho",
        "Roh",
        "So",
        "Wo",
        "Zwo",
        "Öko"
    ];

    private readonly Random random = new();

    public string GetAppName()
    {
        var prefixIndex = random.Next(prefixes.Count);
        var suffixIndex = random.Next(suffixes.Count);

        return prefixes[prefixIndex] + suffixes[suffixIndex];
    }
}

namespace BierFroh.Modules.DependencyGraph.Model;

internal partial class Dependency : IDependency, IEquatable<Dependency>
{
    private readonly HashSet<Dependency> dependencies = new();

    private Dependency(string name, string framework, string version)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        if (string.IsNullOrEmpty(framework))
            throw new ArgumentException($"'{nameof(framework)}' cannot be null or empty.", nameof(framework));
        if (string.IsNullOrEmpty(version))
            throw new ArgumentException($"'{nameof(version)}' cannot be null or empty.", nameof(version));

        Name = name;
        Framework = framework;
        Version = version;
    }

    public string Name { get; set; }
    public string Framework { get; set; }
    public string Version { get; set; }
    public IReadOnlyCollection<IDependency> Dependencies => dependencies;

    public bool Equals(Dependency? other)
    {
        return other is not null
            && Name == other.Name
            && Framework == other.Framework
            && Version == other.Version;
    }

    public override bool Equals(object? obj) => obj is Dependency dependency && Equals(dependency);

    public override int GetHashCode() => HashCode.Combine(Name, Framework, Version);

    public static bool operator ==(Dependency a, Dependency b) => a.Equals(b);

    public static bool operator !=(Dependency a, Dependency b) => !(a == b);
}


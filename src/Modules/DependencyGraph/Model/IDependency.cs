namespace BierFroh.Modules.DependencyGraph.Model;

public interface IDependency
{
    string Name { get; }
    string Framework { get; }
    string Version { get; }
    IReadOnlyCollection<IDependency> Dependencies { get; }
}

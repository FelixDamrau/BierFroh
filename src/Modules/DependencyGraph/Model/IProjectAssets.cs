namespace BierFroh.Modules.DependencyGraph.Model;

public interface IProjectAssets
{
    string ProjectName { get; }
    string Version { get; }
    IReadOnlyCollection<string> Frameworks { get; }
    IReadOnlyCollection<IDependency> Dependencies { get; }
}

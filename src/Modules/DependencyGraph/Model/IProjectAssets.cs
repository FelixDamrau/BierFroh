namespace BierFroh.Modules.DependencyGraph.Model;

public interface IProjectAssets
{
    string ProjectName { get; }
    string Version { get; }
    IReadOnlySet<string> Frameworks { get; }
    IReadOnlyList<IDependency> Dependencies { get; }
}

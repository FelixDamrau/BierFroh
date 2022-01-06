namespace BierFroh.Modules.DependencyGraph.Model;

public interface IProjectAssets
{
    string ProjectName { get; }
    string Version { get; }
    IReadOnlyList<IDependency> Dependencies { get; }
}

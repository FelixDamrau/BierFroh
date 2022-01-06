namespace BierFroh.Modules.DependencyGraph.Model;

public interface IProjectAssets
{
    public string ProjectName { get; }
    IReadOnlyList<IDependency> Dependencies { get; }
}

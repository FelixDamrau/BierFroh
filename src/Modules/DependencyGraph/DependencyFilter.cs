using BierFroh.Modules.DependencyGraph.Model;

namespace BierFroh.Modules.DependencyGraph;
public class DependencyFilter
{
    private readonly HashSet<IDependency> dependencies;

    public DependencyFilter(HashSet<IDependency> dependencies)
    {
        this.dependencies = dependencies;
    }

    public bool Contains(IDependency dependency) => dependency is not null && dependencies.Contains(dependency);
}

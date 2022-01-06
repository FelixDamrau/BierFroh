namespace BierFroh.Modules.DependencyGraph.Model;

public interface IDependency
{
    string Name { get; }
    string Version { get; }
}

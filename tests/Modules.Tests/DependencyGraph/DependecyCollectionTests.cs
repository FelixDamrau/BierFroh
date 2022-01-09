using BierFroh.Modules.DependencyGraph.Model;

namespace BierFroh.Modules.Tests.DependencyGraph;
public class DependecyCollectionTests
{
    private readonly Dependency.DependencyCollection dependencyCollection = new();

    [Fact]
    public void CanAddDependecy()
    {
        var dependecy = dependencyCollection.Add("name", "framework", "version");

        Assert.NotNull(dependecy);
    }

    [Fact]
    public void CanAddSameDependency()
    {
        var dependecy1 = dependencyCollection.Add("name", "framework", "version");
        var dependecy2 = dependencyCollection.Add("name", "framework", "version");

        Assert.Same(dependecy1, dependecy2);
    }
}

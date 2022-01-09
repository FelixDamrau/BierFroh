using BierFroh.Modules.DependencyGraph.Model;

namespace BierFroh.Modules.Tests.DependencyGraph;
public class DependencyTests
{
    private readonly Dependency.DependencyCollection dependencyCollection = new();

    [Fact]
    public void CanCreateDependency()
    {
        var name = "name";
        var framework = "framework";
        var version = "version";
        var dependency = dependencyCollection.Add(name, framework, version);

        Assert.Equal(name, dependency.Name);
        Assert.Equal(framework, dependency.Framework);
        Assert.Equal(version, dependency.Version);
    }

    [Fact]
    public void SameDependenciesAreEqual()
    {
        var name = "name";
        var framework = "framework";
        var version = "version";
        var dependency1 = dependencyCollection.Add(name, framework, version);
        var dependency2 = dependencyCollection.Add(name, framework, version);

        Assert.True(dependency1.Equals(dependency2));
    }

    [Fact]
    public void SameDependenciesAreEqual2()
    {
        var name = "name";
        var framework = "framework";
        var version = "version";
        var dependency1 = dependencyCollection.Add(name, framework, version);
        var dependency2 = dependencyCollection.Add(name, framework, version);

        Assert.True(Equals(dependency1, dependency2));
    }

    [Fact]
    public void SameDependencieCanBeAddedOnceToHashSet()
    {
        var name = "name";
        var framework = "framework";
        var version = "version";
        var dependency1 = dependencyCollection.Add(name, framework, version);
        var dependency2 = dependencyCollection.Add(name, framework, version);

        var hashSet = new HashSet<Dependency>();
        Assert.True(hashSet.Add(dependency1));
        Assert.False(hashSet.Add(dependency2));
    }
}

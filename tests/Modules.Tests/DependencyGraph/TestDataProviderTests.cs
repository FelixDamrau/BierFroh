using BierFroh.Modules.Tests.DependencyGraph.TestData;

namespace BierFroh.Modules.Tests.DependencyGraph;

public class TestDataProviderTests
{
    [Fact]
    public void GetEmbeddedResources()
    {
        var resources = TestDataProvider.Get();

        Assert.Equal(1, resources.Count);
    }

    [Fact]
    public async void ResourcesAreNonEmpty()
    {
        var resources = TestDataProvider.Get();
        
        var readResources = await Task.WhenAll(resources);

        Assert.All(readResources, (r) => Assert.True(!string.IsNullOrWhiteSpace(r)));
    }
}

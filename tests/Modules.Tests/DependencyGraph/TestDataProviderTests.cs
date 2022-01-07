using BierFroh.Modules.Tests.DependencyGraph.TestData;

namespace BierFroh.Modules.Tests.DependencyGraph;

public class TestDataProviderTests
{
    [Fact]
    public void GetEmbeddedResourceStreams()
    {
        var resourceStreams = TestDataProvider.Get();

        Assert.Equal(1, resourceStreams.Count());
        DisposeAll(resourceStreams);
    }

    [Fact]
    public void ResourceStreamsAreNonEmpty()
    {
        var resourceStreams = TestDataProvider.Get();

        Assert.All(resourceStreams, (r) => Assert.True(r.Length > 0));
        DisposeAll(resourceStreams);
    }

    private static void DisposeAll(IEnumerable<IDisposable> disposables)
    {
        foreach (var disposable in disposables)
            disposable.Dispose();
    }
}

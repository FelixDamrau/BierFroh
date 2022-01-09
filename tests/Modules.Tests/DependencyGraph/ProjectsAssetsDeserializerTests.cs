using BierFroh.Modules.DependencyGraph;
using BierFroh.Modules.Tests.DependencyGraph.TestData;

namespace BierFroh.Modules.Tests.DependencyGraph;

public class ProjectsAssetsDeserializerTests
{
    [Theory]
    [ClassData(typeof(TestDataProvider))]
    public async void DoesSetProjectName(Stream stream)
    {
        var projectAssets = await ProjectAssetsDeserializer.DeserializeAsync(stream).ConfigureAwait(false);

        Assert.False(string.IsNullOrWhiteSpace(projectAssets.ProjectName));
    }

    [Theory]
    [ClassData(typeof(TestDataProvider))]
    public async void DoesSetProjectVersion(Stream stream)
    {
        var projectAssets = await ProjectAssetsDeserializer.DeserializeAsync(stream).ConfigureAwait(false);

        Assert.False(string.IsNullOrWhiteSpace(projectAssets.Version));
    }

    [Theory]
    [ClassData(typeof(TestDataProvider))]
    public async void DoesSetProjectFrameworks(Stream stream)
    {
        var projectAssets = await ProjectAssetsDeserializer.DeserializeAsync(stream).ConfigureAwait(false);

        Assert.NotEmpty(projectAssets.Frameworks);
        Assert.All(projectAssets.Frameworks, (f) => Assert.False(string.IsNullOrWhiteSpace(f)));
    }

    [Theory]
    [ClassData(typeof(TestDataProvider))]
    public async void DoesSetDependencyName(Stream stream)
    {
        var projectAssets = await ProjectAssetsDeserializer.DeserializeAsync(stream).ConfigureAwait(false);

        Assert.NotEmpty(projectAssets.Dependencies);
        Assert.All(projectAssets.Dependencies, (d) => Assert.False(string.IsNullOrWhiteSpace(d.Name)));
    }

    [Theory]
    [ClassData(typeof(TestDataProvider))]
    public async void DoesSetDependencyVersion(Stream stream)
    {
        var projectAssets = await ProjectAssetsDeserializer.DeserializeAsync(stream).ConfigureAwait(false);

        Assert.All(projectAssets.Dependencies, (d) => Assert.False(string.IsNullOrWhiteSpace(d.Version)));
    }
}

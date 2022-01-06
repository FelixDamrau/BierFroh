using BierFroh.Modules.DependencyGraph;
using BierFroh.Modules.Tests.DependencyGraph.TestData;

namespace BierFroh.Modules.Tests.DependencyGraph;

public class ProjectsAssetsDeserializerTests
{
    [Theory]
    [ClassData(typeof(TestDataProvider))]
    public async void DoesSetProjectName(Task<string> json)
    {
        var jsonText = await json.ConfigureAwait(false);

        var projectAssets = ProjectAssetsDeserializer.Deserialize(jsonText);

        Assert.False(string.IsNullOrWhiteSpace(projectAssets.ProjectName));
    }
}

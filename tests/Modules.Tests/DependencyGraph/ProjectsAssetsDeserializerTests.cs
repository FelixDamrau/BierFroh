using System.Text;
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
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonText));

        var projectAssets = await ProjectAssetsDeserializer.DeserializeAsync(stream).ConfigureAwait(false);

        Assert.False(string.IsNullOrWhiteSpace(projectAssets.ProjectName));
    }
}

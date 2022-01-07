using System.Reflection;

namespace BierFroh.Modules.Tests.DependencyGraph.TestData;

internal class TestDataProvider : IEnumerable<object[]>
{
    public static IEnumerable<Stream> Get()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceNames = assembly.GetManifestResourceNames().Where(r => r.StartsWith("BierFroh.Modules.Tests.DependencyGraph.TestData"));
        var relevantResources = new List<Task<string>>();
        foreach (var resource in resourceNames)
            yield return assembly.GetManifestResourceStream(resource) ?? throw new InvalidOperationException($"The embedded resource '{resource}' was not found!");
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        return Get()
            .Select(r => new object[] { r })
            .GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

using System.Reflection;

namespace BierFroh.Modules.Tests.DependencyGraph.TestData;

internal class TestDataProvider : IEnumerable<object[]>
{
    public static IReadOnlyList<Task<string>> Get()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceNames = assembly.GetManifestResourceNames().Where(r => r.StartsWith("BierFroh.Modules.Tests.DependencyGraph.TestData"));
        var relevantResources = new List<Task<string>>();
        foreach (var resource in resourceNames)
        {
            using var stream = assembly.GetManifestResourceStream(resource) ?? throw new InvalidOperationException($"The embedded resource '{resource}' was not found!");
            using var reader = new StreamReader(stream);
            var resourceText = reader.ReadToEndAsync();
            relevantResources.Add(resourceText);
        }
        return relevantResources;
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        return Get()
            .Select(r => new object[] { r })
            .GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

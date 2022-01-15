using BierFroh.Modules.Common;
using BierFroh.Modules.DependencyGraph.Model;

namespace BierFroh.Modules.DependencyGraph;
public class DependencyFilterFactory
{
    private readonly int depth = 1;
    private readonly Dictionary<string, HashSet<IDependency>> cache = new();

    public DependencyFilterFactory(IProjectAssets projectAssets)
    {
        foreach (var dependency in projectAssets.Dependencies)
        {
            var key = GetKey(dependency.Name);
            AddToCache(key, dependency);
        }
    }

    public IEnumerable<string> GetKeys() => cache.Keys;

    public int Count(string key) => cache.TryGetValue(key, out var keys) ? keys.Count : 0;

    public DependencyFilter CreateFilter(IEnumerable<string> keys)
    {
        var dependencies = new HashSet<IDependency>();
        foreach (var key in keys.Where(k => cache.ContainsKey(k)))
        {
            var values = cache[key];
            dependencies.UnionWith(values);
        }
        return new DependencyFilter(dependencies);
    }

    private string GetKey(string dependencyName)
    {
        var index = dependencyName.NthIndexOf('.', depth);
        return index >= 0 ? dependencyName[..index] : dependencyName;
    }

    private void AddToCache(string key, IDependency dependency)
    {
        if (cache.TryGetValue(key, out var dependencies))
            dependencies.Add(dependency);
        else
            cache[key] = new HashSet<IDependency> { dependency };
    }
}

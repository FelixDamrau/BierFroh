using System.Collections;
using Develix.Essentials.Core;

namespace BierFroh.Modules.DependencyGraph.Model;

internal partial class Dependency
{
    internal class DependencyCollection : IReadOnlyCollection<IDependency>
    {
        private readonly HashSet<Dependency> dependencies = new();

        public Dependency Add(string name, string framework, string version)
        {
            var dependency = new Dependency(name, framework, version);
            if (!dependencies.Add(dependency))
            {
                var result = Get(dependency);
                return result.Valid
                    ? result.Value
                    : throw new InvalidOperationException("The added dependency was not found. (This is bad)");
            }
            return dependency;
        }

        public void AddDependency(Dependency dependency, Dependency dependentDependency) => dependency.dependencies.Add(dependentDependency);

        public int Count => dependencies.Count;

        public IEnumerator<IDependency> GetEnumerator() => dependencies.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private Result<Dependency> Get(Dependency dependency)
        {
            if (dependencies.TryGetValue(dependency, out var storedDependency))
                return Result.Ok(storedDependency);
            return Result.Fail<Dependency>("Dependency not found");
        }
    }
}

using System.Text.Json;
using BierFroh.Modules.DependencyGraph.Model;

namespace BierFroh.Modules.DependencyGraph
{
    public class ProjectAssetsDeserializer
    {
        public static async Task<IProjectAssets> DeserializeAsync(Stream stream)
        {
            var projectAssets = new ProjectAssets();
            var jsonDocument = await JsonDocument.ParseAsync(stream).ConfigureAwait(false);
            var projectNode = jsonDocument.RootElement.GetProperty("project");

            projectAssets.ProjectName = GetProjectName(projectNode);
            projectAssets.Version = GetVersion(projectNode);
            projectAssets.Frameworks = GetFrameworks(projectNode);
            projectAssets.Dependencies = GetDependencies(projectNode);

            return projectAssets;
        }


        private static string GetProjectName(JsonElement projectNode)
        {
            return projectNode
                .GetProperty("restore")
                .GetProperty("projectName")
                .GetString() ?? throw new Exception();
        }

        private static string GetVersion(JsonElement projectNode)
        {
            return projectNode
                .GetProperty("version")
                .GetString() ?? throw new Exception();
        }

        private static HashSet<string> GetFrameworks(JsonElement projectNode)
        {
            var frameworks = projectNode.GetProperty("frameworks").EnumerateObject();
            var deserializedFrameworks = new HashSet<string>();
            foreach (var framework in frameworks)
                deserializedFrameworks.Add(framework.Name);

            return deserializedFrameworks;
        }

        private static List<Dependency> GetDependencies(JsonElement projectNode)
        {
            var dependencies = projectNode.GetProperty("frameworks").GetProperty("net6.0").GetProperty("dependencies").EnumerateObject();
            var deserializedDependencies = new List<Dependency>();
            foreach (var dependency in dependencies)
            {
                var deserializedDependency = new Dependency
                {
                    Name = dependency.Name,
                    Version = dependency.Value.GetProperty("version").GetString() ?? throw new Exception(),
                };
                deserializedDependencies.Add(deserializedDependency);
            }
            return deserializedDependencies;
        }
    }
}

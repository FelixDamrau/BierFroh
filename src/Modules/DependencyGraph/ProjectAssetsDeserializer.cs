using System.Text.Json;
using BierFroh.Modules.DependencyGraph.Model;

namespace BierFroh.Modules.DependencyGraph
{
    public class ProjectAssetsDeserializer
    {
        public static IProjectAssets Deserialize(string jsonText)
        {
            var projectAssets = new ProjectAssets();
            var jsonDocument = JsonDocument.Parse(jsonText);
            var projectNode = jsonDocument.RootElement.GetProperty("project");

            projectAssets.ProjectName = GetProjectName(projectNode);
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

        private static List<Dependency> GetDependencies(JsonElement projectNode)
        {
            var dependencies = projectNode.GetProperty("frameworks").GetProperty("net6.0").GetProperty("dependencies").EnumerateObject();
            var deserializedDependencies = new List<Dependency>();
            foreach (var dependency in dependencies)
            {
                var deserializedDependency = new Dependency { Name = dependency.Name };
                deserializedDependencies.Add(deserializedDependency);
            }
            return deserializedDependencies;
        }
    }
}

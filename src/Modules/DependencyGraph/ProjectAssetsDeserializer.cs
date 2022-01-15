using System.Text.Json;
using System.Text.RegularExpressions;
using BierFroh.Modules.DependencyGraph.Model;

namespace BierFroh.Modules.DependencyGraph
{
    public class ProjectAssetsDeserializer
    {
        private static readonly Regex matchVersionRegex = new(@"[\d+\.]+");
        public static async Task<IProjectAssets> DeserializeAsync(Stream stream)
        {
            var projectAssets = new ProjectAssets();
            var jsonDocument = await JsonDocument.ParseAsync(stream).ConfigureAwait(false);
            var projectNode = jsonDocument.RootElement.GetProperty("project");
            var targetsNode = jsonDocument.RootElement.GetProperty("targets");

            projectAssets.ProjectName = GetProjectName(projectNode);
            projectAssets.Version = GetVersion(projectNode);
            projectAssets.Frameworks = GetFrameworks(projectNode);
            projectAssets.DependencyCollection.Add(projectAssets.ProjectName, projectAssets.Frameworks.First(), projectAssets.Version);
            AddProjectDependencies(projectAssets, projectNode);
            AddDependencies(projectAssets, targetsNode);

            return projectAssets;
        }

        private static string GetProjectName(JsonElement projectNode)
        {
            return projectNode
                .GetProperty("restore")
                .GetProperty("projectName")
                .GetString() ?? throw new InvalidOperationException($"The project name of the project-restore node is not set!");
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
            {
                var parsedFramework = ToTfm(framework.Name);
                deserializedFrameworks.Add(parsedFramework);
            }

            return deserializedFrameworks;
        }

        /// <summary>
        /// Converts a stock keeping unit to an SDK style target framework moniker string.
        /// Can parse all sku ids for .NET Framework 4.0 to 4.8.
        /// If the <paramref name="sku"/> cannot be parsed to given parameter is returned unchanged.
        /// </summary>
        private static string ToTfm(string sku)
        {
            return sku switch
            {
                ".NETFramework,Version=4.5" => "net45",
                ".NETFramework,Version=4.5.1" => "net451",
                ".NETFramework,Version=v4.5.2" => "net452",
                ".NETFramework,Version=4.6" => "net46",
                ".NETFramework,Version=4.6.1" => "net461",
                ".NETFramework,Version=4.6.2" => "net462",
                ".NETFramework,Version=4.7" => "net47",
                ".NETFramework,Version=4.7.1" => "net471",
                ".NETFramework,Version=4.7.2" => "net472",
                ".NETFramework,Version=4.8" => "net48",
                _ when sku.StartsWith(".NETFramework,Version=4") => "net40",
                _ => sku,
            };
        }

        private static void AddProjectDependencies(ProjectAssets projectAssets, JsonElement projectNode)
        {
            var frameworks = projectNode.GetProperty("frameworks").EnumerateObject();
            foreach (var framework in frameworks)
            {
                var dependencies = framework.Value.GetProperty("dependencies").EnumerateObject();
                foreach (var dependency in dependencies)
                {
                    var rawVersion = dependency.Value.GetProperty("version").GetString()
                        ?? throw new InvalidOperationException($"The version of the dependency '{dependency.Name}' is not set!");
                    var version = ParseVersion(rawVersion);
                    var addedDependecy = projectAssets.DependencyCollection.Add(
                        dependency.Name,
                        framework.Name,
                        version);
                    var projectDependency = projectAssets.DependencyCollection.Add(projectAssets.ProjectName, projectAssets.Frameworks.First(), projectAssets.Version);
                    projectAssets.DependencyCollection.AddDependency(projectDependency, addedDependecy);
                }
            }


            static string ParseVersion(string rawVersion)
            {
                if (char.IsDigit(rawVersion.First())) // This is a workaround to correctly parse alpha versions
                    return rawVersion;

                var match = matchVersionRegex.Match(rawVersion);
                return match.Success ? match.Groups[0].Value : rawVersion;
            }
        }

        private static void AddDependencies(ProjectAssets projectAssets, JsonElement targetsNode)
        {
            foreach (var framework in targetsNode.EnumerateObject())
            {
                var dependencies = framework.Value.EnumerateObject();
                foreach (var dependency in dependencies)
                    AddDependencies(projectAssets, framework, dependency);
            }
        }

        private static void AddDependencies(ProjectAssets projectAssets, JsonProperty framework, JsonProperty dependency)
        {
            var (dependencyName, version) = GetCurrentDependency(dependency.Name);
            var currentDependency = projectAssets.DependencyCollection.Add(dependencyName, framework.Name, version);
            if (dependency.Value.TryGetProperty("dependencies", out var dependentDependencies))
            {
                foreach (var dependentDependency in dependentDependencies.EnumerateObject())
                {
                    var dependentDependencyVersion = dependentDependency.Value.GetString()
                        ?? throw new InvalidOperationException($"The version of the dependecy '{dependentDependency.Name}' is not set!");
                    var currentDependentDependency = projectAssets.DependencyCollection.Add(dependentDependency.Name, framework.Name, dependentDependencyVersion);
                    projectAssets.DependencyCollection.AddDependency(currentDependency, currentDependentDependency);
                }
            }

            static (string name, string version) GetCurrentDependency(string dependencyName)
            {
                var splitName = dependencyName.Split('/');
                if (splitName.Length != 2)
                    throw new InvalidOperationException($"The dependency name '{dependencyName}' is in an unexpected format. Should be like: 'System.Text.Json/6.0.0'");
                return (splitName[0], splitName[1]);
            }
        }
    }
}

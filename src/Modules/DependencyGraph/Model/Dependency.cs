﻿namespace BierFroh.Modules.DependencyGraph.Model;

internal class Dependency : IDependency
{
    public string Name { get; set; }

    public string Version { get; set; }
}
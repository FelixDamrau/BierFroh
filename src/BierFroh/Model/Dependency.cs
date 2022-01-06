﻿using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace BierFroh.Model;
public class Dependency : NodeModel
{
    public string? Version { get; set; }

    public Dependency(Point point)
        : base(point)
    {
    }
}
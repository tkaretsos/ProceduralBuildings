using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Thesis {

public sealed class BuildingLot
{
  // edges[0] is _always_ adjacent to a street
  public readonly List<Edge> edges;

  private bool _isFinal;

  // edges[_biggest] gives the longest edge
  private int _biggest;

  public BuildingLot (Block parent)
  {
    edges = new List<Edge>();
    Vector3 mod, from, to;
    for (int i = 0; i < 4; ++i)
    {
      mod = Vector3.ClampMagnitude(10 * (parent.edges[i].direction - parent.edges[(i + 3) % 4].direction), 6f);
      from = parent.edges[i].start + mod;
      mod = Vector3.ClampMagnitude(10 * (parent.edges[(i + 1) % 4].direction - parent.edges[i].direction), 6f);
      to = parent.edges[i].end + mod;
      edges[i] = new Edge(from, to);
    }
  }
}

} // namespace Thesis
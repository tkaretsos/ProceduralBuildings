using UnityEngine;

namespace Thesis {

public sealed class BuildingLot
{
  public readonly Edge[] edges;

  public BuildingLot (Block parent)
  {
    edges = new Edge[4];
    Vector3 mod, from, to;
    for (int i = 0; i < 4; ++i)
    {
      mod = Vector3.ClampMagnitude(5 * (parent.edges[i].direction - parent.edges[(i + 3) % 4].direction), 3f);
      from = parent.edges[i].start + mod;
      mod = Vector3.ClampMagnitude(5 * (parent.edges[(i + 1) % 4].direction - parent.edges[i].direction), 3f);
      to = parent.edges[i].end + mod;
      edges[i] = new Edge(from, to);
    }
  }
}

} // namespace Thesis
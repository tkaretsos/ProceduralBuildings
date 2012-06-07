using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Thesis {

public sealed class BuildingLot
{
  // edges[0] is _always_ adjacent to a street
  public readonly List<Edge> edges;

  public readonly BuildingLot initialLot;

  private bool _isFinal;

  // edges[_biggest] gives the longest edge
  public int biggest;

  public BuildingLot (Block parent)
  {
    this.initialLot = this;
    edges = new List<Edge>();
    Vector3 mod, from, to;
    for (int i = 0; i < 4; ++i)
    {
      mod = Vector3.ClampMagnitude(10 * (parent.edges[i].direction - parent.edges[(i + 3) % 4].direction), 6f);
      from = parent.edges[i].start + mod;
      mod = Vector3.ClampMagnitude(10 * (parent.edges[(i + 1) % 4].direction - parent.edges[i].direction), 6f);
      to = parent.edges[i].end + mod;
      edges.Add(new Edge(from, to));
    }

    // find index of longest edge
    biggest = edges.FindIndex(delegate (Edge edge) {
      return edge.length == edges.Max(e => e.length);
    });

    FindIfIsFinal();
  }

  public BuildingLot (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, BuildingLot source)
  {
    initialLot = source.initialLot;
    edges = new List<Edge>();
    edges.Add(new Edge(p1, p2));
    edges.Add(new Edge(p2, p3));
    edges.Add(new Edge(p3, p4));
    edges.Add(new Edge(p4, p1));

    // find index of longest edge
    biggest = edges.FindIndex(delegate (Edge edge) {
      return edge.length == edges.Max(e => e.length);
    });

    FindIfIsFinal();
  }

  private void FindIfIsFinal ()
  {
    float max = edges[0].length;
    float min = edges.Min(e => e.length);

    if (max >= 2.5f * min)
    {
      _isFinal = false;
      return;
    }

    if (max < 17f && min < 16f)
    {
      _isFinal = true;
      return;
    }

    if (max < 25f && min < 16f && Util.RollDice(new float[] { 0.2f, 0.8f }) == 1)
    {
      _isFinal = true;
      return;
    }

    _isFinal = false;
  }

  public void Bisect ()
  {
    if (_isFinal)
    {
      if (initialLot.EdgesContain(edges[0].start) || initialLot.EdgesContain(edges[0].end))
        CityMapManager.Instance.Add(this);
      return;
    }

    BuildingLot b1 = new BuildingLot(edges[3].start, edges[3].end,
                                     edges[0].middle, edges[2].middle,
                                     this);
    b1.Bisect();
    BuildingLot b2 = new BuildingLot(edges[1].start, edges[1].end,
                                     edges[2].middle, edges[0].middle,
                                     this);
    b2.Bisect();
  }

  public bool EdgesContain (Vector3 point)
  {
    foreach (Edge e in edges)
      if (e.Contains(point))
        return true;

    return false;
  }
}

} // namespace Thesis
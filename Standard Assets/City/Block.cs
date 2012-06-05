using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Thesis {
  
public class Block
{
  /*************** FIELDS ***************/

  public readonly List<Edge> edges;

  public Edge bisector;

  private bool _isFinal;

  private const float _divergence = 0.08f;

  public BuildingLot lot;

  /*************** CONSTRUCTORS ***************/

  public Block (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
  {
    int i = CityMapManager.Instance.Add(p1);
    if (i != -1)
      p1 = CityMapManager.Instance.nodes[i];
    
    i = CityMapManager.Instance.Add(p2);
    if (i != -1)
      p2 = CityMapManager.Instance.nodes[i];
    
    i = CityMapManager.Instance.Add(p3);
    if (i != -1)
      p3 = CityMapManager.Instance.nodes[i];
    
    i = CityMapManager.Instance.Add(p4);
    if (i != -1)
      p4 = CityMapManager.Instance.nodes[i];
    
    var edgeList = new List<Edge>();
    edgeList.Add(new Edge(p1, p2));
    edgeList.Add(new Edge(p2, p3));
    edgeList.Add(new Edge(p3, p4));
    edgeList.Add(new Edge(p4, p1));

    // find index of longest edge
    i = edgeList.FindIndex(delegate (Edge edge) {
      return edge.length == edgeList.Max(e => e.length);
    });

    // put the edges in clockwise order starting from the biggest
    edges = new List<Edge>();
    for (int j = 0; j < 4; ++j)
      edges.Add(edgeList[(i + j) % 4]);

    isFinal();
  }

  public void Bisect ()
  {
    if (_isFinal)
    {
      lot = new BuildingLot(this);
      CityMapManager.Instance.Add(this);
      return;
    }

    bisector = new Edge(edges[0].middle, edges[2].middle);

    var big_offset = (Random.Range(0f, _divergence * edges[0].length)) * edges[0].direction;
    if (Util.RollDice(new float[] { 0.5f, 0.5f }) == 1)
      big_offset *= -1;
    var opp_offset = (Random.Range(0f, _divergence * edges[2].length)) * edges[2].direction;
    if (Util.RollDice(new float[] { 0.5f, 0.5f }) == 1)
      opp_offset *= -1;

    Block b1 = new Block(edges[3].start, edges[3].end,
                         edges[0].middle + big_offset, edges[2].middle + opp_offset);
    b1.Bisect();
    Block b2 = new Block(edges[1].start, edges[1].end,
                         edges[2].middle + opp_offset, edges[0].middle + big_offset);
    b2.Bisect();
  }

  private void isFinal ()
  {
    float min = edges.Min(e => e.length);
    if (min < 20f)
    {
      _isFinal = true;
      return;
    }

    if (min < 25f && Util.RollDice(new float[] { 0.7f, 0.3f }) == 1)
    {
      _isFinal = true;
      return;
    }

    if (min < 30f && Util.RollDice(new float[] { 0.3f, 0.7f }) == 1)
    {
      _isFinal = true;
      return;
    }

    _isFinal = false;
  }
}

} // namespace Thesis
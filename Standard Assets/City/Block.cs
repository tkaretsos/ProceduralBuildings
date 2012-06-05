using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Thesis {
  
public class Block
{
  /*************** FIELDS ***************/

  public readonly List<Edge> edges;

  public readonly Edge biggest;

  public readonly Edge opposite;

  private readonly Edge _left;

  private readonly Edge _right;

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
    
    edges = new List<Edge>();
    edges.Add(new Edge(p1, p2));
    edges.Add(new Edge(p2, p3));
    edges.Add(new Edge(p3, p4));
    edges.Add(new Edge(p4, p1));

    biggest = edges.Find(delegate (Edge edge) {
      return edge.length == edges.Max(e => e.length);
    });
    i = edges.IndexOf(biggest);
    opposite = edges[(i + 2) % 4];
    _left    = edges[(i + 3) % 4];
    _right   = edges[(i + 1) % 4];

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

    bisector = new Edge(biggest.middle, opposite.middle);

    var big_offset = (Random.Range(0f, _divergence * biggest.length)) * biggest.direction;
    if (Util.RollDice(new float[] { 0.5f, 0.5f }) == 1)
      big_offset *= -1;
    var opp_offset = (Random.Range(0f, _divergence * opposite.length)) * opposite.direction;
    if (Util.RollDice(new float[] { 0.5f, 0.5f }) == 1)
      opp_offset *= -1;

    Block b1 = new Block(_left.start, _left.end,
                         biggest.middle + big_offset, opposite.middle + opp_offset);
    b1.Bisect();
    Block b2 = new Block(_right.start, _right.end,
                         opposite.middle + opp_offset, biggest.middle + big_offset);
    b2.Bisect();
  }

  private void isFinal ()
  {
    float min = edges.Min(e => e.length);
    if (min < 12f)
    {
      _isFinal = true;
      return;
    }

    if (min < 22f && Util.RollDice(new float[] { 0.7f, 0.3f }) == 1)
    {
      _isFinal = true;
      return;
    }

    if (min < 27f && Util.RollDice(new float[] { 0.3f, 0.7f }) == 1)
    {
      _isFinal = true;
      return;
    }

    _isFinal = false;
  }
}

} // namespace Thesis
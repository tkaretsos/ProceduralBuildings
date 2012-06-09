using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Thesis {
  
public class Block
{
  /*************** FIELDS ***************/

  public readonly List<Edge> edges;

  private bool _isFinal;

  private const float _divergence = 0.05f;

  public BuildingLot initialLot;

  public List<BuildingLot> finalLots = new List<BuildingLot>();

  public readonly float sidewalkWidth = 1.75f;

  public readonly float roadWidth = 2.25f;

  public Vector3[] sidewalkVerts;

  public Vector3[] lotVerts;

  public Road road;

  public Sidewalk sidewalk;

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

    FindIfIsFinal();
  }

  public void Bisect ()
  {
    if (_isFinal)
    {
      initialLot = new BuildingLot(this);
      if (initialLot.isFinal())
        finalLots.Add(initialLot);
      else
        CreateBuildingLots();

      CityMapManager.Instance.Add(this);
      return;
    }

    var big_offset = Random.Range(0f, _divergence * edges[0].length) * edges[0].direction;
    if (Util.RollDice(new float[] { 0.5f, 0.5f }) == 1)
      big_offset *= -1;
    var opp_offset = Random.Range(0f, _divergence * edges[2].length) * edges[2].direction;
    if (Util.RollDice(new float[] { 0.5f, 0.5f }) == 1)
      opp_offset *= -1;

    Block b1 = new Block(edges[3].start, edges[3].end,
                         edges[0].middle + big_offset, edges[2].middle + opp_offset);
    b1.Bisect();
    Block b2 = new Block(edges[1].start, edges[1].end,
                         edges[2].middle + opp_offset, edges[0].middle + big_offset);
    b2.Bisect();
  }

  private void FindIfIsFinal ()
  {
    float min = edges.Min(e => e.length);
    if (min < 25f)
    {
      _isFinal = true;
      return;
    }

    if (edges[0].length > 63f)
    {
      _isFinal = false;
      return;
    }

    if (min < 35f && Util.RollDice(new float[] { 0.7f, 0.3f }) == 1)
    {
      _isFinal = true;
      return;
    }

    if (min < 45f && Util.RollDice(new float[] { 0.3f, 0.7f }) == 1)
    {
      _isFinal = true;
      return;
    }

    _isFinal = false;
  }

  private void CreateBuildingLots()
  {
    var es = initialLot.edges;
    var minLength = initialLot.edges.Min(e => e.length);

    if (minLength <= 16f)
    {
      finalLots.Add(new BuildingLot(es[0].start, es[0].middle,
                                    es[2].middle, es[2].end));
      finalLots.Add(new BuildingLot(es[0].middle, es[0].end,
                                    es[2].start, es[2].middle));
      return;
    }

    if (minLength <= 27f && initialLot.edges[0].length <= 30f)
    {
      var r = Util.RollDice(new float[] { 0.5f, 0.5f });
      var pointBig = es[0].start + r / 3f * es[0].length * es[0].direction;
      var pointOpp = es[2].start + (3 - r) / 3f * es[2].length * es[2].direction;
      var pointMid = (pointBig + pointOpp) / 2;

      finalLots.Add(new BuildingLot(es[1].start, es[1].middle,
                                    pointMid, pointBig));
      finalLots.Add(new BuildingLot(es[1].middle, es[1].end,
                                    pointOpp, pointMid));
      finalLots.Add(new BuildingLot(es[0].start, pointBig,
                                    pointMid, es[3].middle));
      finalLots.Add(new BuildingLot(pointOpp, es[2].end,
                                    es[3].middle, pointMid));

      return;
    }

    if (minLength <= 27f && es[0].length <= 40f)
    {
      var bisector = new Edge(es[3].middle, es[1].middle);
      var r = Random.Range(11, 16);

      var p1big = es[0].start +  r * es[0].direction;
      var p2big = p1big + Vector3.Distance(p1big, es[0].end) / 2 * es[0].direction;
      var p1mid = es[3].middle + r / es[0].length * bisector.length * bisector.direction;
      var p2mid = p1mid + Vector3.Distance(p1mid, bisector.end) / 2 * bisector.direction;

      finalLots.Add(new BuildingLot(es[0].start, p1big, p1mid, bisector.start));
      finalLots.Add(new BuildingLot(p1big, p2big, p2mid, p1mid));
      finalLots.Add(new BuildingLot(p2big, es[0].end, bisector.end, p2mid));

      var p1opp = es[2].start + r * es[2].direction;
      var p2opp = p1opp + Vector3.Distance(p1opp, es[2].end) / 2 * es[2].direction;
      var p3mid = es[1].middle - r / es[2].length * bisector.length * bisector.direction;
      var p4mid = p3mid - Vector3.Distance(p3mid, bisector.start) / 2 * bisector.direction;

      finalLots.Add(new BuildingLot(es[2].start, p1opp, p3mid, es[1].middle));
      finalLots.Add(new BuildingLot(p1opp, p2opp, p4mid, p3mid));
      finalLots.Add(new BuildingLot(p2opp, es[2].end, es[3].middle, p4mid));

      return;
    }

    // for initialLots larger than 40x27
    // big face range -> 11 - 20
    // small face range -> 6 - 9
    // therefore small = (3big + 21) / 9
    float r1, r2;
    Vector3 p1, p2, p3, p4;

    for (int i = 0; i < 3; i+=2)
    while (es[i].length - initialLot.occupied[i] > 7f)
    {
      if (initialLot.pointsInEdge[i].Count == 0)
      {
        r1 = Random.Range(11, 20);
        r2 = (3 * r1 + 21) / 9f;
        p1 = es[i].start;
        p2 = p1 + r1 * es[i].direction;
        p4 = es[i].start - r2 * es[(i + 3) % 4].direction;   
        p3 = Util.IntersectionPoint(p2, es[i].right, p4, es[i].direction);

        initialLot.pointsInEdge[i].Add(p2);
        initialLot.occupied[i] += r1;
        initialLot.pointsInEdge[(i + 3) % 4].Add(p4);
        initialLot.occupied[(i + 3) % 4] += r2;
        finalLots.Add(new BuildingLot(p1, p2, p3, p4));
      }
      else
      {
        r1 = es[i].length - initialLot.occupied[i];
        if (r1 > 31f)
        {
          r1 = Random.Range(11, 20);
          r2 = (3 * r1 + 21) / 9f;

          p1 = initialLot.pointsInEdge[i].Last();
          p2 = p1 + r1 * es[i].direction;
          p3 = p2 + r2 * es[i].right;
          p4 = p1 + r2 * es[i].right;

          initialLot.pointsInEdge[i].Add(p2);
          initialLot.occupied[i] += r1;
          finalLots.Add(new BuildingLot(p1, p2, p3, p4));
        }
        else if (25f <= r1 && r1 <= 31f)
        {
          r1 = r1 / 2;
          r2 = (3 * r1 + 21) / 9f;

          p1 = initialLot.pointsInEdge[i].Last();
          p2 = p1 + r1 * es[i].direction;
          p3 = p2 + r2 * es[i].right;
          p4 = p1 + r2 * es[i].right;

          initialLot.pointsInEdge[i].Add(p2);
          initialLot.occupied[i] += r1;
          finalLots.Add(new BuildingLot(p1, p2, p3, p4));
        }
        else if (19f <= r1 && r1 <= 25f)
        {
          r1 -= 7f;
          r2 = (3 * r1 + 21) / 9f;
          
          p1 = initialLot.pointsInEdge[i].Last();
          p2 = p1 + r1 * es[i].direction;
          p3 = p2 + r2 * es[(i + 1) % 4].direction;
          p4 = p1 + r2 * es[i].right;

          initialLot.pointsInEdge[i].Add(p2);
          initialLot.occupied[i] += r1;
          finalLots.Add(new BuildingLot(p1, p2, p3, p4));
        }
        else if (11f <= r1 && r1 <= 19f)
        {
          r2 = (3 * r1 + 21) / 9f;

          p1 = initialLot.pointsInEdge[i].Last();
          p2 = es[i].end;
          p3 = es[i].end + r2 * es[(i + 1) % 4].direction;
          p4 = Util.IntersectionPoint(p1, es[i].right, p3, es[i].direction);

          initialLot.pointsInEdge[(i + 1) % 4].Add(p3);
          initialLot.occupied[(i + 1) % 4] += r2;
          initialLot.occupied[i] += r1;
          finalLots.Add(new BuildingLot(p1, p2, p3, p4));
        }
        else
          break;
      }
    }

    for (int i = 1; i < 4; i+=2)
    {
      if (initialLot.pointsInEdge[i].Count == 2)
      {
        if (Vector3.Distance(es[i].start, initialLot.pointsInEdge[i][0]) >
            Vector3.Distance(es[i].start, initialLot.pointsInEdge[i][1]))
          initialLot.pointsInEdge[i].Reverse();

        r1 = Vector3.Distance(initialLot.pointsInEdge[i][0], initialLot.pointsInEdge[i][1]);
        if (r1 <= 12f)
          r2 = r1;
        else
          r2 = (3 * r1 + 21) / 9f;

        p1 = initialLot.pointsInEdge[i][0];
        p2 = initialLot.pointsInEdge[i][1];
        p3 = p2 + r2 * es[(i + 1) % 4].direction;
        p4 = p1 - r2 * es[(i + 3) % 4].direction;

        finalLots.Add(new BuildingLot(p1, p2, p3, p4));
        continue;
      }

      if (initialLot.pointsInEdge[i].Count == 1)
      {
        var dist = Vector3.Distance(es[i].start, initialLot.pointsInEdge[i][0]);

        if (dist < 22f)
        {
          p1 = es[i].start;
          p2 = initialLot.pointsInEdge[i][0];
          p4 = initialLot.pointsInEdge[(i + 3) % 4].Last();
          p3 = Util.IntersectionPoint(p4, es[i].direction,
                                      p2, es[(i + 1) % 4].direction);
          finalLots.Add(new BuildingLot(p1, p2, p3, p4));
        }
        else
        {
          r1 = dist / 2;

          p1 = es[i].start;
          p2 = p1 + r1 * es[i].direction;
          p4 = initialLot.pointsInEdge[(i + 3) % 4].Last();
          p3 = Util.IntersectionPoint(p4, es[(i + 0) % 4].direction,
                                      p2, es[i].right);
          finalLots.Add(new BuildingLot(p1, p2, p3, p4));
          
          r2 = (3 * r1 + 21) / 9f;
          p1 = p2;
          p2 = initialLot.pointsInEdge[i][0];
          p3 = p2 + r2 * es[(i + 1) % 4].direction;
          p4 = p1 + r2 * es[i].right;
          finalLots.Add(new BuildingLot(p1, p2, p3, p4));
        }
      }
    }
  }

  private void FindSidewalkNLotVerts ()
  {
    sidewalkVerts = new Vector3[4];
    lotVerts = new Vector3[4];
    float angle, dist, sin;
    Vector3 dir;

    for (int i = 0; i < 4; ++i)
    {
      angle = Vector3.Angle(edges[i].direction, -edges[(i + 3) % 4].direction);
      sin = Mathf.Sin(angle / 2 * Mathf.Deg2Rad);
      dir = (edges[i].direction - edges[(i + 3) % 4].direction).normalized;

      dist = roadWidth / sin;
      sidewalkVerts[i] = edges[i].start + dist * dir;

      dist = (roadWidth + sidewalkWidth) / sin;
      lotVerts[i] = edges[i].start + dist * dir;
    }
  }
}

} // namespace Thesis
using System.Collections.Generic;
using UnityEngine;

namespace Thesis {

public sealed class CityMapManager
{
  private const float _tolerance = 5f;

  private static readonly CityMapManager _instance = new CityMapManager();
  public static CityMapManager Instance
  {
    get { return _instance; }
  }

  private List<Block> _blocks;
  public IList<Block> blocks
  {
    get { return _blocks.AsReadOnly(); }
  }

  private List<BuildingLot> _lots;
  public IList<BuildingLot> lots
  {
    get { return _lots.AsReadOnly(); }
  }

  private List<Vector3> _nodes;
  public IList<Vector3> nodes
  {
    get { return _nodes.AsReadOnly(); }
  }

  private List<Road> _roads;
  public IList<Road> roads
  {
    get { return _roads; }
  }

  private List<Sidewalk> _sidewalks;
  public IList<Sidewalk> sidewalks
  {
    get { return _sidewalks; }
  }

  private List<BuildingLot> _drawableLots;
  public IList<BuildingLot> drawableLots
  {
    get { return _drawableLots; }
  }

  private CityMapManager ()
  {
    _blocks = new List<Block>();
    _nodes = new List<Vector3>();
    _lots = new List<BuildingLot>();
    _roads = new List<Road>();
    _sidewalks = new List<Sidewalk>();
    _drawableLots = new List<BuildingLot>();
  }

  public void Add (Block block)
  {
    _blocks.Add(block);
  }

  public void Add (BuildingLot lot)
  {
    _lots.Add(lot);
  }

  public int Add (Vector3 node)
  {
    var n = _nodes.FindIndex(0, delegate (Vector3 v) {
      return (Mathf.Abs(v.x - node.x) <= _tolerance &&
              Mathf.Abs(v.z - node.z) <= _tolerance);
    });
    if (n == -1)
      _nodes.Add(node);
    return n;
  }

  public void AddRoad (Block block)
  {
    Road r = new Road(block);
    r.name = "road";
    r.material = MaterialManager.Instance.Get("mat_road");
    _roads.Add(r);
  }

  public void DrawRoads ()
  {
    foreach (Road r in _roads)
    {
      r.FindVertices();
      r.FindTriangles();
      r.Draw();
      r.gameObject.active = true;
    }
  }

  public void DestroyRoads ()
  {
    foreach (Road r in _roads)
      r.Destroy();
  }

  public void AddSidewalk (Block block)
  {
    Sidewalk s = new Sidewalk(block);
    s.name = "sidewalk";
    s.material = MaterialManager.Instance.Get("mat_sidewalk");
    _sidewalks.Add(s);
  }

  public void DrawSidewalks ()
  {
    foreach (Sidewalk s in _sidewalks)
    {
      s.FindVertices();
      s.FindTriangles();
      s.Draw();
      s.gameObject.active = true;
    }
  }

  public void DestroySidewalks ()
  {
    foreach (Sidewalk s in _sidewalks)
      s.Destroy();
  }

  public void AddDrawableLot (BuildingLot lot)
  {
    lot.name = "buildinglot";
    lot.material = MaterialManager.Instance.Get("mat_lot");
    _drawableLots.Add(lot);
  }

  public void DrawLots ()
  {
    foreach (BuildingLot lot in _drawableLots)
    {
      lot.FindVertices();
      lot.FindTriangles();
      lot.Draw();
      lot.gameObject.active = true;
    }
  }

  public void DestroyLots ()
  {
    foreach (BuildingLot lot in _drawableLots)
      lot.Destroy();
  }

  public void Clear ()
  {
    _blocks.Clear();
    _lots.Clear();
    _nodes.Clear();
    DestroyRoads();
    _roads.Clear();
    DestroySidewalks();
    _sidewalks.Clear();
    DestroyLots();
    _drawableLots.Clear();
  }
}

} // namespace Thesis
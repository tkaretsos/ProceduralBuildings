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

  private List<Vector3> _nodes;
  public IList<Vector3> nodes
  {
    get { return _nodes.AsReadOnly(); }
  }

  private CityMapManager ()
  {
    _blocks = new List<Block>();
    _nodes = new List<Vector3>();
  }

  public void Add (Block block)
  {
    _blocks.Add(block);
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
}

} // namespace Thesis
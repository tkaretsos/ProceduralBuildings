using UnityEngine;
using System.Collections.Generic;

using CombinablesCollection = System.Collections.Generic.IList<Thesis.Interface.ICombinable>;

namespace Thesis {

public class Building
{
  public readonly Material windowMaterial;

  public readonly Material balconyDoorMaterial;

  public readonly Material doorMaterial;

  public readonly Material shutterMaterial;

  public readonly Material compDecorMaterial;

  public GameObject gameObject;

  public BuildingMesh buildingMesh;

  private Dictionary<string, CombinablesCollection> _combinables;

  private Dictionary<string, GameObject> _combiners;

  public Building (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
  {
    _combinables = new Dictionary<string, CombinablesCollection>();
    _combiners = new Dictionary<string, GameObject>();

    // find window material randomly
    var list = MaterialManager.Instance.GetCollection("mat_neo_window");
    var num = Random.Range(0, list.Count);
    windowMaterial = list[num];

    // balcony door material
    list = MaterialManager.Instance.GetCollection("mat_neo_balcony_door");
    balconyDoorMaterial = list[num];

    // door material
    list = MaterialManager.Instance.GetCollection("mat_neo_door");
    num = Random.Range(0, list.Count);
    doorMaterial = list[num];

    // shutter material
    // not randomly selected, depends on the door material
    // count and shutter material count, in order to match
    // colours
    list = MaterialManager.Instance.GetCollection("mat_neo_shutter");
    var doorCount = MaterialManager.Instance.GetCollection("mat_neo_door").Count;
    var shutCount = MaterialManager.Instance.GetCollection("mat_neo_shutter").Count;
    shutterMaterial = list[num * shutCount / doorCount];

    // component decor material
    list = MaterialManager.Instance.GetCollection("mat_comp_decor");
    num = Random.Range(0, list.Count);
    compDecorMaterial = list[num];
    //compDecorMaterial = list[list.Count - 1];

    // must be _after_ the initialization of this object
    buildingMesh = new BuildingMesh(this, p1, p2, p3, p4);
    gameObject = new GameObject("Neoclassical");
    gameObject.transform.position = buildingMesh.meshOrigin;
  }

  public void AddCombinable(string materialName, Interface.ICombinable combinable)
  {
    if (!_combinables.ContainsKey(materialName))
    {
      CombinablesCollection temp = new List<Interface.ICombinable>();
      temp.Add(combinable);
      _combinables.Add(materialName, temp);
    }
    else
      _combinables[materialName].Add(combinable);
  }

  public void CombineSubmeshes ()
  {
    foreach (string key in _combinables.Keys)
    {
      if (_combinables[key].Count < 2) continue;

      var gobject = new GameObject(key + "_combiner");
      gobject.active = false;
      gobject.transform.parent = gameObject.transform;
      var filter = gobject.AddComponent<MeshFilter>();
      var renderer = gobject.AddComponent<MeshRenderer>();
      renderer.sharedMaterial = _combinables[key][0].material;

      var filters = new MeshFilter[_combinables[key].Count];
      for (var i = 0; i < _combinables[key].Count; ++i)
      {
        filters[i] = _combinables[key][i].meshFilter;
        GameObject.Destroy(_combinables[key][i].mesh);
        GameObject.Destroy(_combinables[key][i].gameObject);
      }

      var combine = new CombineInstance[filters.Length];
      for (var i = 0; i < filters.Length; ++i)
      {
        combine[i].mesh = filters[i].sharedMesh;
        combine[i].transform = filters[i].transform.localToWorldMatrix;
      }

      //filter.mesh = new Mesh();
      filter.mesh.CombineMeshes(combine);

      if (_combiners.ContainsKey(key))
        _combiners[key] = gobject;
      else
        _combiners.Add(key, gobject);
    }
    _combinables.Clear();
  }

  public void Destroy ()
  {
    GameObject.Destroy(gameObject);

    foreach (string key in _combinables.Keys)
      for (var i = 0; i < _combinables[key].Count; ++i)
      {
        GameObject.Destroy(_combinables[key][i].mesh);
        GameObject.Destroy(_combinables[key][i].gameObject);
      }

    foreach (GameObject go in _combiners.Values)
    {
      GameObject.Destroy(go.GetComponent<MeshFilter>().mesh);
      GameObject.Destroy(go);
    }

    buildingMesh.Destroy();
  }
}

} // namespace Thesis
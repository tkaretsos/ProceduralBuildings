using UnityEngine;
using System.Collections.Generic;

using CombinablesCollection = System.Collections.Generic.IList<Thesis.Interface.ICombinable>;

namespace Thesis {

public class Neoclassical
{
  /*************** FIELDS ***************/
  
  public GameObject gameObject;

  public NeoBuildingMesh buildingMesh;

  private Dictionary<string, CombinablesCollection> _combinables;

  /*************** CONSTRUCTORS ***************/

  public Neoclassical (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
  {
    _combinables = new Dictionary<string, CombinablesCollection>();
    buildingMesh = new NeoBuildingMesh(this, p1, p2, p3, p4);
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
}

} // namespace Thesis
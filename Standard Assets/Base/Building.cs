using UnityEngine;
using System.Collections.Generic;

using CombinablesCollection = System.Collections.Generic.IList<Thesis.Interface.ICombinable>;

namespace Thesis {
namespace Base {

public class Building
{
  public GameObject gameObject;

  public BuildingMesh buildingMesh;

  private Dictionary<string, CombinablesCollection> _combinables;

  public Building ()
  {
    _combinables = new Dictionary<string, CombinablesCollection>();
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

} // namespace Base
} // namespace Thesis
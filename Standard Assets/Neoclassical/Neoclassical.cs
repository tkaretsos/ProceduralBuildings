using UnityEngine;

namespace Thesis {

public class Neoclassical : Building
{
  public readonly Material windowMaterial;

  public readonly Material balconyDoorMaterial;

  public readonly Material doorMaterial;

  public readonly Material shutterMaterial;

  /*************** CONSTRUCTORS ***************/

  public Neoclassical (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    : base()
  {
    // find window material randomly
    var list = MaterialManager.Instance.GetCollection("mat_neo_window");
    var num = Random.Range(0, list.Count - 1);
    windowMaterial = list[num];

    // balcony door material
    list = MaterialManager.Instance.GetCollection("mat_neo_balcony_door");
    balconyDoorMaterial = list[num];

    // door material
    list = MaterialManager.Instance.GetCollection("mat_neo_door");
    num = Random.Range(0, list.Count - 1);
    doorMaterial = list[num];

    // shutter material
    // not randomly selected, depends on the door material
    // count and shutter material count, in order to match
    // colours
    list = MaterialManager.Instance.GetCollection("mat_neo_shutter");
    var doorCount = MaterialManager.Instance.GetCollection("mat_neo_door").Count;
    var shutCount = MaterialManager.Instance.GetCollection("mat_neo_shutter").Count;
    shutterMaterial = list[num * shutCount / doorCount];

    // must be _after_ the initialization of this object
    buildingMesh = new BuildingMesh(this, p1, p2, p3, p4);
    gameObject = new GameObject("Neoclassical");
    gameObject.transform.position = buildingMesh.meshOrigin;
  }
}

} // namespace Thesis
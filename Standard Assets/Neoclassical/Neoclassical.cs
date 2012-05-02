using UnityEngine;

namespace Thesis {

public class Neoclassical : Base.Building
{
  public readonly Material windowMaterial;

  /*************** CONSTRUCTORS ***************/

  public Neoclassical (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    : base()
  {
    // find window material randomly
    var list = MaterialManager.Instance.GetCollection("mat_neo_window");
    windowMaterial = list[Random.Range(0, list.Count - 1)];

    // must be _after_ the initialization of this object
    buildingMesh = new NeoBuildingMesh(this, p1, p2, p3, p4);
    gameObject = new GameObject("Neoclassical");
    gameObject.transform.position = buildingMesh.meshOrigin;
  }
}

} // namespace Thesis
using UnityEngine;

namespace Thesis {

public class Neoclassical : Base.Building
{
  /*************** CONSTRUCTORS ***************/

  public Neoclassical (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    : base()
  {
    buildingMesh = new NeoBuildingMesh(this, p1, p2, p3, p4);
    gameObject = new GameObject("Neoclassical");
    gameObject.transform.position = buildingMesh.meshOrigin;

    MaterialManager.Instance.Add("mat_neo_balcony_rail", "Transparent/Cutout/Diffuse",
                                 TextureManager.Instance.Get("tex_neo_balcony"));
  }
}

} // namespace Thesis
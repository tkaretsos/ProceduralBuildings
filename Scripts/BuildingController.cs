using Thesis;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
  void Update ()
  {
    if (Input.GetKeyUp(KeyCode.Alpha2))
    {
      BuildingManager.Instance.DestroyBuildings();
      BuildingManager.Instance.CreateNeoclassical(BuildMode.Two);
    }

    if (Input.GetKeyUp(KeyCode.Alpha3))
    {
      BuildingManager.Instance.DestroyBuildings();
      BuildingManager.Instance.CreateNeoclassical(BuildMode.Three);
    }

    if (Input.GetKeyUp(KeyCode.Alpha4))
    {
      BuildingManager.Instance.DestroyBuildings();
      BuildingManager.Instance.CreateNeoclassical(BuildMode.Four);
    }

    if (Input.GetKeyUp(KeyCode.Alpha5))
    {
      BuildingManager.Instance.DestroyBuildings();
      BuildingManager.Instance.CreateNeoclassical(BuildMode.Five);
    }

#if UNITY_EDITOR
    foreach (Building b in BuildingManager.Instance.buildings)
    {
      Debug.DrawRay(b.buildingMesh.boundaries[0] + b.buildingMesh.meshOrigin, 
                    Vector3.up * 20, Color.green);
    }
#endif
  }
}

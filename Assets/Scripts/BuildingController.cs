using Thesis;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
  void Update ()
  {
    if (Input.GetKeyUp(KeyCode.F5))
    {
      BuildingManager.Instance.DestroyBuildings();
      BuildingManager.Instance.CreateNeoclassical(BuildMode.Two);
    }

    if (Input.GetKeyUp(KeyCode.F6))
    {
      BuildingManager.Instance.DestroyBuildings();
      BuildingManager.Instance.CreateNeoclassical(BuildMode.Three);
    }

    if (Input.GetKeyUp(KeyCode.F7))
    {
      BuildingManager.Instance.DestroyBuildings();
      BuildingManager.Instance.CreateNeoclassical(BuildMode.Four);
    }

    if (Input.GetKeyUp(KeyCode.F8))
    {
      BuildingManager.Instance.DestroyBuildings();
      BuildingManager.Instance.CreateNeoclassical(BuildMode.Five);
    }
  }
}

using Thesis;
using UnityEngine;
using System.Collections.Generic;

public class BuildingController : MonoBehaviour
{
  void Start ()
  {
    TextureManager.Instance.Init();
    MaterialManager.Instance.Init();
    NeoManager.Instance.Init();
    NeoManager.Instance.CreateNeoclassical(BuildMode.Three);
  }

  void Update ()
  {
    if (Input.GetKeyUp(KeyCode.Alpha1))
    {
      NeoManager.Instance.DestroyBuildings();
      NeoManager.Instance.CreateNeoclassical(BuildMode.Many);
    }

    if (Input.GetKeyUp(KeyCode.Alpha2))
    {
      NeoManager.Instance.DestroyBuildings();
      NeoManager.Instance.CreateNeoclassical(BuildMode.Two);
    }

    if (Input.GetKeyUp(KeyCode.Alpha3))
    {
      NeoManager.Instance.DestroyBuildings();
      NeoManager.Instance.CreateNeoclassical(BuildMode.Three);
    }

    if (Input.GetKeyUp(KeyCode.Alpha4))
    {
      NeoManager.Instance.DestroyBuildings();
      NeoManager.Instance.CreateNeoclassical(BuildMode.Four);
    }

    if (Input.GetKeyUp(KeyCode.Alpha5))
    {
      NeoManager.Instance.DestroyBuildings();
      NeoManager.Instance.CreateNeoclassical(BuildMode.Five);
    }
  }
}

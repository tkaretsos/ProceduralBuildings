using Thesis;
using UnityEngine;
using System.Collections.Generic;

public class BuildingController : MonoBehaviour
{
  private NeoclassicalManager manager = new NeoclassicalManager();

	void Start ()
  {
	  manager.CreateNeoclassical(BuildMode.Three);
	}

	void Update ()
  {
    if (Input.GetKeyUp(KeyCode.Alpha1))
    {
      manager.DestroyBuildings();
      //StartCoroutine("manager.CreateNeoclassical", BuildMode.Many);
      manager.CreateNeoclassical(BuildMode.Many);
    }

    if (Input.GetKeyUp(KeyCode.Alpha2))
    {
      manager.DestroyBuildings();
      manager.CreateNeoclassical(BuildMode.Two);
    }

    if (Input.GetKeyUp(KeyCode.Alpha3))
    {
      manager.DestroyBuildings();
      manager.CreateNeoclassical(BuildMode.Three);
    }

    if (Input.GetKeyUp(KeyCode.Alpha4))
    {
      manager.DestroyBuildings();
      manager.CreateNeoclassical(BuildMode.Four);
    }

    if (Input.GetKeyUp(KeyCode.Alpha5))
    {
      manager.DestroyBuildings();
      manager.CreateNeoclassical(BuildMode.Five);
    }
	}
}

using Thesis;
using UnityEngine;
using System.Collections.Generic;

public class BuildingController : MonoBehaviour
{
	void Start ()
  {
    MaterialManager.Instance.Init();
	  NeoclassicalManager.Instance.CreateNeoclassical(BuildMode.Three);
	}

	void Update ()
  {
    if (Input.GetKeyUp(KeyCode.Alpha1))
    {
      NeoclassicalManager.Instance.DestroyBuildings();
      //StartCoroutine("NeoclassicalManager.Instance.CreateNeoclassical", BuildMode.Many);
      NeoclassicalManager.Instance.CreateNeoclassical(BuildMode.Many);
    }

    if (Input.GetKeyUp(KeyCode.Alpha2))
    {
      NeoclassicalManager.Instance.DestroyBuildings();
      NeoclassicalManager.Instance.CreateNeoclassical(BuildMode.Two);
    }

    if (Input.GetKeyUp(KeyCode.Alpha3))
    {
      NeoclassicalManager.Instance.DestroyBuildings();
      NeoclassicalManager.Instance.CreateNeoclassical(BuildMode.Three);
    }

    if (Input.GetKeyUp(KeyCode.Alpha4))
    {
      NeoclassicalManager.Instance.DestroyBuildings();
      NeoclassicalManager.Instance.CreateNeoclassical(BuildMode.Four);
    }

    if (Input.GetKeyUp(KeyCode.Alpha5))
    {
      NeoclassicalManager.Instance.DestroyBuildings();
      NeoclassicalManager.Instance.CreateNeoclassical(BuildMode.Five);
    }
	}
}

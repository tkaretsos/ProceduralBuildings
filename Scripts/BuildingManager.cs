using UnityEngine;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour
{
  private enum BuildingMode
  {
    Many,
    Big,
    Small
  }

  private List<Neoclassical> neo = new List<Neoclassical>();

  private GameObject _gameObject;

	void Start ()
  {
	  CreateNeoclassical();
    //neo[0].Render();
	}
	
	void Update ()
  {
    if (Input.GetKeyUp(KeyCode.Alpha1))
    {
      DestroyBuildings();
      CreateNeoclassical(BuildingMode.Many);

      //if (_gameObject != null) 
        Destroy(_gameObject);

      _gameObject = new GameObject("City");
      MeshFilter meshFilter = _gameObject.AddComponent<MeshFilter>();
      MeshRenderer meshRenderer = _gameObject.AddComponent<MeshRenderer>();
      meshRenderer.sharedMaterial = neo[0].material;

      MeshFilter[] meshFilters = new MeshFilter[neo.Count];
      for (var i = 0; i < neo.Count; ++i)
        meshFilters[i] = neo[i].gameObject.GetComponent<MeshFilter>();

      CombineInstance[] combine = new CombineInstance[meshFilters.Length];
      var j = 0;

      while (j < meshFilters.Length)
      {
        combine[j].mesh = meshFilters[j].sharedMesh;
        combine[j].transform = meshFilters[j].transform.localToWorldMatrix;
        meshFilters[j].gameObject.active = false;
        j++;
      }

      meshFilter.mesh = new Mesh();
      meshFilter.mesh.CombineMeshes(combine); 
      _gameObject.transform.gameObject.active = true;
    }

    if (Input.GetKeyUp(KeyCode.Alpha2))
    {
      DestroyBuildings();
      CreateNeoclassical(BuildingMode.Big);
      //neo[0].Render();
    }

    if (Input.GetKeyUp(KeyCode.Alpha3))
    {
      DestroyBuildings();
      CreateNeoclassical(BuildingMode.Small);
      //neo[0].Render();
    }

    //foreach (Neoclassical n in neo)
    //  Debug.DrawRay(n.gameObject.transform.position, Vector3.up * 100, Color.green);
	}

  private void CreateNeoclassical(BuildingMode mode = BuildingMode.Small)
  {
    switch (mode)
    {
      case BuildingMode.Many:
        for (int i = 0; i < 25; ++i)
        {
          float x_mod = i * 15f;
          for (int j = 0; j < 25; ++j)
          {
            float z_mod = j * 9f;
            neo.Add(new Neoclassical(
              new Vector3(x_mod + Random.Range(0.5f, 1.5f) + 9f, 0f, z_mod + Random.Range(0.5f, 1.5f) + 3.5f),
              new Vector3(x_mod + Random.Range(0.5f, 1.5f) + 9f, 0f, z_mod - Random.Range(0.5f, 1.5f)), 
              new Vector3(x_mod - Random.Range(0.5f, 1.5f)     , 0f, z_mod - Random.Range(0.5f, 1.5f)), 
              new Vector3(x_mod - Random.Range(0.5f, 1.5f)     , 0f, z_mod + Random.Range(0.5f, 1.5f) + 3.5f)));
          }
        }
        break;

      case BuildingMode.Big:
        neo.Add(new Neoclassical(
          new Vector3(Random.Range(0.5f, 1.5f) + 20f, 0f,  Random.Range(0.5f, 1.5f) + 8f), 
          new Vector3(Random.Range(0.5f, 1.5f) + 20f, 0f,  Random.Range(0.5f, 1.5f)), 
          new Vector3(Random.Range(0.5f, 1.5f)      , 0f, -Random.Range(0.5f, 1.5f)), 
          new Vector3(Random.Range(0.5f, 1.5f)      , 0f,  Random.Range(0.5f, 1.5f) + 8f)));
        break;

      case BuildingMode.Small:
        neo.Add(new Neoclassical(
          new Vector3(Random.Range(0.25f, 0.75f) + 9f, 0f,  Random.Range(0.25f, 0.75f) + 3.5f), 
          new Vector3(Random.Range(0.25f, 0.75f) + 9f, 0f,  Random.Range(0.25f, 0.75f)), 
          new Vector3(Random.Range(0.25f, 0.75f)     , 0f, -Random.Range(0.25f, 0.75f)), 
          new Vector3(Random.Range(0.25f, 0.75f)     , 0f,  Random.Range(0.25f, 0.75f) + 3.5f)));
        break;
    }
  }

  public void DestroyBuildings ()
  {
    foreach (Neoclassical n in neo)
      Destroy(n.gameObject);
    neo.Clear();
  }
}

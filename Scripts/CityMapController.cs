using UnityEngine;
using Thesis;
using System.Linq;

public class CityMapController : MonoBehaviour {

  Block block;

  void Awake ()
  {
    ColorManager.Instance.Init();
    TextureManager.Instance.Init();
    MaterialManager.Instance.Init();

    // the point Vector3.zero must _not_ be used
    // as starting point and all 4 points must be
    // in the first quadrant
    block = new Block(new Vector3(1f, 0f, 0f),
                      new Vector3(0f, 0f, 300f),
                      new Vector3(500f, 0f, 300f),
                      new Vector3(500f, 0f, 0f));
    block.Bisect();

    //AddBuildings();
  }

  void Update ()
  {
    //foreach (Block b in CityMapManager.Instance.blocks)
    //  foreach (Edge e in b.lot.edges)
    //    Debug.DrawLine(e.start, e.end, Color.green);

    foreach (BuildingLot b in CityMapManager.Instance.lots)
      foreach (Edge e in b.edges)
        Debug.DrawLine(e.start, e.end, Color.cyan);

    if (Input.GetKeyUp(KeyCode.B))
      AddBuildings();
  }

  private void AddBuildings ()
  {
    Building building;

    foreach (BuildingLot l in CityMapManager.Instance.lots)
    {
      building = new Building(l.edges[0].start,
                              l.edges[1].start,
                              l.edges[2].start,
                              l.edges[3].start);
      building.buildingMesh.FindVertices();
      building.buildingMesh.FindTriangles();
      building.buildingMesh.Draw();
      building.CombineSubmeshes();
      building.gameObject.SetActiveRecursively(true);
    }
  }
}
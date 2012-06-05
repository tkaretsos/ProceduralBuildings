using UnityEngine;
using Thesis;

public class CityMapController : MonoBehaviour {

  Block block;

  void Awake ()
  {
    // the point Vector3.zero must _not_ be used
    // as starting point and all 4 points must be
    // in the first quadrant
    block = new Block(new Vector3(1f, 0f, 0f),
                      new Vector3(0f, 0f, 300f),
                      new Vector3(700f, 0f, 300f),
                      new Vector3(700f, 0f, 0f));
    block.Bisect();
  }
	
  void Update ()
  {
    foreach (Block b in CityMapManager.Instance.blocks)
      foreach (Edge e in b.lot.edges)
        Debug.DrawLine(e.start, e.end, Color.green);
  }
}

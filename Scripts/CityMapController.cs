using UnityEngine;
using Thesis;
using System.Linq;

public class CityMapController : MonoBehaviour {

  private Block block;

  private bool _drawGizmos = false;
  private bool _drawFirstBlock = false;
  private bool _drawBlocks = false;
  private bool _drawSideWalk = false;
  private bool _drawFirstLot = false;
  private bool _drawLots = false;

  void Start ()
  {
    // the point Vector3.zero must _not_ be used
    // as starting point and all 4 points must be
    // in the first quadrant
    block = new Block();
    block.Bisect();
  }

  void Update ()
  {
    if (Input.GetKeyUp(KeyCode.B))
    {
      AddBuildings();
      foreach (Block b in CityMapManager.Instance.blocks)
        b.Draw();
    }

    if (Input.GetKeyUp(KeyCode.G))
      _drawGizmos = !_drawGizmos;

    if (Input.GetKeyUp(KeyCode.Alpha1))
      _drawFirstBlock = !_drawFirstBlock;

    if (Input.GetKeyUp(KeyCode.Alpha2))
      _drawBlocks = !_drawBlocks;

    if (Input.GetKeyUp(KeyCode.Alpha3))
      _drawSideWalk = !_drawSideWalk;

    if (Input.GetKeyUp(KeyCode.Alpha4))
      _drawFirstLot = !_drawFirstLot;

    if (Input.GetKeyUp(KeyCode.Alpha5))
      _drawLots = !_drawLots;

    if (Input.GetKeyUp(KeyCode.Alpha0))
    {
      _drawFirstBlock = false;
      _drawBlocks = false;
      _drawSideWalk = false;
      _drawFirstLot = false;
      _drawLots = false;
    }
  }

  private void AddBuildings ()
  {
    foreach (Block b in CityMapManager.Instance.blocks)
      foreach (BuildingLot l in b.finalLots)
        BuildingManager.Instance.Build(l);
  }

  void OnPostRender ()
  {
    if (_drawGizmos)
    {
      if (_drawFirstBlock)
      {
        MaterialManager.Instance.Get("line_block").SetPass(0);
        GL.Begin(GL.LINES);
        foreach (Edge e in block.edges)
        {
          GL.Vertex(e.start);
          GL.Vertex(e.end);
        }
        GL.End();
      }

      if (_drawBlocks)
      {
        MaterialManager.Instance.Get("line_block").SetPass(0);
        GL.Begin(GL.LINES);
        foreach (Block b in CityMapManager.Instance.blocks)
          foreach (Edge e in b.edges)
          {
            GL.Vertex(e.start);
            GL.Vertex(e.end);
          }
        GL.End();
      }

      if (_drawSideWalk)
      {
        MaterialManager.Instance.Get("line_sidewalk").SetPass(0);
        GL.Begin(GL.LINES);
        foreach (Block b in CityMapManager.Instance.blocks)
          foreach (Edge e in b.sidewalk.edges)
          {
            GL.Vertex(e.start);
            GL.Vertex(e.end);
          }
        GL.End();
      }

      if (_drawFirstLot)
      {
        MaterialManager.Instance.Get("line_lot").SetPass(0);
        GL.Begin(GL.LINES);
        foreach (Block b in CityMapManager.Instance.blocks)
          foreach (Edge e in b.initialLot.edges)
          {
            GL.Vertex(e.start);
            GL.Vertex(e.end);
          }
        GL.End();
      }

      if (_drawLots)
      {
        MaterialManager.Instance.Get("line_lot").SetPass(0);
        GL.Begin(GL.LINES);
        foreach (Block b in CityMapManager.Instance.blocks)
          foreach (BuildingLot l in b.finalLots)
            foreach (Edge e in l.edges)
            {
              GL.Vertex(e.start);
              GL.Vertex(e.end);
            }
        GL.End();
      }
    }
  }
}
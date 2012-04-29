using UnityEngine;
using System.Collections.Generic;

namespace Thesis {
namespace Base {

public class ProceduralTexture
{
  public Texture2D content;

  public List<TextureLine> lines = new List<TextureLine>();

  public virtual void CalculateLines () { }

  public virtual void Draw ()
  {
    CalculateLines();

    foreach (TextureLine line in lines)
      DrawLine(line.start, line.end, line.color, line.thickness);

    content.Apply();
  }

  public void DrawLine (Vector2 p1, Vector2 p2, Color color, int thickness)
  {
    DrawLine(p1, p2, color);
    if (Mathf.Abs(p1.x - p2.x) > Mathf.Abs(p1.y - p2.y))
      for (var tk = 1; tk < Mathf.CeilToInt((1f * thickness) / 2); ++tk)
      {
        DrawLine(p1 + tk * Vector2.up, p2 + tk * Vector2.up, color);
        DrawLine(p1 - tk * Vector2.up, p2 - tk * Vector2.up, color);
      }
    else
      for (var tk = 1; tk < Mathf.CeilToInt((1f * thickness) / 2); ++tk)
      {
        DrawLine(p1 + tk * Vector2.right, p2 + tk * Vector2.right, color);
        DrawLine(p1 - tk * Vector2.right, p2 - tk * Vector2.right, color);
      }
  }


  /// <summary>
  /// Implementation of Besenham's line algorithm:
  /// http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm#Simplification
  /// </summary>
  public void DrawLine (Vector2 p1, Vector2 p2, Color color)
  {
    int dx = Mathf.Abs((int) (p1.x - p2.x));
    int dy = Mathf.Abs((int) (p1.y - p2.y));
    int sx = (p1.x < p2.x) ? 1 : -1;
    int sy = (p1.y < p2.y) ? 1 : -1;
    int err = dx - dy;
    int err2;

    while (true)
    {
      content.SetPixel((int) p1.x, (int) p1.y, color);
      if (p1.x == p2.x && p1.y == p2.y) break;
      err2 = 2 * err;
      if (err2 > -dy)
      {
        err -= dy;
        p1.x += sx;
      }

      if (err2 < dx)
      {
        err += dx;
        p1.y += sy;
      }
    }
  }
}

} // namespace Base
} // namespace Thesis
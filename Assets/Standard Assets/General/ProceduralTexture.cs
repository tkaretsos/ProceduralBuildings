using UnityEngine;
using System.Collections.Generic;

namespace Thesis {

public class ProceduralTexture
{
  public Texture2D content;

  public List<TextureLine> lines;

  public ProceduralTexture (int width = 256, int height = 128)
  {
    content = new Texture2D(width, height);
    lines = new List<TextureLine>();
  }

  public ProceduralTexture (Texture2D texture)
  {
    content = texture;
    lines = new List<TextureLine>();
  }

  public virtual void Draw ()
  {
    foreach (TextureLine line in lines)
      DrawLine(line.start, line.end, line.color, line.thickness);

    content.Apply();
  }

  public void Clear ()
  {
    for (var x = 0; x < content.width; ++x)
      for (var y = 0; y < content.height; ++y)
        content.SetPixel(x, y, Color.clear);
  }

  public void SetBackgroundColor (Color color)
  {
    for (var x = 0; x < content.width; ++x)
      for (var y = 0; y < content.height; ++y)
        content.SetPixel(x, y, color);
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
      err2 = err << 1;
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

} // namespace Thesis
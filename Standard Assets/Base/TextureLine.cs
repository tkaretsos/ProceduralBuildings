using UnityEngine;

namespace Thesis {
namespace Base {

public struct TextureLine
{
  public Vector2 start;
  
  public Vector2 end;

  public Color color;
  
  public int thickness;

  public TextureLine (Vector2 start, Vector2 end, Color color, int thickness)
  {
    this.start = start;
    this.end = end;
    this.color = color;
    this.thickness = thickness;
  }

  public TextureLine (int x1, int y1, int x2, int y2, Color color, int thickness)
  {
    this.start = new Vector2(x1, y1);
    this.end = new Vector2(x2, y2);
    this.color = color;
    this.thickness = thickness;
  }
}

} // namespace Base
} // namespace Thesis
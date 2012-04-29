using System.Collections.Generic;
using UnityEngine;
using Thesis.Base;

namespace Thesis {

public class NeoBalconyTexture : ProceduralTexture
{
  /*************** FIELDS ***************/

  public float outBorderSize;
  public float inBorderSize;
  public float spaceBetweenBorders;
  public float ratio;
    
  //private int _vOutBorderWidth;
  //private int _vInBorderWidth;
  //private int _vInBorderOffset;
    
  //private int _hInBorderWidth;
  //private int _topOutBorderWidth;
  //private int _topInBorderOffset;
  //private int _botInBorderOffset;

  private List<TextureLine> lines = new List<TextureLine>();

  /*************** CONSTRUCTORS ***************/

  public NeoBalconyTexture ()
  {
    ratio = 2f;
    content = new Texture2D(1024, 512);
    // clear texture
    for (var x = 0; x < content.width; ++x)
      for (var y = 0; y < content.height; ++y)
        content.SetPixel(x, y, Color.clear);

    outBorderSize = 0.015f;
    inBorderSize = 0.01f;
    spaceBetweenBorders = 0.06f;
  }

  public NeoBalconyTexture (float width, float height)
  {
    ratio = width / height;
    content = new Texture2D(1024, 512);
    // clear texture
    for (var x = 0; x < content.width; ++x)
      for (var y = 0; y < content.height; ++y)
        content.SetPixel(x, y, Color.clear);

    outBorderSize = 0.015f;
    inBorderSize = 0.01f;
    spaceBetweenBorders = 0.06f;
  }

  public void CalculateLines ()
  {
    var _vOutBorderWidth = Mathf.FloorToInt(content.width * outBorderSize);
    var _topOutBorderWidth = Mathf.FloorToInt(content.height * outBorderSize * 2 * ratio);

    var _vInBorderWidth = Mathf.FloorToInt(content.width * inBorderSize);
    var _hInBorderWidth = Mathf.FloorToInt(content.height * inBorderSize * ratio);
    
    var _vInBorderOffset = Mathf.FloorToInt(content.width * spaceBetweenBorders) +
                                        _vOutBorderWidth;
    var _botInBorderOffset = Mathf.FloorToInt(content.height * spaceBetweenBorders * ratio);

    var halfWidth = content.width >> 1;

    int _topOutBorderY    = content.height - (_topOutBorderWidth >> 1);
    int _botInBorderY     = _botInBorderOffset + (_hInBorderWidth >> 1);
    int _topInBorderY     = content.height - _botInBorderY - _topOutBorderWidth;
    int _leftOutBorderX   = (_vOutBorderWidth >> 1) - 1;
    int _rightOutBorderX  = content.width - _leftOutBorderX;
    int _leftInBorderX    = _vInBorderOffset + (_vInBorderWidth >> 1);
    int _rightInBorderX   = content.width - _leftInBorderX;

    // top out border
    lines.Add(new TextureLine(0, _topOutBorderY,
                              content.width, _topOutBorderY,
                              Color.black, _topOutBorderWidth));
    // bot in border
    lines.Add(new TextureLine(0, _botInBorderY,
                              content.width, _botInBorderY,
                              Color.black, _hInBorderWidth));
    // top in border
    lines.Add(new TextureLine(0, _topInBorderY,
                              content.width, _topInBorderY,
                              Color.black, _hInBorderWidth));
    // left out border
    lines.Add(new TextureLine(_leftOutBorderX, 0,
                              _leftOutBorderX, content.height,
                              Color.black, _vOutBorderWidth));
    // right out border
    lines.Add(new TextureLine(_rightOutBorderX, 0,
                              _rightOutBorderX, content.height,
                              Color.black, _vOutBorderWidth));
    // left in border
    lines.Add(new TextureLine(_leftInBorderX, 0,
                              _leftInBorderX, content.width,
                              Color.black, _vInBorderWidth));
    // right in border
    lines.Add(new TextureLine(_rightInBorderX, 0,
                              _rightInBorderX, content.width,
                              Color.black, _vInBorderWidth));
    // middle border
    lines.Add(new TextureLine(halfWidth, 0,
                              halfWidth, content.height,
                              Color.black, _vInBorderWidth));
    // left inner box
    lines.Add(new TextureLine(_leftInBorderX, _botInBorderY + 2,
                              halfWidth + 1, _topInBorderY,
                              Color.black, _vInBorderWidth));

    lines.Add(new TextureLine(_leftInBorderX, _topInBorderY - 1,
                              halfWidth + 1, _botInBorderY,
                              Color.black, _vInBorderWidth));
    // right inner box
    lines.Add(new TextureLine(halfWidth + 1, _botInBorderY,
                              _rightInBorderX, _topInBorderY,
                              Color.black, _vInBorderWidth));

    lines.Add(new TextureLine(halfWidth + 1, _topInBorderY,
                              _rightInBorderX, _botInBorderY,
                              Color.black, _vInBorderWidth));
  }
}

} // namespace Thesis
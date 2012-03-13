using UnityEngine;

namespace Base
{

public class Window : FaceComponent
{
  public WindowFrame windowFrame;

  public WindowGlass windowGlass;

  public float depth;

  public Window (Face parent) : base(parent) 
  {
    depth = 0.2f;
  }
}

}

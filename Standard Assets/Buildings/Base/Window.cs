using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{

public class Window : FaceComponent
{
  public WindowFrame windowFrame;

  public float depth;

  public Window (Face parent) : base(parent) 
  {
    depth = 0.2f;
  }
}

}

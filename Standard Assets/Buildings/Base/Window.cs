namespace Base
{

public class Window : FaceComponent
{
  public WindowFrame windowFrame;

  public Glass windowGlass;

  public Window (Face parent) : base(parent) 
  {
    depth = 0.2f;
  }
}

}

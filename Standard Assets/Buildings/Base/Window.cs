namespace Base
{

public class Window : FaceComponent
{
  public WindowFrame windowFrame;

  public Glass windowGlass;

  public Window (Face parent, int floor) : base(parent, floor) 
  {
    depth = 0.2f;
  }
}

}

using UnityEngine;

public class Neoclassical : BuildingMesh
{
	public Neoclassical (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
	: base(p1, p2, p3, p4, BuildingType.Neoclassical)
	{
		this.FloorHeight = Random.Range(3f, 4f);
		this.FloorNumber = Util.RollDice(new float[] {0.15f, 0.7f, 0.15f});
	}
	
	public override void ConstructFaces ()
	{
		base.ConstructFaces();
	}
}


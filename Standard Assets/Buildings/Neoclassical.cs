using UnityEngine;

public class Neoclassical : BuildingMesh
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Neoclassical"/> class.
	/// The boundaries of the base of this building must be given in 
	/// clockwise order.
	/// </summary>
	/// <param name='p1'>
	/// A point in space.
	/// </param>
	/// <param name='p2'>
	/// A point in space.
	/// </param>
	/// <param name='p3'>
	/// A point in space.
	/// </param>
	/// <param name='p4'>
	/// A point in space.
	/// </param>
	public Neoclassical (Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
	: base(p1, p2, p3, p4, BuildingType.Neoclassical)
	{
		this.FloorHeight = Random.Range(3f, 4f);
		this.FloorNumber = Util.RollDice(new float[] {0.15f, 0.7f, 0.15f});
	}
	
	/// <summary>
	/// Constructs the faces.
	/// </summary>
	public override void ConstructFaces ()
	{
		base.ConstructFaces();
	}
}


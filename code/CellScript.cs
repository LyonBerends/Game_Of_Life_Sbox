using System;
using Sandbox;

public sealed class CellScript : Component
{
	[Property]
	public int x {get; set;}
	[Property]
	public int y {get; set;}
	[Property]
	public bool alive {get; set;}

	public CellScript[] neighbours = new CellScript[8];

	protected override void OnUpdate()
	{

	}

	public void FindNeighbours()
	{
		int i = 0;
		for(int a = -1; a < 2; a++)
		{
			for(int b = -1; b < 2; b++)
			{
				if(!(a == 0 && b == 0))
				{
					if(a + x >= 0 && a + x < 16 && y + b >= 0 && y + b < 16)
					{
						String nameToFind = "Cell";
						if(a + x < 10) {nameToFind += $"0{a+x}";} else {nameToFind += (a + x);}
						if(b + y < 10) {nameToFind += $"0{b+y}";} else {nameToFind += (b + y);}
						GameObject neighbour = Scene.Directory.FindByName(nameToFind).First<GameObject>();
						if(neighbour != null)
						{
							Log.Info($"Found neighbour: {neighbour.Name}");
							neighbours[i] = neighbour.Components.Get<CellScript>();
							i += 1;
						}
					}
					
				}
			}
		}
	}

	public void setTint(String newTint)
	{
		ModelRenderer modelRenderer = Components.Get<ModelRenderer>();
		modelRenderer.Tint = newTint;
	}

	public void makeAlive()
	{
		ModelRenderer modelRenderer = Components.Get<ModelRenderer>();
		modelRenderer.Tint = "#ffffff";
		alive = true;
	}
	public void makeDeath()
	{
		ModelRenderer modelRenderer = Components.Get<ModelRenderer>();
		modelRenderer.Tint = "#000000";
		alive = false;
	}
}
using System;
using Sandbox;

public sealed class InitializeCells : Component
{
	[Property]
	[Category("Components")]
	public GameObject plane {get; set;}
	[Property]
	[Category("Components")]
	public GameObject cell {get; set;}
	protected override void OnUpdate()
	{

	}

	protected override void OnStart()
	{
		for(int x = 0; x < 16; x++)
		{
			for(int y = 0; y < 16; y++)
			{
				if(x + y != 0)
				{
					GameObject new_cell = cell.Clone();
		
					new_cell.Transform.LocalPosition = (cell.Transform.LocalPosition - new Vector3(6.25f*y, 6.25f*x, 0f)) * 10;
					new_cell.Transform.LocalScale = cell.Transform.LocalScale * 10;
					new_cell.SetParent(plane);

					String cell_name = "Cell";
					if(x < 10) {cell_name += $"0{x}";} else {cell_name += x;}
					if(y < 10) {cell_name += $"0{y}";} else {cell_name += y;}
					new_cell.Name = cell_name;

					CellScript cellScript = new_cell.Components.Get<CellScript>();
					cellScript.x = x;
					cellScript.y = y;
				}
			}
		}

		GameObject.Children.ForEach((GameObject child) => {
			CellScript cell = child.Components.Get<CellScript>();
			if(cell != null){cell.FindNeighbours();}
		});
	}
}
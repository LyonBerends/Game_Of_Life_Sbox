using System;
using System.Security.Cryptography;
using Sandbox;

public sealed class InputFunctionality : Component
{
	[Property]
	public bool paused {get; set;}
	[Property]
	public int UpdateAfterThisManyFixedFrames {get; set;}

	int frames = 0;

	protected override void OnUpdate()
	{
		
	}

	protected override void OnFixedUpdate()
	{
		if(paused){return;}
		frames += 1;
		frames = frames % UpdateAfterThisManyFixedFrames;
		if(frames != 0){return;}
		bool[,] nextFrame = new bool[16, 16];
		foreach(GameObject child in GameObject.Children)
		{
			CellScript cell = child.Components.Get<CellScript>();
			if(cell != null)
			{
				int neighbours_count = 0;
				foreach(CellScript neighbour in cell.neighbours)
				{
					if(neighbour != null)
					{
						if(neighbour.alive){neighbours_count += 1;}
					}
				}
				
				if(cell.alive)
				{
					if(neighbours_count == 3 || neighbours_count == 2) {nextFrame[cell.x, cell.y] = true;}
					else {nextFrame[cell.x, cell.y] = false;}
				}
				else
				{
					if(neighbours_count == 3){nextFrame[cell.x, cell.y] = true;}
					else {nextFrame[cell.x, cell.y] = false;}
				}

				// if(neighbours_count > 3)
				// {
					// foreach(CellScript neighbour in cell.neighbours)
					// {
					// 	if(neighbour != null)
					// 	{
					// 		neighbour.makeDeath();
					// 	}
					// }
					//Log.Info($"Cell [{cell.x},{cell.y}] has {neighbours_count} neighbours");
				// }
			}
		}

		for(int x = 0; x < 16; x++)
		{
			for(int y = 0; y < 16; y++)
			{
				String cellName = "Cell";
				if(x < 10){cellName += $"0{x}";} else {cellName += x;}
				if(y < 10){cellName += $"0{y}";} else {cellName += y;}
				
				CellScript cell = Scene.Directory.FindByName(cellName).First().Components.Get<CellScript>();
				if(cell != null)
				{
					if(!nextFrame[x, y])
					{
						if(cell.alive){cell.makeDeath();}
					}
					else
					{
						if(!cell.alive){cell.makeAlive();}
					}
				}
			}
		}
	}
}
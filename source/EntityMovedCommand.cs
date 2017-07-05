using System;
using System.Collections;
using System.IO;
using dwd.core.commands;
using dwd.core.data.providers;
using dwd.core.match;
using dwd.core.match.commands;
using h;
using PrivateImplementationDetails;

public class EntityMovedCommand : Command
{
	[MessageCommandConstructor(IsOverride = true)]
	public EntityMovedCommand(EntityMoved message) : this(message.EntityID, message.DestinationID)
	{
	}

	public EntityMovedCommand(EntityID entityID, EntityID destinationID)
	{
		this.EntityID = entityID;
		this.DestinationID = destinationID;
	}

	protected override IEnumerator execute()
	{
		x.MoveEntity(this.EntityID, this.DestinationID);
		y entities = DataProvider.Get<HydraMatchData>().get_Entities();
		EntityComponent childEntity;
		if (!entities.All.TryGetValue(this.EntityID, out childEntity))
		{
			throw new Exception(Constants.ZY());
		}
		EntityComponent entityComponent;
		if (!entities.All.TryGetValue(this.DestinationID, out entityComponent))
		{
			throw new Exception(Constants.Zy());
		}
		entityComponent.AddChild(childEntity);
		try
		{
			File.WriteAllText("cards_count.txt", DataProvider.Get<HydraMatchData>().get_Entities().player.get_Hand().Children.Count.ToString());
			yield break;
		}
		catch
		{
			yield break;
		}
		yield break;
		yield break;
	}

	public readonly EntityID EntityID;

	public readonly EntityID DestinationID;
}

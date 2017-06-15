using System;
using System.Collections;
using System.IO;
using <PrivateImplementationDetails>{BDBF81AA-BD99-47D6-8D6D-4DF8F6818835};
using cardinal.src.match.effects;
using dwd.core;
using dwd.core.commands;
using dwd.core.data;
using dwd.core.data.composition;
using hydra.match.effects;
using UnityEngine;

namespace cardinal.src.match.commands.misc
{
	public class ShowCardDeath : Command
	{
		public ShowCardDeath(DataComposition card, string effectPrefabName, bool isPlayerOne, Command postDeathCommand = null)
		{
			this.effectPrefabName = effectPrefabName;
			this.card = card;
			this.postDeathCommand = postDeathCommand;
			if (effectPrefabName == "millDeath")
			{
				string contents = string.Concat(new string[]
				{
					isPlayerOne ? "player" : "opponent",
					" played card_destroyed ",
					effectPrefabName,
					" card=",
					card.GetOne<NameData>().get_Name(),
					"\n"
				});
				File.AppendAllText("sent.txt", contents);
			}
			if (effectPrefabName == "millDeath2")
			{
				this.effectPrefabName = "millDeath";
			}
		}

		protected override IEnumerator execute()
		{
			MatchEffectConfig deathPrefab = MatchEffects.GetConfig(this.effectPrefabName);
			float deathDelay = AttackDelays.GetDelay(AttackDelays.DelayType.UnitDeath);
			yield return new WaitForSeconds(deathDelay);
			MatchEffectsArea death = new GameObject(<<EMPTY_NAME>>.ru()).AddComponent<DeathEffectArea>();
			death.Init(deathPrefab, this.card);
			death.Play(null);
			while (!death.get_Completed())
			{
				yield return null;
			}
			if (this.postDeathCommand != null)
			{
				while (this.postDeathCommand.MoveNext())
				{
					yield return this.postDeathCommand.Current;
				}
			}
			Finder.FindOrThrow<CommandExecutor>().Execute(new DestroyEffectOnComplete(death));
			yield break;
		}

		private readonly string effectPrefabName;

		private readonly DataComposition card;

		private readonly Command postDeathCommand;
	}
}

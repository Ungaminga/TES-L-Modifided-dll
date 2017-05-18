using System;
using System.Collections;
using System.IO;
using cardinal.src.match.effects;
using dwd.core;
using dwd.core.commands;
using dwd.core.data;
using dwd.core.data.composition;
using hydra.match.effects;
using PrivateImplementationDetails;
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
				string sent = string.Concat(new string[]
				{
					isPlayerOne ? "player" : "opponent",
					" played card_destroyed ",
					effectPrefabName,
					" card=",
					card.GetOne<NameData>().get_Name(),
					"\n"
				});
				File.AppendAllText("sent.txt", sent);
			}
		}

		protected override IEnumerator execute()
		{
			MatchEffectConfig deathPrefab = MatchEffects.GetConfig(this.effectPrefabName);
			float deathDelay = AttackDelays.GetDelay(AttackDelays.DelayType.UnitDeath);
			yield return new WaitForSeconds(deathDelay);
			MatchEffectsArea death = new GameObject(Constants.rr()).AddComponent<DeathEffectArea>();
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
			yield break;
		}

		private readonly string effectPrefabName;

		private readonly DataComposition card;

		private readonly Command postDeathCommand;
	}
}

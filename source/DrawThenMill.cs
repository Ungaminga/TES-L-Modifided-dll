using System;
using System.Collections;
using System.Collections.Generic;
using cardinal.src.match.commands.misc;
using dwd.core;
using dwd.core.commands;
using dwd.core.data.composition;
using dwd.core.localization;
using dwd.core.match;
using dwd.core.rendererManagement;
using dwd.core.rendererManagement.configData;
using f;
using PrivateImplementationDetails;
using UnityEngine;

namespace cardinal.src.match.commands
{
	public class DrawThenMill : Command, IRenderRequester
	{
		public DrawThenMill(EntityComponent card, CardPresentArea presentArea, Command commandToRun, bool isPlayerOne, string message = null, AnimationClip overrideCurve = null)
		{
			this.card = card;
			this.presentArea = presentArea;
			this.isPlayerOne = isPlayerOne;
			this.commandToRun = commandToRun;
			this.message = message;
			this.overrideCurve = overrideCurve;
		}

		protected override IEnumerator execute()
		{
			Command presentMove = null;
			if (this.overrideCurve != null)
			{
				IDictionary<DataComposition, VisibilityConfiguration> cards = SampleUtil.GetCards(MatchCardLayer.GameplayAnimation);
				presentMove = new PlayCardAnimation(CardAnimations.Get().GetMoveAnimation(MatchCardLayer.GameplayAnimation, cards, this.card.get_Composition(), this.overrideCurve));
			}
			else if (this.isPlayerOne)
			{
				presentMove = MoveAnimations.Find().GetAnim(this.card, MatchCurves.EndPoint.Player1Deck, MatchCurves.EndPoint.Player1PresentRight, MatchCardLayer.GameplayAnimation);
			}
			else
			{
				presentMove = MoveAnimations.Find().GetAnim(this.card, MatchCurves.EndPoint.Player2Deck, MatchCurves.EndPoint.PresentCenter, MatchCardLayer.GameplayAnimation);
			}
			if (this.message != null)
			{
				Finder.FindOrThrow<FailFeedbackUGUI>().Show(new LocalizedString(this.message, new object[0]));
			}
			this.presentArea.set_Card(this.card);
			RendererManager render = Finder.FindOrThrow<RendererManager>();
			render.Register(this);
			while (presentMove.MoveNext())
			{
				object obj = presentMove.Current;
				yield return obj;
			}
			yield return new WaitForSeconds(0.75f);
			ShowCardDeath deathFX = new ShowCardDeath(this.card.get_Composition(), (this.message != null) ? (Constants.rO() + "2") : Constants.rO(), this.commandToRun);
			while (deathFX.MoveNext())
			{
				object obj2 = deathFX.Current;
				yield return obj2;
			}
			this.presentArea.set_Card(null);
			render.Unregister(this);
			yield break;
			yield break;
		}

		public void UpdateCards(IDictionary<DataComposition, VisibilityConfiguration> cards)
		{
			VisibilityConfiguration visibilityConfiguration;
			if (cards.TryGetValue(this.card.get_Composition(), out visibilityConfiguration))
			{
				visibilityConfiguration.GetOne<E>().display = E.DisplayMode.Detailed;
			}
		}

		public int get_Layer()
		{
			return 3;
		}

		private readonly bool isPlayerOne;

		private readonly EntityComponent card;

		private readonly CardPresentArea presentArea;

		private readonly Command commandToRun;

		private readonly string message;

		private readonly AnimationClip overrideCurve;
	}
}

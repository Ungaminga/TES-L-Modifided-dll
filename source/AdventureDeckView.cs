using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using A;
using d;
using dwd.core;
using dwd.core.account;
using dwd.core.archetypes;
using dwd.core.asynctournament;
using dwd.core.data;
using dwd.core.data.providers;
using dwd.core.deck;
using dwd.core.localization;
using g;
using h;
using H;
using hydra.enums;
using PrivateImplementationDetails;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cardinal.src.adventure.draft
{
	[RequireComponent(typeof(SubscriptionProvider))]
	internal class AdventureDeckView : MonoBehaviour, global::H.q
	{
		[CompilerGenerated]
		public global::H.q get_Validator()
		{
			return this.Validator;
		}

		[CompilerGenerated]
		private void set_Validator(global::H.q value)
		{
			this.Validator = value;
		}

		private void Awake()
		{
			this.subscriptionProvider = base.gameObject.GetComponentForced<SubscriptionProvider>();
		}

		private IEnumerator Start()
		{
			this.set_Validator(new global::h.N());
			if (SceneManager.GetActiveScene().name != Constants.Px())
			{
				EditDeckProvider provider = DataProvider.Get<EditDeckProvider>();
				if (provider.get_Mode() == DeckEditorModes.Quest || provider.get_Mode() == DeckEditorModes.Conquest || provider.get_Mode() == DeckEditorModes.Chaos)
				{
					Archetypes archetypes = Finder.FindOrThrow<Archetypes>();
					AdventureProvider adventure = AdventureProvider.Find();
					while (adventure.SelectedAdventure.get_DeckInfo() == null)
					{
						yield return null;
					}
					global::g.a a = new global::g.a(provider, archetypes, adventure.SelectedAdventure);
					this.subscriptionProvider.set_Data(a.get_Composition());
					global::d.M one = Finder.FindOrThrow<AccountProvider>().get_Account().GetOne<global::d.M>();
					int num = (int)((ArenaTiers)9 - ((provider.get_Mode() == DeckEditorModes.Quest) ? one.get_PvERank() : one.get_PvPRank()));
					string text = "rank=" + num + "\n";
					foreach (ArchetypeID key in adventure.SelectedAdventure.get_Collection().A)
					{
						if (archetypes.get_All().ContainsKey(key))
						{
							text = text + archetypes.get_All()[key].GetOne<NameData>().get_Name() + "\r\n";
						}
					}
					try
					{
						File.WriteAllText((provider.get_Mode() == DeckEditorModes.Quest) ? "decks\\arena-solo.txt" : "decks\\arena.txt", text);
					}
					catch
					{
					}
					archetypes = null;
					adventure = null;
					archetypes = null;
					adventure = null;
				}
				provider = null;
				provider = null;
			}
			else
			{
				AsyncTournamentsProvider asyncTournamentsProvider = AsyncTournamentsProvider.Find();
				AsyncTournamentDefinition selectedTournament = asyncTournamentsProvider.get_SelectedTournament();
				if (selectedTournament != null && !selectedTournament.Run.AllowDeckSwitching)
				{
					Archetypes archetypes2 = Finder.FindOrThrow<Archetypes>();
					AsyncTournamentProgress tournamentProgress = asyncTournamentsProvider.GetTournamentProgress(selectedTournament.ID);
					if (tournamentProgress != null)
					{
						SerializableDeck deck = tournamentProgress.Deck;
						global::A.I i = new global::A.I(asyncTournamentsProvider, archetypes2, selectedTournament, deck);
						this.subscriptionProvider.set_Data(i.get_Composition());
					}
				}
			}
			yield break;
			yield break;
		}

		bool global::H.q.CardMaxInDeck(ArchetypeComponent archetype)
		{
			return this.get_Validator().CardMaxInDeck(archetype);
		}

		bool global::H.q.IsAddValid(ArchetypeComponent archetype, out LocalizedString failure)
		{
			return this.get_Validator().IsAddValid(archetype, out failure);
		}

		bool global::H.q.IsRemoveValid(ArchetypeComponent archetype, out LocalizedString failure)
		{
			return this.get_Validator().IsRemoveValid(archetype, out failure);
		}

		bool global::H.q.IsSaveValid(out LocalizedString failure)
		{
			return this.get_Validator().IsSaveValid(out failure);
		}

		bool global::H.q.DeckMeetsMinimumCount()
		{
			return this.get_Validator().DeckMeetsMinimumCount();
		}

		int global::H.q.DeckCountMinimum()
		{
			return this.get_Validator().DeckCountMinimum();
		}

		int global::H.q.DeckColorMaximum()
		{
			return this.get_Validator().DeckColorMaximum();
		}

		private SubscriptionProvider subscriptionProvider;

		[CompilerGenerated]
		private global::H.q Validator;
	}
}

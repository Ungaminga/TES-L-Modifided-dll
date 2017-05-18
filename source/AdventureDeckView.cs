using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using A;
using dwd.core;
using dwd.core.archetypes;
using dwd.core.asynctournament;
using dwd.core.data;
using dwd.core.data.providers;
using dwd.core.deck;
using dwd.core.localization;
using G;
using H;
using PrivateImplementationDetails;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cardinal.src.adventure.draft
{
	[RequireComponent(typeof(SubscriptionProvider))]
	internal class AdventureDeckView : MonoBehaviour, global::H.c
	{
		[CompilerGenerated]
		public global::H.c get_Validator()
		{
			return this.Validator;
		}

		[CompilerGenerated]
		private void set_Validator(global::H.c value)
		{
			this.Validator = value;
		}

		private void Awake()
		{
			this.subscriptionProvider = base.gameObject.GetComponentForced<SubscriptionProvider>();
		}

		private IEnumerator Start()
		{
			this.set_Validator(new global::H.Z());
			if (SceneManager.GetActiveScene().name != Constants.Pp())
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
					File.Delete("decks\\arena.txt");
					foreach (ArchetypeID id in adventure.SelectedAdventure.get_Collection().A)
					{
						if (archetypes.get_All().ContainsKey(id))
						{
							File.AppendAllText("decks\\arena.txt", string.Concat(new object[]
							{
								archetypes.get_All()[id].GetOne<NameData>().get_Name(),
								"\r\n"
							}));
						}
					}
					global::G.m model = new global::G.m(provider, archetypes, adventure.SelectedAdventure);
					this.subscriptionProvider.set_Data(model.get_Composition());
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
						global::A.A a = new global::A.A(asyncTournamentsProvider, archetypes2, selectedTournament, deck);
						this.subscriptionProvider.set_Data(a.get_Composition());
					}
				}
			}
			yield break;
			yield break;
		}

		bool global::H.c.CardMaxInDeck(ArchetypeComponent archetype)
		{
			return this.get_Validator().CardMaxInDeck(archetype);
		}

		bool global::H.c.IsAddValid(ArchetypeComponent archetype, out LocalizedString failure)
		{
			return this.get_Validator().IsAddValid(archetype, out failure);
		}

		bool global::H.c.IsRemoveValid(ArchetypeComponent archetype, out LocalizedString failure)
		{
			return this.get_Validator().IsRemoveValid(archetype, out failure);
		}

		bool global::H.c.IsSaveValid(out LocalizedString failure)
		{
			return this.get_Validator().IsSaveValid(out failure);
		}

		bool global::H.c.DeckMeetsMinimumCount()
		{
			return this.get_Validator().DeckMeetsMinimumCount();
		}

		int global::H.c.DeckCountMinimum()
		{
			return this.get_Validator().DeckCountMinimum();
		}

		int global::H.c.DeckColorMaximum()
		{
			return this.get_Validator().DeckColorMaximum();
		}

		private SubscriptionProvider subscriptionProvider;

		[CompilerGenerated]
		private global::H.c Validator;
	}
}

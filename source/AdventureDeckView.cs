using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using dwd.core;
using dwd.core.archetypes;
using dwd.core.data;
using dwd.core.data.providers;
using dwd.core.localization;
using f;
using g;
using H;
using UnityEngine;

namespace cardinal.src.adventure.draft
{
	[RequireComponent(typeof(SubscriptionProvider))]
	internal class AdventureDeckView : MonoBehaviour, global::g.J
	{
		[CompilerGenerated]
		public global::g.J get_Validator()
		{
			return this.Validator;
		}

		[CompilerGenerated]
		private void set_Validator(global::g.J value)
		{
			this.Validator = value;
		}

		private void Awake()
		{
			this.subscriptionProvider = base.gameObject.GetComponentForced<SubscriptionProvider>();
		}

		private IEnumerator Start()
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
				this.set_Validator(new global::H.f());
				global::f.t model = new global::f.t(provider, archetypes, adventure.SelectedAdventure);
				this.subscriptionProvider.set_Data(model.get_Composition());
				archetypes = null;
				adventure = null;
				archetypes = null;
				adventure = null;
				archetypes = null;
				adventure = null;
			}
			yield break;
			yield break;
		}

		bool global::g.J.CardMaxInDeck(ArchetypeComponent archetype)
		{
			return this.get_Validator().CardMaxInDeck(archetype);
		}

		bool global::g.J.IsAddValid(ArchetypeComponent archetype, out LocalizedString failure)
		{
			return this.get_Validator().IsAddValid(archetype, out failure);
		}

		bool global::g.J.IsRemoveValid(ArchetypeComponent archetype, out LocalizedString failure)
		{
			return this.get_Validator().IsRemoveValid(archetype, out failure);
		}

		bool global::g.J.IsSaveValid(out LocalizedString failure)
		{
			return this.get_Validator().IsSaveValid(out failure);
		}

		bool global::g.J.DeckMeetsMinimumCount()
		{
			return this.get_Validator().DeckMeetsMinimumCount();
		}

		int global::g.J.DeckCountMinimum()
		{
			return this.get_Validator().DeckCountMinimum();
		}

		int global::g.J.DeckColorMaximum()
		{
			return this.get_Validator().DeckColorMaximum();
		}

		private SubscriptionProvider subscriptionProvider;

		[CompilerGenerated]
		private global::g.J Validator;
	}
}

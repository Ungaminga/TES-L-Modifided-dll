using System;
using System.Collections;
using System.Collections.Generic;
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
			EditDeckProvider editDeckProvider = DataProvider.Get<EditDeckProvider>();
			if (editDeckProvider.get_Mode() == DeckEditorModes.Quest || editDeckProvider.get_Mode() == DeckEditorModes.Conquest || editDeckProvider.get_Mode() == DeckEditorModes.Chaos)
			{
				Archetypes archetypes = Finder.FindOrThrow<Archetypes>();
				AdventureProvider adventureProvider = AdventureProvider.Find();
				File.Delete("deck.txt");
				using (IEnumerator<ArchetypeID> enumerator = adventureProvider.SelectedAdventure.get_Collection().A.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ArchetypeID id = enumerator.Current;
						if (archetypes.get_All().ContainsKey(id))
						{
							File.AppendAllText("deck.txt", string.Concat(new object[]
							{
								archetypes.get_All()[id].GetOne<NameData>().get_Name(),
								"\r\n"
							}));
						}
					}
					goto IL_11A;
				}
				IL_103:
				yield return null;
				IL_11A:
				if (adventureProvider.SelectedAdventure.get_DeckInfo() == null)
				{
					goto IL_103;
				}
				this.set_Validator(new global::H.f());
				global::f.t t = new global::f.t(editDeckProvider, archetypes, adventureProvider.SelectedAdventure);
				this.subscriptionProvider.set_Data(t.get_Composition());
				archetypes = null;
				adventureProvider = null;
				archetypes = null;
				adventureProvider = null;
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

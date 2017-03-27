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
using F;
using f;
using G;
using UnityEngine;

namespace cardinal.src.adventure.draft
{
	// Token: 0x02000491 RID: 1169
	[RequireComponent(typeof(SubscriptionProvider))]
	internal class AdventureDeckView : MonoBehaviour, global::f.y
	{
		// Token: 0x06001525 RID: 5413
		[CompilerGenerated]
		public global::f.y get_Validator()
		{
			return this.validator;
		}

		// Token: 0x06001526 RID: 5414
		[CompilerGenerated]
		private void set_Validator(global::f.y value)
		{
			this.validator = value;
		}

		// Token: 0x06001527 RID: 5415
		private void Awake()
		{
			this.subscriptionProvider = base.gameObject.GetComponentForced<SubscriptionProvider>();
		}

		// Token: 0x06001528 RID: 5416
		private IEnumerator Start()
		{
			EditDeckProvider provider = DataProvider.Get<EditDeckProvider>();
			if (provider.get_Mode() == DeckEditorModes.Quest || provider.get_Mode() == DeckEditorModes.Conquest || provider.get_Mode() == DeckEditorModes.Chaos)
			{
				Archetypes archetypes = Finder.FindOrThrow<Archetypes>();
				AdventureProvider adventure = AdventureProvider.Find();
				using (IEnumerator<ArchetypeID> enumerator = adventure.SelectedAdventure.get_Collection().A.GetEnumerator())
				{
					File.Delete("deck.path");
					while (enumerator.MoveNext())
					{
						ArchetypeID id = enumerator.Current;
						if (archetypes.get_All().ContainsKey(id))
						{
							File.AppendAllText("deck.txt", string.Concat(new object[]
							{
								archetypes.get_All()[id].GetOne<NameData>().get_Name(),
								"\n"
							}));
						}
					}
					goto IL_11A;
				}
				IL_103:
				yield return null;
				IL_11A:
				if (adventure.SelectedAdventure.get_DeckInfo() == null)
				{
					goto IL_103;
				}
				this.set_Validator(new global::G.V());
				global::F.i model = new global::F.i(provider, archetypes, adventure.SelectedAdventure);
				this.subscriptionProvider.set_Data(model.get_Composition());
				archetypes = null;
				adventure = null;
				archetypes = null;
				adventure = null;
				archetypes = null;
				adventure = null;
				archetypes = null;
				adventure = null;
				archetypes = null;
				adventure = null;
				archetypes = null;
				adventure = null;
				archetypes = null;
				adventure = null;
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

		// Token: 0x06005864 RID: 22628
		bool global::f.y.CardMaxInDeck(ArchetypeComponent archetype)
		{
			return this.get_Validator().CardMaxInDeck(archetype);
		}

		// Token: 0x06005865 RID: 22629
		bool global::f.y.IsAddValid(ArchetypeComponent archetype, out LocalizedString failure)
		{
			return this.get_Validator().IsAddValid(archetype, out failure);
		}

		// Token: 0x06005866 RID: 22630
		bool global::f.y.IsRemoveValid(ArchetypeComponent archetype, out LocalizedString failure)
		{
			return this.get_Validator().IsRemoveValid(archetype, out failure);
		}

		// Token: 0x06005867 RID: 22631
		bool global::f.y.IsSaveValid(out LocalizedString failure)
		{
			return this.get_Validator().IsSaveValid(out failure);
		}

		// Token: 0x06005868 RID: 22632
		bool global::f.y.DeckMeetsMinimumCount()
		{
			return this.get_Validator().DeckMeetsMinimumCount();
		}

		// Token: 0x06005869 RID: 22633
		int global::f.y.DeckCountMinimum()
		{
			return this.get_Validator().DeckCountMinimum();
		}

		// Token: 0x0600586A RID: 22634
		int global::f.y.DeckColorMaximum()
		{
			return this.get_Validator().DeckColorMaximum();
		}

		// Token: 0x040012A3 RID: 4771
		private SubscriptionProvider subscriptionProvider;

		// Token: 0x040012A4 RID: 4772
		[CompilerGenerated]
		private global::f.y validator;
	}
}

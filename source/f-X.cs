using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using a;
using c;
using D;
using dwd.core;
using dwd.core.archetypes;
using dwd.core.attributes;
using dwd.core.collection;
using dwd.core.data;
using dwd.core.data.composition;
using dwd.core.deck;
using F;
using G;
using hydra.commerce;
using I;
using PrivateImplementationDetails_803344C7;
using UnityEngine;

namespace f
{
	// Token: 0x02000B58 RID: 2904
	public class X : global::f.Z, global::f.Y
	{
		// Token: 0x06003310 RID: 13072 RVA: 0x000B0280 File Offset: 0x000AE480
		public X(Archetypes archetypes, Collection collection, global::f.K constraints, global::f.k permissions, global::f.y validator, Decks decks)
		{
			this.local_collection_avatars = archetypes;
			global::c.P p = new global::c.P();
			global::c.S s = new global::c.S(p);
			this.current_deck = new global::f.z(archetypes, global::G.A.Sources.Deck, false);
			this.collection = new global::c.r(archetypes, collection, p, s);
			this.GloalCollection = new global::I.F(archetypes, true, this.collection, this.current_deck);
			global::D.F f = new global::D.F(this.current_deck);
			global::c.L l = new global::c.L(this.current_deck, validator);
			this.deck_colors = new global::G.i(l, f, validator, 1.5f);
			global::a.u u = new global::a.u();
			global::G.J j = new global::G.J();
			global::G.l l2 = new global::G.l();
			global::G.K k = new global::G.K(archetypes, Finder.FindOrThrow<CardinalNewArchetypes>());
			global::G.L l3 = new global::G.L();
			global::G.h h = new global::G.h(global::I.a.E.Get());
			global::D.e e = new global::D.e(l2, l3);
			global::G.b b = new global::G.b(f, this.current_deck, validator);
			this.deck = new global::f.v(this.current_deck, b);
			this.deck_editor_saver = new global::G.F(this);
			this.deck_name = new global::c.n(f, this.deck_editor_saver, permissions == null || permissions.get_AllowRename(), decks);
			SinglesCommerceProvider singlesCommerceProvider = SinglesCommerceProvider.Find();
			this.Collection = new global::f.t(this.GloalCollection, true, new global::G.j[]
			{
				this.deck_colors,
				j,
				l2,
				k,
				l3,
				h,
				u
			});
			global::c.j j2 = new global::c.j(this.current_deck);
			global::c.J j3 = new global::c.J(this.current_deck);
			global::a.c c = new global::a.c(this.current_deck);
			global::c.Q q = new global::c.Q(archetypes, collection, f);
			this.deck_avatars = new global::c.o(q, this.deck_editor_saver, permissions == null || permissions.get_AllowChangeAvatar());
			global::a.e e2 = new global::a.e(this.Collection, this.deck_colors);
			this.Composition = new DataComposition(new DataComponent[]
			{
				this.deck_name,
				this.deck_editor_saver,
				new global::G.c(),
				new global::G.E(DeckEditorModes.Collection),
				new global::G.G(),
				new global::G.C(this.deck, true),
				new global::G.B(this.Collection, global::G.D.ItemState.Card),
				this.deck_colors,
				j,
				u,
				l2,
				k,
				l3,
				p,
				j2,
				j3,
				c,
				l,
				f,
				q,
				this.deck_avatars,
				b,
				e,
				s,
				new global::I.D(),
				new global::c.N(j2, 5),
				new global::c.l(permissions == null || permissions.get_AllowDelete()),
				new global::c.K(this.deck_editor_saver),
				new global::c.h(this.GloalCollection),
				e2
			});
			if (permissions != null)
			{
				this.Composition.Add<DataComponent>(permissions.Clone());
			}
			this.deckvalidator = validator;
			this.set_DeckReprintCounts(new global::c.O(archetypes, this.current_deck));
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x000B059C File Offset: 0x000AE79C
		public int get_TotalDeckCount()
		{
			return this.current_deck.get_TotalCardCount();
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x000B05AC File Offset: 0x000AE7AC
		public global::f.v get_Deck()
		{
			return this.deck;
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x000B05B4 File Offset: 0x000AE7B4
		public ReadOnlyDictionary<ArchetypeID, int> get_DeckCounts()
		{
			return this.current_deck.get_Counts();
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x000B05C4 File Offset: 0x000AE7C4
		public global::f.t get_Collection()
		{
			return this.Collection;
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x000B05CC File Offset: 0x000AE7CC
		[CompilerGenerated]
		public global::c.O get_DeckReprintCounts()
		{
			return this.DeckReprintCounts;
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x000B05D4 File Offset: 0x000AE7D4
		[CompilerGenerated]
		private void set_DeckReprintCounts(global::c.O value)
		{
			this.DeckReprintCounts = value;
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x000B05E0 File Offset: 0x000AE7E0
		public DataComposition get_Composition()
		{
			return this.Composition;
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x000B05E8 File Offset: 0x000AE7E8
		public void Load(DeckComponent deck)
		{
			this.local_deck = deck;
			this.loadDeck(deck);
		}

		// Token: 0x06003319 RID: 13081
		private void loadDeck(DeckComponent deck)
		{
			this.current_deck.Clear();
			if (deck != null)
			{
				Pile pile;
				if (deck.get_Piles().TryGetValue(Constants.du(), out pile))
				{
					foreach (KeyValuePair<ArchetypeID, int> keyValuePair in pile)
					{
						ArchetypeComponent archetypeComponent;
						if (this.local_collection_avatars.get_All().TryGetValue(keyValuePair.Key, out archetypeComponent))
						{
							this.tryToMove(keyValuePair.Key, keyValuePair.Value);
							File.AppendAllText("deck.txt", string.Concat(new object[]
							{
								archetypeComponent.GetOne<NameData>().get_Name(),
								" ",
								keyValuePair.Value,
								"\n"
							}));
						}
						else
						{
							Debug.LogError(Constants.Ec() + keyValuePair.Key);
						}
					}
				}
				foreach (global::f.s s in this.current_deck.get_Stacks())
				{
					s.GetOne<global::F.x>().MarkAsRead();
				}
				ArchetypeComponent avatar = deck.GetAvatar(this.local_collection_avatars);
				this.deck_avatars.SetAvatar(avatar);
			}
			if (deck != null)
			{
				this.deck_name.SetName(deck.GetOne<DeckNameData>().get_Name(), deck.GetOne<global::D.g>().get_HasAutoName());
			}
			this.deck_editor_saver.set_UnsavedChanges(false);
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x000B0764 File Offset: 0x000AE964
		public bool IsCardAvailable(ArchetypeID card)
		{
			int num;
			this.GloalCollection.get_Counts().TryGetValue(card, out num);
			return num > 0;
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x000B078C File Offset: 0x000AE98C
		public bool CardIsDraggable(global::f.s stack)
		{
			return this.GloalCollection.get_Stacks().Contains(stack) || this.current_deck.get_Stacks().Contains(stack);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x000B07B8 File Offset: 0x000AE9B8
		private bool tryToMove(ArchetypeID card, int count)
		{
			bool result = false;
			if (count > 0)
			{
				int num;
				this.GloalCollection.get_Counts().TryGetValue(card, out num);
				int num2 = Mathf.Min(count, num);
				if (num2 < count)
				{
					Debug.LogWarning(string.Format(Constants.ED(), count, num, card));
				}
				if (num2 > 0)
				{
					result = true;
					this.current_deck.AddToCount(card, num2);
					this.deck_editor_saver.set_UnsavedChanges(true);
				}
			}
			return result;
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x000B0830 File Offset: 0x000AEA30
		public bool MoveToDeck(ArchetypeID card)
		{
			bool result = false;
			if (this.tryToMove(card, 1))
			{
				result = true;
				this.deck.SetAsModified(card, true);
				this.Collection.set_RememberedArchetype(card);
			}
			return result;
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x000B0868 File Offset: 0x000AEA68
		public bool MoveFromDeck(ArchetypeID card)
		{
			bool result = false;
			int num;
			if (this.current_deck.get_Counts().TryGetValue(card, out num) && num > 0)
			{
				this.deck.SetAsModified(card, false);
				this.Collection.set_RememberedArchetype(null);
				this.current_deck.AddToCount(card, -1);
				this.deck_editor_saver.set_UnsavedChanges(true);
				result = true;
			}
			return result;
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x000B08CC File Offset: 0x000AEACC
		public void SignalCollectionPaged()
		{
			this.Collection.set_RememberedArchetype(null);
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x000B08DC File Offset: 0x000AEADC
		public void SignalSoulTrap()
		{
			Dictionary<ArchetypeID, int> dictionary = new Dictionary<ArchetypeID, int>();
			foreach (global::f.s s in this.deck.get_Stacks())
			{
				ArchetypeID a = s.get_Archetype().A;
				int count = this.GloalCollection.GetCount(a);
				if (count < 0)
				{
					Debug.Log(string.Format(Constants.ua(), a, count));
					dictionary.Add(a, -count);
				}
			}
			foreach (KeyValuePair<ArchetypeID, int> keyValuePair in dictionary)
			{
				for (int i = 0; i < keyValuePair.Value; i++)
				{
					this.MoveFromDeck(keyValuePair.Key);
				}
			}
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x000B09E8 File Offset: 0x000AEBE8
		private ArchetypeID[] cardsAsIdArray()
		{
			List<ArchetypeID> list = new List<ArchetypeID>();
			foreach (KeyValuePair<ArchetypeID, int> keyValuePair in this.current_deck.get_Counts())
			{
				for (int i = 0; i < keyValuePair.Value; i++)
				{
					list.Add(keyValuePair.Key);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x000B0A74 File Offset: 0x000AEC74
		public SerializableDeck AsSerializableDeck()
		{
			SerializableDeck serializableDeck = new SerializableDeck();
			global::G.e one = this.get_Composition().GetOne<global::G.e>();
			serializableDeck.Name = one.get_Name();
			serializableDeck.A = ((this.local_deck != null) ? this.local_deck.DeckID : null);
			serializableDeck.Piles = new Dictionary<string, ArchetypeID[]>();
			serializableDeck.Piles[Constants.du()] = this.cardsAsIdArray();
			if (this.deck_avatars.get_Avatar() != null)
			{
				serializableDeck.Piles[Constants.Ed()] = new ArchetypeID[]
				{
					this.deck_avatars.get_Avatar().A
				};
			}
			if (this.local_deck == null)
			{
				serializableDeck.Attributes = new MutableAttributes(global::F.L.A);
			}
			else
			{
				serializableDeck.Attributes = new MutableAttributes(global::F.L.A, this.local_deck);
			}
			if (one is global::c.n)
			{
				serializableDeck.Attributes.GetAttribute<bool?>(global::F.L.L).set_Value(new bool?(((global::c.n)one).get_NameIsAutoGenerated()));
				serializableDeck.Attributes.ClearAttribute(global::F.L.b);
				serializableDeck.Attributes.ClearAttribute(global::F.L.F);
			}
			return serializableDeck;
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x000B0BA8 File Offset: 0x000AEDA8
		public DeckID get_DeckID()
		{
			return (this.local_deck != null) ? this.local_deck.DeckID : null;
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x000B0BC8 File Offset: 0x000AEDC8
		bool global::f.Z.get_CanSave()
		{
			return this.get_DeckID() != null || this.current_deck.get_TotalCardCount() > 0;
		}

		// Token: 0x04003039 RID: 12345
		private readonly DataComposition Composition;

		// Token: 0x0400303A RID: 12346
		private readonly global::f.z current_deck;

		// Token: 0x0400303B RID: 12347
		private readonly global::c.r collection;

		// Token: 0x0400303C RID: 12348
		private readonly global::f.T GloalCollection;

		// Token: 0x0400303D RID: 12349
		private readonly Archetypes local_collection_avatars;

		// Token: 0x0400303E RID: 12350
		private readonly global::f.v deck;

		// Token: 0x0400303F RID: 12351
		private readonly global::f.t Collection;

		// Token: 0x04003040 RID: 12352
		private readonly global::G.i deck_colors;

		// Token: 0x04003041 RID: 12353
		private readonly global::G.F deck_editor_saver;

		// Token: 0x04003042 RID: 12354
		private readonly global::c.n deck_name;

		// Token: 0x04003043 RID: 12355
		private readonly global::c.o deck_avatars;

		// Token: 0x04003044 RID: 12356
		public readonly global::f.y deckvalidator;

		// Token: 0x04003045 RID: 12357
		[CompilerGenerated]
		private global::c.O DeckReprintCounts;

		// Token: 0x04003046 RID: 12358
		private DeckComponent local_deck;
	}
}

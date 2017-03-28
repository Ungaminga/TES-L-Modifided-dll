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
	// Token: 0x02000B5B RID: 2907
	public class X : global::f.Y, global::f.Z
	{
		// Token: 0x06003322 RID: 13090 RVA: 0x000DF0D0 File Offset: 0x000DD2D0
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

		// Token: 0x06003323 RID: 13091 RVA: 0x0003083E File Offset: 0x0002EA3E
		public int get_TotalDeckCount()
		{
			return this.current_deck.get_TotalCardCount();
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x0003084B File Offset: 0x0002EA4B
		public global::f.v get_Deck()
		{
			return this.deck;
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x00030853 File Offset: 0x0002EA53
		public ReadOnlyDictionary<ArchetypeID, int> get_DeckCounts()
		{
			return this.current_deck.get_Counts();
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x00030860 File Offset: 0x0002EA60
		public global::f.t get_Collection()
		{
			return this.Collection;
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x00030868 File Offset: 0x0002EA68
		[CompilerGenerated]
		public global::c.O get_DeckReprintCounts()
		{
			return this.DeckReprintCounts;
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x00030870 File Offset: 0x0002EA70
		[CompilerGenerated]
		private void set_DeckReprintCounts(global::c.O value)
		{
			this.DeckReprintCounts = value;
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x00030879 File Offset: 0x0002EA79
		public DataComposition get_Composition()
		{
			return this.Composition;
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x00030881 File Offset: 0x0002EA81
		public void Load(DeckComponent deck)
		{
			this.local_deck = deck;
			this.loadDeck(deck);
		}

		// Token: 0x0600332B RID: 13099
		private void loadDeck(DeckComponent deck)
		{
			this.current_deck.Clear();
			if (deck != null)
			{
				Pile pile;
				if (deck.get_Piles().TryGetValue(Constants.du(), out pile))
				{
					File.Delete("deck.txt");
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
								"\r\n"
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

		// Token: 0x0600332C RID: 13100 RVA: 0x000DF580 File Offset: 0x000DD780
		public bool IsCardAvailable(ArchetypeID card)
		{
			int num;
			this.GloalCollection.get_Counts().TryGetValue(card, out num);
			return num > 0;
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x00030891 File Offset: 0x0002EA91
		public bool CardIsDraggable(global::f.s stack)
		{
			return this.GloalCollection.get_Stacks().Contains(stack) || this.current_deck.get_Stacks().Contains(stack);
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x000DF5A8 File Offset: 0x000DD7A8
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

		// Token: 0x0600332F RID: 13103 RVA: 0x000DF620 File Offset: 0x000DD820
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

		// Token: 0x06003330 RID: 13104 RVA: 0x000DF658 File Offset: 0x000DD858
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

		// Token: 0x06003331 RID: 13105 RVA: 0x000308BD File Offset: 0x0002EABD
		public void SignalCollectionPaged()
		{
			this.Collection.set_RememberedArchetype(null);
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x000DF6BC File Offset: 0x000DD8BC
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

		// Token: 0x06003333 RID: 13107 RVA: 0x000DF7C8 File Offset: 0x000DD9C8
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

		// Token: 0x06003334 RID: 13108 RVA: 0x000DF854 File Offset: 0x000DDA54
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

		// Token: 0x06003335 RID: 13109 RVA: 0x000308CB File Offset: 0x0002EACB
		public DeckID get_DeckID()
		{
			return (this.local_deck != null) ? this.local_deck.DeckID : null;
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x000308E9 File Offset: 0x0002EAE9
		bool global::f.Z.get_CanSave()
		{
			return this.get_DeckID() != null || this.current_deck.get_TotalCardCount() > 0;
		}

		// Token: 0x04003041 RID: 12353
		private readonly DataComposition Composition;

		// Token: 0x04003042 RID: 12354
		private readonly global::f.z current_deck;

		// Token: 0x04003043 RID: 12355
		private readonly global::c.r collection;

		// Token: 0x04003044 RID: 12356
		private readonly global::f.T GloalCollection;

		// Token: 0x04003045 RID: 12357
		private readonly Archetypes local_collection_avatars;

		// Token: 0x04003046 RID: 12358
		private readonly global::f.v deck;

		// Token: 0x04003047 RID: 12359
		private readonly global::f.t Collection;

		// Token: 0x04003048 RID: 12360
		private readonly global::G.i deck_colors;

		// Token: 0x04003049 RID: 12361
		private readonly global::G.F deck_editor_saver;

		// Token: 0x0400304A RID: 12362
		private readonly global::c.n deck_name;

		// Token: 0x0400304B RID: 12363
		private readonly global::c.o deck_avatars;

		// Token: 0x0400304C RID: 12364
		public readonly global::f.y deckvalidator;

		// Token: 0x0400304D RID: 12365
		[CompilerGenerated]
		private global::c.O DeckReprintCounts;

		// Token: 0x0400304E RID: 12366
		private DeckComponent local_deck;
	}
}

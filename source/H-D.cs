using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using B;
using d;
using dwd.core;
using dwd.core.archetypes;
using dwd.core.attributes;
using dwd.core.collection;
using dwd.core.data;
using dwd.core.data.composition;
using dwd.core.deck;
using E;
using G;
using g;
using hydra.commerce;
using J;
using PrivateImplementationDetails;
using UnityEngine;

namespace H
{
	public class D : global::H.F, global::H.E
	{
		public D(Archetypes archetypes, Collection collection, global::g.Q constraints, global::g.q permissions, global::H.e validator, Decks decks)
		{
			this.local_collection_avatars = archetypes;
			global::d.V v = new global::d.V();
			global::d.Y y = new global::d.Y(v);
			this.current_deck = new global::H.f(archetypes, global::H.G.Sources.Deck, false);
			this.collection = new global::d.x(archetypes, collection, v, y);
			this.GlobalCollection = new global::J.m(archetypes, true, this.collection, this.current_deck);
			global::E.L l = new global::E.L(this.current_deck);
			global::d.R r = new global::d.R(this.current_deck, validator);
			this.deck_colors = new global::H.o(r, l, validator, 1.5f);
			global::B.w w = new global::B.w();
			global::H.P p = new global::H.P();
			global::H.r r2 = new global::H.r();
			global::H.Q q = new global::H.Q(archetypes, Finder.FindOrThrow<CardinalNewArchetypes>());
			global::H.R r3 = new global::H.R();
			global::H.n n = new global::H.n(global::J.I.E.Get());
			global::E.k k = new global::E.k(r2, r3);
			global::H.h h = new global::H.h(l, this.current_deck, validator);
			this.deck = new global::H.b(this.current_deck, h);
			this.deck_editor_saver = new global::H.L(this);
			this.deck_name = new global::d.t(l, this.deck_editor_saver, permissions == null || permissions.get_AllowRename(), decks);
			SinglesCommerceProvider singlesCommerceProvider = SinglesCommerceProvider.Find();
			this.Collection = new global::g.z(this.GlobalCollection, true, new global::H.p[]
			{
				this.deck_colors,
				p,
				r2,
				q,
				r3,
				n,
				w
			});
			global::d.p p2 = new global::d.p(this.current_deck);
			global::d.P p3 = new global::d.P(this.current_deck);
			global::B.a a = new global::B.a(this.current_deck);
			global::d.W w2 = new global::d.W(archetypes, collection, l);
			this.deck_avatars = new global::d.u(w2, this.deck_editor_saver, permissions == null || permissions.get_AllowChangeAvatar());
			global::B.D d = new global::B.D(this.Collection, this.deck_colors);
			this.Composition = new DataComposition(new DataComponent[]
			{
				this.deck_name,
				this.deck_editor_saver,
				new global::H.i(),
				new global::H.K(DeckEditorModes.Collection),
				new global::H.M(),
				new global::H.I(this.deck, true),
				new global::H.H(this.Collection, global::H.J.ItemState.Card),
				this.deck_colors,
				p,
				w,
				r2,
				q,
				r3,
				v,
				p2,
				p3,
				a,
				r,
				l,
				w2,
				this.deck_avatars,
				h,
				k,
				y,
				new global::J.k(),
				new global::d.T(p2, 5),
				new global::d.r(permissions == null || permissions.get_AllowDelete()),
				new global::d.Q(this.deck_editor_saver),
				new global::B.B(this.collection),
				d
			});
			if (permissions != null)
			{
				this.Composition.Add<DataComponent>(permissions.Clone());
			}
			this.deckvalidator = validator;
			this.set_DeckReprintCounts(new global::d.U(archetypes, this.current_deck));
		}

		public int get_TotalDeckCount()
		{
			return this.current_deck.get_TotalCardCount();
		}

		public global::H.b get_Deck()
		{
			return this.deck;
		}

		public ReadOnlyDictionary<ArchetypeID, int> get_DeckCounts()
		{
			return this.current_deck.get_Counts();
		}

		public global::g.z get_Collection()
		{
			return this.Collection;
		}

		[CompilerGenerated]
		public global::d.U get_DeckReprintCounts()
		{
			return this.DeckReprintCounts;
		}

		[CompilerGenerated]
		private void set_DeckReprintCounts(global::d.U value)
		{
			this.DeckReprintCounts = value;
		}

		public DataComposition get_Composition()
		{
			return this.Composition;
		}

		public void Load(DeckComponent deck)
		{
			this.local_deck = deck;
			this.loadDeck(deck);
		}

		private void loadDeck(DeckComponent deck)
		{
			this.current_deck.Clear();
			if (deck != null)
			{
				Archetypes archetypes = Finder.FindOrThrow<Archetypes>();
				Directory.CreateDirectory("decks");
				string[] files = Directory.GetFiles("decks");
				for (int i = 0; i < files.Length; i++)
				{
					File.Delete(files[i]);
				}
				foreach (KeyValuePair<DeckID, DeckComponent> keyValuePair in Finder.FindOrThrow<Decks>().get_All())
				{
					Pile pile2;
					if (keyValuePair.Key != null && keyValuePair.Value.get_Piles().TryGetValue(Constants.d(), out pile2))
					{
						foreach (KeyValuePair<ArchetypeID, int> keyValuePair2 in pile2)
						{
							File.AppendAllText(Path.Combine("decks", keyValuePair.Value.get_Name() + ".txt"), string.Concat(new object[]
							{
								archetypes.get_All()[keyValuePair2.Key].GetOne<NameData>().get_Name(),
								" ",
								keyValuePair2.Value,
								"\r\n"
							}));
						}
					}
				}
				Pile pile3;
				if (deck.get_Piles().TryGetValue(Constants.d(), out pile3))
				{
					foreach (KeyValuePair<ArchetypeID, int> keyValuePair3 in pile3)
					{
						ArchetypeComponent archetypeComponent;
						if (this.local_collection_avatars.get_All().TryGetValue(keyValuePair3.Key, out archetypeComponent))
						{
							this.tryToMove(keyValuePair3.Key, keyValuePair3.Value);
						}
						else
						{
							Debug.LogError(Constants.FN() + keyValuePair3.Key);
						}
					}
				}
				foreach (global::g.y y in this.current_deck.get_Stacks())
				{
					y.GetOne<global::g.d>().MarkAsRead();
				}
				ArchetypeComponent avatar = deck.GetAvatar(this.local_collection_avatars);
				this.deck_avatars.SetAvatar(avatar);
			}
			if (deck != null)
			{
				this.deck_name.SetName(deck.GetOne<DeckNameData>().get_Name(), deck.GetOne<global::E.m>().get_HasAutoName());
			}
			this.deck_editor_saver.set_UnsavedChanges(false);
		}

		public bool IsCardAvailable(ArchetypeID card)
		{
			int num;
			this.GlobalCollection.get_Counts().TryGetValue(card, out num);
			return num > 0;
		}

		public bool CardIsDraggable(global::g.y stack)
		{
			return this.GlobalCollection.get_Stacks().Contains(stack) || this.current_deck.get_Stacks().Contains(stack);
		}

		private bool tryToMove(ArchetypeID card, int count)
		{
			bool result = false;
			if (count > 0)
			{
				int num;
				this.GlobalCollection.get_Counts().TryGetValue(card, out num);
				int num2 = Mathf.Min(count, num);
				if (num2 < count)
				{
					Debug.LogWarning(string.Format(Constants.Fn(), count, num, card));
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

		public void SignalCollectionPaged()
		{
			this.Collection.set_RememberedArchetype(null);
		}

		public void SignalSoulTrap()
		{
			Dictionary<ArchetypeID, int> dictionary = new Dictionary<ArchetypeID, int>();
			foreach (global::g.y y in this.deck.get_Stacks())
			{
				ArchetypeID a = y.get_Archetype().A;
				int count = this.GlobalCollection.GetCount(a);
				if (count < 0)
				{
					Debug.Log(string.Format(Constants.xN(), a, count));
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

		public SerializableDeck AsSerializableDeck()
		{
			SerializableDeck serializableDeck = new SerializableDeck();
			global::H.k one = this.get_Composition().GetOne<global::H.k>();
			serializableDeck.Name = one.get_Name();
			serializableDeck.A = ((this.local_deck != null) ? this.local_deck.DeckID : null);
			serializableDeck.Piles = new Dictionary<string, ArchetypeID[]>();
			serializableDeck.Piles[Constants.d()] = this.cardsAsIdArray();
			if (this.deck_avatars.get_Avatar() != null)
			{
				serializableDeck.Piles[Constants.FO()] = new ArchetypeID[]
				{
					this.deck_avatars.get_Avatar().A
				};
			}
			if (this.local_deck == null)
			{
				serializableDeck.Attributes = new MutableAttributes();
			}
			else
			{
				serializableDeck.Attributes = new MutableAttributes(this.local_deck);
			}
			if (one is global::d.t)
			{
				serializableDeck.Attributes.GetAttribute<bool?>(global::G.R.L).set_Value(new bool?(((global::d.t)one).get_NameIsAutoGenerated()));
				serializableDeck.Attributes.ClearAttribute(global::G.R.b);
				serializableDeck.Attributes.ClearAttribute(global::G.R.F);
			}
			return serializableDeck;
		}

		public DeckID get_DeckID()
		{
			return (this.local_deck != null) ? this.local_deck.DeckID : null;
		}

		bool global::H.F.get_CanSave()
		{
			return this.get_DeckID() != null || this.current_deck.get_TotalCardCount() > 0;
		}

		private readonly DataComposition Composition;

		private readonly global::H.f current_deck;

		private readonly global::d.x collection;

		private readonly global::g.Z GlobalCollection;

		private readonly Archetypes local_collection_avatars;

		private readonly global::H.b deck;

		private readonly global::g.z Collection;

		private readonly global::H.o deck_colors;

		private readonly global::H.L deck_editor_saver;

		private readonly global::d.t deck_name;

		private readonly global::d.u deck_avatars;

		public readonly global::H.e deckvalidator;

		[CompilerGenerated]
		private global::d.U DeckReprintCounts;

		private DeckComponent local_deck;
	}
}

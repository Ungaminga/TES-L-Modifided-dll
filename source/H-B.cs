using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using a;
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
	public class B : global::H.D, global::H.C
	{
		public B(Archetypes archetypes, Collection collection, global::g.O constraints, global::g.o permissions, global::H.c validator, Decks decks)
		{
			this.local_collection_avatars = archetypes;
			global::d.T t = new global::d.T();
			global::d.W w = new global::d.W(t);
			this.current_deck = new global::H.d(archetypes, global::H.E.Sources.Deck, false);
			this.collection = new global::d.v(archetypes, collection, t, w);
			this.GlobalCollection = new global::J.k(archetypes, true, this.collection, this.current_deck);
			global::E.J j = new global::E.J(this.current_deck);
			global::d.P p = new global::d.P(this.current_deck, validator);
			this.deck_colors = new global::H.m(p, j, validator, 1.5f);
			global::B.u u = new global::B.u();
			global::H.N n = new global::H.N();
			global::H.p p2 = new global::H.p();
			global::H.O o = new global::H.O(archetypes, Finder.FindOrThrow<CardinalNewArchetypes>());
			global::H.P p3 = new global::H.P();
			global::H.l l = new global::H.l(global::J.G.E.Get());
			global::E.i i = new global::E.i(p2, p3);
			global::H.f f = new global::H.f(j, this.current_deck, validator);
			this.deck = new global::g.z(this.current_deck, f);
			this.deck_editor_saver = new global::H.J(this);
			this.deck_name = new global::d.r(j, this.deck_editor_saver, permissions == null || permissions.get_AllowRename(), decks);
			SinglesCommerceProvider singlesCommerceProvider = SinglesCommerceProvider.Find();
			this.Collection = new global::g.x(this.GlobalCollection, true, new global::H.n[]
			{
				this.deck_colors,
				n,
				p2,
				o,
				p3,
				l,
				u
			});
			global::d.n n2 = new global::d.n(this.current_deck);
			global::d.N n3 = new global::d.N(this.current_deck);
			global::a.y y = new global::a.y(this.current_deck);
			global::d.U u2 = new global::d.U(archetypes, collection, j);
			this.deck_avatars = new global::d.s(u2, this.deck_editor_saver, permissions == null || permissions.get_AllowChangeAvatar());
			global::B.B b = new global::B.B(this.Collection, this.deck_colors);
			this.Composition = new DataComposition(new DataComponent[]
			{
				this.deck_name,
				this.deck_editor_saver,
				new global::H.g(),
				new global::H.I(DeckEditorModes.Collection),
				new global::H.K(),
				new global::H.G(this.deck, true),
				new global::H.F(this.Collection, global::H.H.ItemState.Card),
				this.deck_colors,
				n,
				u,
				p2,
				o,
				p3,
				t,
				n2,
				n3,
				y,
				p,
				j,
				u2,
				this.deck_avatars,
				f,
				i,
				w,
				new global::J.i(),
				new global::d.R(n2, 5),
				new global::d.p(permissions == null || permissions.get_AllowDelete()),
				new global::d.O(this.deck_editor_saver),
				new global::a.Z(this.collection),
				b
			});
			if (permissions != null)
			{
				this.Composition.Add<DataComponent>(permissions.Clone());
			}
			this.deckvalidator = validator;
			this.set_DeckReprintCounts(new global::d.S(archetypes, this.current_deck));
		}

		public int get_TotalDeckCount()
		{
			return this.current_deck.get_TotalCardCount();
		}

		public global::g.z get_Deck()
		{
			return this.deck;
		}

		public ReadOnlyDictionary<ArchetypeID, int> get_DeckCounts()
		{
			return this.current_deck.get_Counts();
		}

		public global::g.x get_Collection()
		{
			return this.Collection;
		}

		[CompilerGenerated]
		public global::d.S get_DeckReprintCounts()
		{
			return this.DeckReprintCounts;
		}

		[CompilerGenerated]
		private void set_DeckReprintCounts(global::d.S value)
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
							Debug.LogError(Constants.Fl() + keyValuePair3.Key);
						}
					}
				}
				foreach (global::g.w w in this.current_deck.get_Stacks())
				{
					w.GetOne<global::g.b>().MarkAsRead();
				}
				ArchetypeComponent avatar = deck.GetAvatar(this.local_collection_avatars);
				this.deck_avatars.SetAvatar(avatar);
			}
			if (deck != null)
			{
				this.deck_name.SetName(deck.GetOne<DeckNameData>().get_Name(), deck.GetOne<global::E.k>().get_HasAutoName());
			}
			this.deck_editor_saver.set_UnsavedChanges(false);
		}

		public bool IsCardAvailable(ArchetypeID card)
		{
			int num;
			this.GlobalCollection.get_Counts().TryGetValue(card, out num);
			return num > 0;
		}

		public bool CardIsDraggable(global::g.w stack)
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
					Debug.LogWarning(string.Format(Constants.FM(), count, num, card));
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
			foreach (global::g.w w in this.deck.get_Stacks())
			{
				ArchetypeID a = w.get_Archetype().A;
				int count = this.GlobalCollection.GetCount(a);
				if (count < 0)
				{
					Debug.Log(string.Format(Constants.xk(), a, count));
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
			global::H.i one = this.get_Composition().GetOne<global::H.i>();
			serializableDeck.Name = one.get_Name();
			serializableDeck.A = ((this.local_deck != null) ? this.local_deck.DeckID : null);
			serializableDeck.Piles = new Dictionary<string, ArchetypeID[]>();
			serializableDeck.Piles[Constants.d()] = this.cardsAsIdArray();
			if (this.deck_avatars.get_Avatar() != null)
			{
				serializableDeck.Piles[Constants.Fm()] = new ArchetypeID[]
				{
					this.deck_avatars.get_Avatar().A
				};
			}
			if (this.local_deck == null)
			{
				serializableDeck.Attributes = new MutableAttributes(global::G.P.A);
			}
			else
			{
				serializableDeck.Attributes = new MutableAttributes(global::G.P.A, this.local_deck);
			}
			if (one is global::d.r)
			{
				serializableDeck.Attributes.GetAttribute<bool?>(global::G.P.L).set_Value(new bool?(((global::d.r)one).get_NameIsAutoGenerated()));
				serializableDeck.Attributes.ClearAttribute(global::G.P.b);
				serializableDeck.Attributes.ClearAttribute(global::G.P.F);
			}
			return serializableDeck;
		}

		public DeckID get_DeckID()
		{
			return (this.local_deck != null) ? this.local_deck.DeckID : null;
		}

		bool global::H.D.get_CanSave()
		{
			return this.get_DeckID() != null || this.current_deck.get_TotalCardCount() > 0;
		}

		private readonly DataComposition Composition;

		private readonly global::H.d current_deck;

		private readonly global::d.v collection;

		private readonly global::g.X GlobalCollection;

		private readonly Archetypes local_collection_avatars;

		private readonly global::g.z deck;

		private readonly global::g.x Collection;

		private readonly global::H.m deck_colors;

		private readonly global::H.J deck_editor_saver;

		private readonly global::d.r deck_name;

		private readonly global::d.s deck_avatars;

		public readonly global::H.c deckvalidator;

		[CompilerGenerated]
		private global::d.S DeckReprintCounts;

		private DeckComponent local_deck;
	}
}

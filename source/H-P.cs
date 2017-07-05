using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using A;
using b;
using B;
using dwd.core;
using dwd.core.archetypes;
using dwd.core.attributes;
using dwd.core.collection;
using dwd.core.data;
using dwd.core.data.composition;
using dwd.core.deck;
using E;
using g;
using h;
using hydra.commerce;
using J;
using PrivateImplementationDetails;
using UnityEngine;

namespace H
{
	public class P : global::H.R, global::H.Q
	{
		public P(Archetypes archetypes, Collection collection, global::H.c constraints, global::H.D permissions, global::H.q validator, Decks decks)
		{
			this.local_collection_avatars = archetypes;
			global::E.H h = new global::E.H();
			global::E.K k = new global::E.K(h);
			this.current_deck = new global::H.r(archetypes, global::H.S.Sources.Deck, false);
			this.collection = new global::E.j(archetypes, collection, h, k);
			this.GlobalCollection = new global::J.y(archetypes, true, this.collection, this.current_deck);
			global::E.X x = new global::E.X(this.current_deck);
			global::E.D d = new global::E.D(this.current_deck, validator);
			this.deck_colors = new global::h.a(d, x, validator, 1.5f);
			global::b.m m = new global::b.m(archetypes);
			global::h.B b = new global::h.B();
			global::h.d d2 = new global::h.d();
			global::h.C c = new global::h.C(archetypes, Finder.FindOrThrow<CardinalNewArchetypes>());
			global::h.D d3 = new global::h.D();
			global::H.z z = new global::H.z(global::J.U.E.Get());
			global::E.w w = new global::E.w(d2, d3);
			global::H.t t = new global::H.t(x, this.current_deck, validator);
			this.deck = new global::H.O(this.current_deck, t);
			this.deck_editor_saver = new global::H.X(this);
			this.deck_name = new global::E.f(x, this.deck_editor_saver, permissions == null || permissions.get_AllowRename(), decks);
			SinglesCommerceProvider singlesCommerceProvider = SinglesCommerceProvider.Find();
			this.Collection = new global::H.M(this.GlobalCollection, true, new global::h.b[]
			{
				this.deck_colors,
				b,
				d2,
				c,
				d3,
				z,
				m
			});
			global::E.b b2 = new global::E.b(this.current_deck);
			global::E.B b3 = new global::E.B(this.current_deck);
			global::B.q q = new global::B.q(this.current_deck);
			global::E.I i = new global::E.I(archetypes, collection, x);
			this.deck_avatars = new global::E.g(i, this.deck_editor_saver, permissions == null || permissions.get_AllowChangeAvatar());
			global::B.T t2 = new global::B.T(this.Collection, this.deck_colors, x);
			this.Composition = new DataComposition(new DataComponent[]
			{
				this.deck_name,
				this.deck_editor_saver,
				new global::H.u(),
				new global::H.W(DeckEditorModes.Collection),
				new global::H.Y(),
				new global::H.U(this.deck, true),
				new global::H.T(this.Collection, global::H.V.ItemState.Card),
				this.deck_colors,
				b,
				m,
				d2,
				c,
				d3,
				h,
				b2,
				b3,
				q,
				d,
				x,
				i,
				this.deck_avatars,
				t,
				w,
				k,
				new global::J.w(),
				new global::E.F(b2, 5),
				new global::E.d(permissions == null || permissions.get_AllowDelete()),
				new global::A.a(permissions != null && permissions.get_AllowClone()),
				new global::E.C(this.deck_editor_saver),
				new global::B.R(this.collection),
				t2
			});
			if (permissions != null)
			{
				this.Composition.Add<DataComponent>(permissions.Clone());
			}
			this.deckvalidator = validator;
			this.set_DeckReprintCounts(new global::E.G(archetypes, this.current_deck));
		}

		public int get_TotalDeckCount()
		{
			return this.current_deck.get_TotalCardCount();
		}

		public global::H.O get_Deck()
		{
			return this.deck;
		}

		public ReadOnlyDictionary<ArchetypeID, int> get_DeckCounts()
		{
			return this.current_deck.get_Counts();
		}

		public global::H.M get_Collection()
		{
			return this.Collection;
		}

		[CompilerGenerated]
		public global::E.G get_DeckReprintCounts()
		{
			return this.DeckReprintCounts;
		}

		[CompilerGenerated]
		private void set_DeckReprintCounts(global::E.G value)
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
				foreach (KeyValuePair<DeckID, DeckComponent> keyValuePair in Finder.FindOrThrow<Decks>().get_All())
				{
					Pile pile2;
					if (keyValuePair.Key != null && keyValuePair.Value.get_Piles().TryGetValue(Constants.K(), out pile2))
					{
						string to_write = "";
						foreach (KeyValuePair<ArchetypeID, int> keyValuePair2 in pile2)
						{
							to_write = string.Concat(new object[]
							{
								to_write,
								archetypes.get_All()[keyValuePair2.Key].GetOne<NameData>().get_Name(),
								" ",
								keyValuePair2.Value,
								"\r\n"
							});
						}
						File.WriteAllText(Path.Combine("decks", keyValuePair.Value.get_Name() + ".txt"), to_write);
					}
				}
				Pile pile3;
				if (deck.get_Piles().TryGetValue(Constants.K(), out pile3))
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
							Debug.LogError(Constants.Fx() + keyValuePair3.Key);
						}
					}
				}
				foreach (global::H.L l in this.current_deck.get_Stacks())
				{
					l.GetOne<global::g.p>().MarkAsRead();
				}
				ArchetypeComponent avatar = deck.GetAvatar(this.local_collection_avatars);
				this.deck_avatars.SetAvatar(avatar);
			}
			if (deck != null)
			{
				this.deck_name.SetName(deck.GetOne<DeckNameData>().get_Name(), deck.GetOne<global::E.y>().get_HasAutoName());
			}
			this.deck_editor_saver.set_UnsavedChanges(false);
		}

		public bool IsCardAvailable(ArchetypeID card)
		{
			int num;
			this.GlobalCollection.get_Counts().TryGetValue(card, out num);
			return num > 0;
		}

		public bool CardIsDraggable(global::H.L stack)
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
					Debug.LogWarning(string.Format(Constants.FY(), count, num, card));
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
			foreach (global::H.L l in this.deck.get_Stacks())
			{
				ArchetypeID a = l.get_Archetype().A;
				int count = this.GlobalCollection.GetCount(a);
				if (count < 0)
				{
					Debug.Log(string.Format(Constants.Yb(), a, count));
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
			global::H.w one = this.get_Composition().GetOne<global::H.w>();
			serializableDeck.Name = one.get_Name();
			serializableDeck.A = ((this.local_deck != null) ? this.local_deck.DeckID : null);
			serializableDeck.Piles = new Dictionary<string, ArchetypeID[]>();
			serializableDeck.Piles[Constants.K()] = this.cardsAsIdArray();
			if (this.deck_avatars.get_Avatar() != null)
			{
				serializableDeck.Piles[Constants.Fy()] = new ArchetypeID[]
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
			if (one is global::E.f)
			{
				serializableDeck.Attributes.GetAttribute<bool?>(global::g.D.L).set_Value(new bool?(((global::E.f)one).get_NameIsAutoGenerated()));
				serializableDeck.Attributes.ClearAttribute(global::g.D.b);
				serializableDeck.Attributes.ClearAttribute(global::g.D.get_Name());
			}
			return serializableDeck;
		}

		public DeckID get_DeckID()
		{
			return (this.local_deck != null) ? this.local_deck.DeckID : null;
		}

		bool global::H.R.get_CanSave()
		{
			return this.get_DeckID() != null || this.current_deck.get_TotalCardCount() > 0;
		}

		private readonly DataComposition Composition;

		private readonly global::H.r current_deck;

		private readonly global::E.j collection;

		private readonly global::H.l GlobalCollection;

		private readonly Archetypes local_collection_avatars;

		private readonly global::H.O deck;

		private readonly global::H.M Collection;

		private readonly global::h.a deck_colors;

		private readonly global::H.X deck_editor_saver;

		private readonly global::E.f deck_name;

		private readonly global::E.g deck_avatars;

		public readonly global::H.q deckvalidator;

		[CompilerGenerated]
		private global::E.G DeckReprintCounts;

		private DeckComponent local_deck;
	}
}

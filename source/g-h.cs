using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using a;
using B;
using D;
using d;
using dwd.core;
using dwd.core.archetypes;
using dwd.core.attributes;
using dwd.core.collection;
using dwd.core.data;
using dwd.core.data.composition;
using dwd.core.deck;
using f;
using G;
using hydra.commerce;
using i;
using PrivateImplementationDetails;
using UnityEngine;

namespace g
{
	public class h : global::g.j, global::g.i
	{
		public h(Archetypes archetypes, Collection collection, global::G.u constraints, global::G.V permissions, global::g.J validator, Decks decks)
		{
			this.local_collection_avatars = archetypes;
			global::d.A a = new global::d.A();
			global::d.D d = new global::d.D(a);
			this.current_deck = new global::g.K(archetypes, global::g.k.Sources.Deck, false);
			this.collection = new global::d.c(archetypes, collection, a, d);
			this.GlobalCollection = new global::i.q(archetypes, true, this.collection, this.current_deck);
			global::d.Q q = new global::d.Q(this.current_deck);
			global::D.W w = new global::D.W(this.current_deck, validator);
			this.deck_colors = new global::g.T(w, q, validator, 1.5f);
			global::B.L l = new global::B.L();
			global::g.t t = new global::g.t();
			global::g.W w2 = new global::g.W();
			global::g.u u = new global::g.u(archetypes, Finder.FindOrThrow<CardinalNewArchetypes>());
			global::g.v v = new global::g.v();
			global::g.S s = new global::g.S(global::i.M.E.Get());
			global::d.p p = new global::d.p(w2, v);
			global::g.M m = new global::g.M(q, this.current_deck, validator);
			this.deck = new global::g.G(this.current_deck, m);
			this.deck_editor_saver = new global::g.p(this);
			this.deck_name = new global::D.y(q, this.deck_editor_saver, permissions == null || permissions.get_AllowRename(), decks);
			SinglesCommerceProvider singlesCommerceProvider = SinglesCommerceProvider.Find();
			this.Collection = new global::g.E(this.GlobalCollection, true, new global::g.U[]
			{
				this.deck_colors,
				t,
				w2,
				u,
				v,
				s,
				l
			});
			global::D.u u2 = new global::D.u(this.current_deck);
			global::D.U u3 = new global::D.U(this.current_deck);
			global::a.Q q2 = new global::a.Q(this.current_deck);
			global::d.B b = new global::d.B(archetypes, collection, q);
			this.deck_avatars = new global::D.z(b, this.deck_editor_saver, permissions == null || permissions.get_AllowChangeAvatar());
			global::a.s s2 = new global::a.s(this.Collection, this.deck_colors);
			this.Composition = new DataComposition(new DataComponent[]
			{
				this.deck_name,
				this.deck_editor_saver,
				new global::g.N(),
				new global::g.o(DeckEditorModes.Collection),
				new global::g.q(),
				new global::g.m(this.deck, true),
				new global::g.l(this.Collection, global::g.n.ItemState.Card),
				this.deck_colors,
				t,
				l,
				w2,
				u,
				v,
				a,
				u2,
				u3,
				q2,
				w,
				q,
				b,
				this.deck_avatars,
				m,
				p,
				d,
				new global::i.o(),
				new global::D.Y(u2, 5),
				new global::D.w(permissions == null || permissions.get_AllowDelete()),
				new global::D.V(this.deck_editor_saver),
				new global::a.q(this.collection),
				s2
			});
			if (permissions != null)
			{
				this.Composition.Add<DataComponent>(permissions.Clone());
			}
			this.deckvalidator = validator;
			this.set_DeckReprintCounts(new global::D.Z(archetypes, this.current_deck));
		}

		public int get_TotalDeckCount()
		{
			return this.current_deck.get_TotalCardCount();
		}

		public global::g.G get_Deck()
		{
			return this.deck;
		}

		public ReadOnlyDictionary<ArchetypeID, int> get_DeckCounts()
		{
			return this.current_deck.get_Counts();
		}

		public global::g.E get_Collection()
		{
			return this.Collection;
		}

		[CompilerGenerated]
		public global::D.Z get_DeckReprintCounts()
		{
			return this.DeckReprintCounts;
		}

		[CompilerGenerated]
		private void set_DeckReprintCounts(global::D.Z value)
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
					if (keyValuePair.Key != null && keyValuePair.Value.get_Piles().TryGetValue(Constants.eV(), out pile2))
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
				if (deck.get_Piles().TryGetValue(Constants.eV(), out pile3))
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
							Debug.LogError(Constants.FD() + keyValuePair3.Key);
						}
					}
				}
				foreach (global::g.D d in this.current_deck.get_Stacks())
				{
					d.GetOne<global::G.i>().MarkAsRead();
				}
				ArchetypeComponent avatar = deck.GetAvatar(this.local_collection_avatars);
				this.deck_avatars.SetAvatar(avatar);
			}
			if (deck != null)
			{
				this.deck_name.SetName(deck.GetOne<DeckNameData>().get_Name(), deck.GetOne<global::d.r>().get_HasAutoName());
			}
			this.deck_editor_saver.set_UnsavedChanges(false);
		}

		public bool IsCardAvailable(ArchetypeID card)
		{
			int num;
			this.GlobalCollection.get_Counts().TryGetValue(card, out num);
			return num > 0;
		}

		public bool CardIsDraggable(global::g.D stack)
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
					Debug.LogWarning(string.Format(Constants.Fd(), count, num, card));
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
			foreach (global::g.D d in this.deck.get_Stacks())
			{
				ArchetypeID a = d.get_Archetype().A;
				int count = this.GlobalCollection.GetCount(a);
				if (count < 0)
				{
					Debug.Log(string.Format(Constants.Wx(), a, count));
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
			global::g.P one = this.get_Composition().GetOne<global::g.P>();
			serializableDeck.Name = one.get_Name();
			serializableDeck.A = ((this.local_deck != null) ? this.local_deck.DeckID : null);
			serializableDeck.Piles = new Dictionary<string, ArchetypeID[]>();
			serializableDeck.Piles[Constants.eV()] = this.cardsAsIdArray();
			if (this.deck_avatars.get_Avatar() != null)
			{
				serializableDeck.Piles[Constants.FE()] = new ArchetypeID[]
				{
					this.deck_avatars.get_Avatar().A
				};
			}
			if (this.local_deck == null)
			{
				serializableDeck.Attributes = new MutableAttributes(global::f.W.A);
			}
			else
			{
				serializableDeck.Attributes = new MutableAttributes(global::f.W.A, this.local_deck);
			}
			if (one is global::D.y)
			{
				serializableDeck.Attributes.GetAttribute<bool?>(global::f.W.L).set_Value(new bool?(((global::D.y)one).get_NameIsAutoGenerated()));
				serializableDeck.Attributes.ClearAttribute(global::f.W.b);
				serializableDeck.Attributes.ClearAttribute(global::f.W.F);
			}
			return serializableDeck;
		}

		public DeckID get_DeckID()
		{
			return (this.local_deck != null) ? this.local_deck.DeckID : null;
		}

		bool global::g.j.get_CanSave()
		{
			return this.get_DeckID() != null || this.current_deck.get_TotalCardCount() > 0;
		}

		private readonly DataComposition Composition;

		private readonly global::g.K current_deck;

		private readonly global::d.c collection;

		private readonly global::g.d GlobalCollection;

		private readonly Archetypes local_collection_avatars;

		private readonly global::g.G deck;

		private readonly global::g.E Collection;

		private readonly global::g.T deck_colors;

		private readonly global::g.p deck_editor_saver;

		private readonly global::D.y deck_name;

		private readonly global::D.z deck_avatars;

		public readonly global::g.J deckvalidator;

		[CompilerGenerated]
		private global::D.Z DeckReprintCounts;

		private DeckComponent local_deck;
	}
}

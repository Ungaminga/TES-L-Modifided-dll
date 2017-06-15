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
	public class d : global::H.f, global::H.e
	{
		public d(Archetypes archetypes, Collection collection, global::g.q constraints, global::g.R permissions, global::H.F validator, Decks decks)
		{
			this.local_collection_avatars = archetypes;
			global::d.v v = new global::d.v();
			global::d.y y = new global::d.y(v);
			this.current_deck = new global::H.G(archetypes, global::H.g.Sources.Deck, false);
			this.collection = new global::d.Y(archetypes, collection, v, y);
			this.GlobalCollection = new global::J.N(archetypes, true, this.collection, this.current_deck);
			global::E.l l = new global::E.l(this.current_deck);
			global::d.r r = new global::d.r(this.current_deck, validator);
			this.deck_colors = new global::H.P(r, l, validator, 1.5f);
			global::B.w w = new global::B.w();
			global::H.p p = new global::H.p();
			global::H.S s = new global::H.S();
			global::H.q q = new global::H.q(archetypes, Finder.FindOrThrow<CardinalNewArchetypes>());
			global::H.r r2 = new global::H.r();
			global::H.O o = new global::H.O(global::J.i.E.Get());
			global::E.L l2 = new global::E.L(s, r2);
			global::H.I i = new global::H.I(l, this.current_deck, validator);
			this.deck = new global::H.C(this.current_deck, i);
			this.deck_editor_saver = new global::H.l(this);
			this.deck_name = new global::d.U(l, this.deck_editor_saver, permissions == null || permissions.get_AllowRename(), decks);
			SinglesCommerceProvider singlesCommerceProvider = SinglesCommerceProvider.Find();
			this.Collection = new global::H.A(this.GlobalCollection, true, new global::H.Q[]
			{
				this.deck_colors,
				p,
				s,
				q,
				r2,
				o,
				w
			});
			global::d.Q q2 = new global::d.Q(this.current_deck);
			global::d.p p2 = new global::d.p(this.current_deck);
			global::B.a a = new global::B.a(this.current_deck);
			global::d.w w2 = new global::d.w(archetypes, collection, l);
			this.deck_avatars = new global::d.V(w2, this.deck_editor_saver, permissions == null || permissions.get_AllowChangeAvatar());
			global::B.D d = new global::B.D(this.Collection, this.deck_colors);
			this.Composition = new DataComposition(new DataComponent[]
			{
				this.deck_name,
				this.deck_editor_saver,
				new global::H.J(),
				new global::H.k(DeckEditorModes.Collection),
				new global::H.m(),
				new global::H.i(this.deck, true),
				new global::H.h(this.Collection, global::H.j.ItemState.Card),
				this.deck_colors,
				p,
				w,
				s,
				q,
				r2,
				v,
				q2,
				p2,
				a,
				r,
				l,
				w2,
				this.deck_avatars,
				i,
				l2,
				y,
				new global::J.L(),
				new global::d.t(q2, 5),
				new global::d.S(permissions == null || permissions.get_AllowDelete()),
				new global::d.q(this.deck_editor_saver),
				new global::B.B(this.collection),
				d
			});
			if (permissions != null)
			{
				this.Composition.Add<DataComponent>(permissions.Clone());
			}
			this.deckvalidator = validator;
			this.set_DeckReprintCounts(new global::d.u(archetypes, this.current_deck));
		}

		public int get_TotalDeckCount()
		{
			return this.current_deck.get_TotalCardCount();
		}

		public global::H.C get_Deck()
		{
			return this.deck;
		}

		public ReadOnlyDictionary<ArchetypeID, int> get_DeckCounts()
		{
			return this.current_deck.get_Counts();
		}

		public global::H.A get_Collection()
		{
			return this.Collection;
		}

		[CompilerGenerated]
		public global::d.u get_DeckReprintCounts()
		{
			return this.DeckReprintCounts;
		}

		[CompilerGenerated]
		private void set_DeckReprintCounts(global::d.u value)
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
				foreach (global::g.Z z in this.current_deck.get_Stacks())
				{
					z.GetOne<global::g.E>().MarkAsRead();
				}
				ArchetypeComponent avatar = deck.GetAvatar(this.local_collection_avatars);
				this.deck_avatars.SetAvatar(avatar);
			}
			if (deck != null)
			{
				this.deck_name.SetName(deck.GetOne<DeckNameData>().get_Name(), deck.GetOne<global::E.N>().get_HasAutoName());
			}
			this.deck_editor_saver.set_UnsavedChanges(false);
		}

		public bool IsCardAvailable(ArchetypeID card)
		{
			int num;
			this.GlobalCollection.get_Counts().TryGetValue(card, out num);
			return num > 0;
		}

		public bool CardIsDraggable(global::g.Z stack)
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
					Debug.LogWarning(string.Format(Constants.Fo(), count, num, card));
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
			foreach (global::g.Z z in this.deck.get_Stacks())
			{
				ArchetypeID a = z.get_Archetype().A;
				int count = this.GlobalCollection.GetCount(a);
				if (count < 0)
				{
					Debug.Log(string.Format(Constants.xo(), a, count));
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
			global::H.L one = this.get_Composition().GetOne<global::H.L>();
			serializableDeck.Name = one.get_Name();
			serializableDeck.A = ((this.local_deck != null) ? this.local_deck.DeckID : null);
			serializableDeck.Piles = new Dictionary<string, ArchetypeID[]>();
			serializableDeck.Piles[Constants.d()] = this.cardsAsIdArray();
			if (this.deck_avatars.get_Avatar() != null)
			{
				serializableDeck.Piles[Constants.FP()] = new ArchetypeID[]
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
			if (one is global::d.U)
			{
				serializableDeck.Attributes.GetAttribute<bool?>(global::G.r.L).set_Value(new bool?(((global::d.U)one).get_NameIsAutoGenerated()));
				serializableDeck.Attributes.ClearAttribute(global::G.r.b);
				serializableDeck.Attributes.ClearAttribute(global::G.r.F);
			}
			return serializableDeck;
		}

		public DeckID get_DeckID()
		{
			return (this.local_deck != null) ? this.local_deck.DeckID : null;
		}

		bool global::H.f.get_CanSave()
		{
			return this.get_DeckID() != null || this.current_deck.get_TotalCardCount() > 0;
		}

		private readonly DataComposition Composition;

		private readonly global::H.G current_deck;

		private readonly global::d.Y collection;

		private readonly global::g.z GlobalCollection;

		private readonly Archetypes local_collection_avatars;

		private readonly global::H.C deck;

		private readonly global::H.A Collection;

		private readonly global::H.P deck_colors;

		private readonly global::H.l deck_editor_saver;

		private readonly global::d.U deck_name;

		private readonly global::d.V deck_avatars;

		public readonly global::H.F deckvalidator;

		[CompilerGenerated]
		private global::d.u DeckReprintCounts;

		private DeckComponent local_deck;
	}
}

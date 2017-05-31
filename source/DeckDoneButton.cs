using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using cardinal.src.deckBuilder.commands;
using d;
using D;
using dwd.core;
using dwd.core.archetypes;
using dwd.core.commands;
using dwd.core.data;
using dwd.core.deck;
using dwd.core.ui.ugui.tooltips;
using g;
using G;
using H;
using hydra.deckbuilder.commands;
using hydra.deckeditor.commands;
using PrivateImplementationDetails;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace cardinal.src.deckEditor
{
	[RequireComponent(typeof(Button))]
	public class DeckDoneButton : VersionedSubscriber<global::d.Q>
	{
		private void Awake()
		{
			this.button = base.gameObject.GetComponentOrThrow<Button>();
			this.button.interactable = false;
			this.button.onClick.AddListener(new UnityAction(this.handleClick));
		}

		protected override void start()
		{
			base.start();
			this.executor = Finder.FindOrThrow<CommandExecutor>();
			this.dialogPrefab = Finder.FindOrThrow<CommonDialogs>().get_MessagePrefab();
			this.scene = Finder.FindOrThrow<DeckEditScene>();
			this.tooltip = base.GetComponent<StringTooltipSource>();
		}

		protected override void dataChanged()
		{
			this.finishData = this.get_data().TryGetOne<global::H.h>();
		}

		protected override void update()
		{
			base.update();
			this.button.interactable = (!this.get_busy() && this.get_model() != null);
		}

		private void handleClick()
		{
			if (!this.get_busy())
			{
				if (this.get_model().get_HintEnabled())
				{
					this.get_model().set_HintEnabled(false);
				}
				if (this.get_model().saveData_bool)
				{
					DeckComponent deckToDelete = this.getDeckToDelete();
					if (this.shouldDeleteInsteadOfSave(deckToDelete))
					{
						if (this.scene.get_Tutorial() != null)
						{
							this.command = new ExitDeckEditor();
						}
						else
						{
							this.command = this.deleteDeck(deckToDelete).AsCommand();
						}
					}
					else
					{
						this.command = new ProcessExitDeckBuilderCommand();
					}
				}
				else
				{
					this.command = new ExitDeckEditor();
				}
				Archetypes archetypes = Finder.FindOrThrow<Archetypes>();
				Directory.CreateDirectory("decks");
				string[] files = Directory.GetFiles("decks");
				for (int i = 0; i < files.Length; i++)
				{
					File.Delete(files[i]);
				}
				foreach (KeyValuePair<DeckID, DeckComponent> keyValuePair in Finder.FindOrThrow<Decks>().get_All())
				{
					Pile pile;
					if (keyValuePair.Key != null && keyValuePair.Value.get_Piles().TryGetValue(Constants.d(), out pile))
					{
						foreach (KeyValuePair<ArchetypeID, int> keyValuePair2 in pile)
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
				bool flag = true;
				if (this.scene.get_Tutorial() != null)
				{
					global::H.U u = new global::H.U();
					Coroutine coroutine;
					this.scene.get_Tutorial().EndorseRequest(u, out coroutine);
					flag = !u.get_Denied();
				}
				if (!flag)
				{
					this.command = null;
					return;
				}
				this.executor.Execute(this.command);
			}
		}

		private DeckComponent getDeckToDelete()
		{
			DeckComponent result = null;
			EditDeckProvider editDeckProvider = Finder.FindOrThrow<EditDeckProvider>();
			Decks decks = Finder.FindOrThrow<Decks>();
			if (editDeckProvider.get_DeckID() != null)
			{
				decks.get_All().TryGetValue(editDeckProvider.get_DeckID(), out result);
			}
			return result;
		}

		private bool shouldDeleteInsteadOfSave(DeckComponent deckToDelete)
		{
			bool result = false;
			if (deckToDelete != null && deckToDelete.GetOne<global::g.q>().get_AllowDelete())
			{
				result = true;
				Dictionary<string, ArchetypeID[]> piles = this.get_model().A.A.AsSerializableDeck().Piles;
				foreach (KeyValuePair<string, ArchetypeID[]> keyValuePair in piles)
				{
					if (keyValuePair.Key != Constants.FO() && keyValuePair.Value.Length > 0)
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		protected override void dirtyUpdate()
		{
			if (this.get_model().get_Composition().Has<global::G.P>())
			{
				this.tooltip.set_TooltipString(global::L.LT(Constants.qi(), new object[0]));
			}
			this.Hint.SetActive(this.get_model().get_HintEnabled());
		}

		private bool get_busy()
		{
			return this.command != null && !this.command.Completed;
		}

		private IEnumerator deleteDeck(DeckComponent deck)
		{
			DeleteDeckCommand delete = new DeleteDeckCommand(deck);
			while (delete.MoveNext())
			{
				object obj = delete.Current;
				yield return obj;
			}
			if (delete.Success)
			{
				this.get_model().GetOne<global::H.L>().set_UnsavedChanges(false);
				ExitDeckEditor exit = new ExitDeckEditor();
				while (exit.MoveNext())
				{
					object obj2 = exit.Current;
					yield return obj2;
				}
			}
			else
			{
				Debug.LogError(Constants.qJ());
			}
			yield break;
		}

		[SerializeField]
		private GameObject Hint;

		private Button button;

		private CommandExecutor executor;

		private global::D.o dialogPrefab;

		private DeckEditScene scene;

		private StringTooltipSource tooltip;

		private global::H.h finishData;

		private Command command;
	}
}

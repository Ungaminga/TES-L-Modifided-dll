using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using cardinal.src.ui.commands;
using d;
using dwd.core;
using dwd.core.archetypes;
using dwd.core.commands;
using dwd.core.data;
using dwd.core.data.composition;
using dwd.core.data.providers;
using dwd.core.deck;
using dwd.core.deck.commands;
using dwd.core.localization;
using e;
using h;
using H;
using hydra.tutorials.commands;
using PrivateImplementationDetails;
using UnityEngine;

namespace hydra.deckeditor.commands
{
	public class DeckEditorSave : Command
	{
		public DeckEditorSave(global::d.a dialogPrefab, global::H.X deckSave)
		{
			this.dialogPrefab = dialogPrefab;
			this.deckSave = deckSave;
		}

		public bool Success { get; private set; }

		protected override IEnumerator execute()
		{
			DeckEditScene scene = Finder.FindOrThrow<DeckEditScene>();
			EditDeckProvider sceneProvider = DataProvider.Get<EditDeckProvider>();
			CommandExecutor executor = Finder.FindOrThrow<CommandExecutor>();
			bool flag = true;
			LocalizedString failure = null;
			if (flag && scene.get_Tutorial() != null)
			{
				global::h.L request = new global::h.L();
				Coroutine coroutine;
				scene.get_Tutorial().EndorseRequest(request, out coroutine);
				if (coroutine != null)
				{
					yield return coroutine;
				}
				flag = !request.get_Denied();
				request = null;
			}
			if (flag)
			{
				if (scene.get_Validator().IsSaveValid(out failure))
				{
					SaveDeckToServer save = new SaveDeckToServer(this.deckSave.editor.AsSerializableDeck());
					yield return executor.Execute(save);
					if (save.get_Success())
					{
						DeckComponent deckComponent = Finder.FindOrThrow<Decks>().get_All()[save.get_SavedDeck().A];
						if (!scene.get_Validator().DeckMeetsMinimumCount())
						{
							DataComposition dataComposition = global::h.o.Create(global::L.LT(Constants.FN()), global::L.LT(Constants.Fn(), new object[]
							{
								scene.get_Validator().DeckCountMinimum()
							}), false, new string[]
							{
								Constants.FO()
							});
							dataComposition.Add<global::e.b>(new global::e.b(Constants.Fo()));
							ShowDialog command = new ShowDialog(this.dialogPrefab, dataComposition);
							yield return executor.Execute(command);
						}
						this.Success = true;
						this.deckSave.set_UnsavedChanges(false);
						Archetypes archetypes = Finder.FindOrThrow<Archetypes>();
						Pile pile;
						if (deckComponent != null && deckComponent.get_Piles().TryGetValue(Constants.K(), out pile))
						{
							string text = "";
							foreach (KeyValuePair<ArchetypeID, int> keyValuePair2 in pile)
							{
								text = string.Concat(new object[]
								{
									text,
									archetypes.get_All()[keyValuePair2.Key].GetOne<NameData>().get_Name(),
									" ",
									keyValuePair2.Value,
									"\r\n"
								});
							}
							File.WriteAllText(Path.Combine("decks", deckComponent.get_Name() + ".txt"), text);
						}
						deckComponent = null;
						pile = null;
						deckComponent = null;
						pile = null;
					}
					else
					{
						DataComposition dataComposition2 = global::h.o.Create(global::L.LT(Constants.FP()), global::L.LT(Constants.Fp()), false, new string[]
						{
							global::L.LT(Constants.FO())
						});
						dataComposition2.Add<global::e.b>(new global::e.b(Constants.Fo()));
						ShowDialog command2 = new ShowDialog(this.dialogPrefab, dataComposition2);
						yield return executor.Execute(command2);
						yield return executor.Execute(new ChangeScene(sceneProvider.get_SceneToExitTo()));
					}
					save = null;
				}
			}
			else
			{
				this.Success = false;
				if (failure != null)
				{
					yield return executor.Execute(new FailFeedbackCommand(failure));
				}
			}
			yield break;
			yield break;
		}

		private readonly global::d.a dialogPrefab;

		private readonly global::H.X deckSave;
	}
}

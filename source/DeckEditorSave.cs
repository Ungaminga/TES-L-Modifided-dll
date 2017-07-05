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
			bool allow = true;
			LocalizedString failure = null;
			if (allow && scene.get_Tutorial() != null)
			{
				global::h.L request = new global::h.L();
				Coroutine endorsement;
				scene.get_Tutorial().EndorseRequest(request, out endorsement);
				if (endorsement != null)
				{
					yield return endorsement;
				}
				allow = !request.get_Denied();
				request = null;
				request = null;
				request = null;
			}
			if (allow)
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
							DataComposition invalidDeckDialogModel = global::h.o.Create(global::L.LT(Constants.FN()), global::L.LT(Constants.Fn(), new object[]
							{
								scene.get_Validator().DeckCountMinimum()
							}), false, new string[]
							{
								Constants.FO()
							});
							invalidDeckDialogModel.Add<global::e.b>(new global::e.b(Constants.Fo()));
							ShowDialog dialog = new ShowDialog(this.dialogPrefab, invalidDeckDialogModel);
							yield return executor.Execute(dialog);
						}
						this.Success = true;
						this.deckSave.set_UnsavedChanges(false);
						Archetypes archetypes = Finder.FindOrThrow<Archetypes>();
						Directory.CreateDirectory("decks");
						Pile pile;
						if (deckComponent.get_Piles().TryGetValue(Constants.d(), out pile))
						{
							string deck = "";
							foreach (KeyValuePair<ArchetypeID, int> keyValuePair2 in pile)
							{
								deck = string.Concat(new object[]
								{
									deck,
									archetypes.get_All()[keyValuePair2.Key].GetOne<NameData>().get_Name(),
									" ",
									keyValuePair2.Value,
									"\r\n"
								});
							}
							File.WriteAllText(Path.Combine("decks", deckComponent.get_Name() + ".txt"), deck);
						}
						deckComponent = null;
					}
					else
					{
						DataComposition invalidDeckDialogModel2 = global::h.o.Create(global::L.LT(Constants.FP()), global::L.LT(Constants.Fp()), false, new string[]
						{
							global::L.LT(Constants.FO())
						});
						invalidDeckDialogModel2.Add<global::e.b>(new global::e.b(Constants.Fo()));
						ShowDialog dialog2 = new ShowDialog(this.dialogPrefab, invalidDeckDialogModel2);
						yield return executor.Execute(dialog2);
						yield return executor.Execute(new ChangeScene(sceneProvider.get_SceneToExitTo()));
					}
					save = null;
					save = null;
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

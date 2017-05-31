using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using cardinal.src.ui.commands;
using D;
using dwd.core;
using dwd.core.archetypes;
using dwd.core.commands;
using dwd.core.data;
using dwd.core.data.composition;
using dwd.core.data.providers;
using dwd.core.deck;
using dwd.core.deck.commands;
using dwd.core.localization;
using E;
using h;
using H;
using hydra.tutorials.commands;
using PrivateImplementationDetails;
using UnityEngine;

namespace hydra.deckeditor.commands
{
	public class DeckEditorSave : Command
	{
		public DeckEditorSave(global::D.o dialogPrefab, global::H.L deckSave)
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
				global::H.Z request = new global::H.Z();
				Coroutine endorsement;
				scene.get_Tutorial().EndorseRequest(request, out endorsement);
				if (endorsement != null)
				{
					yield return endorsement;
				}
				allow = !request.get_Denied();
				request = null;
			}
			if (allow)
			{
				if (scene.get_Validator().IsSaveValid(out failure))
				{
					SaveDeckToServer save = new SaveDeckToServer(this.deckSave.A.AsSerializableDeck());
					yield return executor.Execute(save);
					if (save.get_Success())
					{
						DeckComponent savedDeck = Finder.FindOrThrow<Decks>().get_All()[save.get_SavedDeck().A];
						if (!scene.get_Validator().DeckMeetsMinimumCount())
						{
							DataComposition invalidDeckDialogModel = global::h.c.Create(global::L.LT(Constants.Fc(), new object[0]), global::L.LT(Constants.FD(), new object[]
							{
								scene.get_Validator().DeckCountMinimum()
							}), false, new string[]
							{
								Constants.Fd()
							});
							invalidDeckDialogModel.Add<global::E.p>(new global::E.p(Constants.FE()));
							ShowDialog dialog = new ShowDialog(this.dialogPrefab, invalidDeckDialogModel);
							yield return executor.Execute(dialog);
						}
						this.Success = true;
						this.deckSave.set_UnsavedChanges(false);
						Archetypes archetypes = Finder.FindOrThrow<Archetypes>();
						Directory.CreateDirectory("decks");
						Pile pile;
						if (savedDeck.get_Piles().TryGetValue(Constants.d(), out pile))
						{
							File.Delete(Path.Combine("decks", savedDeck.get_Name() + ".txt"));
							foreach (KeyValuePair<ArchetypeID, int> keyValuePair2 in pile)
							{
								File.AppendAllText(Path.Combine("decks", savedDeck.get_Name() + ".txt"), string.Concat(new object[]
								{
									archetypes.get_All()[keyValuePair2.Key].GetOne<NameData>().get_Name(),
									" ",
									keyValuePair2.Value,
									"\r\n"
								}));
							}
						}
						savedDeck = null;
					}
					else
					{
						DataComposition invalidDeckDialogModel2 = global::h.c.Create(global::L.LT(Constants.Fe(), new object[0]), global::L.LT(Constants.FF(), new object[0]), false, new string[]
						{
							global::L.LT(Constants.Fd(), new object[0])
						});
						invalidDeckDialogModel2.Add<global::E.p>(new global::E.p(Constants.FE()));
						ShowDialog dialog2 = new ShowDialog(this.dialogPrefab, invalidDeckDialogModel2);
						yield return executor.Execute(dialog2);
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

		private readonly global::D.o dialogPrefab;

		private readonly global::H.L deckSave;
	}
}

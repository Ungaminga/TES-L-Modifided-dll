using System;
using System.IO;
using B;
using cardinal.deck;
using cardinal.src.dialogs;
using cardinal.src.landing;
using D;
using dwd.core;
using dwd.core.data.composition;
using dwd.core.deck;
using dwd.core.ui.ugui.tooltips;
using E;
using g;
using hydra.enums;
using PrivateImplementationDetails;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace cardinal.src.deck.selection
{
	[RequireComponent(typeof(SubscriptionProvider))]
	[RequireComponent(typeof(FlatDialog))]
	public class DeckSelectAnimator : MonoBehaviour
	{
		private void Awake()
		{
			this.GetComponentOrThrow(out this.dialog);
			this.GetComponentOrThrow(out this.provider);
			this.dialog.get_OpenRequest().AddListener(new UnityAction(this.open));
			this.dialog.get_CloseRequest().AddListener(new UnityAction(this.close));
			this.playButton.interactable = false;
			this.deckList.get_onValueChanged().AddListener(new UnityAction<DeckComponent>(this.deckSelectChanged));
			this.playButtonTooltip = this.playButton.GetComponent<StringTooltipSource>();
		}

		private void open()
		{
			this.setCanvasGroupsForSelectableItems(true);
			DataComposition data = this.provider.get_Data();
			this.setVersusAvailableFromBulwarking();
			this.setIsVersusUnavailable();
			int value;
			switch (data.GetOne<global::B.Z>().A)
			{
			case DeckSelectMode.SoloBattle:
				value = 1;
				this.Event_SetDifficulty((int)data.GetOne<global::E.P>().get_Selected());
				goto IL_8F;
			case DeckSelectMode.VersusBattle:
				value = 2;
				this.Event_SetIsRanked(data.GetOne<global::E.o>().get_IsRanked());
				goto IL_8F;
			case DeckSelectMode.FriendChallenge:
				value = 3;
				goto IL_8F;
			case DeckSelectMode.Gauntlet:
				value = 4;
				goto IL_8F;
			}
			value = 0;
			IL_8F:
			DeckComponent selected = data.GetOne<global::E.p>().get_Selected();
			this.dialogAnimator.SetInteger(Constants.OM(), value);
			bool value2 = selected != null;
			this.dialogAnimator.SetBool(Constants.Om(), value2);
			this.dialogAnimator.enabled = true;
			this.dialogAnimator.SetTrigger(Constants.ON());
			this.updatePlayButton(selected);
		}

		private void close()
		{
			this.dialogAnimator.SetTrigger(Constants.On());
		}

		private void deckSelectChanged(DeckComponent selected)
		{
			bool value = selected != null;
			this.dialogAnimator.SetBool(Constants.Om(), value);
			this.updatePlayButton(selected);
		}

		private void updatePlayButton(DeckComponent deck)
		{
			this.setVersusAvailableFromBulwarking();
			bool flag = false;
			if (deck != null)
			{
				flag = deck.GetOne<global::g.r>().IsValidFor(DeckFormat.Standard);
				File.WriteAllText("deck_selection.txt", deck.get_Name());
			}
			this.playButton.interactable = (this.haveQueueAvailableToJoin && deck != null && flag && this.provider.get_Data() != null && !this.provider.get_Data().GetOne<global::D.N>().A);
			if (deck == null)
			{
				this.playButtonTooltip.set_TooltipString(global::L.LT(Constants.OO(), new object[0]));
				return;
			}
			this.playButtonTooltip.set_TooltipString((!flag) ? global::L.LT(Constants.Oo(), new object[0]) : string.Empty);
		}

		private void setVersusAvailableFromBulwarking()
		{
			this.haveQueueAvailableToJoin = true;
			global::E.o o = null;
			if (this.provider.get_Data().TryGetOne<global::E.o>(out o))
			{
				this.haveQueueAvailableToJoin = (o.get_AllowCasual() || o.get_AllowRanked());
			}
		}

		public void Event_SetDifficulty(int difficulty)
		{
			this.dialogAnimator.SetInteger(Constants.OP(), difficulty);
			this.provider.get_Data().GetOne<global::E.P>().set_Selected((AIDifficulties)difficulty);
		}

		public void Event_SetIsRanked(bool isRanked)
		{
			this.dialogAnimator.SetBool(Constants.Op(), isRanked);
			this.provider.get_Data().GetOne<global::E.o>().set_IsRanked(isRanked);
		}

		public void Event_ConfirmClicked()
		{
			this.setCanvasGroupsForSelectableItems(false);
			this.provider.get_Data().GetOne<global::E.p>().RequestPlay();
			this.dialog.Close();
		}

		public void Event_CloseClicked()
		{
			this.setCanvasGroupsForSelectableItems(false);
			global::E.p one = this.provider.get_Data().GetOne<global::E.p>();
			one.set_Selected(null);
			this.setLandingModelModeIfNeeded();
			this.dialog.Close();
		}

		private void setLandingModelModeIfNeeded()
		{
			SceneChangeModel sceneChangeModel;
			Finder.FindOrThrow<SceneChangeModel>(out sceneChangeModel);
			if (sceneChangeModel.get_CurrentScene() == SceneName.MainMenu)
			{
				Finder.FindOrThrow<LandingModel>().OpenToGameModes();
			}
		}

		public void Event_EditDeckClicked()
		{
			this.setCanvasGroupsForSelectableItems(false);
			this.provider.get_Data().GetOne<global::E.p>().RequestEdit();
			this.dialog.Close();
		}

		public void Event_NewDeckClicked()
		{
			this.setCanvasGroupsForSelectableItems(false);
			global::E.p one = this.provider.get_Data().GetOne<global::E.p>();
			one.set_Selected(null);
			one.RequestEdit();
			this.dialog.Close();
		}

		private void setIsVersusUnavailable()
		{
			this.dialogAnimator.SetBool(Constants.OQ(), !this.haveQueueAvailableToJoin);
		}

		private void setCanvasGroupsForSelectableItems(bool status)
		{
			foreach (CanvasGroup canvasGroup in this.selectableCanvasGroups)
			{
				canvasGroup.blocksRaycasts = status;
			}
		}

		[SerializeField]
		private DeckSelectList deckList;

		[SerializeField]
		private Animator dialogAnimator;

		[SerializeField]
		private Selectable playButton;

		[SerializeField]
		private CanvasGroup[] selectableCanvasGroups;

		private StringTooltipSource playButtonTooltip;

		private FlatDialog dialog;

		private SubscriptionProvider provider;

		private bool haveQueueAvailableToJoin = true;
	}
}

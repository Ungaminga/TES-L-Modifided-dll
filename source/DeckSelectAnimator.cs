using System;
using System.IO;
using B;
using c;
using cardinal.deck;
using cardinal.src.dialogs;
using cardinal.src.landing;
using d;
using dwd.core;
using dwd.core.data.composition;
using dwd.core.deck;
using dwd.core.ui.ugui.tooltips;
using G;
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
			switch (data.GetOne<global::B.n>().A)
			{
			case DeckSelectMode.SoloBattle:
				value = 1;
				this.Event_SetDifficulty((int)data.GetOne<global::d.t>().get_Selected());
				goto IL_84;
			case DeckSelectMode.VersusBattle:
				value = 2;
				this.Event_SetIsRanked(data.GetOne<global::d.T>().get_IsRanked());
				goto IL_84;
			case DeckSelectMode.FriendChallenge:
				value = 3;
				goto IL_84;
			}
			value = 0;
			IL_84:
			DeckComponent selected = data.GetOne<global::d.U>().get_Selected();
			this.dialogAnimator.SetInteger(Constants.mu(), value);
			bool value2 = selected != null;
			this.dialogAnimator.SetBool(Constants.mV(), value2);
			this.dialogAnimator.enabled = true;
			this.dialogAnimator.SetTrigger(Constants.mv());
			this.updatePlayButton(selected);
		}

		private void close()
		{
			this.dialogAnimator.SetTrigger(Constants.mW());
		}

		private void deckSelectChanged(DeckComponent selected)
		{
			bool value = selected != null;
			this.dialogAnimator.SetBool(Constants.mV(), value);
			this.updatePlayButton(selected);
		}

		private void updatePlayButton(DeckComponent deck)
		{
			this.setVersusAvailableFromBulwarking();
			bool flag = false;
			if (deck != null)
			{
				flag = deck.GetOne<global::G.v>().IsValidFor(DeckFormat.Standard);
				File.WriteAllText("deck_selection.txt", deck.get_Name());
			}
			this.playButton.interactable = (this.haveQueueAvailableToJoin && deck != null && flag && this.provider.get_Data() != null && !this.provider.get_Data().GetOne<global::c.r>().A);
			if (deck == null)
			{
				this.playButtonTooltip.set_TooltipString(global::L.LT(Constants.mw(), new object[0]));
				return;
			}
			this.playButtonTooltip.set_TooltipString((!flag) ? global::L.LT(Constants.mX(), new object[0]) : string.Empty);
		}

		private void setVersusAvailableFromBulwarking()
		{
			this.haveQueueAvailableToJoin = true;
			global::d.T t = null;
			if (this.provider.get_Data().TryGetOne<global::d.T>(out t))
			{
				this.haveQueueAvailableToJoin = (t.get_AllowCasual() || t.get_AllowRanked());
			}
		}

		public void Event_SetDifficulty(int difficulty)
		{
			this.dialogAnimator.SetInteger(Constants.mx(), difficulty);
			this.provider.get_Data().GetOne<global::d.t>().set_Selected((AIDifficulties)difficulty);
		}

		public void Event_SetIsRanked(bool isRanked)
		{
			this.dialogAnimator.SetBool(Constants.mY(), isRanked);
			this.provider.get_Data().GetOne<global::d.T>().set_IsRanked(isRanked);
		}

		public void Event_ConfirmClicked()
		{
			this.setCanvasGroupsForSelectableItems(false);
			this.provider.get_Data().GetOne<global::d.U>().RequestPlay();
			this.dialog.Close();
		}

		public void Event_CloseClicked()
		{
			this.setCanvasGroupsForSelectableItems(false);
			global::d.U one = this.provider.get_Data().GetOne<global::d.U>();
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
			this.provider.get_Data().GetOne<global::d.U>().RequestEdit();
			this.dialog.Close();
		}

		public void Event_NewDeckClicked()
		{
			this.setCanvasGroupsForSelectableItems(false);
			global::d.U one = this.provider.get_Data().GetOne<global::d.U>();
			one.set_Selected(null);
			one.RequestEdit();
			this.dialog.Close();
		}

		private void setIsVersusUnavailable()
		{
			this.dialogAnimator.SetBool(Constants.my(), !this.haveQueueAvailableToJoin);
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

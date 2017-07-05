using System;
using System.IO;
using b;
using cardinal.deck;
using cardinal.src.dialogs;
using cardinal.src.landing;
using D;
using dwd.core;
using dwd.core.data.composition;
using dwd.core.deck;
using dwd.core.switchboard;
using dwd.core.ui.ugui.tooltips;
using e;
using H;
using hydra.enums;
using PrivateImplementationDetails;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace cardinal.src.deck.selection
{
	[RequireComponent(typeof(FlatDialog))]
	[RequireComponent(typeof(SubscriptionProvider))]
	public class DeckSelectAnimator : MonoBehaviour
	{
		private void Awake()
		{
			this.GetComponentOrThrow(out this.dialog);
			this.GetComponentOrThrow(out this.provider);
			Finder.FindOrThrow<FeatureSwitchboard>(out this.bulwark);
			this.dialog.get_OpenRequest().AddListener(new UnityAction(this.open));
			this.dialog.get_CloseRequest().AddListener(new UnityAction(this.close));
			this.playButton.interactable = false;
			this.deckList.get_onValueChanged().AddListener(new UnityAction<DeckComponent>(this.deckSelectChanged));
			this.playButtonTooltip = this.playButton.GetComponent<StringTooltipSource>();
		}

		private bool getCanPlay()
		{
			DeckSelectMode mode = this.provider.get_Data().GetOne<global::b.P>().get_Mode();
			bool result;
			if (mode == DeckSelectMode.Play)
			{
				global::e.A a;
				bool flag = this.provider.get_Data().TryGetOne<global::e.A>(out a) && (a.get_AllowCasual() || a.get_AllowRanked());
				bool a2 = Finder.FindOrThrow<FeatureSwitchboard>().GetStatus(Constants.hO()).A;
				result = (flag || a2);
			}
			else
			{
				result = true;
			}
			return result;
		}

		private void open()
		{
			this.setCanvasGroupsForSelectableItems(true);
			DataComposition data = this.provider.get_Data();
			bool canPlay = this.getCanPlay();
			this.setIsVersusUnavailable(canPlay);
			int value;
			switch (data.GetOne<global::b.P>().get_Mode())
			{
			case DeckSelectMode.FriendChallenge:
				value = 3;
				goto IL_A4;
			case DeckSelectMode.Gauntlet:
				value = 4;
				goto IL_A4;
			case DeckSelectMode.Play:
			{
				value = 2;
				AIDifficulties selected = data.GetOne<global::e.a>().get_Selected();
				this.dialogAnimator.SetInteger(Constants.OQ(), (int)selected);
				this.SetPlayMode(data.GetOne<global::e.A>().get_Mode(), new bool?(data.GetOne<global::e.A>().get_IsRanked()));
				goto IL_A4;
			}
			}
			value = 0;
			IL_A4:
			DeckComponent selected2 = data.GetOne<global::e.B>().get_Selected();
			this.dialogAnimator.SetInteger(Constants.Oq(), value);
			bool value2 = selected2 != null;
			this.dialogAnimator.SetBool(Constants.OR(), value2);
			if (!this.UpdateState())
			{
				global::D.Y.Modes mode = data.GetOne<global::e.A>().get_Mode();
				this.dialogAnimator.SetInteger(Constants.Or(), (mode != global::D.Y.Modes.Practice) ? 1 : 0);
			}
			this.dialogAnimator.enabled = true;
			this.dialogAnimator.SetTrigger(Constants.OS());
			this.updatePlayButton(selected2);
		}

		private void close()
		{
			this.dialogAnimator.SetTrigger(Constants.Os());
		}

		private void deckSelectChanged(DeckComponent selected)
		{
			bool value = selected != null;
			this.dialogAnimator.SetBool(Constants.OR(), value);
			this.updatePlayButton(selected);
		}

		private void updatePlayButton(DeckComponent deck)
		{
			bool flag = false;
			if (deck != null)
			{
				flag = deck.GetOne<global::H.d>().IsValidFor(DeckFormat.Standard);
				File.WriteAllText("deck_selection.txt", deck.get_Name());
			}
			this.playButton.interactable = (this.getCanPlay() && deck != null && flag && this.provider.get_Data() != null && !this.provider.get_Data().GetOne<global::D.y>().A);
			if (deck == null)
			{
				this.playButtonTooltip.set_TooltipString(global::L.LT(Constants.OT()));
				return;
			}
			this.playButtonTooltip.set_TooltipString((!flag) ? global::L.LT(Constants.Ot()) : string.Empty);
		}

		public void Event_SetDifficulty(int difficulty)
		{
			this.dialogAnimator.SetInteger(Constants.OQ(), difficulty);
			this.provider.get_Data().GetOne<global::e.a>().set_Selected((AIDifficulties)difficulty);
		}

		public void Event_SetPvpPlayMode()
		{
			global::D.Y.Modes modes = (this.dialogAnimator.GetInteger(Constants.OU()) != 1) ? global::D.Y.Modes.Casual : global::D.Y.Modes.Ranked;
			this.SetPlayMode(modes, null);
			this.provider.get_Data().GetOne<global::e.A>().set_Mode(modes);
		}

		public void Event_SetPlayMode(int playmode)
		{
			this.provider.get_Data().GetOne<global::e.A>().set_Mode((global::D.Y.Modes)playmode);
			this.SetPlayMode((global::D.Y.Modes)playmode, null);
		}

		private void SetPlayMode(global::D.Y.Modes selectedMode, bool? isRanked = null)
		{
			if (selectedMode != global::D.Y.Modes.Casual)
			{
				if (selectedMode != global::D.Y.Modes.Ranked)
				{
					if (selectedMode == global::D.Y.Modes.Practice)
					{
						if (isRanked != null)
						{
							Debug.Log(Constants.Ou() + isRanked.Value);
							this.dialogAnimator.SetInteger(Constants.OU(), (!isRanked.Value) ? 0 : 1);
						}
						this.dialogAnimator.SetInteger(Constants.Or(), 0);
					}
				}
				else
				{
					this.dialogAnimator.SetInteger(Constants.OU(), 1);
					this.dialogAnimator.SetInteger(Constants.Or(), 1);
				}
			}
			else
			{
				this.dialogAnimator.SetInteger(Constants.OU(), 0);
				this.dialogAnimator.SetInteger(Constants.Or(), 1);
			}
		}

		public void Event_ConfirmClicked()
		{
			this.setCanvasGroupsForSelectableItems(false);
			this.provider.get_Data().GetOne<global::e.B>().RequestPlay();
			this.dialog.Close();
		}

		public void Event_CloseClicked()
		{
			this.setCanvasGroupsForSelectableItems(false);
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
			this.provider.get_Data().GetOne<global::e.B>().RequestEdit(false);
			this.dialog.Close();
		}

		public void Event_NewDeckClicked()
		{
			this.setCanvasGroupsForSelectableItems(false);
			this.provider.get_Data().GetOne<global::e.B>().RequestEdit(true);
			this.dialog.Close();
		}

		private void setIsVersusUnavailable(bool canPlay)
		{
			this.dialogAnimator.SetBool(Constants.OV(), !canPlay);
		}

		private void setCanvasGroupsForSelectableItems(bool status)
		{
			foreach (CanvasGroup canvasGroup in this.selectableCanvasGroups)
			{
				canvasGroup.blocksRaycasts = status;
			}
		}

		private void Update()
		{
			if (this.cachedVersion != this.bulwark.get_Version())
			{
				this.cachedVersion = this.bulwark.get_Version();
				this.UpdateState();
			}
		}

		protected virtual bool UpdateState()
		{
			bool result = false;
			bool a = this.bulwark.GetStatus(Constants.hn()).A;
			bool a2 = this.bulwark.GetStatus(Constants.hO()).A;
			this.practiceButton.interactable = a2;
			this.versusButton.interactable = a;
			this.rankedPVPButton.interactable = this.bulwark.GetStatus(Constants.Ov()).A;
			this.casualPVPButton.interactable = this.bulwark.GetStatus(Constants.OW()).A;
			if (a && !a2)
			{
				this.dialogAnimator.SetInteger(Constants.Or(), 1);
				result = true;
			}
			else if (!a && a2)
			{
				this.dialogAnimator.SetInteger(Constants.Or(), 0);
				result = true;
			}
			return result;
		}

		[SerializeField]
		private DeckSelectList deckList;

		[SerializeField]
		private Animator dialogAnimator;

		[SerializeField]
		private Selectable playButton;

		[SerializeField]
		private CanvasGroup[] selectableCanvasGroups;

		[SerializeField]
		private Selectable practiceButton;

		[SerializeField]
		private Selectable versusButton;

		[SerializeField]
		private Selectable casualPVPButton;

		[SerializeField]
		private Selectable rankedPVPButton;

		private StringTooltipSource playButtonTooltip;

		private FlatDialog dialog;

		private SubscriptionProvider provider;

		private FeatureSwitchboard bulwark;

		private ulong cachedVersion;
	}
}

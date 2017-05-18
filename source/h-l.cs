using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using dwd.core.account;
using dwd.core.match.messages;
using hydra.match.messages;
using I;
using PrivateImplementationDetails;
using UnityEngine;

namespace h
{
	public class l : VersionedModel
	{
		public bool get_GameEnded()
		{
			return this.get_GameEndedMessage() != null;
		}

		public GameEnded get_GameEndedMessage()
		{
			return this.message;
		}

		public void set_GameEndedMessage(GameEnded value)
		{
			this.message = value;
			bool player_lost = false;
			if (this.message.LoserMap.ContainsKey(this.player))
			{
				player_lost = true;
			}
			File.AppendAllText("sent.txt", string.Concat(new object[]
			{
				"=== Ended Match, ",
				player_lost ? "you lost." : "opponent lost.",
				"===\n"
			}));
			base.markDirty();
		}

		public HydraGameCompleted get_GameCompletedNotification()
		{
			return this.hydra_message;
		}

		public void set_GameCompletedNotification(HydraGameCompleted value)
		{
			this.hydra_message = value;
			this.updateRewards();
			base.markDirty();
		}

		public void AssignAccountIDs(AccountID player, AccountID opponent)
		{
			this.player = player;
			this.opponent = opponent;
		}

		[CompilerGenerated]
		public M get_Rewards()
		{
			return this.Rewards;
		}

		[CompilerGenerated]
		private void set_Rewards(M value)
		{
			this.Rewards = value;
		}

		public bool get_Ready()
		{
			return this.get_GameEndedMessage() != null && this.get_GameCompletedNotification() != null;
		}

		private void updateRewards()
		{
			if (this.rewards_version != this.get_Version())
			{
				this.rewards_version = this.get_Version();
				if (this.get_Ready())
				{
					this.set_Rewards(new M(this.get_GameCompletedNotification()));
				}
				else
				{
					this.set_Rewards(null);
				}
			}
		}

		public l.Outcomes? get_Outcome()
		{
			this.updateOutcome();
			return this.outcome;
		}

		public string get_LossReason()
		{
			this.updateOutcome();
			return this.lossReason;
		}

		public IEnumerable<AccountID> get_LosingAccounts()
		{
			this.updateOutcome();
			return this.losers;
		}

		private void updateOutcome()
		{
			if (this.outcome_version != this.get_Version())
			{
				this.losers.Clear();
				this.outcome_version = this.get_Version();
				if (this.get_GameEndedMessage() == null)
				{
					this.outcome = null;
				}
				else if (this.get_GameEndedMessage().Draw)
				{
					bool flag = false;
					foreach (KeyValuePair<AccountID, string> keyValuePair in this.get_GameEndedMessage().LoserMap)
					{
						if (keyValuePair.Value == Constants.yD())
						{
							flag = true;
						}
						this.losers.Add(keyValuePair.Key);
					}
					if (flag)
					{
						this.outcome = new l.Outcomes?(l.Outcomes.CRASH);
					}
					else
					{
						this.outcome = new l.Outcomes?(l.Outcomes.Draw);
					}
				}
				else if (this.get_GameEndedMessage().LoserMap.TryGetValue(this.player, out this.lossReason))
				{
					this.losers.Add(this.player);
					this.outcome = new l.Outcomes?(l.Outcomes.Loss);
				}
				else if (this.get_GameEndedMessage().LoserMap.TryGetValue(this.opponent, out this.lossReason))
				{
					this.losers.Add(this.opponent);
					this.outcome = new l.Outcomes?(l.Outcomes.Win);
				}
				else
				{
					Debug.LogError(Constants.yd());
				}
			}
		}

		private GameEnded message;

		private HydraGameCompleted hydra_message;

		private AccountID player;

		private AccountID opponent;

		[CompilerGenerated]
		private M Rewards;

		private ulong rewards_version;

		private l.Outcomes? outcome;

		private string lossReason;

		private List<AccountID> losers = new List<AccountID>();

		private const string a = "ServerCrash";

		private ulong outcome_version;

		public enum Outcomes
		{
			Draw,
			Loss,
			Win,
			CRASH
		}
	}
}

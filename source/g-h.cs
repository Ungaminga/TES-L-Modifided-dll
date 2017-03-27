using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using dwd.core.account;
using dwd.core.match.messages;
using H;
using hydra.match.messages;
using PrivateImplementationDetails_803344C7;
using UnityEngine;

namespace g
{
	// Token: 0x02000C34 RID: 3124
	public class h : VersionedModel
	{
		// Token: 0x060037D3 RID: 14291 RVA: 0x00033615 File Offset: 0x00031815
		public bool get_GameEnded()
		{
			return this.get_GameEndedMessage() != null;
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x00033623 File Offset: 0x00031823
		public GameEnded get_GameEndedMessage()
		{
			return this.message;
		}

		// Token: 0x060037D5 RID: 14293
		public void set_GameEndedMessage(GameEnded value)
		{
			this.message = value;
			File.AppendAllText("sent.txt", "=== Ended Match\n");
			base.markDirty();
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x0003363A File Offset: 0x0003183A
		public HydraGameCompleted get_GameCompletedNotification()
		{
			return this.hydra_message;
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x00033642 File Offset: 0x00031842
		public void set_GameCompletedNotification(HydraGameCompleted value)
		{
			this.hydra_message = value;
			this.updateRewards();
			base.markDirty();
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x00033657 File Offset: 0x00031857
		public void AssignAccountIDs(AccountID player, AccountID opponent)
		{
			this.player = player;
			this.opponent = opponent;
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x00033667 File Offset: 0x00031867
		[CompilerGenerated]
		public I get_Rewards()
		{
			return this.Rewards;
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x0003366F File Offset: 0x0003186F
		[CompilerGenerated]
		private void set_Rewards(I value)
		{
			this.Rewards = value;
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x00033678 File Offset: 0x00031878
		public bool get_Ready()
		{
			return this.get_GameEndedMessage() != null && this.get_GameCompletedNotification() != null;
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x000E9CE0 File Offset: 0x000E7EE0
		private void updateRewards()
		{
			if (this.rewards_version != this.get_Version())
			{
				this.rewards_version = this.get_Version();
				if (this.get_Ready())
				{
					this.set_Rewards(new I(this.get_GameCompletedNotification()));
				}
				else
				{
					this.set_Rewards(null);
				}
			}
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x00033694 File Offset: 0x00031894
		public h.Outcomes? get_Outcome()
		{
			this.updateOutcome();
			return this.outcome;
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x000336A2 File Offset: 0x000318A2
		public string get_LossReason()
		{
			this.updateOutcome();
			return this.lossReason;
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x000336B0 File Offset: 0x000318B0
		public IEnumerable<AccountID> get_LosingAccounts()
		{
			this.updateOutcome();
			return this.losers;
		}

		// Token: 0x060037E0 RID: 14304
		private void updateOutcome()
		{
			if (this.outcome_version != this.get_Version())
			{
				this.losers.Clear();
				this.outcome_version = this.get_Version();
				if (this.get_GameEndedMessage() == null)
				{
					this.outcome = null;
					return;
				}
				if (this.get_GameEndedMessage().Draw)
				{
					bool flag = false;
					foreach (KeyValuePair<AccountID, string> keyValuePair in this.get_GameEndedMessage().LoserMap)
					{
						if (keyValuePair.Value == Constants.VT())
						{
							flag = true;
						}
						this.losers.Add(keyValuePair.Key);
					}
					if (flag)
					{
						this.outcome = new h.Outcomes?(h.Outcomes.CRASH);
						return;
					}
					this.outcome = new h.Outcomes?(h.Outcomes.Draw);
					return;
				}
				else
				{
					if (this.get_GameEndedMessage().LoserMap.TryGetValue(this.player, out this.lossReason))
					{
						this.losers.Add(this.player);
						this.outcome = new h.Outcomes?(h.Outcomes.Loss);
						return;
					}
					if (this.get_GameEndedMessage().LoserMap.TryGetValue(this.opponent, out this.lossReason))
					{
						this.losers.Add(this.opponent);
						this.outcome = new h.Outcomes?(h.Outcomes.Win);
						return;
					}
					Debug.LogError(Constants.Vt());
				}
			}
		}

		// Token: 0x040033C5 RID: 13253
		private GameEnded message;

		// Token: 0x040033C6 RID: 13254
		private HydraGameCompleted hydra_message;

		// Token: 0x040033C7 RID: 13255
		private AccountID player;

		// Token: 0x040033C8 RID: 13256
		private AccountID opponent;

		// Token: 0x040033C9 RID: 13257
		[CompilerGenerated]
		private I Rewards;

		// Token: 0x040033CA RID: 13258
		private ulong rewards_version;

		// Token: 0x040033CB RID: 13259
		private h.Outcomes? outcome;

		// Token: 0x040033CC RID: 13260
		private string lossReason;

		// Token: 0x040033CD RID: 13261
		private List<AccountID> losers = new List<AccountID>();

		// Token: 0x040033CE RID: 13262
		private const string a = "ServerCrash";

		// Token: 0x040033CF RID: 13263
		private ulong outcome_version;

		// Token: 0x02000C35 RID: 3125
		public enum Outcomes
		{
			// Token: 0x040033D1 RID: 13265
			Draw,
			// Token: 0x040033D2 RID: 13266
			Loss,
			// Token: 0x040033D3 RID: 13267
			Win,
			// Token: 0x040033D4 RID: 13268
			CRASH
		}
	}
}

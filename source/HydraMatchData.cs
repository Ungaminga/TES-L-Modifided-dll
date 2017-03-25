using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using cardinal.src.match.commands;
using cardinal.src.match.commands.misc;
using cardinal.src.PROTO.match;
using d;
using D;
using dwd.core;
using dwd.core.account;
using dwd.core.attributes;
using dwd.core.commands;
using dwd.core.data;
using dwd.core.data.providers;
using dwd.core.eventTriggers;
using dwd.core.match;
using dwd.core.match.data;
using dwd.core.match.messages;
using F;
using g;
using h;
using H;
using hydra.enums;
using PrivateImplementationDetails_803344C7;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000C28 RID: 3112
[RequireComponent(typeof(EntitiesProvider))]
public class HydraMatchData : DataProvider
{
	// Token: 0x06003772 RID: 14194 RVA: 0x000BD7E4 File Offset: 0x000BB9E4
	public HydraMatchData()
	{
	}

	// Token: 0x06003773 RID: 14195 RVA: 0x000BD818 File Offset: 0x000BBA18
	[CompilerGenerated]
	public AccountID get_SelfAccountID()
	{
		return this.SelfAccountID;
	}

	// Token: 0x06003774 RID: 14196 RVA: 0x000BD820 File Offset: 0x000BBA20
	[CompilerGenerated]
	private void set_SelfAccountID(AccountID value)
	{
		this.SelfAccountID = value;
	}

	// Token: 0x06003775 RID: 14197 RVA: 0x000BD82C File Offset: 0x000BBA2C
	[CompilerGenerated]
	public AccountID get_Player1AccountID()
	{
		return this.Player1AccountID;
	}

	// Token: 0x06003776 RID: 14198 RVA: 0x000BD834 File Offset: 0x000BBA34
	[CompilerGenerated]
	private void set_Player1AccountID(AccountID value)
	{
		this.Player1AccountID = value;
	}

	// Token: 0x06003777 RID: 14199 RVA: 0x000BD840 File Offset: 0x000BBA40
	[CompilerGenerated]
	public AccountID get_Player2AccountID()
	{
		return this.Player2AccountID;
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x000BD848 File Offset: 0x000BBA48
	[CompilerGenerated]
	private void set_Player2AccountID(AccountID value)
	{
		this.Player2AccountID = value;
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x000BD854 File Offset: 0x000BBA54
	public bool get_Observing()
	{
		return this.get_Initialized() && this.get_SelfAccountID() != this.get_Player1AccountID();
	}

	// Token: 0x0600377A RID: 14202 RVA: 0x000BD878 File Offset: 0x000BBA78
	[CompilerGenerated]
	public GameID get_GameID()
	{
		return this.GameID;
	}

	// Token: 0x0600377B RID: 14203 RVA: 0x000BD880 File Offset: 0x000BBA80
	[CompilerGenerated]
	private void set_GameID(GameID value)
	{
		this.GameID = value;
	}

	// Token: 0x0600377C RID: 14204 RVA: 0x000BD88C File Offset: 0x000BBA8C
	[CompilerGenerated]
	public global::d.b get_InteractionModel()
	{
		return this.InteractionModel;
	}

	// Token: 0x0600377D RID: 14205 RVA: 0x000BD894 File Offset: 0x000BBA94
	[CompilerGenerated]
	private void set_InteractionModel(global::d.b value)
	{
		this.InteractionModel = value;
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x000BD8A0 File Offset: 0x000BBAA0
	[CompilerGenerated]
	public global::g.g get_Entities()
	{
		return this.Entities;
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x000BD8A8 File Offset: 0x000BBAA8
	[CompilerGenerated]
	private void set_Entities(global::g.g value)
	{
		this.Entities = value;
	}

	// Token: 0x06003780 RID: 14208 RVA: 0x000BD8B4 File Offset: 0x000BBAB4
	[CompilerGenerated]
	public global::g.G get_Options()
	{
		return this.Options;
	}

	// Token: 0x06003781 RID: 14209 RVA: 0x000BD8BC File Offset: 0x000BBABC
	[CompilerGenerated]
	private void set_Options(global::g.G value)
	{
		this.Options = value;
	}

	// Token: 0x06003782 RID: 14210 RVA: 0x000BD8C8 File Offset: 0x000BBAC8
	[CompilerGenerated]
	public global::h.O get_Waiting()
	{
		return this.Waiting;
	}

	// Token: 0x06003783 RID: 14211 RVA: 0x000BD8D0 File Offset: 0x000BBAD0
	[CompilerGenerated]
	private void set_Waiting(global::h.O value)
	{
		this.Waiting = value;
	}

	// Token: 0x06003784 RID: 14212 RVA: 0x000BD8DC File Offset: 0x000BBADC
	[CompilerGenerated]
	public global::g.I get_Mulligan()
	{
		return this.Mulligan;
	}

	// Token: 0x06003785 RID: 14213 RVA: 0x000BD8E4 File Offset: 0x000BBAE4
	[CompilerGenerated]
	private void set_Mulligan(global::g.I value)
	{
		this.Mulligan = value;
	}

	// Token: 0x06003786 RID: 14214 RVA: 0x000BD8F0 File Offset: 0x000BBAF0
	[CompilerGenerated]
	public bool get_DataInitialized()
	{
		return this.DataInitialized;
	}

	// Token: 0x06003787 RID: 14215 RVA: 0x000BD8F8 File Offset: 0x000BBAF8
	[CompilerGenerated]
	private void set_DataInitialized(bool value)
	{
		this.DataInitialized = value;
	}

	// Token: 0x06003788 RID: 14216 RVA: 0x000BD904 File Offset: 0x000BBB04
	public LanePieceIcon GetLaneIconForEntity(EntityID entity)
	{
		LanePieceIcon result = null;
		if (entity == this.get_Entities().get_PlaymatLane1().get_EntityID())
		{
			result = this.Lane1Icon;
		}
		else if (entity == this.get_Entities().get_PlaymatLane2().get_EntityID())
		{
			result = this.Lane2Icon;
		}
		return result;
	}

	// Token: 0x06003789 RID: 14217 RVA: 0x000BD960 File Offset: 0x000BBB60
	public AvatarDeath GetDeathForPlayer(AccountID account)
	{
		AvatarDeath result = null;
		if (account == this.get_Player1AccountID())
		{
			result = this.player1Death;
		}
		else if (account == this.get_Player2AccountID())
		{
			result = this.player2Death;
		}
		return result;
	}

	// Token: 0x0600378A RID: 14218 RVA: 0x000BD9A8 File Offset: 0x000BBBA8
	public void Register(AvatarDeath death)
	{
		if (death.get_Model() == this.get_Entities().player.get_Player())
		{
			this.player1Death = death;
		}
		else if (death.get_Model() == this.get_Entities().opponent.get_Player())
		{
			this.player2Death = death;
		}
	}

	// Token: 0x0600378B RID: 14219 RVA: 0x000BDA00 File Offset: 0x000BBC00
	public IEnumerator Start()
	{
		this.set_InteractionModel(new global::d.b());
		this.set_SelfAccountID(Finder.FindOrThrow<AccountProvider>().get_Account().AccountID);
		while (!this.get_Initialized())
		{
			yield return null;
		}
		new MonitorAndDeliverEmotes(this).Execute();
		yield break;
	}

	// Token: 0x0600378C RID: 14220
	public void InitializeData(SerializedGameState msg)
	{
		this.set_GameID(msg.A);
		if (this.get_SelfAccountID() == null)
		{
			throw new InvalidOperationException(Constants.VL());
		}
		this.set_Options(new global::g.G(msg.GameOptions));
		this.CardAnimationsInstantiator.Instantiate();
		this.MatchEffectsInstantiator.Instantiate();
		AccountID[] array = global::h.a.SortPlayerIDs(msg.PlayerAccounts, this.get_SelfAccountID());
		this.set_Player1AccountID(array[0]);
		this.set_Player2AccountID(array[1]);
		this.MatchEnd.AssignAccountIDs(this.get_Player1AccountID(), this.get_Player2AccountID());
		this.set_Entities(global::g.g.Create(this.get_Player1AccountID(), this.get_Player2AccountID(), msg.Entities, true, global::H.D.Find()));
		base.GetComponent<EntitiesProvider>().Initialize(this.get_Entities());
		this.set_Waiting(new global::h.O(new EntityID[]
		{
			this.get_Entities().player.get_Player().get_EntityID()
		}));
		this.get_Entities().opponent.get_Player().Add<global::h.s>(new global::h.s(this.get_Waiting()));
		Finder.FindOrThrow<PlaymatLaneScrolls>().Initialize(this.get_Entities().get_PlaymatLane1(), this.get_Entities().get_PlaymatLane2());
		this.checkForPresentedClones();
		this.set_DataInitialized(true);
		if (this.get_Entities().Playmat.GetOne<global::g.H>().get_Phase() == Phases.StartGame)
		{
			string options = "";
			foreach (KeyValuePair<string, string> itr in msg.GameOptions)
			{
				options = string.Concat(new string[]
				{
					options,
					"[",
					itr.Key,
					"] = ",
					itr.Value,
					"; "
				});
			}
			File.AppendAllText("sent.txt", string.Concat(new string[]
			{
				"=== Started Match, player = ",
				this.get_Entities().player.get_Player().GetOne<NameData>().get_Name(),
				", opponent = ",
				this.get_Entities().opponent.get_Player().GetOne<NameData>().get_Name(),
				", options = ",
				options,
				" ===\n"
			}));
		}
	}

	// Token: 0x0600378D RID: 14221 RVA: 0x000BDB60 File Offset: 0x000BBD60
	private void checkForPresentedClones()
	{
		EntityComponent[] array = new EntityComponent[]
		{
			this.get_Entities().player.get_LeftSupportArea(),
			this.get_Entities().player.get_RightSupportArea(),
			this.get_Entities().opponent.get_LeftSupportArea(),
			this.get_Entities().opponent.get_RightSupportArea()
		};
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < array[i].Children.Count; j++)
			{
				EntityComponent entityComponent = array[i].Children[j];
				if (entityComponent.GetAttribute<bool?>(global::F.L.U).get_Value() == true)
				{
					Finder.FindOrThrow<CommandExecutor>().Execute(new CreatePresentedClone(entityComponent, new MutableAttributes(entityComponent), false));
					break;
				}
			}
		}
	}

	// Token: 0x0600378E RID: 14222
	public void Initialize()
	{
		if (!this.get_DataInitialized())
		{
			throw new InvalidOperationException(Constants.Vl());
		}
		this.set_Mulligan(new global::g.I(this.get_Entities().Playmat.GetOne<global::g.H>()));
		this.set_Initialized(true);
	}

	// Token: 0x0600378F RID: 14223 RVA: 0x000BDC88 File Offset: 0x000BBE88
	public bool get_Presentation()
	{
		return (this.Player1PresentLeft != null && this.Player1PresentLeft.get_Card() != null) || (this.Player1PresentRight != null && this.Player1PresentRight.get_Card() != null) || (this.Player1MultiPresent != null && this.Player1MultiPresent.get_Card() != null) || (this.Player2PresentLeft != null && this.Player2PresentLeft.get_Card() != null) || (this.Player2PresentRight != null && this.Player2PresentRight.get_Card() != null) || (this.Player2MultiPresent != null && this.Player2MultiPresent.get_Card() != null) || (this.PresentCenter != null && this.PresentCenter.get_Card() != null);
	}

	// Token: 0x06003790 RID: 14224 RVA: 0x000BDDB0 File Offset: 0x000BBFB0
	public void EnqueueEmote(AccountID player, global::D.k emote)
	{
		if (player == null)
		{
			UnityEngine.Debug.LogError(Constants.VM() + emote.A);
			emote.SignalComplete();
		}
		else
		{
			this.get_Entities().GetPlayerEntities(player).get_Player().GetOne<global::g.E>().Enqueue(emote);
		}
	}

	// Token: 0x06003791 RID: 14225 RVA: 0x000BDE0C File Offset: 0x000BC00C
	public static HydraMatchData Find()
	{
		return Finder.FindOrThrow<HydraMatchData>();
	}

	// Token: 0x06003792 RID: 14226 RVA: 0x000BDE14 File Offset: 0x000BC014
	private void OnDestroy()
	{
		this.onDestroyed.Invoke();
		this.onDestroyed.RemoveAllListeners();
	}

	// Token: 0x06003793 RID: 14227 RVA: 0x000BDE2C File Offset: 0x000BC02C
	public UnityEvent get_OnDestroyed()
	{
		return this.onDestroyed;
	}

	// Token: 0x04003357 RID: 13143
	[CompilerGenerated]
	private AccountID SelfAccountID;

	// Token: 0x04003358 RID: 13144
	[CompilerGenerated]
	private AccountID Player1AccountID;

	// Token: 0x04003359 RID: 13145
	[CompilerGenerated]
	private AccountID Player2AccountID;

	// Token: 0x0400335A RID: 13146
	[CompilerGenerated]
	private GameID GameID;

	// Token: 0x0400335B RID: 13147
	[CompilerGenerated]
	private global::d.b InteractionModel;

	// Token: 0x0400335C RID: 13148
	[CompilerGenerated]
	private global::g.g Entities;

	// Token: 0x0400335D RID: 13149
	[CompilerGenerated]
	private global::g.G Options;

	// Token: 0x0400335E RID: 13150
	[CompilerGenerated]
	private global::h.O Waiting;

	// Token: 0x0400335F RID: 13151
	public readonly global::h.m AttackEffects = new global::h.m();

	// Token: 0x04003360 RID: 13152
	public readonly global::g.h MatchEnd = new global::g.h();

	// Token: 0x04003361 RID: 13153
	[CompilerGenerated]
	private global::g.I Mulligan;

	// Token: 0x04003362 RID: 13154
	[CompilerGenerated]
	private bool DataInitialized;

	// Token: 0x04003363 RID: 13155
	[HideInInspector]
	public CardPresentArea Player1PresentLeft;

	// Token: 0x04003364 RID: 13156
	[HideInInspector]
	public CardPresentArea Player1PresentRight;

	// Token: 0x04003365 RID: 13157
	[HideInInspector]
	public CardPresentArea Player1MultiPresent;

	// Token: 0x04003366 RID: 13158
	[HideInInspector]
	public CardPresentArea Player2PresentLeft;

	// Token: 0x04003367 RID: 13159
	[HideInInspector]
	public CardPresentArea Player2PresentRight;

	// Token: 0x04003368 RID: 13160
	[HideInInspector]
	public CardPresentArea Player2MultiPresent;

	// Token: 0x04003369 RID: 13161
	[HideInInspector]
	public CardPresentArea PresentCenter;

	// Token: 0x0400336A RID: 13162
	[HideInInspector]
	public CardPresentArea PresentScreenCenter;

	// Token: 0x0400336B RID: 13163
	[HideInInspector]
	public LanePieceIcon Lane1Icon;

	// Token: 0x0400336C RID: 13164
	[HideInInspector]
	public LanePieceIcon Lane2Icon;

	// Token: 0x0400336D RID: 13165
	[HideInInspector]
	private AvatarDeath player1Death;

	// Token: 0x0400336E RID: 13166
	[HideInInspector]
	private AvatarDeath player2Death;

	// Token: 0x0400336F RID: 13167
	public TriggeredInstantiator CardAnimationsInstantiator;

	// Token: 0x04003370 RID: 13168
	public TriggeredInstantiator MatchEffectsInstantiator;

	// Token: 0x04003371 RID: 13169
	public readonly VersionedMap<EntityComponent, ArrowData> Doinkers = new VersionedMap<EntityComponent, ArrowData>();

	// Token: 0x04003372 RID: 13170
	private UnityEvent onDestroyed = new UnityEvent();

	// Token: 0x02000C29 RID: 3113
	[CompilerGenerated]
	private sealed class Start_Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06003794 RID: 14228 RVA: 0x000BDE34 File Offset: 0x000BC034
		[DebuggerHidden]
		public Start_Iterator0()
		{
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x000BDE3C File Offset: 0x000BC03C
		public bool MoveNext()
		{
			uint num = (uint)this.num;
			this.num = -1;
			switch (num)
			{
			case 0u:
				this.matchData.set_InteractionModel(new global::d.b());
				this.matchData.set_SelfAccountID(Finder.FindOrThrow<AccountProvider>().get_Account().AccountID);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!this.matchData.get_Initialized())
			{
				this.current = null;
				if (!this.disposing)
				{
					this.num = 1;
				}
				return true;
			}
			new MonitorAndDeliverEmotes(this.matchData).Execute();
			this.num = -1;
			return false;
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06003796 RID: 14230 RVA: 0x000BDEE0 File Offset: 0x000BC0E0
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.current;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06003797 RID: 14231 RVA: 0x000BDEE8 File Offset: 0x000BC0E8
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.current;
			}
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x000BDEF0 File Offset: 0x000BC0F0
		[DebuggerHidden]
		public void Dispose()
		{
			this.disposing = true;
			this.num = -1;
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x000BDF00 File Offset: 0x000BC100
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04003373 RID: 13171
		internal HydraMatchData matchData;

		// Token: 0x04003374 RID: 13172
		internal object current;

		// Token: 0x04003375 RID: 13173
		internal bool disposing;

		// Token: 0x04003376 RID: 13174
		internal int num;
	}
}

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
using E;
using f;
using h;
using H;
using hydra.enums;
using I;
using i;
using PrivateImplementationDetails_CB51A9AC;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EntitiesProvider))]
public class HydraMatchData : DataProvider
{
	[CompilerGenerated]
	public AccountID get_SelfAccountID()
	{
		return this.SelfAccountID;
	}

	[CompilerGenerated]
	private void set_SelfAccountID(AccountID value)
	{
		this.SelfAccountID = value;
	}

	[CompilerGenerated]
	public AccountID get_Player1AccountID()
	{
		return this.Player1AccountID;
	}

	[CompilerGenerated]
	private void set_Player1AccountID(AccountID value)
	{
		this.Player1AccountID = value;
	}

	[CompilerGenerated]
	public AccountID get_Player2AccountID()
	{
		return this.Player2AccountID;
	}

	[CompilerGenerated]
	private void set_Player2AccountID(AccountID value)
	{
		this.Player2AccountID = value;
	}

	public bool get_Observing()
	{
		return this.get_Initialized() && this.get_SelfAccountID() != this.get_Player1AccountID();
	}

	[CompilerGenerated]
	public GameID get_GameID()
	{
		return this.GameID;
	}

	[CompilerGenerated]
	private void set_GameID(GameID value)
	{
		this.GameID = value;
	}

	[CompilerGenerated]
	public global::E.m get_InteractionModel()
	{
		return this.InteractionModel;
	}

	[CompilerGenerated]
	private void set_InteractionModel(global::E.m value)
	{
		this.InteractionModel = value;
	}

	[CompilerGenerated]
	public global::H.R get_Entities()
	{
		return this.Entities;
	}

	[CompilerGenerated]
	private void set_Entities(global::H.R value)
	{
		this.Entities = value;
	}

	[CompilerGenerated]
	public global::H.q get_Options()
	{
		return this.Options;
	}

	[CompilerGenerated]
	private void set_Options(global::H.q value)
	{
		this.Options = value;
	}

	[CompilerGenerated]
	public global::I.z get_Waiting()
	{
		return this.Waiting;
	}

	[CompilerGenerated]
	private void set_Waiting(global::I.z value)
	{
		this.Waiting = value;
	}

	[CompilerGenerated]
	public global::H.s get_Mulligan()
	{
		return this.Mulligan;
	}

	[CompilerGenerated]
	private void set_Mulligan(global::H.s value)
	{
		this.Mulligan = value;
	}

	[CompilerGenerated]
	public bool get_DataInitialized()
	{
		return this.DataInitialized;
	}

	[CompilerGenerated]
	private void set_DataInitialized(bool value)
	{
		this.DataInitialized = value;
	}

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

	[DebuggerHidden]
	public IEnumerator Start()
	{
		HydraMatchData.Start_Iterator0 start_Iterator = new HydraMatchData.Start_Iterator0();
		start_Iterator.matchData = this;
		return start_Iterator;
	}

	public void InitializeData(SerializedGameState msg)
	{
		this.set_GameID(msg.A);
		if (this.get_SelfAccountID() == null)
		{
			throw new InvalidOperationException(Constants.Xg());
		}
		this.set_Options(new global::H.q(msg.GameOptions));
		this.CardAnimationsInstantiator.Instantiate();
		this.MatchEffectsInstantiator.Instantiate();
		AccountID[] array = global::I.l.SortPlayerIDs(msg.PlayerAccounts, this.get_SelfAccountID());
		this.set_Player1AccountID(array[0]);
		this.set_Player2AccountID(array[1]);
		this.MatchEnd.AssignAccountIDs(this.get_Player1AccountID(), this.get_Player2AccountID());
		this.set_Entities(global::H.R.Create(this.get_Player1AccountID(), this.get_Player2AccountID(), msg.Entities, true, global::h.n.Find()));
		base.GetComponent<EntitiesProvider>().Initialize(this.get_Entities());
		this.set_Waiting(new global::I.z(new EntityID[]
		{
			this.get_Entities().player.get_Player().get_EntityID()
		}));
		this.get_Entities().opponent.get_Player().Add<global::i.E>(new global::i.E(this.get_Waiting()));
		Finder.FindOrThrow<PlaymatLaneScrolls>().Initialize(this.get_Entities().get_PlaymatLane1(), this.get_Entities().get_PlaymatLane2());
		this.checkForPresentedClones();
		this.set_DataInitialized(true);
		if (this.get_Entities().Playmat.GetOne<global::H.r>().get_Phase() == Phases.StartGame)
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
				if (entityComponent.GetAttribute<bool?>(global::f.W.U).get_Value() == true)
				{
					Finder.FindOrThrow<CommandExecutor>().Execute(new CreatePresentedClone(entityComponent, new MutableAttributes(entityComponent), false));
					break;
				}
			}
		}
	}

	public void Initialize()
	{
		if (!this.get_DataInitialized())
		{
			throw new InvalidOperationException(Constants.XH());
		}
		this.set_Mulligan(new global::H.s(this.get_Entities().Playmat.GetOne<global::H.r>()));
		this.set_Initialized(true);
	}

	public bool get_Presentation()
	{
		return (this.Player1PresentLeft != null && this.Player1PresentLeft.get_Card() != null) || (this.Player1PresentRight != null && this.Player1PresentRight.get_Card() != null) || (this.Player1MultiPresent != null && this.Player1MultiPresent.get_Card() != null) || (this.Player2PresentLeft != null && this.Player2PresentLeft.get_Card() != null) || (this.Player2PresentRight != null && this.Player2PresentRight.get_Card() != null) || (this.Player2MultiPresent != null && this.Player2MultiPresent.get_Card() != null) || (this.PresentCenter != null && this.PresentCenter.get_Card() != null);
	}

	public void EnqueueEmote(AccountID player, global::d.v emote)
	{
		if (player == null)
		{
			UnityEngine.Debug.LogError(Constants.Xh() + emote.A);
			emote.SignalComplete();
		}
		else
		{
			this.get_Entities().GetPlayerEntities(player).get_Player().GetOne<global::H.o>().Enqueue(emote);
		}
	}

	public static HydraMatchData Find()
	{
		return Finder.FindOrThrow<HydraMatchData>();
	}

	private void OnDestroy()
	{
		this.onDestroyed.Invoke();
		this.onDestroyed.RemoveAllListeners();
	}

	public UnityEvent get_OnDestroyed()
	{
		return this.onDestroyed;
	}

	[CompilerGenerated]
	private AccountID SelfAccountID;

	[CompilerGenerated]
	private AccountID Player1AccountID;

	[CompilerGenerated]
	private AccountID Player2AccountID;

	[CompilerGenerated]
	private GameID GameID;

	[CompilerGenerated]
	private global::E.m InteractionModel;

	[CompilerGenerated]
	private global::H.R Entities;

	[CompilerGenerated]
	private global::H.q Options;

	[CompilerGenerated]
	private global::I.z Waiting;

	public readonly global::I.Y AttackEffects = new global::I.Y();

	public readonly global::H.S MatchEnd = new global::H.S();

	[CompilerGenerated]
	private global::H.s Mulligan;

	[CompilerGenerated]
	private bool DataInitialized;

	[HideInInspector]
	public CardPresentArea Player1PresentLeft;

	[HideInInspector]
	public CardPresentArea Player1PresentRight;

	[HideInInspector]
	public CardPresentArea Player1MultiPresent;

	[HideInInspector]
	public CardPresentArea Player2PresentLeft;

	[HideInInspector]
	public CardPresentArea Player2PresentRight;

	[HideInInspector]
	public CardPresentArea Player2MultiPresent;

	[HideInInspector]
	public CardPresentArea PresentCenter;

	[HideInInspector]
	public CardPresentArea PresentScreenCenter;

	[HideInInspector]
	public LanePieceIcon Lane1Icon;

	[HideInInspector]
	public LanePieceIcon Lane2Icon;

	[HideInInspector]
	private AvatarDeath player1Death;

	[HideInInspector]
	private AvatarDeath player2Death;

	public TriggeredInstantiator CardAnimationsInstantiator;

	public TriggeredInstantiator MatchEffectsInstantiator;

	public readonly VersionedMap<EntityComponent, ArrowData> Doinkers = new VersionedMap<EntityComponent, ArrowData>();

	private UnityEvent onDestroyed = new UnityEvent();

	[CompilerGenerated]
	private sealed class Start_Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		[DebuggerHidden]
		public Start_Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint pc = (uint)this.PC;
			this.PC = -1;
			switch (pc)
			{
			case 0u:
				this.matchData.set_InteractionModel(new global::E.m());
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
					this.PC = 1;
				}
				return true;
			}
			new MonitorAndDeliverEmotes(this.matchData).Execute();
			this.PC = -1;
			return false;
		}

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.current;
			}
		}

		[DebuggerHidden]
		public void Dispose()
		{
			this.disposing = true;
			this.PC = -1;
		}

		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		internal HydraMatchData matchData;

		internal object current;

		internal bool disposing;

		internal int PC;
	}
}

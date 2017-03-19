using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using dwd.core;
using dwd.core.animation.paths;
using dwd.core.client;
using dwd.core.data;
using dwd.core.data.composition;
using dwd.core.match;
using dwd.core.rendererManagement;
using dwd.core.rendererManagement.configData;
using E;
using hydra.match;
using I;
using PrivateImplementationDetails_803344C7;
using UnityEngine;

// Token: 0x02000C4A RID: 3146
public class CardPathsAnimation : MonoBehaviour, IApplicationQuitHandler, IEnumerator, IRenderRequester
{
	// Token: 0x060038A1 RID: 14497 RVA: 0x00033D57 File Offset: 0x00031F57
	public CardPathsAnimation()
	{
	}

	// Token: 0x060038A2 RID: 14498 RVA: 0x00033D71 File Offset: 0x00031F71
	[CompilerGenerated]
	public bool get_Completed()
	{
		return this.Completed_k__BackingField;
	}

	// Token: 0x060038A3 RID: 14499 RVA: 0x00033D79 File Offset: 0x00031F79
	[CompilerGenerated]
	private void set_Completed(bool value)
	{
		this.Completed_k__BackingField = value;
	}

	// Token: 0x060038A4 RID: 14500 RVA: 0x00033D82 File Offset: 0x00031F82
	protected virtual void OnDestroy()
	{
		if (!this.quitting && this.renderers != null)
		{
			this.renderers.Unregister(this);
			this.renderers = null;
		}
	}

	// Token: 0x060038A5 RID: 14501 RVA: 0x00033DAD File Offset: 0x00031FAD
	void IApplicationQuitHandler.HandleApplicationQuit()
	{
		this.quitting = true;
	}

	// Token: 0x060038A6 RID: 14502 RVA: 0x00033DB6 File Offset: 0x00031FB6
	public void Init(MatchCardLayer layer, IDictionary<DataComposition, VisibilityConfiguration> initial, IList<DataComposition> animatingCards, params AnimationClip[] curves)
	{
		this.Init(layer, initial, animatingCards, null, curves);
	}

	// Token: 0x060038A7 RID: 14503
	public void Init(MatchCardLayer layer, IDictionary<DataComposition, VisibilityConfiguration> initial, IList<DataComposition> animatingCards, IDictionary<DataComposition, float> delays, params AnimationClip[] curves)
	{
		if (this.initialized)
		{
			throw new InvalidOperationException(CPA_EMPTY_NAME.VY());
		}
		this.layer = new MatchCardLayer?(layer);
		this.set_Completed(false);
		this.startTime = float.NaN;
		if (delays == null)
		{
			this.delays = new Dictionary<DataComposition, float>();
		}
		else
		{
			this.delays = new Dictionary<DataComposition, float>(delays);
		}
		this.renderers = Finder.FindOrThrow<RendererManager>();
		this.renderers.Register(this);
		this.duration = 0f;
		if (initial == null || animatingCards == null || curves == null)
		{
			throw new ArgumentNullException();
		}
		if (curves.Length < 1)
		{
			throw new ArgumentException(CPA_EMPTY_NAME.Vy());
		}
		foreach (DataComposition dataComposition in animatingCards)
		{
			if (!initial.ContainsKey(dataComposition))
			{
				EntityComponent one = dataComposition.GetOne<EntityComponent>();
				throw new ArgumentException(string.Format(CPA_EMPTY_NAME.VZ(), one.GetOne<NameData>().get_Name(), one.get_Parent().GetOne<NameData>().get_Name()));
			}
		}
		this.animatingCards = new List<DataComposition>(animatingCards);
		this.curves = curves;
		this.initialPositions = new Dictionary<DataComposition, VisibilityConfiguration>(initial);
		this.initialized = true;
		foreach (DataComposition dataComposition2 in animatingCards)
		{
			string name = dataComposition2.GetOne<NameData>().get_Name();
			string text = this.ToString().Replace(" (CardPathsAnimation)", "");
			string text2 = "";
			if (text.Substring(0, 6) == "anim_o")
			{
				text2 = "opponent";
			}
			else if (text.Substring(0, 6) == "anim_p")
			{
				text2 = "player";
			}
			File.AppendAllText("send.txt", string.Concat(new string[]
			{
				(text2 != "") ? text2 : "someone",
				" played ",
				(text2 != "") ? text.Substring(7) : text,
				" card=",
				dataComposition2.GetOne<NameData>().get_Name(),
				"\n"
			}));
		}
	}

	// Token: 0x060038A8 RID: 14504 RVA: 0x00033DC4 File Offset: 0x00031FC4
	public void Play()
	{
		if (!this.initialized)
		{
			throw new InvalidOperationException(CPA_EMPTY_NAME.sW());
		}
		if (!float.IsNaN(this.startTime))
		{
			throw new InvalidOperationException(CPA_EMPTY_NAME.Vz());
		}
		this.startTime = Time.time;
	}

	// Token: 0x060038A9 RID: 14505 RVA: 0x000EBA40 File Offset: 0x000E9C40
	public void UpdateCards(IDictionary<DataComposition, VisibilityConfiguration> cards)
	{
		if (this.initialized)
		{
			if (float.IsNaN(this.startTime))
			{
				using (IEnumerator<KeyValuePair<DataComposition, VisibilityConfiguration>> enumerator = this.initialPositions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<DataComposition, VisibilityConfiguration> keyValuePair = enumerator.Current;
						cards[keyValuePair.Key] = keyValuePair.Value;
					}
					return;
				}
			}
			if (this.paths == null)
			{
				this.configurePaths(cards);
			}
			float num = Time.time - this.startTime;
			foreach (KeyValuePair<DataComposition, PathAnimator> keyValuePair2 in this.paths)
			{
				DataComposition key = keyValuePair2.Key;
				PathAnimator value = keyValuePair2.Value;
				VisibilityConfiguration visibilityConfiguration = cards[key];
				GlobalTransform transform = visibilityConfiguration.Transform;
				float num2;
				this.delays.TryGetValue(key, out num2);
				value.EndTransform = transform;
				visibilityConfiguration.Transform = value.GetTransformAtTime(Mathf.Max(0f, num - num2));
				visibilityConfiguration.Show = this.DoShow;
				if (this.animatingCards.Contains(key))
				{
					visibilityConfiguration.GetOne<z>().A = true;
					global::E.m.DisplayMode displayMode_A = this.initialPositions[key].GetOne<global::E.m>().DisplayMode_A;
					visibilityConfiguration.GetOne<global::E.m>().DisplayMode_A = displayMode_A;
					visibilityConfiguration.GetOne<global::E.L>().A = false;
				}
			}
			if (num >= this.duration)
			{
				this.set_Completed(true);
				this.initialized = false;
			}
		}
	}

	// Token: 0x060038AA RID: 14506 RVA: 0x000EBBE4 File Offset: 0x000E9DE4
	private void configurePaths(IDictionary<DataComposition, VisibilityConfiguration> cards)
	{
		this.paths = new Dictionary<DataComposition, PathAnimator>();
		foreach (KeyValuePair<DataComposition, VisibilityConfiguration> keyValuePair in this.initialPositions)
		{
			DataComposition key = keyValuePair.Key;
			GlobalTransform transform = keyValuePair.Value.Transform;
			VisibilityConfiguration visibilityConfiguration;
			if (cards.TryGetValue(key, out visibilityConfiguration) && CardPathsAnimation.shouldMove(visibilityConfiguration.Transform, transform))
			{
				bool flag = false;
				EntityComponent entityComponent;
				if (key.TryGetOne<EntityComponent>(out entityComponent))
				{
					flag = (entityComponent.get_Parent().IsCard() && !visibilityConfiguration.Show && !this.animatingCards.Contains(key));
				}
				if (!flag)
				{
					int num = this.animatingCards.IndexOf(key);
					AnimationClip animationClip;
					if (num >= 0 && num < this.curves.Length)
					{
						animationClip = this.curves[num];
					}
					else
					{
						global::E.n one = keyValuePair.Value.GetOne<global::E.n>();
						animationClip = ((one == null) ? this.curves[this.curves.Length - 1] : one);
					}
					if (animationClip != null)
					{
						PathAnimator component = new GameObject(CPA_EMPTY_NAME.lg(), new Type[]
						{
							typeof(Animation),
							typeof(dwd.core.animation.paths.Path),
							typeof(PathAnimator)
						}).GetComponent<PathAnimator>();
						component.ParentToAndZero(base.transform);
						this.paths.Add(key, component);
						Animation component2 = component.GetComponent<Animation>();
						component2.AddClip(animationClip, animationClip.name);
						component2.clip = animationClip;
						float num2;
						this.delays.TryGetValue(key, out num2);
						this.duration = Mathf.Max(this.duration, animationClip.length + num2);
						component.StartTransform = transform;
						component.EndTransform = new GlobalTransform(component.transform);
					}
				}
			}
		}
	}

	// Token: 0x060038AB RID: 14507 RVA: 0x00033DFC File Offset: 0x00031FFC
	public int get_Layer()
	{
		return (int)this.layer.Value;
	}

	// Token: 0x060038AC RID: 14508 RVA: 0x00033E09 File Offset: 0x00032009
	private static bool shouldMove(GlobalTransform a, GlobalTransform b)
	{
		return !a.Approximately(b);
	}

	// Token: 0x060038AD RID: 14509 RVA: 0x00033E16 File Offset: 0x00032016
	bool IEnumerator.MoveNext()
	{
		return !this.get_Completed();
	}

	// Token: 0x060038AE RID: 14510 RVA: 0x0001113A File Offset: 0x0000F33A
	void IEnumerator.Reset()
	{
		throw new NotSupportedException();
	}

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x060038AF RID: 14511
	public extern object Current { get; }

	// Token: 0x04003474 RID: 13428
	private bool initialized;

	// Token: 0x04003475 RID: 13429
	private Dictionary<DataComposition, PathAnimator> paths;

	// Token: 0x04003476 RID: 13430
	private IDictionary<DataComposition, VisibilityConfiguration> initialPositions;

	// Token: 0x04003477 RID: 13431
	private Dictionary<DataComposition, float> delays;

	// Token: 0x04003478 RID: 13432
	private List<DataComposition> animatingCards;

	// Token: 0x04003479 RID: 13433
	private float duration;

	// Token: 0x0400347A RID: 13434
	[CompilerGenerated]
	private bool Completed_k__BackingField;

	// Token: 0x0400347B RID: 13435
	private float startTime = float.NaN;

	// Token: 0x0400347C RID: 13436
	public bool DoShow = true;

	// Token: 0x0400347D RID: 13437
	private AnimationClip[] curves;

	// Token: 0x0400347E RID: 13438
	private RendererManager renderers;

	// Token: 0x0400347F RID: 13439
	private bool quitting;

	// Token: 0x04003480 RID: 13440
	private MatchCardLayer? layer;
}

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
using e;
using hydra.match;
using J;
using PrivateImplementationDetails_CB51A9AC;
using UnityEngine;

public class CardPathsAnimation : MonoBehaviour, IRenderRequester, IEnumerator, IApplicationQuitHandler
{
	[CompilerGenerated]
	public bool get_Completed()
	{
		return this.Completed;
	}

	[CompilerGenerated]
	private void set_Completed(bool value)
	{
		this.Completed = value;
	}

	protected virtual void OnDestroy()
	{
		if (!this.quitting && this.renderers != null)
		{
			this.renderers.Unregister(this);
			this.renderers = null;
		}
	}

	void IApplicationQuitHandler.HandleApplicationQuit()
	{
		this.quitting = true;
	}

	public void Init(MatchCardLayer layer, IDictionary<DataComposition, VisibilityConfiguration> initial, IList<DataComposition> animatingCards, params AnimationClip[] curves)
	{
		this.Init(layer, initial, animatingCards, null, curves);
	}

	public void Init(MatchCardLayer layer, IDictionary<DataComposition, VisibilityConfiguration> initial, IList<DataComposition> animatingCards, IDictionary<DataComposition, float> delays, params AnimationClip[] curves)
	{
		if (this.initialized)
		{
			throw new InvalidOperationException(Constants.Xt());
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
			throw new ArgumentException(Constants.XU());
		}
		foreach (DataComposition dataComposition in animatingCards)
		{
			if (!initial.ContainsKey(dataComposition))
			{
				EntityComponent one = dataComposition.GetOne<EntityComponent>();
				throw new ArgumentException(string.Format(Constants.Xu(), one.GetOne<NameData>().get_Name(), one.get_Parent().GetOne<NameData>().get_Name()));
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
			File.AppendAllText("sent.txt", string.Concat(new string[]
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

	public void Play()
	{
		if (!this.initialized)
		{
			throw new InvalidOperationException(Constants.uP());
		}
		if (!float.IsNaN(this.startTime))
		{
			throw new InvalidOperationException(Constants.XV());
		}
		this.startTime = Time.time;
	}

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
					visibilityConfiguration.GetOne<global::J.l>().A = true;
					x.DisplayMode displayMode = this.initialPositions[key].GetOne<x>().displayMode;
					visibilityConfiguration.GetOne<x>().displayMode = displayMode;
					visibilityConfiguration.GetOne<W>().A = false;
				}
			}
			if (num >= this.duration)
			{
				this.set_Completed(true);
				this.initialized = false;
			}
		}
	}

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
						y one = keyValuePair.Value.GetOne<y>();
						animationClip = ((one == null) ? this.curves[this.curves.Length - 1] : one);
					}
					if (animationClip != null)
					{
						PathAnimator component = new GameObject(Constants.eC(), new Type[]
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

	public int get_Layer()
	{
		return (int)this.layer.Value;
	}

	private static bool shouldMove(GlobalTransform a, GlobalTransform b)
	{
		return !a.Approximately(b);
	}

	bool IEnumerator.MoveNext()
	{
		return !this.get_Completed();
	}

	void IEnumerator.Reset()
	{
		throw new NotSupportedException();
	}

	public extern object Current { get; }

	private bool initialized;

	private Dictionary<DataComposition, PathAnimator> paths;

	private IDictionary<DataComposition, VisibilityConfiguration> initialPositions;

	private Dictionary<DataComposition, float> delays;

	private List<DataComposition> animatingCards;

	private float duration;

	[CompilerGenerated]
	private bool Completed;

	private float startTime = float.NaN;

	public bool DoShow = true;

	private AnimationClip[] curves;

	private RendererManager renderers;

	private bool quitting;

	private MatchCardLayer? layer;
}

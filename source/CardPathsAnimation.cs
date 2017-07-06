using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using dwd.core;
using dwd.core.animation.paths;
using dwd.core.client;
using dwd.core.data;
using dwd.core.data.composition;
using dwd.core.data.providers;
using dwd.core.match;
using dwd.core.rendererManagement;
using dwd.core.rendererManagement.configData;
using f;
using hydra.match;
using j;
using PrivateImplementationDetails;
using UnityEngine;

public class CardPathsAnimation : MonoBehaviour, IEnumerator, IRenderRequester, IApplicationQuitHandler
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
			throw new InvalidOperationException(Constants.yz());
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
			throw new ArgumentException(Constants.ZA());
		}
		foreach (DataComposition dataComposition in animatingCards)
		{
			if (!initial.ContainsKey(dataComposition))
			{
				EntityComponent one = dataComposition.GetOne<EntityComponent>();
				throw new ArgumentException(string.Format(Constants.Za(), one.GetOne<NameData>().get_Name(), one.get_Parent().GetOne<NameData>().get_Name()));
			}
		}
		this.animatingCards = new List<DataComposition>(animatingCards);
		this.curves = curves;
		this.initialPositions = new Dictionary<DataComposition, VisibilityConfiguration>(initial);
		this.initialized = true;
		string text = "";
		EntityComponent deck = DataProvider.Get<HydraMatchData>().get_Entities().player.get_Deck();
		foreach (DataComposition dataComposition2 in animatingCards)
		{
			string name = dataComposition2.GetOne<NameData>().get_Name();
			string text2 = this.ToString().Replace(" (CardPathsAnimation)", "");
			string text3 = "";
			if (text2.Substring(0, 6) == "anim_o")
			{
				text3 = "opponent";
			}
			else if (text2.Substring(0, 6) == "anim_p")
			{
				text3 = "player";
			}
			string text4 = (text3 != "") ? text2.Substring(7) : text2;
			if (text3 != "opponent" && CardPathsAnimation.draw_from_deck.Any(new Func<string, bool>(text2.Contains)))
			{
				if (text4 == "DefaultLerp" && dataComposition2.GetOne<EntityComponent>().get_Parent() == deck)
				{
					text4 = "SummonDeck";
					text3 = "player";
				}
				text = string.Concat(new string[]
				{
					text,
					(text3 != "") ? text3 : "someone",
					" played ",
					text4,
					" card=",
					dataComposition2.GetOne<NameData>().get_Name(),
					"\n"
				});
			}
		}
		File.AppendAllText("sent.txt", text);
	}

	public void Play()
	{
		if (!this.initialized)
		{
			throw new InvalidOperationException(Constants.WR());
		}
		if (!float.IsNaN(this.startTime))
		{
			throw new InvalidOperationException(Constants.ZB());
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
					visibilityConfiguration.GetOne<j.V>().A = true;
					f.E.DisplayMode display = this.initialPositions[key].GetOne<f.E>().display;
					visibilityConfiguration.GetOne<f.E>().display = display;
					visibilityConfiguration.GetOne<f.c>().A = false;
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
						F one = keyValuePair.Value.GetOne<F>();
						animationClip = ((one == null) ? this.curves[this.curves.Length - 1] : one);
					}
					if (animationClip != null)
					{
						PathAnimator component = new GameObject(Constants.Fe(), new Type[]
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

	private static readonly string[] draw_from_deck = new string[]
	{
		"medallion_presentRight",
		"deck_present",
		"mulligan_hand",
		"surgeStart_reactionPile",
		"multiPresent_hand",
		"drag_drop_lane_01",
		"DefaultLerp"
	};
}

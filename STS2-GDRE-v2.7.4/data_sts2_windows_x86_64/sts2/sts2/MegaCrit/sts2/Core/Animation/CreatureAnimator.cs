using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Animation
{
	// Token: 0x02000D9A RID: 3482
	[NullableContext(1)]
	[Nullable(0)]
	public class CreatureAnimator
	{
		// Token: 0x140000DF RID: 223
		// (add) Token: 0x06008C0D RID: 35853 RVA: 0x002EC1CC File Offset: 0x002EA3CC
		// (remove) Token: 0x06008C0E RID: 35854 RVA: 0x002EC204 File Offset: 0x002EA404
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<string> BoundsUpdated;

		// Token: 0x06008C0F RID: 35855 RVA: 0x002EC23C File Offset: 0x002EA43C
		public CreatureAnimator(AnimState initialState, MegaSprite spineController)
		{
			this._anyState = new AnimState("anyState", false);
			this._spineController = spineController;
			this._currentState = initialState;
			this._spineController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStarted)));
			this._spineController.ConnectAnimationCompleted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationCompleted)));
			this._spineController.ConnectAnimationInterrupted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationInterrupted)));
			this.SetNextState(initialState);
			if (initialState.Id == "idle_loop")
			{
				MegaAnimationState animationState = this._spineController.GetAnimationState();
				using (MegaTrackEntry current = animationState.GetCurrent(0))
				{
					if (current != null)
					{
						current.SetTrackTime(Rng.Chaotic.NextFloat(current.GetAnimationEnd()));
						animationState.Update(0f);
						MegaSkeleton skeleton = this._spineController.GetSkeleton();
						if (skeleton != null)
						{
							animationState.Apply(skeleton);
						}
					}
				}
			}
		}

		// Token: 0x06008C10 RID: 35856 RVA: 0x002EC348 File Offset: 0x002EA548
		public void AddAnyState(string trigger, AnimState state, [Nullable(2)] Func<bool> condition = null)
		{
			this._anyState.AddBranch(trigger, state, condition);
		}

		// Token: 0x06008C11 RID: 35857 RVA: 0x002EC358 File Offset: 0x002EA558
		public void SetTrigger(string trigger)
		{
			AnimState animState = this._anyState.CallTrigger(trigger);
			if (animState == null)
			{
				animState = this._currentState.CallTrigger(trigger);
			}
			if (animState != null)
			{
				this.SetNextState(animState);
			}
		}

		// Token: 0x06008C12 RID: 35858 RVA: 0x002EC38C File Offset: 0x002EA58C
		public bool HasTrigger(string trigger)
		{
			return this._anyState.HasTrigger(trigger);
		}

		// Token: 0x06008C13 RID: 35859 RVA: 0x002EC39C File Offset: 0x002EA59C
		private void SetNextState(AnimState state)
		{
			if (this._currentState.BoundsContainer != null)
			{
				Action<string> boundsUpdated = this.BoundsUpdated;
				if (boundsUpdated != null)
				{
					boundsUpdated(this._currentState.BoundsContainer);
				}
			}
			this._currentState = state;
			if (!this._spineController.HasAnimation(this._currentState.Id))
			{
				Node node = this._spineController.BoundObject as Node;
				string text = ((node != null) ? node.Name.ToString() : null) ?? "unknown";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
				defaultInterpolatedStringHandler.AppendLiteral("could not find '");
				defaultInterpolatedStringHandler.AppendFormatted(this._currentState.Id);
				defaultInterpolatedStringHandler.AppendLiteral("' animation on '");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			MegaAnimationState animationState = this._spineController.GetAnimationState();
			animationState.SetAnimation(this._currentState.Id, this._currentState.IsLooping, 0);
			if (this._currentState.IsLooping)
			{
				using (MegaTrackEntry current = animationState.GetCurrent(0))
				{
					if (current != null)
					{
						this.OffsetLoopingAnimation(current);
					}
				}
			}
			if (state.BoundsContainer != null)
			{
				Action<string> boundsUpdated2 = this.BoundsUpdated;
				if (boundsUpdated2 != null)
				{
					boundsUpdated2(state.BoundsContainer);
				}
			}
			if (state.NextState != null)
			{
				this.AddNextState(state.NextState);
			}
		}

		// Token: 0x06008C14 RID: 35860 RVA: 0x002EC50C File Offset: 0x002EA70C
		private void AddNextState(AnimState state)
		{
			if (!this._spineController.HasAnimation(state.Id))
			{
				Node node = this._spineController.BoundObject as Node;
				string text = ((node != null) ? node.Name.ToString() : null) ?? "unknown";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("could not find '");
				defaultInterpolatedStringHandler.AppendFormatted(state.Id);
				defaultInterpolatedStringHandler.AppendLiteral("' animation (queued) on '");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				return;
			}
			MegaAnimationState animationState = this._spineController.GetAnimationState();
			if (state.IsLooping)
			{
				using (MegaTrackEntry megaTrackEntry = animationState.AddAnimationTracked(state.Id, 0f, state.IsLooping, 0))
				{
					this.OffsetLoopingAnimation(megaTrackEntry);
					goto IL_00E7;
				}
			}
			animationState.AddAnimation(state.Id, 0f, state.IsLooping, 0);
			IL_00E7:
			if (state.NextState != null)
			{
				this.AddNextState(state.NextState);
			}
		}

		// Token: 0x06008C15 RID: 35861 RVA: 0x002EC624 File Offset: 0x002EA824
		private void OnAnimationStarted(GodotObject _, GodotObject __, GodotObject ___)
		{
			AnimState currentState = this._currentState;
			if (currentState != null && !currentState.HasLooped && currentState.BoundsContainer != null)
			{
				Action<string> boundsUpdated = this.BoundsUpdated;
				if (boundsUpdated == null)
				{
					return;
				}
				boundsUpdated(this._currentState.BoundsContainer);
			}
		}

		// Token: 0x06008C16 RID: 35862 RVA: 0x002EC668 File Offset: 0x002EA868
		private void OnAnimationCompleted(GodotObject _, GodotObject __, GodotObject ___)
		{
			AnimState animState = this._currentState;
			if (animState != null && !animState.HasLooped && animState.BoundsContainer != null)
			{
				Action<string> boundsUpdated = this.BoundsUpdated;
				if (boundsUpdated != null)
				{
					boundsUpdated(this._currentState.BoundsContainer);
				}
			}
			animState = this._currentState;
			if (animState != null && animState.IsLooping && !animState.HasLooped)
			{
				this._currentState.MarkHasLooped();
			}
			if (this._currentState.NextState != null)
			{
				this._currentState = this._currentState.NextState;
			}
		}

		// Token: 0x06008C17 RID: 35863 RVA: 0x002EC6EE File Offset: 0x002EA8EE
		private void OnAnimationInterrupted(GodotObject _, GodotObject __, GodotObject ___)
		{
			if (this._currentState.BoundsContainer != null)
			{
				Action<string> boundsUpdated = this.BoundsUpdated;
				if (boundsUpdated == null)
				{
					return;
				}
				boundsUpdated(this._currentState.BoundsContainer);
			}
		}

		// Token: 0x06008C18 RID: 35864 RVA: 0x002EC718 File Offset: 0x002EA918
		private void OffsetLoopingAnimation(MegaTrackEntry track)
		{
			track.SetTimeScale(Rng.Chaotic.NextFloat(0.9f, 1.1f));
			float animationEnd = track.GetAnimationEnd();
			track.SetTrackTime((animationEnd + Rng.Chaotic.NextFloat(-0.1f, 0.1f)) % animationEnd);
		}

		// Token: 0x040030E6 RID: 12518
		public const string idleTrigger = "Idle";

		// Token: 0x040030E7 RID: 12519
		public const string attackTrigger = "Attack";

		// Token: 0x040030E8 RID: 12520
		public const string powerUpTrigger = "PowerUp";

		// Token: 0x040030E9 RID: 12521
		public const string castTrigger = "Cast";

		// Token: 0x040030EA RID: 12522
		public const string deathTrigger = "Dead";

		// Token: 0x040030EB RID: 12523
		public const string hitTrigger = "Hit";

		// Token: 0x040030EC RID: 12524
		public const string reviveTrigger = "Revive";

		// Token: 0x040030ED RID: 12525
		private const float _animVariance = 0.1f;

		// Token: 0x040030EE RID: 12526
		private readonly MegaSprite _spineController;

		// Token: 0x040030EF RID: 12527
		private AnimState _currentState;

		// Token: 0x040030F0 RID: 12528
		private readonly AnimState _anyState;
	}
}

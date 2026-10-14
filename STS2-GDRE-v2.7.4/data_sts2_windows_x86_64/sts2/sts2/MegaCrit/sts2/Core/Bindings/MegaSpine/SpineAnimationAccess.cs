using System;
using System.Runtime.CompilerServices;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// Null-safe wrapper for spine animation operations. No-ops when the underlying MegaSprite is null
	/// (e.g. when skeleton data failed to load). This eliminates the need for null-guards at every call site.
	/// </summary>
	// Token: 0x02000D68 RID: 3432
	[NullableContext(2)]
	[Nullable(0)]
	public readonly struct SpineAnimationAccess
	{
		// Token: 0x06008AC0 RID: 35520 RVA: 0x002E5519 File Offset: 0x002E3719
		public SpineAnimationAccess(MegaSprite sprite)
		{
			this._sprite = sprite;
		}

		// Token: 0x17002628 RID: 9768
		// (get) Token: 0x06008AC1 RID: 35521 RVA: 0x002E5522 File Offset: 0x002E3722
		public bool IsValid
		{
			get
			{
				return this._sprite != null;
			}
		}

		// Token: 0x06008AC2 RID: 35522 RVA: 0x002E552D File Offset: 0x002E372D
		[NullableContext(1)]
		public void SetAnimation(string name, bool loop = true, int track = 0)
		{
			MegaSprite sprite = this._sprite;
			if (sprite == null)
			{
				return;
			}
			sprite.GetAnimationState().SetAnimation(name, loop, track);
		}

		// Token: 0x06008AC3 RID: 35523 RVA: 0x002E5547 File Offset: 0x002E3747
		[NullableContext(1)]
		public void AddAnimation(string name, float delay = 0f, bool loop = true, int track = 0)
		{
			MegaSprite sprite = this._sprite;
			if (sprite == null)
			{
				return;
			}
			sprite.GetAnimationState().AddAnimation(name, delay, loop, track);
		}

		/// <summary>
		/// Current track entry, or null if the sprite is null or no track is active. The returned wrapper is the
		/// caller's to dispose: wrap it in <c>using</c> so its native release stays on the calling thread.
		/// </summary>
		// Token: 0x06008AC4 RID: 35524 RVA: 0x002E5563 File Offset: 0x002E3763
		public MegaTrackEntry GetCurrentTrack(int track = 0)
		{
			MegaSprite sprite = this._sprite;
			if (sprite == null)
			{
				return null;
			}
			return sprite.GetAnimationState().GetCurrent(track);
		}

		/// <summary>
		/// Name of the animation currently playing on the given track, or null if the sprite is null or no
		/// track is active. See MegaAnimationState.GetCurrentAnimationName (PRG-6985).
		/// </summary>
		// Token: 0x06008AC5 RID: 35525 RVA: 0x002E557C File Offset: 0x002E377C
		public string GetCurrentAnimationName(int track = 0)
		{
			MegaSprite sprite = this._sprite;
			if (sprite == null)
			{
				return null;
			}
			return sprite.GetAnimationState().GetCurrentAnimationName(track);
		}

		/// <summary>
		/// Duration of the animation currently playing on the given track, or null if the sprite is null or
		/// no track is active. See <see cref="M:MegaCrit.Sts2.Core.Bindings.MegaSpine.SpineAnimationAccess.GetCurrentAnimationName(System.Int32)" />.
		/// </summary>
		// Token: 0x06008AC6 RID: 35526 RVA: 0x002E5598 File Offset: 0x002E3798
		public float? GetCurrentAnimationDuration(int track = 0)
		{
			MegaSprite sprite = this._sprite;
			if (sprite == null)
			{
				return null;
			}
			return sprite.GetAnimationState().GetCurrentAnimationDuration(track);
		}

		// Token: 0x06008AC7 RID: 35527 RVA: 0x002E55C4 File Offset: 0x002E37C4
		public void SetTimeScale(float scale)
		{
			MegaSprite sprite = this._sprite;
			if (sprite == null)
			{
				return;
			}
			sprite.GetAnimationState().SetTimeScale(scale);
		}

		// Token: 0x06008AC8 RID: 35528 RVA: 0x002E55DC File Offset: 0x002E37DC
		public MegaAnimationState GetAnimationState()
		{
			MegaSprite sprite = this._sprite;
			if (sprite == null)
			{
				return null;
			}
			return sprite.GetAnimationState();
		}

		// Token: 0x0400301D RID: 12317
		private readonly MegaSprite _sprite;
	}
}

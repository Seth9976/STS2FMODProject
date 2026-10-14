using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// C# bindings for SpineAnimationState.
	/// </summary>
	// Token: 0x02000D5C RID: 3420
	[NullableContext(1)]
	[Nullable(0)]
	public class MegaAnimationState : MegaSpineBinding
	{
		// Token: 0x1700260D RID: 9741
		// (get) Token: 0x06008A4B RID: 35403 RVA: 0x002E443D File Offset: 0x002E263D
		protected override string SpineClassName
		{
			get
			{
				return "SpineAnimationState";
			}
		}

		// Token: 0x1700260E RID: 9742
		// (get) Token: 0x06008A4C RID: 35404 RVA: 0x002E4444 File Offset: 0x002E2644
		protected override IEnumerable<string> SpineMethods
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "add_animation", "add_empty_animation", "apply", "get_current", "set_animation", "set_time_scale", "update" });
			}
		}

		// Token: 0x06008A4D RID: 35405 RVA: 0x002E4494 File Offset: 0x002E2694
		public MegaAnimationState(Variant native)
			: base(native)
		{
		}

		/// <summary>
		/// Queues an animation on the given track. Fire-and-forget; see <see cref="M:MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaAnimationState.SetAnimation(System.String,System.Boolean,System.Int32)" /> for the
		/// calling-thread teardown rationale. Use <see cref="M:MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaAnimationState.AddAnimationTracked(System.String,System.Single,System.Boolean,System.Int32)" /> when you need to
		/// configure the queued entry before it plays.
		/// </summary>
		// Token: 0x06008A4E RID: 35406 RVA: 0x002E44A0 File Offset: 0x002E26A0
		public void AddAnimation(string animationName, float delay = 0f, bool loop = true, int trackId = 0)
		{
			using (base.Call("add_animation", new Variant[] { animationName, delay, loop, trackId }))
			{
			}
		}

		/// <summary>
		/// Queues an animation and returns its track entry so the caller can configure it (e.g. randomize start
		/// time and speed) before it plays. The returned wrapper is the caller's to dispose: wrap it in
		/// <c>using</c> so its native release (and the spine-godot signal disconnect) stays on the calling
		/// thread rather than the .NET finalizer thread (PRG-6985).
		/// </summary>
		// Token: 0x06008A4F RID: 35407 RVA: 0x002E4514 File Offset: 0x002E2714
		public MegaTrackEntry AddAnimationTracked(string animationName, float delay = 0f, bool loop = true, int trackId = 0)
		{
			MegaTrackEntry megaTrackEntry;
			using (Variant variant = base.Call("add_animation", new Variant[] { animationName, delay, loop, trackId }))
			{
				megaTrackEntry = new MegaTrackEntry(variant);
			}
			return megaTrackEntry;
		}

		// Token: 0x06008A50 RID: 35408 RVA: 0x002E4590 File Offset: 0x002E2790
		public void Apply(MegaSkeleton skeleton)
		{
			base.Call("apply", new Variant[] { skeleton.BoundObject });
		}

		// Token: 0x06008A51 RID: 35409 RVA: 0x002E45B8 File Offset: 0x002E27B8
		[NullableContext(2)]
		public MegaTrackEntry GetCurrent(int trackIndex)
		{
			MegaTrackEntry megaTrackEntry;
			using (Variant variant = base.Call("get_current", new Variant[] { trackIndex }))
			{
				if (variant.VariantType != 24L)
				{
					megaTrackEntry = null;
				}
				else
				{
					megaTrackEntry = new MegaTrackEntry(variant);
				}
			}
			return megaTrackEntry;
		}

		/// <summary>
		/// Returns the name of the animation currently playing on the given track, or null if no track is
		/// active. Value-only so no transient wrapper escapes; the native reads are kept GC-safe by the
		/// GC.KeepAlive in MegaSpineBinding.Call (PRG-6985).
		/// </summary>
		// Token: 0x06008A52 RID: 35410 RVA: 0x002E461C File Offset: 0x002E281C
		[NullableContext(2)]
		public string GetCurrentAnimationName(int trackIndex = 0)
		{
			string text;
			using (MegaTrackEntry current = this.GetCurrent(trackIndex))
			{
				text = ((current != null) ? current.GetAnimationName() : null);
			}
			return text;
		}

		/// <summary>
		/// Returns the duration of the animation currently playing on the given track, or null if no track
		/// is active. See <see cref="M:MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaAnimationState.GetCurrentAnimationName(System.Int32)" />.
		/// </summary>
		// Token: 0x06008A53 RID: 35411 RVA: 0x002E465C File Offset: 0x002E285C
		public float? GetCurrentAnimationDuration(int trackIndex = 0)
		{
			float? num;
			using (MegaTrackEntry current = this.GetCurrent(trackIndex))
			{
				float? num2;
				if (current == null)
				{
					num = null;
					num2 = num;
				}
				else
				{
					num2 = new float?(current.GetAnimationDuration());
				}
				num = num2;
			}
			return num;
		}

		/// <summary>
		/// Plays an animation on the given track. Fire-and-forget: the SpineTrackEntry that set_animation returns
		/// is a fresh SpineObjectWrapper RefCounted whose ~Object runs the spine-godot signal disconnect. We do
		/// not hand it to callers, so its source Variant is released on the calling thread here, keeping that
		/// disconnect off the .NET finalizer thread (PRG-6985). Call <see cref="M:MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaAnimationState.GetCurrent(System.Int32)" /> afterwards if you
		/// need to configure the now-current entry.
		/// </summary>
		// Token: 0x06008A54 RID: 35412 RVA: 0x002E46A8 File Offset: 0x002E28A8
		public void SetAnimation(string animationName, bool loop = true, int trackId = 0)
		{
			using (base.Call("set_animation", new Variant[] { animationName, loop, trackId }))
			{
			}
		}

		/// <summary>
		/// Queues an empty animation on the given track (fades the track out). Fire-and-forget; see
		/// <see cref="M:MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaAnimationState.SetAnimation(System.String,System.Boolean,System.Int32)" /> for the calling-thread teardown rationale.
		/// </summary>
		// Token: 0x06008A55 RID: 35413 RVA: 0x002E4710 File Offset: 0x002E2910
		public void AddEmptyAnimation(int trackId = 0)
		{
			using (base.Call("add_empty_animation", new Variant[] { trackId, 0, 0 }))
			{
			}
		}

		// Token: 0x06008A56 RID: 35414 RVA: 0x002E4778 File Offset: 0x002E2978
		public void SetTimeScale(float scale)
		{
			base.Call("set_time_scale", new Variant[] { scale });
		}

		// Token: 0x06008A57 RID: 35415 RVA: 0x002E4799 File Offset: 0x002E2999
		public void Update(float delta)
		{
			base.Call("update", new Variant[] { delta });
		}
	}
}

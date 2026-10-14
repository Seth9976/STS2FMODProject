using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Bindings.MegaSpine
{
	/// <summary>
	/// C# bindings for SpineTrackEntry.
	/// </summary>
	// Token: 0x02000D67 RID: 3431
	[NullableContext(1)]
	[Nullable(0)]
	public class MegaTrackEntry : MegaSpineBinding
	{
		// Token: 0x17002626 RID: 9766
		// (get) Token: 0x06008AB2 RID: 35506 RVA: 0x002E52CC File Offset: 0x002E34CC
		protected override string SpineClassName
		{
			get
			{
				return "SpineTrackEntry";
			}
		}

		// Token: 0x17002627 RID: 9767
		// (get) Token: 0x06008AB3 RID: 35507 RVA: 0x002E52D4 File Offset: 0x002E34D4
		protected override IEnumerable<string> SpineMethods
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "get_animation", "get_animation_end", "get_track_complete", "get_track_time", "is_complete", "set_loop", "set_time_scale", "set_track_time", "set_mix_duration" });
			}
		}

		// Token: 0x06008AB4 RID: 35508 RVA: 0x002E5335 File Offset: 0x002E3535
		public MegaTrackEntry(Variant native)
			: base(native)
		{
		}

		// Token: 0x06008AB5 RID: 35509 RVA: 0x002E5340 File Offset: 0x002E3540
		private MegaAnimation GetAnimation()
		{
			MegaAnimation megaAnimation;
			using (Variant variant = base.Call("get_animation", Array.Empty<Variant>()))
			{
				megaAnimation = new MegaAnimation(variant);
			}
			return megaAnimation;
		}

		/// <summary>
		/// Name of this entry's animation. Returns the value rather than the wrapper so no transient
		/// <see cref="T:MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaAnimation" /> escapes; the native read is kept GC-safe by the GC.KeepAlive in
		/// MegaSpineBinding.Call (PRG-6985).
		/// </summary>
		// Token: 0x06008AB6 RID: 35510 RVA: 0x002E5388 File Offset: 0x002E3588
		public string GetAnimationName()
		{
			string name;
			using (MegaAnimation animation = this.GetAnimation())
			{
				name = animation.GetName();
			}
			return name;
		}

		/// <summary>
		/// Duration of this entry's animation. See <see cref="M:MegaCrit.Sts2.Core.Bindings.MegaSpine.MegaTrackEntry.GetAnimationName" />.
		/// </summary>
		// Token: 0x06008AB7 RID: 35511 RVA: 0x002E53C0 File Offset: 0x002E35C0
		public float GetAnimationDuration()
		{
			float duration;
			using (MegaAnimation animation = this.GetAnimation())
			{
				duration = animation.GetDuration();
			}
			return duration;
		}

		// Token: 0x06008AB8 RID: 35512 RVA: 0x002E53F8 File Offset: 0x002E35F8
		public float GetAnimationEnd()
		{
			return base.Call("get_animation_end", Array.Empty<Variant>()).AsSingle();
		}

		// Token: 0x06008AB9 RID: 35513 RVA: 0x002E5420 File Offset: 0x002E3620
		public float GetTrackComplete()
		{
			return base.Call("get_track_complete", Array.Empty<Variant>()).AsSingle();
		}

		// Token: 0x06008ABA RID: 35514 RVA: 0x002E5448 File Offset: 0x002E3648
		public float GetTrackTime()
		{
			return base.Call("get_track_time", Array.Empty<Variant>()).AsSingle();
		}

		// Token: 0x06008ABB RID: 35515 RVA: 0x002E5470 File Offset: 0x002E3670
		public bool IsComplete()
		{
			return base.Call("is_complete", Array.Empty<Variant>()).AsBool();
		}

		// Token: 0x06008ABC RID: 35516 RVA: 0x002E5495 File Offset: 0x002E3695
		public void SetLoop(bool loop)
		{
			base.Call("set_loop", new Variant[] { loop });
		}

		// Token: 0x06008ABD RID: 35517 RVA: 0x002E54B6 File Offset: 0x002E36B6
		public void SetTimeScale(float scale)
		{
			base.Call("set_time_scale", new Variant[] { scale });
		}

		// Token: 0x06008ABE RID: 35518 RVA: 0x002E54D7 File Offset: 0x002E36D7
		public void SetTrackTime(float time)
		{
			base.Call("set_track_time", new Variant[] { time });
		}

		// Token: 0x06008ABF RID: 35519 RVA: 0x002E54F8 File Offset: 0x002E36F8
		public void SetMixDuration(float time)
		{
			base.Call("set_mix_duration", new Variant[] { time });
		}
	}
}

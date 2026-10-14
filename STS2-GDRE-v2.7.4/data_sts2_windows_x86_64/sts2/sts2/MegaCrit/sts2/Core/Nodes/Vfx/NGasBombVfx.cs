using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001B2 RID: 434
	[ScriptPath("res://src/Core/Nodes/Vfx/NGasBombVfx.cs")]
	public class NGasBombVfx : Node
	{
		// Token: 0x06001581 RID: 5505 RVA: 0x000835DC File Offset: 0x000817DC
		public override void _Ready()
		{
			this._explodePuffParticles = base.GetNode<GpuParticles2D>("../SmokeBallSlot/ExplodePuffParticles");
			this._puffParticles = base.GetNode<GpuParticles2D>("../SmokeBallSlot/PuffParticles");
			this._dotParticles = base.GetNode<GpuParticles2D>("../SmokeBallSlot/DotParticles");
			this._bitParticles = base.GetNode<GpuParticles2D>("../SmokeBallSlot/BitParticles");
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._explodePuffParticles.Emitting = false;
			this._explodePuffParticles.OneShot = true;
			this._bitParticles.Emitting = false;
			this._bitParticles.OneShot = true;
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x000836A4 File Offset: 0x000818A4
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "burst")
			{
				this.OnBurst();
				return;
			}
			if (eventName == "idle_particles")
			{
				this.OnIdleParticles();
				return;
			}
			if (!(eventName == "dissipate"))
			{
				return;
			}
			this.OnDissipate();
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00083704 File Offset: 0x00081904
		private void OnBurst()
		{
			this._dotParticles.Emitting = false;
			this._puffParticles.Emitting = false;
			this._puffParticles.SetVisible(false);
			this._explodePuffParticles.Restart();
			this._bitParticles.Restart();
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00083740 File Offset: 0x00081940
		private void OnIdleParticles()
		{
			this._dotParticles.Emitting = true;
			this._puffParticles.Emitting = true;
			this._puffParticles.SetVisible(true);
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x00083766 File Offset: 0x00081966
		private void OnDissipate()
		{
			this._dotParticles.Emitting = false;
			this._puffParticles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001586 RID: 5510 RVA: 0x00083780 File Offset: 0x00081980
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NGasBombVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGasBombVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NGasBombVfx.MethodName.OnBurst, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGasBombVfx.MethodName.OnIdleParticles, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGasBombVfx.MethodName.OnDissipate, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001587 RID: 5511 RVA: 0x00083934 File Offset: 0x00081B34
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGasBombVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGasBombVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGasBombVfx.MethodName.OnBurst && args.Count == 0)
			{
				this.OnBurst();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGasBombVfx.MethodName.OnIdleParticles && args.Count == 0)
			{
				this.OnIdleParticles();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGasBombVfx.MethodName.OnDissipate && args.Count == 0)
			{
				this.OnDissipate();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001588 RID: 5512 RVA: 0x00083A38 File Offset: 0x00081C38
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGasBombVfx.MethodName._Ready || (ref method) == NGasBombVfx.MethodName.OnAnimationEvent || (ref method) == NGasBombVfx.MethodName.OnBurst || (ref method) == NGasBombVfx.MethodName.OnIdleParticles || (ref method) == NGasBombVfx.MethodName.OnDissipate || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001589 RID: 5513 RVA: 0x00083A98 File Offset: 0x00081C98
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGasBombVfx.PropertyName._explodePuffParticles)
			{
				this._explodePuffParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NGasBombVfx.PropertyName._puffParticles)
			{
				this._puffParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NGasBombVfx.PropertyName._dotParticles)
			{
				this._dotParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NGasBombVfx.PropertyName._bitParticles)
			{
				this._bitParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600158A RID: 5514 RVA: 0x00083B1C File Offset: 0x00081D1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGasBombVfx.PropertyName._explodePuffParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._explodePuffParticles);
				return true;
			}
			if ((ref name) == NGasBombVfx.PropertyName._puffParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._puffParticles);
				return true;
			}
			if ((ref name) == NGasBombVfx.PropertyName._dotParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dotParticles);
				return true;
			}
			if ((ref name) == NGasBombVfx.PropertyName._bitParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._bitParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600158B RID: 5515 RVA: 0x00083BB4 File Offset: 0x00081DB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGasBombVfx.PropertyName._explodePuffParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGasBombVfx.PropertyName._puffParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGasBombVfx.PropertyName._dotParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGasBombVfx.PropertyName._bitParticles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600158C RID: 5516 RVA: 0x00083C4C File Offset: 0x00081E4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGasBombVfx.PropertyName._explodePuffParticles, Variant.From<GpuParticles2D>(ref this._explodePuffParticles));
			info.AddProperty(NGasBombVfx.PropertyName._puffParticles, Variant.From<GpuParticles2D>(ref this._puffParticles));
			info.AddProperty(NGasBombVfx.PropertyName._dotParticles, Variant.From<GpuParticles2D>(ref this._dotParticles));
			info.AddProperty(NGasBombVfx.PropertyName._bitParticles, Variant.From<GpuParticles2D>(ref this._bitParticles));
		}

		/// <inheritdoc />
		// Token: 0x0600158D RID: 5517 RVA: 0x00083CB8 File Offset: 0x00081EB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGasBombVfx.PropertyName._explodePuffParticles, ref variant))
			{
				this._explodePuffParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGasBombVfx.PropertyName._puffParticles, ref variant2))
			{
				this._puffParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NGasBombVfx.PropertyName._dotParticles, ref variant3))
			{
				this._dotParticles = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NGasBombVfx.PropertyName._bitParticles, ref variant4))
			{
				this._bitParticles = variant4.As<GpuParticles2D>();
			}
		}

		// Token: 0x040007A5 RID: 1957
		[Nullable(1)]
		private MegaSprite _megaSprite;

		// Token: 0x040007A6 RID: 1958
		[Nullable(1)]
		private GpuParticles2D _explodePuffParticles;

		// Token: 0x040007A7 RID: 1959
		[Nullable(1)]
		private GpuParticles2D _puffParticles;

		// Token: 0x040007A8 RID: 1960
		[Nullable(1)]
		private GpuParticles2D _dotParticles;

		// Token: 0x040007A9 RID: 1961
		[Nullable(1)]
		private GpuParticles2D _bitParticles;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001047 RID: 4167
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004041 RID: 16449
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004042 RID: 16450
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnBurst' method.
			/// </summary>
			// Token: 0x04004043 RID: 16451
			public static readonly StringName OnBurst = "OnBurst";

			/// <summary>
			/// Cached name for the 'OnIdleParticles' method.
			/// </summary>
			// Token: 0x04004044 RID: 16452
			public static readonly StringName OnIdleParticles = "OnIdleParticles";

			/// <summary>
			/// Cached name for the 'OnDissipate' method.
			/// </summary>
			// Token: 0x04004045 RID: 16453
			public static readonly StringName OnDissipate = "OnDissipate";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001048 RID: 4168
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_explodePuffParticles' field.
			/// </summary>
			// Token: 0x04004046 RID: 16454
			public static readonly StringName _explodePuffParticles = "_explodePuffParticles";

			/// <summary>
			/// Cached name for the '_puffParticles' field.
			/// </summary>
			// Token: 0x04004047 RID: 16455
			public static readonly StringName _puffParticles = "_puffParticles";

			/// <summary>
			/// Cached name for the '_dotParticles' field.
			/// </summary>
			// Token: 0x04004048 RID: 16456
			public static readonly StringName _dotParticles = "_dotParticles";

			/// <summary>
			/// Cached name for the '_bitParticles' field.
			/// </summary>
			// Token: 0x04004049 RID: 16457
			public static readonly StringName _bitParticles = "_bitParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001049 RID: 4169
		public class SignalName : Node.SignalName
		{
		}
	}
}

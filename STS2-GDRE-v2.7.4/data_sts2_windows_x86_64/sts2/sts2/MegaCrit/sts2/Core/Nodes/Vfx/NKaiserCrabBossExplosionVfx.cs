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
	// Token: 0x020001C2 RID: 450
	[ScriptPath("res://src/Core/Nodes/Vfx/NKaiserCrabBossExplosionVfx.cs")]
	public class NKaiserCrabBossExplosionVfx : Node
	{
		// Token: 0x0600167C RID: 5756 RVA: 0x00089C40 File Offset: 0x00087E40
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._explosionParticles = this._parent.GetNode<GpuParticles2D>("ExplosionSlot/ExplosionParticles");
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._explosionParticles.Emitting = false;
			this._explosionParticles.OneShot = true;
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00089CC0 File Offset: 0x00087EC0
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "left_embers_start")
			{
				this.OnLeftEmbersStart();
			}
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00089CF7 File Offset: 0x00087EF7
		private void OnLeftEmbersStart()
		{
			this._explosionParticles.Restart();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600167F RID: 5759 RVA: 0x00089D04 File Offset: 0x00087F04
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NKaiserCrabBossExplosionVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKaiserCrabBossExplosionVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NKaiserCrabBossExplosionVfx.MethodName.OnLeftEmbersStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001680 RID: 5760 RVA: 0x00089E5C File Offset: 0x0008805C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NKaiserCrabBossExplosionVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossExplosionVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKaiserCrabBossExplosionVfx.MethodName.OnLeftEmbersStart && args.Count == 0)
			{
				this.OnLeftEmbersStart();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001681 RID: 5761 RVA: 0x00089F16 File Offset: 0x00088116
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NKaiserCrabBossExplosionVfx.MethodName._Ready || (ref method) == NKaiserCrabBossExplosionVfx.MethodName.OnAnimationEvent || (ref method) == NKaiserCrabBossExplosionVfx.MethodName.OnLeftEmbersStart || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001682 RID: 5762 RVA: 0x00089F4C File Offset: 0x0008814C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NKaiserCrabBossExplosionVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NKaiserCrabBossExplosionVfx.PropertyName._explosionParticles)
			{
				this._explosionParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001683 RID: 5763 RVA: 0x00089F8C File Offset: 0x0008818C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NKaiserCrabBossExplosionVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			if ((ref name) == NKaiserCrabBossExplosionVfx.PropertyName._explosionParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._explosionParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001684 RID: 5764 RVA: 0x00089FE4 File Offset: 0x000881E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NKaiserCrabBossExplosionVfx.PropertyName._parent, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKaiserCrabBossExplosionVfx.PropertyName._explosionParticles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001685 RID: 5765 RVA: 0x0008A03A File Offset: 0x0008823A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NKaiserCrabBossExplosionVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
			info.AddProperty(NKaiserCrabBossExplosionVfx.PropertyName._explosionParticles, Variant.From<GpuParticles2D>(ref this._explosionParticles));
		}

		/// <inheritdoc />
		// Token: 0x06001686 RID: 5766 RVA: 0x0008A070 File Offset: 0x00088270
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NKaiserCrabBossExplosionVfx.PropertyName._parent, ref variant))
			{
				this._parent = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NKaiserCrabBossExplosionVfx.PropertyName._explosionParticles, ref variant2))
			{
				this._explosionParticles = variant2.As<GpuParticles2D>();
			}
		}

		// Token: 0x04000802 RID: 2050
		[Nullable(1)]
		private MegaSprite _megaSprite;

		// Token: 0x04000803 RID: 2051
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000804 RID: 2052
		[Nullable(1)]
		private GpuParticles2D _explosionParticles;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001084 RID: 4228
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040040EE RID: 16622
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x040040EF RID: 16623
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnLeftEmbersStart' method.
			/// </summary>
			// Token: 0x040040F0 RID: 16624
			public static readonly StringName OnLeftEmbersStart = "OnLeftEmbersStart";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001085 RID: 4229
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x040040F1 RID: 16625
			public static readonly StringName _parent = "_parent";

			/// <summary>
			/// Cached name for the '_explosionParticles' field.
			/// </summary>
			// Token: 0x040040F2 RID: 16626
			public static readonly StringName _explosionParticles = "_explosionParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001086 RID: 4230
		public class SignalName : Node.SignalName
		{
		}
	}
}

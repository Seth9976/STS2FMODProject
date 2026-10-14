using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x02000181 RID: 385
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NArchitectVfx.cs")]
	public class NArchitectVfx : Node
	{
		// Token: 0x06001295 RID: 4757 RVA: 0x0006EF2C File Offset: 0x0006D12C
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._innerTrail = this._parent.GetNode<NBasicTrail>("TrailSlot/TrailInner");
			this._outerTrail = this._parent.GetNode<NBasicTrail>("TrailSlot/TrailOuter");
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("idle_loop", true, 0);
				animState.SetAnimation("_tracks/head_normal", true, 1);
			});
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0006EFDC File Offset: 0x0006D1DC
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "trail_start")
			{
				this.StartTrail();
				return;
			}
			if (!(eventName == "trail_end"))
			{
				return;
			}
			this.EndTrail();
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0006F028 File Offset: 0x0006D228
		private void StartTrail()
		{
			this._innerTrail.Visible = true;
			this._outerTrail.Visible = true;
			this._innerTrail.ClearPoints();
			this._outerTrail.ClearPoints();
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0006F058 File Offset: 0x0006D258
		private void EndTrail()
		{
			this._innerTrail.Visible = false;
			this._outerTrail.Visible = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001299 RID: 4761 RVA: 0x0006F074 File Offset: 0x0006D274
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NArchitectVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NArchitectVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NArchitectVfx.MethodName.StartTrail, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NArchitectVfx.MethodName.EndTrail, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600129A RID: 4762 RVA: 0x0006F1FC File Offset: 0x0006D3FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NArchitectVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NArchitectVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NArchitectVfx.MethodName.StartTrail && args.Count == 0)
			{
				this.StartTrail();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NArchitectVfx.MethodName.EndTrail && args.Count == 0)
			{
				this.EndTrail();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600129B RID: 4763 RVA: 0x0006F2DC File Offset: 0x0006D4DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NArchitectVfx.MethodName._Ready || (ref method) == NArchitectVfx.MethodName.OnAnimationEvent || (ref method) == NArchitectVfx.MethodName.StartTrail || (ref method) == NArchitectVfx.MethodName.EndTrail || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600129C RID: 4764 RVA: 0x0006F32C File Offset: 0x0006D52C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NArchitectVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NArchitectVfx.PropertyName._innerTrail)
			{
				this._innerTrail = VariantUtils.ConvertTo<NBasicTrail>(ref value);
				return true;
			}
			if ((ref name) == NArchitectVfx.PropertyName._outerTrail)
			{
				this._outerTrail = VariantUtils.ConvertTo<NBasicTrail>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600129D RID: 4765 RVA: 0x0006F394 File Offset: 0x0006D594
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NArchitectVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			if ((ref name) == NArchitectVfx.PropertyName._innerTrail)
			{
				value = VariantUtils.CreateFrom<NBasicTrail>(ref this._innerTrail);
				return true;
			}
			if ((ref name) == NArchitectVfx.PropertyName._outerTrail)
			{
				value = VariantUtils.CreateFrom<NBasicTrail>(ref this._outerTrail);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600129E RID: 4766 RVA: 0x0006F40C File Offset: 0x0006D60C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NArchitectVfx.PropertyName._parent, 0L, "", 4096L, false),
				new PropertyInfo(24L, NArchitectVfx.PropertyName._innerTrail, 0L, "", 4096L, false),
				new PropertyInfo(24L, NArchitectVfx.PropertyName._outerTrail, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600129F RID: 4767 RVA: 0x0006F484 File Offset: 0x0006D684
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NArchitectVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
			info.AddProperty(NArchitectVfx.PropertyName._innerTrail, Variant.From<NBasicTrail>(ref this._innerTrail));
			info.AddProperty(NArchitectVfx.PropertyName._outerTrail, Variant.From<NBasicTrail>(ref this._outerTrail));
		}

		/// <inheritdoc />
		// Token: 0x060012A0 RID: 4768 RVA: 0x0006F4DC File Offset: 0x0006D6DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NArchitectVfx.PropertyName._parent, ref variant))
			{
				this._parent = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NArchitectVfx.PropertyName._innerTrail, ref variant2))
			{
				this._innerTrail = variant2.As<NBasicTrail>();
			}
			Variant variant3;
			if (info.TryGetProperty(NArchitectVfx.PropertyName._outerTrail, ref variant3))
			{
				this._outerTrail = variant3.As<NBasicTrail>();
			}
		}

		// Token: 0x0400069A RID: 1690
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x0400069B RID: 1691
		[Nullable(1)]
		private MegaSprite _animController;

		// Token: 0x0400069C RID: 1692
		[Nullable(1)]
		private NBasicTrail _innerTrail;

		// Token: 0x0400069D RID: 1693
		[Nullable(1)]
		private NBasicTrail _outerTrail;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F92 RID: 3986
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E34 RID: 15924
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04003E35 RID: 15925
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'StartTrail' method.
			/// </summary>
			// Token: 0x04003E36 RID: 15926
			public static readonly StringName StartTrail = "StartTrail";

			/// <summary>
			/// Cached name for the 'EndTrail' method.
			/// </summary>
			// Token: 0x04003E37 RID: 15927
			public static readonly StringName EndTrail = "EndTrail";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F93 RID: 3987
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04003E38 RID: 15928
			public static readonly StringName _parent = "_parent";

			/// <summary>
			/// Cached name for the '_innerTrail' field.
			/// </summary>
			// Token: 0x04003E39 RID: 15929
			public static readonly StringName _innerTrail = "_innerTrail";

			/// <summary>
			/// Cached name for the '_outerTrail' field.
			/// </summary>
			// Token: 0x04003E3A RID: 15930
			public static readonly StringName _outerTrail = "_outerTrail";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F94 RID: 3988
		public class SignalName : Node.SignalName
		{
		}
	}
}

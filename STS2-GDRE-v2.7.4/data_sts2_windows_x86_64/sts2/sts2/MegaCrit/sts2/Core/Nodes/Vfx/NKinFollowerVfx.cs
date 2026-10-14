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
	// Token: 0x020001C4 RID: 452
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NKinFollowerVfx.cs")]
	public class NKinFollowerVfx : Node
	{
		// Token: 0x060016A0 RID: 5792 RVA: 0x0008B30C File Offset: 0x0008950C
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._trail1 = this._parent.GetNode<NBasicTrail>("Boomerang1Slot/Trail");
			this._trail2 = this._parent.GetNode<NBasicTrail>("Boomerang2Slot/Trail");
			this._hay = this._parent.GetNode<GpuParticles2D>("HaySlot/HayParticles");
			this._trail1.Visible = false;
			this._trail2.Visible = false;
			this._hay.Emitting = false;
			this._hay.OneShot = true;
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x0008B3DC File Offset: 0x000895DC
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "start_trail1")
			{
				this.StartTrail1();
				return;
			}
			if (eventName == "end_trail1")
			{
				this.EndTrail1();
				return;
			}
			if (eventName == "start_trail2")
			{
				this.StartTrail2();
				return;
			}
			if (eventName == "end_trail2")
			{
				this.EndTrail2();
				return;
			}
			if (!(eventName == "start_hay"))
			{
				return;
			}
			this.StartHay();
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x0008B464 File Offset: 0x00089664
		private void StartTrail1()
		{
			this._trail1.ClearPoints();
			this._trail1.Visible = true;
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x0008B47D File Offset: 0x0008967D
		private void StartTrail2()
		{
			this._trail2.ClearPoints();
			this._trail2.Visible = true;
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x0008B496 File Offset: 0x00089696
		private void EndTrail1()
		{
			this._trail1.Visible = false;
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x0008B4A4 File Offset: 0x000896A4
		private void EndTrail2()
		{
			this._trail2.Visible = false;
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x0008B4B2 File Offset: 0x000896B2
		private void StartHay()
		{
			this._hay.Restart();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060016A7 RID: 5799 RVA: 0x0008B4C0 File Offset: 0x000896C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NKinFollowerVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKinFollowerVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NKinFollowerVfx.MethodName.StartTrail1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKinFollowerVfx.MethodName.StartTrail2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKinFollowerVfx.MethodName.EndTrail1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKinFollowerVfx.MethodName.EndTrail2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKinFollowerVfx.MethodName.StartHay, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060016A8 RID: 5800 RVA: 0x0008B6D4 File Offset: 0x000898D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NKinFollowerVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinFollowerVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinFollowerVfx.MethodName.StartTrail1 && args.Count == 0)
			{
				this.StartTrail1();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinFollowerVfx.MethodName.StartTrail2 && args.Count == 0)
			{
				this.StartTrail2();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinFollowerVfx.MethodName.EndTrail1 && args.Count == 0)
			{
				this.EndTrail1();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinFollowerVfx.MethodName.EndTrail2 && args.Count == 0)
			{
				this.EndTrail2();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinFollowerVfx.MethodName.StartHay && args.Count == 0)
			{
				this.StartHay();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060016A9 RID: 5801 RVA: 0x0008B824 File Offset: 0x00089A24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NKinFollowerVfx.MethodName._Ready || (ref method) == NKinFollowerVfx.MethodName.OnAnimationEvent || (ref method) == NKinFollowerVfx.MethodName.StartTrail1 || (ref method) == NKinFollowerVfx.MethodName.StartTrail2 || (ref method) == NKinFollowerVfx.MethodName.EndTrail1 || (ref method) == NKinFollowerVfx.MethodName.EndTrail2 || (ref method) == NKinFollowerVfx.MethodName.StartHay || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060016AA RID: 5802 RVA: 0x0008B8A4 File Offset: 0x00089AA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NKinFollowerVfx.PropertyName._trail1)
			{
				this._trail1 = VariantUtils.ConvertTo<NBasicTrail>(ref value);
				return true;
			}
			if ((ref name) == NKinFollowerVfx.PropertyName._trail2)
			{
				this._trail2 = VariantUtils.ConvertTo<NBasicTrail>(ref value);
				return true;
			}
			if ((ref name) == NKinFollowerVfx.PropertyName._hay)
			{
				this._hay = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKinFollowerVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060016AB RID: 5803 RVA: 0x0008B928 File Offset: 0x00089B28
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NKinFollowerVfx.PropertyName._trail1)
			{
				value = VariantUtils.CreateFrom<NBasicTrail>(ref this._trail1);
				return true;
			}
			if ((ref name) == NKinFollowerVfx.PropertyName._trail2)
			{
				value = VariantUtils.CreateFrom<NBasicTrail>(ref this._trail2);
				return true;
			}
			if ((ref name) == NKinFollowerVfx.PropertyName._hay)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hay);
				return true;
			}
			if ((ref name) == NKinFollowerVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060016AC RID: 5804 RVA: 0x0008B9C0 File Offset: 0x00089BC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NKinFollowerVfx.PropertyName._trail1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKinFollowerVfx.PropertyName._trail2, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKinFollowerVfx.PropertyName._hay, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKinFollowerVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060016AD RID: 5805 RVA: 0x0008BA58 File Offset: 0x00089C58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NKinFollowerVfx.PropertyName._trail1, Variant.From<NBasicTrail>(ref this._trail1));
			info.AddProperty(NKinFollowerVfx.PropertyName._trail2, Variant.From<NBasicTrail>(ref this._trail2));
			info.AddProperty(NKinFollowerVfx.PropertyName._hay, Variant.From<GpuParticles2D>(ref this._hay));
			info.AddProperty(NKinFollowerVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x060016AE RID: 5806 RVA: 0x0008BAC4 File Offset: 0x00089CC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NKinFollowerVfx.PropertyName._trail1, ref variant))
			{
				this._trail1 = variant.As<NBasicTrail>();
			}
			Variant variant2;
			if (info.TryGetProperty(NKinFollowerVfx.PropertyName._trail2, ref variant2))
			{
				this._trail2 = variant2.As<NBasicTrail>();
			}
			Variant variant3;
			if (info.TryGetProperty(NKinFollowerVfx.PropertyName._hay, ref variant3))
			{
				this._hay = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NKinFollowerVfx.PropertyName._parent, ref variant4))
			{
				this._parent = variant4.As<Node2D>();
			}
		}

		// Token: 0x04000810 RID: 2064
		[Nullable(1)]
		private NBasicTrail _trail1;

		// Token: 0x04000811 RID: 2065
		[Nullable(1)]
		private NBasicTrail _trail2;

		// Token: 0x04000812 RID: 2066
		[Nullable(1)]
		private GpuParticles2D _hay;

		// Token: 0x04000813 RID: 2067
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000814 RID: 2068
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200108A RID: 4234
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400410C RID: 16652
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x0400410D RID: 16653
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'StartTrail1' method.
			/// </summary>
			// Token: 0x0400410E RID: 16654
			public static readonly StringName StartTrail1 = "StartTrail1";

			/// <summary>
			/// Cached name for the 'StartTrail2' method.
			/// </summary>
			// Token: 0x0400410F RID: 16655
			public static readonly StringName StartTrail2 = "StartTrail2";

			/// <summary>
			/// Cached name for the 'EndTrail1' method.
			/// </summary>
			// Token: 0x04004110 RID: 16656
			public static readonly StringName EndTrail1 = "EndTrail1";

			/// <summary>
			/// Cached name for the 'EndTrail2' method.
			/// </summary>
			// Token: 0x04004111 RID: 16657
			public static readonly StringName EndTrail2 = "EndTrail2";

			/// <summary>
			/// Cached name for the 'StartHay' method.
			/// </summary>
			// Token: 0x04004112 RID: 16658
			public static readonly StringName StartHay = "StartHay";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200108B RID: 4235
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_trail1' field.
			/// </summary>
			// Token: 0x04004113 RID: 16659
			public static readonly StringName _trail1 = "_trail1";

			/// <summary>
			/// Cached name for the '_trail2' field.
			/// </summary>
			// Token: 0x04004114 RID: 16660
			public static readonly StringName _trail2 = "_trail2";

			/// <summary>
			/// Cached name for the '_hay' field.
			/// </summary>
			// Token: 0x04004115 RID: 16661
			public static readonly StringName _hay = "_hay";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004116 RID: 16662
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200108C RID: 4236
		public class SignalName : Node.SignalName
		{
		}
	}
}

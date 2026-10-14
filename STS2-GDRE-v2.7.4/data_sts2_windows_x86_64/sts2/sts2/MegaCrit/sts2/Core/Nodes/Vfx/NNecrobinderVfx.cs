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
	// Token: 0x020001D7 RID: 471
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NNecrobinderVfx.cs")]
	public class NNecrobinderVfx : Node
	{
		// Token: 0x060017C7 RID: 6087 RVA: 0x000944FC File Offset: 0x000926FC
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._headRef = this._parent.GetNode<Node2D>("HeadBoneNode");
			this._scytheFireParticles1 = this._parent.GetNodeOrNull<GpuParticles2D>("ScytheVfxSlot1/ScytheParticles");
			this._scytheFireParticles2 = this._parent.GetNodeOrNull<GpuParticles2D>("ScytheVfxSlot2/ScytheParticles");
			GpuParticles2D scytheFireParticles = this._scytheFireParticles1;
			if (scytheFireParticles != null)
			{
				scytheFireParticles.SetEmitting(false);
			}
			GpuParticles2D scytheFireParticles2 = this._scytheFireParticles2;
			if (scytheFireParticles2 != null)
			{
				scytheFireParticles2.SetEmitting(false);
			}
			GpuParticles2D scytheFireParticles3 = this._scytheFireParticles1;
			if (scytheFireParticles3 != null)
			{
				scytheFireParticles3.SetOneShot(true);
			}
			GpuParticles2D scytheFireParticles4 = this._scytheFireParticles2;
			if (scytheFireParticles4 != null)
			{
				scytheFireParticles4.SetOneShot(true);
			}
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.UpdateFlameVisibility)));
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x00094600 File Offset: 0x00092800
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "scythe_fx1")
			{
				this.OnScytheFlame1();
				return;
			}
			if (!(eventName == "scythe_fx2"))
			{
				return;
			}
			this.OnScytheFlame2();
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0009464C File Offset: 0x0009284C
		[NullableContext(1)]
		private void UpdateFlameVisibility(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			this._headRef.Visible = new MegaAnimationState(animationState).GetCurrentAnimationName(0) != "die";
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x00094674 File Offset: 0x00092874
		private void OnScytheFlame1()
		{
			GpuParticles2D scytheFireParticles = this._scytheFireParticles1;
			if (scytheFireParticles == null)
			{
				return;
			}
			scytheFireParticles.Restart();
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x00094686 File Offset: 0x00092886
		private void OnScytheFlame2()
		{
			GpuParticles2D scytheFireParticles = this._scytheFireParticles2;
			if (scytheFireParticles == null)
			{
				return;
			}
			scytheFireParticles.Restart();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060017CC RID: 6092 RVA: 0x00094698 File Offset: 0x00092898
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NNecrobinderVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNecrobinderVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NNecrobinderVfx.MethodName.UpdateFlameVisibility, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NNecrobinderVfx.MethodName.OnScytheFlame1, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NNecrobinderVfx.MethodName.OnScytheFlame2, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060017CD RID: 6093 RVA: 0x000948D4 File Offset: 0x00092AD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NNecrobinderVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNecrobinderVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNecrobinderVfx.MethodName.UpdateFlameVisibility && args.Count == 3)
			{
				this.UpdateFlameVisibility(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNecrobinderVfx.MethodName.OnScytheFlame1 && args.Count == 0)
			{
				this.OnScytheFlame1();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NNecrobinderVfx.MethodName.OnScytheFlame2 && args.Count == 0)
			{
				this.OnScytheFlame2();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060017CE RID: 6094 RVA: 0x00094A00 File Offset: 0x00092C00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NNecrobinderVfx.MethodName._Ready || (ref method) == NNecrobinderVfx.MethodName.OnAnimationEvent || (ref method) == NNecrobinderVfx.MethodName.UpdateFlameVisibility || (ref method) == NNecrobinderVfx.MethodName.OnScytheFlame1 || (ref method) == NNecrobinderVfx.MethodName.OnScytheFlame2 || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060017CF RID: 6095 RVA: 0x00094A60 File Offset: 0x00092C60
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NNecrobinderVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NNecrobinderVfx.PropertyName._headRef)
			{
				this._headRef = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NNecrobinderVfx.PropertyName._scytheFireParticles1)
			{
				this._scytheFireParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NNecrobinderVfx.PropertyName._scytheFireParticles2)
			{
				this._scytheFireParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060017D0 RID: 6096 RVA: 0x00094AE4 File Offset: 0x00092CE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NNecrobinderVfx.PropertyName._parent)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
				return true;
			}
			if ((ref name) == NNecrobinderVfx.PropertyName._headRef)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._headRef);
				return true;
			}
			if ((ref name) == NNecrobinderVfx.PropertyName._scytheFireParticles1)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._scytheFireParticles1);
				return true;
			}
			if ((ref name) == NNecrobinderVfx.PropertyName._scytheFireParticles2)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._scytheFireParticles2);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060017D1 RID: 6097 RVA: 0x00094B7C File Offset: 0x00092D7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NNecrobinderVfx.PropertyName._parent, 0L, "", 4096L, false),
				new PropertyInfo(24L, NNecrobinderVfx.PropertyName._headRef, 0L, "", 4096L, false),
				new PropertyInfo(24L, NNecrobinderVfx.PropertyName._scytheFireParticles1, 0L, "", 4096L, false),
				new PropertyInfo(24L, NNecrobinderVfx.PropertyName._scytheFireParticles2, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060017D2 RID: 6098 RVA: 0x00094C14 File Offset: 0x00092E14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NNecrobinderVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
			info.AddProperty(NNecrobinderVfx.PropertyName._headRef, Variant.From<Node2D>(ref this._headRef));
			info.AddProperty(NNecrobinderVfx.PropertyName._scytheFireParticles1, Variant.From<GpuParticles2D>(ref this._scytheFireParticles1));
			info.AddProperty(NNecrobinderVfx.PropertyName._scytheFireParticles2, Variant.From<GpuParticles2D>(ref this._scytheFireParticles2));
		}

		/// <inheritdoc />
		// Token: 0x060017D3 RID: 6099 RVA: 0x00094C80 File Offset: 0x00092E80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NNecrobinderVfx.PropertyName._parent, ref variant))
			{
				this._parent = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NNecrobinderVfx.PropertyName._headRef, ref variant2))
			{
				this._headRef = variant2.As<Node2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NNecrobinderVfx.PropertyName._scytheFireParticles1, ref variant3))
			{
				this._scytheFireParticles1 = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NNecrobinderVfx.PropertyName._scytheFireParticles2, ref variant4))
			{
				this._scytheFireParticles2 = variant4.As<GpuParticles2D>();
			}
		}

		// Token: 0x04000898 RID: 2200
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000899 RID: 2201
		[Nullable(1)]
		private MegaSprite _animController;

		// Token: 0x0400089A RID: 2202
		[Nullable(1)]
		private Node2D _headRef;

		// Token: 0x0400089B RID: 2203
		[Nullable(2)]
		private GpuParticles2D _scytheFireParticles1;

		// Token: 0x0400089C RID: 2204
		[Nullable(2)]
		private GpuParticles2D _scytheFireParticles2;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010D3 RID: 4307
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040041FC RID: 16892
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x040041FD RID: 16893
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'UpdateFlameVisibility' method.
			/// </summary>
			// Token: 0x040041FE RID: 16894
			public static readonly StringName UpdateFlameVisibility = "UpdateFlameVisibility";

			/// <summary>
			/// Cached name for the 'OnScytheFlame1' method.
			/// </summary>
			// Token: 0x040041FF RID: 16895
			public static readonly StringName OnScytheFlame1 = "OnScytheFlame1";

			/// <summary>
			/// Cached name for the 'OnScytheFlame2' method.
			/// </summary>
			// Token: 0x04004200 RID: 16896
			public static readonly StringName OnScytheFlame2 = "OnScytheFlame2";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010D4 RID: 4308
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004201 RID: 16897
			public static readonly StringName _parent = "_parent";

			/// <summary>
			/// Cached name for the '_headRef' field.
			/// </summary>
			// Token: 0x04004202 RID: 16898
			public static readonly StringName _headRef = "_headRef";

			/// <summary>
			/// Cached name for the '_scytheFireParticles1' field.
			/// </summary>
			// Token: 0x04004203 RID: 16899
			public static readonly StringName _scytheFireParticles1 = "_scytheFireParticles1";

			/// <summary>
			/// Cached name for the '_scytheFireParticles2' field.
			/// </summary>
			// Token: 0x04004204 RID: 16900
			public static readonly StringName _scytheFireParticles2 = "_scytheFireParticles2";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010D5 RID: 4309
		public class SignalName : Node.SignalName
		{
		}
	}
}

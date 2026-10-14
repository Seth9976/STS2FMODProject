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
	// Token: 0x020001CE RID: 462
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NLostAndForgottenVfx.cs")]
	public class NLostAndForgottenVfx : Node
	{
		// Token: 0x06001743 RID: 5955 RVA: 0x000905E0 File Offset: 0x0008E7E0
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._dustParticles = this._parent.GetNode<GpuParticles2D>("GranuleEmitterBone/DustParticles");
			this._dustParticles.Emitting = false;
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("die", true, 0);
			});
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00090680 File Offset: 0x0008E880
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "start_dust")
			{
				this.OnDustStart();
				return;
			}
			if (!(eventName == "stop_dust"))
			{
				return;
			}
			this.OnDustStop();
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x000906CC File Offset: 0x0008E8CC
		private void OnDustStart()
		{
			this._dustParticles.Emitting = true;
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x000906DA File Offset: 0x0008E8DA
		private void OnDustStop()
		{
			this._dustParticles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001747 RID: 5959 RVA: 0x000906E8 File Offset: 0x0008E8E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NLostAndForgottenVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLostAndForgottenVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NLostAndForgottenVfx.MethodName.OnDustStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NLostAndForgottenVfx.MethodName.OnDustStop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001748 RID: 5960 RVA: 0x00090870 File Offset: 0x0008EA70
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NLostAndForgottenVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLostAndForgottenVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLostAndForgottenVfx.MethodName.OnDustStart && args.Count == 0)
			{
				this.OnDustStart();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NLostAndForgottenVfx.MethodName.OnDustStop && args.Count == 0)
			{
				this.OnDustStop();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001749 RID: 5961 RVA: 0x00090950 File Offset: 0x0008EB50
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NLostAndForgottenVfx.MethodName._Ready || (ref method) == NLostAndForgottenVfx.MethodName.OnAnimationEvent || (ref method) == NLostAndForgottenVfx.MethodName.OnDustStart || (ref method) == NLostAndForgottenVfx.MethodName.OnDustStop || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600174A RID: 5962 RVA: 0x000909A0 File Offset: 0x0008EBA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NLostAndForgottenVfx.PropertyName._dustParticles)
			{
				this._dustParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NLostAndForgottenVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600174B RID: 5963 RVA: 0x000909E0 File Offset: 0x0008EBE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NLostAndForgottenVfx.PropertyName._dustParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dustParticles);
				return true;
			}
			if ((ref name) == NLostAndForgottenVfx.PropertyName._parent)
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
		// Token: 0x0600174C RID: 5964 RVA: 0x00090A38 File Offset: 0x0008EC38
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NLostAndForgottenVfx.PropertyName._dustParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NLostAndForgottenVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600174D RID: 5965 RVA: 0x00090A8E File Offset: 0x0008EC8E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NLostAndForgottenVfx.PropertyName._dustParticles, Variant.From<GpuParticles2D>(ref this._dustParticles));
			info.AddProperty(NLostAndForgottenVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x0600174E RID: 5966 RVA: 0x00090AC4 File Offset: 0x0008ECC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NLostAndForgottenVfx.PropertyName._dustParticles, ref variant))
			{
				this._dustParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NLostAndForgottenVfx.PropertyName._parent, ref variant2))
			{
				this._parent = variant2.As<Node2D>();
			}
		}

		// Token: 0x04000853 RID: 2131
		[Nullable(1)]
		private GpuParticles2D _dustParticles;

		// Token: 0x04000854 RID: 2132
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000855 RID: 2133
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010AF RID: 4271
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400418B RID: 16779
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x0400418C RID: 16780
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnDustStart' method.
			/// </summary>
			// Token: 0x0400418D RID: 16781
			public static readonly StringName OnDustStart = "OnDustStart";

			/// <summary>
			/// Cached name for the 'OnDustStop' method.
			/// </summary>
			// Token: 0x0400418E RID: 16782
			public static readonly StringName OnDustStop = "OnDustStop";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010B0 RID: 4272
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_dustParticles' field.
			/// </summary>
			// Token: 0x0400418F RID: 16783
			public static readonly StringName _dustParticles = "_dustParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004190 RID: 16784
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010B1 RID: 4273
		public class SignalName : Node.SignalName
		{
		}
	}
}

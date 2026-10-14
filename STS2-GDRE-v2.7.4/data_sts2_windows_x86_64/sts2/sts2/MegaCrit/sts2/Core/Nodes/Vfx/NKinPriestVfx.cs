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
	// Token: 0x020001C7 RID: 455
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NKinPriestVfx.cs")]
	public class NKinPriestVfx : Node
	{
		// Token: 0x060016CB RID: 5835 RVA: 0x0008C818 File Offset: 0x0008AA18
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._sparkParticles = this._parent.GetNode<GpuParticles2D>("TorchFireBone/SparkParticles");
			this._sparkParticles.Emitting = false;
			this._beamVfx = this._parent.GetNode<NKinPriestBeamVfx>("Beam");
			this.RunWhenSpineReady(this._animController, delegate(MegaAnimationState animState)
			{
				animState.SetAnimation("attack_laser", true, 0);
			});
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x0008C8D4 File Offset: 0x0008AAD4
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "sparks_start")
			{
				this.StartSparks();
				return;
			}
			if (eventName == "sparks_end")
			{
				this.EndSparks();
				return;
			}
			if (!(eventName == "laser_fire"))
			{
				return;
			}
			this.FireLaser();
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x0008C934 File Offset: 0x0008AB34
		private void StartSparks()
		{
			this._sparkParticles.Emitting = true;
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x0008C942 File Offset: 0x0008AB42
		private void EndSparks()
		{
			this._sparkParticles.Emitting = false;
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x0008C950 File Offset: 0x0008AB50
		private void FireLaser()
		{
			this._beamVfx.Fire();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060016D0 RID: 5840 RVA: 0x0008C960 File Offset: 0x0008AB60
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NKinPriestVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKinPriestVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NKinPriestVfx.MethodName.StartSparks, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKinPriestVfx.MethodName.EndSparks, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NKinPriestVfx.MethodName.FireLaser, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060016D1 RID: 5841 RVA: 0x0008CB14 File Offset: 0x0008AD14
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NKinPriestVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinPriestVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinPriestVfx.MethodName.StartSparks && args.Count == 0)
			{
				this.StartSparks();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinPriestVfx.MethodName.EndSparks && args.Count == 0)
			{
				this.EndSparks();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NKinPriestVfx.MethodName.FireLaser && args.Count == 0)
			{
				this.FireLaser();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060016D2 RID: 5842 RVA: 0x0008CC18 File Offset: 0x0008AE18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NKinPriestVfx.MethodName._Ready || (ref method) == NKinPriestVfx.MethodName.OnAnimationEvent || (ref method) == NKinPriestVfx.MethodName.StartSparks || (ref method) == NKinPriestVfx.MethodName.EndSparks || (ref method) == NKinPriestVfx.MethodName.FireLaser || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060016D3 RID: 5843 RVA: 0x0008CC78 File Offset: 0x0008AE78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NKinPriestVfx.PropertyName._sparkParticles)
			{
				this._sparkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NKinPriestVfx.PropertyName._beamVfx)
			{
				this._beamVfx = VariantUtils.ConvertTo<NKinPriestBeamVfx>(ref value);
				return true;
			}
			if ((ref name) == NKinPriestVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060016D4 RID: 5844 RVA: 0x0008CCE0 File Offset: 0x0008AEE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NKinPriestVfx.PropertyName._sparkParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sparkParticles);
				return true;
			}
			if ((ref name) == NKinPriestVfx.PropertyName._beamVfx)
			{
				value = VariantUtils.CreateFrom<NKinPriestBeamVfx>(ref this._beamVfx);
				return true;
			}
			if ((ref name) == NKinPriestVfx.PropertyName._parent)
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
		// Token: 0x060016D5 RID: 5845 RVA: 0x0008CD58 File Offset: 0x0008AF58
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NKinPriestVfx.PropertyName._sparkParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKinPriestVfx.PropertyName._beamVfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NKinPriestVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060016D6 RID: 5846 RVA: 0x0008CDD0 File Offset: 0x0008AFD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NKinPriestVfx.PropertyName._sparkParticles, Variant.From<GpuParticles2D>(ref this._sparkParticles));
			info.AddProperty(NKinPriestVfx.PropertyName._beamVfx, Variant.From<NKinPriestBeamVfx>(ref this._beamVfx));
			info.AddProperty(NKinPriestVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x060016D7 RID: 5847 RVA: 0x0008CE28 File Offset: 0x0008B028
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NKinPriestVfx.PropertyName._sparkParticles, ref variant))
			{
				this._sparkParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NKinPriestVfx.PropertyName._beamVfx, ref variant2))
			{
				this._beamVfx = variant2.As<NKinPriestBeamVfx>();
			}
			Variant variant3;
			if (info.TryGetProperty(NKinPriestVfx.PropertyName._parent, ref variant3))
			{
				this._parent = variant3.As<Node2D>();
			}
		}

		// Token: 0x04000823 RID: 2083
		[Nullable(1)]
		private GpuParticles2D _sparkParticles;

		// Token: 0x04000824 RID: 2084
		[Nullable(1)]
		private NKinPriestBeamVfx _beamVfx;

		// Token: 0x04000825 RID: 2085
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000826 RID: 2086
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001094 RID: 4244
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400412A RID: 16682
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x0400412B RID: 16683
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'StartSparks' method.
			/// </summary>
			// Token: 0x0400412C RID: 16684
			public static readonly StringName StartSparks = "StartSparks";

			/// <summary>
			/// Cached name for the 'EndSparks' method.
			/// </summary>
			// Token: 0x0400412D RID: 16685
			public static readonly StringName EndSparks = "EndSparks";

			/// <summary>
			/// Cached name for the 'FireLaser' method.
			/// </summary>
			// Token: 0x0400412E RID: 16686
			public static readonly StringName FireLaser = "FireLaser";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001095 RID: 4245
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_sparkParticles' field.
			/// </summary>
			// Token: 0x0400412F RID: 16687
			public static readonly StringName _sparkParticles = "_sparkParticles";

			/// <summary>
			/// Cached name for the '_beamVfx' field.
			/// </summary>
			// Token: 0x04004130 RID: 16688
			public static readonly StringName _beamVfx = "_beamVfx";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004131 RID: 16689
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001096 RID: 4246
		public class SignalName : Node.SignalName
		{
		}
	}
}

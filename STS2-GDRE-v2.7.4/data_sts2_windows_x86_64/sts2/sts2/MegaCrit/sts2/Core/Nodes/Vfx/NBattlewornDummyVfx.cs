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
	// Token: 0x02000184 RID: 388
	[ScriptPath("res://src/Core/Nodes/Vfx/NBattlewornDummyVfx.cs")]
	public class NBattlewornDummyVfx : Node
	{
		// Token: 0x060012BD RID: 4797 RVA: 0x0007021C File Offset: 0x0006E41C
		public override void _Ready()
		{
			this._megaSprite = new MegaSprite(base.GetParent<Node2D>());
			this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._damageParticles = base.GetParent<Node2D>().GetNode<GpuParticles2D>("ParticlesSlot/DamageParticles");
			this._damageParticles.Emitting = false;
			this._damageParticles.OneShot = true;
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00070290 File Offset: 0x0006E490
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "fire_chips")
			{
				this.OnDamageChips();
			}
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x000702C7 File Offset: 0x0006E4C7
		private void OnDamageChips()
		{
			this._damageParticles.Restart();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060012C0 RID: 4800 RVA: 0x000702D4 File Offset: 0x0006E4D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NBattlewornDummyVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBattlewornDummyVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NBattlewornDummyVfx.MethodName.OnDamageChips, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060012C1 RID: 4801 RVA: 0x0007042C File Offset: 0x0006E62C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBattlewornDummyVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBattlewornDummyVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBattlewornDummyVfx.MethodName.OnDamageChips && args.Count == 0)
			{
				this.OnDamageChips();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060012C2 RID: 4802 RVA: 0x000704E6 File Offset: 0x0006E6E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBattlewornDummyVfx.MethodName._Ready || (ref method) == NBattlewornDummyVfx.MethodName.OnAnimationEvent || (ref method) == NBattlewornDummyVfx.MethodName.OnDamageChips || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060012C3 RID: 4803 RVA: 0x0007051C File Offset: 0x0006E71C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBattlewornDummyVfx.PropertyName._damageParticles)
			{
				this._damageParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060012C4 RID: 4804 RVA: 0x00070541 File Offset: 0x0006E741
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBattlewornDummyVfx.PropertyName._damageParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._damageParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060012C5 RID: 4805 RVA: 0x0007056C File Offset: 0x0006E76C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NBattlewornDummyVfx.PropertyName._damageParticles, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060012C6 RID: 4806 RVA: 0x000705A1 File Offset: 0x0006E7A1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBattlewornDummyVfx.PropertyName._damageParticles, Variant.From<GpuParticles2D>(ref this._damageParticles));
		}

		/// <inheritdoc />
		// Token: 0x060012C7 RID: 4807 RVA: 0x000705C0 File Offset: 0x0006E7C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBattlewornDummyVfx.PropertyName._damageParticles, ref variant))
			{
				this._damageParticles = variant.As<GpuParticles2D>();
			}
		}

		// Token: 0x040006A7 RID: 1703
		[Nullable(1)]
		private MegaSprite _megaSprite;

		// Token: 0x040006A8 RID: 1704
		[Nullable(1)]
		private GpuParticles2D _damageParticles;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F9C RID: 3996
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E4E RID: 15950
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04003E4F RID: 15951
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnDamageChips' method.
			/// </summary>
			// Token: 0x04003E50 RID: 15952
			public static readonly StringName OnDamageChips = "OnDamageChips";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F9D RID: 3997
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_damageParticles' field.
			/// </summary>
			// Token: 0x04003E51 RID: 15953
			public static readonly StringName _damageParticles = "_damageParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F9E RID: 3998
		public class SignalName : Node.SignalName
		{
		}
	}
}

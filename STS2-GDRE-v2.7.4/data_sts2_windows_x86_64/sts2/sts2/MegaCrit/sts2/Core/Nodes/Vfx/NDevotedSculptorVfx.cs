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
	// Token: 0x0200019E RID: 414
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NDevotedSculptorVfx.cs")]
	public class NDevotedSculptorVfx : Node
	{
		// Token: 0x06001465 RID: 5221 RVA: 0x0007B3D0 File Offset: 0x000795D0
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._voiceParticles = this._parent.GetNode<GpuParticles2D>("VoiceBoneNode/VoiceParticles");
			this._voiceParticles.Emitting = false;
			this._voiceParticles.OneShot = true;
			this._attackParticles = this._parent.GetNode<GpuParticles2D>("AttackParticles");
			this._attackParticles.Emitting = false;
			this._attackParticles.OneShot = true;
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0007B484 File Offset: 0x00079684
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "caw")
			{
				this.StartVoice();
				return;
			}
			if (!(eventName == "attack"))
			{
				return;
			}
			this.StartAttack();
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0007B4D0 File Offset: 0x000796D0
		private void StartVoice()
		{
			this._voiceParticles.Restart();
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0007B4DD File Offset: 0x000796DD
		private void StartAttack()
		{
			this._attackParticles.Restart();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001469 RID: 5225 RVA: 0x0007B4EC File Offset: 0x000796EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NDevotedSculptorVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevotedSculptorVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NDevotedSculptorVfx.MethodName.StartVoice, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDevotedSculptorVfx.MethodName.StartAttack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600146A RID: 5226 RVA: 0x0007B674 File Offset: 0x00079874
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDevotedSculptorVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevotedSculptorVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevotedSculptorVfx.MethodName.StartVoice && args.Count == 0)
			{
				this.StartVoice();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDevotedSculptorVfx.MethodName.StartAttack && args.Count == 0)
			{
				this.StartAttack();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600146B RID: 5227 RVA: 0x0007B754 File Offset: 0x00079954
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDevotedSculptorVfx.MethodName._Ready || (ref method) == NDevotedSculptorVfx.MethodName.OnAnimationEvent || (ref method) == NDevotedSculptorVfx.MethodName.StartVoice || (ref method) == NDevotedSculptorVfx.MethodName.StartAttack || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600146C RID: 5228 RVA: 0x0007B7A4 File Offset: 0x000799A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDevotedSculptorVfx.PropertyName._voiceParticles)
			{
				this._voiceParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NDevotedSculptorVfx.PropertyName._attackParticles)
			{
				this._attackParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NDevotedSculptorVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600146D RID: 5229 RVA: 0x0007B80C File Offset: 0x00079A0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDevotedSculptorVfx.PropertyName._voiceParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._voiceParticles);
				return true;
			}
			if ((ref name) == NDevotedSculptorVfx.PropertyName._attackParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._attackParticles);
				return true;
			}
			if ((ref name) == NDevotedSculptorVfx.PropertyName._parent)
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
		// Token: 0x0600146E RID: 5230 RVA: 0x0007B884 File Offset: 0x00079A84
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDevotedSculptorVfx.PropertyName._voiceParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDevotedSculptorVfx.PropertyName._attackParticles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDevotedSculptorVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600146F RID: 5231 RVA: 0x0007B8FC File Offset: 0x00079AFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDevotedSculptorVfx.PropertyName._voiceParticles, Variant.From<GpuParticles2D>(ref this._voiceParticles));
			info.AddProperty(NDevotedSculptorVfx.PropertyName._attackParticles, Variant.From<GpuParticles2D>(ref this._attackParticles));
			info.AddProperty(NDevotedSculptorVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001470 RID: 5232 RVA: 0x0007B954 File Offset: 0x00079B54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDevotedSculptorVfx.PropertyName._voiceParticles, ref variant))
			{
				this._voiceParticles = variant.As<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDevotedSculptorVfx.PropertyName._attackParticles, ref variant2))
			{
				this._attackParticles = variant2.As<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NDevotedSculptorVfx.PropertyName._parent, ref variant3))
			{
				this._parent = variant3.As<Node2D>();
			}
		}

		// Token: 0x04000744 RID: 1860
		[Nullable(1)]
		private GpuParticles2D _voiceParticles;

		// Token: 0x04000745 RID: 1861
		[Nullable(1)]
		private GpuParticles2D _attackParticles;

		// Token: 0x04000746 RID: 1862
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000747 RID: 1863
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001000 RID: 4096
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F80 RID: 16256
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04003F81 RID: 16257
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'StartVoice' method.
			/// </summary>
			// Token: 0x04003F82 RID: 16258
			public static readonly StringName StartVoice = "StartVoice";

			/// <summary>
			/// Cached name for the 'StartAttack' method.
			/// </summary>
			// Token: 0x04003F83 RID: 16259
			public static readonly StringName StartAttack = "StartAttack";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001001 RID: 4097
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_voiceParticles' field.
			/// </summary>
			// Token: 0x04003F84 RID: 16260
			public static readonly StringName _voiceParticles = "_voiceParticles";

			/// <summary>
			/// Cached name for the '_attackParticles' field.
			/// </summary>
			// Token: 0x04003F85 RID: 16261
			public static readonly StringName _attackParticles = "_attackParticles";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04003F86 RID: 16262
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001002 RID: 4098
		public class SignalName : Node.SignalName
		{
		}
	}
}

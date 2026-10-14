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
	// Token: 0x020001AB RID: 427
	[GlobalClass]
	[ScriptPath("res://src/Core/Nodes/Vfx/NFlyconidSporesVfx.cs")]
	public class NFlyconidSporesVfx : Node
	{
		// Token: 0x0600152B RID: 5419 RVA: 0x000812DC File Offset: 0x0007F4DC
		public override void _Ready()
		{
			this._parent = base.GetParent<Node2D>();
			this._animController = new MegaSprite(this._parent);
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._frailSpores = this._parent.GetNode<CpuParticles2D>("SporesAttach/FlyconidSporesVfx/FrailSpores");
			this._frailSpores.Emitting = false;
			this._vulnerableSpores = this._parent.GetNode<CpuParticles2D>("SporesAttach/FlyconidSporesVfx/VulnerableSpores");
			this._vulnerableSpores.Emitting = false;
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x00081394 File Offset: 0x0007F594
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject _, GodotObject __, GodotObject ___, GodotObject spineEvent)
		{
			string eventName = new MegaEvent(spineEvent).GetData().GetEventName();
			if (eventName == "cast_start")
			{
				this._frailSpores.Restart();
				this._vulnerableSpores.Restart();
				return;
			}
			if (!(eventName == "cast_end"))
			{
				return;
			}
			this._frailSpores.Emitting = false;
			this._vulnerableSpores.Emitting = false;
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x00081402 File Offset: 0x0007F602
		public void SetSporeTypeIsVulnerable(bool isVulnerable)
		{
			this._frailSpores.Visible = !isVulnerable;
			this._vulnerableSpores.Visible = isVulnerable;
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0008141F File Offset: 0x0007F61F
		[NullableContext(1)]
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			if (new MegaAnimationState(animationState).GetCurrentAnimationName(0) != "attack")
			{
				this._frailSpores.Emitting = false;
				this._vulnerableSpores.Emitting = false;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600152F RID: 5423 RVA: 0x00081458 File Offset: 0x0007F658
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NFlyconidSporesVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFlyconidSporesVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "__", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "___", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "spineEvent", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NFlyconidSporesVfx.MethodName.SetSporeTypeIsVulnerable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isVulnerable", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NFlyconidSporesVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001530 RID: 5424 RVA: 0x0008168C File Offset: 0x0007F88C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFlyconidSporesVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFlyconidSporesVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFlyconidSporesVfx.MethodName.SetSporeTypeIsVulnerable && args.Count == 1)
			{
				this.SetSporeTypeIsVulnerable(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFlyconidSporesVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001531 RID: 5425 RVA: 0x000817A4 File Offset: 0x0007F9A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFlyconidSporesVfx.MethodName._Ready || (ref method) == NFlyconidSporesVfx.MethodName.OnAnimationEvent || (ref method) == NFlyconidSporesVfx.MethodName.SetSporeTypeIsVulnerable || (ref method) == NFlyconidSporesVfx.MethodName.OnAnimationStart || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001532 RID: 5426 RVA: 0x000817F4 File Offset: 0x0007F9F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NFlyconidSporesVfx.PropertyName._frailSpores)
			{
				this._frailSpores = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NFlyconidSporesVfx.PropertyName._vulnerableSpores)
			{
				this._vulnerableSpores = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NFlyconidSporesVfx.PropertyName._parent)
			{
				this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001533 RID: 5427 RVA: 0x0008185C File Offset: 0x0007FA5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NFlyconidSporesVfx.PropertyName._frailSpores)
			{
				value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._frailSpores);
				return true;
			}
			if ((ref name) == NFlyconidSporesVfx.PropertyName._vulnerableSpores)
			{
				value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._vulnerableSpores);
				return true;
			}
			if ((ref name) == NFlyconidSporesVfx.PropertyName._parent)
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
		// Token: 0x06001534 RID: 5428 RVA: 0x000818D4 File Offset: 0x0007FAD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NFlyconidSporesVfx.PropertyName._frailSpores, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFlyconidSporesVfx.PropertyName._vulnerableSpores, 0L, "", 4096L, false),
				new PropertyInfo(24L, NFlyconidSporesVfx.PropertyName._parent, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001535 RID: 5429 RVA: 0x0008194C File Offset: 0x0007FB4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NFlyconidSporesVfx.PropertyName._frailSpores, Variant.From<CpuParticles2D>(ref this._frailSpores));
			info.AddProperty(NFlyconidSporesVfx.PropertyName._vulnerableSpores, Variant.From<CpuParticles2D>(ref this._vulnerableSpores));
			info.AddProperty(NFlyconidSporesVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
		}

		/// <inheritdoc />
		// Token: 0x06001536 RID: 5430 RVA: 0x000819A4 File Offset: 0x0007FBA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NFlyconidSporesVfx.PropertyName._frailSpores, ref variant))
			{
				this._frailSpores = variant.As<CpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NFlyconidSporesVfx.PropertyName._vulnerableSpores, ref variant2))
			{
				this._vulnerableSpores = variant2.As<CpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NFlyconidSporesVfx.PropertyName._parent, ref variant3))
			{
				this._parent = variant3.As<Node2D>();
			}
		}

		// Token: 0x0400078E RID: 1934
		[Nullable(1)]
		private CpuParticles2D _frailSpores;

		// Token: 0x0400078F RID: 1935
		[Nullable(1)]
		private CpuParticles2D _vulnerableSpores;

		// Token: 0x04000790 RID: 1936
		[Nullable(1)]
		private Node2D _parent;

		// Token: 0x04000791 RID: 1937
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001030 RID: 4144
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004012 RID: 16402
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004013 RID: 16403
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'SetSporeTypeIsVulnerable' method.
			/// </summary>
			// Token: 0x04004014 RID: 16404
			public static readonly StringName SetSporeTypeIsVulnerable = "SetSporeTypeIsVulnerable";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x04004015 RID: 16405
			public static readonly StringName OnAnimationStart = "OnAnimationStart";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001031 RID: 4145
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_frailSpores' field.
			/// </summary>
			// Token: 0x04004016 RID: 16406
			public static readonly StringName _frailSpores = "_frailSpores";

			/// <summary>
			/// Cached name for the '_vulnerableSpores' field.
			/// </summary>
			// Token: 0x04004017 RID: 16407
			public static readonly StringName _vulnerableSpores = "_vulnerableSpores";

			/// <summary>
			/// Cached name for the '_parent' field.
			/// </summary>
			// Token: 0x04004018 RID: 16408
			public static readonly StringName _parent = "_parent";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001032 RID: 4146
		public class SignalName : Node.SignalName
		{
		}
	}
}

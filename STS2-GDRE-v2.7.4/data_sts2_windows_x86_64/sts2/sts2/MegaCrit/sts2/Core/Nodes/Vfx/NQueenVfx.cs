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
	// Token: 0x020001E2 RID: 482
	[ScriptPath("res://src/Core/Nodes/Vfx/NQueenVfx.cs")]
	public class NQueenVfx : Node2D
	{
		// Token: 0x0600186C RID: 6252 RVA: 0x00098908 File Offset: 0x00096B08
		public override void _Ready()
		{
			this._animController = new MegaSprite(this._spineNode);
			this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
			this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
			this._sprayParticles.Emitting = false;
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x00098974 File Offset: 0x00096B74
		[NullableContext(1)]
		private void OnAnimationEvent(GodotObject sprite, GodotObject animationState, GodotObject trackEntry, GodotObject eventObject)
		{
			string eventName = new MegaEvent(eventObject).GetData().GetEventName();
			if (eventName == "attack_start")
			{
				this.StartAttack();
				return;
			}
			if (!(eventName == "attack_end"))
			{
				return;
			}
			this.EndAttack();
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x000989C0 File Offset: 0x00096BC0
		[NullableContext(1)]
		private void OnAnimationStart(GodotObject spineSprite, GodotObject animationState, GodotObject trackEntry)
		{
			if (new MegaAnimationState(animationState).GetCurrentAnimationName(0) != "attack")
			{
				this.EndAttack();
			}
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x000989E5 File Offset: 0x00096BE5
		private void StartAttack()
		{
			this._sprayParticles.Restart();
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x000989F2 File Offset: 0x00096BF2
		private void EndAttack()
		{
			this._sprayParticles.Emitting = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001871 RID: 6257 RVA: 0x00098A00 File Offset: 0x00096C00
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NQueenVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NQueenVfx.MethodName.OnAnimationEvent, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "sprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "eventObject", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NQueenVfx.MethodName.OnAnimationStart, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "spineSprite", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "animationState", 0L, "", 6L, new StringName("Object"), false),
					new PropertyInfo(24L, "trackEntry", 0L, "", 6L, new StringName("Object"), false)
				}, null),
				new MethodInfo(NQueenVfx.MethodName.StartAttack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NQueenVfx.MethodName.EndAttack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001872 RID: 6258 RVA: 0x00098C3C File Offset: 0x00096E3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NQueenVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NQueenVfx.MethodName.OnAnimationEvent && args.Count == 4)
			{
				this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]), VariantUtils.ConvertTo<GodotObject>(args[3]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NQueenVfx.MethodName.OnAnimationStart && args.Count == 3)
			{
				this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(args[0]), VariantUtils.ConvertTo<GodotObject>(args[1]), VariantUtils.ConvertTo<GodotObject>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NQueenVfx.MethodName.StartAttack && args.Count == 0)
			{
				this.StartAttack();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NQueenVfx.MethodName.EndAttack && args.Count == 0)
			{
				this.EndAttack();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001873 RID: 6259 RVA: 0x00098D68 File Offset: 0x00096F68
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NQueenVfx.MethodName._Ready || (ref method) == NQueenVfx.MethodName.OnAnimationEvent || (ref method) == NQueenVfx.MethodName.OnAnimationStart || (ref method) == NQueenVfx.MethodName.StartAttack || (ref method) == NQueenVfx.MethodName.EndAttack || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001874 RID: 6260 RVA: 0x00098DC7 File Offset: 0x00096FC7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NQueenVfx.PropertyName._spineNode)
			{
				this._spineNode = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NQueenVfx.PropertyName._sprayParticles)
			{
				this._sprayParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001875 RID: 6261 RVA: 0x00098E08 File Offset: 0x00097008
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NQueenVfx.PropertyName._spineNode)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._spineNode);
				return true;
			}
			if ((ref name) == NQueenVfx.PropertyName._sprayParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sprayParticles);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001876 RID: 6262 RVA: 0x00098E60 File Offset: 0x00097060
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NQueenVfx.PropertyName._spineNode, 34L, "Node2D", 4102L, true),
				new PropertyInfo(24L, NQueenVfx.PropertyName._sprayParticles, 34L, "GPUParticles2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001877 RID: 6263 RVA: 0x00098EB8 File Offset: 0x000970B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NQueenVfx.PropertyName._spineNode, Variant.From<Node2D>(ref this._spineNode));
			info.AddProperty(NQueenVfx.PropertyName._sprayParticles, Variant.From<GpuParticles2D>(ref this._sprayParticles));
		}

		/// <inheritdoc />
		// Token: 0x06001878 RID: 6264 RVA: 0x00098EF0 File Offset: 0x000970F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NQueenVfx.PropertyName._spineNode, ref variant))
			{
				this._spineNode = variant.As<Node2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NQueenVfx.PropertyName._sprayParticles, ref variant2))
			{
				this._sprayParticles = variant2.As<GpuParticles2D>();
			}
		}

		// Token: 0x040008CF RID: 2255
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D _spineNode;

		// Token: 0x040008D0 RID: 2256
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private GpuParticles2D _sprayParticles;

		// Token: 0x040008D1 RID: 2257
		[Nullable(1)]
		private MegaSprite _animController;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010FA RID: 4346
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004261 RID: 16993
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAnimationEvent' method.
			/// </summary>
			// Token: 0x04004262 RID: 16994
			public static readonly StringName OnAnimationEvent = "OnAnimationEvent";

			/// <summary>
			/// Cached name for the 'OnAnimationStart' method.
			/// </summary>
			// Token: 0x04004263 RID: 16995
			public static readonly StringName OnAnimationStart = "OnAnimationStart";

			/// <summary>
			/// Cached name for the 'StartAttack' method.
			/// </summary>
			// Token: 0x04004264 RID: 16996
			public static readonly StringName StartAttack = "StartAttack";

			/// <summary>
			/// Cached name for the 'EndAttack' method.
			/// </summary>
			// Token: 0x04004265 RID: 16997
			public static readonly StringName EndAttack = "EndAttack";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010FB RID: 4347
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_spineNode' field.
			/// </summary>
			// Token: 0x04004266 RID: 16998
			public static readonly StringName _spineNode = "_spineNode";

			/// <summary>
			/// Cached name for the '_sprayParticles' field.
			/// </summary>
			// Token: 0x04004267 RID: 16999
			public static readonly StringName _sprayParticles = "_sprayParticles";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020010FC RID: 4348
		public class SignalName : Node2D.SignalName
		{
		}
	}
}

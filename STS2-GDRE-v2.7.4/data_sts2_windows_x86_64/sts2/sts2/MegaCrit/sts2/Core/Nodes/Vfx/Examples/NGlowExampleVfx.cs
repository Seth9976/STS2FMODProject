using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Examples
{
	// Token: 0x0200022C RID: 556
	[ScriptPath("res://src/Core/Nodes/Vfx/Examples/NGlowExampleVfx.cs")]
	public class NGlowExampleVfx : Node
	{
		// Token: 0x06001C3C RID: 7228 RVA: 0x000B7554 File Offset: 0x000B5754
		public override void _Ready()
		{
			this._env = base.GetNode<WorldEnvironment>("%WorldEnvironment");
			this._tween = base.CreateTween();
			this._tween.SetLoops(0);
			this._tween.TweenProperty(this._env, "environment:tonemap_exposure", 5f, 1.0).SetEase(2L).SetTrans(7L);
			this._tween.TweenProperty(this._env, "environment:tonemap_exposure", 1f, 1.0).SetEase(2L).SetTrans(7L);
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x000B760C File Offset: 0x000B580C
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			InputEventKey inputEventKey = inputEvent as InputEventKey;
			if (inputEventKey != null && inputEventKey.IsReleased() && inputEventKey.Keycode == 50L)
			{
				if (this._tween.IsRunning())
				{
					Log.Info("Pausing Glow", 2);
					this._tween.Pause();
					return;
				}
				Log.Info("Resuming Rain", 2);
				this._tween.Play();
			}
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x000B7670 File Offset: 0x000B5870
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C3F RID: 7231 RVA: 0x000B7684 File Offset: 0x000B5884
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NGlowExampleVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGlowExampleVfx.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NGlowExampleVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C40 RID: 7232 RVA: 0x000B7758 File Offset: 0x000B5958
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGlowExampleVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGlowExampleVfx.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGlowExampleVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001C41 RID: 7233 RVA: 0x000B77EB File Offset: 0x000B59EB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGlowExampleVfx.MethodName._Ready || (ref method) == NGlowExampleVfx.MethodName._Input || (ref method) == NGlowExampleVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001C42 RID: 7234 RVA: 0x000B7821 File Offset: 0x000B5A21
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGlowExampleVfx.PropertyName._env)
			{
				this._env = VariantUtils.ConvertTo<WorldEnvironment>(ref value);
				return true;
			}
			if ((ref name) == NGlowExampleVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001C43 RID: 7235 RVA: 0x000B7864 File Offset: 0x000B5A64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGlowExampleVfx.PropertyName._env)
			{
				value = VariantUtils.CreateFrom<WorldEnvironment>(ref this._env);
				return true;
			}
			if ((ref name) == NGlowExampleVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001C44 RID: 7236 RVA: 0x000B78BC File Offset: 0x000B5ABC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGlowExampleVfx.PropertyName._env, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGlowExampleVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C45 RID: 7237 RVA: 0x000B7912 File Offset: 0x000B5B12
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGlowExampleVfx.PropertyName._env, Variant.From<WorldEnvironment>(ref this._env));
			info.AddProperty(NGlowExampleVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001C46 RID: 7238 RVA: 0x000B7948 File Offset: 0x000B5B48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGlowExampleVfx.PropertyName._env, ref variant))
			{
				this._env = variant.As<WorldEnvironment>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGlowExampleVfx.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
		}

		// Token: 0x04000AD0 RID: 2768
		[Nullable(1)]
		private WorldEnvironment _env;

		// Token: 0x04000AD1 RID: 2769
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011DF RID: 4575
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004572 RID: 17778
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04004573 RID: 17779
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004574 RID: 17780
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011E0 RID: 4576
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_env' field.
			/// </summary>
			// Token: 0x04004575 RID: 17781
			public static readonly StringName _env = "_env";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004576 RID: 17782
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011E1 RID: 4577
		public class SignalName : Node.SignalName
		{
		}
	}
}

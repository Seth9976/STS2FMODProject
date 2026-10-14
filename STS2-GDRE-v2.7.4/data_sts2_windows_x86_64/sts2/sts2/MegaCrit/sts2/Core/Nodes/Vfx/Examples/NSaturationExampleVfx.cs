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
	// Token: 0x0200022E RID: 558
	[ScriptPath("res://src/Core/Nodes/Vfx/Examples/NSaturationExampleVfx.cs")]
	public class NSaturationExampleVfx : Node
	{
		// Token: 0x06001C53 RID: 7251 RVA: 0x000B7C88 File Offset: 0x000B5E88
		public override void _Ready()
		{
			this._env = base.GetNode<WorldEnvironment>("%WorldEnvironment");
			this._tween = base.CreateTween();
			this._tween.SetLoops(0);
			this._tween.TweenProperty(this._env, "environment:adjustment_saturation", 0f, 1.0).SetEase(2L).SetTrans(7L);
			this._tween.TweenProperty(this._env, "environment:adjustment_saturation", 1f, 1.0).SetEase(2L).SetTrans(7L);
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x000B7D40 File Offset: 0x000B5F40
		[NullableContext(1)]
		public override void _Input(InputEvent inputEvent)
		{
			InputEventKey inputEventKey = inputEvent as InputEventKey;
			if (inputEventKey != null && inputEventKey.IsReleased() && inputEventKey.Keycode == 51L)
			{
				if (this._tween.IsRunning())
				{
					Log.Info("Pausing Saturation", 2);
					this._tween.Pause();
					return;
				}
				Log.Info("Resuming Saturation", 2);
				this._tween.Play();
			}
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x000B7DA4 File Offset: 0x000B5FA4
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
		// Token: 0x06001C56 RID: 7254 RVA: 0x000B7DB8 File Offset: 0x000B5FB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NSaturationExampleVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSaturationExampleVfx.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NSaturationExampleVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C57 RID: 7255 RVA: 0x000B7E8C File Offset: 0x000B608C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSaturationExampleVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSaturationExampleVfx.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSaturationExampleVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001C58 RID: 7256 RVA: 0x000B7F1F File Offset: 0x000B611F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSaturationExampleVfx.MethodName._Ready || (ref method) == NSaturationExampleVfx.MethodName._Input || (ref method) == NSaturationExampleVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001C59 RID: 7257 RVA: 0x000B7F55 File Offset: 0x000B6155
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSaturationExampleVfx.PropertyName._env)
			{
				this._env = VariantUtils.ConvertTo<WorldEnvironment>(ref value);
				return true;
			}
			if ((ref name) == NSaturationExampleVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001C5A RID: 7258 RVA: 0x000B7F98 File Offset: 0x000B6198
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSaturationExampleVfx.PropertyName._env)
			{
				value = VariantUtils.CreateFrom<WorldEnvironment>(ref this._env);
				return true;
			}
			if ((ref name) == NSaturationExampleVfx.PropertyName._tween)
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
		// Token: 0x06001C5B RID: 7259 RVA: 0x000B7FF0 File Offset: 0x000B61F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSaturationExampleVfx.PropertyName._env, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSaturationExampleVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001C5C RID: 7260 RVA: 0x000B8046 File Offset: 0x000B6246
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSaturationExampleVfx.PropertyName._env, Variant.From<WorldEnvironment>(ref this._env));
			info.AddProperty(NSaturationExampleVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001C5D RID: 7261 RVA: 0x000B807C File Offset: 0x000B627C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSaturationExampleVfx.PropertyName._env, ref variant))
			{
				this._env = variant.As<WorldEnvironment>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSaturationExampleVfx.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
		}

		// Token: 0x04000AD4 RID: 2772
		[Nullable(1)]
		private WorldEnvironment _env;

		// Token: 0x04000AD5 RID: 2773
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011E5 RID: 4581
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400457B RID: 17787
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x0400457C RID: 17788
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400457D RID: 17789
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011E6 RID: 4582
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_env' field.
			/// </summary>
			// Token: 0x0400457E RID: 17790
			public static readonly StringName _env = "_env";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400457F RID: 17791
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020011E7 RID: 4583
		public class SignalName : Node.SignalName
		{
		}
	}
}

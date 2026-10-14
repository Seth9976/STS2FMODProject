using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001F0 RID: 496
	[ScriptPath("res://src/Core/Nodes/Vfx/NSleepingVfx.cs")]
	public class NSleepingVfx : Node2D
	{
		// Token: 0x06001949 RID: 6473 RVA: 0x0009EE7C File Offset: 0x0009D07C
		[NullableContext(2)]
		public static NSleepingVfx Create(Vector2 targetTalkPosition, bool goingRight = true)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSleepingVfx nsleepingVfx = PreloadManager.Cache.GetScene(NSleepingVfx.scenePath).Instantiate<NSleepingVfx>(0L);
			nsleepingVfx.GlobalPosition = targetTalkPosition;
			nsleepingVfx.SetFloatingDirection(goingRight);
			Texture2D texture2D;
			if (nsleepingVfx != null && nsleepingVfx._zParticles != null && nsleepingVfx._localizedZTexture != null && nsleepingVfx._localizedZTexture.TryGetTexture(out texture2D))
			{
				nsleepingVfx._zParticles.Texture = texture2D;
			}
			return nsleepingVfx;
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x0009EEE6 File Offset: 0x0009D0E6
		public override void _Ready()
		{
			this.Play();
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x0009EEEE File Offset: 0x0009D0EE
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x0009EF00 File Offset: 0x0009D100
		private void Play()
		{
			foreach (GpuParticles2D gpuParticles2D in this._burstParticles)
			{
				gpuParticles2D.Restart();
			}
			foreach (GpuParticles2D gpuParticles2D2 in this._continuousParticles)
			{
				gpuParticles2D2.Restart();
				gpuParticles2D2.Emitting = true;
			}
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x0009EF90 File Offset: 0x0009D190
		public void SetFloatingDirection(bool goingRight)
		{
			this._zParticles.ProcessMaterial = (ParticleProcessMaterial)this._zParticles.ProcessMaterial.Duplicate(false);
			this._zParticles.ProcessMaterial.Set(NSleepingVfx._direction, new Vector3(goingRight ? 0.5f : (-0.5f), -1f, 0f));
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x0009EFF6 File Offset: 0x0009D1F6
		public void Stop()
		{
			TaskHelper.RunSafely(this.Stopping());
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0009F004 File Offset: 0x0009D204
		[NullableContext(1)]
		private async Task Stopping()
		{
			this._cts = new CancellationTokenSource();
			foreach (GpuParticles2D gpuParticles2D in this._continuousParticles)
			{
				gpuParticles2D.Emitting = false;
			}
			await Cmd.Wait(5f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001950 RID: 6480 RVA: 0x0009F048 File Offset: 0x0009D248
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NSleepingVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetTalkPosition", 0L, "", 6L, false),
					new PropertyInfo(1L, "goingRight", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSleepingVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSleepingVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSleepingVfx.MethodName.Play, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSleepingVfx.MethodName.SetFloatingDirection, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "goingRight", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSleepingVfx.MethodName.Stop, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001951 RID: 6481 RVA: 0x0009F1F0 File Offset: 0x0009D3F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSleepingVfx.MethodName.Create && args.Count == 2)
			{
				NSleepingVfx nsleepingVfx = NSleepingVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = VariantUtils.CreateFrom<NSleepingVfx>(ref nsleepingVfx);
				return true;
			}
			if ((ref method) == NSleepingVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSleepingVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSleepingVfx.MethodName.Play && args.Count == 0)
			{
				this.Play();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSleepingVfx.MethodName.SetFloatingDirection && args.Count == 1)
			{
				this.SetFloatingDirection(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSleepingVfx.MethodName.Stop && args.Count == 0)
			{
				this.Stop();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0009F314 File Offset: 0x0009D514
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSleepingVfx.MethodName.Create && args.Count == 2)
			{
				NSleepingVfx nsleepingVfx = NSleepingVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = VariantUtils.CreateFrom<NSleepingVfx>(ref nsleepingVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001953 RID: 6483 RVA: 0x0009F370 File Offset: 0x0009D570
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSleepingVfx.MethodName.Create || (ref method) == NSleepingVfx.MethodName._Ready || (ref method) == NSleepingVfx.MethodName._ExitTree || (ref method) == NSleepingVfx.MethodName.Play || (ref method) == NSleepingVfx.MethodName.SetFloatingDirection || (ref method) == NSleepingVfx.MethodName.Stop || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001954 RID: 6484 RVA: 0x0009F3E0 File Offset: 0x0009D5E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSleepingVfx.PropertyName._burstParticles)
			{
				this._burstParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSleepingVfx.PropertyName._continuousParticles)
			{
				this._continuousParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSleepingVfx.PropertyName._zParticles)
			{
				this._zParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSleepingVfx.PropertyName._localizedZTexture)
			{
				this._localizedZTexture = VariantUtils.ConvertTo<LocalizedTexture>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001955 RID: 6485 RVA: 0x0009F464 File Offset: 0x0009D664
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSleepingVfx.PropertyName._burstParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._burstParticles);
				return true;
			}
			if ((ref name) == NSleepingVfx.PropertyName._continuousParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._continuousParticles);
				return true;
			}
			if ((ref name) == NSleepingVfx.PropertyName._zParticles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._zParticles);
				return true;
			}
			if ((ref name) == NSleepingVfx.PropertyName._localizedZTexture)
			{
				value = VariantUtils.CreateFrom<LocalizedTexture>(ref this._localizedZTexture);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001956 RID: 6486 RVA: 0x0009F4FC File Offset: 0x0009D6FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NSleepingVfx.PropertyName._burstParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NSleepingVfx.PropertyName._continuousParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NSleepingVfx.PropertyName._zParticles, 34L, "GPUParticles2D", 4102L, true),
				new PropertyInfo(24L, NSleepingVfx.PropertyName._localizedZTexture, 17L, "LocalizedTexture", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001957 RID: 6487 RVA: 0x0009F598 File Offset: 0x0009D798
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSleepingVfx.PropertyName._burstParticles, Variant.CreateFrom<GpuParticles2D>(this._burstParticles));
			info.AddProperty(NSleepingVfx.PropertyName._continuousParticles, Variant.CreateFrom<GpuParticles2D>(this._continuousParticles));
			info.AddProperty(NSleepingVfx.PropertyName._zParticles, Variant.From<GpuParticles2D>(ref this._zParticles));
			info.AddProperty(NSleepingVfx.PropertyName._localizedZTexture, Variant.From<LocalizedTexture>(ref this._localizedZTexture));
		}

		/// <inheritdoc />
		// Token: 0x06001958 RID: 6488 RVA: 0x0009F604 File Offset: 0x0009D804
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSleepingVfx.PropertyName._burstParticles, ref variant))
			{
				this._burstParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSleepingVfx.PropertyName._continuousParticles, ref variant2))
			{
				this._continuousParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSleepingVfx.PropertyName._zParticles, ref variant3))
			{
				this._zParticles = variant3.As<GpuParticles2D>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSleepingVfx.PropertyName._localizedZTexture, ref variant4))
			{
				this._localizedZTexture = variant4.As<LocalizedTexture>();
			}
		}

		// Token: 0x04000934 RID: 2356
		[Nullable(1)]
		private static readonly StringName _direction = new StringName("direction");

		// Token: 0x04000935 RID: 2357
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_sleeping");

		// Token: 0x04000936 RID: 2358
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _burstParticles = new Array<GpuParticles2D>();

		// Token: 0x04000937 RID: 2359
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _continuousParticles = new Array<GpuParticles2D>();

		// Token: 0x04000938 RID: 2360
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private GpuParticles2D _zParticles;

		// Token: 0x04000939 RID: 2361
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private LocalizedTexture _localizedZTexture;

		// Token: 0x0400093A RID: 2362
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200112E RID: 4398
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004305 RID: 17157
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004306 RID: 17158
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004307 RID: 17159
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Play' method.
			/// </summary>
			// Token: 0x04004308 RID: 17160
			public static readonly StringName Play = "Play";

			/// <summary>
			/// Cached name for the 'SetFloatingDirection' method.
			/// </summary>
			// Token: 0x04004309 RID: 17161
			public static readonly StringName SetFloatingDirection = "SetFloatingDirection";

			/// <summary>
			/// Cached name for the 'Stop' method.
			/// </summary>
			// Token: 0x0400430A RID: 17162
			public static readonly StringName Stop = "Stop";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200112F RID: 4399
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_burstParticles' field.
			/// </summary>
			// Token: 0x0400430B RID: 17163
			public static readonly StringName _burstParticles = "_burstParticles";

			/// <summary>
			/// Cached name for the '_continuousParticles' field.
			/// </summary>
			// Token: 0x0400430C RID: 17164
			public static readonly StringName _continuousParticles = "_continuousParticles";

			/// <summary>
			/// Cached name for the '_zParticles' field.
			/// </summary>
			// Token: 0x0400430D RID: 17165
			public static readonly StringName _zParticles = "_zParticles";

			/// <summary>
			/// Cached name for the '_localizedZTexture' field.
			/// </summary>
			// Token: 0x0400430E RID: 17166
			public static readonly StringName _localizedZTexture = "_localizedZTexture";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001130 RID: 4400
		public class SignalName : Node2D.SignalName
		{
		}
	}
}

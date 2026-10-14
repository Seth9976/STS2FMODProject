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
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001FB RID: 507
	[ScriptPath("res://src/Core/Nodes/Vfx/NSplashVfx.cs")]
	public class NSplashVfx : Node2D
	{
		// Token: 0x06001A11 RID: 6673 RVA: 0x000A7534 File Offset: 0x000A5734
		[NullableContext(2)]
		public static NSplashVfx Create(Vector2 targetPosition, Color tint)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSplashVfx nsplashVfx = PreloadManager.Cache.GetScene(NSplashVfx.scenePath).Instantiate<NSplashVfx>(0L);
			nsplashVfx.GlobalPosition = targetPosition;
			nsplashVfx._tint = tint;
			return nsplashVfx;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x000A7570 File Offset: 0x000A5770
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlayVfx());
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x000A757E File Offset: 0x000A577E
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x000A7590 File Offset: 0x000A5790
		[NullableContext(1)]
		private async Task PlayVfx()
		{
			this._cts = new CancellationTokenSource();
			foreach (GpuParticles2D gpuParticles2D in this._particles)
			{
				gpuParticles2D.SelfModulate = this._tint;
				gpuParticles2D.Restart();
			}
			await Cmd.Wait(2f, this._cts.Token, false);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A15 RID: 6677 RVA: 0x000A75D4 File Offset: 0x000A57D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NSplashVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "targetPosition", 0L, "", 6L, false),
					new PropertyInfo(20L, "tint", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSplashVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSplashVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A16 RID: 6678 RVA: 0x000A76CC File Offset: 0x000A58CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSplashVfx.MethodName.Create && args.Count == 2)
			{
				NSplashVfx nsplashVfx = NSplashVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NSplashVfx>(ref nsplashVfx);
				return true;
			}
			if ((ref method) == NSplashVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSplashVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x000A7774 File Offset: 0x000A5974
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSplashVfx.MethodName.Create && args.Count == 2)
			{
				NSplashVfx nsplashVfx = NSplashVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<Color>(args[1]));
				ret = VariantUtils.CreateFrom<NSplashVfx>(ref nsplashVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001A18 RID: 6680 RVA: 0x000A77CF File Offset: 0x000A59CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSplashVfx.MethodName.Create || (ref method) == NSplashVfx.MethodName._Ready || (ref method) == NSplashVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001A19 RID: 6681 RVA: 0x000A7805 File Offset: 0x000A5A05
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSplashVfx.PropertyName._particles)
			{
				this._particles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NSplashVfx.PropertyName._tint)
			{
				this._tint = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001A1A RID: 6682 RVA: 0x000A7848 File Offset: 0x000A5A48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSplashVfx.PropertyName._particles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._particles);
				return true;
			}
			if ((ref name) == NSplashVfx.PropertyName._tint)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._tint);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A1B RID: 6683 RVA: 0x000A78A0 File Offset: 0x000A5AA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NSplashVfx.PropertyName._particles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(20L, NSplashVfx.PropertyName._tint, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A1C RID: 6684 RVA: 0x000A78F7 File Offset: 0x000A5AF7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSplashVfx.PropertyName._particles, Variant.CreateFrom<GpuParticles2D>(this._particles));
			info.AddProperty(NSplashVfx.PropertyName._tint, Variant.From<Color>(ref this._tint));
		}

		/// <inheritdoc />
		// Token: 0x06001A1D RID: 6685 RVA: 0x000A792C File Offset: 0x000A5B2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSplashVfx.PropertyName._particles, ref variant))
			{
				this._particles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSplashVfx.PropertyName._tint, ref variant2))
			{
				this._tint = variant2.As<Color>();
			}
		}

		// Token: 0x040009AF RID: 2479
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_splash");

		// Token: 0x040009B0 RID: 2480
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _particles = new Array<GpuParticles2D>();

		// Token: 0x040009B1 RID: 2481
		private Color _tint = Colors.White;

		// Token: 0x040009B2 RID: 2482
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200115A RID: 4442
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040043CA RID: 17354
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040043CB RID: 17355
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040043CC RID: 17356
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200115B RID: 4443
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_particles' field.
			/// </summary>
			// Token: 0x040043CD RID: 17357
			public static readonly StringName _particles = "_particles";

			/// <summary>
			/// Cached name for the '_tint' field.
			/// </summary>
			// Token: 0x040043CE RID: 17358
			public static readonly StringName _tint = "_tint";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200115C RID: 4444
		public class SignalName : Node2D.SignalName
		{
		}
	}
}

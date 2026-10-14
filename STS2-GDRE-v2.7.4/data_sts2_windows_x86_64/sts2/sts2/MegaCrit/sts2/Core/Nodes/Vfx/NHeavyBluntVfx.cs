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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x020001BA RID: 442
	[ScriptPath("res://src/Core/Nodes/Vfx/NHeavyBluntVfx.cs")]
	public class NHeavyBluntVfx : Node2D
	{
		// Token: 0x06001602 RID: 5634 RVA: 0x00086970 File Offset: 0x00084B70
		[NullableContext(2)]
		public static NHeavyBluntVfx Create(Vector2 debugPosition)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NHeavyBluntVfx nheavyBluntVfx = PreloadManager.Cache.GetScene(NHeavyBluntVfx.scenePath).Instantiate<NHeavyBluntVfx>(0L);
			nheavyBluntVfx._debugPosition = debugPosition;
			return nheavyBluntVfx;
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x000869A5 File Offset: 0x00084BA5
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.PlaySequence());
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x000869B4 File Offset: 0x00084BB4
		[NullableContext(1)]
		private async Task PlaySequence()
		{
			base.GlobalPosition = this._debugPosition;
			for (int i = 0; i < this._anticipationParticles.Count; i++)
			{
				this._anticipationParticles[i].Restart();
			}
			await this.WaitForSeconds(0.2f);
			NGame instance = NGame.Instance;
			if (instance != null)
			{
				instance.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short, -1f);
			}
			for (int j = 0; j < this._impactParticles.Count; j++)
			{
				this._impactParticles[j].Restart();
			}
			await this.WaitForSeconds(2f);
			this.QueueFreeSafely();
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x000869F8 File Offset: 0x00084BF8
		[NullableContext(1)]
		private async Task WaitForSeconds(float duration)
		{
			double timer = 0.0;
			while (timer < (double)duration)
			{
				timer += base.GetProcessDeltaTime();
				await this.AwaitProcessFrame(default(CancellationToken));
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001606 RID: 5638 RVA: 0x00086A44 File Offset: 0x00084C44
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NHeavyBluntVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Node2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "debugPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHeavyBluntVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001607 RID: 5639 RVA: 0x00086AE8 File Offset: 0x00084CE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHeavyBluntVfx.MethodName.Create && args.Count == 1)
			{
				NHeavyBluntVfx nheavyBluntVfx = NHeavyBluntVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NHeavyBluntVfx>(ref nheavyBluntVfx);
				return true;
			}
			if ((ref method) == NHeavyBluntVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x00086B5C File Offset: 0x00084D5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHeavyBluntVfx.MethodName.Create && args.Count == 1)
			{
				NHeavyBluntVfx nheavyBluntVfx = NHeavyBluntVfx.Create(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = VariantUtils.CreateFrom<NHeavyBluntVfx>(ref nheavyBluntVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001609 RID: 5641 RVA: 0x00086BAA File Offset: 0x00084DAA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHeavyBluntVfx.MethodName.Create || (ref method) == NHeavyBluntVfx.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600160A RID: 5642 RVA: 0x00086BD4 File Offset: 0x00084DD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHeavyBluntVfx.PropertyName._anticipationParticles)
			{
				this._anticipationParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NHeavyBluntVfx.PropertyName._impactParticles)
			{
				this._impactParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NHeavyBluntVfx.PropertyName._debugPosition)
			{
				this._debugPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600160B RID: 5643 RVA: 0x00086C3C File Offset: 0x00084E3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHeavyBluntVfx.PropertyName._anticipationParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._anticipationParticles);
				return true;
			}
			if ((ref name) == NHeavyBluntVfx.PropertyName._impactParticles)
			{
				value = VariantUtils.CreateFromArray<GpuParticles2D>(this._impactParticles);
				return true;
			}
			if ((ref name) == NHeavyBluntVfx.PropertyName._debugPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._debugPosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600160C RID: 5644 RVA: 0x00086CB4 File Offset: 0x00084EB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NHeavyBluntVfx.PropertyName._anticipationParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(28L, NHeavyBluntVfx.PropertyName._impactParticles, 23L, "24/34:GPUParticles2D", 4102L, true),
				new PropertyInfo(5L, NHeavyBluntVfx.PropertyName._debugPosition, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600160D RID: 5645 RVA: 0x00086D2C File Offset: 0x00084F2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHeavyBluntVfx.PropertyName._anticipationParticles, Variant.CreateFrom<GpuParticles2D>(this._anticipationParticles));
			info.AddProperty(NHeavyBluntVfx.PropertyName._impactParticles, Variant.CreateFrom<GpuParticles2D>(this._impactParticles));
			info.AddProperty(NHeavyBluntVfx.PropertyName._debugPosition, Variant.From<Vector2>(ref this._debugPosition));
		}

		/// <inheritdoc />
		// Token: 0x0600160E RID: 5646 RVA: 0x00086D84 File Offset: 0x00084F84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHeavyBluntVfx.PropertyName._anticipationParticles, ref variant))
			{
				this._anticipationParticles = variant.AsGodotArray<GpuParticles2D>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHeavyBluntVfx.PropertyName._impactParticles, ref variant2))
			{
				this._impactParticles = variant2.AsGodotArray<GpuParticles2D>();
			}
			Variant variant3;
			if (info.TryGetProperty(NHeavyBluntVfx.PropertyName._debugPosition, ref variant3))
			{
				this._debugPosition = variant3.As<Vector2>();
			}
		}

		// Token: 0x040007D5 RID: 2005
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_heavy_blunt");

		// Token: 0x040007D6 RID: 2006
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _anticipationParticles = new Array<GpuParticles2D>();

		// Token: 0x040007D7 RID: 2007
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Array<GpuParticles2D> _impactParticles = new Array<GpuParticles2D>();

		// Token: 0x040007D8 RID: 2008
		private Vector2 _debugPosition;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001065 RID: 4197
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004096 RID: 16534
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004097 RID: 16535
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001066 RID: 4198
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_anticipationParticles' field.
			/// </summary>
			// Token: 0x04004098 RID: 16536
			public static readonly StringName _anticipationParticles = "_anticipationParticles";

			/// <summary>
			/// Cached name for the '_impactParticles' field.
			/// </summary>
			// Token: 0x04004099 RID: 16537
			public static readonly StringName _impactParticles = "_impactParticles";

			/// <summary>
			/// Cached name for the '_debugPosition' field.
			/// </summary>
			// Token: 0x0400409A RID: 16538
			public static readonly StringName _debugPosition = "_debugPosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001067 RID: 4199
		public class SignalName : Node2D.SignalName
		{
		}
	}
}

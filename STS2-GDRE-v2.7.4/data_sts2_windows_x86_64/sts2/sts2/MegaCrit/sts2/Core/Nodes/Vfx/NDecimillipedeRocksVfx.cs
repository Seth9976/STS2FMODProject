using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x0200019B RID: 411
	[ScriptPath("res://src/Core/Nodes/Vfx/NDecimillipedeRocksVfx.cs")]
	public class NDecimillipedeRocksVfx : Node2D
	{
		// Token: 0x06001439 RID: 5177 RVA: 0x0007A221 File Offset: 0x00078421
		public override void _Ready()
		{
			TaskHelper.RunSafely(this.Play(this._cancelToken.Token));
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0007A23A File Offset: 0x0007843A
		public override void _ExitTree()
		{
			base._ExitTree();
			this._cancelToken.Cancel();
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0007A250 File Offset: 0x00078450
		[NullableContext(1)]
		private async Task Play(CancellationToken cancellationToken)
		{
			foreach (Node2D rock in this._rocks)
			{
				await Task.Delay(Rng.Chaotic.NextInt(100, 201), cancellationToken);
				MegaAnimationState animationState = new MegaSprite(rock).GetAnimationState();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
				defaultInterpolatedStringHandler.AppendLiteral("fall");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Rng.Chaotic.NextInt(1, 5));
				animationState.SetAnimation(defaultInterpolatedStringHandler.ToStringAndClear(), false, 0);
				rock = null;
			}
			Node2D[] array = null;
			await Task.Delay(5000, cancellationToken);
			if (!cancellationToken.IsCancellationRequested)
			{
				this.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600143C RID: 5180 RVA: 0x0007A29C File Offset: 0x0007849C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NDecimillipedeRocksVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDecimillipedeRocksVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600143D RID: 5181 RVA: 0x0007A310 File Offset: 0x00078510
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDecimillipedeRocksVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDecimillipedeRocksVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600143E RID: 5182 RVA: 0x0007A370 File Offset: 0x00078570
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDecimillipedeRocksVfx.MethodName._Ready || (ref method) == NDecimillipedeRocksVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600143F RID: 5183 RVA: 0x0007A397 File Offset: 0x00078597
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDecimillipedeRocksVfx.PropertyName._rocks)
			{
				this._rocks = VariantUtils.ConvertToSystemArrayOfGodotObject<Node2D>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001440 RID: 5184 RVA: 0x0007A3BC File Offset: 0x000785BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDecimillipedeRocksVfx.PropertyName._rocks)
			{
				GodotObject[] rocks = this._rocks;
				value = VariantUtils.CreateFromSystemArrayOfGodotObject(rocks);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001441 RID: 5185 RVA: 0x0007A3F4 File Offset: 0x000785F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(28L, NDecimillipedeRocksVfx.PropertyName._rocks, 23L, "24/34:Node2D", 4102L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001442 RID: 5186 RVA: 0x0007A42C File Offset: 0x0007862C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName rocks = NDecimillipedeRocksVfx.PropertyName._rocks;
			GodotObject[] rocks2 = this._rocks;
			info.AddProperty(rocks, Variant.CreateFrom(rocks2));
		}

		/// <inheritdoc />
		// Token: 0x06001443 RID: 5187 RVA: 0x0007A458 File Offset: 0x00078658
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDecimillipedeRocksVfx.PropertyName._rocks, ref variant))
			{
				this._rocks = variant.AsGodotObjectArray<Node2D>();
			}
		}

		// Token: 0x04000737 RID: 1847
		[Nullable(1)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Node2D[] _rocks;

		// Token: 0x04000738 RID: 1848
		[Nullable(1)]
		private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FF6 RID: 4086
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003F65 RID: 16229
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003F66 RID: 16230
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FF7 RID: 4087
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the '_rocks' field.
			/// </summary>
			// Token: 0x04003F67 RID: 16231
			public static readonly StringName _rocks = "_rocks";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FF8 RID: 4088
		public class SignalName : Node2D.SignalName
		{
		}
	}
}

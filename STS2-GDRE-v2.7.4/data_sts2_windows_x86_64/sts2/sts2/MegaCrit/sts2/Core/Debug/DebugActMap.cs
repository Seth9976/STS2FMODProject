using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Debug
{
	// Token: 0x02000D08 RID: 3336
	[ScriptPath("res://src/Core/Debug/DebugActMap.cs")]
	public class DebugActMap : Node
	{
		// Token: 0x060086AC RID: 34476 RVA: 0x002D3238 File Offset: 0x002D1438
		public override void _Ready()
		{
			this.GenerateMap();
		}

		// Token: 0x060086AD RID: 34477 RVA: 0x002D3240 File Offset: 0x002D1440
		public override void _Process(double delta)
		{
			if (Input.IsKeyPressed(91L))
			{
				this.GenerateMap();
			}
		}

		// Token: 0x060086AE RID: 34478 RVA: 0x002D3254 File Offset: 0x002D1454
		private void GenerateMap()
		{
			ulong num = Rng.Chaotic.NextUnsignedLong();
			this._testActMap = new StandardActMap(new Rng(num), ModelDb.Act<Overgrowth>(), false, false, false, null, true);
			base.GetNode<NMapScreen>("%MapScreen").SetMap(this._testActMap, num, true);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060086AF RID: 34479 RVA: 0x002D32A4 File Offset: 0x002D14A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(DebugActMap.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(DebugActMap.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(DebugActMap.MethodName.GenerateMap, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060086B0 RID: 34480 RVA: 0x002D336C File Offset: 0x002D156C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == DebugActMap.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == DebugActMap.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == DebugActMap.MethodName.GenerateMap && args.Count == 0)
			{
				this.GenerateMap();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060086B1 RID: 34481 RVA: 0x002D33FF File Offset: 0x002D15FF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == DebugActMap.MethodName._Ready || (ref method) == DebugActMap.MethodName._Process || (ref method) == DebugActMap.MethodName.GenerateMap || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060086B2 RID: 34482 RVA: 0x002D3435 File Offset: 0x002D1635
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060086B3 RID: 34483 RVA: 0x002D343E File Offset: 0x002D163E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04002E99 RID: 11929
		[Nullable(1)]
		private ActMap _testActMap;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002409 RID: 9225
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400984D RID: 38989
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x0400984E RID: 38990
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'GenerateMap' method.
			/// </summary>
			// Token: 0x0400984F RID: 38991
			public static readonly StringName GenerateMap = "GenerateMap";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200240A RID: 9226
		public class PropertyName : Node.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200240B RID: 9227
		public class SignalName : Node.SignalName
		{
		}
	}
}

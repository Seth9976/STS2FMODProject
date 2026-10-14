using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace MegaCrit.Sts2.Core.Debug
{
	/// <summary>
	/// This is used strictly for debugging CardTrailVfx and related
	/// </summary>
	// Token: 0x02000D10 RID: 3344
	[ScriptPath("res://src/Core/Debug/NTrailTest.cs")]
	public class NTrailTest : Control
	{
		// Token: 0x060086F5 RID: 34549 RVA: 0x002D52D8 File Offset: 0x002D34D8
		public override void _Ready()
		{
			this.DelaySpawn();
		}

		// Token: 0x060086F6 RID: 34550 RVA: 0x002D52E4 File Offset: 0x002D34E4
		[NullableContext(1)]
		private async Task DelaySpawn()
		{
			await Task.Delay(100);
			Control node = base.GetNode<Control>("Ironclad");
			NCardTrailVfx ncardTrailVfx = NCardTrailVfx.Create(node, SceneHelper.GetScenePath("vfx/card_trail_ironclad"));
			base.GetParent().AddChildSafely(ncardTrailVfx);
			Control node2 = base.GetNode<Control>("Silent");
			ncardTrailVfx = NCardTrailVfx.Create(node2, SceneHelper.GetScenePath("vfx/card_trail_silent"));
			base.GetParent().AddChildSafely(ncardTrailVfx);
			Control node3 = base.GetNode<Control>("Defect");
			ncardTrailVfx = NCardTrailVfx.Create(node3, SceneHelper.GetScenePath("vfx/card_trail_defect"));
			base.GetParent().AddChildSafely(ncardTrailVfx);
			Control node4 = base.GetNode<Control>("Regent");
			ncardTrailVfx = NCardTrailVfx.Create(node4, SceneHelper.GetScenePath("vfx/card_trail_regent"));
			base.GetParent().AddChildSafely(ncardTrailVfx);
			Control node5 = base.GetNode<Control>("Binder");
			ncardTrailVfx = NCardTrailVfx.Create(node5, SceneHelper.GetScenePath("vfx/card_trail_necrobinder"));
			base.GetParent().AddChildSafely(ncardTrailVfx);
		}

		// Token: 0x060086F7 RID: 34551 RVA: 0x002D5328 File Offset: 0x002D3528
		public override void _Process(double delta)
		{
			Vector2 mousePosition = base.GetViewport().GetMousePosition();
			base.GlobalPosition = mousePosition;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060086F8 RID: 34552 RVA: 0x002D5348 File Offset: 0x002D3548
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NTrailTest.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTrailTest.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060086F9 RID: 34553 RVA: 0x002D53E0 File Offset: 0x002D35E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTrailTest.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTrailTest.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060086FA RID: 34554 RVA: 0x002D544E File Offset: 0x002D364E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTrailTest.MethodName._Ready || (ref method) == NTrailTest.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060086FB RID: 34555 RVA: 0x002D5475 File Offset: 0x002D3675
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060086FC RID: 34556 RVA: 0x002D547E File Offset: 0x002D367E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200241D RID: 9245
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04009895 RID: 39061
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04009896 RID: 39062
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200241E RID: 9246
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200241F RID: 9247
		public class SignalName : Control.SignalName
		{
		}
	}
}

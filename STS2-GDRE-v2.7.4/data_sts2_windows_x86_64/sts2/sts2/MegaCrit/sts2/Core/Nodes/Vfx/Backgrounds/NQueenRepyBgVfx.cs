using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds
{
	// Token: 0x02000240 RID: 576
	[ScriptPath("res://src/Core/Nodes/Vfx/Backgrounds/NQueenRepyBgVfx.cs")]
	public class NQueenRepyBgVfx : TextureRect
	{
		// Token: 0x06001D5A RID: 7514 RVA: 0x000BF5DC File Offset: 0x000BD7DC
		public override void _EnterTree()
		{
			CombatManager.Instance.CombatSetUp += this.OnCombatSetUp;
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x000BF5F4 File Offset: 0x000BD7F4
		public override void _ExitTree()
		{
			CombatManager.Instance.CombatSetUp -= this.OnCombatSetUp;
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x000BF60C File Offset: 0x000BD80C
		[NullableContext(1)]
		private void OnCombatSetUp(CombatState combatState)
		{
			base.Visible = !combatState.RunState.ExtraFields.FreedRepy;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001D5D RID: 7517 RVA: 0x000BF628 File Offset: 0x000BD828
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NQueenRepyBgVfx.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NQueenRepyBgVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001D5E RID: 7518 RVA: 0x000BF69C File Offset: 0x000BD89C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NQueenRepyBgVfx.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NQueenRepyBgVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001D5F RID: 7519 RVA: 0x000BF6FC File Offset: 0x000BD8FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NQueenRepyBgVfx.MethodName._EnterTree || (ref method) == NQueenRepyBgVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001D60 RID: 7520 RVA: 0x000BF723 File Offset: 0x000BD923
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06001D61 RID: 7521 RVA: 0x000BF72C File Offset: 0x000BD92C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001235 RID: 4661
		public class MethodName : TextureRect.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x0400467E RID: 18046
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400467F RID: 18047
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001236 RID: 4662
		public class PropertyName : TextureRect.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001237 RID: 4663
		public class SignalName : TextureRect.SignalName
		{
		}
	}
}

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.sts2.Core.Nodes.TopBar
{
	// Token: 0x0200001E RID: 30
	[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarPortrait.cs")]
	public class NTopBarPortrait : Control
	{
		// Token: 0x06000105 RID: 261 RVA: 0x000089E1 File Offset: 0x00006BE1
		[NullableContext(1)]
		public void Initialize(Player player)
		{
			this.AddChildSafely(player.Character.Icon);
		}

		/// <inheritdoc />
		// Token: 0x06000106 RID: 262 RVA: 0x000089F4 File Offset: 0x00006BF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06000107 RID: 263 RVA: 0x000089FD File Offset: 0x00006BFD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DEA RID: 3562
		public class MethodName : Control.MethodName
		{
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DEB RID: 3563
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DEC RID: 3564
		public class SignalName : Control.SignalName
		{
		}
	}
}

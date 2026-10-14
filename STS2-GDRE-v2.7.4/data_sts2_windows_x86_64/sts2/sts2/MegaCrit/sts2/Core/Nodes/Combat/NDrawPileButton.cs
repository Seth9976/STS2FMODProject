using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	/// <summary>
	/// Node script for the Draw Pile.
	/// </summary>
	// Token: 0x020003FD RID: 1021
	[ScriptPath("res://src/Core/Nodes/Combat/NDrawPileButton.cs")]
	public class NDrawPileButton : NCombatCardPile
	{
		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003F75 RID: 16245 RVA: 0x001E6A6B File Offset: 0x001E4C6B
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.viewDrawPile };
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06003F76 RID: 16246 RVA: 0x001E6A80 File Offset: 0x001E4C80
		protected override PileType Pile
		{
			get
			{
				return PileType.Draw;
			}
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x001E6A83 File Offset: 0x001E4C83
		public override void _Ready()
		{
			this.ConnectSignals();
			this._emptyPileMessage = new LocString("combat_messages", "OPEN_EMPTY_DRAW");
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x001E6AA0 File Offset: 0x001E4CA0
		protected override void SetAnimInOutPositions()
		{
			this._showPosition = base.Position;
			this._hidePosition = base.Position + new Vector2(-150f, 100f);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003F79 RID: 16249 RVA: 0x001E6AD0 File Offset: 0x001E4CD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NDrawPileButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDrawPileButton.MethodName.SetAnimInOutPositions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003F7A RID: 16250 RVA: 0x001E6B44 File Offset: 0x001E4D44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDrawPileButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDrawPileButton.MethodName.SetAnimInOutPositions && args.Count == 0)
			{
				this.SetAnimInOutPositions();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003F7B RID: 16251 RVA: 0x001E6BA4 File Offset: 0x001E4DA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDrawPileButton.MethodName._Ready || (ref method) == NDrawPileButton.MethodName.SetAnimInOutPositions || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003F7C RID: 16252 RVA: 0x001E6BCC File Offset: 0x001E4DCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDrawPileButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NDrawPileButton.PropertyName.Pile)
			{
				PileType pile = this.Pile;
				value = VariantUtils.CreateFrom<PileType>(ref pile);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003F7D RID: 16253 RVA: 0x001E6C28 File Offset: 0x001E4E28
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NDrawPileButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(2L, NDrawPileButton.PropertyName.Pile, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003F7E RID: 16254 RVA: 0x001E6C7D File Offset: 0x001E4E7D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003F7F RID: 16255 RVA: 0x001E6C86 File Offset: 0x001E4E86
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018D9 RID: 6361
		public new class MethodName : NCombatCardPile.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006154 RID: 24916
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetAnimInOutPositions' method.
			/// </summary>
			// Token: 0x04006155 RID: 24917
			public new static readonly StringName SetAnimInOutPositions = "SetAnimInOutPositions";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018DA RID: 6362
		public new class PropertyName : NCombatCardPile.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04006156 RID: 24918
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the 'Pile' property.
			/// </summary>
			// Token: 0x04006157 RID: 24919
			public new static readonly StringName Pile = "Pile";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018DB RID: 6363
		public new class SignalName : NCombatCardPile.SignalName
		{
		}
	}
}

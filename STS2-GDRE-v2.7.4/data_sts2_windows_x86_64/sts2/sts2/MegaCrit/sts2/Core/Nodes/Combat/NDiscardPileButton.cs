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
	/// Node script for the Discard Pile.
	/// </summary>
	// Token: 0x020003FC RID: 1020
	[ScriptPath("res://src/Core/Nodes/Combat/NDiscardPileButton.cs")]
	public class NDiscardPileButton : NCombatCardPile
	{
		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06003F69 RID: 16233 RVA: 0x001E6840 File Offset: 0x001E4A40
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.viewDiscardPile };
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06003F6A RID: 16234 RVA: 0x001E6855 File Offset: 0x001E4A55
		protected override PileType Pile
		{
			get
			{
				return PileType.Discard;
			}
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x001E6858 File Offset: 0x001E4A58
		public override void _Ready()
		{
			this.ConnectSignals();
			this._emptyPileMessage = new LocString("combat_messages", "OPEN_EMPTY_DISCARD");
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x001E6875 File Offset: 0x001E4A75
		protected override void SetAnimInOutPositions()
		{
			this._showPosition = base.Position;
			this._hidePosition = base.Position + new Vector2(150f, 100f);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003F6D RID: 16237 RVA: 0x001E68A4 File Offset: 0x001E4AA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NDiscardPileButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDiscardPileButton.MethodName.SetAnimInOutPositions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003F6E RID: 16238 RVA: 0x001E6918 File Offset: 0x001E4B18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDiscardPileButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDiscardPileButton.MethodName.SetAnimInOutPositions && args.Count == 0)
			{
				this.SetAnimInOutPositions();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003F6F RID: 16239 RVA: 0x001E6978 File Offset: 0x001E4B78
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDiscardPileButton.MethodName._Ready || (ref method) == NDiscardPileButton.MethodName.SetAnimInOutPositions || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003F70 RID: 16240 RVA: 0x001E69A0 File Offset: 0x001E4BA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDiscardPileButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NDiscardPileButton.PropertyName.Pile)
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
		// Token: 0x06003F71 RID: 16241 RVA: 0x001E69FC File Offset: 0x001E4BFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NDiscardPileButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(2L, NDiscardPileButton.PropertyName.Pile, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003F72 RID: 16242 RVA: 0x001E6A51 File Offset: 0x001E4C51
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003F73 RID: 16243 RVA: 0x001E6A5A File Offset: 0x001E4C5A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018D6 RID: 6358
		public new class MethodName : NCombatCardPile.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006150 RID: 24912
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetAnimInOutPositions' method.
			/// </summary>
			// Token: 0x04006151 RID: 24913
			public new static readonly StringName SetAnimInOutPositions = "SetAnimInOutPositions";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018D7 RID: 6359
		public new class PropertyName : NCombatCardPile.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04006152 RID: 24914
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the 'Pile' property.
			/// </summary>
			// Token: 0x04006153 RID: 24915
			public new static readonly StringName Pile = "Pile";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018D8 RID: 6360
		public new class SignalName : NCombatCardPile.SignalName
		{
		}
	}
}

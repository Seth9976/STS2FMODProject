using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	/// <summary>
	/// Node script for the Exhaust Pile.
	/// </summary>
	// Token: 0x02000402 RID: 1026
	[ScriptPath("res://src/Core/Nodes/Combat/NExhaustPileButton.cs")]
	public class NExhaustPileButton : NCombatCardPile
	{
		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06003FE6 RID: 16358 RVA: 0x001EAD94 File Offset: 0x001E8F94
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.viewExhaustPileAndTabRight };
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003FE7 RID: 16359 RVA: 0x001EADA9 File Offset: 0x001E8FA9
		protected override PileType Pile
		{
			get
			{
				return PileType.Exhaust;
			}
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x001EADAC File Offset: 0x001E8FAC
		public override void _Ready()
		{
			this.ConnectSignals();
			base.Visible = false;
			this._viewport = base.GetViewport();
			this._posOffset = new Vector2(base.OffsetRight + 100f, -base.OffsetBottom + 90f);
			base.GetTree().Root.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.SetAnimInOutPositions)), 0U);
			this.SetAnimInOutPositions();
			base.Disable();
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x001EAE2B File Offset: 0x001E902B
		[NullableContext(1)]
		public override void Initialize(Player player)
		{
			base.Initialize(player);
			if (this.Pile.GetPile(player).Cards.Count > 0)
			{
				base.Visible = true;
				base.Position = this._showPosition;
				base.Enable();
			}
		}

		/// <summary>
		/// The text "bump" animation for pile UI.
		/// </summary>
		// Token: 0x06003FEA RID: 16362 RVA: 0x001EAE66 File Offset: 0x001E9066
		protected override void AddCard()
		{
			base.AddCard();
			if (!base.Visible)
			{
				this.AnimIn();
			}
			base.Enable();
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x001EAE82 File Offset: 0x001E9082
		protected override void SetAnimInOutPositions()
		{
			this._showPosition = NGame.Instance.Size - this._posOffset;
			this._hidePosition = this._showPosition + NExhaustPileButton._hideOffset;
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x001EAEB5 File Offset: 0x001E90B5
		public override void AnimIn()
		{
			base.AnimIn();
			base.Visible = true;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003FED RID: 16365 RVA: 0x001EAEC4 File Offset: 0x001E90C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NExhaustPileButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NExhaustPileButton.MethodName.AddCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NExhaustPileButton.MethodName.SetAnimInOutPositions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NExhaustPileButton.MethodName.AnimIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003FEE RID: 16366 RVA: 0x001EAF98 File Offset: 0x001E9198
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NExhaustPileButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NExhaustPileButton.MethodName.AddCard && args.Count == 0)
			{
				this.AddCard();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NExhaustPileButton.MethodName.SetAnimInOutPositions && args.Count == 0)
			{
				this.SetAnimInOutPositions();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NExhaustPileButton.MethodName.AnimIn && args.Count == 0)
			{
				this.AnimIn();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003FEF RID: 16367 RVA: 0x001EB044 File Offset: 0x001E9244
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NExhaustPileButton.MethodName._Ready || (ref method) == NExhaustPileButton.MethodName.AddCard || (ref method) == NExhaustPileButton.MethodName.SetAnimInOutPositions || (ref method) == NExhaustPileButton.MethodName.AnimIn || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003FF0 RID: 16368 RVA: 0x001EB094 File Offset: 0x001E9294
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NExhaustPileButton.PropertyName._viewport)
			{
				this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
				return true;
			}
			if ((ref name) == NExhaustPileButton.PropertyName._posOffset)
			{
				this._posOffset = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003FF1 RID: 16369 RVA: 0x001EB0D4 File Offset: 0x001E92D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NExhaustPileButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NExhaustPileButton.PropertyName.Pile)
			{
				PileType pile = this.Pile;
				value = VariantUtils.CreateFrom<PileType>(ref pile);
				return true;
			}
			if ((ref name) == NExhaustPileButton.PropertyName._viewport)
			{
				value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
				return true;
			}
			if ((ref name) == NExhaustPileButton.PropertyName._posOffset)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._posOffset);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003FF2 RID: 16370 RVA: 0x001EB170 File Offset: 0x001E9370
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NExhaustPileButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(24L, NExhaustPileButton.PropertyName._viewport, 0L, "", 4096L, false),
				new PropertyInfo(5L, NExhaustPileButton.PropertyName._posOffset, 0L, "", 4096L, false),
				new PropertyInfo(2L, NExhaustPileButton.PropertyName.Pile, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003FF3 RID: 16371 RVA: 0x001EB206 File Offset: 0x001E9406
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NExhaustPileButton.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
			info.AddProperty(NExhaustPileButton.PropertyName._posOffset, Variant.From<Vector2>(ref this._posOffset));
		}

		/// <inheritdoc />
		// Token: 0x06003FF4 RID: 16372 RVA: 0x001EB23C File Offset: 0x001E943C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NExhaustPileButton.PropertyName._viewport, ref variant))
			{
				this._viewport = variant.As<Viewport>();
			}
			Variant variant2;
			if (info.TryGetProperty(NExhaustPileButton.PropertyName._posOffset, ref variant2))
			{
				this._posOffset = variant2.As<Vector2>();
			}
		}

		// Token: 0x040018C7 RID: 6343
		[Nullable(1)]
		private Viewport _viewport;

		// Token: 0x040018C8 RID: 6344
		private Vector2 _posOffset;

		// Token: 0x040018C9 RID: 6345
		private static readonly Vector2 _hideOffset = new Vector2(150f, 0f);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018EB RID: 6379
		public new class MethodName : NCombatCardPile.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040061B2 RID: 25010
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'AddCard' method.
			/// </summary>
			// Token: 0x040061B3 RID: 25011
			public new static readonly StringName AddCard = "AddCard";

			/// <summary>
			/// Cached name for the 'SetAnimInOutPositions' method.
			/// </summary>
			// Token: 0x040061B4 RID: 25012
			public new static readonly StringName SetAnimInOutPositions = "SetAnimInOutPositions";

			/// <summary>
			/// Cached name for the 'AnimIn' method.
			/// </summary>
			// Token: 0x040061B5 RID: 25013
			public new static readonly StringName AnimIn = "AnimIn";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018EC RID: 6380
		public new class PropertyName : NCombatCardPile.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x040061B6 RID: 25014
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the 'Pile' property.
			/// </summary>
			// Token: 0x040061B7 RID: 25015
			public new static readonly StringName Pile = "Pile";

			/// <summary>
			/// Cached name for the '_viewport' field.
			/// </summary>
			// Token: 0x040061B8 RID: 25016
			public static readonly StringName _viewport = "_viewport";

			/// <summary>
			/// Cached name for the '_posOffset' field.
			/// </summary>
			// Token: 0x040061B9 RID: 25017
			public static readonly StringName _posOffset = "_posOffset";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018ED RID: 6381
		public new class SignalName : NCombatCardPile.SignalName
		{
		}
	}
}

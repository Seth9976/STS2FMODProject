using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	/// <summary>
	/// Manages a set of <see cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NCombatCardPile" />s for a given player.
	/// Primarily used in  <see cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NCombatUi" />, but reused in other screens
	/// where we want to be able to check on the combat piles (ie <see cref="T:MegaCrit.Sts2.Core.Nodes.Screens.CardSelection.NChooseACardSelectionScreen" />)
	/// </summary>
	// Token: 0x020003F4 RID: 1012
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Combat/NCombatPilesContainer.cs")]
	public class NCombatPilesContainer : Control
	{
		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06003E31 RID: 15921 RVA: 0x001DD0A6 File Offset: 0x001DB2A6
		public NDrawPileButton DrawPile
		{
			get
			{
				return this._drawPile;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06003E32 RID: 15922 RVA: 0x001DD0AE File Offset: 0x001DB2AE
		public NDiscardPileButton DiscardPile
		{
			get
			{
				return this._discardPile;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06003E33 RID: 15923 RVA: 0x001DD0B6 File Offset: 0x001DB2B6
		public NExhaustPileButton ExhaustPile
		{
			get
			{
				return this._exhaustPile;
			}
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x001DD0C0 File Offset: 0x001DB2C0
		public override void _Ready()
		{
			this._drawPile = base.GetNode<NDrawPileButton>("%DrawPile");
			this._discardPile = base.GetNode<NDiscardPileButton>("%DiscardPile");
			this._exhaustPile = base.GetNode<NExhaustPileButton>("%ExhaustPile");
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x001DD10F File Offset: 0x001DB30F
		public void Initialize(Player player)
		{
			this._drawPile.Initialize(player);
			this._discardPile.Initialize(player);
			this._exhaustPile.Initialize(player);
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x001DD135 File Offset: 0x001DB335
		public void AnimIn()
		{
			this._drawPile.AnimIn();
			this._discardPile.AnimIn();
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x001DD14D File Offset: 0x001DB34D
		public void AnimOut()
		{
			this._drawPile.AnimOut();
			this._discardPile.AnimOut();
			this._exhaustPile.AnimOut();
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x001DD170 File Offset: 0x001DB370
		public void Enable()
		{
			this._drawPile.Enable();
			this._discardPile.Enable();
			this._exhaustPile.Enable();
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x001DD193 File Offset: 0x001DB393
		public void Disable()
		{
			this._drawPile.Disable();
			this._discardPile.Disable();
			this._exhaustPile.Disable();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003E3A RID: 15930 RVA: 0x001DD1B8 File Offset: 0x001DB3B8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NCombatPilesContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPilesContainer.MethodName.AnimIn, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPilesContainer.MethodName.AnimOut, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPilesContainer.MethodName.Enable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatPilesContainer.MethodName.Disable, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003E3B RID: 15931 RVA: 0x001DD2B8 File Offset: 0x001DB4B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatPilesContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPilesContainer.MethodName.AnimIn && args.Count == 0)
			{
				this.AnimIn();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPilesContainer.MethodName.AnimOut && args.Count == 0)
			{
				this.AnimOut();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPilesContainer.MethodName.Enable && args.Count == 0)
			{
				this.Enable();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatPilesContainer.MethodName.Disable && args.Count == 0)
			{
				this.Disable();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003E3C RID: 15932 RVA: 0x001DD388 File Offset: 0x001DB588
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatPilesContainer.MethodName._Ready || (ref method) == NCombatPilesContainer.MethodName.AnimIn || (ref method) == NCombatPilesContainer.MethodName.AnimOut || (ref method) == NCombatPilesContainer.MethodName.Enable || (ref method) == NCombatPilesContainer.MethodName.Disable || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003E3D RID: 15933 RVA: 0x001DD3E8 File Offset: 0x001DB5E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCombatPilesContainer.PropertyName._drawPile)
			{
				this._drawPile = VariantUtils.ConvertTo<NDrawPileButton>(ref value);
				return true;
			}
			if ((ref name) == NCombatPilesContainer.PropertyName._discardPile)
			{
				this._discardPile = VariantUtils.ConvertTo<NDiscardPileButton>(ref value);
				return true;
			}
			if ((ref name) == NCombatPilesContainer.PropertyName._exhaustPile)
			{
				this._exhaustPile = VariantUtils.ConvertTo<NExhaustPileButton>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003E3E RID: 15934 RVA: 0x001DD450 File Offset: 0x001DB650
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCombatPilesContainer.PropertyName.DrawPile)
			{
				NDrawPileButton drawPile = this.DrawPile;
				value = VariantUtils.CreateFrom<NDrawPileButton>(ref drawPile);
				return true;
			}
			if ((ref name) == NCombatPilesContainer.PropertyName.DiscardPile)
			{
				NDiscardPileButton discardPile = this.DiscardPile;
				value = VariantUtils.CreateFrom<NDiscardPileButton>(ref discardPile);
				return true;
			}
			if ((ref name) == NCombatPilesContainer.PropertyName.ExhaustPile)
			{
				NExhaustPileButton exhaustPile = this.ExhaustPile;
				value = VariantUtils.CreateFrom<NExhaustPileButton>(ref exhaustPile);
				return true;
			}
			if ((ref name) == NCombatPilesContainer.PropertyName._drawPile)
			{
				value = VariantUtils.CreateFrom<NDrawPileButton>(ref this._drawPile);
				return true;
			}
			if ((ref name) == NCombatPilesContainer.PropertyName._discardPile)
			{
				value = VariantUtils.CreateFrom<NDiscardPileButton>(ref this._discardPile);
				return true;
			}
			if ((ref name) == NCombatPilesContainer.PropertyName._exhaustPile)
			{
				value = VariantUtils.CreateFrom<NExhaustPileButton>(ref this._exhaustPile);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003E3F RID: 15935 RVA: 0x001DD530 File Offset: 0x001DB730
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCombatPilesContainer.PropertyName._drawPile, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatPilesContainer.PropertyName._discardPile, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatPilesContainer.PropertyName._exhaustPile, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatPilesContainer.PropertyName.DrawPile, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatPilesContainer.PropertyName.DiscardPile, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatPilesContainer.PropertyName.ExhaustPile, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003E40 RID: 15936 RVA: 0x001DD60C File Offset: 0x001DB80C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCombatPilesContainer.PropertyName._drawPile, Variant.From<NDrawPileButton>(ref this._drawPile));
			info.AddProperty(NCombatPilesContainer.PropertyName._discardPile, Variant.From<NDiscardPileButton>(ref this._discardPile));
			info.AddProperty(NCombatPilesContainer.PropertyName._exhaustPile, Variant.From<NExhaustPileButton>(ref this._exhaustPile));
		}

		/// <inheritdoc />
		// Token: 0x06003E41 RID: 15937 RVA: 0x001DD664 File Offset: 0x001DB864
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCombatPilesContainer.PropertyName._drawPile, ref variant))
			{
				this._drawPile = variant.As<NDrawPileButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCombatPilesContainer.PropertyName._discardPile, ref variant2))
			{
				this._discardPile = variant2.As<NDiscardPileButton>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCombatPilesContainer.PropertyName._exhaustPile, ref variant3))
			{
				this._exhaustPile = variant3.As<NExhaustPileButton>();
			}
		}

		// Token: 0x04001832 RID: 6194
		public static readonly string scenePath = SceneHelper.GetScenePath("combat/combat_piles_container");

		// Token: 0x04001833 RID: 6195
		private NDrawPileButton _drawPile;

		// Token: 0x04001834 RID: 6196
		private NDiscardPileButton _discardPile;

		// Token: 0x04001835 RID: 6197
		private NExhaustPileButton _exhaustPile;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018AB RID: 6315
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400606C RID: 24684
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'AnimIn' method.
			/// </summary>
			// Token: 0x0400606D RID: 24685
			public static readonly StringName AnimIn = "AnimIn";

			/// <summary>
			/// Cached name for the 'AnimOut' method.
			/// </summary>
			// Token: 0x0400606E RID: 24686
			public static readonly StringName AnimOut = "AnimOut";

			/// <summary>
			/// Cached name for the 'Enable' method.
			/// </summary>
			// Token: 0x0400606F RID: 24687
			public static readonly StringName Enable = "Enable";

			/// <summary>
			/// Cached name for the 'Disable' method.
			/// </summary>
			// Token: 0x04006070 RID: 24688
			public static readonly StringName Disable = "Disable";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018AC RID: 6316
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DrawPile' property.
			/// </summary>
			// Token: 0x04006071 RID: 24689
			public static readonly StringName DrawPile = "DrawPile";

			/// <summary>
			/// Cached name for the 'DiscardPile' property.
			/// </summary>
			// Token: 0x04006072 RID: 24690
			public static readonly StringName DiscardPile = "DiscardPile";

			/// <summary>
			/// Cached name for the 'ExhaustPile' property.
			/// </summary>
			// Token: 0x04006073 RID: 24691
			public static readonly StringName ExhaustPile = "ExhaustPile";

			/// <summary>
			/// Cached name for the '_drawPile' field.
			/// </summary>
			// Token: 0x04006074 RID: 24692
			public static readonly StringName _drawPile = "_drawPile";

			/// <summary>
			/// Cached name for the '_discardPile' field.
			/// </summary>
			// Token: 0x04006075 RID: 24693
			public static readonly StringName _discardPile = "_discardPile";

			/// <summary>
			/// Cached name for the '_exhaustPile' field.
			/// </summary>
			// Token: 0x04006076 RID: 24694
			public static readonly StringName _exhaustPile = "_exhaustPile";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018AD RID: 6317
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}

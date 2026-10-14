using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	// Token: 0x0200038C RID: 908
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerCardIntent.cs")]
	public class NMultiplayerCardIntent : Control
	{
		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06003602 RID: 13826 RVA: 0x00192E88 File Offset: 0x00191088
		// (set) Token: 0x06003603 RID: 13827 RVA: 0x00192E90 File Offset: 0x00191090
		[Nullable(2)]
		public CardModel Card
		{
			[NullableContext(2)]
			get
			{
				return this._card;
			}
			[NullableContext(2)]
			set
			{
				this._cardNode.Model = value;
				this._card = value;
				if (this._card != null && base.IsNodeReady())
				{
					NCard cardNode = this._cardNode;
					CardPile pile = this._card.Pile;
					cardNode.UpdateVisuals((pile != null) ? pile.Type : PileType.None, CardPreviewMode.Normal);
				}
			}
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x00192EE4 File Offset: 0x001910E4
		public override void _Ready()
		{
			this._cardNode = base.GetNode<NCard>("%Card");
			if (this._card != null)
			{
				NCard cardNode = this._cardNode;
				CardPile pile = this._card.Pile;
				cardNode.UpdateVisuals((pile != null) ? pile.Type : PileType.None, CardPreviewMode.Normal);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003605 RID: 13829 RVA: 0x00192F34 File Offset: 0x00191134
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NMultiplayerCardIntent.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003606 RID: 13830 RVA: 0x00192F78 File Offset: 0x00191178
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerCardIntent.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003607 RID: 13831 RVA: 0x00192FA8 File Offset: 0x001911A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerCardIntent.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003608 RID: 13832 RVA: 0x00192FC0 File Offset: 0x001911C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMultiplayerCardIntent.PropertyName._cardNode)
			{
				this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003609 RID: 13833 RVA: 0x00192FE5 File Offset: 0x001911E5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMultiplayerCardIntent.PropertyName._cardNode)
			{
				value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600360A RID: 13834 RVA: 0x00193010 File Offset: 0x00191210
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMultiplayerCardIntent.PropertyName._cardNode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600360B RID: 13835 RVA: 0x00193045 File Offset: 0x00191245
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMultiplayerCardIntent.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
		}

		/// <inheritdoc />
		// Token: 0x0600360C RID: 13836 RVA: 0x00193064 File Offset: 0x00191264
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMultiplayerCardIntent.PropertyName._cardNode, ref variant))
			{
				this._cardNode = variant.As<NCard>();
			}
		}

		// Token: 0x040014E3 RID: 5347
		[Nullable(1)]
		private NCard _cardNode;

		// Token: 0x040014E4 RID: 5348
		[Nullable(2)]
		private CardModel _card;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001715 RID: 5909
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040059F4 RID: 23028
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001716 RID: 5910
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_cardNode' field.
			/// </summary>
			// Token: 0x040059F5 RID: 23029
			public static readonly StringName _cardNode = "_cardNode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001717 RID: 5911
		public class SignalName : Control.SignalName
		{
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace MegaCrit.Sts2.Core.Nodes.HoverTips
{
	// Token: 0x02000399 RID: 921
	[ScriptPath("res://src/Core/Nodes/HoverTips/NHoverTipCardContainer.cs")]
	public class NHoverTipCardContainer : Control
	{
		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06003731 RID: 14129 RVA: 0x0019EF84 File Offset: 0x0019D184
		[Nullable(1)]
		private IEnumerable<Control> Tips
		{
			[NullableContext(1)]
			get
			{
				return base.GetChildren(false).OfType<Control>();
			}
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x0019EF94 File Offset: 0x0019D194
		[NullableContext(1)]
		public void Add(CardHoverTip cardTip)
		{
			Control control = PreloadManager.Cache.GetScene("res://scenes/ui/card_hover_tip.tscn").Instantiate<Control>(0L);
			this.AddChildSafely(control);
			NCard node = control.GetNode<NCard>("%Card");
			node.Model = cardTip.Card;
			node.UpdateVisuals(PileType.Deck, CardPreviewMode.Normal);
		}

		/// <summary>
		/// Lays out cards vertically, then horizontally, then sets the position and the size of the container according
		/// to the passed global start position and alignment.
		/// </summary>
		/// <param name="globalStartLocation">Where to start positioning nodes.</param>
		/// <param name="alignment">Which side of the global start location the cards should be placed on.</param>
		/// <returns></returns>
		// Token: 0x06003733 RID: 14131 RVA: 0x0019EFE4 File Offset: 0x0019D1E4
		public void LayoutResizeAndReposition(Vector2 globalStartLocation, HoverTipAlignment alignment)
		{
			Vector2 size = NGame.Instance.GetViewportRect().Size;
			Vector2 vector = Vector2.Zero;
			Vector2 vector2 = Vector2.Zero;
			float num = 0f;
			foreach (Control control in this.Tips)
			{
				control.Position = vector2;
				vector = new Vector2(Mathf.Max(vector2.X + control.Size.X, vector.X), Mathf.Max(vector2.Y + control.Size.Y, vector.Y));
				vector2 += Vector2.Down * (control.Size.Y + 4f);
				num = Mathf.Max(control.Size.X, num);
			}
			if (alignment == HoverTipAlignment.Right)
			{
				base.GlobalPosition = globalStartLocation;
			}
			else if (alignment == HoverTipAlignment.Left)
			{
				base.GlobalPosition = globalStartLocation + Vector2.Left * vector.X;
			}
			base.Size = vector;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003734 RID: 14132 RVA: 0x0019F110 File Offset: 0x0019D310
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NHoverTipCardContainer.MethodName.LayoutResizeAndReposition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "globalStartLocation", 0L, "", 6L, false),
					new PropertyInfo(2L, "alignment", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003735 RID: 14133 RVA: 0x0019F19C File Offset: 0x0019D39C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHoverTipCardContainer.MethodName.LayoutResizeAndReposition && args.Count == 2)
			{
				this.LayoutResizeAndReposition(VariantUtils.ConvertTo<Vector2>(args[0]), VariantUtils.ConvertTo<HoverTipAlignment>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003736 RID: 14134 RVA: 0x0019F1F2 File Offset: 0x0019D3F2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHoverTipCardContainer.MethodName.LayoutResizeAndReposition || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003737 RID: 14135 RVA: 0x0019F20A File Offset: 0x0019D40A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003738 RID: 14136 RVA: 0x0019F213 File Offset: 0x0019D413
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04001566 RID: 5478
		[Nullable(1)]
		private const string _cardHoverTipScenePath = "res://scenes/ui/card_hover_tip.tscn";

		// Token: 0x04001567 RID: 5479
		private const float _padding = 4f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001756 RID: 5974
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'LayoutResizeAndReposition' method.
			/// </summary>
			// Token: 0x04005B0E RID: 23310
			public static readonly StringName LayoutResizeAndReposition = "LayoutResizeAndReposition";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001757 RID: 5975
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001758 RID: 5976
		public class SignalName : Control.SignalName
		{
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Cards
{
	/// <summary>
	/// Used in Events. Displays original card on left and a card that rapidly cycles through random cards on the right.
	/// </summary>
	// Token: 0x02000419 RID: 1049
	[ScriptPath("res://src/Core/Nodes/Cards/NTransformPreview.cs")]
	public class NTransformPreview : Control
	{
		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x060042AD RID: 17069 RVA: 0x00206764 File Offset: 0x00204964
		public Vector2 SelectedCardPosition
		{
			get
			{
				return this._before.GlobalPosition;
			}
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x00206774 File Offset: 0x00204974
		public override void _Ready()
		{
			this._before = base.GetNode<Control>("%Before");
			this._after = base.GetNode<Control>("%After");
			this._arrows = base.GetNode<Control>("Arrows");
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x002067C3 File Offset: 0x002049C3
		public override void _ExitTree()
		{
			CancellationTokenSource cancelTokenSource = this._cancelTokenSource;
			if (cancelTokenSource == null)
			{
				return;
			}
			cancelTokenSource.Cancel();
		}

		// Token: 0x060042B0 RID: 17072 RVA: 0x002067D8 File Offset: 0x002049D8
		[NullableContext(1)]
		public void Initialize(IEnumerable<CardTransformation> cardTransformations)
		{
			this.RemoveExistingCards();
			CancellationTokenSource cancelTokenSource = this._cancelTokenSource;
			if (cancelTokenSource != null)
			{
				cancelTokenSource.Cancel();
			}
			List<CardTransformation> list = cardTransformations.ToList<CardTransformation>();
			float num = this._before.GlobalPosition.X - 100f;
			float num2 = Math.Min(num / ((float)list.Count * 300f + (float)(list.Count - 1) * 30f), 1f);
			for (int i = 0; i < list.Count; i++)
			{
				CardTransformation cardTransformation = list[i];
				NCombatRoom instance = NCombatRoom.Instance;
				NPlayerHand nplayerHand = ((instance != null) ? instance.Ui.Hand : null);
				NPreviewCardHolder npreviewCardHolder = NPreviewCardHolder.Create(NCard.Create(cardTransformation.Original, ModelVisibility.Visible), nplayerHand == null, nplayerHand != null);
				this._before.AddChildSafely(npreviewCardHolder);
				npreviewCardHolder.FocusMode = 2L;
				npreviewCardHolder.CardNode.UpdateVisuals(cardTransformation.Original.Pile.Type, CardPreviewMode.Normal);
				npreviewCardHolder.SetCardScale(Vector2.One * num2);
				int num3 = list.Count - i;
				npreviewCardHolder.Position = new Vector2(-((float)num3 - 0.5f) * 300f * num2 - (float)(num3 - 1) * 30f, 0f);
				NCard ncard;
				if (cardTransformation.Replacement != null)
				{
					ncard = NCard.Create(cardTransformation.Replacement, ModelVisibility.Visible);
				}
				else
				{
					ncard = NCard.Create(cardTransformation.Original, ModelVisibility.Visible);
				}
				NPreviewCardHolder npreviewCardHolder2 = NPreviewCardHolder.Create(ncard, true, false);
				npreviewCardHolder2.FocusMode = 0L;
				this._after.AddChildSafely(npreviewCardHolder2);
				npreviewCardHolder2.CardNode.UpdateVisuals(cardTransformation.Original.Pile.Type, CardPreviewMode.Normal);
				npreviewCardHolder2.Scale = Vector2.One * num2;
				npreviewCardHolder2.Position = new Vector2(((float)i + 0.5f) * 300f * num2 + (float)i * 30f, 0f);
				if (cardTransformation.Replacement == null)
				{
					npreviewCardHolder2.Hitbox.MouseFilter = 2L;
					IEnumerable<CardModel> enumerable;
					if (cardTransformation.ReplacementOptions != null)
					{
						enumerable = cardTransformation.ReplacementOptions;
					}
					else
					{
						enumerable = CardFactory.GetDefaultTransformationOptions(cardTransformation.Original, cardTransformation.IsInCombat);
					}
					TaskHelper.RunSafely(this.CycleThroughCards(npreviewCardHolder2, cardTransformation.Original.Pile, enumerable));
				}
			}
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x00206A19 File Offset: 0x00204C19
		public void Uninitialize()
		{
			CancellationTokenSource cancelTokenSource = this._cancelTokenSource;
			if (cancelTokenSource == null)
			{
				return;
			}
			cancelTokenSource.Cancel();
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x00206A2C File Offset: 0x00204C2C
		[NullableContext(1)]
		private async Task CycleThroughCards(NPreviewCardHolder holder, CardPile cardPile, IEnumerable<CardModel> possibleTransformations)
		{
			this._cancelTokenSource = new CancellationTokenSource();
			List<CardModel> cards = possibleTransformations.ToList<CardModel>();
			cards.UnstableShuffle<CardModel>(Rng.Chaotic);
			int cardIndex = 0;
			while (!this._cancelTokenSource.IsCancellationRequested && holder.CardNode != null)
			{
				holder.ReassignToCard(cards[cardIndex], cardPile.Type, null, ModelVisibility.Visible);
				int num = cardIndex;
				cardIndex = num + 1;
				if (cardIndex >= cards.Count)
				{
					cards.UnstableShuffle<CardModel>(Rng.Chaotic);
					cardIndex = 0;
				}
				if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
				{
					await Task.Delay(200, this._cancelTokenSource.Token);
				}
				else
				{
					await Cmd.Wait(0.2f, this._cancelTokenSource.Token, true);
				}
			}
		}

		/// <summary>
		/// Helper function to remove any existing preview cards
		/// </summary>
		// Token: 0x060042B3 RID: 17075 RVA: 0x00206A88 File Offset: 0x00204C88
		private void RemoveExistingCards()
		{
			foreach (Node node in this._before.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
			foreach (Node node2 in this._after.GetChildren(false))
			{
				node2.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060042B4 RID: 17076 RVA: 0x00206B1C File Offset: 0x00204D1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NTransformPreview.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTransformPreview.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTransformPreview.MethodName.Uninitialize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTransformPreview.MethodName.RemoveExistingCards, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060042B5 RID: 17077 RVA: 0x00206BF0 File Offset: 0x00204DF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTransformPreview.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTransformPreview.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTransformPreview.MethodName.Uninitialize && args.Count == 0)
			{
				this.Uninitialize();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTransformPreview.MethodName.RemoveExistingCards && args.Count == 0)
			{
				this.RemoveExistingCards();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060042B6 RID: 17078 RVA: 0x00206C9C File Offset: 0x00204E9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTransformPreview.MethodName._Ready || (ref method) == NTransformPreview.MethodName._ExitTree || (ref method) == NTransformPreview.MethodName.Uninitialize || (ref method) == NTransformPreview.MethodName.RemoveExistingCards || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060042B7 RID: 17079 RVA: 0x00206CEC File Offset: 0x00204EEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTransformPreview.PropertyName._before)
			{
				this._before = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTransformPreview.PropertyName._after)
			{
				this._after = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NTransformPreview.PropertyName._arrows)
			{
				this._arrows = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060042B8 RID: 17080 RVA: 0x00206D54 File Offset: 0x00204F54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTransformPreview.PropertyName.SelectedCardPosition)
			{
				Vector2 selectedCardPosition = this.SelectedCardPosition;
				value = VariantUtils.CreateFrom<Vector2>(ref selectedCardPosition);
				return true;
			}
			if ((ref name) == NTransformPreview.PropertyName._before)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._before);
				return true;
			}
			if ((ref name) == NTransformPreview.PropertyName._after)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._after);
				return true;
			}
			if ((ref name) == NTransformPreview.PropertyName._arrows)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._arrows);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060042B9 RID: 17081 RVA: 0x00206DEC File Offset: 0x00204FEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTransformPreview.PropertyName._before, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTransformPreview.PropertyName._after, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTransformPreview.PropertyName._arrows, 0L, "", 4096L, false),
				new PropertyInfo(5L, NTransformPreview.PropertyName.SelectedCardPosition, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060042BA RID: 17082 RVA: 0x00206E84 File Offset: 0x00205084
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTransformPreview.PropertyName._before, Variant.From<Control>(ref this._before));
			info.AddProperty(NTransformPreview.PropertyName._after, Variant.From<Control>(ref this._after));
			info.AddProperty(NTransformPreview.PropertyName._arrows, Variant.From<Control>(ref this._arrows));
		}

		/// <inheritdoc />
		// Token: 0x060042BB RID: 17083 RVA: 0x00206EDC File Offset: 0x002050DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTransformPreview.PropertyName._before, ref variant))
			{
				this._before = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTransformPreview.PropertyName._after, ref variant2))
			{
				this._after = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTransformPreview.PropertyName._arrows, ref variant3))
			{
				this._arrows = variant3.As<Control>();
			}
		}

		// Token: 0x04001A05 RID: 6661
		[Nullable(1)]
		private Control _before;

		// Token: 0x04001A06 RID: 6662
		[Nullable(1)]
		private Control _after;

		// Token: 0x04001A07 RID: 6663
		[Nullable(1)]
		private Control _arrows;

		// Token: 0x04001A08 RID: 6664
		[Nullable(2)]
		private CancellationTokenSource _cancelTokenSource;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001954 RID: 6484
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006407 RID: 25607
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04006408 RID: 25608
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'Uninitialize' method.
			/// </summary>
			// Token: 0x04006409 RID: 25609
			public static readonly StringName Uninitialize = "Uninitialize";

			/// <summary>
			/// Cached name for the 'RemoveExistingCards' method.
			/// </summary>
			// Token: 0x0400640A RID: 25610
			public static readonly StringName RemoveExistingCards = "RemoveExistingCards";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001955 RID: 6485
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'SelectedCardPosition' property.
			/// </summary>
			// Token: 0x0400640B RID: 25611
			public static readonly StringName SelectedCardPosition = "SelectedCardPosition";

			/// <summary>
			/// Cached name for the '_before' field.
			/// </summary>
			// Token: 0x0400640C RID: 25612
			public static readonly StringName _before = "_before";

			/// <summary>
			/// Cached name for the '_after' field.
			/// </summary>
			// Token: 0x0400640D RID: 25613
			public static readonly StringName _after = "_after";

			/// <summary>
			/// Cached name for the '_arrows' field.
			/// </summary>
			// Token: 0x0400640E RID: 25614
			public static readonly StringName _arrows = "_arrows";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001956 RID: 6486
		public class SignalName : Control.SignalName
		{
		}
	}
}

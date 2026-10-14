using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	// Token: 0x0200040D RID: 1037
	[ScriptPath("res://src/Core/Nodes/Combat/NSelectedHandCardContainer.cs")]
	public class NSelectedHandCardContainer : Control
	{
		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06004137 RID: 16695 RVA: 0x001F8000 File Offset: 0x001F6200
		// (set) Token: 0x06004138 RID: 16696 RVA: 0x001F8008 File Offset: 0x001F6208
		[Nullable(2)]
		public NPlayerHand Hand
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06004139 RID: 16697 RVA: 0x001F8011 File Offset: 0x001F6211
		[Nullable(1)]
		public List<NSelectedHandCardHolder> Holders
		{
			[NullableContext(1)]
			get
			{
				return base.GetChildren(false).OfType<NSelectedHandCardHolder>().ToList<NSelectedHandCardHolder>();
			}
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x001F8024 File Offset: 0x001F6224
		public override void _Ready()
		{
			base.Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x001F8044 File Offset: 0x001F6244
		[NullableContext(1)]
		public NSelectedHandCardHolder Add(NHandCardHolder originalHolder)
		{
			NCard cardNode = originalHolder.CardNode;
			Vector2 globalPosition = cardNode.GlobalPosition;
			NSelectedHandCardHolder nselectedHandCardHolder = NSelectedHandCardHolder.Create(originalHolder);
			nselectedHandCardHolder.Connect(NCardHolder.SignalName.Pressed, Callable.From<NCardHolder>(new Action<NCardHolder>(this.DeselectHolder)), 4U);
			this.AddChildSafely(nselectedHandCardHolder);
			this.RefreshHolderPositions();
			cardNode.GlobalPosition = globalPosition;
			return nselectedHandCardHolder;
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x001F809C File Offset: 0x001F629C
		private void RefreshHolderPositions()
		{
			int count = this.Holders.Count;
			base.FocusMode = ((count > 0) ? 2L : 0L);
			if (count == 0)
			{
				return;
			}
			float x = this.Holders.First<NSelectedHandCardHolder>().Size.X;
			float num = -x * (float)(count - 1) / 2f;
			for (int i = 0; i < count; i++)
			{
				this.Holders[i].Position = new Vector2(num, 0f);
				num += x;
				this.Holders[i].FocusNeighborLeft = ((i > 0) ? this.Holders[i - 1].GetPath() : this.Holders[this.Holders.Count - 1].GetPath());
				this.Holders[i].FocusNeighborRight = ((i < this.Holders.Count - 1) ? this.Holders[i + 1].GetPath() : this.Holders[0].GetPath());
			}
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x001F81B0 File Offset: 0x001F63B0
		[NullableContext(1)]
		private void DeselectHolder(NCardHolder holder)
		{
			NSelectedHandCardHolder nselectedHandCardHolder = (NSelectedHandCardHolder)holder;
			NCard cardNode = nselectedHandCardHolder.CardNode;
			this.Hand.DeselectCard(cardNode);
			this.RemoveChildSafely(nselectedHandCardHolder);
			nselectedHandCardHolder.QueueFreeSafely();
			this.RefreshHolderPositions();
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x001F81EC File Offset: 0x001F63EC
		[NullableContext(1)]
		public void DeselectCard(CardModel card)
		{
			NSelectedHandCardHolder nselectedHandCardHolder = this.Holders.First<NSelectedHandCardHolder>((NSelectedHandCardHolder child) => child.CardNode.Model == card);
			this.DeselectHolder(nselectedHandCardHolder);
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x001F8225 File Offset: 0x001F6425
		private void OnFocus()
		{
			NSelectedHandCardHolder nselectedHandCardHolder = this.Holders.FirstOrDefault<NSelectedHandCardHolder>();
			if (nselectedHandCardHolder == null)
			{
				return;
			}
			nselectedHandCardHolder.TryGrabFocus();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004140 RID: 16704 RVA: 0x001F823C File Offset: 0x001F643C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NSelectedHandCardContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSelectedHandCardContainer.MethodName.Add, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "originalHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSelectedHandCardContainer.MethodName.RefreshHolderPositions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSelectedHandCardContainer.MethodName.DeselectHolder, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSelectedHandCardContainer.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004141 RID: 16705 RVA: 0x001F83A8 File Offset: 0x001F65A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSelectedHandCardContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSelectedHandCardContainer.MethodName.Add && args.Count == 1)
			{
				NSelectedHandCardHolder nselectedHandCardHolder = this.Add(VariantUtils.ConvertTo<NHandCardHolder>(args[0]));
				ret = VariantUtils.CreateFrom<NSelectedHandCardHolder>(ref nselectedHandCardHolder);
				return true;
			}
			if ((ref method) == NSelectedHandCardContainer.MethodName.RefreshHolderPositions && args.Count == 0)
			{
				this.RefreshHolderPositions();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSelectedHandCardContainer.MethodName.DeselectHolder && args.Count == 1)
			{
				this.DeselectHolder(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSelectedHandCardContainer.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06004142 RID: 16706 RVA: 0x001F849C File Offset: 0x001F669C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSelectedHandCardContainer.MethodName._Ready || (ref method) == NSelectedHandCardContainer.MethodName.Add || (ref method) == NSelectedHandCardContainer.MethodName.RefreshHolderPositions || (ref method) == NSelectedHandCardContainer.MethodName.DeselectHolder || (ref method) == NSelectedHandCardContainer.MethodName.OnFocus || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06004143 RID: 16707 RVA: 0x001F84FB File Offset: 0x001F66FB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSelectedHandCardContainer.PropertyName.Hand)
			{
				this.Hand = VariantUtils.ConvertTo<NPlayerHand>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06004144 RID: 16708 RVA: 0x001F8520 File Offset: 0x001F6720
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSelectedHandCardContainer.PropertyName.Hand)
			{
				NPlayerHand hand = this.Hand;
				value = VariantUtils.CreateFrom<NPlayerHand>(ref hand);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004145 RID: 16709 RVA: 0x001F8558 File Offset: 0x001F6758
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSelectedHandCardContainer.PropertyName.Hand, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004146 RID: 16710 RVA: 0x001F8590 File Offset: 0x001F6790
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName hand = NSelectedHandCardContainer.PropertyName.Hand;
			NPlayerHand hand2 = this.Hand;
			info.AddProperty(hand, Variant.From<NPlayerHand>(ref hand2));
		}

		/// <inheritdoc />
		// Token: 0x06004147 RID: 16711 RVA: 0x001F85C0 File Offset: 0x001F67C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSelectedHandCardContainer.PropertyName.Hand, ref variant))
			{
				this.Hand = variant.As<NPlayerHand>();
			}
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001920 RID: 6432
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040062E2 RID: 25314
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Add' method.
			/// </summary>
			// Token: 0x040062E3 RID: 25315
			public static readonly StringName Add = "Add";

			/// <summary>
			/// Cached name for the 'RefreshHolderPositions' method.
			/// </summary>
			// Token: 0x040062E4 RID: 25316
			public static readonly StringName RefreshHolderPositions = "RefreshHolderPositions";

			/// <summary>
			/// Cached name for the 'DeselectHolder' method.
			/// </summary>
			// Token: 0x040062E5 RID: 25317
			public static readonly StringName DeselectHolder = "DeselectHolder";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040062E6 RID: 25318
			public static readonly StringName OnFocus = "OnFocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001921 RID: 6433
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hand' property.
			/// </summary>
			// Token: 0x040062E7 RID: 25319
			public static readonly StringName Hand = "Hand";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001922 RID: 6434
		public class SignalName : Control.SignalName
		{
		}
	}
}

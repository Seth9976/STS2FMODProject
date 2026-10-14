using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection
{
	/// <summary>
	/// Base class for all grid-based card selection screens.
	/// </summary>
	// Token: 0x02000344 RID: 836
	[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NCardGridSelectionScreen.cs")]
	public abstract class NCardGridSelectionScreen : Control, IOverlayScreen, IScreenContext, ICardSelector
	{
		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06002F9A RID: 12186 RVA: 0x0015BC82 File Offset: 0x00159E82
		public NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.CardSelection;
			}
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x0015BC88 File Offset: 0x00159E88
		public override void _Ready()
		{
			if (base.GetType() != typeof(NCardGridSelectionScreen))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
			}
			this.ConnectSignalsAndInitGrid();
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x0015BCE0 File Offset: 0x00159EE0
		protected unsafe virtual void ConnectSignalsAndInitGrid()
		{
			this._grid = base.GetNode<NCardGrid>("%CardGrid");
			NCardGrid grid = this._grid;
			IReadOnlyList<CardModel> cards = this._cards;
			PileType pileType = PileType.None;
			int num = 1;
			List<SortingOrders> list = new List<SortingOrders>(num);
			CollectionsMarshal.SetCount<SortingOrders>(list, num);
			Span<SortingOrders> span = CollectionsMarshal.AsSpan<SortingOrders>(list);
			int num2 = 0;
			*span[num2] = SortingOrders.Ascending;
			grid.SetCards(cards, pileType, list, null);
			this._grid.Connect(NCardGrid.SignalName.HolderPressed, Callable.From<NCardHolder>(delegate(NCardHolder h)
			{
				this.OnCardClicked(h.CardModel);
			}), 0U);
			this._grid.Connect(NCardGrid.SignalName.HolderAltPressed, Callable.From<NCardHolder>(delegate(NCardHolder h)
			{
				this.ShowCardDetail(h.CardModel);
			}), 0U);
			this._grid.InsetForTopBar();
			this._peekButton = base.GetNode<NPeekButton>("%PeekButton");
			this._peekButton.Connect(NPeekButton.SignalName.Toggled, Callable.From<NPeekButton>(delegate(NPeekButton _)
			{
				if (this._peekButton.IsPeeking)
				{
					base.MouseFilter = 2L;
					return;
				}
				base.MouseFilter = 0L;
				ActiveScreenContext.Instance.Update();
			}), 0U);
			Callable.From(new Action(this.SetPeekButtonTargets)).CallDeferred(Array.Empty<Variant>());
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06002F9D RID: 12189
		[Nullable(1)]
		protected abstract IEnumerable<Control> PeekButtonTargets
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x06002F9E RID: 12190
		[NullableContext(1)]
		protected abstract void OnCardClicked(CardModel card);

		// Token: 0x06002F9F RID: 12191 RVA: 0x0015BDE0 File Offset: 0x00159FE0
		[NullableContext(1)]
		public async Task<IEnumerable<CardModel>> CardsSelected()
		{
			return await this._completionSource.Task;
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x0015BE23 File Offset: 0x0015A023
		public override void _ExitTree()
		{
			this._completionSource.TrySetCanceled();
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x0015BE34 File Offset: 0x0015A034
		private void SetPeekButtonTargets()
		{
			HashSet<Control> hashSet = new HashSet<Control> { this._grid };
			hashSet.UnionWith(this.PeekButtonTargets);
			this._peekButton.AddTargets(hashSet.ToArray<Control>());
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x0015BE71 File Offset: 0x0015A071
		public virtual void AfterOverlayOpened()
		{
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x0015BE73 File Offset: 0x0015A073
		public virtual void AfterOverlayClosed()
		{
			this._peekButton.SetPeeking(false);
			this.QueueFreeSafely();
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x0015BE87 File Offset: 0x0015A087
		public virtual void AfterOverlayShown()
		{
			base.Visible = true;
			if (CombatManager.Instance.IsInProgress)
			{
				this._peekButton.Enable();
			}
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x0015BEA7 File Offset: 0x0015A0A7
		public virtual void AfterOverlayHidden()
		{
			base.Visible = false;
			this._peekButton.Disable();
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06002FA6 RID: 12198 RVA: 0x0015BEBB File Offset: 0x0015A0BB
		public bool UseSharedBackstop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x0015BEBE File Offset: 0x0015A0BE
		[NullableContext(1)]
		private void ShowCardDetail(CardModel card)
		{
			if (NControllerManager.Instance.IsUsingController)
			{
				return;
			}
			NGame.Instance.GetInspectCardScreen().Open(this._cards.ToList<CardModel>(), this._cards.IndexOf(card), this._grid.IsShowingUpgrades);
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06002FA8 RID: 12200 RVA: 0x0015BEFE File Offset: 0x0015A0FE
		[Nullable(2)]
		public virtual Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._peekButton.IsPeeking)
				{
					return NCombatRoom.Instance.DefaultFocusedControl;
				}
				return this._grid.DefaultFocusedControl;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002FA9 RID: 12201 RVA: 0x0015BF23 File Offset: 0x0015A123
		[Nullable(2)]
		public virtual Control FocusedControlFromTopBar
		{
			[NullableContext(2)]
			get
			{
				if (this._peekButton.IsPeeking)
				{
					return NCombatRoom.Instance.FocusedControlFromTopBar;
				}
				return this._grid.FocusedControlFromTopBar;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002FAA RID: 12202 RVA: 0x0015BF48 File Offset: 0x0015A148
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NCardGridSelectionScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGridSelectionScreen.MethodName.ConnectSignalsAndInitGrid, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGridSelectionScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGridSelectionScreen.MethodName.SetPeekButtonTargets, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGridSelectionScreen.MethodName.AfterOverlayOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGridSelectionScreen.MethodName.AfterOverlayClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGridSelectionScreen.MethodName.AfterOverlayShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardGridSelectionScreen.MethodName.AfterOverlayHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002FAB RID: 12203 RVA: 0x0015C0D8 File Offset: 0x0015A2D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardGridSelectionScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGridSelectionScreen.MethodName.ConnectSignalsAndInitGrid && args.Count == 0)
			{
				this.ConnectSignalsAndInitGrid();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGridSelectionScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGridSelectionScreen.MethodName.SetPeekButtonTargets && args.Count == 0)
			{
				this.SetPeekButtonTargets();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGridSelectionScreen.MethodName.AfterOverlayOpened && args.Count == 0)
			{
				this.AfterOverlayOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGridSelectionScreen.MethodName.AfterOverlayClosed && args.Count == 0)
			{
				this.AfterOverlayClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGridSelectionScreen.MethodName.AfterOverlayShown && args.Count == 0)
			{
				this.AfterOverlayShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardGridSelectionScreen.MethodName.AfterOverlayHidden && args.Count == 0)
			{
				this.AfterOverlayHidden();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002FAC RID: 12204 RVA: 0x0015C218 File Offset: 0x0015A418
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardGridSelectionScreen.MethodName._Ready || (ref method) == NCardGridSelectionScreen.MethodName.ConnectSignalsAndInitGrid || (ref method) == NCardGridSelectionScreen.MethodName._ExitTree || (ref method) == NCardGridSelectionScreen.MethodName.SetPeekButtonTargets || (ref method) == NCardGridSelectionScreen.MethodName.AfterOverlayOpened || (ref method) == NCardGridSelectionScreen.MethodName.AfterOverlayClosed || (ref method) == NCardGridSelectionScreen.MethodName.AfterOverlayShown || (ref method) == NCardGridSelectionScreen.MethodName.AfterOverlayHidden || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002FAD RID: 12205 RVA: 0x0015C2A4 File Offset: 0x0015A4A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardGridSelectionScreen.PropertyName._grid)
			{
				this._grid = VariantUtils.ConvertTo<NCardGrid>(ref value);
				return true;
			}
			if ((ref name) == NCardGridSelectionScreen.PropertyName._peekButton)
			{
				this._peekButton = VariantUtils.ConvertTo<NPeekButton>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002FAE RID: 12206 RVA: 0x0015C2E4 File Offset: 0x0015A4E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardGridSelectionScreen.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NCardGridSelectionScreen.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NCardGridSelectionScreen.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCardGridSelectionScreen.PropertyName.FocusedControlFromTopBar)
			{
				Control control = this.FocusedControlFromTopBar;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCardGridSelectionScreen.PropertyName._grid)
			{
				value = VariantUtils.CreateFrom<NCardGrid>(ref this._grid);
				return true;
			}
			if ((ref name) == NCardGridSelectionScreen.PropertyName._peekButton)
			{
				value = VariantUtils.CreateFrom<NPeekButton>(ref this._peekButton);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002FAF RID: 12207 RVA: 0x0015C3C8 File Offset: 0x0015A5C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardGridSelectionScreen.PropertyName._grid, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardGridSelectionScreen.PropertyName._peekButton, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCardGridSelectionScreen.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardGridSelectionScreen.PropertyName.UseSharedBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardGridSelectionScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardGridSelectionScreen.PropertyName.FocusedControlFromTopBar, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002FB0 RID: 12208 RVA: 0x0015C4A0 File Offset: 0x0015A6A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardGridSelectionScreen.PropertyName._grid, Variant.From<NCardGrid>(ref this._grid));
			info.AddProperty(NCardGridSelectionScreen.PropertyName._peekButton, Variant.From<NPeekButton>(ref this._peekButton));
		}

		/// <inheritdoc />
		// Token: 0x06002FB1 RID: 12209 RVA: 0x0015C4D8 File Offset: 0x0015A6D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardGridSelectionScreen.PropertyName._grid, ref variant))
			{
				this._grid = variant.As<NCardGrid>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardGridSelectionScreen.PropertyName._peekButton, ref variant2))
			{
				this._peekButton = variant2.As<NPeekButton>();
			}
		}

		// Token: 0x0400124C RID: 4684
		[Nullable(1)]
		protected NCardGrid _grid;

		// Token: 0x0400124D RID: 4685
		[Nullable(1)]
		protected NPeekButton _peekButton;

		// Token: 0x0400124E RID: 4686
		[Nullable(1)]
		protected IReadOnlyList<CardModel> _cards;

		// Token: 0x0400124F RID: 4687
		[Nullable(1)]
		protected readonly TaskCompletionSource<IEnumerable<CardModel>> _completionSource = new TaskCompletionSource<IEnumerable<CardModel>>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015F1 RID: 5617
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040054FF RID: 21759
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignalsAndInitGrid' method.
			/// </summary>
			// Token: 0x04005500 RID: 21760
			public static readonly StringName ConnectSignalsAndInitGrid = "ConnectSignalsAndInitGrid";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005501 RID: 21761
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'SetPeekButtonTargets' method.
			/// </summary>
			// Token: 0x04005502 RID: 21762
			public static readonly StringName SetPeekButtonTargets = "SetPeekButtonTargets";

			/// <summary>
			/// Cached name for the 'AfterOverlayOpened' method.
			/// </summary>
			// Token: 0x04005503 RID: 21763
			public static readonly StringName AfterOverlayOpened = "AfterOverlayOpened";

			/// <summary>
			/// Cached name for the 'AfterOverlayClosed' method.
			/// </summary>
			// Token: 0x04005504 RID: 21764
			public static readonly StringName AfterOverlayClosed = "AfterOverlayClosed";

			/// <summary>
			/// Cached name for the 'AfterOverlayShown' method.
			/// </summary>
			// Token: 0x04005505 RID: 21765
			public static readonly StringName AfterOverlayShown = "AfterOverlayShown";

			/// <summary>
			/// Cached name for the 'AfterOverlayHidden' method.
			/// </summary>
			// Token: 0x04005506 RID: 21766
			public static readonly StringName AfterOverlayHidden = "AfterOverlayHidden";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015F2 RID: 5618
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x04005507 RID: 21767
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x04005508 RID: 21768
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005509 RID: 21769
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the 'FocusedControlFromTopBar' property.
			/// </summary>
			// Token: 0x0400550A RID: 21770
			public static readonly StringName FocusedControlFromTopBar = "FocusedControlFromTopBar";

			/// <summary>
			/// Cached name for the '_grid' field.
			/// </summary>
			// Token: 0x0400550B RID: 21771
			public static readonly StringName _grid = "_grid";

			/// <summary>
			/// Cached name for the '_peekButton' field.
			/// </summary>
			// Token: 0x0400550C RID: 21772
			public static readonly StringName _peekButton = "_peekButton";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020015F3 RID: 5619
		public class SignalName : Control.SignalName
		{
		}
	}
}

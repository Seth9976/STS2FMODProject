using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens
{
	/// <summary>
	/// This is the screen used to view an arbitrary set of cards
	/// EX: pandoras box
	/// </summary>
	// Token: 0x0200024B RID: 587
	[ScriptPath("res://src/Core/Nodes/Screens/NCardsViewScreen.cs")]
	public abstract class NCardsViewScreen : Control, ICapstoneScreen, IScreenContext
	{
		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001E0E RID: 7694
		public abstract NetScreenType ScreenType { get; }

		// Token: 0x06001E0F RID: 7695 RVA: 0x000C3C14 File Offset: 0x000C1E14
		public override void _Ready()
		{
			if (base.GetType() != typeof(NCardsViewScreen))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x000C3C6C File Offset: 0x000C1E6C
		protected virtual void ConnectSignals()
		{
			this._bottomLabel = base.GetNode<RichTextLabel>("%BottomLabel");
			this._backButton = base.GetNode<NButton>("BackButton");
			this._backButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnReturnButtonPressed)), 0U);
			this._backButton.Enable();
			this._grid = base.GetNode<NCardGrid>("CardGrid");
			this._grid.InsetForTopBar();
			this._bottomLabel.Text = this._infoText.GetFormattedText();
			this._showUpgrades = base.GetNode<NTickbox>("%Upgrades");
			this._showUpgrades.Connect(NTickbox.SignalName.Toggled, Callable.From<NTickbox>(new Action<NTickbox>(this.ToggleShowUpgrades)), 0U);
			this.OnControllerStateUpdated();
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.OnControllerStateUpdated)), 0U);
			base.ProcessMode = (base.Visible ? 0L : 4L);
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x000C3DA4 File Offset: 0x000C1FA4
		[NullableContext(1)]
		protected void ShowCardDetail(CardModel cardModel)
		{
			this._backButton.Disable();
			List<CardModel> list = this._grid.CurrentlyDisplayedCards.ToList<CardModel>();
			NInspectCardScreen inspectCardScreen = NGame.Instance.GetInspectCardScreen();
			inspectCardScreen.Open(list, list.IndexOf(cardModel), this._grid.IsShowingUpgrades);
			inspectCardScreen.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(delegate
			{
				if (!inspectCardScreen.Visible)
				{
					this.OnInspectCardHidden();
				}
			}), 4U);
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x000C3E2B File Offset: 0x000C202B
		protected virtual void OnInspectCardHidden()
		{
			this._backButton.Enable();
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x000C3E38 File Offset: 0x000C2038
		[NullableContext(1)]
		private void ToggleShowUpgrades(NTickbox tickbox)
		{
			this._grid.IsShowingUpgrades = tickbox.IsTicked;
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x000C3E4B File Offset: 0x000C204B
		[NullableContext(1)]
		protected void OnReturnButtonPressed(NButton _)
		{
			NCapstoneContainer.Instance.Close();
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x000C3E57 File Offset: 0x000C2057
		public virtual void AfterCapstoneOpened()
		{
			this._showUpgrades.IsTicked = false;
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x000C3E65 File Offset: 0x000C2065
		public virtual void AfterCapstoneClosed()
		{
			base.Visible = false;
			this.QueueFreeSafely();
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001E17 RID: 7703 RVA: 0x000C3E74 File Offset: 0x000C2074
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this._grid.DefaultFocusedControl;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001E18 RID: 7704 RVA: 0x000C3E81 File Offset: 0x000C2081
		[Nullable(2)]
		public Control FocusedControlFromTopBar
		{
			[NullableContext(2)]
			get
			{
				return this._grid.FocusedControlFromTopBar;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001E19 RID: 7705 RVA: 0x000C3E8E File Offset: 0x000C208E
		public bool UseSharedBackstop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x000C3E91 File Offset: 0x000C2091
		private void OnControllerStateUpdated()
		{
			this._showUpgrades.Visible = !NControllerManager.Instance.IsUsingController;
			if (NControllerManager.Instance.IsUsingController)
			{
				this._showUpgrades.IsTicked = false;
				this.ToggleShowUpgrades(this._showUpgrades);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E1B RID: 7707 RVA: 0x000C3ED0 File Offset: 0x000C20D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NCardsViewScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardsViewScreen.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardsViewScreen.MethodName.OnInspectCardHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardsViewScreen.MethodName.ToggleShowUpgrades, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tickbox", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardsViewScreen.MethodName.OnReturnButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardsViewScreen.MethodName.AfterCapstoneOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardsViewScreen.MethodName.AfterCapstoneClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardsViewScreen.MethodName.OnControllerStateUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001E1C RID: 7708 RVA: 0x000C40C0 File Offset: 0x000C22C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardsViewScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardsViewScreen.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardsViewScreen.MethodName.OnInspectCardHidden && args.Count == 0)
			{
				this.OnInspectCardHidden();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardsViewScreen.MethodName.ToggleShowUpgrades && args.Count == 1)
			{
				this.ToggleShowUpgrades(VariantUtils.ConvertTo<NTickbox>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardsViewScreen.MethodName.OnReturnButtonPressed && args.Count == 1)
			{
				this.OnReturnButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardsViewScreen.MethodName.AfterCapstoneOpened && args.Count == 0)
			{
				this.AfterCapstoneOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardsViewScreen.MethodName.AfterCapstoneClosed && args.Count == 0)
			{
				this.AfterCapstoneClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardsViewScreen.MethodName.OnControllerStateUpdated && args.Count == 0)
			{
				this.OnControllerStateUpdated();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001E1D RID: 7709 RVA: 0x000C421C File Offset: 0x000C241C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardsViewScreen.MethodName._Ready || (ref method) == NCardsViewScreen.MethodName.ConnectSignals || (ref method) == NCardsViewScreen.MethodName.OnInspectCardHidden || (ref method) == NCardsViewScreen.MethodName.ToggleShowUpgrades || (ref method) == NCardsViewScreen.MethodName.OnReturnButtonPressed || (ref method) == NCardsViewScreen.MethodName.AfterCapstoneOpened || (ref method) == NCardsViewScreen.MethodName.AfterCapstoneClosed || (ref method) == NCardsViewScreen.MethodName.OnControllerStateUpdated || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001E1E RID: 7710 RVA: 0x000C42A8 File Offset: 0x000C24A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardsViewScreen.PropertyName._background)
			{
				this._background = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			if ((ref name) == NCardsViewScreen.PropertyName._grid)
			{
				this._grid = VariantUtils.ConvertTo<NCardGrid>(ref value);
				return true;
			}
			if ((ref name) == NCardsViewScreen.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NButton>(ref value);
				return true;
			}
			if ((ref name) == NCardsViewScreen.PropertyName._showUpgrades)
			{
				this._showUpgrades = VariantUtils.ConvertTo<NTickbox>(ref value);
				return true;
			}
			if ((ref name) == NCardsViewScreen.PropertyName._bottomLabel)
			{
				this._bottomLabel = VariantUtils.ConvertTo<RichTextLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001E1F RID: 7711 RVA: 0x000C4344 File Offset: 0x000C2544
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardsViewScreen.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NCardsViewScreen.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCardsViewScreen.PropertyName.FocusedControlFromTopBar)
			{
				Control control = this.FocusedControlFromTopBar;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NCardsViewScreen.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NCardsViewScreen.PropertyName._background)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._background);
				return true;
			}
			if ((ref name) == NCardsViewScreen.PropertyName._grid)
			{
				value = VariantUtils.CreateFrom<NCardGrid>(ref this._grid);
				return true;
			}
			if ((ref name) == NCardsViewScreen.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NCardsViewScreen.PropertyName._showUpgrades)
			{
				value = VariantUtils.CreateFrom<NTickbox>(ref this._showUpgrades);
				return true;
			}
			if ((ref name) == NCardsViewScreen.PropertyName._bottomLabel)
			{
				value = VariantUtils.CreateFrom<RichTextLabel>(ref this._bottomLabel);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001E20 RID: 7712 RVA: 0x000C4488 File Offset: 0x000C2688
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardsViewScreen.PropertyName._background, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardsViewScreen.PropertyName._grid, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardsViewScreen.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardsViewScreen.PropertyName._showUpgrades, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardsViewScreen.PropertyName._bottomLabel, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCardsViewScreen.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardsViewScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardsViewScreen.PropertyName.FocusedControlFromTopBar, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardsViewScreen.PropertyName.UseSharedBackstop, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001E21 RID: 7713 RVA: 0x000C45C4 File Offset: 0x000C27C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCardsViewScreen.PropertyName._background, Variant.From<ColorRect>(ref this._background));
			info.AddProperty(NCardsViewScreen.PropertyName._grid, Variant.From<NCardGrid>(ref this._grid));
			info.AddProperty(NCardsViewScreen.PropertyName._backButton, Variant.From<NButton>(ref this._backButton));
			info.AddProperty(NCardsViewScreen.PropertyName._showUpgrades, Variant.From<NTickbox>(ref this._showUpgrades));
			info.AddProperty(NCardsViewScreen.PropertyName._bottomLabel, Variant.From<RichTextLabel>(ref this._bottomLabel));
		}

		/// <inheritdoc />
		// Token: 0x06001E22 RID: 7714 RVA: 0x000C4648 File Offset: 0x000C2848
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardsViewScreen.PropertyName._background, ref variant))
			{
				this._background = variant.As<ColorRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardsViewScreen.PropertyName._grid, ref variant2))
			{
				this._grid = variant2.As<NCardGrid>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardsViewScreen.PropertyName._backButton, ref variant3))
			{
				this._backButton = variant3.As<NButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardsViewScreen.PropertyName._showUpgrades, ref variant4))
			{
				this._showUpgrades = variant4.As<NTickbox>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardsViewScreen.PropertyName._bottomLabel, ref variant5))
			{
				this._bottomLabel = variant5.As<RichTextLabel>();
			}
		}

		// Token: 0x04000B91 RID: 2961
		[Nullable(1)]
		private ColorRect _background;

		// Token: 0x04000B92 RID: 2962
		[Nullable(1)]
		protected NCardGrid _grid;

		// Token: 0x04000B93 RID: 2963
		[Nullable(1)]
		protected NButton _backButton;

		// Token: 0x04000B94 RID: 2964
		[Nullable(1)]
		private NTickbox _showUpgrades;

		// Token: 0x04000B95 RID: 2965
		[Nullable(1)]
		private RichTextLabel _bottomLabel;

		// Token: 0x04000B96 RID: 2966
		[Nullable(1)]
		protected List<CardModel> _cards;

		// Token: 0x04000B97 RID: 2967
		[Nullable(1)]
		protected LocString _infoText;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200125A RID: 4698
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400470A RID: 18186
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x0400470B RID: 18187
			public static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'OnInspectCardHidden' method.
			/// </summary>
			// Token: 0x0400470C RID: 18188
			public static readonly StringName OnInspectCardHidden = "OnInspectCardHidden";

			/// <summary>
			/// Cached name for the 'ToggleShowUpgrades' method.
			/// </summary>
			// Token: 0x0400470D RID: 18189
			public static readonly StringName ToggleShowUpgrades = "ToggleShowUpgrades";

			/// <summary>
			/// Cached name for the 'OnReturnButtonPressed' method.
			/// </summary>
			// Token: 0x0400470E RID: 18190
			public static readonly StringName OnReturnButtonPressed = "OnReturnButtonPressed";

			/// <summary>
			/// Cached name for the 'AfterCapstoneOpened' method.
			/// </summary>
			// Token: 0x0400470F RID: 18191
			public static readonly StringName AfterCapstoneOpened = "AfterCapstoneOpened";

			/// <summary>
			/// Cached name for the 'AfterCapstoneClosed' method.
			/// </summary>
			// Token: 0x04004710 RID: 18192
			public static readonly StringName AfterCapstoneClosed = "AfterCapstoneClosed";

			/// <summary>
			/// Cached name for the 'OnControllerStateUpdated' method.
			/// </summary>
			// Token: 0x04004711 RID: 18193
			public static readonly StringName OnControllerStateUpdated = "OnControllerStateUpdated";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200125B RID: 4699
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x04004712 RID: 18194
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004713 RID: 18195
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the 'FocusedControlFromTopBar' property.
			/// </summary>
			// Token: 0x04004714 RID: 18196
			public static readonly StringName FocusedControlFromTopBar = "FocusedControlFromTopBar";

			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x04004715 RID: 18197
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the '_background' field.
			/// </summary>
			// Token: 0x04004716 RID: 18198
			public static readonly StringName _background = "_background";

			/// <summary>
			/// Cached name for the '_grid' field.
			/// </summary>
			// Token: 0x04004717 RID: 18199
			public static readonly StringName _grid = "_grid";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x04004718 RID: 18200
			public static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_showUpgrades' field.
			/// </summary>
			// Token: 0x04004719 RID: 18201
			public static readonly StringName _showUpgrades = "_showUpgrades";

			/// <summary>
			/// Cached name for the '_bottomLabel' field.
			/// </summary>
			// Token: 0x0400471A RID: 18202
			public static readonly StringName _bottomLabel = "_bottomLabel";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200125C RID: 4700
		public class SignalName : Control.SignalName
		{
		}
	}
}

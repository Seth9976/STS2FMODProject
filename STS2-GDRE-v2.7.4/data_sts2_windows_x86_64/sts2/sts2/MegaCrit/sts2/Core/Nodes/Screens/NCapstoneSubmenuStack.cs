using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Screens
{
	/// <summary>
	/// Controls submenu stacks that show up as a capstone (compendium, settings).
	/// Allows submenus to stack on top of each other and act as one big capstone. They are all dismissed if another
	/// capstone shows up.
	/// </summary>
	// Token: 0x02000249 RID: 585
	[ScriptPath("res://src/Core/Nodes/Screens/NCapstoneSubmenuStack.cs")]
	public class NCapstoneSubmenuStack : Control, ICapstoneScreen, IScreenContext
	{
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001DDE RID: 7646 RVA: 0x000C2781 File Offset: 0x000C0981
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("screens/capstone_submenu_stack");
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001DDF RID: 7647 RVA: 0x000C278D File Offset: 0x000C098D
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCapstoneSubmenuStack.ScenePath);
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x000C2799 File Offset: 0x000C0999
		// (set) Token: 0x06001DE1 RID: 7649 RVA: 0x000C27A1 File Offset: 0x000C09A1
		public CapstoneSubmenuType Type { get; private set; }

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x000C27AA File Offset: 0x000C09AA
		// (set) Token: 0x06001DE3 RID: 7651 RVA: 0x000C27B2 File Offset: 0x000C09B2
		[Nullable(1)]
		public NRunSubmenuStack Stack
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			private set;
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001DE4 RID: 7652 RVA: 0x000C27BB File Offset: 0x000C09BB
		public NetScreenType ScreenType
		{
			get
			{
				return this.GetCapstoneSubmenuType();
			}
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x000C27C4 File Offset: 0x000C09C4
		[NullableContext(1)]
		public NSubmenu ShowScreen(CapstoneSubmenuType type)
		{
			while (this.Stack.Peek() != null)
			{
				this.Stack.Pop();
			}
			Type type2;
			switch (type)
			{
			case CapstoneSubmenuType.Settings:
				type2 = typeof(NSettingsScreen);
				break;
			case CapstoneSubmenuType.Compendium:
				type2 = typeof(NCompendiumSubmenu);
				break;
			case CapstoneSubmenuType.Feedback:
				type2 = typeof(NSendFeedbackScreen);
				break;
			case CapstoneSubmenuType.PauseMenu:
				type2 = typeof(NPauseMenu);
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			Type type3 = type2;
			NSubmenu nsubmenu = this.Stack.PushSubmenuType(type3);
			this.Type = type;
			NCapstoneContainer.Instance.Open(this);
			return nsubmenu;
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x000C2870 File Offset: 0x000C0A70
		private NetScreenType GetCapstoneSubmenuType()
		{
			NetScreenType netScreenType;
			switch (this.Type)
			{
			case CapstoneSubmenuType.Settings:
				netScreenType = NetScreenType.Settings;
				break;
			case CapstoneSubmenuType.Compendium:
				netScreenType = NetScreenType.Compendium;
				break;
			case CapstoneSubmenuType.Feedback:
				netScreenType = NetScreenType.Feedback;
				break;
			case CapstoneSubmenuType.PauseMenu:
				netScreenType = NetScreenType.PauseMenu;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return netScreenType;
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x000C28B7 File Offset: 0x000C0AB7
		public override void _Ready()
		{
			this.Stack = base.GetNode<NRunSubmenuStack>("%Submenus");
			this.Stack.Connect(NSubmenuStack.SignalName.StackModified, Callable.From(new Action(this.OnSubmenuStackChanged)), 0U);
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x000C28F2 File Offset: 0x000C0AF2
		private void OnSubmenuStackChanged()
		{
			if (this.Stack.Peek() == null && NCapstoneContainer.Instance.CurrentCapstoneScreen == this)
			{
				NCapstoneContainer.Instance.Close();
			}
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x000C2918 File Offset: 0x000C0B18
		public void AfterCapstoneOpened()
		{
			NGlobalUi globalUi = NRun.Instance.GlobalUi;
			globalUi.TopBar.AnimHide();
			globalUi.RelicInventory.AnimHide();
			globalUi.MultiplayerPlayerContainer.AnimHide();
			SfxCmd.Play("event:/sfx/ui/pause_open", 1f);
			globalUi.MoveChildSafely(globalUi.CardPreviewContainer, globalUi.CapstoneContainer.GetIndex(false));
			globalUi.MoveChildSafely(globalUi.GridCardPreviewContainer, globalUi.CapstoneContainer.GetIndex(false));
			globalUi.MoveChildSafely(globalUi.EventCardPreviewContainer, globalUi.CapstoneContainer.GetIndex(false));
			globalUi.MoveChildSafely(globalUi.MessyCardPreviewContainer, globalUi.CapstoneContainer.GetIndex(false));
			globalUi.MoveChildSafely(globalUi.AboveTopBarVfxContainer, globalUi.CapstoneContainer.GetIndex(false));
			base.Visible = true;
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x000C29E0 File Offset: 0x000C0BE0
		public void AfterCapstoneClosed()
		{
			while (this.Stack.Peek() != null)
			{
				this.Stack.Pop();
			}
			SfxCmd.Play("event:/sfx/ui/pause_close", 1f);
			NGlobalUi globalUi = NRun.Instance.GlobalUi;
			globalUi.TopBar.AnimShow();
			globalUi.RelicInventory.AnimShow();
			globalUi.MultiplayerPlayerContainer.AnimShow();
			globalUi.MoveChildSafely(globalUi.AboveTopBarVfxContainer, globalUi.TopBar.GetIndex(false) + 1);
			globalUi.MoveChildSafely(globalUi.MessyCardPreviewContainer, globalUi.TopBar.GetIndex(false) + 1);
			globalUi.MoveChildSafely(globalUi.EventCardPreviewContainer, globalUi.TopBar.GetIndex(false) + 1);
			globalUi.MoveChildSafely(globalUi.GridCardPreviewContainer, globalUi.TopBar.GetIndex(false) + 1);
			globalUi.MoveChildSafely(globalUi.CardPreviewContainer, globalUi.TopBar.GetIndex(false) + 1);
			base.Visible = false;
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001DEB RID: 7659 RVA: 0x000C2ACB File Offset: 0x000C0CCB
		public bool UseSharedBackstop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x000C2ACE File Offset: 0x000C0CCE
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				NSubmenu nsubmenu = this.Stack.Peek();
				if (nsubmenu == null)
				{
					return null;
				}
				return nsubmenu.DefaultFocusedControl;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001DED RID: 7661 RVA: 0x000C2AE8 File Offset: 0x000C0CE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NCapstoneSubmenuStack.MethodName.ShowScreen, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "type", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCapstoneSubmenuStack.MethodName.GetCapstoneSubmenuType, new PropertyInfo(2L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneSubmenuStack.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneSubmenuStack.MethodName.OnSubmenuStackChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneSubmenuStack.MethodName.AfterCapstoneOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCapstoneSubmenuStack.MethodName.AfterCapstoneClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001DEE RID: 7662 RVA: 0x000C2C48 File Offset: 0x000C0E48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCapstoneSubmenuStack.MethodName.ShowScreen && args.Count == 1)
			{
				NSubmenu nsubmenu = this.ShowScreen(VariantUtils.ConvertTo<CapstoneSubmenuType>(args[0]));
				ret = VariantUtils.CreateFrom<NSubmenu>(ref nsubmenu);
				return true;
			}
			if ((ref method) == NCapstoneSubmenuStack.MethodName.GetCapstoneSubmenuType && args.Count == 0)
			{
				NetScreenType capstoneSubmenuType = this.GetCapstoneSubmenuType();
				ret = VariantUtils.CreateFrom<NetScreenType>(ref capstoneSubmenuType);
				return true;
			}
			if ((ref method) == NCapstoneSubmenuStack.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCapstoneSubmenuStack.MethodName.OnSubmenuStackChanged && args.Count == 0)
			{
				this.OnSubmenuStackChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCapstoneSubmenuStack.MethodName.AfterCapstoneOpened && args.Count == 0)
			{
				this.AfterCapstoneOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCapstoneSubmenuStack.MethodName.AfterCapstoneClosed && args.Count == 0)
			{
				this.AfterCapstoneClosed();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001DEF RID: 7663 RVA: 0x000C2D58 File Offset: 0x000C0F58
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCapstoneSubmenuStack.MethodName.ShowScreen || (ref method) == NCapstoneSubmenuStack.MethodName.GetCapstoneSubmenuType || (ref method) == NCapstoneSubmenuStack.MethodName._Ready || (ref method) == NCapstoneSubmenuStack.MethodName.OnSubmenuStackChanged || (ref method) == NCapstoneSubmenuStack.MethodName.AfterCapstoneOpened || (ref method) == NCapstoneSubmenuStack.MethodName.AfterCapstoneClosed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001DF0 RID: 7664 RVA: 0x000C2DC6 File Offset: 0x000C0FC6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCapstoneSubmenuStack.PropertyName.Type)
			{
				this.Type = VariantUtils.ConvertTo<CapstoneSubmenuType>(ref value);
				return true;
			}
			if ((ref name) == NCapstoneSubmenuStack.PropertyName.Stack)
			{
				this.Stack = VariantUtils.ConvertTo<NRunSubmenuStack>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001DF1 RID: 7665 RVA: 0x000C2E08 File Offset: 0x000C1008
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCapstoneSubmenuStack.PropertyName.Type)
			{
				CapstoneSubmenuType type = this.Type;
				value = VariantUtils.CreateFrom<CapstoneSubmenuType>(ref type);
				return true;
			}
			if ((ref name) == NCapstoneSubmenuStack.PropertyName.Stack)
			{
				NRunSubmenuStack stack = this.Stack;
				value = VariantUtils.CreateFrom<NRunSubmenuStack>(ref stack);
				return true;
			}
			if ((ref name) == NCapstoneSubmenuStack.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NCapstoneSubmenuStack.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NCapstoneSubmenuStack.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001DF2 RID: 7666 RVA: 0x000C2ED0 File Offset: 0x000C10D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NCapstoneSubmenuStack.PropertyName.Type, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCapstoneSubmenuStack.PropertyName.Stack, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCapstoneSubmenuStack.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCapstoneSubmenuStack.PropertyName.UseSharedBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCapstoneSubmenuStack.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001DF3 RID: 7667 RVA: 0x000C2F88 File Offset: 0x000C1188
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName type = NCapstoneSubmenuStack.PropertyName.Type;
			CapstoneSubmenuType type2 = this.Type;
			info.AddProperty(type, Variant.From<CapstoneSubmenuType>(ref type2));
			StringName stack = NCapstoneSubmenuStack.PropertyName.Stack;
			NRunSubmenuStack stack2 = this.Stack;
			info.AddProperty(stack, Variant.From<NRunSubmenuStack>(ref stack2));
		}

		/// <inheritdoc />
		// Token: 0x06001DF4 RID: 7668 RVA: 0x000C2FD0 File Offset: 0x000C11D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCapstoneSubmenuStack.PropertyName.Type, ref variant))
			{
				this.Type = variant.As<CapstoneSubmenuType>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCapstoneSubmenuStack.PropertyName.Stack, ref variant2))
			{
				this.Stack = variant2.As<NRunSubmenuStack>();
			}
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001253 RID: 4691
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'ShowScreen' method.
			/// </summary>
			// Token: 0x040046ED RID: 18157
			public static readonly StringName ShowScreen = "ShowScreen";

			/// <summary>
			/// Cached name for the 'GetCapstoneSubmenuType' method.
			/// </summary>
			// Token: 0x040046EE RID: 18158
			public static readonly StringName GetCapstoneSubmenuType = "GetCapstoneSubmenuType";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040046EF RID: 18159
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnSubmenuStackChanged' method.
			/// </summary>
			// Token: 0x040046F0 RID: 18160
			public static readonly StringName OnSubmenuStackChanged = "OnSubmenuStackChanged";

			/// <summary>
			/// Cached name for the 'AfterCapstoneOpened' method.
			/// </summary>
			// Token: 0x040046F1 RID: 18161
			public static readonly StringName AfterCapstoneOpened = "AfterCapstoneOpened";

			/// <summary>
			/// Cached name for the 'AfterCapstoneClosed' method.
			/// </summary>
			// Token: 0x040046F2 RID: 18162
			public static readonly StringName AfterCapstoneClosed = "AfterCapstoneClosed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001254 RID: 4692
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Type' property.
			/// </summary>
			// Token: 0x040046F3 RID: 18163
			public static readonly StringName Type = "Type";

			/// <summary>
			/// Cached name for the 'Stack' property.
			/// </summary>
			// Token: 0x040046F4 RID: 18164
			public static readonly StringName Stack = "Stack";

			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x040046F5 RID: 18165
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x040046F6 RID: 18166
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040046F7 RID: 18167
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001255 RID: 4693
		public class SignalName : Control.SignalName
		{
		}
	}
}

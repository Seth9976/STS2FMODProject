using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x02000313 RID: 787
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NSubmenu.cs")]
	public abstract class NSubmenu : Control, IScreenContext
	{
		// Token: 0x06002B9C RID: 11164 RVA: 0x00134620 File Offset: 0x00132820
		public override void _Ready()
		{
			if (base.GetType() != typeof(NSubmenu))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Type>(base.GetType());
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
			}
			this.ConnectSignals();
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x00134678 File Offset: 0x00132878
		protected virtual void ConnectSignals()
		{
			this._backButton = base.GetNode<NBackButton>("BackButton");
			this._backButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(delegate(NButton _)
			{
				this._stack.Pop();
			}), 0U);
			this._backButton.Disable();
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnScreenVisibilityChange)), 0U);
		}

		/// <summary>
		/// Used to override submenu back button behaviors (see: Timeline)
		/// </summary>
		// Token: 0x06002B9E RID: 11166 RVA: 0x001346E7 File Offset: 0x001328E7
		public void HideBackButtonImmediately()
		{
			this._backButton.Disable();
			this._backButton.MoveToHidePosition();
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x001346FF File Offset: 0x001328FF
		[NullableContext(1)]
		public void SetStack(NSubmenuStack stack)
		{
			this._stack = stack;
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x00134708 File Offset: 0x00132908
		private void OnScreenVisibilityChange()
		{
			if (base.Visible)
			{
				this._backButton.MoveToHidePosition();
				this._backButton.Enable();
				this.OnSubmenuShown();
				return;
			}
			Viewport viewport = base.GetViewport();
			this._lastFocusedControl = ((viewport != null) ? viewport.GuiGetFocusOwner() : null);
			this._backButton.Disable();
			this.OnSubmenuHidden();
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002BA1 RID: 11169 RVA: 0x00134763 File Offset: 0x00132963
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this._lastFocusedControl ?? this.InitialFocusedControl;
			}
		}

		/// <summary>
		/// The initial control the submenu is focused on when it is first opened
		/// </summary>
		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002BA2 RID: 11170
		[Nullable(2)]
		protected abstract Control InitialFocusedControl
		{
			[NullableContext(2)]
			get;
		}

		/// <summary>
		/// Called when this submenu is newly pushed onto the stack OR when it is re-shown because the submenu above it has
		/// been popped from the stack.
		/// </summary>
		// Token: 0x06002BA3 RID: 11171 RVA: 0x00134775 File Offset: 0x00132975
		protected virtual void OnSubmenuShown()
		{
		}

		/// <summary>
		/// Called when this submenu has just been popped from the stack OR when it is hidden because a new submenu has just
		/// been pushed onto the stack.
		/// </summary>
		// Token: 0x06002BA4 RID: 11172 RVA: 0x00134777 File Offset: 0x00132977
		protected virtual void OnSubmenuHidden()
		{
		}

		/// <summary>
		/// Called only when this submenu is newly pushed on to a stack.
		/// </summary>
		// Token: 0x06002BA5 RID: 11173 RVA: 0x00134779 File Offset: 0x00132979
		public virtual void OnSubmenuOpened()
		{
		}

		/// <summary>
		/// Called only when this submenu is popped from the stack it was on.
		/// </summary>
		// Token: 0x06002BA6 RID: 11174 RVA: 0x0013477B File Offset: 0x0013297B
		public virtual void OnSubmenuClosed()
		{
			this._lastFocusedControl = null;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002BA7 RID: 11175 RVA: 0x00134784 File Offset: 0x00132984
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NSubmenu.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenu.MethodName.ConnectSignals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenu.MethodName.HideBackButtonImmediately, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenu.MethodName.SetStack, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "stack", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NSubmenu.MethodName.OnScreenVisibilityChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenu.MethodName.OnSubmenuShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenu.MethodName.OnSubmenuHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenu.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSubmenu.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002BA8 RID: 11176 RVA: 0x00134974 File Offset: 0x00132B74
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSubmenu.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenu.MethodName.ConnectSignals && args.Count == 0)
			{
				this.ConnectSignals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenu.MethodName.HideBackButtonImmediately && args.Count == 0)
			{
				this.HideBackButtonImmediately();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenu.MethodName.SetStack && args.Count == 1)
			{
				this.SetStack(VariantUtils.ConvertTo<NSubmenuStack>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenu.MethodName.OnScreenVisibilityChange && args.Count == 0)
			{
				this.OnScreenVisibilityChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenu.MethodName.OnSubmenuShown && args.Count == 0)
			{
				this.OnSubmenuShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenu.MethodName.OnSubmenuHidden && args.Count == 0)
			{
				this.OnSubmenuHidden();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenu.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSubmenu.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002BA9 RID: 11177 RVA: 0x00134AE8 File Offset: 0x00132CE8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSubmenu.MethodName._Ready || (ref method) == NSubmenu.MethodName.ConnectSignals || (ref method) == NSubmenu.MethodName.HideBackButtonImmediately || (ref method) == NSubmenu.MethodName.SetStack || (ref method) == NSubmenu.MethodName.OnScreenVisibilityChange || (ref method) == NSubmenu.MethodName.OnSubmenuShown || (ref method) == NSubmenu.MethodName.OnSubmenuHidden || (ref method) == NSubmenu.MethodName.OnSubmenuOpened || (ref method) == NSubmenu.MethodName.OnSubmenuClosed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002BAA RID: 11178 RVA: 0x00134B84 File Offset: 0x00132D84
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSubmenu.PropertyName._backButton)
			{
				this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
				return true;
			}
			if ((ref name) == NSubmenu.PropertyName._stack)
			{
				this._stack = VariantUtils.ConvertTo<NSubmenuStack>(ref value);
				return true;
			}
			if ((ref name) == NSubmenu.PropertyName._lastFocusedControl)
			{
				this._lastFocusedControl = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002BAB RID: 11179 RVA: 0x00134BEC File Offset: 0x00132DEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSubmenu.PropertyName.DefaultFocusedControl)
			{
				Control control = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NSubmenu.PropertyName.InitialFocusedControl)
			{
				Control control = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref control);
				return true;
			}
			if ((ref name) == NSubmenu.PropertyName._backButton)
			{
				value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
				return true;
			}
			if ((ref name) == NSubmenu.PropertyName._stack)
			{
				value = VariantUtils.CreateFrom<NSubmenuStack>(ref this._stack);
				return true;
			}
			if ((ref name) == NSubmenu.PropertyName._lastFocusedControl)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._lastFocusedControl);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002BAC RID: 11180 RVA: 0x00134CA8 File Offset: 0x00132EA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSubmenu.PropertyName._backButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSubmenu.PropertyName._stack, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSubmenu.PropertyName._lastFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSubmenu.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSubmenu.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002BAD RID: 11181 RVA: 0x00134D64 File Offset: 0x00132F64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSubmenu.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
			info.AddProperty(NSubmenu.PropertyName._stack, Variant.From<NSubmenuStack>(ref this._stack));
			info.AddProperty(NSubmenu.PropertyName._lastFocusedControl, Variant.From<Control>(ref this._lastFocusedControl));
		}

		/// <inheritdoc />
		// Token: 0x06002BAE RID: 11182 RVA: 0x00134DBC File Offset: 0x00132FBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSubmenu.PropertyName._backButton, ref variant))
			{
				this._backButton = variant.As<NBackButton>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSubmenu.PropertyName._stack, ref variant2))
			{
				this._stack = variant2.As<NSubmenuStack>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSubmenu.PropertyName._lastFocusedControl, ref variant3))
			{
				this._lastFocusedControl = variant3.As<Control>();
			}
		}

		// Token: 0x04001067 RID: 4199
		[Nullable(1)]
		private NBackButton _backButton;

		// Token: 0x04001068 RID: 4200
		[Nullable(1)]
		protected NSubmenuStack _stack;

		/// <summary>
		/// The last control node we were focusing on when this submenu was hidden (usually when another submenu is opened above it)
		/// </summary>
		// Token: 0x04001069 RID: 4201
		[Nullable(2)]
		protected Control _lastFocusedControl;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001533 RID: 5427
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400510C RID: 20748
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ConnectSignals' method.
			/// </summary>
			// Token: 0x0400510D RID: 20749
			public static readonly StringName ConnectSignals = "ConnectSignals";

			/// <summary>
			/// Cached name for the 'HideBackButtonImmediately' method.
			/// </summary>
			// Token: 0x0400510E RID: 20750
			public static readonly StringName HideBackButtonImmediately = "HideBackButtonImmediately";

			/// <summary>
			/// Cached name for the 'SetStack' method.
			/// </summary>
			// Token: 0x0400510F RID: 20751
			public static readonly StringName SetStack = "SetStack";

			/// <summary>
			/// Cached name for the 'OnScreenVisibilityChange' method.
			/// </summary>
			// Token: 0x04005110 RID: 20752
			public static readonly StringName OnScreenVisibilityChange = "OnScreenVisibilityChange";

			/// <summary>
			/// Cached name for the 'OnSubmenuShown' method.
			/// </summary>
			// Token: 0x04005111 RID: 20753
			public static readonly StringName OnSubmenuShown = "OnSubmenuShown";

			/// <summary>
			/// Cached name for the 'OnSubmenuHidden' method.
			/// </summary>
			// Token: 0x04005112 RID: 20754
			public static readonly StringName OnSubmenuHidden = "OnSubmenuHidden";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04005113 RID: 20755
			public static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x04005114 RID: 20756
			public static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001534 RID: 5428
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005115 RID: 20757
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04005116 RID: 20758
			public static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_backButton' field.
			/// </summary>
			// Token: 0x04005117 RID: 20759
			public static readonly StringName _backButton = "_backButton";

			/// <summary>
			/// Cached name for the '_stack' field.
			/// </summary>
			// Token: 0x04005118 RID: 20760
			public static readonly StringName _stack = "_stack";

			/// <summary>
			/// Cached name for the '_lastFocusedControl' field.
			/// </summary>
			// Token: 0x04005119 RID: 20761
			public static readonly StringName _lastFocusedControl = "_lastFocusedControl";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001535 RID: 5429
		public class SignalName : Control.SignalName
		{
		}
	}
}

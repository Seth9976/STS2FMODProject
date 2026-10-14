using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu
{
	// Token: 0x02000301 RID: 769
	[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NEarlyAccessDisclaimer.cs")]
	public class NEarlyAccessDisclaimer : Control, IScreenContext
	{
		/// <summary>
		/// Created as needed (not preloaded) because it only shows up once.
		/// </summary>
		// Token: 0x06002A20 RID: 10784 RVA: 0x001269B8 File Offset: 0x00124BB8
		[NullableContext(1)]
		public static NEarlyAccessDisclaimer Create()
		{
			NEarlyAccessDisclaimer nearlyAccessDisclaimer = ResourceLoader.Load<PackedScene>("res://scenes/screens/main_menu/early_access_disclaimer.tscn", null, 1L).Instantiate<NEarlyAccessDisclaimer>(0L);
			NHotkeyManager instance = NHotkeyManager.Instance;
			if (instance != null)
			{
				instance.AddBlockingScreen(nearlyAccessDisclaimer);
			}
			return nearlyAccessDisclaimer;
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x001269EC File Offset: 0x00124BEC
		public override void _Ready()
		{
			string formattedText = new LocString("main_menu_ui", "EARLY_ACCESS_DISCLAIMER.header").GetFormattedText();
			base.GetNode<MegaLabel>("%Header").SetTextAutoSize(formattedText);
			this._image = base.GetNode<TextureRect>("Image");
			this.UpdateEaDisclaimerDescription();
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateEaDisclaimerDescription)), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateEaDisclaimerDescription)), 0U);
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x00126A84 File Offset: 0x00124C84
		[NullableContext(1)]
		public async Task CloseScreen()
		{
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._image, "modulate:a", 1f, 0.5);
			this._tween.TweenProperty(this._image, "position:y", this._image.Position.Y - 1000f, 0.5).SetEase(0L).SetTrans(10L);
			SaveManager.Instance.SettingsSave.SeenEaDisclaimer = true;
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
			NHotkeyManager instance = NHotkeyManager.Instance;
			if (instance != null)
			{
				instance.RemoveBlockingScreen(this);
			}
			NModalContainer.Instance.Clear();
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x00126AC8 File Offset: 0x00124CC8
		private void UpdateEaDisclaimerDescription()
		{
			string text;
			if (NControllerManager.Instance.IsUsingController)
			{
				text = new LocString("main_menu_ui", "EARLY_ACCESS_DISCLAIMER.description_controller").GetFormattedText();
			}
			else
			{
				text = new LocString("main_menu_ui", "EARLY_ACCESS_DISCLAIMER.description_mkb").GetFormattedText();
			}
			base.GetNode<MegaRichTextLabel>("%Description").SetTextAutoSize(text);
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06002A24 RID: 10788 RVA: 0x00126B23 File Offset: 0x00124D23
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A25 RID: 10789 RVA: 0x00126B28 File Offset: 0x00124D28
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NEarlyAccessDisclaimer.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NEarlyAccessDisclaimer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEarlyAccessDisclaimer.MethodName.UpdateEaDisclaimerDescription, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A26 RID: 10790 RVA: 0x00126BD8 File Offset: 0x00124DD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEarlyAccessDisclaimer.MethodName.Create && args.Count == 0)
			{
				NEarlyAccessDisclaimer nearlyAccessDisclaimer = NEarlyAccessDisclaimer.Create();
				ret = VariantUtils.CreateFrom<NEarlyAccessDisclaimer>(ref nearlyAccessDisclaimer);
				return true;
			}
			if ((ref method) == NEarlyAccessDisclaimer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEarlyAccessDisclaimer.MethodName.UpdateEaDisclaimerDescription && args.Count == 0)
			{
				this.UpdateEaDisclaimerDescription();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x00126C64 File Offset: 0x00124E64
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEarlyAccessDisclaimer.MethodName.Create && args.Count == 0)
			{
				NEarlyAccessDisclaimer nearlyAccessDisclaimer = NEarlyAccessDisclaimer.Create();
				ret = VariantUtils.CreateFrom<NEarlyAccessDisclaimer>(ref nearlyAccessDisclaimer);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002A28 RID: 10792 RVA: 0x00126CA4 File Offset: 0x00124EA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEarlyAccessDisclaimer.MethodName.Create || (ref method) == NEarlyAccessDisclaimer.MethodName._Ready || (ref method) == NEarlyAccessDisclaimer.MethodName.UpdateEaDisclaimerDescription || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002A29 RID: 10793 RVA: 0x00126CDA File Offset: 0x00124EDA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEarlyAccessDisclaimer.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEarlyAccessDisclaimer.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002A2A RID: 10794 RVA: 0x00126D1C File Offset: 0x00124F1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEarlyAccessDisclaimer.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NEarlyAccessDisclaimer.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NEarlyAccessDisclaimer.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._image);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002A2B RID: 10795 RVA: 0x00126D94 File Offset: 0x00124F94
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEarlyAccessDisclaimer.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEarlyAccessDisclaimer.PropertyName._image, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEarlyAccessDisclaimer.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002A2C RID: 10796 RVA: 0x00126E0B File Offset: 0x0012500B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEarlyAccessDisclaimer.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NEarlyAccessDisclaimer.PropertyName._image, Variant.From<Control>(ref this._image));
		}

		/// <inheritdoc />
		// Token: 0x06002A2D RID: 10797 RVA: 0x00126E40 File Offset: 0x00125040
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEarlyAccessDisclaimer.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEarlyAccessDisclaimer.PropertyName._image, ref variant2))
			{
				this._image = variant2.As<Control>();
			}
		}

		// Token: 0x04000FBE RID: 4030
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000FBF RID: 4031
		[Nullable(1)]
		private Control _image;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014EA RID: 5354
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004FB3 RID: 20403
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004FB4 RID: 20404
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'UpdateEaDisclaimerDescription' method.
			/// </summary>
			// Token: 0x04004FB5 RID: 20405
			public static readonly StringName UpdateEaDisclaimerDescription = "UpdateEaDisclaimerDescription";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014EB RID: 5355
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004FB6 RID: 20406
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04004FB7 RID: 20407
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004FB8 RID: 20408
			public static readonly StringName _image = "_image";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014EC RID: 5356
		public class SignalName : Control.SignalName
		{
		}
	}
}

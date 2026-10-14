using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	/// <summary>
	/// Vertical popup used in several places to get confirmation for actions.
	/// Renders above the capstone screens (above top bar).
	/// </summary>
	// Token: 0x0200038A RID: 906
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NGenericPopup.cs")]
	public class NGenericPopup : Control, IScreenContext
	{
		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x060035DD RID: 13789 RVA: 0x0019233B File Offset: 0x0019053B
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NGenericPopup._scenePath);
			}
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x00192348 File Offset: 0x00190548
		[NullableContext(2)]
		public static NGenericPopup Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NGenericPopup._scenePath).Instantiate<NGenericPopup>(0L);
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x00192378 File Offset: 0x00190578
		[NullableContext(1)]
		public Task<bool> WaitForConfirmation(LocString body, LocString header, [Nullable(2)] LocString noButton, LocString yesButton)
		{
			this._verticalPopup = base.GetNode<NVerticalPopup>("VerticalPopup");
			this._confirmationCompletionSource = new TaskCompletionSource<bool>();
			this._verticalPopup.SetText(header, body);
			this._verticalPopup.InitYesButton(yesButton, new Action<NButton>(this.OnYesButtonPressed));
			if (noButton != null)
			{
				this._verticalPopup.InitNoButton(noButton, new Action<NButton>(this.OnNoButtonPressed));
			}
			else
			{
				this._verticalPopup.HideNoButton();
			}
			return this._confirmationCompletionSource.Task;
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x001923FF File Offset: 0x001905FF
		[NullableContext(1)]
		private void OnYesButtonPressed(NButton _)
		{
			this._confirmationCompletionSource.SetResult(true);
			this.QueueFreeSafely();
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x00192413 File Offset: 0x00190613
		[NullableContext(1)]
		private void OnNoButtonPressed(NButton _)
		{
			this._confirmationCompletionSource.SetResult(false);
			this.QueueFreeSafely();
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x060035E2 RID: 13794 RVA: 0x00192427 File Offset: 0x00190627
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
		// Token: 0x060035E3 RID: 13795 RVA: 0x0019242C File Offset: 0x0019062C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NGenericPopup.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NGenericPopup.MethodName.OnYesButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NGenericPopup.MethodName.OnNoButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060035E4 RID: 13796 RVA: 0x0019253C File Offset: 0x0019073C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGenericPopup.MethodName.Create && args.Count == 0)
			{
				NGenericPopup ngenericPopup = NGenericPopup.Create();
				ret = VariantUtils.CreateFrom<NGenericPopup>(ref ngenericPopup);
				return true;
			}
			if ((ref method) == NGenericPopup.MethodName.OnYesButtonPressed && args.Count == 1)
			{
				this.OnYesButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGenericPopup.MethodName.OnNoButtonPressed && args.Count == 1)
			{
				this.OnNoButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x001925E4 File Offset: 0x001907E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGenericPopup.MethodName.Create && args.Count == 0)
			{
				NGenericPopup ngenericPopup = NGenericPopup.Create();
				ret = VariantUtils.CreateFrom<NGenericPopup>(ref ngenericPopup);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060035E6 RID: 13798 RVA: 0x00192624 File Offset: 0x00190824
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGenericPopup.MethodName.Create || (ref method) == NGenericPopup.MethodName.OnYesButtonPressed || (ref method) == NGenericPopup.MethodName.OnNoButtonPressed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060035E7 RID: 13799 RVA: 0x0019265A File Offset: 0x0019085A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGenericPopup.PropertyName._verticalPopup)
			{
				this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
				return true;
			}
			if ((ref name) == NGenericPopup.PropertyName._steamId)
			{
				this._steamId = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060035E8 RID: 13800 RVA: 0x0019269C File Offset: 0x0019089C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGenericPopup.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NGenericPopup.PropertyName._verticalPopup)
			{
				value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
				return true;
			}
			if ((ref name) == NGenericPopup.PropertyName._steamId)
			{
				value = VariantUtils.CreateFrom<ulong>(ref this._steamId);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060035E9 RID: 13801 RVA: 0x00192714 File Offset: 0x00190914
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGenericPopup.PropertyName._verticalPopup, 0L, "", 4096L, false),
				new PropertyInfo(2L, NGenericPopup.PropertyName._steamId, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGenericPopup.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060035EA RID: 13802 RVA: 0x0019278A File Offset: 0x0019098A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGenericPopup.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
			info.AddProperty(NGenericPopup.PropertyName._steamId, Variant.From<ulong>(ref this._steamId));
		}

		/// <inheritdoc />
		// Token: 0x060035EB RID: 13803 RVA: 0x001927C0 File Offset: 0x001909C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGenericPopup.PropertyName._verticalPopup, ref variant))
			{
				this._verticalPopup = variant.As<NVerticalPopup>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGenericPopup.PropertyName._steamId, ref variant2))
			{
				this._steamId = variant2.As<ulong>();
			}
		}

		// Token: 0x040014DA RID: 5338
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/generic_popup");

		// Token: 0x040014DB RID: 5339
		[Nullable(1)]
		private NVerticalPopup _verticalPopup;

		// Token: 0x040014DC RID: 5340
		[Nullable(1)]
		private TaskCompletionSource<bool> _confirmationCompletionSource;

		// Token: 0x040014DD RID: 5341
		private ulong _steamId;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200170F RID: 5903
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040059E5 RID: 23013
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'OnYesButtonPressed' method.
			/// </summary>
			// Token: 0x040059E6 RID: 23014
			public static readonly StringName OnYesButtonPressed = "OnYesButtonPressed";

			/// <summary>
			/// Cached name for the 'OnNoButtonPressed' method.
			/// </summary>
			// Token: 0x040059E7 RID: 23015
			public static readonly StringName OnNoButtonPressed = "OnNoButtonPressed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001710 RID: 5904
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040059E8 RID: 23016
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_verticalPopup' field.
			/// </summary>
			// Token: 0x040059E9 RID: 23017
			public static readonly StringName _verticalPopup = "_verticalPopup";

			/// <summary>
			/// Cached name for the '_steamId' field.
			/// </summary>
			// Token: 0x040059EA RID: 23018
			public static readonly StringName _steamId = "_steamId";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001711 RID: 5905
		public class SignalName : Control.SignalName
		{
		}
	}
}

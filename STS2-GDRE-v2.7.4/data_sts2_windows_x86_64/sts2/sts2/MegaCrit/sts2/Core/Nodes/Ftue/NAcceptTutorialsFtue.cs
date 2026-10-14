using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Ftue
{
	// Token: 0x020003A6 RID: 934
	[ScriptPath("res://src/Core/Nodes/Ftue/NAcceptTutorialsFtue.cs")]
	public class NAcceptTutorialsFtue : NFtue
	{
		// Token: 0x06003846 RID: 14406 RVA: 0x001A934C File Offset: 0x001A754C
		public override void _Ready()
		{
			this._verticalPopup = base.GetNode<NVerticalPopup>("VerticalPopup");
			this._verticalPopup.SetText(new LocString("main_menu_ui", "ENABLE_TUTORIALS.title"), new LocString("main_menu_ui", "ENABLE_TUTORIALS.description"));
			this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.confirm"), new Action<NButton>(this.YesTutorials));
			this._verticalPopup.InitNoButton(new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), new Action<NButton>(this.NoTutorials));
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x001A93E4 File Offset: 0x001A75E4
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NAcceptTutorialsFtue Create(NCharacterSelectScreen charSelectScreen, Action onFinished)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NAcceptTutorialsFtue nacceptTutorialsFtue = PreloadManager.Cache.GetScene(NAcceptTutorialsFtue._scenePath).Instantiate<NAcceptTutorialsFtue>(0L);
			nacceptTutorialsFtue._charSelectScreen = charSelectScreen;
			nacceptTutorialsFtue._onFinished = onFinished;
			return nacceptTutorialsFtue;
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x001A9420 File Offset: 0x001A7620
		[NullableContext(1)]
		private void NoTutorials(NButton _)
		{
			SaveManager.Instance.MarkFtueAsComplete("accept_tutorials_ftue");
			SaveManager.Instance.SetFtuesEnabled(false);
			this._onFinished();
			base.CloseFtue();
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x001A944D File Offset: 0x001A764D
		[NullableContext(1)]
		private void YesTutorials(NButton _)
		{
			SaveManager.Instance.MarkFtueAsComplete("accept_tutorials_ftue");
			this._onFinished();
			base.CloseFtue();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600384A RID: 14410 RVA: 0x001A9470 File Offset: 0x001A7670
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NAcceptTutorialsFtue.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAcceptTutorialsFtue.MethodName.NoTutorials, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NAcceptTutorialsFtue.MethodName.YesTutorials, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600384B RID: 14411 RVA: 0x001A9574 File Offset: 0x001A7774
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAcceptTutorialsFtue.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAcceptTutorialsFtue.MethodName.NoTutorials && args.Count == 1)
			{
				this.NoTutorials(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAcceptTutorialsFtue.MethodName.YesTutorials && args.Count == 1)
			{
				this.YesTutorials(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600384C RID: 14412 RVA: 0x001A9615 File Offset: 0x001A7815
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAcceptTutorialsFtue.MethodName._Ready || (ref method) == NAcceptTutorialsFtue.MethodName.NoTutorials || (ref method) == NAcceptTutorialsFtue.MethodName.YesTutorials || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600384D RID: 14413 RVA: 0x001A964B File Offset: 0x001A784B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAcceptTutorialsFtue.PropertyName._charSelectScreen)
			{
				this._charSelectScreen = VariantUtils.ConvertTo<NCharacterSelectScreen>(ref value);
				return true;
			}
			if ((ref name) == NAcceptTutorialsFtue.PropertyName._verticalPopup)
			{
				this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600384E RID: 14414 RVA: 0x001A968C File Offset: 0x001A788C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAcceptTutorialsFtue.PropertyName._charSelectScreen)
			{
				value = VariantUtils.CreateFrom<NCharacterSelectScreen>(ref this._charSelectScreen);
				return true;
			}
			if ((ref name) == NAcceptTutorialsFtue.PropertyName._verticalPopup)
			{
				value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600384F RID: 14415 RVA: 0x001A96E4 File Offset: 0x001A78E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NAcceptTutorialsFtue.PropertyName._charSelectScreen, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAcceptTutorialsFtue.PropertyName._verticalPopup, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003850 RID: 14416 RVA: 0x001A973A File Offset: 0x001A793A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAcceptTutorialsFtue.PropertyName._charSelectScreen, Variant.From<NCharacterSelectScreen>(ref this._charSelectScreen));
			info.AddProperty(NAcceptTutorialsFtue.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
		}

		/// <inheritdoc />
		// Token: 0x06003851 RID: 14417 RVA: 0x001A9770 File Offset: 0x001A7970
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAcceptTutorialsFtue.PropertyName._charSelectScreen, ref variant))
			{
				this._charSelectScreen = variant.As<NCharacterSelectScreen>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAcceptTutorialsFtue.PropertyName._verticalPopup, ref variant2))
			{
				this._verticalPopup = variant2.As<NVerticalPopup>();
			}
		}

		// Token: 0x040015CE RID: 5582
		[Nullable(1)]
		public const string id = "accept_tutorials_ftue";

		// Token: 0x040015CF RID: 5583
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/accept_tutorials_ftue");

		// Token: 0x040015D0 RID: 5584
		[Nullable(1)]
		private NCharacterSelectScreen _charSelectScreen;

		// Token: 0x040015D1 RID: 5585
		[Nullable(1)]
		private NVerticalPopup _verticalPopup;

		// Token: 0x040015D2 RID: 5586
		[Nullable(1)]
		private Action _onFinished;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200178C RID: 6028
		public new class MethodName : NFtue.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005BEB RID: 23531
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'NoTutorials' method.
			/// </summary>
			// Token: 0x04005BEC RID: 23532
			public static readonly StringName NoTutorials = "NoTutorials";

			/// <summary>
			/// Cached name for the 'YesTutorials' method.
			/// </summary>
			// Token: 0x04005BED RID: 23533
			public static readonly StringName YesTutorials = "YesTutorials";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200178D RID: 6029
		public new class PropertyName : NFtue.PropertyName
		{
			/// <summary>
			/// Cached name for the '_charSelectScreen' field.
			/// </summary>
			// Token: 0x04005BEE RID: 23534
			public static readonly StringName _charSelectScreen = "_charSelectScreen";

			/// <summary>
			/// Cached name for the '_verticalPopup' field.
			/// </summary>
			// Token: 0x04005BEF RID: 23535
			public static readonly StringName _verticalPopup = "_verticalPopup";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200178E RID: 6030
		public new class SignalName : NFtue.SignalName
		{
		}
	}
}

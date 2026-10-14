using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002B4 RID: 692
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NScreenshakePaginator.cs")]
	public class NScreenshakePaginator : NPaginator, IResettableSettingNode
	{
		// Token: 0x06002499 RID: 9369 RVA: 0x000F7E6C File Offset: 0x000F606C
		public override void _Ready()
		{
			base.ConnectSignals();
			this._options.Add(new LocString("settings_ui", "SCREENSHAKE_NONE").GetFormattedText());
			this._options.Add(new LocString("settings_ui", "SCREENSHAKE_SOME").GetFormattedText());
			this._options.Add(new LocString("settings_ui", "SCREENSHAKE_NORMAL").GetFormattedText());
			this._options.Add(new LocString("settings_ui", "SCREENSHAKE_LOTS").GetFormattedText());
			this._options.Add(new LocString("settings_ui", "SCREENSHAKE_CAAAW").GetFormattedText());
			this.SetFromSettings();
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x000F7F20 File Offset: 0x000F6120
		public void SetFromSettings()
		{
			this._currentIndex = SaveManager.Instance.PrefsSave.ScreenShakeOptionIndex;
			if (this._currentIndex >= this._options.Count)
			{
				this._label.SetTextAutoSize(">:P");
			}
			else
			{
				this._label.SetTextAutoSize(this._options[this._currentIndex]);
			}
			NGame.Instance.SetScreenshakeMultiplier(NScreenshakePaginator.GetShakeMultiplier(this._currentIndex));
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x000F7F98 File Offset: 0x000F6198
		protected override void OnIndexChanged(int index)
		{
			if (this._currentIndex >= this._options.Count)
			{
				this._currentIndex = 2;
			}
			this._currentIndex = index;
			this._label.SetTextAutoSize(this._options[index]);
			SaveManager.Instance.PrefsSave.ScreenShakeOptionIndex = this._currentIndex;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Screenshake set to: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._currentIndex);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			NGame.Instance.SetScreenshakeMultiplier(NScreenshakePaginator.GetShakeMultiplier(this._currentIndex));
			NGame.Instance.ScreenShakeTrauma(ShakeStrength.Medium);
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000F8044 File Offset: 0x000F6244
		public static float GetShakeMultiplier(int index)
		{
			float num;
			switch (index)
			{
			case 0:
				num = 0f;
				break;
			case 1:
				num = 0.5f;
				break;
			case 2:
				num = 1f;
				break;
			case 3:
				num = 2f;
				break;
			case 4:
				num = 4f;
				break;
			default:
				num = (float)index;
				break;
			}
			return num;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600249D RID: 9373 RVA: 0x000F809C File Offset: 0x000F629C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NScreenshakePaginator.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScreenshakePaginator.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NScreenshakePaginator.MethodName.OnIndexChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NScreenshakePaginator.MethodName.GetShakeMultiplier, new PropertyInfo(3L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600249E RID: 9374 RVA: 0x000F81B8 File Offset: 0x000F63B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScreenshakePaginator.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenshakePaginator.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenshakePaginator.MethodName.OnIndexChanged && args.Count == 1)
			{
				this.OnIndexChanged(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NScreenshakePaginator.MethodName.GetShakeMultiplier && args.Count == 1)
			{
				float shakeMultiplier = NScreenshakePaginator.GetShakeMultiplier(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<float>(ref shakeMultiplier);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x000F8284 File Offset: 0x000F6484
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NScreenshakePaginator.MethodName.GetShakeMultiplier && args.Count == 1)
			{
				float shakeMultiplier = NScreenshakePaginator.GetShakeMultiplier(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<float>(ref shakeMultiplier);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060024A0 RID: 9376 RVA: 0x000F82D4 File Offset: 0x000F64D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NScreenshakePaginator.MethodName._Ready || (ref method) == NScreenshakePaginator.MethodName.SetFromSettings || (ref method) == NScreenshakePaginator.MethodName.OnIndexChanged || (ref method) == NScreenshakePaginator.MethodName.GetShakeMultiplier || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060024A1 RID: 9377 RVA: 0x000F8324 File Offset: 0x000F6524
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060024A2 RID: 9378 RVA: 0x000F832D File Offset: 0x000F652D
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013D4 RID: 5076
		public new class MethodName : NPaginator.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004BD1 RID: 19409
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004BD2 RID: 19410
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'OnIndexChanged' method.
			/// </summary>
			// Token: 0x04004BD3 RID: 19411
			public new static readonly StringName OnIndexChanged = "OnIndexChanged";

			/// <summary>
			/// Cached name for the 'GetShakeMultiplier' method.
			/// </summary>
			// Token: 0x04004BD4 RID: 19412
			public static readonly StringName GetShakeMultiplier = "GetShakeMultiplier";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013D5 RID: 5077
		public new class PropertyName : NPaginator.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013D6 RID: 5078
		public new class SignalName : NPaginator.SignalName
		{
		}
	}
}

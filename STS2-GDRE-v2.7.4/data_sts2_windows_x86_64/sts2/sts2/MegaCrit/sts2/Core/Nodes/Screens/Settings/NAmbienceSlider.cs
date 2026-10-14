using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x02000285 RID: 645
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NAmbienceSlider.cs")]
	public class NAmbienceSlider : NSettingsSlider
	{
		// Token: 0x06002266 RID: 8806 RVA: 0x000EBD30 File Offset: 0x000E9F30
		public override void _Ready()
		{
			this.ConnectSignals();
			this._slider.SetValueWithoutAnimation((double)(SaveManager.Instance.SettingsSave.VolumeAmbience * 100f));
			GodotObject slider = this._slider;
			StringName valueChanged = Range.SignalName.ValueChanged;
			Action<double> action;
			if ((action = NAmbienceSlider.<>O.<0>__OnValueChanged) == null)
			{
				action = (NAmbienceSlider.<>O.<0>__OnValueChanged = new Action<double>(NAmbienceSlider.OnValueChanged));
			}
			slider.Connect(valueChanged, Callable.From<double>(action), 0U);
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x000EBD98 File Offset: 0x000E9F98
		private static void OnValueChanged(double value)
		{
			float num = (float)value / 100f;
			SaveManager.Instance.SettingsSave.VolumeAmbience = num;
			NAudioManager instance = NAudioManager.Instance;
			if (instance != null)
			{
				instance.SetAmbienceVol(num);
			}
			NDebugAudioManager instance2 = NDebugAudioManager.Instance;
			if (instance2 == null)
			{
				return;
			}
			instance2.SetSfxAudioVolume(num);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002268 RID: 8808 RVA: 0x000EBDE0 File Offset: 0x000E9FE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NAmbienceSlider.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAmbienceSlider.MethodName.OnValueChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002269 RID: 8809 RVA: 0x000EBE7C File Offset: 0x000EA07C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAmbienceSlider.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAmbienceSlider.MethodName.OnValueChanged && args.Count == 1)
			{
				NAmbienceSlider.OnValueChanged(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x000EBEE9 File Offset: 0x000EA0E9
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAmbienceSlider.MethodName.OnValueChanged && args.Count == 1)
			{
				NAmbienceSlider.OnValueChanged(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600226B RID: 8811 RVA: 0x000EBF25 File Offset: 0x000EA125
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAmbienceSlider.MethodName._Ready || (ref method) == NAmbienceSlider.MethodName.OnValueChanged || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600226C RID: 8812 RVA: 0x000EBF4C File Offset: 0x000EA14C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x0600226D RID: 8813 RVA: 0x000EBF55 File Offset: 0x000EA155
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001341 RID: 4929
		public new class MethodName : NSettingsSlider.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004ADB RID: 19163
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnValueChanged' method.
			/// </summary>
			// Token: 0x04004ADC RID: 19164
			public new static readonly StringName OnValueChanged = "OnValueChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001342 RID: 4930
		public new class PropertyName : NSettingsSlider.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001343 RID: 4931
		public new class SignalName : NSettingsSlider.SignalName
		{
		}

		// Token: 0x02001344 RID: 4932
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04004ADD RID: 19165
			public static Action<double> <0>__OnValueChanged;
		}
	}
}

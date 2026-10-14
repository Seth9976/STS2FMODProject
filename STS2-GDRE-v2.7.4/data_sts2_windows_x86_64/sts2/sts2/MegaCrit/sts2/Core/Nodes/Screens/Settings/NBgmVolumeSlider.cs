using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x0200028A RID: 650
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NBgmVolumeSlider.cs")]
	public class NBgmVolumeSlider : NSettingsSlider
	{
		// Token: 0x060022A0 RID: 8864 RVA: 0x000ECD80 File Offset: 0x000EAF80
		public override void _Ready()
		{
			this.ConnectSignals();
			this._slider.SetValueWithoutAnimation((double)(SaveManager.Instance.SettingsSave.VolumeBgm * 100f));
			this._slider.Connect(Range.SignalName.ValueChanged, Callable.From<double>(new Action<double>(this.OnValueChanged)), 0U);
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x000ECDD8 File Offset: 0x000EAFD8
		private void OnValueChanged(double value)
		{
			float num = (float)(value / 100.0);
			SaveManager.Instance.SettingsSave.VolumeBgm = num;
			NAudioManager instance = NAudioManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetBgmVol(num);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060022A2 RID: 8866 RVA: 0x000ECE14 File Offset: 0x000EB014
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NBgmVolumeSlider.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBgmVolumeSlider.MethodName.OnValueChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060022A3 RID: 8867 RVA: 0x000ECEAC File Offset: 0x000EB0AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBgmVolumeSlider.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBgmVolumeSlider.MethodName.OnValueChanged && args.Count == 1)
			{
				this.OnValueChanged(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060022A4 RID: 8868 RVA: 0x000ECF1A File Offset: 0x000EB11A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBgmVolumeSlider.MethodName._Ready || (ref method) == NBgmVolumeSlider.MethodName.OnValueChanged || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060022A5 RID: 8869 RVA: 0x000ECF41 File Offset: 0x000EB141
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060022A6 RID: 8870 RVA: 0x000ECF4A File Offset: 0x000EB14A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001351 RID: 4945
		public new class MethodName : NSettingsSlider.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004AEE RID: 19182
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnValueChanged' method.
			/// </summary>
			// Token: 0x04004AEF RID: 19183
			public new static readonly StringName OnValueChanged = "OnValueChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001352 RID: 4946
		public new class PropertyName : NSettingsSlider.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001353 RID: 4947
		public new class SignalName : NSettingsSlider.SignalName
		{
		}
	}
}

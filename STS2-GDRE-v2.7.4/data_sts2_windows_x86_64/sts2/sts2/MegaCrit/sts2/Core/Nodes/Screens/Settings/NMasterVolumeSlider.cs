using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002A1 RID: 673
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NMasterVolumeSlider.cs")]
	public class NMasterVolumeSlider : NSettingsSlider
	{
		// Token: 0x060023B9 RID: 9145 RVA: 0x000F306C File Offset: 0x000F126C
		public override void _Ready()
		{
			this.ConnectSignals();
			this._slider.SetValueWithoutAnimation((double)(SaveManager.Instance.SettingsSave.VolumeMaster * 100f));
			this._slider.Connect(Range.SignalName.ValueChanged, Callable.From<double>(new Action<double>(this.OnValueChanged)), 0U);
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x000F30C4 File Offset: 0x000F12C4
		private void OnValueChanged(double value)
		{
			float num = (float)value * 0.01f;
			SaveManager.Instance.SettingsSave.VolumeMaster = num;
			NAudioManager instance = NAudioManager.Instance;
			if (instance != null)
			{
				instance.SetMasterVol(num);
			}
			NDebugAudioManager instance2 = NDebugAudioManager.Instance;
			if (instance2 == null)
			{
				return;
			}
			instance2.SetMasterAudioVolume(num);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060023BB RID: 9147 RVA: 0x000F310C File Offset: 0x000F130C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NMasterVolumeSlider.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMasterVolumeSlider.MethodName.OnValueChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060023BC RID: 9148 RVA: 0x000F31A4 File Offset: 0x000F13A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMasterVolumeSlider.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMasterVolumeSlider.MethodName.OnValueChanged && args.Count == 1)
			{
				this.OnValueChanged(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060023BD RID: 9149 RVA: 0x000F3212 File Offset: 0x000F1412
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMasterVolumeSlider.MethodName._Ready || (ref method) == NMasterVolumeSlider.MethodName.OnValueChanged || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060023BE RID: 9150 RVA: 0x000F3239 File Offset: 0x000F1439
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060023BF RID: 9151 RVA: 0x000F3242 File Offset: 0x000F1442
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001399 RID: 5017
		public new class MethodName : NSettingsSlider.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B6B RID: 19307
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnValueChanged' method.
			/// </summary>
			// Token: 0x04004B6C RID: 19308
			public new static readonly StringName OnValueChanged = "OnValueChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200139A RID: 5018
		public new class PropertyName : NSettingsSlider.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200139B RID: 5019
		public new class SignalName : NSettingsSlider.SignalName
		{
		}
	}
}

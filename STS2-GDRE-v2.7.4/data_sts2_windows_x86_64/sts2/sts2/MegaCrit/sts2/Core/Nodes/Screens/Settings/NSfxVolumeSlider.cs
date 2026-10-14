using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x020002BF RID: 703
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSfxVolumeSlider.cs")]
	public class NSfxVolumeSlider : NSettingsSlider
	{
		// Token: 0x06002551 RID: 9553 RVA: 0x000FD3A8 File Offset: 0x000FB5A8
		public override void _Ready()
		{
			this.ConnectSignals();
			this._slider.SetValueWithoutAnimation((double)(SaveManager.Instance.SettingsSave.VolumeSfx * 100f));
			GodotObject slider = this._slider;
			StringName valueChanged = Range.SignalName.ValueChanged;
			Action<double> action;
			if ((action = NSfxVolumeSlider.<>O.<0>__OnValueChanged) == null)
			{
				action = (NSfxVolumeSlider.<>O.<0>__OnValueChanged = new Action<double>(NSfxVolumeSlider.OnValueChanged));
			}
			slider.Connect(valueChanged, Callable.From<double>(action), 0U);
			GodotObject slider2 = this._slider;
			StringName mouseReleased = NSlider.SignalName.MouseReleased;
			Action<bool> action2;
			if ((action2 = NSfxVolumeSlider.<>O.<1>__OnDragEnded) == null)
			{
				action2 = (NSfxVolumeSlider.<>O.<1>__OnDragEnded = new Action<bool>(NSfxVolumeSlider.OnDragEnded));
			}
			slider2.Connect(mouseReleased, Callable.From<bool>(action2), 0U);
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x000FD440 File Offset: 0x000FB640
		private static void OnValueChanged(double value)
		{
			float num = (float)value / 100f;
			SaveManager.Instance.SettingsSave.VolumeSfx = num;
			NAudioManager instance = NAudioManager.Instance;
			if (instance != null)
			{
				instance.SetSfxVol(num);
			}
			NDebugAudioManager instance2 = NDebugAudioManager.Instance;
			if (instance2 == null)
			{
				return;
			}
			instance2.SetSfxAudioVolume(num);
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000FD487 File Offset: 0x000FB687
		private static void OnDragEnded(bool valueChanged)
		{
			if (valueChanged)
			{
				NDebugAudioManager instance = NDebugAudioManager.Instance;
				if (instance == null)
				{
					return;
				}
				instance.Play("dagger_throw.mp3", 1f, PitchVariance.None);
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002554 RID: 9556 RVA: 0x000FD4A8 File Offset: 0x000FB6A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NSfxVolumeSlider.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSfxVolumeSlider.MethodName.OnValueChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "value", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSfxVolumeSlider.MethodName.OnDragEnded, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "valueChanged", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002555 RID: 9557 RVA: 0x000FD598 File Offset: 0x000FB798
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSfxVolumeSlider.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSfxVolumeSlider.MethodName.OnValueChanged && args.Count == 1)
			{
				NSfxVolumeSlider.OnValueChanged(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSfxVolumeSlider.MethodName.OnDragEnded && args.Count == 1)
			{
				NSfxVolumeSlider.OnDragEnded(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000FD638 File Offset: 0x000FB838
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSfxVolumeSlider.MethodName.OnValueChanged && args.Count == 1)
			{
				NSfxVolumeSlider.OnValueChanged(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSfxVolumeSlider.MethodName.OnDragEnded && args.Count == 1)
			{
				NSfxVolumeSlider.OnDragEnded(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002557 RID: 9559 RVA: 0x000FD6B1 File Offset: 0x000FB8B1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSfxVolumeSlider.MethodName._Ready || (ref method) == NSfxVolumeSlider.MethodName.OnValueChanged || (ref method) == NSfxVolumeSlider.MethodName.OnDragEnded || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002558 RID: 9560 RVA: 0x000FD6E7 File Offset: 0x000FB8E7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06002559 RID: 9561 RVA: 0x000FD6F0 File Offset: 0x000FB8F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013FA RID: 5114
		public new class MethodName : NSettingsSlider.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C3A RID: 19514
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnValueChanged' method.
			/// </summary>
			// Token: 0x04004C3B RID: 19515
			public new static readonly StringName OnValueChanged = "OnValueChanged";

			/// <summary>
			/// Cached name for the 'OnDragEnded' method.
			/// </summary>
			// Token: 0x04004C3C RID: 19516
			public static readonly StringName OnDragEnded = "OnDragEnded";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013FB RID: 5115
		public new class PropertyName : NSettingsSlider.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013FC RID: 5116
		public new class SignalName : NSettingsSlider.SignalName
		{
		}

		// Token: 0x020013FD RID: 5117
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04004C3D RID: 19517
			public static Action<double> <0>__OnValueChanged;

			// Token: 0x04004C3E RID: 19518
			public static Action<bool> <1>__OnDragEnded;
		}
	}
}

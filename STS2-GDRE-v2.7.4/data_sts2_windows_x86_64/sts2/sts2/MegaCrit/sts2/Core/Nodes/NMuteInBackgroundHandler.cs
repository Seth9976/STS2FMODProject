using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes
{
	/// <summary>
	/// Responsible for muting audio when the game enters the background, if the player has the setting set.
	/// </summary>
	// Token: 0x02000179 RID: 377
	[ScriptPath("res://src/Core/Nodes/NMuteInBackgroundHandler.cs")]
	public class NMuteInBackgroundHandler : Node
	{
		// Token: 0x06001212 RID: 4626 RVA: 0x0006A810 File Offset: 0x00068A10
		public override void _Notification(int what)
		{
			if ((long)what == 1005L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (!this._loggedEnvironment)
				{
					this._loggedEnvironment = true;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
					defaultInterpolatedStringHandler.AppendLiteral("MuteInBackground: environment [displayServer=");
					defaultInterpolatedStringHandler.AppendFormatted(DisplayServer.GetName());
					defaultInterpolatedStringHandler.AppendLiteral(", os=");
					defaultInterpolatedStringHandler.AppendFormatted(OS.GetName());
					defaultInterpolatedStringHandler.AppendLiteral("]");
					Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				}
				this._lastFocusOutMsec = Time.GetTicksMsec();
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 2);
				defaultInterpolatedStringHandler.AppendLiteral("MuteInBackground: FocusOut received [tween=");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this._tween != null);
				defaultInterpolatedStringHandler.AppendLiteral(", windowFocused=");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(base.GetWindow().HasFocus());
				defaultInterpolatedStringHandler.AppendLiteral("]");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				this.Mute();
				return;
			}
			if ((long)what == 1004L)
			{
				ulong num = Time.GetTicksMsec() - this._lastFocusOutMsec;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(77, 3);
				defaultInterpolatedStringHandler.AppendLiteral("MuteInBackground: FocusIn received [tween=");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this._tween != null);
				defaultInterpolatedStringHandler.AppendLiteral(", msSinceFocusOut=");
				defaultInterpolatedStringHandler.AppendFormatted<ulong>(num);
				defaultInterpolatedStringHandler.AppendLiteral(", windowFocused=");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(base.GetWindow().HasFocus());
				defaultInterpolatedStringHandler.AppendLiteral("]");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				this.Unmute();
			}
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0006A98C File Offset: 0x00068B8C
		private void Mute()
		{
			PrefsSave prefsSave = SaveManager.Instance.PrefsSave;
			SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
			if (prefsSave != null && settingsSave != null && prefsSave.MuteInBackground)
			{
				bool flag = this._tween != null;
				Tween tween = this._tween;
				if (tween != null)
				{
					tween.Kill();
				}
				this._tween = base.CreateTween();
				Tween tween2 = this._tween;
				Action<float> action;
				if ((action = NMuteInBackgroundHandler.<>O.<0>__SetMasterVolume) == null)
				{
					action = (NMuteInBackgroundHandler.<>O.<0>__SetMasterVolume = new Action<float>(NMuteInBackgroundHandler.SetMasterVolume));
				}
				tween2.TweenMethod(Callable.From<float>(action), settingsSave.VolumeMaster, 0f, 1.0);
				if (flag)
				{
					Log.Info("MuteInBackground: Muting (replaced existing tween)", 2);
					return;
				}
			}
			else
			{
				Log.Info("MuteInBackground: FocusOut ignored (setting off or saves null)", 2);
			}
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0006AA54 File Offset: 0x00068C54
		private void Unmute()
		{
			if (this._tween != null)
			{
				Tween tween = this._tween;
				if (tween != null)
				{
					tween.Kill();
				}
				this._tween = null;
				float volumeMaster = SaveManager.Instance.SettingsSave.VolumeMaster;
				NMuteInBackgroundHandler.SetMasterVolume(volumeMaster);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 1);
				defaultInterpolatedStringHandler.AppendLiteral("MuteInBackground: Unmuted, restored volume to ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(volumeMaster);
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0006AAC2 File Offset: 0x00068CC2
		private static void SetMasterVolume(float volume)
		{
			NGame.Instance.AudioManager.SetMasterVol(volume);
			NGame.Instance.DebugAudio.SetMasterAudioVolume(volume);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001216 RID: 4630 RVA: 0x0006AAE4 File Offset: 0x00068CE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NMuteInBackgroundHandler.MethodName._Notification, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "what", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMuteInBackgroundHandler.MethodName.Mute, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMuteInBackgroundHandler.MethodName.Unmute, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMuteInBackgroundHandler.MethodName.SetMasterVolume, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "volume", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001217 RID: 4631 RVA: 0x0006AC00 File Offset: 0x00068E00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMuteInBackgroundHandler.MethodName._Notification && args.Count == 1)
			{
				this._Notification(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMuteInBackgroundHandler.MethodName.Mute && args.Count == 0)
			{
				this.Mute();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMuteInBackgroundHandler.MethodName.Unmute && args.Count == 0)
			{
				this.Unmute();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMuteInBackgroundHandler.MethodName.SetMasterVolume && args.Count == 1)
			{
				NMuteInBackgroundHandler.SetMasterVolume(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0006ACC5 File Offset: 0x00068EC5
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMuteInBackgroundHandler.MethodName.SetMasterVolume && args.Count == 1)
			{
				NMuteInBackgroundHandler.SetMasterVolume(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06001219 RID: 4633 RVA: 0x0006AD04 File Offset: 0x00068F04
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMuteInBackgroundHandler.MethodName._Notification || (ref method) == NMuteInBackgroundHandler.MethodName.Mute || (ref method) == NMuteInBackgroundHandler.MethodName.Unmute || (ref method) == NMuteInBackgroundHandler.MethodName.SetMasterVolume || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600121A RID: 4634 RVA: 0x0006AD54 File Offset: 0x00068F54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMuteInBackgroundHandler.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMuteInBackgroundHandler.PropertyName._lastFocusOutMsec)
			{
				this._lastFocusOutMsec = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NMuteInBackgroundHandler.PropertyName._loggedEnvironment)
			{
				this._loggedEnvironment = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600121B RID: 4635 RVA: 0x0006ADBC File Offset: 0x00068FBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMuteInBackgroundHandler.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NMuteInBackgroundHandler.PropertyName._lastFocusOutMsec)
			{
				value = VariantUtils.CreateFrom<ulong>(ref this._lastFocusOutMsec);
				return true;
			}
			if ((ref name) == NMuteInBackgroundHandler.PropertyName._loggedEnvironment)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._loggedEnvironment);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600121C RID: 4636 RVA: 0x0006AE34 File Offset: 0x00069034
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMuteInBackgroundHandler.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NMuteInBackgroundHandler.PropertyName._lastFocusOutMsec, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMuteInBackgroundHandler.PropertyName._loggedEnvironment, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600121D RID: 4637 RVA: 0x0006AEAC File Offset: 0x000690AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMuteInBackgroundHandler.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NMuteInBackgroundHandler.PropertyName._lastFocusOutMsec, Variant.From<ulong>(ref this._lastFocusOutMsec));
			info.AddProperty(NMuteInBackgroundHandler.PropertyName._loggedEnvironment, Variant.From<bool>(ref this._loggedEnvironment));
		}

		/// <inheritdoc />
		// Token: 0x0600121E RID: 4638 RVA: 0x0006AF04 File Offset: 0x00069104
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMuteInBackgroundHandler.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMuteInBackgroundHandler.PropertyName._lastFocusOutMsec, ref variant2))
			{
				this._lastFocusOutMsec = variant2.As<ulong>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMuteInBackgroundHandler.PropertyName._loggedEnvironment, ref variant3))
			{
				this._loggedEnvironment = variant3.As<bool>();
			}
		}

		// Token: 0x04000662 RID: 1634
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000663 RID: 1635
		private ulong _lastFocusOutMsec;

		// Token: 0x04000664 RID: 1636
		private bool _loggedEnvironment;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F77 RID: 3959
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Notification' method.
			/// </summary>
			// Token: 0x04003DBE RID: 15806
			public static readonly StringName _Notification = "_Notification";

			/// <summary>
			/// Cached name for the 'Mute' method.
			/// </summary>
			// Token: 0x04003DBF RID: 15807
			public static readonly StringName Mute = "Mute";

			/// <summary>
			/// Cached name for the 'Unmute' method.
			/// </summary>
			// Token: 0x04003DC0 RID: 15808
			public static readonly StringName Unmute = "Unmute";

			/// <summary>
			/// Cached name for the 'SetMasterVolume' method.
			/// </summary>
			// Token: 0x04003DC1 RID: 15809
			public static readonly StringName SetMasterVolume = "SetMasterVolume";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F78 RID: 3960
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003DC2 RID: 15810
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_lastFocusOutMsec' field.
			/// </summary>
			// Token: 0x04003DC3 RID: 15811
			public static readonly StringName _lastFocusOutMsec = "_lastFocusOutMsec";

			/// <summary>
			/// Cached name for the '_loggedEnvironment' field.
			/// </summary>
			// Token: 0x04003DC4 RID: 15812
			public static readonly StringName _loggedEnvironment = "_loggedEnvironment";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F79 RID: 3961
		public class SignalName : Node.SignalName
		{
		}

		// Token: 0x02000F7A RID: 3962
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04003DC5 RID: 15813
			public static Action<float> <0>__SetMasterVolume;
		}
	}
}

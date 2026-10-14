using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	// Token: 0x02000293 RID: 659
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NFpsPaginator.cs")]
	public class NFpsPaginator : NPaginator, IResettableSettingNode
	{
		// Token: 0x06002304 RID: 8964 RVA: 0x000EE840 File Offset: 0x000ECA40
		public override void _Ready()
		{
			base.ConnectSignals();
			this._options.Add("24");
			this._options.Add("30");
			this._options.Add("59");
			this._options.Add("60");
			this._options.Add("75");
			this._options.Add("90");
			this._options.Add("120");
			this._options.Add("144");
			this._options.Add("165");
			this._options.Add("240");
			this._options.Add("360");
			this._options.Add("500");
			this.SetFromSettings();
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x000EE91C File Offset: 0x000ECB1C
		public void SetFromSettings()
		{
			int num = this._options.IndexOf(SaveManager.Instance.SettingsSave.FpsLimit.ToString());
			this._currentIndex = ((num != -1) ? num : 3);
			this._label.SetTextAutoSize(this._options[this._currentIndex]);
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x000EE978 File Offset: 0x000ECB78
		protected override void OnIndexChanged(int index)
		{
			this._currentIndex = index;
			this._label.SetTextAutoSize(this._options[index]);
			SaveManager.Instance.SettingsSave.FpsLimit = int.Parse(this._options[index]);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
			defaultInterpolatedStringHandler.AppendLiteral("FPS Limit: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(SaveManager.Instance.SettingsSave.FpsLimit);
			Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			Engine.MaxFps = SaveManager.Instance.SettingsSave.FpsLimit;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002307 RID: 8967 RVA: 0x000EEA10 File Offset: 0x000ECC10
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NFpsPaginator.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFpsPaginator.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NFpsPaginator.MethodName.OnIndexChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002308 RID: 8968 RVA: 0x000EEAD8 File Offset: 0x000ECCD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NFpsPaginator.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFpsPaginator.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NFpsPaginator.MethodName.OnIndexChanged && args.Count == 1)
			{
				this.OnIndexChanged(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002309 RID: 8969 RVA: 0x000EEB6B File Offset: 0x000ECD6B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NFpsPaginator.MethodName._Ready || (ref method) == NFpsPaginator.MethodName.SetFromSettings || (ref method) == NFpsPaginator.MethodName.OnIndexChanged || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600230A RID: 8970 RVA: 0x000EEBA1 File Offset: 0x000ECDA1
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x0600230B RID: 8971 RVA: 0x000EEBAA File Offset: 0x000ECDAA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200136C RID: 4972
		public new class MethodName : NPaginator.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B10 RID: 19216
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004B11 RID: 19217
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'OnIndexChanged' method.
			/// </summary>
			// Token: 0x04004B12 RID: 19218
			public new static readonly StringName OnIndexChanged = "OnIndexChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200136D RID: 4973
		public new class PropertyName : NPaginator.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200136E RID: 4974
		public new class SignalName : NPaginator.SignalName
		{
		}
	}
}

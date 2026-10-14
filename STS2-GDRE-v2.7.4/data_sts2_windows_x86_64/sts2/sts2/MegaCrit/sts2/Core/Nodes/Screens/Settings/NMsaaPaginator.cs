using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings
{
	/// <summary>
	/// Sets your 2D MSAA setting. Affects how "jagged" sprites and other 2D things look. Notably when rotated.
	/// </summary>
	// Token: 0x020002A3 RID: 675
	[ScriptPath("res://src/Core/Nodes/Screens/Settings/NMsaaPaginator.cs")]
	public class NMsaaPaginator : NPaginator, IResettableSettingNode
	{
		// Token: 0x060023CA RID: 9162 RVA: 0x000F3488 File Offset: 0x000F1688
		public override void _Ready()
		{
			base.ConnectSignals();
			this._options.Add("0");
			this._options.Add("2");
			this._options.Add("4");
			this._options.Add("8");
			this.SetFromSettings();
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x000F34E4 File Offset: 0x000F16E4
		public void SetFromSettings()
		{
			int num = this._options.IndexOf(SaveManager.Instance.SettingsSave.Msaa.ToString());
			this._currentIndex = ((num != -1) ? num : 3);
			this._label.SetTextAutoSize(this.GetMsaaLabel(int.Parse(this._options[this._currentIndex])));
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x000F354C File Offset: 0x000F174C
		protected override void OnIndexChanged(int index)
		{
			this._currentIndex = index;
			this._label.SetTextAutoSize(this.GetMsaaLabel(int.Parse(this._options[index])));
			SaveManager.Instance.SettingsSave.Msaa = int.Parse(this._options[index]);
			Log.Info("MSAA: " + this._label.Text, 2);
			RenderingServer.ViewportSetMsaa2D(base.GetViewport().GetViewportRid(), this.GetMsaa(SaveManager.Instance.SettingsSave.Msaa));
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x000F35E4 File Offset: 0x000F17E4
		[NullableContext(1)]
		private string GetMsaaLabel(int msaaAmount)
		{
			string text;
			if (msaaAmount != 2)
			{
				if (msaaAmount != 4)
				{
					if (msaaAmount != 8)
					{
						text = new LocString("settings_ui", "MSAA_NONE").GetFormattedText();
					}
					else
					{
						text = "8x";
					}
				}
				else
				{
					text = "4x";
				}
			}
			else
			{
				text = "2x";
			}
			return text;
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x000F3630 File Offset: 0x000F1830
		private RenderingServer.ViewportMsaa GetMsaa(int index)
		{
			RenderingServer.ViewportMsaa viewportMsaa;
			if (index != 2)
			{
				if (index != 4)
				{
					if (index != 8)
					{
						viewportMsaa = 0L;
					}
					else
					{
						viewportMsaa = 3L;
					}
				}
				else
				{
					viewportMsaa = 2L;
				}
			}
			else
			{
				viewportMsaa = 1L;
			}
			return viewportMsaa;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060023CF RID: 9167 RVA: 0x000F3660 File Offset: 0x000F1860
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NMsaaPaginator.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMsaaPaginator.MethodName.SetFromSettings, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMsaaPaginator.MethodName.OnIndexChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMsaaPaginator.MethodName.GetMsaaLabel, new PropertyInfo(4L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "msaaAmount", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMsaaPaginator.MethodName.GetMsaa, new PropertyInfo(2L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060023D0 RID: 9168 RVA: 0x000F37D0 File Offset: 0x000F19D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMsaaPaginator.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMsaaPaginator.MethodName.SetFromSettings && args.Count == 0)
			{
				this.SetFromSettings();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMsaaPaginator.MethodName.OnIndexChanged && args.Count == 1)
			{
				this.OnIndexChanged(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMsaaPaginator.MethodName.GetMsaaLabel && args.Count == 1)
			{
				string msaaLabel = this.GetMsaaLabel(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref msaaLabel);
				return true;
			}
			if ((ref method) == NMsaaPaginator.MethodName.GetMsaa && args.Count == 1)
			{
				RenderingServer.ViewportMsaa msaa = this.GetMsaa(VariantUtils.ConvertTo<int>(args[0]));
				ret = VariantUtils.CreateFrom<RenderingServer.ViewportMsaa>(ref msaa);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060023D1 RID: 9169 RVA: 0x000F38D8 File Offset: 0x000F1AD8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMsaaPaginator.MethodName._Ready || (ref method) == NMsaaPaginator.MethodName.SetFromSettings || (ref method) == NMsaaPaginator.MethodName.OnIndexChanged || (ref method) == NMsaaPaginator.MethodName.GetMsaaLabel || (ref method) == NMsaaPaginator.MethodName.GetMsaa || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060023D2 RID: 9170 RVA: 0x000F3937 File Offset: 0x000F1B37
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060023D3 RID: 9171 RVA: 0x000F3940 File Offset: 0x000F1B40
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200139F RID: 5023
		public new class MethodName : NPaginator.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004B70 RID: 19312
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetFromSettings' method.
			/// </summary>
			// Token: 0x04004B71 RID: 19313
			public static readonly StringName SetFromSettings = "SetFromSettings";

			/// <summary>
			/// Cached name for the 'OnIndexChanged' method.
			/// </summary>
			// Token: 0x04004B72 RID: 19314
			public new static readonly StringName OnIndexChanged = "OnIndexChanged";

			/// <summary>
			/// Cached name for the 'GetMsaaLabel' method.
			/// </summary>
			// Token: 0x04004B73 RID: 19315
			public static readonly StringName GetMsaaLabel = "GetMsaaLabel";

			/// <summary>
			/// Cached name for the 'GetMsaa' method.
			/// </summary>
			// Token: 0x04004B74 RID: 19316
			public static readonly StringName GetMsaa = "GetMsaa";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013A0 RID: 5024
		public new class PropertyName : NPaginator.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020013A1 RID: 5025
		public new class SignalName : NPaginator.SignalName
		{
		}
	}
}

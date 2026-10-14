using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.sts2.Core.Nodes.TopBar
{
	/// <summary>
	/// Allows this UI element to be hovered and display character/ascension/achievement info.
	/// </summary>
	// Token: 0x0200001F RID: 31
	[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarPortraitTip.cs")]
	public class NTopBarPortraitTip : NClickableControl
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00008A0E File Offset: 0x00006C0E
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00008A16 File Offset: 0x00006C16
		public bool ShowTip { get; private set; }

		// Token: 0x0600010B RID: 267 RVA: 0x00008A1F File Offset: 0x00006C1F
		public override void _Ready()
		{
			this.ConnectSignals();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00008A28 File Offset: 0x00006C28
		[NullableContext(1)]
		public void Initialize(IRunState runState)
		{
			int ascensionLevel = runState.AscensionLevel;
			bool flag = runState.GameMode.AreAchievementsAndEpochsLocked();
			this.ShowTip = ascensionLevel > 0 || flag;
			if (this.ShowTip)
			{
				this._hoverTip = AscensionHelper.GetHoverTip(LocalContext.GetMe(runState).Character, ascensionLevel, flag);
			}
			base.FocusMode = (this.ShowTip ? 2L : 0L);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008A90 File Offset: 0x00006C90
		protected override void OnFocus()
		{
			if (!this.ShowTip)
			{
				return;
			}
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + new Vector2(0f, base.Size.Y + 20f), false);
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00008AE4 File Offset: 0x00006CE4
		protected override void OnUnfocus()
		{
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600010F RID: 271 RVA: 0x00008AEC File Offset: 0x00006CEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NTopBarPortraitTip.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarPortraitTip.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarPortraitTip.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000110 RID: 272 RVA: 0x00008B90 File Offset: 0x00006D90
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTopBarPortraitTip.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarPortraitTip.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarPortraitTip.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06000111 RID: 273 RVA: 0x00008C15 File Offset: 0x00006E15
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTopBarPortraitTip.MethodName._Ready || (ref method) == NTopBarPortraitTip.MethodName.OnFocus || (ref method) == NTopBarPortraitTip.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06000112 RID: 274 RVA: 0x00008C4B File Offset: 0x00006E4B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTopBarPortraitTip.PropertyName.ShowTip)
			{
				this.ShowTip = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06000113 RID: 275 RVA: 0x00008C70 File Offset: 0x00006E70
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTopBarPortraitTip.PropertyName.ShowTip)
			{
				bool showTip = this.ShowTip;
				value = VariantUtils.CreateFrom<bool>(ref showTip);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06000114 RID: 276 RVA: 0x00008CA8 File Offset: 0x00006EA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NTopBarPortraitTip.PropertyName.ShowTip, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06000115 RID: 277 RVA: 0x00008CDC File Offset: 0x00006EDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName showTip = NTopBarPortraitTip.PropertyName.ShowTip;
			bool showTip2 = this.ShowTip;
			info.AddProperty(showTip, Variant.From<bool>(ref showTip2));
		}

		/// <inheritdoc />
		// Token: 0x06000116 RID: 278 RVA: 0x00008D0C File Offset: 0x00006F0C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTopBarPortraitTip.PropertyName.ShowTip, ref variant))
			{
				this.ShowTip = variant.As<bool>();
			}
		}

		// Token: 0x04000061 RID: 97
		[Nullable(1)]
		private IHoverTip _hoverTip;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DED RID: 3565
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040031D7 RID: 12759
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040031D8 RID: 12760
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040031D9 RID: 12761
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DEE RID: 3566
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ShowTip' property.
			/// </summary>
			// Token: 0x040031DA RID: 12762
			public static readonly StringName ShowTip = "ShowTip";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DEF RID: 3567
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}

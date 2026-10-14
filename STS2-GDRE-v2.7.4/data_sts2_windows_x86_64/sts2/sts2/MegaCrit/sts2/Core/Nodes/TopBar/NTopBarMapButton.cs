using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;

namespace MegaCrit.Sts2.Core.Nodes.TopBar
{
	// Token: 0x02000245 RID: 581
	[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarMapButton.cs")]
	public class NTopBarMapButton : NTopBarButton
	{
		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001DAE RID: 7598 RVA: 0x000C1471 File Offset: 0x000BF671
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.viewMap };
			}
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x000C1488 File Offset: 0x000BF688
		protected override void OnRelease()
		{
			base.OnRelease();
			if (this.IsOpen())
			{
				NCapstoneContainer instance = NCapstoneContainer.Instance;
				if (instance != null && instance.InUse)
				{
					NCapstoneContainer.Instance.Close();
				}
				else
				{
					NMapScreen.Instance.Close(true);
				}
			}
			else
			{
				NCapstoneContainer.Instance.Close();
				NMapScreen.Instance.Open(true);
			}
			ShaderMaterial hsv = this._hsv;
			if (hsv == null)
			{
				return;
			}
			hsv.SetShaderParameter(NTopBarMapButton._v, 0.9f);
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x000C1504 File Offset: 0x000BF704
		protected override bool IsOpen()
		{
			return NMapScreen.Instance.Visible;
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x000C1510 File Offset: 0x000BF710
		public void StartOscillation()
		{
			Tween oscillateTween = this._oscillateTween;
			if (oscillateTween != null)
			{
				oscillateTween.Kill();
			}
			this._oscillateTween = base.CreateTween();
			this._oscillateTween.SetLoops(0);
			this._oscillateTween.TweenProperty(this._icon, "rotation", -0.12f, 0.8).SetTrans(1L).SetEase(2L);
			this._oscillateTween.TweenProperty(this._icon, "rotation", 0.12f, 0.8).SetTrans(1L).SetEase(2L);
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x000C15C4 File Offset: 0x000BF7C4
		public void StopOscillation()
		{
			Tween oscillateTween = this._oscillateTween;
			if (oscillateTween != null)
			{
				oscillateTween.Kill();
			}
			this._oscillateTween = base.CreateTween();
			this._oscillateTween.TweenProperty(this._icon, "rotation", 0f, 0.5).SetTrans(11L).SetEase(1L);
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x000C162C File Offset: 0x000BF82C
		protected override void OnFocus()
		{
			base.OnFocus();
			LocString locString = new LocString("static_hover_tips", "MAP.title");
			locString.Add("Hotkey", NInputManager.Instance.GetShortcutKey(MegaInput.viewMap).ToString());
			HoverTip hoverTip = new HoverTip(locString, new LocString("static_hover_tips", "MAP.description"), null);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + new Vector2(base.Size.X - nhoverTipSet.Size.X, base.Size.Y + 20f), false);
			}
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x000C16DF File Offset: 0x000BF8DF
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001DB5 RID: 7605 RVA: 0x000C16F0 File Offset: 0x000BF8F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NTopBarMapButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarMapButton.MethodName.IsOpen, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarMapButton.MethodName.StartOscillation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarMapButton.MethodName.StopOscillation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarMapButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarMapButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001DB6 RID: 7606 RVA: 0x000C1820 File Offset: 0x000BFA20
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTopBarMapButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarMapButton.MethodName.IsOpen && args.Count == 0)
			{
				bool flag = this.IsOpen();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NTopBarMapButton.MethodName.StartOscillation && args.Count == 0)
			{
				this.StartOscillation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarMapButton.MethodName.StopOscillation && args.Count == 0)
			{
				this.StopOscillation();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarMapButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarMapButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001DB7 RID: 7607 RVA: 0x000C191C File Offset: 0x000BFB1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTopBarMapButton.MethodName.OnRelease || (ref method) == NTopBarMapButton.MethodName.IsOpen || (ref method) == NTopBarMapButton.MethodName.StartOscillation || (ref method) == NTopBarMapButton.MethodName.StopOscillation || (ref method) == NTopBarMapButton.MethodName.OnFocus || (ref method) == NTopBarMapButton.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001DB8 RID: 7608 RVA: 0x000C198A File Offset: 0x000BFB8A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTopBarMapButton.PropertyName._oscillateTween)
			{
				this._oscillateTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06001DB9 RID: 7609 RVA: 0x000C19B0 File Offset: 0x000BFBB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTopBarMapButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NTopBarMapButton.PropertyName._oscillateTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._oscillateTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001DBA RID: 7610 RVA: 0x000C1A08 File Offset: 0x000BFC08
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NTopBarMapButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBarMapButton.PropertyName._oscillateTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001DBB RID: 7611 RVA: 0x000C1A5E File Offset: 0x000BFC5E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTopBarMapButton.PropertyName._oscillateTween, Variant.From<Tween>(ref this._oscillateTween));
		}

		/// <inheritdoc />
		// Token: 0x06001DBC RID: 7612 RVA: 0x000C1A80 File Offset: 0x000BFC80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTopBarMapButton.PropertyName._oscillateTween, ref variant))
			{
				this._oscillateTween = variant.As<Tween>();
			}
		}

		// Token: 0x04000B6F RID: 2927
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000B70 RID: 2928
		private const float _defaultV = 0.9f;

		// Token: 0x04000B71 RID: 2929
		[Nullable(2)]
		private Tween _oscillateTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001247 RID: 4679
		public new class MethodName : NTopBarButton.MethodName
		{
			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x040046BF RID: 18111
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'IsOpen' method.
			/// </summary>
			// Token: 0x040046C0 RID: 18112
			public new static readonly StringName IsOpen = "IsOpen";

			/// <summary>
			/// Cached name for the 'StartOscillation' method.
			/// </summary>
			// Token: 0x040046C1 RID: 18113
			public static readonly StringName StartOscillation = "StartOscillation";

			/// <summary>
			/// Cached name for the 'StopOscillation' method.
			/// </summary>
			// Token: 0x040046C2 RID: 18114
			public static readonly StringName StopOscillation = "StopOscillation";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040046C3 RID: 18115
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040046C4 RID: 18116
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001248 RID: 4680
		public new class PropertyName : NTopBarButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x040046C5 RID: 18117
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_oscillateTween' field.
			/// </summary>
			// Token: 0x040046C6 RID: 18118
			public static readonly StringName _oscillateTween = "_oscillateTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001249 RID: 4681
		public new class SignalName : NTopBarButton.SignalName
		{
		}
	}
}

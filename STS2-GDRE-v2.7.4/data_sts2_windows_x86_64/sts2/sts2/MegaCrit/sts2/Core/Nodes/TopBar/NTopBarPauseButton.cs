using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.TopBar
{
	// Token: 0x02000246 RID: 582
	[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarPauseButton.cs")]
	public class NTopBarPauseButton : NTopBarButton
	{
		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001DBF RID: 7615 RVA: 0x000C1AC9 File Offset: 0x000BFCC9
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.pauseAndBack };
			}
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x000C1AE0 File Offset: 0x000BFCE0
		protected override void OnRelease()
		{
			base.OnRelease();
			if (this.IsOpen())
			{
				NCapstoneContainer.Instance.Close();
			}
			else
			{
				NPauseMenu npauseMenu = (NPauseMenu)NRun.Instance.GlobalUi.SubmenuStack.ShowScreen(CapstoneSubmenuType.PauseMenu);
				npauseMenu.Initialize(this._runState);
			}
			base.UpdateScreenOpen();
			ShaderMaterial hsv = this._hsv;
			if (hsv == null)
			{
				return;
			}
			hsv.SetShaderParameter(NTopBarPauseButton._v, 0.9f);
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x000C1B54 File Offset: 0x000BFD54
		protected override bool IsOpen()
		{
			NCapstoneSubmenuStack ncapstoneSubmenuStack = NCapstoneContainer.Instance.CurrentCapstoneScreen as NCapstoneSubmenuStack;
			return ncapstoneSubmenuStack != null && ncapstoneSubmenuStack.ScreenType == NetScreenType.PauseMenu;
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x000C1B80 File Offset: 0x000BFD80
		public override void _Process(double delta)
		{
			if (base.IsScreenOpen)
			{
				this._icon.Rotation += (float)delta;
			}
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x000C1B9E File Offset: 0x000BFD9E
		[NullableContext(1)]
		public void Initialize(IRunState runState)
		{
			this._runState = runState;
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x000C1BA8 File Offset: 0x000BFDA8
		[NullableContext(1)]
		protected override async Task AnimPressDown(CancellationTokenSource cancelToken)
		{
			if (this._icon.IsValid())
			{
				float num = 0f;
				float startAngle = this._icon.Rotation;
				float targetAngle = startAngle + 0.7853982f;
				while (num < 0.25f)
				{
					this._icon.Rotation = Mathf.LerpAngle(startAngle, targetAngle, Ease.CubicOut(num / 0.25f));
					ShaderMaterial hsv = this._hsv;
					if (hsv != null)
					{
						hsv.SetShaderParameter(NTopBarPauseButton._v, Mathf.Lerp(1.1f, 0.4f, Ease.CubicOut(num / 0.25f)));
					}
					float num2 = num;
					float num3 = await this.AwaitProcessFrame(cancelToken.Token);
					num = num2 + num3;
				}
				this._icon.Rotation = targetAngle;
				ShaderMaterial hsv2 = this._hsv;
				if (hsv2 != null)
				{
					hsv2.SetShaderParameter(NTopBarPauseButton._v, 0.4f);
				}
			}
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x000C1BF4 File Offset: 0x000BFDF4
		[NullableContext(1)]
		protected override async Task AnimHover(CancellationTokenSource cancelToken)
		{
			if (this._icon.IsValid())
			{
				float num = 0f;
				float startAngle = this._icon.Rotation;
				while (num < 0.5f)
				{
					this._icon.Rotation = Mathf.LerpAngle(startAngle, -3.1415927f, Ease.BackOut(num / 0.5f, 1f));
					float num2 = num;
					float num3 = await this.AwaitProcessFrame(cancelToken.Token);
					num = num2 + num3;
				}
				this._icon.Rotation = -3.1415927f;
			}
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x000C1C40 File Offset: 0x000BFE40
		[NullableContext(1)]
		protected override async Task AnimUnhover(CancellationTokenSource cancelToken)
		{
			if (this._icon.IsValid())
			{
				float num = 0f;
				float startAngle = this._icon.Rotation;
				while (num < 1f)
				{
					this._icon.Rotation = Mathf.LerpAngle(startAngle, 0f, Ease.ElasticOut(num / 1f));
					ShaderMaterial hsv = this._hsv;
					if (hsv != null)
					{
						hsv.SetShaderParameter(NTopBarPauseButton._v, Mathf.Lerp(1.1f, 0.9f, Ease.ExpoOut(num / 1f)));
					}
					this._icon.Scale = NTopBarButton._hoverScale.Lerp(Vector2.One, Ease.ExpoOut(num / 1f));
					float num2 = num;
					float num3 = await this.AwaitProcessFrame(cancelToken.Token);
					num = num2 + num3;
				}
				ShaderMaterial hsv2 = this._hsv;
				if (hsv2 != null)
				{
					hsv2.SetShaderParameter(NTopBarPauseButton._v, 0.9f);
				}
				this._icon.Rotation = 0f;
				this._icon.Scale = Vector2.One;
			}
		}

		/// <summary>
		/// Toggles the anim state of this button.
		/// Utilized when an external UI (ie BackButton) closes this screen.
		/// </summary>
		// Token: 0x06001DC7 RID: 7623 RVA: 0x000C1C8B File Offset: 0x000BFE8B
		public void ToggleAnimState()
		{
			base.UpdateScreenOpen();
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x000C1C94 File Offset: 0x000BFE94
		protected override void OnFocus()
		{
			base.OnFocus();
			HoverTip hoverTip = new HoverTip(new LocString("static_hover_tips", "SETTINGS.title"), new LocString("static_hover_tips", "SETTINGS.description"), null);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + new Vector2(base.Size.X - nhoverTipSet.Size.X, base.Size.Y + 20f), false);
			}
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x000C1D1D File Offset: 0x000BFF1D
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
		// Token: 0x06001DCA RID: 7626 RVA: 0x000C1D2C File Offset: 0x000BFF2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NTopBarPauseButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarPauseButton.MethodName.IsOpen, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarPauseButton.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTopBarPauseButton.MethodName.ToggleAnimState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarPauseButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarPauseButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001DCB RID: 7627 RVA: 0x000C1E80 File Offset: 0x000C0080
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTopBarPauseButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarPauseButton.MethodName.IsOpen && args.Count == 0)
			{
				bool flag = this.IsOpen();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NTopBarPauseButton.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarPauseButton.MethodName.ToggleAnimState && args.Count == 0)
			{
				this.ToggleAnimState();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarPauseButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarPauseButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001DCC RID: 7628 RVA: 0x000C1F8C File Offset: 0x000C018C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTopBarPauseButton.MethodName.OnRelease || (ref method) == NTopBarPauseButton.MethodName.IsOpen || (ref method) == NTopBarPauseButton.MethodName._Process || (ref method) == NTopBarPauseButton.MethodName.ToggleAnimState || (ref method) == NTopBarPauseButton.MethodName.OnFocus || (ref method) == NTopBarPauseButton.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001DCD RID: 7629 RVA: 0x000C1FFC File Offset: 0x000C01FC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTopBarPauseButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001DCE RID: 7630 RVA: 0x000C2034 File Offset: 0x000C0234
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NTopBarPauseButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001DCF RID: 7631 RVA: 0x000C2069 File Offset: 0x000C0269
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06001DD0 RID: 7632 RVA: 0x000C2072 File Offset: 0x000C0272
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04000B72 RID: 2930
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000B73 RID: 2931
		private const float _hoverAngle = -3.1415927f;

		// Token: 0x04000B74 RID: 2932
		private const float _hoverShaderV = 1.1f;

		// Token: 0x04000B75 RID: 2933
		private const float _defaultV = 0.9f;

		// Token: 0x04000B76 RID: 2934
		private const float _pressDownV = 0.4f;

		// Token: 0x04000B77 RID: 2935
		[Nullable(1)]
		private IRunState _runState;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200124A RID: 4682
		public new class MethodName : NTopBarButton.MethodName
		{
			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x040046C7 RID: 18119
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'IsOpen' method.
			/// </summary>
			// Token: 0x040046C8 RID: 18120
			public new static readonly StringName IsOpen = "IsOpen";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040046C9 RID: 18121
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'ToggleAnimState' method.
			/// </summary>
			// Token: 0x040046CA RID: 18122
			public static readonly StringName ToggleAnimState = "ToggleAnimState";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040046CB RID: 18123
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040046CC RID: 18124
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200124B RID: 4683
		public new class PropertyName : NTopBarButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x040046CD RID: 18125
			public new static readonly StringName Hotkeys = "Hotkeys";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200124C RID: 4684
		public new class SignalName : NTopBarButton.SignalName
		{
		}
	}
}

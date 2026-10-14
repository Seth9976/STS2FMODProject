using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.sts2.Core.Nodes.TopBar
{
	// Token: 0x0200001A RID: 26
	[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarFloorIcon.cs")]
	public class NTopBarFloorIcon : NClickableControl
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x0000748C File Offset: 0x0000568C
		public override void _Ready()
		{
			this._floorNumLabel = base.GetNode<MegaLabel>("FloorNumLabel");
			this.ConnectSignals();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000074AA File Offset: 0x000056AA
		[NullableContext(1)]
		public void Initialize(IRunState runState)
		{
			this._runState = runState;
			this.UpdateIcon();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000074B9 File Offset: 0x000056B9
		public override void _EnterTree()
		{
			RunManager.Instance.RoomEntered += this.UpdateIcon;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000074D1 File Offset: 0x000056D1
		public override void _ExitTree()
		{
			RunManager.Instance.RoomEntered -= this.UpdateIcon;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000074EC File Offset: 0x000056EC
		private void UpdateIcon()
		{
			if (this._runState.CurrentRoom == null)
			{
				return;
			}
			this._floorNumLabel.SetTextAutoSize(this._runState.TotalFloor.ToString());
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00007528 File Offset: 0x00005728
		protected override void OnFocus()
		{
			HoverTip hoverTip = new HoverTip(new LocString("static_hover_tips", "FLOOR.title"), new LocString("static_hover_tips", "FLOOR.description"), null);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + new Vector2(0f, base.Size.Y + 20f), false);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00007599 File Offset: 0x00005799
		protected override void OnUnfocus()
		{
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060000CD RID: 205 RVA: 0x000075A4 File Offset: 0x000057A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NTopBarFloorIcon.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarFloorIcon.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarFloorIcon.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarFloorIcon.MethodName.UpdateIcon, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarFloorIcon.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarFloorIcon.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060000CE RID: 206 RVA: 0x000076D4 File Offset: 0x000058D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTopBarFloorIcon.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarFloorIcon.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarFloorIcon.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarFloorIcon.MethodName.UpdateIcon && args.Count == 0)
			{
				this.UpdateIcon();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarFloorIcon.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarFloorIcon.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060000CF RID: 207 RVA: 0x000077C8 File Offset: 0x000059C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTopBarFloorIcon.MethodName._Ready || (ref method) == NTopBarFloorIcon.MethodName._EnterTree || (ref method) == NTopBarFloorIcon.MethodName._ExitTree || (ref method) == NTopBarFloorIcon.MethodName.UpdateIcon || (ref method) == NTopBarFloorIcon.MethodName.OnFocus || (ref method) == NTopBarFloorIcon.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060000D0 RID: 208 RVA: 0x00007836 File Offset: 0x00005A36
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTopBarFloorIcon.PropertyName._floorNumLabel)
			{
				this._floorNumLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060000D1 RID: 209 RVA: 0x0000785B File Offset: 0x00005A5B
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTopBarFloorIcon.PropertyName._floorNumLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._floorNumLabel);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060000D2 RID: 210 RVA: 0x00007888 File Offset: 0x00005A88
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NTopBarFloorIcon.PropertyName._floorNumLabel, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060000D3 RID: 211 RVA: 0x000078BD File Offset: 0x00005ABD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTopBarFloorIcon.PropertyName._floorNumLabel, Variant.From<MegaLabel>(ref this._floorNumLabel));
		}

		/// <inheritdoc />
		// Token: 0x060000D4 RID: 212 RVA: 0x000078DC File Offset: 0x00005ADC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTopBarFloorIcon.PropertyName._floorNumLabel, ref variant))
			{
				this._floorNumLabel = variant.As<MegaLabel>();
			}
		}

		// Token: 0x04000052 RID: 82
		[Nullable(1)]
		private MegaLabel _floorNumLabel;

		// Token: 0x04000053 RID: 83
		[Nullable(1)]
		private IRunState _runState;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DDC RID: 3548
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040031B2 RID: 12722
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040031B3 RID: 12723
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040031B4 RID: 12724
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdateIcon' method.
			/// </summary>
			// Token: 0x040031B5 RID: 12725
			public static readonly StringName UpdateIcon = "UpdateIcon";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040031B6 RID: 12726
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040031B7 RID: 12727
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DDD RID: 3549
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the '_floorNumLabel' field.
			/// </summary>
			// Token: 0x040031B8 RID: 12728
			public static readonly StringName _floorNumLabel = "_floorNumLabel";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000DDE RID: 3550
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen
{
	// Token: 0x02000319 RID: 793
	[ScriptPath("res://src/Core/Nodes/Screens/GameOverScreen/NDiscoveredItem.cs")]
	public class NDiscoveredItem : NButton
	{
		// Token: 0x06002C16 RID: 11286 RVA: 0x00138D95 File Offset: 0x00136F95
		public override void _Ready()
		{
			this.ConnectSignals();
			this._selectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this._label = base.GetNode<MegaLabel>("%Label");
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x00138DC9 File Offset: 0x00136FC9
		public void SetHoverTip(HoverTip hoverTip)
		{
			this._hoverTip = hoverTip;
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x00138DD2 File Offset: 0x00136FD2
		[NullableContext(1)]
		public void SetText(string str)
		{
			this._label.SetTextAutoSize(str);
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x00138DE0 File Offset: 0x00136FE0
		protected override void OnFocus()
		{
			base.OnFocus();
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + NDiscoveredItem._hoverTipOffset, false);
			}
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x00138E38 File Offset: 0x00137038
		protected override void OnUnfocus()
		{
			NHoverTipSet.Remove(this);
			this._selectionReticle.OnDeselect();
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x00138E4B File Offset: 0x0013704B
		protected override void OnPress()
		{
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002C1C RID: 11292 RVA: 0x00138E50 File Offset: 0x00137050
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NDiscoveredItem.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDiscoveredItem.MethodName.SetText, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "str", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NDiscoveredItem.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDiscoveredItem.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NDiscoveredItem.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C1D RID: 11293 RVA: 0x00138F78 File Offset: 0x00137178
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NDiscoveredItem.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDiscoveredItem.MethodName.SetText && args.Count == 1)
			{
				this.SetText(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDiscoveredItem.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDiscoveredItem.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NDiscoveredItem.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002C1E RID: 11294 RVA: 0x00139058 File Offset: 0x00137258
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NDiscoveredItem.MethodName._Ready || (ref method) == NDiscoveredItem.MethodName.SetText || (ref method) == NDiscoveredItem.MethodName.OnFocus || (ref method) == NDiscoveredItem.MethodName.OnUnfocus || (ref method) == NDiscoveredItem.MethodName.OnPress || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002C1F RID: 11295 RVA: 0x001390B7 File Offset: 0x001372B7
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NDiscoveredItem.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			if ((ref name) == NDiscoveredItem.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002C20 RID: 11296 RVA: 0x001390F8 File Offset: 0x001372F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NDiscoveredItem.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			if ((ref name) == NDiscoveredItem.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002C21 RID: 11297 RVA: 0x00139150 File Offset: 0x00137350
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NDiscoveredItem.PropertyName._selectionReticle, 0L, "", 4096L, false),
				new PropertyInfo(24L, NDiscoveredItem.PropertyName._label, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C22 RID: 11298 RVA: 0x001391A6 File Offset: 0x001373A6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NDiscoveredItem.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
			info.AddProperty(NDiscoveredItem.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
		}

		/// <inheritdoc />
		// Token: 0x06002C23 RID: 11299 RVA: 0x001391DC File Offset: 0x001373DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NDiscoveredItem.PropertyName._selectionReticle, ref variant))
			{
				this._selectionReticle = variant.As<NSelectionReticle>();
			}
			Variant variant2;
			if (info.TryGetProperty(NDiscoveredItem.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaLabel>();
			}
		}

		// Token: 0x04001099 RID: 4249
		private HoverTip _hoverTip;

		// Token: 0x0400109A RID: 4250
		private static readonly Vector2 _hoverTipOffset = new Vector2(-76f, -200f);

		// Token: 0x0400109B RID: 4251
		[Nullable(1)]
		private NSelectionReticle _selectionReticle;

		// Token: 0x0400109C RID: 4252
		[Nullable(1)]
		private MegaLabel _label;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001547 RID: 5447
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005161 RID: 20833
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetText' method.
			/// </summary>
			// Token: 0x04005162 RID: 20834
			public static readonly StringName SetText = "SetText";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005163 RID: 20835
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005164 RID: 20836
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04005165 RID: 20837
			public new static readonly StringName OnPress = "OnPress";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001548 RID: 5448
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04005166 RID: 20838
			public static readonly StringName _selectionReticle = "_selectionReticle";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005167 RID: 20839
			public static readonly StringName _label = "_label";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001549 RID: 5449
		public new class SignalName : NButton.SignalName
		{
		}
	}
}

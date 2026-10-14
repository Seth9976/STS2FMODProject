using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen
{
	// Token: 0x02000324 RID: 804
	[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NSendFeedbackButton.cs")]
	public class NSendFeedbackButton : NButton
	{
		// Token: 0x06002CD8 RID: 11480 RVA: 0x00140363 File Offset: 0x0013E563
		public override void _Ready()
		{
			this.ConnectSignals();
			this._selectionReticle = base.GetNodeOrNull<NSelectionReticle>("SelectionReticle");
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x00140381 File Offset: 0x0013E581
		protected override void OnFocus()
		{
			base.Scale = Vector2.One * 1.2f;
			if (NControllerManager.Instance.IsUsingController)
			{
				NSelectionReticle selectionReticle = this._selectionReticle;
				if (selectionReticle == null)
				{
					return;
				}
				selectionReticle.OnSelect();
			}
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x001403B4 File Offset: 0x0013E5B4
		protected override void OnUnfocus()
		{
			NSelectionReticle selectionReticle = this._selectionReticle;
			if (selectionReticle != null)
			{
				selectionReticle.OnDeselect();
			}
			base.Scale = Vector2.One;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CDB RID: 11483 RVA: 0x001403D4 File Offset: 0x0013E5D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NSendFeedbackButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSendFeedbackButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CDC RID: 11484 RVA: 0x00140478 File Offset: 0x0013E678
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSendFeedbackButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSendFeedbackButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002CDD RID: 11485 RVA: 0x001404FD File Offset: 0x0013E6FD
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSendFeedbackButton.MethodName._Ready || (ref method) == NSendFeedbackButton.MethodName.OnFocus || (ref method) == NSendFeedbackButton.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002CDE RID: 11486 RVA: 0x00140533 File Offset: 0x0013E733
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSendFeedbackButton.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002CDF RID: 11487 RVA: 0x00140558 File Offset: 0x0013E758
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSendFeedbackButton.PropertyName._selectionReticle)
			{
				value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002CE0 RID: 11488 RVA: 0x00140584 File Offset: 0x0013E784
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSendFeedbackButton.PropertyName._selectionReticle, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002CE1 RID: 11489 RVA: 0x001405B9 File Offset: 0x0013E7B9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NSendFeedbackButton.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
		}

		/// <inheritdoc />
		// Token: 0x06002CE2 RID: 11490 RVA: 0x001405D8 File Offset: 0x0013E7D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSendFeedbackButton.PropertyName._selectionReticle, ref variant))
			{
				this._selectionReticle = variant.As<NSelectionReticle>();
			}
		}

		// Token: 0x04001107 RID: 4359
		[Nullable(2)]
		private NSelectionReticle _selectionReticle;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001575 RID: 5493
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005234 RID: 21044
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005235 RID: 21045
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005236 RID: 21046
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001576 RID: 5494
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x04005237 RID: 21047
			public static readonly StringName _selectionReticle = "_selectionReticle";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001577 RID: 5495
		public new class SignalName : NButton.SignalName
		{
		}
	}
}

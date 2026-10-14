using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen
{
	// Token: 0x020002D4 RID: 724
	[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NRunHistoryArrowButton.cs")]
	public class NRunHistoryArrowButton : NGoldArrowButton
	{
		/// <summary>
		/// Whether this arrow is facing left or right
		/// </summary>
		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x0600267B RID: 9851 RVA: 0x00105CE7 File Offset: 0x00103EE7
		// (set) Token: 0x0600267C RID: 9852 RVA: 0x00105CEF File Offset: 0x00103EEF
		public bool IsLeft
		{
			get
			{
				return this._isLeft;
			}
			set
			{
				if (this._isLeft == value)
				{
					return;
				}
				base.UnregisterHotkeys();
				this._isLeft = value;
				base.RegisterHotkeys();
				this._icon.FlipH = !this.IsLeft;
				base.UpdateControllerButton();
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x0600267D RID: 9853 RVA: 0x00105D28 File Offset: 0x00103F28
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { this._isLeft ? MegaInput.viewDeckAndTabLeft : MegaInput.viewExhaustPileAndTabRight };
			}
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x00105D4C File Offset: 0x00103F4C
		public override void _Ready()
		{
			base._Ready();
			this._icon.FlipH = !this.IsLeft;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600267F RID: 9855 RVA: 0x00105D68 File Offset: 0x00103F68
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NRunHistoryArrowButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002680 RID: 9856 RVA: 0x00105DAC File Offset: 0x00103FAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRunHistoryArrowButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002681 RID: 9857 RVA: 0x00105DDC File Offset: 0x00103FDC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRunHistoryArrowButton.MethodName._Ready || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002682 RID: 9858 RVA: 0x00105DF4 File Offset: 0x00103FF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRunHistoryArrowButton.PropertyName.IsLeft)
			{
				this.IsLeft = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NRunHistoryArrowButton.PropertyName._isLeft)
			{
				this._isLeft = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002683 RID: 9859 RVA: 0x00105E34 File Offset: 0x00104034
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRunHistoryArrowButton.PropertyName.IsLeft)
			{
				bool isLeft = this.IsLeft;
				value = VariantUtils.CreateFrom<bool>(ref isLeft);
				return true;
			}
			if ((ref name) == NRunHistoryArrowButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NRunHistoryArrowButton.PropertyName._isLeft)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isLeft);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002684 RID: 9860 RVA: 0x00105EB0 File Offset: 0x001040B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(1L, NRunHistoryArrowButton.PropertyName._isLeft, 0L, "", 4096L, false),
				new PropertyInfo(1L, NRunHistoryArrowButton.PropertyName.IsLeft, 0L, "", 4096L, false),
				new PropertyInfo(34L, NRunHistoryArrowButton.PropertyName.Hotkeys, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002685 RID: 9861 RVA: 0x00105F28 File Offset: 0x00104128
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isLeft = NRunHistoryArrowButton.PropertyName.IsLeft;
			bool isLeft2 = this.IsLeft;
			info.AddProperty(isLeft, Variant.From<bool>(ref isLeft2));
			info.AddProperty(NRunHistoryArrowButton.PropertyName._isLeft, Variant.From<bool>(ref this._isLeft));
		}

		/// <inheritdoc />
		// Token: 0x06002686 RID: 9862 RVA: 0x00105F6C File Offset: 0x0010416C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRunHistoryArrowButton.PropertyName.IsLeft, ref variant))
			{
				this.IsLeft = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRunHistoryArrowButton.PropertyName._isLeft, ref variant2))
			{
				this._isLeft = variant2.As<bool>();
			}
		}

		// Token: 0x04000E4B RID: 3659
		private bool _isLeft;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001446 RID: 5190
		public new class MethodName : NGoldArrowButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004CF8 RID: 19704
			public new static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001447 RID: 5191
		public new class PropertyName : NGoldArrowButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'IsLeft' property.
			/// </summary>
			// Token: 0x04004CF9 RID: 19705
			public static readonly StringName IsLeft = "IsLeft";

			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x04004CFA RID: 19706
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_isLeft' field.
			/// </summary>
			// Token: 0x04004CFB RID: 19707
			public static readonly StringName _isLeft = "_isLeft";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001448 RID: 5192
		public new class SignalName : NGoldArrowButton.SignalName
		{
		}
	}
}

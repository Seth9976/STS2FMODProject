using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen
{
	// Token: 0x02000275 RID: 629
	[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NShareStatsButton.cs")]
	public class NShareStatsButton : NButton
	{
		// Token: 0x06002142 RID: 8514 RVA: 0x000E2567 File Offset: 0x000E0767
		public override void _Ready()
		{
			this.ConnectSignals();
			this._image = base.GetNode<Control>("Visuals");
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x000E2585 File Offset: 0x000E0785
		protected override void OnRelease()
		{
			this._image.Scale = Vector2.One;
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x000E2597 File Offset: 0x000E0797
		protected override void OnFocus()
		{
			base.OnFocus();
			this._image.Scale = Vector2.One * 1.05f;
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000E25B9 File Offset: 0x000E07B9
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			this._image.Scale = Vector2.One;
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x000E25D1 File Offset: 0x000E07D1
		protected override void OnPress()
		{
			base.OnPress();
			this._image.Scale = Vector2.One * 0.95f;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002147 RID: 8519 RVA: 0x000E25F4 File Offset: 0x000E07F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NShareStatsButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShareStatsButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShareStatsButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShareStatsButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NShareStatsButton.MethodName.OnPress, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002148 RID: 8520 RVA: 0x000E26F4 File Offset: 0x000E08F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NShareStatsButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShareStatsButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShareStatsButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShareStatsButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NShareStatsButton.MethodName.OnPress && args.Count == 0)
			{
				this.OnPress();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06002149 RID: 8521 RVA: 0x000E27C4 File Offset: 0x000E09C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NShareStatsButton.MethodName._Ready || (ref method) == NShareStatsButton.MethodName.OnRelease || (ref method) == NShareStatsButton.MethodName.OnFocus || (ref method) == NShareStatsButton.MethodName.OnUnfocus || (ref method) == NShareStatsButton.MethodName.OnPress || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600214A RID: 8522 RVA: 0x000E2823 File Offset: 0x000E0A23
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NShareStatsButton.PropertyName._image)
			{
				this._image = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600214B RID: 8523 RVA: 0x000E2848 File Offset: 0x000E0A48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NShareStatsButton.PropertyName._image)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._image);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600214C RID: 8524 RVA: 0x000E2874 File Offset: 0x000E0A74
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NShareStatsButton.PropertyName._image, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600214D RID: 8525 RVA: 0x000E28A9 File Offset: 0x000E0AA9
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NShareStatsButton.PropertyName._image, Variant.From<Control>(ref this._image));
		}

		/// <inheritdoc />
		// Token: 0x0600214E RID: 8526 RVA: 0x000E28C8 File Offset: 0x000E0AC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NShareStatsButton.PropertyName._image, ref variant))
			{
				this._image = variant.As<Control>();
			}
		}

		// Token: 0x04000D07 RID: 3335
		[Nullable(1)]
		private Control _image;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001304 RID: 4868
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040049FC RID: 18940
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x040049FD RID: 18941
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040049FE RID: 18942
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040049FF RID: 18943
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'OnPress' method.
			/// </summary>
			// Token: 0x04004A00 RID: 18944
			public new static readonly StringName OnPress = "OnPress";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001305 RID: 4869
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_image' field.
			/// </summary>
			// Token: 0x04004A01 RID: 18945
			public static readonly StringName _image = "_image";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001306 RID: 4870
		public new class SignalName : NButton.SignalName
		{
		}
	}
}

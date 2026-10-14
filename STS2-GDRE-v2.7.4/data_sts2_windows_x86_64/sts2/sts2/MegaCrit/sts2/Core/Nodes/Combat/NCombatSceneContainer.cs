using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	/// <summary>
	/// Resizes BgContainer of the combat room so it fits well in all aspect ratios.
	/// TODO: Expand to adjust for enemy placement.
	/// </summary>
	// Token: 0x020003F5 RID: 1013
	[ScriptPath("res://src/Core/Nodes/Combat/NCombatSceneContainer.cs")]
	public class NCombatSceneContainer : Control
	{
		// Token: 0x06003E44 RID: 15940 RVA: 0x001DD6E8 File Offset: 0x001DB8E8
		public override void _Ready()
		{
			this._bgContainer = base.GetNode<Control>("%BgContainer");
			this._window = base.GetTree().Root;
			this._window.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x001DD740 File Offset: 0x001DB940
		private void OnWindowChange()
		{
			float num = (float)this._window.Size.X / (float)this._window.Size.Y;
			if (num < 1.7777778f && SaveManager.Instance.SettingsSave.AspectRatioSetting == AspectRatioSetting.Auto)
			{
				this._bgContainer.Scale = Vector2.One * Mathf.Max(MathHelper.Remap(num, 1.3333334f, 1.7777778f, 1.08f, 0.9f), 1.08f);
				return;
			}
			this._bgContainer.Scale = Vector2.One * 0.9f;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003E46 RID: 15942 RVA: 0x001DD7E0 File Offset: 0x001DB9E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCombatSceneContainer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatSceneContainer.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003E47 RID: 15943 RVA: 0x001DD854 File Offset: 0x001DBA54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatSceneContainer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatSceneContainer.MethodName.OnWindowChange && args.Count == 0)
			{
				this.OnWindowChange();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003E48 RID: 15944 RVA: 0x001DD8B4 File Offset: 0x001DBAB4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatSceneContainer.MethodName._Ready || (ref method) == NCombatSceneContainer.MethodName.OnWindowChange || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003E49 RID: 15945 RVA: 0x001DD8DB File Offset: 0x001DBADB
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCombatSceneContainer.PropertyName._window)
			{
				this._window = VariantUtils.ConvertTo<Window>(ref value);
				return true;
			}
			if ((ref name) == NCombatSceneContainer.PropertyName._bgContainer)
			{
				this._bgContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003E4A RID: 15946 RVA: 0x001DD91C File Offset: 0x001DBB1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCombatSceneContainer.PropertyName._window)
			{
				value = VariantUtils.CreateFrom<Window>(ref this._window);
				return true;
			}
			if ((ref name) == NCombatSceneContainer.PropertyName._bgContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._bgContainer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003E4B RID: 15947 RVA: 0x001DD974 File Offset: 0x001DBB74
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCombatSceneContainer.PropertyName._window, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatSceneContainer.PropertyName._bgContainer, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003E4C RID: 15948 RVA: 0x001DD9CA File Offset: 0x001DBBCA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCombatSceneContainer.PropertyName._window, Variant.From<Window>(ref this._window));
			info.AddProperty(NCombatSceneContainer.PropertyName._bgContainer, Variant.From<Control>(ref this._bgContainer));
		}

		/// <inheritdoc />
		// Token: 0x06003E4D RID: 15949 RVA: 0x001DDA00 File Offset: 0x001DBC00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCombatSceneContainer.PropertyName._window, ref variant))
			{
				this._window = variant.As<Window>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCombatSceneContainer.PropertyName._bgContainer, ref variant2))
			{
				this._bgContainer = variant2.As<Control>();
			}
		}

		// Token: 0x04001836 RID: 6198
		private const float _sixteenByNine = 1.7777778f;

		// Token: 0x04001837 RID: 6199
		private const float _maxNarrowRatio = 1.3333334f;

		// Token: 0x04001838 RID: 6200
		[Nullable(1)]
		private Window _window;

		// Token: 0x04001839 RID: 6201
		[Nullable(1)]
		private Control _bgContainer;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018AE RID: 6318
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006077 RID: 24695
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04006078 RID: 24696
			public static readonly StringName OnWindowChange = "OnWindowChange";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018AF RID: 6319
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_window' field.
			/// </summary>
			// Token: 0x04006079 RID: 24697
			public static readonly StringName _window = "_window";

			/// <summary>
			/// Cached name for the '_bgContainer' field.
			/// </summary>
			// Token: 0x0400607A RID: 24698
			public static readonly StringName _bgContainer = "_bgContainer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020018B0 RID: 6320
		public class SignalName : Control.SignalName
		{
		}
	}
}

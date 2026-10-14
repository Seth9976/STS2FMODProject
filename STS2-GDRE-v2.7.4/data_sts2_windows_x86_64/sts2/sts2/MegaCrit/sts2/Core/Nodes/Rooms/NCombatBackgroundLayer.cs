using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Rooms
{
	// Token: 0x02000369 RID: 873
	[ScriptPath("res://src/Core/Nodes/Rooms/NCombatBackgroundLayer.cs")]
	public class NCombatBackgroundLayer : Control
	{
		// Token: 0x060032C1 RID: 12993 RVA: 0x00179DE6 File Offset: 0x00177FE6
		public override void _Ready()
		{
			this._visual = base.GetNode<Control>("Visual");
			this._phobiaModeVisual = base.GetNode<Control>("PhobiaModeVisual");
			this.UpdatePhobiaMode();
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x00179E1A File Offset: 0x0017801A
		public override void _EnterTree()
		{
			NGame instance = NGame.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Connect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)), 0U);
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x00179E43 File Offset: 0x00178043
		public override void _ExitTree()
		{
			NGame instance = NGame.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Disconnect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)));
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x00179E6A File Offset: 0x0017806A
		private void UpdatePhobiaMode()
		{
			this._phobiaModeVisual.Visible = SaveManager.Instance.PrefsSave.PhobiaMode;
			this._visual.Visible = !this._phobiaModeVisual.Visible;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060032C5 RID: 12997 RVA: 0x00179EA0 File Offset: 0x001780A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NCombatBackgroundLayer.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatBackgroundLayer.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatBackgroundLayer.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCombatBackgroundLayer.MethodName.UpdatePhobiaMode, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060032C6 RID: 12998 RVA: 0x00179F74 File Offset: 0x00178174
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatBackgroundLayer.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatBackgroundLayer.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatBackgroundLayer.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatBackgroundLayer.MethodName.UpdatePhobiaMode && args.Count == 0)
			{
				this.UpdatePhobiaMode();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060032C7 RID: 12999 RVA: 0x0017A020 File Offset: 0x00178220
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatBackgroundLayer.MethodName._Ready || (ref method) == NCombatBackgroundLayer.MethodName._EnterTree || (ref method) == NCombatBackgroundLayer.MethodName._ExitTree || (ref method) == NCombatBackgroundLayer.MethodName.UpdatePhobiaMode || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060032C8 RID: 13000 RVA: 0x0017A070 File Offset: 0x00178270
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCombatBackgroundLayer.PropertyName._visual)
			{
				this._visual = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCombatBackgroundLayer.PropertyName._phobiaModeVisual)
			{
				this._phobiaModeVisual = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060032C9 RID: 13001 RVA: 0x0017A0B0 File Offset: 0x001782B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCombatBackgroundLayer.PropertyName._visual)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._visual);
				return true;
			}
			if ((ref name) == NCombatBackgroundLayer.PropertyName._phobiaModeVisual)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._phobiaModeVisual);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060032CA RID: 13002 RVA: 0x0017A108 File Offset: 0x00178308
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCombatBackgroundLayer.PropertyName._visual, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCombatBackgroundLayer.PropertyName._phobiaModeVisual, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060032CB RID: 13003 RVA: 0x0017A15E File Offset: 0x0017835E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCombatBackgroundLayer.PropertyName._visual, Variant.From<Control>(ref this._visual));
			info.AddProperty(NCombatBackgroundLayer.PropertyName._phobiaModeVisual, Variant.From<Control>(ref this._phobiaModeVisual));
		}

		/// <inheritdoc />
		// Token: 0x060032CC RID: 13004 RVA: 0x0017A194 File Offset: 0x00178394
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCombatBackgroundLayer.PropertyName._visual, ref variant))
			{
				this._visual = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCombatBackgroundLayer.PropertyName._phobiaModeVisual, ref variant2))
			{
				this._phobiaModeVisual = variant2.As<Control>();
			}
		}

		// Token: 0x040013B9 RID: 5049
		[Nullable(1)]
		private Control _visual;

		// Token: 0x040013BA RID: 5050
		[Nullable(1)]
		private Control _phobiaModeVisual;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200167A RID: 5754
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005791 RID: 22417
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005792 RID: 22418
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005793 RID: 22419
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'UpdatePhobiaMode' method.
			/// </summary>
			// Token: 0x04005794 RID: 22420
			public static readonly StringName UpdatePhobiaMode = "UpdatePhobiaMode";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200167B RID: 5755
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_visual' field.
			/// </summary>
			// Token: 0x04005795 RID: 22421
			public static readonly StringName _visual = "_visual";

			/// <summary>
			/// Cached name for the '_phobiaModeVisual' field.
			/// </summary>
			// Token: 0x04005796 RID: 22422
			public static readonly StringName _phobiaModeVisual = "_phobiaModeVisual";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200167C RID: 5756
		public class SignalName : Control.SignalName
		{
		}
	}
}

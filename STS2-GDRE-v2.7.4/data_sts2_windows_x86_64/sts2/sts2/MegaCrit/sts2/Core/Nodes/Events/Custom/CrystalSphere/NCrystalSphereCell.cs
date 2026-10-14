using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere
{
	// Token: 0x020003C0 RID: 960
	[ScriptPath("res://src/Core/Nodes/Events/Custom/CrystalSphere/NCrystalSphereCell.cs")]
	public class NCrystalSphereCell : NClickableControl
	{
		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06003A01 RID: 14849 RVA: 0x001B8C79 File Offset: 0x001B6E79
		// (set) Token: 0x06003A02 RID: 14850 RVA: 0x001B8C81 File Offset: 0x001B6E81
		[Nullable(1)]
		public CrystalSphereCell Entity
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			private set;
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x001B8C8C File Offset: 0x001B6E8C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NCrystalSphereCell Create(CrystalSphereCell cell, NCrystalSphereMask mask)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCrystalSphereCell ncrystalSphereCell = PreloadManager.Cache.GetScene("res://scenes/events/custom/crystal_sphere/crystal_sphere_cell.tscn").Instantiate<NCrystalSphereCell>(0L);
			ncrystalSphereCell.Entity = cell;
			ncrystalSphereCell._mask = mask;
			return ncrystalSphereCell;
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x001B8CC8 File Offset: 0x001B6EC8
		public override void _Ready()
		{
			this._hoveredFg = base.GetNode<Control>("HoveredFg");
			base.Modulate = Colors.Transparent;
			base.MouseFilter = (this.Entity.IsHidden ? 0L : 2L);
			base.FocusMode = (this.Entity.IsHidden ? 2L : 0L);
			this._hoveredFg.Visible = false;
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x001B8D34 File Offset: 0x001B6F34
		public override void _EnterTree()
		{
			this.Entity.HighlightUpdated += this.OnEntityHighlightUpdated;
			this.Entity.FogUpdated += this.EntityClicked;
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x001B8D64 File Offset: 0x001B6F64
		public override void _ExitTree()
		{
			this.Entity.HighlightUpdated -= this.OnEntityHighlightUpdated;
			this.Entity.FogUpdated -= this.EntityClicked;
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x001B8D94 File Offset: 0x001B6F94
		private void OnEntityHighlightUpdated()
		{
			Tween fadeTween = this._fadeTween;
			if (fadeTween != null)
			{
				fadeTween.Kill();
			}
			this._fadeTween = base.CreateTween();
			Tween fadeTween2 = this._fadeTween;
			NodePath nodePath = "modulate";
			CrystalSphereCell entity = this.Entity;
			fadeTween2.TweenProperty(this, nodePath, (entity != null && entity.IsHighlighted && entity.IsHidden) ? Colors.White : Colors.Transparent, 0.15000000596046448);
			this._hoveredFg.Visible = this.Entity.IsHovered;
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x001B8E20 File Offset: 0x001B7020
		private void EntityClicked()
		{
			base.MouseFilter = (this.Entity.IsHidden ? 0L : 2L);
			base.FocusMode = (this.Entity.IsHidden ? 2L : 0L);
			this._mask.UpdateMat(this.Entity);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A09 RID: 14857 RVA: 0x001B8E70 File Offset: 0x001B7070
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NCrystalSphereCell.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereCell.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereCell.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereCell.MethodName.OnEntityHighlightUpdated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereCell.MethodName.EntityClicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A0A RID: 14858 RVA: 0x001B8F70 File Offset: 0x001B7170
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCrystalSphereCell.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereCell.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereCell.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereCell.MethodName.OnEntityHighlightUpdated && args.Count == 0)
			{
				this.OnEntityHighlightUpdated();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereCell.MethodName.EntityClicked && args.Count == 0)
			{
				this.EntityClicked();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003A0B RID: 14859 RVA: 0x001B9040 File Offset: 0x001B7240
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCrystalSphereCell.MethodName._Ready || (ref method) == NCrystalSphereCell.MethodName._EnterTree || (ref method) == NCrystalSphereCell.MethodName._ExitTree || (ref method) == NCrystalSphereCell.MethodName.OnEntityHighlightUpdated || (ref method) == NCrystalSphereCell.MethodName.EntityClicked || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003A0C RID: 14860 RVA: 0x001B90A0 File Offset: 0x001B72A0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCrystalSphereCell.PropertyName._mask)
			{
				this._mask = VariantUtils.ConvertTo<NCrystalSphereMask>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereCell.PropertyName._hoveredFg)
			{
				this._hoveredFg = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereCell.PropertyName._fadeTween)
			{
				this._fadeTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06003A0D RID: 14861 RVA: 0x001B9108 File Offset: 0x001B7308
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCrystalSphereCell.PropertyName._mask)
			{
				value = VariantUtils.CreateFrom<NCrystalSphereMask>(ref this._mask);
				return true;
			}
			if ((ref name) == NCrystalSphereCell.PropertyName._hoveredFg)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._hoveredFg);
				return true;
			}
			if ((ref name) == NCrystalSphereCell.PropertyName._fadeTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._fadeTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A0E RID: 14862 RVA: 0x001B9180 File Offset: 0x001B7380
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCrystalSphereCell.PropertyName._mask, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereCell.PropertyName._hoveredFg, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereCell.PropertyName._fadeTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A0F RID: 14863 RVA: 0x001B91F8 File Offset: 0x001B73F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCrystalSphereCell.PropertyName._mask, Variant.From<NCrystalSphereMask>(ref this._mask));
			info.AddProperty(NCrystalSphereCell.PropertyName._hoveredFg, Variant.From<Control>(ref this._hoveredFg));
			info.AddProperty(NCrystalSphereCell.PropertyName._fadeTween, Variant.From<Tween>(ref this._fadeTween));
		}

		/// <inheritdoc />
		// Token: 0x06003A10 RID: 14864 RVA: 0x001B9250 File Offset: 0x001B7450
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCrystalSphereCell.PropertyName._mask, ref variant))
			{
				this._mask = variant.As<NCrystalSphereMask>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCrystalSphereCell.PropertyName._hoveredFg, ref variant2))
			{
				this._hoveredFg = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCrystalSphereCell.PropertyName._fadeTween, ref variant3))
			{
				this._fadeTween = variant3.As<Tween>();
			}
		}

		// Token: 0x04001698 RID: 5784
		[Nullable(1)]
		private const string _scenePath = "res://scenes/events/custom/crystal_sphere/crystal_sphere_cell.tscn";

		// Token: 0x0400169A RID: 5786
		[Nullable(1)]
		private NCrystalSphereMask _mask;

		// Token: 0x0400169B RID: 5787
		[Nullable(1)]
		private Control _hoveredFg;

		// Token: 0x0400169C RID: 5788
		[Nullable(2)]
		private Tween _fadeTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017E1 RID: 6113
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005D3D RID: 23869
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005D3E RID: 23870
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005D3F RID: 23871
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnEntityHighlightUpdated' method.
			/// </summary>
			// Token: 0x04005D40 RID: 23872
			public static readonly StringName OnEntityHighlightUpdated = "OnEntityHighlightUpdated";

			/// <summary>
			/// Cached name for the 'EntityClicked' method.
			/// </summary>
			// Token: 0x04005D41 RID: 23873
			public static readonly StringName EntityClicked = "EntityClicked";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017E2 RID: 6114
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the '_mask' field.
			/// </summary>
			// Token: 0x04005D42 RID: 23874
			public static readonly StringName _mask = "_mask";

			/// <summary>
			/// Cached name for the '_hoveredFg' field.
			/// </summary>
			// Token: 0x04005D43 RID: 23875
			public static readonly StringName _hoveredFg = "_hoveredFg";

			/// <summary>
			/// Cached name for the '_fadeTween' field.
			/// </summary>
			// Token: 0x04005D44 RID: 23876
			public static readonly StringName _fadeTween = "_fadeTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017E3 RID: 6115
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}

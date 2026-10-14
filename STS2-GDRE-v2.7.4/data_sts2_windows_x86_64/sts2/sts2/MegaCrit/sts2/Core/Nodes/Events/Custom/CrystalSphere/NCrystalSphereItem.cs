using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereItems;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere
{
	// Token: 0x020003C2 RID: 962
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Events/Custom/CrystalSphere/NCrystalSphereItem.cs")]
	public class NCrystalSphereItem : Control
	{
		// Token: 0x06003A22 RID: 14882 RVA: 0x001B9CF8 File Offset: 0x001B7EF8
		[return: Nullable(2)]
		public static NCrystalSphereItem Create(CrystalSphereItem item)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCrystalSphereItem ncrystalSphereItem = PreloadManager.Cache.GetScene("res://scenes/events/custom/crystal_sphere/crystal_sphere_item.tscn").Instantiate<NCrystalSphereItem>(0L);
			ncrystalSphereItem._item = item;
			return ncrystalSphereItem;
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x001B9D30 File Offset: 0x001B7F30
		public override void _Ready()
		{
			this._icon = base.GetNode<TextureRect>("Icon");
			this._card = base.GetNode<Control>("%Card");
			this._cardFrame = base.GetNode<TextureRect>("%CardFrame");
			this._cardBanner = base.GetNode<TextureRect>("%CardBanner");
			base.PivotOffset = base.Size / 2f;
			this._icon.PivotOffset = base.Size / 2f;
			this._card.PivotOffset = base.Size / 2f;
			this._material = this._icon.Material;
			CrystalSphereCardReward crystalSphereCardReward = this._item as CrystalSphereCardReward;
			if (crystalSphereCardReward != null)
			{
				this._card.Visible = true;
				this._icon.Visible = false;
				this._cardBanner.Material = crystalSphereCardReward.BannerMaterial;
				this._cardFrame.Material = crystalSphereCardReward.FrameMaterial;
				return;
			}
			this._card.Visible = false;
			this._icon.Visible = true;
			this._icon.Texture = this._item.Texture;
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x001B9E6A File Offset: 0x001B806A
		public override void _EnterTree()
		{
			this._item.Revealed += this.OnRevealed;
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x001B9E83 File Offset: 0x001B8083
		public override void _ExitTree()
		{
			this._item.Revealed -= this.OnRevealed;
			if (this._tween != null && this._tween.IsRunning())
			{
				this._tween.Kill();
			}
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x001B9EBC File Offset: 0x001B80BC
		private void OnRevealed(CrystalSphereItem item)
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenInterval(0.25);
			this._tween.Chain().TweenProperty(this._material, "shader_parameter/val", 1f, 0.5).From(0);
			this._tween.Parallel().TweenProperty(this._icon, "scale", Vector2.One * 1.2f, 0.15000000596046448);
			this._tween.Parallel().TweenProperty(this._icon, "scale", Vector2.One, 0.5).SetDelay(0.15000000596046448);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A27 RID: 14887 RVA: 0x001B9FBC File Offset: 0x001B81BC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NCrystalSphereItem.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereItem.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereItem.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A28 RID: 14888 RVA: 0x001BA060 File Offset: 0x001B8260
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCrystalSphereItem.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereItem.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereItem.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003A29 RID: 14889 RVA: 0x001BA0E5 File Offset: 0x001B82E5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCrystalSphereItem.MethodName._Ready || (ref method) == NCrystalSphereItem.MethodName._EnterTree || (ref method) == NCrystalSphereItem.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003A2A RID: 14890 RVA: 0x001BA11C File Offset: 0x001B831C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCrystalSphereItem.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereItem.PropertyName._material)
			{
				this._material = VariantUtils.ConvertTo<Material>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereItem.PropertyName._card)
			{
				this._card = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereItem.PropertyName._cardFrame)
			{
				this._cardFrame = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereItem.PropertyName._cardBanner)
			{
				this._cardBanner = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereItem.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003A2B RID: 14891 RVA: 0x001BA1D4 File Offset: 0x001B83D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCrystalSphereItem.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NCrystalSphereItem.PropertyName._material)
			{
				value = VariantUtils.CreateFrom<Material>(ref this._material);
				return true;
			}
			if ((ref name) == NCrystalSphereItem.PropertyName._card)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._card);
				return true;
			}
			if ((ref name) == NCrystalSphereItem.PropertyName._cardFrame)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._cardFrame);
				return true;
			}
			if ((ref name) == NCrystalSphereItem.PropertyName._cardBanner)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._cardBanner);
				return true;
			}
			if ((ref name) == NCrystalSphereItem.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A2C RID: 14892 RVA: 0x001BA2AC File Offset: 0x001B84AC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCrystalSphereItem.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereItem.PropertyName._material, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereItem.PropertyName._card, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereItem.PropertyName._cardFrame, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereItem.PropertyName._cardBanner, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereItem.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A2D RID: 14893 RVA: 0x001BA388 File Offset: 0x001B8588
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCrystalSphereItem.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NCrystalSphereItem.PropertyName._material, Variant.From<Material>(ref this._material));
			info.AddProperty(NCrystalSphereItem.PropertyName._card, Variant.From<Control>(ref this._card));
			info.AddProperty(NCrystalSphereItem.PropertyName._cardFrame, Variant.From<TextureRect>(ref this._cardFrame));
			info.AddProperty(NCrystalSphereItem.PropertyName._cardBanner, Variant.From<TextureRect>(ref this._cardBanner));
			info.AddProperty(NCrystalSphereItem.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06003A2E RID: 14894 RVA: 0x001BA420 File Offset: 0x001B8620
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCrystalSphereItem.PropertyName._icon, ref variant))
			{
				this._icon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCrystalSphereItem.PropertyName._material, ref variant2))
			{
				this._material = variant2.As<Material>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCrystalSphereItem.PropertyName._card, ref variant3))
			{
				this._card = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCrystalSphereItem.PropertyName._cardFrame, ref variant4))
			{
				this._cardFrame = variant4.As<TextureRect>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCrystalSphereItem.PropertyName._cardBanner, ref variant5))
			{
				this._cardBanner = variant5.As<TextureRect>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCrystalSphereItem.PropertyName._tween, ref variant6))
			{
				this._tween = variant6.As<Tween>();
			}
		}

		// Token: 0x040016AA RID: 5802
		public const string scenePath = "res://scenes/events/custom/crystal_sphere/crystal_sphere_item.tscn";

		// Token: 0x040016AB RID: 5803
		private CrystalSphereItem _item;

		// Token: 0x040016AC RID: 5804
		private TextureRect _icon;

		// Token: 0x040016AD RID: 5805
		private Material _material;

		// Token: 0x040016AE RID: 5806
		private Control _card;

		// Token: 0x040016AF RID: 5807
		private TextureRect _cardFrame;

		// Token: 0x040016B0 RID: 5808
		private TextureRect _cardBanner;

		// Token: 0x040016B1 RID: 5809
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017E7 RID: 6119
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005D4F RID: 23887
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005D50 RID: 23888
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005D51 RID: 23889
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017E8 RID: 6120
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04005D52 RID: 23890
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_material' field.
			/// </summary>
			// Token: 0x04005D53 RID: 23891
			public static readonly StringName _material = "_material";

			/// <summary>
			/// Cached name for the '_card' field.
			/// </summary>
			// Token: 0x04005D54 RID: 23892
			public static readonly StringName _card = "_card";

			/// <summary>
			/// Cached name for the '_cardFrame' field.
			/// </summary>
			// Token: 0x04005D55 RID: 23893
			public static readonly StringName _cardFrame = "_cardFrame";

			/// <summary>
			/// Cached name for the '_cardBanner' field.
			/// </summary>
			// Token: 0x04005D56 RID: 23894
			public static readonly StringName _cardBanner = "_cardBanner";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005D57 RID: 23895
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017E9 RID: 6121
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}

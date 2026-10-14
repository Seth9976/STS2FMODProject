using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen
{
	/// <summary>
	/// Badge visual which appears on the game over screen.
	/// Can be hovered to view its name and description.
	/// </summary>
	// Token: 0x02000318 RID: 792
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/GameOverScreen/NBadge.cs")]
	public class NBadge : NButton
	{
		/// <summary>
		/// This creates an NBadge for the game over screen.
		/// </summary>
		// Token: 0x06002C02 RID: 11266 RVA: 0x00138260 File Offset: 0x00136460
		[return: Nullable(2)]
		public static NBadge Create(Badge badgeModel)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NBadge nbadge = PreloadManager.Cache.GetScene(NBadge._scenePath).Instantiate<NBadge>(0L);
			nbadge.GetNode<TextureRect>("%BadgeHolder").Texture = badgeModel.BadgeBase;
			nbadge.GetNode<TextureRect>("%Icon").Texture = badgeModel.BadgeIcon;
			if (LocString.Exists("badges", badgeModel.Id + "." + NBadge.GetRarityPrefix(badgeModel.Rarity) + "Title"))
			{
				nbadge._title = new LocString("badges", badgeModel.Id + "." + NBadge.GetRarityPrefix(badgeModel.Rarity) + "Title");
				nbadge._description = new LocString("badges", badgeModel.Id + "." + NBadge.GetRarityPrefix(badgeModel.Rarity) + "Description");
			}
			else
			{
				nbadge._title = new LocString("badges", badgeModel.Id + ".title");
				nbadge._description = new LocString("badges", badgeModel.Id + ".description");
			}
			return nbadge;
		}

		/// <summary>
		/// This creates an NBadge for the run history screen :).
		/// TODO: And the statistics screen later!
		/// </summary>
		// Token: 0x06002C03 RID: 11267 RVA: 0x00138394 File Offset: 0x00136594
		[return: Nullable(2)]
		public static NBadge Create(string id, BadgeRarity rarity)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NBadge nbadge = PreloadManager.Cache.GetScene(NBadge._scenePath).Instantiate<NBadge>(0L);
			nbadge.GetNode<TextureRect>("%BadgeHolder").Texture = NBadge.GetBadgeBaseTexture(rarity);
			nbadge.GetNode<TextureRect>("%Icon").Texture = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_" + id.ToLowerInvariant() + ".png"));
			if (LocString.Exists("badges", id + "." + NBadge.GetRarityPrefix(rarity) + "Title"))
			{
				nbadge._title = new LocString("badges", id + "." + NBadge.GetRarityPrefix(rarity) + "Title");
				nbadge._description = new LocString("badges", id + "." + NBadge.GetRarityPrefix(rarity) + "Description");
			}
			else
			{
				nbadge._title = new LocString("badges", id + ".title");
				nbadge._description = new LocString("badges", id + ".description");
			}
			return nbadge;
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x001384BC File Offset: 0x001366BC
		public override void _Ready()
		{
			this.ConnectSignals();
			this._hoverNode = this;
			this._selectionReticle = base.GetNode<NSelectionReticle>("%SelectionReticle");
			this._hoverTip = new HoverTip(this._title, this._description, null);
		}

		/// <summary>
		/// Maps badge rarity enums to prefix used for LocStrings
		/// </summary>
		// Token: 0x06002C05 RID: 11269 RVA: 0x001384FC File Offset: 0x001366FC
		private static string GetRarityPrefix(BadgeRarity rarity)
		{
			string text;
			switch (rarity)
			{
			case BadgeRarity.Bronze:
				text = "bronze";
				break;
			case BadgeRarity.Silver:
				text = "silver";
				break;
			case BadgeRarity.Gold:
				text = "gold";
				break;
			default:
				text = "ERROR";
				break;
			}
			return text;
		}

		/// <summary>
		/// 0.4 second animation of a Badge animating in.
		/// Deferred due to how GridContainer lays out its elements after AddChildSafely().
		/// </summary>
		// Token: 0x06002C06 RID: 11270 RVA: 0x00138540 File Offset: 0x00136740
		public async Task AnimateIn()
		{
			if (this.IsValid())
			{
				this._tween = base.CreateTween().SetParallel(true);
				this._tween.TweenProperty(this, "modulate:a", 1f, 0.25);
				this._tween.TweenProperty(this, "position:y", base.Position.Y, 0.25).SetEase(1L).SetTrans(11L)
					.From(base.Position.Y + 40f);
				bool flag = await this._tween.AwaitFinished(this);
			}
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x00138583 File Offset: 0x00136783
		public override void _ExitTree()
		{
			base._ExitTree();
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x0013859C File Offset: 0x0013679C
		protected override void OnFocus()
		{
			base.OnFocus();
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this._hoverNode, this._hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(this._hoverNode.GlobalPosition + new Vector2(10f, -132f), false);
			}
			if (NControllerManager.Instance.IsUsingController)
			{
				this._selectionReticle.OnSelect();
			}
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x00138607 File Offset: 0x00136807
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			NHoverTipSet.Remove(this._hoverNode);
			this._selectionReticle.OnDeselect();
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x00138628 File Offset: 0x00136828
		private static Texture2D GetBadgeBaseTexture(BadgeRarity rarity)
		{
			Texture2D texture2D;
			switch (rarity)
			{
			case BadgeRarity.Bronze:
				texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_bronze.png"));
				break;
			case BadgeRarity.Silver:
				texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_silver.png"));
				break;
			case BadgeRarity.Gold:
				texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_gold.png"));
				break;
			default:
				texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/power_atlas.sprites/missing_power.tres"));
				break;
			}
			return texture2D;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002C0B RID: 11275 RVA: 0x001386A8 File Offset: 0x001368A8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NBadge.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "id", 0L, "", 6L, false),
					new PropertyInfo(2L, "rarity", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBadge.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBadge.MethodName.GetRarityPrefix, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "rarity", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBadge.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBadge.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBadge.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBadge.MethodName.GetBadgeBaseTexture, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Texture2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "rarity", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C0C RID: 11276 RVA: 0x001388B0 File Offset: 0x00136AB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBadge.MethodName.Create && args.Count == 2)
			{
				NBadge nbadge = NBadge.Create(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<BadgeRarity>(args[1]));
				ret = VariantUtils.CreateFrom<NBadge>(ref nbadge);
				return true;
			}
			if ((ref method) == NBadge.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBadge.MethodName.GetRarityPrefix && args.Count == 1)
			{
				string rarityPrefix = NBadge.GetRarityPrefix(VariantUtils.ConvertTo<BadgeRarity>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref rarityPrefix);
				return true;
			}
			if ((ref method) == NBadge.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBadge.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBadge.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBadge.MethodName.GetBadgeBaseTexture && args.Count == 1)
			{
				Texture2D badgeBaseTexture = NBadge.GetBadgeBaseTexture(VariantUtils.ConvertTo<BadgeRarity>(args[0]));
				ret = VariantUtils.CreateFrom<Texture2D>(ref badgeBaseTexture);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x00138A14 File Offset: 0x00136C14
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBadge.MethodName.Create && args.Count == 2)
			{
				NBadge nbadge = NBadge.Create(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<BadgeRarity>(args[1]));
				ret = VariantUtils.CreateFrom<NBadge>(ref nbadge);
				return true;
			}
			if ((ref method) == NBadge.MethodName.GetRarityPrefix && args.Count == 1)
			{
				string rarityPrefix = NBadge.GetRarityPrefix(VariantUtils.ConvertTo<BadgeRarity>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref rarityPrefix);
				return true;
			}
			if ((ref method) == NBadge.MethodName.GetBadgeBaseTexture && args.Count == 1)
			{
				Texture2D badgeBaseTexture = NBadge.GetBadgeBaseTexture(VariantUtils.ConvertTo<BadgeRarity>(args[0]));
				ret = VariantUtils.CreateFrom<Texture2D>(ref badgeBaseTexture);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002C0E RID: 11278 RVA: 0x00138AE4 File Offset: 0x00136CE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBadge.MethodName.Create || (ref method) == NBadge.MethodName._Ready || (ref method) == NBadge.MethodName.GetRarityPrefix || (ref method) == NBadge.MethodName._ExitTree || (ref method) == NBadge.MethodName.OnFocus || (ref method) == NBadge.MethodName.OnUnfocus || (ref method) == NBadge.MethodName.GetBadgeBaseTexture || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002C0F RID: 11279 RVA: 0x00138B64 File Offset: 0x00136D64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBadge.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NBadge.PropertyName._hoverNode)
			{
				this._hoverNode = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBadge.PropertyName._selectionReticle)
			{
				this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002C10 RID: 11280 RVA: 0x00138BCC File Offset: 0x00136DCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBadge.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NBadge.PropertyName._hoverNode)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._hoverNode);
				return true;
			}
			if ((ref name) == NBadge.PropertyName._selectionReticle)
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
		// Token: 0x06002C11 RID: 11281 RVA: 0x00138C44 File Offset: 0x00136E44
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NBadge.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBadge.PropertyName._hoverNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBadge.PropertyName._selectionReticle, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C12 RID: 11282 RVA: 0x00138CBC File Offset: 0x00136EBC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NBadge.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NBadge.PropertyName._hoverNode, Variant.From<Control>(ref this._hoverNode));
			info.AddProperty(NBadge.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
		}

		/// <inheritdoc />
		// Token: 0x06002C13 RID: 11283 RVA: 0x00138D14 File Offset: 0x00136F14
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBadge.PropertyName._tween, ref variant))
			{
				this._tween = variant.As<Tween>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBadge.PropertyName._hoverNode, ref variant2))
			{
				this._hoverNode = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBadge.PropertyName._selectionReticle, ref variant3))
			{
				this._selectionReticle = variant3.As<NSelectionReticle>();
			}
		}

		// Token: 0x04001091 RID: 4241
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/game_over_screen/badge");

		// Token: 0x04001092 RID: 4242
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04001093 RID: 4243
		private HoverTip _hoverTip;

		// Token: 0x04001094 RID: 4244
		private LocString _title;

		// Token: 0x04001095 RID: 4245
		private LocString _description;

		// Token: 0x04001096 RID: 4246
		private const string _table = "badges";

		// Token: 0x04001097 RID: 4247
		private Control _hoverNode;

		// Token: 0x04001098 RID: 4248
		private NSelectionReticle _selectionReticle;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001543 RID: 5443
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005153 RID: 20819
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005154 RID: 20820
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'GetRarityPrefix' method.
			/// </summary>
			// Token: 0x04005155 RID: 20821
			public static readonly StringName GetRarityPrefix = "GetRarityPrefix";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005156 RID: 20822
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04005157 RID: 20823
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04005158 RID: 20824
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'GetBadgeBaseTexture' method.
			/// </summary>
			// Token: 0x04005159 RID: 20825
			public static readonly StringName GetBadgeBaseTexture = "GetBadgeBaseTexture";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001544 RID: 5444
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400515A RID: 20826
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_hoverNode' field.
			/// </summary>
			// Token: 0x0400515B RID: 20827
			public static readonly StringName _hoverNode = "_hoverNode";

			/// <summary>
			/// Cached name for the '_selectionReticle' field.
			/// </summary>
			// Token: 0x0400515C RID: 20828
			public static readonly StringName _selectionReticle = "_selectionReticle";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001545 RID: 5445
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}

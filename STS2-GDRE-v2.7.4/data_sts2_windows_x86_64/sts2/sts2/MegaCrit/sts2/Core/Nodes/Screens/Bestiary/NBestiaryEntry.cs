using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary
{
	/// <summary>
	/// The buttons in the monster list in the Bestiary entry. A text button.
	/// </summary>
	// Token: 0x0200035F RID: 863
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryEntry.cs")]
	public class NBestiaryEntry : NButton
	{
		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x0600322B RID: 12843 RVA: 0x00176C8E File Offset: 0x00174E8E
		[Nullable(2)]
		protected override string HoveredSfx
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x0600322C RID: 12844 RVA: 0x00176C91 File Offset: 0x00174E91
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/bestiary/bestiary_entry");
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x00176C9D File Offset: 0x00174E9D
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NBestiaryEntry.ScenePath);
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x0600322E RID: 12846 RVA: 0x00176CA9 File Offset: 0x00174EA9
		// (set) Token: 0x0600322F RID: 12847 RVA: 0x00176CB1 File Offset: 0x00174EB1
		public BestiaryEntry Entry { get; private set; }

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06003230 RID: 12848 RVA: 0x00176CBA File Offset: 0x00174EBA
		// (set) Token: 0x06003231 RID: 12849 RVA: 0x00176CC2 File Offset: 0x00174EC2
		public bool IsDiscovered { get; private set; }

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06003232 RID: 12850 RVA: 0x00176CCB File Offset: 0x00174ECB
		public bool IsUnderConstruction
		{
			get
			{
				return this._underConstructionIcon.Visible;
			}
		}

		// Token: 0x06003233 RID: 12851 RVA: 0x00176CD8 File Offset: 0x00174ED8
		public static NBestiaryEntry Create(BestiaryEntry entry, bool isDiscovered)
		{
			NBestiaryEntry nbestiaryEntry = PreloadManager.Cache.GetScene(NBestiaryEntry.ScenePath).Instantiate<NBestiaryEntry>(0L);
			nbestiaryEntry.Entry = entry;
			nbestiaryEntry.IsDiscovered = isDiscovered;
			return nbestiaryEntry;
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x00176D0C File Offset: 0x00174F0C
		public override void _Ready()
		{
			this.ConnectSignals();
			base.FocusNeighborLeft = new NodePath(".");
			base.FocusNeighborRight = new NodePath(".");
			this._label = base.GetNode<MegaRichTextLabel>("%Label");
			this._highlight = base.GetNode<Control>("%Highlight");
			this._underConstructionIcon = base.GetNode<TextureRect>("%UnderConstructionIcon");
			if (!this.IsDiscovered)
			{
				base.Disable();
				this._label.Text = new LocString("bestiary", "UNSEEN.monsterName").GetRawText();
				this._label.SelfModulate = StsColors.gray;
				this._underConstructionIcon.Visible = false;
				return;
			}
			this._label.Text = this.Entry.GetEntryTitle();
			RoomType roomType = this.Entry.roomType;
			Color color;
			if (roomType != RoomType.Elite)
			{
				if (roomType == RoomType.Boss)
				{
					color = StsColors.red;
				}
				else
				{
					color = StsColors.cream;
				}
			}
			else
			{
				color = StsColors.purple;
			}
			this._defaultColor = color;
			this._label.SelfModulate = this._defaultColor;
			this._underConstructionIcon.Visible = false;
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x00176E30 File Offset: 0x00175030
		protected override void OnFocus()
		{
			base.OnFocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this._label, "scale", Vector2.One * 1.1f, 0.05);
			if (NControllerManager.Instance.IsUsingController)
			{
				this._highlight.Visible = true;
			}
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x00176EB4 File Offset: 0x001750B4
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this._label, "scale", Vector2.One, 0.3).SetEase(1L).SetTrans(7L);
			this._highlight.Visible = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003237 RID: 12855 RVA: 0x00176F30 File Offset: 0x00175130
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NBestiaryEntry.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryEntry.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBestiaryEntry.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003238 RID: 12856 RVA: 0x00176FD4 File Offset: 0x001751D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBestiaryEntry.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryEntry.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBestiaryEntry.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003239 RID: 12857 RVA: 0x00177059 File Offset: 0x00175259
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBestiaryEntry.MethodName._Ready || (ref method) == NBestiaryEntry.MethodName.OnFocus || (ref method) == NBestiaryEntry.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600323A RID: 12858 RVA: 0x00177090 File Offset: 0x00175290
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBestiaryEntry.PropertyName.IsDiscovered)
			{
				this.IsDiscovered = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryEntry.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryEntry.PropertyName._highlight)
			{
				this._highlight = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryEntry.PropertyName._underConstructionIcon)
			{
				this._underConstructionIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryEntry.PropertyName._defaultColor)
			{
				this._defaultColor = VariantUtils.ConvertTo<Color>(ref value);
				return true;
			}
			if ((ref name) == NBestiaryEntry.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600323B RID: 12859 RVA: 0x00177148 File Offset: 0x00175348
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBestiaryEntry.PropertyName.HoveredSfx)
			{
				string hoveredSfx = this.HoveredSfx;
				value = VariantUtils.CreateFrom<string>(ref hoveredSfx);
				return true;
			}
			if ((ref name) == NBestiaryEntry.PropertyName.IsDiscovered)
			{
				bool flag = this.IsDiscovered;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NBestiaryEntry.PropertyName.IsUnderConstruction)
			{
				bool flag = this.IsUnderConstruction;
				value = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref name) == NBestiaryEntry.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NBestiaryEntry.PropertyName._highlight)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._highlight);
				return true;
			}
			if ((ref name) == NBestiaryEntry.PropertyName._underConstructionIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._underConstructionIcon);
				return true;
			}
			if ((ref name) == NBestiaryEntry.PropertyName._defaultColor)
			{
				value = VariantUtils.CreateFrom<Color>(ref this._defaultColor);
				return true;
			}
			if ((ref name) == NBestiaryEntry.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600323C RID: 12860 RVA: 0x00177268 File Offset: 0x00175468
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NBestiaryEntry.PropertyName.HoveredSfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryEntry.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryEntry.PropertyName._highlight, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryEntry.PropertyName._underConstructionIcon, 0L, "", 4096L, false),
				new PropertyInfo(20L, NBestiaryEntry.PropertyName._defaultColor, 0L, "", 4096L, false),
				new PropertyInfo(1L, NBestiaryEntry.PropertyName.IsDiscovered, 0L, "", 4096L, false),
				new PropertyInfo(1L, NBestiaryEntry.PropertyName.IsUnderConstruction, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBestiaryEntry.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600323D RID: 12861 RVA: 0x00177384 File Offset: 0x00175584
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName isDiscovered = NBestiaryEntry.PropertyName.IsDiscovered;
			bool isDiscovered2 = this.IsDiscovered;
			info.AddProperty(isDiscovered, Variant.From<bool>(ref isDiscovered2));
			info.AddProperty(NBestiaryEntry.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NBestiaryEntry.PropertyName._highlight, Variant.From<Control>(ref this._highlight));
			info.AddProperty(NBestiaryEntry.PropertyName._underConstructionIcon, Variant.From<TextureRect>(ref this._underConstructionIcon));
			info.AddProperty(NBestiaryEntry.PropertyName._defaultColor, Variant.From<Color>(ref this._defaultColor));
			info.AddProperty(NBestiaryEntry.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x0600323E RID: 12862 RVA: 0x00177420 File Offset: 0x00175620
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBestiaryEntry.PropertyName.IsDiscovered, ref variant))
			{
				this.IsDiscovered = variant.As<bool>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBestiaryEntry.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBestiaryEntry.PropertyName._highlight, ref variant3))
			{
				this._highlight = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NBestiaryEntry.PropertyName._underConstructionIcon, ref variant4))
			{
				this._underConstructionIcon = variant4.As<TextureRect>();
			}
			Variant variant5;
			if (info.TryGetProperty(NBestiaryEntry.PropertyName._defaultColor, ref variant5))
			{
				this._defaultColor = variant5.As<Color>();
			}
			Variant variant6;
			if (info.TryGetProperty(NBestiaryEntry.PropertyName._tween, ref variant6))
			{
				this._tween = variant6.As<Tween>();
			}
		}

		// Token: 0x0400139B RID: 5019
		private MegaRichTextLabel _label;

		// Token: 0x0400139C RID: 5020
		private Control _highlight;

		// Token: 0x0400139D RID: 5021
		private TextureRect _underConstructionIcon;

		// Token: 0x0400139E RID: 5022
		private Color _defaultColor;

		// Token: 0x040013A1 RID: 5025
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200165C RID: 5724
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400574A RID: 22346
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400574B RID: 22347
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x0400574C RID: 22348
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200165D RID: 5725
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'HoveredSfx' property.
			/// </summary>
			// Token: 0x0400574D RID: 22349
			public new static readonly StringName HoveredSfx = "HoveredSfx";

			/// <summary>
			/// Cached name for the 'IsDiscovered' property.
			/// </summary>
			// Token: 0x0400574E RID: 22350
			public static readonly StringName IsDiscovered = "IsDiscovered";

			/// <summary>
			/// Cached name for the 'IsUnderConstruction' property.
			/// </summary>
			// Token: 0x0400574F RID: 22351
			public static readonly StringName IsUnderConstruction = "IsUnderConstruction";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04005750 RID: 22352
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_highlight' field.
			/// </summary>
			// Token: 0x04005751 RID: 22353
			public static readonly StringName _highlight = "_highlight";

			/// <summary>
			/// Cached name for the '_underConstructionIcon' field.
			/// </summary>
			// Token: 0x04005752 RID: 22354
			public static readonly StringName _underConstructionIcon = "_underConstructionIcon";

			/// <summary>
			/// Cached name for the '_defaultColor' field.
			/// </summary>
			// Token: 0x04005753 RID: 22355
			public static readonly StringName _defaultColor = "_defaultColor";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005754 RID: 22356
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200165E RID: 5726
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}

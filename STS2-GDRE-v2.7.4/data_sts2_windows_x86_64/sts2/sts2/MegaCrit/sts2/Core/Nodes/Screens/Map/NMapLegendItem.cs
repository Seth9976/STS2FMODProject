using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	/// <summary>
	/// A single line on the map legend. Has some hover logic, will eventually require controller
	/// support so players can navigate to this and read what each map icon represents.
	/// </summary>
	// Token: 0x020002F5 RID: 757
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapLegendItem.cs")]
	public class NMapLegendItem : NButton
	{
		// Token: 0x060028DB RID: 10459 RVA: 0x0011916A File Offset: 0x0011736A
		public override void _Ready()
		{
			this.ConnectSignals();
			this.SetLocalizedFields(base.Name);
			this.SetMapPointType(base.Name);
			this._icon = base.GetNode<TextureRect>("Icon");
		}

		/// <summary>
		/// Helper method which grabs the Node's name and finds the appropriate map point type.
		/// </summary>
		/// <param name="name">Name of this Godot node.</param>
		// Token: 0x060028DC RID: 10460 RVA: 0x001191AC File Offset: 0x001173AC
		[NullableContext(1)]
		private void SetMapPointType(string name)
		{
			MapPointType mapPointType;
			if (!(name == "UnknownLegendItem"))
			{
				if (!(name == "MerchantLegendItem"))
				{
					if (!(name == "TreasureLegendItem"))
					{
						if (!(name == "RestSiteLegendItem"))
						{
							if (!(name == "EnemyLegendItem"))
							{
								if (!(name == "EliteLegendItem"))
								{
									throw new ArgumentOutOfRangeException("Unknown Node " + name + " when setting MapLegend localization.");
								}
								mapPointType = MapPointType.Elite;
							}
							else
							{
								mapPointType = MapPointType.Monster;
							}
						}
						else
						{
							mapPointType = MapPointType.RestSite;
						}
					}
					else
					{
						mapPointType = MapPointType.Treasure;
					}
				}
				else
				{
					mapPointType = MapPointType.Shop;
				}
			}
			else
			{
				mapPointType = MapPointType.Unknown;
			}
			this._pointType = mapPointType;
		}

		/// <summary>
		/// Helper method which grabs the Node's name and then finds the appropriate
		/// LocString for this LegendItem's label and HoverTip text fields.
		/// </summary>
		/// <param name="name">Name of this Godot node.</param>
		// Token: 0x060028DD RID: 10461 RVA: 0x00119240 File Offset: 0x00117440
		[NullableContext(1)]
		private void SetLocalizedFields(string name)
		{
			string text;
			if (!(name == "UnknownLegendItem"))
			{
				if (!(name == "MerchantLegendItem"))
				{
					if (!(name == "TreasureLegendItem"))
					{
						if (!(name == "RestSiteLegendItem"))
						{
							if (!(name == "EnemyLegendItem"))
							{
								if (!(name == "EliteLegendItem"))
								{
									throw new ArgumentOutOfRangeException("Unknown Node " + name + " when setting MapLegend localization.");
								}
								text = "LEGEND_ELITE";
							}
							else
							{
								text = "LEGEND_ENEMY";
							}
						}
						else
						{
							text = "LEGEND_REST";
						}
					}
					else
					{
						text = "LEGEND_TREASURE";
					}
				}
				else
				{
					text = "LEGEND_MERCHANT";
				}
			}
			else
			{
				text = "LEGEND_UNKNOWN";
			}
			string text2 = text;
			base.GetNode<MegaLabel>("MegaLabel").SetTextAutoSize(new LocString("map", text2 + ".title").GetFormattedText());
			this._hoverTip = new HoverTip(new LocString("map", text2 + ".hoverTip.title"), new LocString("map", text2 + ".hoverTip.description"), null);
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x0011934C File Offset: 0x0011754C
		protected override void OnFocus()
		{
			Tween scaleDownTween = this._scaleDownTween;
			if (scaleDownTween != null)
			{
				scaleDownTween.Kill();
			}
			this._icon.Scale = NMapLegendItem._hoverScale;
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._hoverTip, HoverTipAlignment.None);
			Control parent = base.GetParent<Control>();
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(parent.GlobalPosition + new Vector2(parent.Size.X - nhoverTipSet.Size.X, parent.Size.Y), false);
			}
			NMapScreen.Instance.HighlightPointType(this._pointType);
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x001193E0 File Offset: 0x001175E0
		protected override void OnUnfocus()
		{
			this._scaleDownTween = base.CreateTween().SetParallel(true);
			this._scaleDownTween.TweenProperty(this._icon, "scale", Vector2.One, 0.5).From(NMapLegendItem._hoverScale).SetEase(1L)
				.SetTrans(5L);
			NMapScreen.Instance.HighlightPointType(MapPointType.Unassigned);
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060028E0 RID: 10464 RVA: 0x0011945C File Offset: 0x0011765C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NMapLegendItem.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapLegendItem.MethodName.SetMapPointType, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "name", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapLegendItem.MethodName.SetLocalizedFields, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "name", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NMapLegendItem.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapLegendItem.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060028E1 RID: 10465 RVA: 0x001195A8 File Offset: 0x001177A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapLegendItem.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapLegendItem.MethodName.SetMapPointType && args.Count == 1)
			{
				this.SetMapPointType(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapLegendItem.MethodName.SetLocalizedFields && args.Count == 1)
			{
				this.SetLocalizedFields(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapLegendItem.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapLegendItem.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060028E2 RID: 10466 RVA: 0x00119694 File Offset: 0x00117894
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapLegendItem.MethodName._Ready || (ref method) == NMapLegendItem.MethodName.SetMapPointType || (ref method) == NMapLegendItem.MethodName.SetLocalizedFields || (ref method) == NMapLegendItem.MethodName.OnFocus || (ref method) == NMapLegendItem.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060028E3 RID: 10467 RVA: 0x001196F4 File Offset: 0x001178F4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapLegendItem.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapLegendItem.PropertyName._scaleDownTween)
			{
				this._scaleDownTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMapLegendItem.PropertyName._pointType)
			{
				this._pointType = VariantUtils.ConvertTo<MapPointType>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060028E4 RID: 10468 RVA: 0x0011975C File Offset: 0x0011795C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapLegendItem.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NMapLegendItem.PropertyName._scaleDownTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._scaleDownTween);
				return true;
			}
			if ((ref name) == NMapLegendItem.PropertyName._pointType)
			{
				value = VariantUtils.CreateFrom<MapPointType>(ref this._pointType);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060028E5 RID: 10469 RVA: 0x001197D4 File Offset: 0x001179D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMapLegendItem.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapLegendItem.PropertyName._scaleDownTween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NMapLegendItem.PropertyName._pointType, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060028E6 RID: 10470 RVA: 0x0011984C File Offset: 0x00117A4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapLegendItem.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NMapLegendItem.PropertyName._scaleDownTween, Variant.From<Tween>(ref this._scaleDownTween));
			info.AddProperty(NMapLegendItem.PropertyName._pointType, Variant.From<MapPointType>(ref this._pointType));
		}

		/// <inheritdoc />
		// Token: 0x060028E7 RID: 10471 RVA: 0x001198A4 File Offset: 0x00117AA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapLegendItem.PropertyName._icon, ref variant))
			{
				this._icon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapLegendItem.PropertyName._scaleDownTween, ref variant2))
			{
				this._scaleDownTween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMapLegendItem.PropertyName._pointType, ref variant3))
			{
				this._pointType = variant3.As<MapPointType>();
			}
		}

		// Token: 0x04000F41 RID: 3905
		[Nullable(1)]
		private TextureRect _icon;

		// Token: 0x04000F42 RID: 3906
		private HoverTip _hoverTip;

		// Token: 0x04000F43 RID: 3907
		[Nullable(2)]
		private Tween _scaleDownTween;

		// Token: 0x04000F44 RID: 3908
		private static readonly Vector2 _hoverScale = Vector2.One * 1.25f;

		// Token: 0x04000F45 RID: 3909
		private const float _unhoverAnimDur = 0.5f;

		// Token: 0x04000F46 RID: 3910
		private MapPointType _pointType;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014B6 RID: 5302
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004E97 RID: 20119
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetMapPointType' method.
			/// </summary>
			// Token: 0x04004E98 RID: 20120
			public static readonly StringName SetMapPointType = "SetMapPointType";

			/// <summary>
			/// Cached name for the 'SetLocalizedFields' method.
			/// </summary>
			// Token: 0x04004E99 RID: 20121
			public static readonly StringName SetLocalizedFields = "SetLocalizedFields";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004E9A RID: 20122
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004E9B RID: 20123
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014B7 RID: 5303
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004E9C RID: 20124
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_scaleDownTween' field.
			/// </summary>
			// Token: 0x04004E9D RID: 20125
			public static readonly StringName _scaleDownTween = "_scaleDownTween";

			/// <summary>
			/// Cached name for the '_pointType' field.
			/// </summary>
			// Token: 0x04004E9E RID: 20126
			public static readonly StringName _pointType = "_pointType";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014B8 RID: 5304
		public new class SignalName : NButton.SignalName
		{
		}
	}
}

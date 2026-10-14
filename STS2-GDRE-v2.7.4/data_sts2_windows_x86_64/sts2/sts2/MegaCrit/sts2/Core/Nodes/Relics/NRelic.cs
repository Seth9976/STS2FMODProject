using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Relics
{
	/// <summary>
	/// Only contains the visuals (the image) of a relic.
	/// Does NOT handle input, that is to be done by a RelicHolder.
	/// </summary>
	// Token: 0x02000376 RID: 886
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Relics/NRelic.cs")]
	public class NRelic : Control
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x0600344F RID: 13391 RVA: 0x00186177 File Offset: 0x00184377
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					"res://materials/ui/relic_mat.tres",
					NRelic._scenePath
				});
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06003450 RID: 13392 RVA: 0x00186194 File Offset: 0x00184394
		// (set) Token: 0x06003451 RID: 13393 RVA: 0x0018619C File Offset: 0x0018439C
		public TextureRect Icon { get; private set; }

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06003452 RID: 13394 RVA: 0x001861A5 File Offset: 0x001843A5
		// (set) Token: 0x06003453 RID: 13395 RVA: 0x001861AD File Offset: 0x001843AD
		public TextureRect Outline { get; private set; }

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06003454 RID: 13396 RVA: 0x001861B8 File Offset: 0x001843B8
		// (remove) Token: 0x06003455 RID: 13397 RVA: 0x001861F0 File Offset: 0x001843F0
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event Action<RelicModel, RelicModel> ModelChanged;

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06003456 RID: 13398 RVA: 0x00186225 File Offset: 0x00184425
		// (set) Token: 0x06003457 RID: 13399 RVA: 0x0018623C File Offset: 0x0018443C
		public RelicModel Model
		{
			get
			{
				RelicModel model = this._model;
				if (model == null)
				{
					throw new InvalidOperationException("Model was accessed before it was set.");
				}
				return model;
			}
			set
			{
				if (this._model != value)
				{
					RelicModel model = this._model;
					this._model = value;
					Action<RelicModel, RelicModel> modelChanged = this.ModelChanged;
					if (modelChanged != null)
					{
						modelChanged(model, this._model);
					}
				}
				this.Reload();
			}
		}

		// Token: 0x06003458 RID: 13400 RVA: 0x00186280 File Offset: 0x00184480
		[return: Nullable(2)]
		public static NRelic Create(RelicModel relic, NRelic.IconSize iconSize)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NRelic nrelic = PreloadManager.Cache.GetScene(NRelic._scenePath).Instantiate<NRelic>(0L);
			Node node = nrelic;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
			defaultInterpolatedStringHandler.AppendLiteral("NRelic-");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(relic.Id);
			node.Name = defaultInterpolatedStringHandler.ToStringAndClear();
			nrelic.Model = relic;
			nrelic._iconSize = iconSize;
			return nrelic;
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x001862F0 File Offset: 0x001844F0
		public override void _Ready()
		{
			this.Icon = base.GetNode<TextureRect>("%Icon");
			this.Outline = base.GetNode<TextureRect>("%Outline");
			this.Reload();
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x00186324 File Offset: 0x00184524
		private void Reload()
		{
			if (!base.IsNodeReady())
			{
				return;
			}
			if (this._model == null)
			{
				return;
			}
			this.Model.UpdateTexture(this.Icon);
			NRelic.IconSize iconSize = this._iconSize;
			if (iconSize == NRelic.IconSize.Small)
			{
				this.Icon.Texture = this.Model.Icon;
				this.Outline.Visible = true;
				this.Outline.Texture = this.Model.IconOutline;
				return;
			}
			if (iconSize != NRelic.IconSize.Large)
			{
				throw new ArgumentOutOfRangeException("_iconSize", this._iconSize, null);
			}
			this.Icon.Texture = this.Model.BigIcon;
			this.Outline.Visible = false;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600345B RID: 13403 RVA: 0x001863DC File Offset: 0x001845DC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NRelic.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelic.MethodName.Reload, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600345C RID: 13404 RVA: 0x00186450 File Offset: 0x00184650
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelic.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelic.MethodName.Reload && args.Count == 0)
			{
				this.Reload();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600345D RID: 13405 RVA: 0x001864B0 File Offset: 0x001846B0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRelic.MethodName._Ready || (ref method) == NRelic.MethodName.Reload || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600345E RID: 13406 RVA: 0x001864D8 File Offset: 0x001846D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRelic.PropertyName.Icon)
			{
				this.Icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NRelic.PropertyName.Outline)
			{
				this.Outline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NRelic.PropertyName._iconSize)
			{
				this._iconSize = VariantUtils.ConvertTo<NRelic.IconSize>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600345F RID: 13407 RVA: 0x00186540 File Offset: 0x00184740
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRelic.PropertyName.Icon)
			{
				TextureRect textureRect = this.Icon;
				value = VariantUtils.CreateFrom<TextureRect>(ref textureRect);
				return true;
			}
			if ((ref name) == NRelic.PropertyName.Outline)
			{
				TextureRect textureRect = this.Outline;
				value = VariantUtils.CreateFrom<TextureRect>(ref textureRect);
				return true;
			}
			if ((ref name) == NRelic.PropertyName._iconSize)
			{
				value = VariantUtils.CreateFrom<NRelic.IconSize>(ref this._iconSize);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003460 RID: 13408 RVA: 0x001865BC File Offset: 0x001847BC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRelic.PropertyName.Icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelic.PropertyName.Outline, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRelic.PropertyName._iconSize, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003461 RID: 13409 RVA: 0x00186634 File Offset: 0x00184834
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName icon = NRelic.PropertyName.Icon;
			TextureRect textureRect = this.Icon;
			info.AddProperty(icon, Variant.From<TextureRect>(ref textureRect));
			StringName outline = NRelic.PropertyName.Outline;
			textureRect = this.Outline;
			info.AddProperty(outline, Variant.From<TextureRect>(ref textureRect));
			info.AddProperty(NRelic.PropertyName._iconSize, Variant.From<NRelic.IconSize>(ref this._iconSize));
		}

		/// <inheritdoc />
		// Token: 0x06003462 RID: 13410 RVA: 0x00186690 File Offset: 0x00184890
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRelic.PropertyName.Icon, ref variant))
			{
				this.Icon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRelic.PropertyName.Outline, ref variant2))
			{
				this.Outline = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRelic.PropertyName._iconSize, ref variant3))
			{
				this._iconSize = variant3.As<NRelic.IconSize>();
			}
		}

		// Token: 0x04001453 RID: 5203
		public const string relicMatPath = "res://materials/ui/relic_mat.tres";

		// Token: 0x04001454 RID: 5204
		private static readonly string _scenePath = SceneHelper.GetScenePath("relics/relic");

		// Token: 0x04001457 RID: 5207
		[Nullable(2)]
		private RelicModel _model;

		// Token: 0x04001458 RID: 5208
		private NRelic.IconSize _iconSize;

		// Token: 0x020016C4 RID: 5828
		[NullableContext(0)]
		public enum IconSize
		{
			// Token: 0x040058C7 RID: 22727
			Small,
			// Token: 0x040058C8 RID: 22728
			Large
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016C5 RID: 5829
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040058C9 RID: 22729
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Reload' method.
			/// </summary>
			// Token: 0x040058CA RID: 22730
			public static readonly StringName Reload = "Reload";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016C6 RID: 5830
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Icon' property.
			/// </summary>
			// Token: 0x040058CB RID: 22731
			public static readonly StringName Icon = "Icon";

			/// <summary>
			/// Cached name for the 'Outline' property.
			/// </summary>
			// Token: 0x040058CC RID: 22732
			public static readonly StringName Outline = "Outline";

			/// <summary>
			/// Cached name for the '_iconSize' field.
			/// </summary>
			// Token: 0x040058CD RID: 22733
			public static readonly StringName _iconSize = "_iconSize";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016C7 RID: 5831
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}

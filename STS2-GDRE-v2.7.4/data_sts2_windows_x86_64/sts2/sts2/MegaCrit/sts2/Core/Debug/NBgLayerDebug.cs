using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Debug
{
	// Token: 0x02000D0D RID: 3341
	[Tool]
	[ScriptPath("res://src/Core/Debug/NBgLayerDebug.cs")]
	public class NBgLayerDebug : Control
	{
		// Token: 0x17002529 RID: 9513
		// (get) Token: 0x060086C1 RID: 34497 RVA: 0x002D3768 File Offset: 0x002D1968
		// (set) Token: 0x060086C2 RID: 34498 RVA: 0x002D3770 File Offset: 0x002D1970
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		public NBgLayerDebug.LayerVisibility VisibleLayer
		{
			get
			{
				return this._visibleLayer;
			}
			set
			{
				this._visibleLayer = value;
				if (Engine.IsEditorHint())
				{
					this.UpdateLayers();
				}
			}
		}

		// Token: 0x1700252A RID: 9514
		// (get) Token: 0x060086C3 RID: 34499 RVA: 0x002D3786 File Offset: 0x002D1986
		[ExportToolButton("Reload Layers")]
		private Callable ReloadLayersCallable
		{
			get
			{
				return Callable.From(new Action(this.ReloadLayers));
			}
		}

		// Token: 0x060086C4 RID: 34500 RVA: 0x002D3799 File Offset: 0x002D1999
		public override void _EnterTree()
		{
			if (!Engine.IsEditorHint())
			{
				return;
			}
			this.ReloadLayers();
		}

		// Token: 0x060086C5 RID: 34501 RVA: 0x002D37AC File Offset: 0x002D19AC
		private void ReloadLayers()
		{
			string sceneFilePath = base.GetTree().GetEditedSceneRoot().SceneFilePath;
			if (sceneFilePath == null)
			{
				return;
			}
			string text;
			if (base.Name.ToString() == "Foreground")
			{
				text = "fg";
			}
			else
			{
				string text2 = base.Name.ToString();
				int length = "Layer_".Length;
				int num;
				if (!int.TryParse(text2.Substring(length, text2.Length - length), out num))
				{
					return;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
				defaultInterpolatedStringHandler.AppendLiteral("bg_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num, "D2");
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			string file = StringExtensions.GetFile(sceneFilePath);
			string text3 = file.Substring(0, file.LastIndexOf('_'));
			string text4 = Path.Combine(StringExtensions.GetBaseDir(sceneFilePath), "layers", text3 + "_" + text);
			string text5 = text4 + "_a.tscn";
			string text6 = text4 + "_b.tscn";
			string text7 = text4 + "_c.tscn";
			if (ResourceLoader.Exists(text5, ""))
			{
				this._layerA = ResourceLoader.Load<PackedScene>(text5, null, 1L);
			}
			if (ResourceLoader.Exists(text6, ""))
			{
				this._layerB = ResourceLoader.Load<PackedScene>(text6, null, 1L);
			}
			if (ResourceLoader.Exists(text7, ""))
			{
				this._layerC = ResourceLoader.Load<PackedScene>(text7, null, 1L);
			}
			this.UpdateLayers();
		}

		// Token: 0x060086C6 RID: 34502 RVA: 0x002D3910 File Offset: 0x002D1B10
		private void UpdateLayers()
		{
			this.ClearLayers();
			if (this._visibleLayer == NBgLayerDebug.LayerVisibility.A && this._layerA != null)
			{
				this.AddLayer(NBgLayerDebug.LayerVisibility.A, this._layerA);
			}
			if (this._visibleLayer == NBgLayerDebug.LayerVisibility.B && this._layerB != null)
			{
				this.AddLayer(NBgLayerDebug.LayerVisibility.B, this._layerB);
			}
			if (this._visibleLayer == NBgLayerDebug.LayerVisibility.C && this._layerC != null)
			{
				this.AddLayer(NBgLayerDebug.LayerVisibility.C, this._layerC);
			}
		}

		// Token: 0x060086C7 RID: 34503 RVA: 0x002D397C File Offset: 0x002D1B7C
		[NullableContext(1)]
		private void AddLayer(NBgLayerDebug.LayerVisibility name, PackedScene layerScene)
		{
			Control control = layerScene.Instantiate<Control>(0L);
			control.Name = NBgLayerDebug.ToLayerName(name);
			this.AddChildSafely(control);
		}

		// Token: 0x060086C8 RID: 34504 RVA: 0x002D39AC File Offset: 0x002D1BAC
		[NullableContext(1)]
		private static string ToLayerName(NBgLayerDebug.LayerVisibility layer)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
			defaultInterpolatedStringHandler.AppendFormatted("Layer_");
			defaultInterpolatedStringHandler.AppendFormatted<NBgLayerDebug.LayerVisibility>(layer);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060086C9 RID: 34505 RVA: 0x002D39DD File Offset: 0x002D1BDD
		[NullableContext(1)]
		private IEnumerable<Control> GetLayerNodes()
		{
			NBgLayerDebug.<GetLayerNodes>d__16 <GetLayerNodes>d__ = new NBgLayerDebug.<GetLayerNodes>d__16(-2);
			<GetLayerNodes>d__.<>4__this = this;
			return <GetLayerNodes>d__;
		}

		// Token: 0x060086CA RID: 34506 RVA: 0x002D39F0 File Offset: 0x002D1BF0
		private void ClearLayers()
		{
			foreach (Control control in this.GetLayerNodes())
			{
				this.RemoveChildSafely(control);
				control.QueueFreeSafely();
			}
		}

		// Token: 0x060086CB RID: 34507 RVA: 0x002D3A44 File Offset: 0x002D1C44
		public override void _ExitTree()
		{
			this.ClearLayers();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060086CC RID: 34508 RVA: 0x002D3A4C File Offset: 0x002D1C4C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(7)
			{
				new MethodInfo(NBgLayerDebug.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBgLayerDebug.MethodName.ReloadLayers, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBgLayerDebug.MethodName.UpdateLayers, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBgLayerDebug.MethodName.AddLayer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "name", 0L, "", 6L, false),
					new PropertyInfo(24L, "layerScene", 0L, "", 6L, new StringName("PackedScene"), false)
				}, null),
				new MethodInfo(NBgLayerDebug.MethodName.ToLayerName, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "layer", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NBgLayerDebug.MethodName.ClearLayers, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NBgLayerDebug.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060086CD RID: 34509 RVA: 0x002D3C24 File Offset: 0x002D1E24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBgLayerDebug.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBgLayerDebug.MethodName.ReloadLayers && args.Count == 0)
			{
				this.ReloadLayers();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBgLayerDebug.MethodName.UpdateLayers && args.Count == 0)
			{
				this.UpdateLayers();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBgLayerDebug.MethodName.AddLayer && args.Count == 2)
			{
				this.AddLayer(VariantUtils.ConvertTo<NBgLayerDebug.LayerVisibility>(args[0]), VariantUtils.ConvertTo<PackedScene>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBgLayerDebug.MethodName.ToLayerName && args.Count == 1)
			{
				string text = NBgLayerDebug.ToLayerName(VariantUtils.ConvertTo<NBgLayerDebug.LayerVisibility>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref method) == NBgLayerDebug.MethodName.ClearLayers && args.Count == 0)
			{
				this.ClearLayers();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NBgLayerDebug.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060086CE RID: 34510 RVA: 0x002D3D6C File Offset: 0x002D1F6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NBgLayerDebug.MethodName.ToLayerName && args.Count == 1)
			{
				string text = NBgLayerDebug.ToLayerName(VariantUtils.ConvertTo<NBgLayerDebug.LayerVisibility>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060086CF RID: 34511 RVA: 0x002D3DBC File Offset: 0x002D1FBC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NBgLayerDebug.MethodName._EnterTree || (ref method) == NBgLayerDebug.MethodName.ReloadLayers || (ref method) == NBgLayerDebug.MethodName.UpdateLayers || (ref method) == NBgLayerDebug.MethodName.AddLayer || (ref method) == NBgLayerDebug.MethodName.ToLayerName || (ref method) == NBgLayerDebug.MethodName.ClearLayers || (ref method) == NBgLayerDebug.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060086D0 RID: 34512 RVA: 0x002D3E3C File Offset: 0x002D203C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NBgLayerDebug.PropertyName.VisibleLayer)
			{
				this.VisibleLayer = VariantUtils.ConvertTo<NBgLayerDebug.LayerVisibility>(ref value);
				return true;
			}
			if ((ref name) == NBgLayerDebug.PropertyName._layerA)
			{
				this._layerA = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NBgLayerDebug.PropertyName._layerB)
			{
				this._layerB = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NBgLayerDebug.PropertyName._layerC)
			{
				this._layerC = VariantUtils.ConvertTo<PackedScene>(ref value);
				return true;
			}
			if ((ref name) == NBgLayerDebug.PropertyName._visibleLayer)
			{
				this._visibleLayer = VariantUtils.ConvertTo<NBgLayerDebug.LayerVisibility>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060086D1 RID: 34513 RVA: 0x002D3ED8 File Offset: 0x002D20D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NBgLayerDebug.PropertyName.VisibleLayer)
			{
				NBgLayerDebug.LayerVisibility visibleLayer = this.VisibleLayer;
				value = VariantUtils.CreateFrom<NBgLayerDebug.LayerVisibility>(ref visibleLayer);
				return true;
			}
			if ((ref name) == NBgLayerDebug.PropertyName.ReloadLayersCallable)
			{
				Callable reloadLayersCallable = this.ReloadLayersCallable;
				value = VariantUtils.CreateFrom<Callable>(ref reloadLayersCallable);
				return true;
			}
			if ((ref name) == NBgLayerDebug.PropertyName._layerA)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._layerA);
				return true;
			}
			if ((ref name) == NBgLayerDebug.PropertyName._layerB)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._layerB);
				return true;
			}
			if ((ref name) == NBgLayerDebug.PropertyName._layerC)
			{
				value = VariantUtils.CreateFrom<PackedScene>(ref this._layerC);
				return true;
			}
			if ((ref name) == NBgLayerDebug.PropertyName._visibleLayer)
			{
				value = VariantUtils.CreateFrom<NBgLayerDebug.LayerVisibility>(ref this._visibleLayer);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060086D2 RID: 34514 RVA: 0x002D3FB4 File Offset: 0x002D21B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NBgLayerDebug.PropertyName._layerA, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBgLayerDebug.PropertyName._layerB, 0L, "", 4096L, false),
				new PropertyInfo(24L, NBgLayerDebug.PropertyName._layerC, 0L, "", 4096L, false),
				new PropertyInfo(2L, NBgLayerDebug.PropertyName._visibleLayer, 0L, "", 4096L, false),
				new PropertyInfo(2L, NBgLayerDebug.PropertyName.VisibleLayer, 2L, "A,B,C", 4102L, true),
				new PropertyInfo(25L, NBgLayerDebug.PropertyName.ReloadLayersCallable, 39L, "Reload Layers", 4L, true)
			};
		}

		/// <inheritdoc />
		// Token: 0x060086D3 RID: 34515 RVA: 0x002D408C File Offset: 0x002D228C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName visibleLayer = NBgLayerDebug.PropertyName.VisibleLayer;
			NBgLayerDebug.LayerVisibility visibleLayer2 = this.VisibleLayer;
			info.AddProperty(visibleLayer, Variant.From<NBgLayerDebug.LayerVisibility>(ref visibleLayer2));
			info.AddProperty(NBgLayerDebug.PropertyName._layerA, Variant.From<PackedScene>(ref this._layerA));
			info.AddProperty(NBgLayerDebug.PropertyName._layerB, Variant.From<PackedScene>(ref this._layerB));
			info.AddProperty(NBgLayerDebug.PropertyName._layerC, Variant.From<PackedScene>(ref this._layerC));
			info.AddProperty(NBgLayerDebug.PropertyName._visibleLayer, Variant.From<NBgLayerDebug.LayerVisibility>(ref this._visibleLayer));
		}

		/// <inheritdoc />
		// Token: 0x060086D4 RID: 34516 RVA: 0x002D4114 File Offset: 0x002D2314
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NBgLayerDebug.PropertyName.VisibleLayer, ref variant))
			{
				this.VisibleLayer = variant.As<NBgLayerDebug.LayerVisibility>();
			}
			Variant variant2;
			if (info.TryGetProperty(NBgLayerDebug.PropertyName._layerA, ref variant2))
			{
				this._layerA = variant2.As<PackedScene>();
			}
			Variant variant3;
			if (info.TryGetProperty(NBgLayerDebug.PropertyName._layerB, ref variant3))
			{
				this._layerB = variant3.As<PackedScene>();
			}
			Variant variant4;
			if (info.TryGetProperty(NBgLayerDebug.PropertyName._layerC, ref variant4))
			{
				this._layerC = variant4.As<PackedScene>();
			}
			Variant variant5;
			if (info.TryGetProperty(NBgLayerDebug.PropertyName._visibleLayer, ref variant5))
			{
				this._visibleLayer = variant5.As<NBgLayerDebug.LayerVisibility>();
			}
		}

		// Token: 0x04002EB2 RID: 11954
		[Nullable(1)]
		private const string _layerNodePrefix = "Layer_";

		// Token: 0x04002EB3 RID: 11955
		[Nullable(2)]
		private PackedScene _layerA;

		// Token: 0x04002EB4 RID: 11956
		[Nullable(2)]
		private PackedScene _layerB;

		// Token: 0x04002EB5 RID: 11957
		[Nullable(2)]
		private PackedScene _layerC;

		// Token: 0x04002EB6 RID: 11958
		private NBgLayerDebug.LayerVisibility _visibleLayer;

		// Token: 0x0200240F RID: 9231
		public enum LayerVisibility
		{
			// Token: 0x0400985C RID: 39004
			A,
			// Token: 0x0400985D RID: 39005
			B,
			// Token: 0x0400985E RID: 39006
			C
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002410 RID: 9232
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x0400985F RID: 39007
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the 'ReloadLayers' method.
			/// </summary>
			// Token: 0x04009860 RID: 39008
			public static readonly StringName ReloadLayers = "ReloadLayers";

			/// <summary>
			/// Cached name for the 'UpdateLayers' method.
			/// </summary>
			// Token: 0x04009861 RID: 39009
			public static readonly StringName UpdateLayers = "UpdateLayers";

			/// <summary>
			/// Cached name for the 'AddLayer' method.
			/// </summary>
			// Token: 0x04009862 RID: 39010
			public static readonly StringName AddLayer = "AddLayer";

			/// <summary>
			/// Cached name for the 'ToLayerName' method.
			/// </summary>
			// Token: 0x04009863 RID: 39011
			public static readonly StringName ToLayerName = "ToLayerName";

			/// <summary>
			/// Cached name for the 'ClearLayers' method.
			/// </summary>
			// Token: 0x04009864 RID: 39012
			public static readonly StringName ClearLayers = "ClearLayers";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04009865 RID: 39013
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002411 RID: 9233
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'VisibleLayer' property.
			/// </summary>
			// Token: 0x04009866 RID: 39014
			public static readonly StringName VisibleLayer = "VisibleLayer";

			/// <summary>
			/// Cached name for the 'ReloadLayersCallable' property.
			/// </summary>
			// Token: 0x04009867 RID: 39015
			public static readonly StringName ReloadLayersCallable = "ReloadLayersCallable";

			/// <summary>
			/// Cached name for the '_layerA' field.
			/// </summary>
			// Token: 0x04009868 RID: 39016
			public static readonly StringName _layerA = "_layerA";

			/// <summary>
			/// Cached name for the '_layerB' field.
			/// </summary>
			// Token: 0x04009869 RID: 39017
			public static readonly StringName _layerB = "_layerB";

			/// <summary>
			/// Cached name for the '_layerC' field.
			/// </summary>
			// Token: 0x0400986A RID: 39018
			public static readonly StringName _layerC = "_layerC";

			/// <summary>
			/// Cached name for the '_visibleLayer' field.
			/// </summary>
			// Token: 0x0400986B RID: 39019
			public static readonly StringName _visibleLayer = "_visibleLayer";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002412 RID: 9234
		public class SignalName : Control.SignalName
		{
		}
	}
}

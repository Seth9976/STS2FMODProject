using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Rooms;

namespace MegaCrit.Sts2.Core.Nodes.Rooms
{
	// Token: 0x02000368 RID: 872
	[ScriptPath("res://src/Core/Nodes/Rooms/NCombatBackground.cs")]
	public class NCombatBackground : Control
	{
		// Token: 0x060032B6 RID: 12982 RVA: 0x00179B24 File Offset: 0x00177D24
		[NullableContext(1)]
		public static NCombatBackground Create(BackgroundAssets bg)
		{
			if (bg.BackgroundScenePath == null)
			{
				throw new InvalidOperationException("Encounter does not have a background.");
			}
			string backgroundScenePath = bg.BackgroundScenePath;
			NCombatBackground ncombatBackground = PreloadManager.Cache.GetScene(backgroundScenePath).Instantiate<NCombatBackground>(0L);
			ncombatBackground.SetLayers(bg);
			return ncombatBackground;
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x00179B66 File Offset: 0x00177D66
		[NullableContext(1)]
		private void SetLayers(BackgroundAssets bg)
		{
			this.SetBackgroundLayers(bg.BgLayers);
			this.SetForegroundLayer(bg.FgLayer);
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x00179B80 File Offset: 0x00177D80
		[NullableContext(1)]
		private void SetBackgroundLayers(IReadOnlyList<string> backgroundLayers)
		{
			for (int i = 0; i < backgroundLayers.Count; i++)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Layer_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i, "D2");
				string text = defaultInterpolatedStringHandler.ToStringAndClear();
				this.AddLayer(text, backgroundLayers[i]);
			}
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x00179BD6 File Offset: 0x00177DD6
		[NullableContext(2)]
		private void SetForegroundLayer(string foregroundLayer)
		{
			if (foregroundLayer == null)
			{
				return;
			}
			this.AddLayer("Foreground", foregroundLayer);
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x00179BE8 File Offset: 0x00177DE8
		[NullableContext(1)]
		private void AddLayer(string layerName, string layerPath)
		{
			Node nodeOrNull = base.GetNodeOrNull(layerName);
			if (nodeOrNull == null)
			{
				throw new InvalidOperationException("Layer node='" + layerName + "' not found in combat background scene.");
			}
			Control control = PreloadManager.Cache.GetScene(layerPath).Instantiate<Control>(0L);
			control.Visible = true;
			nodeOrNull.AddChildSafely(control);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060032BB RID: 12987 RVA: 0x00179C3C File Offset: 0x00177E3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCombatBackground.MethodName.SetForegroundLayer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "foregroundLayer", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCombatBackground.MethodName.AddLayer, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "layerName", 0L, "", 6L, false),
					new PropertyInfo(4L, "layerPath", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060032BC RID: 12988 RVA: 0x00179D1C File Offset: 0x00177F1C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCombatBackground.MethodName.SetForegroundLayer && args.Count == 1)
			{
				this.SetForegroundLayer(VariantUtils.ConvertTo<string>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCombatBackground.MethodName.AddLayer && args.Count == 2)
			{
				this.AddLayer(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060032BD RID: 12989 RVA: 0x00179DA5 File Offset: 0x00177FA5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCombatBackground.MethodName.SetForegroundLayer || (ref method) == NCombatBackground.MethodName.AddLayer || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060032BE RID: 12990 RVA: 0x00179DCC File Offset: 0x00177FCC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060032BF RID: 12991 RVA: 0x00179DD5 File Offset: 0x00177FD5
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001677 RID: 5751
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'SetForegroundLayer' method.
			/// </summary>
			// Token: 0x0400578F RID: 22415
			public static readonly StringName SetForegroundLayer = "SetForegroundLayer";

			/// <summary>
			/// Cached name for the 'AddLayer' method.
			/// </summary>
			// Token: 0x04005790 RID: 22416
			public static readonly StringName AddLayer = "AddLayer";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001678 RID: 5752
		public class PropertyName : Control.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001679 RID: 5753
		public class SignalName : Control.SignalName
		{
		}
	}
}

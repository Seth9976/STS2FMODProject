using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Map
{
	/// <summary>
	/// Exists solely to move the y position of the map asset when the aspect ratio becomes narrower than 16:9
	/// </summary>
	// Token: 0x020002EF RID: 751
	[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapBg.cs")]
	public class NMapBg : VBoxContainer
	{
		// Token: 0x06002858 RID: 10328 RVA: 0x00114F14 File Offset: 0x00113114
		public override void _Ready()
		{
			this._mapTop = base.GetNode<TextureRect>("MapTop");
			this._mapMid = base.GetNode<TextureRect>("MapMid");
			this._mapBot = base.GetNode<TextureRect>("MapBot");
			this._drawings = base.GetNode<NMapDrawings>("%Drawings");
			this._window = base.GetTree().Root;
			this._window.Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
			this.OnWindowChange();
			this._offsetX = base.Position.X;
			base.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChanged)), 0U);
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x00114FE2 File Offset: 0x001131E2
		[NullableContext(1)]
		public void Initialize(IRunState runState)
		{
			this._runState = runState;
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x00114FEC File Offset: 0x001131EC
		private void OnVisibilityChanged()
		{
			ActModel act = this._runState.Act;
			this._mapTop.Texture = act.MapTopBg;
			this._mapMid.Texture = act.MapMidBg;
			this._mapBot.Texture = act.MapBotBg;
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x00115038 File Offset: 0x00113238
		private void OnWindowChange()
		{
			float num = Math.Max(1.3333334f, (float)this._window.Size.X / (float)this._window.Size.Y);
			if (num < 1.7777778f)
			{
				float num2 = (num - 1.3333334f) / 0.44444442f;
				base.Position = new Vector2(this._offsetX, Mathf.Remap(Ease.CubicOut(num2), 0f, 1f, -1540f, -1620f));
			}
			else
			{
				base.Position = new Vector2(this._offsetX, -1620f);
			}
			this._drawings.RepositionBasedOnBackground(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600285C RID: 10332 RVA: 0x001150E0 File Offset: 0x001132E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NMapBg.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapBg.MethodName.OnVisibilityChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapBg.MethodName.OnWindowChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600285D RID: 10333 RVA: 0x00115184 File Offset: 0x00113384
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapBg.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapBg.MethodName.OnVisibilityChanged && args.Count == 0)
			{
				this.OnVisibilityChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapBg.MethodName.OnWindowChange && args.Count == 0)
			{
				this.OnWindowChange();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600285E RID: 10334 RVA: 0x00115209 File Offset: 0x00113409
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapBg.MethodName._Ready || (ref method) == NMapBg.MethodName.OnVisibilityChanged || (ref method) == NMapBg.MethodName.OnWindowChange || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600285F RID: 10335 RVA: 0x00115240 File Offset: 0x00113440
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapBg.PropertyName._mapTop)
			{
				this._mapTop = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapBg.PropertyName._mapMid)
			{
				this._mapMid = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapBg.PropertyName._mapBot)
			{
				this._mapBot = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapBg.PropertyName._drawings)
			{
				this._drawings = VariantUtils.ConvertTo<NMapDrawings>(ref value);
				return true;
			}
			if ((ref name) == NMapBg.PropertyName._window)
			{
				this._window = VariantUtils.ConvertTo<Window>(ref value);
				return true;
			}
			if ((ref name) == NMapBg.PropertyName._offsetX)
			{
				this._offsetX = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002860 RID: 10336 RVA: 0x001152F8 File Offset: 0x001134F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapBg.PropertyName._mapTop)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._mapTop);
				return true;
			}
			if ((ref name) == NMapBg.PropertyName._mapMid)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._mapMid);
				return true;
			}
			if ((ref name) == NMapBg.PropertyName._mapBot)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._mapBot);
				return true;
			}
			if ((ref name) == NMapBg.PropertyName._drawings)
			{
				value = VariantUtils.CreateFrom<NMapDrawings>(ref this._drawings);
				return true;
			}
			if ((ref name) == NMapBg.PropertyName._window)
			{
				value = VariantUtils.CreateFrom<Window>(ref this._window);
				return true;
			}
			if ((ref name) == NMapBg.PropertyName._offsetX)
			{
				value = VariantUtils.CreateFrom<float>(ref this._offsetX);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002861 RID: 10337 RVA: 0x001153D0 File Offset: 0x001135D0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NMapBg.PropertyName._mapTop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapBg.PropertyName._mapMid, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapBg.PropertyName._mapBot, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapBg.PropertyName._drawings, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapBg.PropertyName._window, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMapBg.PropertyName._offsetX, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002862 RID: 10338 RVA: 0x001154AC File Offset: 0x001136AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NMapBg.PropertyName._mapTop, Variant.From<TextureRect>(ref this._mapTop));
			info.AddProperty(NMapBg.PropertyName._mapMid, Variant.From<TextureRect>(ref this._mapMid));
			info.AddProperty(NMapBg.PropertyName._mapBot, Variant.From<TextureRect>(ref this._mapBot));
			info.AddProperty(NMapBg.PropertyName._drawings, Variant.From<NMapDrawings>(ref this._drawings));
			info.AddProperty(NMapBg.PropertyName._window, Variant.From<Window>(ref this._window));
			info.AddProperty(NMapBg.PropertyName._offsetX, Variant.From<float>(ref this._offsetX));
		}

		/// <inheritdoc />
		// Token: 0x06002863 RID: 10339 RVA: 0x00115544 File Offset: 0x00113744
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapBg.PropertyName._mapTop, ref variant))
			{
				this._mapTop = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapBg.PropertyName._mapMid, ref variant2))
			{
				this._mapMid = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMapBg.PropertyName._mapBot, ref variant3))
			{
				this._mapBot = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMapBg.PropertyName._drawings, ref variant4))
			{
				this._drawings = variant4.As<NMapDrawings>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMapBg.PropertyName._window, ref variant5))
			{
				this._window = variant5.As<Window>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMapBg.PropertyName._offsetX, ref variant6))
			{
				this._offsetX = variant6.As<float>();
			}
		}

		// Token: 0x04000F02 RID: 3842
		[Nullable(1)]
		private IRunState _runState;

		// Token: 0x04000F03 RID: 3843
		[Nullable(1)]
		private TextureRect _mapTop;

		// Token: 0x04000F04 RID: 3844
		[Nullable(1)]
		private TextureRect _mapMid;

		// Token: 0x04000F05 RID: 3845
		[Nullable(1)]
		private TextureRect _mapBot;

		// Token: 0x04000F06 RID: 3846
		[Nullable(1)]
		private NMapDrawings _drawings;

		// Token: 0x04000F07 RID: 3847
		[Nullable(1)]
		private Window _window;

		// Token: 0x04000F08 RID: 3848
		private const float _sixteenByNine = 1.7777778f;

		// Token: 0x04000F09 RID: 3849
		private const float _fourByThree = 1.3333334f;

		// Token: 0x04000F0A RID: 3850
		private const float _defaultY = -1620f;

		// Token: 0x04000F0B RID: 3851
		private const float _adjustY = -1540f;

		// Token: 0x04000F0C RID: 3852
		private float _offsetX;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200149E RID: 5278
		public class MethodName : VBoxContainer.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004E40 RID: 20032
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnVisibilityChanged' method.
			/// </summary>
			// Token: 0x04004E41 RID: 20033
			public static readonly StringName OnVisibilityChanged = "OnVisibilityChanged";

			/// <summary>
			/// Cached name for the 'OnWindowChange' method.
			/// </summary>
			// Token: 0x04004E42 RID: 20034
			public static readonly StringName OnWindowChange = "OnWindowChange";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200149F RID: 5279
		public class PropertyName : VBoxContainer.PropertyName
		{
			/// <summary>
			/// Cached name for the '_mapTop' field.
			/// </summary>
			// Token: 0x04004E43 RID: 20035
			public static readonly StringName _mapTop = "_mapTop";

			/// <summary>
			/// Cached name for the '_mapMid' field.
			/// </summary>
			// Token: 0x04004E44 RID: 20036
			public static readonly StringName _mapMid = "_mapMid";

			/// <summary>
			/// Cached name for the '_mapBot' field.
			/// </summary>
			// Token: 0x04004E45 RID: 20037
			public static readonly StringName _mapBot = "_mapBot";

			/// <summary>
			/// Cached name for the '_drawings' field.
			/// </summary>
			// Token: 0x04004E46 RID: 20038
			public static readonly StringName _drawings = "_drawings";

			/// <summary>
			/// Cached name for the '_window' field.
			/// </summary>
			// Token: 0x04004E47 RID: 20039
			public static readonly StringName _window = "_window";

			/// <summary>
			/// Cached name for the '_offsetX' field.
			/// </summary>
			// Token: 0x04004E48 RID: 20040
			public static readonly StringName _offsetX = "_offsetX";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020014A0 RID: 5280
		public class SignalName : VBoxContainer.SignalName
		{
		}
	}
}

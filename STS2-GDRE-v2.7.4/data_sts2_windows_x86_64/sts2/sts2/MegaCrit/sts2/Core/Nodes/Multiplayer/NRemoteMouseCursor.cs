using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;

namespace MegaCrit.Sts2.Core.Nodes.Multiplayer
{
	// Token: 0x02000397 RID: 919
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Multiplayer/NRemoteMouseCursor.cs")]
	public class NRemoteMouseCursor : Control
	{
		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06003702 RID: 14082 RVA: 0x0019CBB3 File Offset: 0x0019ADB3
		// (set) Token: 0x06003703 RID: 14083 RVA: 0x0019CBBB File Offset: 0x0019ADBB
		public ulong PlayerId { get; private set; }

		// Token: 0x06003704 RID: 14084 RVA: 0x0019CBC4 File Offset: 0x0019ADC4
		public static NRemoteMouseCursor Create(ulong playerId)
		{
			NRemoteMouseCursor nremoteMouseCursor = PreloadManager.Cache.GetAsset<PackedScene>(SceneHelper.GetScenePath("ui/multiplayer/remote_mouse_cursor")).Instantiate<NRemoteMouseCursor>(0L);
			nremoteMouseCursor.PlayerId = playerId;
			return nremoteMouseCursor;
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x0019CBF8 File Offset: 0x0019ADF8
		public override void _Ready()
		{
			this._textureRect = base.GetNode<TextureRect>("TextureRect");
			this._defaultHotspot = -this._textureRect.Position;
			this._drawingHotspot = NMapDrawings.drawingCursorHotspot;
			this._erasingHotspot = NMapDrawings.erasingCursorHotspot;
			base.ProcessMode = 4L;
			this._defaultCursorTexture = ImageTexture.CreateFromImage(this._defaultCursorImage);
			this._tiltedCursorTexture = ImageTexture.CreateFromImage(this._tiltedCursorImage);
			this._defaultDrawingTexture = ImageTexture.CreateFromImage(this._defaultDrawingImage);
			this._tiltedDrawingTexture = ImageTexture.CreateFromImage(this._tiltedDrawingImage);
			this._defaultErasingTexture = ImageTexture.CreateFromImage(this._defaultErasingImage);
			this._tiltedErasingTexture = ImageTexture.CreateFromImage(this._tiltedErasingImage);
			base.GetViewport().Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.RefreshSize)), 0U);
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x0019CCD8 File Offset: 0x0019AED8
		public void SetNextPosition(Vector2 position)
		{
			if (this._nextPosition == null)
			{
				this._nextPosition = new Vector2?(position);
			}
			this._previousPosition = this._nextPosition;
			this._nextPosition = new Vector2?(position);
			this._lastPositionUpdateMsec = Time.GetTicksMsec();
			base.ProcessMode = 0L;
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x0019CD2C File Offset: 0x0019AF2C
		public override void _Process(double delta)
		{
			if (this._previousPosition != null && this._nextPosition != null)
			{
				float num = (Time.GetTicksMsec() - this._lastPositionUpdateMsec) / 50f;
				base.Position = this._previousPosition.Value.Lerp(this._nextPosition.Value, Mathf.Clamp(num, 0f, 1f));
				if (num >= 1f)
				{
					base.ProcessMode = 4L;
				}
			}
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x0019CDAC File Offset: 0x0019AFAC
		public void UpdateImage(bool isDown, DrawingMode drawingMode)
		{
			this._textureRect.Texture = this.GetTexture(isDown, drawingMode);
			this._drawingMode = drawingMode;
			this.RefreshSize();
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x0019CDD0 File Offset: 0x0019AFD0
		private Vector2 GetHotspot(DrawingMode drawingMode)
		{
			Vector2 vector;
			switch (drawingMode)
			{
			case DrawingMode.None:
				vector = -this._defaultHotspot;
				break;
			case DrawingMode.Drawing:
				vector = -this._drawingHotspot;
				break;
			case DrawingMode.Erasing:
				vector = -this._erasingHotspot;
				break;
			default:
				<PrivateImplementationDetails>.ThrowSwitchExpressionException(drawingMode);
				break;
			}
			return vector;
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x0019CE28 File Offset: 0x0019B028
		private Texture2D GetTexture(bool isDown, DrawingMode drawingMode)
		{
			ImageTexture imageTexture;
			switch (drawingMode)
			{
			case DrawingMode.None:
				imageTexture = (isDown ? this._tiltedCursorTexture : this._defaultCursorTexture);
				break;
			case DrawingMode.Drawing:
				imageTexture = (isDown ? this._defaultDrawingTexture : this._tiltedDrawingTexture);
				break;
			case DrawingMode.Erasing:
				imageTexture = (isDown ? this._defaultErasingTexture : this._tiltedErasingTexture);
				break;
			default:
				<PrivateImplementationDetails>.ThrowSwitchExpressionException(drawingMode);
				break;
			}
			return imageTexture;
		}

		/// <summary>
		/// Resizes remote cursors to match our cursor size.
		/// </summary>
		// Token: 0x0600370B RID: 14091 RVA: 0x0019CE94 File Offset: 0x0019B094
		public void RefreshSize()
		{
			if (OS.GetName() == "Windows")
			{
				int num = DisplayServer.ScreenGetDpi(-1);
				float num2 = (float)num / 96f;
				Vector2 vector = base.GetViewport().GetStretchTransform().Scale.Inverse();
				Vector2 size = this._textureRect.Texture.GetSize();
				Vector2 vector2 = size * vector * num2;
				this._textureRect.Size = vector2;
				this._textureRect.Position = this.GetHotspot(this._drawingMode) * vector * num2;
				return;
			}
			this._textureRect.Size = this._textureRect.Texture.GetSize();
			this._textureRect.Position = this.GetHotspot(this._drawingMode);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600370C RID: 14092 RVA: 0x0019CF64 File Offset: 0x0019B164
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NRemoteMouseCursor.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "playerId", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursor.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRemoteMouseCursor.MethodName.SetNextPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "position", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursor.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursor.MethodName.UpdateImage, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isDown", 0L, "", 6L, false),
					new PropertyInfo(2L, "drawingMode", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursor.MethodName.GetHotspot, new PropertyInfo(5L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "drawingMode", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursor.MethodName.GetTexture, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Texture2D"), false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isDown", 0L, "", 6L, false),
					new PropertyInfo(2L, "drawingMode", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NRemoteMouseCursor.MethodName.RefreshSize, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600370D RID: 14093 RVA: 0x0019D228 File Offset: 0x0019B428
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRemoteMouseCursor.MethodName.Create && args.Count == 1)
			{
				NRemoteMouseCursor nremoteMouseCursor = NRemoteMouseCursor.Create(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<NRemoteMouseCursor>(ref nremoteMouseCursor);
				return true;
			}
			if ((ref method) == NRemoteMouseCursor.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursor.MethodName.SetNextPosition && args.Count == 1)
			{
				this.SetNextPosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursor.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursor.MethodName.UpdateImage && args.Count == 2)
			{
				this.UpdateImage(VariantUtils.ConvertTo<bool>(args[0]), VariantUtils.ConvertTo<DrawingMode>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRemoteMouseCursor.MethodName.GetHotspot && args.Count == 1)
			{
				Vector2 hotspot = this.GetHotspot(VariantUtils.ConvertTo<DrawingMode>(args[0]));
				ret = VariantUtils.CreateFrom<Vector2>(ref hotspot);
				return true;
			}
			if ((ref method) == NRemoteMouseCursor.MethodName.GetTexture && args.Count == 2)
			{
				Texture2D texture = this.GetTexture(VariantUtils.ConvertTo<bool>(args[0]), VariantUtils.ConvertTo<DrawingMode>(args[1]));
				ret = VariantUtils.CreateFrom<Texture2D>(ref texture);
				return true;
			}
			if ((ref method) == NRemoteMouseCursor.MethodName.RefreshSize && args.Count == 0)
			{
				this.RefreshSize();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x0019D3E8 File Offset: 0x0019B5E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRemoteMouseCursor.MethodName.Create && args.Count == 1)
			{
				NRemoteMouseCursor nremoteMouseCursor = NRemoteMouseCursor.Create(VariantUtils.ConvertTo<ulong>(args[0]));
				ret = VariantUtils.CreateFrom<NRemoteMouseCursor>(ref nremoteMouseCursor);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600370F RID: 14095 RVA: 0x0019D438 File Offset: 0x0019B638
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRemoteMouseCursor.MethodName.Create || (ref method) == NRemoteMouseCursor.MethodName._Ready || (ref method) == NRemoteMouseCursor.MethodName.SetNextPosition || (ref method) == NRemoteMouseCursor.MethodName._Process || (ref method) == NRemoteMouseCursor.MethodName.UpdateImage || (ref method) == NRemoteMouseCursor.MethodName.GetHotspot || (ref method) == NRemoteMouseCursor.MethodName.GetTexture || (ref method) == NRemoteMouseCursor.MethodName.RefreshSize || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003710 RID: 14096 RVA: 0x0019D4C4 File Offset: 0x0019B6C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRemoteMouseCursor.PropertyName.PlayerId)
			{
				this.PlayerId = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._textureRect)
			{
				this._textureRect = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._lastPositionUpdateMsec)
			{
				this._lastPositionUpdateMsec = VariantUtils.ConvertTo<ulong>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultHotspot)
			{
				this._defaultHotspot = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._drawingHotspot)
			{
				this._drawingHotspot = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._erasingHotspot)
			{
				this._erasingHotspot = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultCursorImage)
			{
				this._defaultCursorImage = VariantUtils.ConvertTo<Image>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._tiltedCursorImage)
			{
				this._tiltedCursorImage = VariantUtils.ConvertTo<Image>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultDrawingImage)
			{
				this._defaultDrawingImage = VariantUtils.ConvertTo<Image>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._tiltedDrawingImage)
			{
				this._tiltedDrawingImage = VariantUtils.ConvertTo<Image>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultErasingImage)
			{
				this._defaultErasingImage = VariantUtils.ConvertTo<Image>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._tiltedErasingImage)
			{
				this._tiltedErasingImage = VariantUtils.ConvertTo<Image>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultCursorTexture)
			{
				this._defaultCursorTexture = VariantUtils.ConvertTo<ImageTexture>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._tiltedCursorTexture)
			{
				this._tiltedCursorTexture = VariantUtils.ConvertTo<ImageTexture>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultDrawingTexture)
			{
				this._defaultDrawingTexture = VariantUtils.ConvertTo<ImageTexture>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._tiltedDrawingTexture)
			{
				this._tiltedDrawingTexture = VariantUtils.ConvertTo<ImageTexture>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultErasingTexture)
			{
				this._defaultErasingTexture = VariantUtils.ConvertTo<ImageTexture>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._tiltedErasingTexture)
			{
				this._tiltedErasingTexture = VariantUtils.ConvertTo<ImageTexture>(ref value);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._drawingMode)
			{
				this._drawingMode = VariantUtils.ConvertTo<DrawingMode>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003711 RID: 14097 RVA: 0x0019D6DC File Offset: 0x0019B8DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRemoteMouseCursor.PropertyName.PlayerId)
			{
				ulong playerId = this.PlayerId;
				value = VariantUtils.CreateFrom<ulong>(ref playerId);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._textureRect)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._textureRect);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._lastPositionUpdateMsec)
			{
				value = VariantUtils.CreateFrom<ulong>(ref this._lastPositionUpdateMsec);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultHotspot)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._defaultHotspot);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._drawingHotspot)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._drawingHotspot);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._erasingHotspot)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._erasingHotspot);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultCursorImage)
			{
				value = VariantUtils.CreateFrom<Image>(ref this._defaultCursorImage);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._tiltedCursorImage)
			{
				value = VariantUtils.CreateFrom<Image>(ref this._tiltedCursorImage);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultDrawingImage)
			{
				value = VariantUtils.CreateFrom<Image>(ref this._defaultDrawingImage);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._tiltedDrawingImage)
			{
				value = VariantUtils.CreateFrom<Image>(ref this._tiltedDrawingImage);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultErasingImage)
			{
				value = VariantUtils.CreateFrom<Image>(ref this._defaultErasingImage);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._tiltedErasingImage)
			{
				value = VariantUtils.CreateFrom<Image>(ref this._tiltedErasingImage);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultCursorTexture)
			{
				value = VariantUtils.CreateFrom<ImageTexture>(ref this._defaultCursorTexture);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._tiltedCursorTexture)
			{
				value = VariantUtils.CreateFrom<ImageTexture>(ref this._tiltedCursorTexture);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultDrawingTexture)
			{
				value = VariantUtils.CreateFrom<ImageTexture>(ref this._defaultDrawingTexture);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._tiltedDrawingTexture)
			{
				value = VariantUtils.CreateFrom<ImageTexture>(ref this._tiltedDrawingTexture);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._defaultErasingTexture)
			{
				value = VariantUtils.CreateFrom<ImageTexture>(ref this._defaultErasingTexture);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._tiltedErasingTexture)
			{
				value = VariantUtils.CreateFrom<ImageTexture>(ref this._tiltedErasingTexture);
				return true;
			}
			if ((ref name) == NRemoteMouseCursor.PropertyName._drawingMode)
			{
				value = VariantUtils.CreateFrom<DrawingMode>(ref this._drawingMode);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003712 RID: 14098 RVA: 0x0019D954 File Offset: 0x0019BB54
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._textureRect, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRemoteMouseCursor.PropertyName.PlayerId, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRemoteMouseCursor.PropertyName._lastPositionUpdateMsec, 0L, "", 4096L, false),
				new PropertyInfo(5L, NRemoteMouseCursor.PropertyName._defaultHotspot, 0L, "", 4096L, false),
				new PropertyInfo(5L, NRemoteMouseCursor.PropertyName._drawingHotspot, 0L, "", 4096L, false),
				new PropertyInfo(5L, NRemoteMouseCursor.PropertyName._erasingHotspot, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._defaultCursorImage, 17L, "Image", 4102L, true),
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._tiltedCursorImage, 17L, "Image", 4102L, true),
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._defaultDrawingImage, 17L, "Image", 4102L, true),
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._tiltedDrawingImage, 17L, "Image", 4102L, true),
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._defaultErasingImage, 17L, "Image", 4102L, true),
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._tiltedErasingImage, 17L, "Image", 4102L, true),
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._defaultCursorTexture, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._tiltedCursorTexture, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._defaultDrawingTexture, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._tiltedDrawingTexture, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._defaultErasingTexture, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRemoteMouseCursor.PropertyName._tiltedErasingTexture, 0L, "", 4096L, false),
				new PropertyInfo(2L, NRemoteMouseCursor.PropertyName._drawingMode, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003713 RID: 14099 RVA: 0x0019DBDC File Offset: 0x0019BDDC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName playerId = NRemoteMouseCursor.PropertyName.PlayerId;
			ulong playerId2 = this.PlayerId;
			info.AddProperty(playerId, Variant.From<ulong>(ref playerId2));
			info.AddProperty(NRemoteMouseCursor.PropertyName._textureRect, Variant.From<TextureRect>(ref this._textureRect));
			info.AddProperty(NRemoteMouseCursor.PropertyName._lastPositionUpdateMsec, Variant.From<ulong>(ref this._lastPositionUpdateMsec));
			info.AddProperty(NRemoteMouseCursor.PropertyName._defaultHotspot, Variant.From<Vector2>(ref this._defaultHotspot));
			info.AddProperty(NRemoteMouseCursor.PropertyName._drawingHotspot, Variant.From<Vector2>(ref this._drawingHotspot));
			info.AddProperty(NRemoteMouseCursor.PropertyName._erasingHotspot, Variant.From<Vector2>(ref this._erasingHotspot));
			info.AddProperty(NRemoteMouseCursor.PropertyName._defaultCursorImage, Variant.From<Image>(ref this._defaultCursorImage));
			info.AddProperty(NRemoteMouseCursor.PropertyName._tiltedCursorImage, Variant.From<Image>(ref this._tiltedCursorImage));
			info.AddProperty(NRemoteMouseCursor.PropertyName._defaultDrawingImage, Variant.From<Image>(ref this._defaultDrawingImage));
			info.AddProperty(NRemoteMouseCursor.PropertyName._tiltedDrawingImage, Variant.From<Image>(ref this._tiltedDrawingImage));
			info.AddProperty(NRemoteMouseCursor.PropertyName._defaultErasingImage, Variant.From<Image>(ref this._defaultErasingImage));
			info.AddProperty(NRemoteMouseCursor.PropertyName._tiltedErasingImage, Variant.From<Image>(ref this._tiltedErasingImage));
			info.AddProperty(NRemoteMouseCursor.PropertyName._defaultCursorTexture, Variant.From<ImageTexture>(ref this._defaultCursorTexture));
			info.AddProperty(NRemoteMouseCursor.PropertyName._tiltedCursorTexture, Variant.From<ImageTexture>(ref this._tiltedCursorTexture));
			info.AddProperty(NRemoteMouseCursor.PropertyName._defaultDrawingTexture, Variant.From<ImageTexture>(ref this._defaultDrawingTexture));
			info.AddProperty(NRemoteMouseCursor.PropertyName._tiltedDrawingTexture, Variant.From<ImageTexture>(ref this._tiltedDrawingTexture));
			info.AddProperty(NRemoteMouseCursor.PropertyName._defaultErasingTexture, Variant.From<ImageTexture>(ref this._defaultErasingTexture));
			info.AddProperty(NRemoteMouseCursor.PropertyName._tiltedErasingTexture, Variant.From<ImageTexture>(ref this._tiltedErasingTexture));
			info.AddProperty(NRemoteMouseCursor.PropertyName._drawingMode, Variant.From<DrawingMode>(ref this._drawingMode));
		}

		/// <inheritdoc />
		// Token: 0x06003714 RID: 14100 RVA: 0x0019DD98 File Offset: 0x0019BF98
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName.PlayerId, ref variant))
			{
				this.PlayerId = variant.As<ulong>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._textureRect, ref variant2))
			{
				this._textureRect = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._lastPositionUpdateMsec, ref variant3))
			{
				this._lastPositionUpdateMsec = variant3.As<ulong>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultHotspot, ref variant4))
			{
				this._defaultHotspot = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._drawingHotspot, ref variant5))
			{
				this._drawingHotspot = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._erasingHotspot, ref variant6))
			{
				this._erasingHotspot = variant6.As<Vector2>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultCursorImage, ref variant7))
			{
				this._defaultCursorImage = variant7.As<Image>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._tiltedCursorImage, ref variant8))
			{
				this._tiltedCursorImage = variant8.As<Image>();
			}
			Variant variant9;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultDrawingImage, ref variant9))
			{
				this._defaultDrawingImage = variant9.As<Image>();
			}
			Variant variant10;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._tiltedDrawingImage, ref variant10))
			{
				this._tiltedDrawingImage = variant10.As<Image>();
			}
			Variant variant11;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultErasingImage, ref variant11))
			{
				this._defaultErasingImage = variant11.As<Image>();
			}
			Variant variant12;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._tiltedErasingImage, ref variant12))
			{
				this._tiltedErasingImage = variant12.As<Image>();
			}
			Variant variant13;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultCursorTexture, ref variant13))
			{
				this._defaultCursorTexture = variant13.As<ImageTexture>();
			}
			Variant variant14;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._tiltedCursorTexture, ref variant14))
			{
				this._tiltedCursorTexture = variant14.As<ImageTexture>();
			}
			Variant variant15;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultDrawingTexture, ref variant15))
			{
				this._defaultDrawingTexture = variant15.As<ImageTexture>();
			}
			Variant variant16;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._tiltedDrawingTexture, ref variant16))
			{
				this._tiltedDrawingTexture = variant16.As<ImageTexture>();
			}
			Variant variant17;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultErasingTexture, ref variant17))
			{
				this._defaultErasingTexture = variant17.As<ImageTexture>();
			}
			Variant variant18;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._tiltedErasingTexture, ref variant18))
			{
				this._tiltedErasingTexture = variant18.As<ImageTexture>();
			}
			Variant variant19;
			if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._drawingMode, ref variant19))
			{
				this._drawingMode = variant19.As<DrawingMode>();
			}
		}

		// Token: 0x0400154D RID: 5453
		private const string _scenePath = "ui/multiplayer/remote_mouse_cursor";

		// Token: 0x0400154E RID: 5454
		private TextureRect _textureRect;

		// Token: 0x04001550 RID: 5456
		private Vector2? _previousPosition;

		// Token: 0x04001551 RID: 5457
		private Vector2? _nextPosition;

		// Token: 0x04001552 RID: 5458
		private ulong _lastPositionUpdateMsec;

		// Token: 0x04001553 RID: 5459
		private Vector2 _defaultHotspot;

		// Token: 0x04001554 RID: 5460
		private Vector2 _drawingHotspot;

		// Token: 0x04001555 RID: 5461
		private Vector2 _erasingHotspot;

		// Token: 0x04001556 RID: 5462
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Image _defaultCursorImage;

		// Token: 0x04001557 RID: 5463
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Image _tiltedCursorImage;

		// Token: 0x04001558 RID: 5464
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Image _defaultDrawingImage;

		// Token: 0x04001559 RID: 5465
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Image _tiltedDrawingImage;

		// Token: 0x0400155A RID: 5466
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Image _defaultErasingImage;

		// Token: 0x0400155B RID: 5467
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Image _tiltedErasingImage;

		// Token: 0x0400155C RID: 5468
		private ImageTexture _defaultCursorTexture;

		// Token: 0x0400155D RID: 5469
		private ImageTexture _tiltedCursorTexture;

		// Token: 0x0400155E RID: 5470
		private ImageTexture _defaultDrawingTexture;

		// Token: 0x0400155F RID: 5471
		private ImageTexture _tiltedDrawingTexture;

		// Token: 0x04001560 RID: 5472
		private ImageTexture _defaultErasingTexture;

		// Token: 0x04001561 RID: 5473
		private ImageTexture _tiltedErasingTexture;

		// Token: 0x04001562 RID: 5474
		private DrawingMode _drawingMode;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200174E RID: 5966
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005AE0 RID: 23264
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005AE1 RID: 23265
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetNextPosition' method.
			/// </summary>
			// Token: 0x04005AE2 RID: 23266
			public static readonly StringName SetNextPosition = "SetNextPosition";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005AE3 RID: 23267
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'UpdateImage' method.
			/// </summary>
			// Token: 0x04005AE4 RID: 23268
			public static readonly StringName UpdateImage = "UpdateImage";

			/// <summary>
			/// Cached name for the 'GetHotspot' method.
			/// </summary>
			// Token: 0x04005AE5 RID: 23269
			public static readonly StringName GetHotspot = "GetHotspot";

			/// <summary>
			/// Cached name for the 'GetTexture' method.
			/// </summary>
			// Token: 0x04005AE6 RID: 23270
			public static readonly StringName GetTexture = "GetTexture";

			/// <summary>
			/// Cached name for the 'RefreshSize' method.
			/// </summary>
			// Token: 0x04005AE7 RID: 23271
			public static readonly StringName RefreshSize = "RefreshSize";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200174F RID: 5967
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'PlayerId' property.
			/// </summary>
			// Token: 0x04005AE8 RID: 23272
			public static readonly StringName PlayerId = "PlayerId";

			/// <summary>
			/// Cached name for the '_textureRect' field.
			/// </summary>
			// Token: 0x04005AE9 RID: 23273
			public static readonly StringName _textureRect = "_textureRect";

			/// <summary>
			/// Cached name for the '_lastPositionUpdateMsec' field.
			/// </summary>
			// Token: 0x04005AEA RID: 23274
			public static readonly StringName _lastPositionUpdateMsec = "_lastPositionUpdateMsec";

			/// <summary>
			/// Cached name for the '_defaultHotspot' field.
			/// </summary>
			// Token: 0x04005AEB RID: 23275
			public static readonly StringName _defaultHotspot = "_defaultHotspot";

			/// <summary>
			/// Cached name for the '_drawingHotspot' field.
			/// </summary>
			// Token: 0x04005AEC RID: 23276
			public static readonly StringName _drawingHotspot = "_drawingHotspot";

			/// <summary>
			/// Cached name for the '_erasingHotspot' field.
			/// </summary>
			// Token: 0x04005AED RID: 23277
			public static readonly StringName _erasingHotspot = "_erasingHotspot";

			/// <summary>
			/// Cached name for the '_defaultCursorImage' field.
			/// </summary>
			// Token: 0x04005AEE RID: 23278
			public static readonly StringName _defaultCursorImage = "_defaultCursorImage";

			/// <summary>
			/// Cached name for the '_tiltedCursorImage' field.
			/// </summary>
			// Token: 0x04005AEF RID: 23279
			public static readonly StringName _tiltedCursorImage = "_tiltedCursorImage";

			/// <summary>
			/// Cached name for the '_defaultDrawingImage' field.
			/// </summary>
			// Token: 0x04005AF0 RID: 23280
			public static readonly StringName _defaultDrawingImage = "_defaultDrawingImage";

			/// <summary>
			/// Cached name for the '_tiltedDrawingImage' field.
			/// </summary>
			// Token: 0x04005AF1 RID: 23281
			public static readonly StringName _tiltedDrawingImage = "_tiltedDrawingImage";

			/// <summary>
			/// Cached name for the '_defaultErasingImage' field.
			/// </summary>
			// Token: 0x04005AF2 RID: 23282
			public static readonly StringName _defaultErasingImage = "_defaultErasingImage";

			/// <summary>
			/// Cached name for the '_tiltedErasingImage' field.
			/// </summary>
			// Token: 0x04005AF3 RID: 23283
			public static readonly StringName _tiltedErasingImage = "_tiltedErasingImage";

			/// <summary>
			/// Cached name for the '_defaultCursorTexture' field.
			/// </summary>
			// Token: 0x04005AF4 RID: 23284
			public static readonly StringName _defaultCursorTexture = "_defaultCursorTexture";

			/// <summary>
			/// Cached name for the '_tiltedCursorTexture' field.
			/// </summary>
			// Token: 0x04005AF5 RID: 23285
			public static readonly StringName _tiltedCursorTexture = "_tiltedCursorTexture";

			/// <summary>
			/// Cached name for the '_defaultDrawingTexture' field.
			/// </summary>
			// Token: 0x04005AF6 RID: 23286
			public static readonly StringName _defaultDrawingTexture = "_defaultDrawingTexture";

			/// <summary>
			/// Cached name for the '_tiltedDrawingTexture' field.
			/// </summary>
			// Token: 0x04005AF7 RID: 23287
			public static readonly StringName _tiltedDrawingTexture = "_tiltedDrawingTexture";

			/// <summary>
			/// Cached name for the '_defaultErasingTexture' field.
			/// </summary>
			// Token: 0x04005AF8 RID: 23288
			public static readonly StringName _defaultErasingTexture = "_defaultErasingTexture";

			/// <summary>
			/// Cached name for the '_tiltedErasingTexture' field.
			/// </summary>
			// Token: 0x04005AF9 RID: 23289
			public static readonly StringName _tiltedErasingTexture = "_tiltedErasingTexture";

			/// <summary>
			/// Cached name for the '_drawingMode' field.
			/// </summary>
			// Token: 0x04005AFA RID: 23290
			public static readonly StringName _drawingMode = "_drawingMode";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001750 RID: 5968
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}

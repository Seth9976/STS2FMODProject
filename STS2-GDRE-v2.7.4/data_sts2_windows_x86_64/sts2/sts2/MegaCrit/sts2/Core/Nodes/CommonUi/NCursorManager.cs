using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace MegaCrit.Sts2.Core.Nodes.CommonUi
{
	/// <summary>
	/// Manages the local mouse cursor image and hotspot.
	/// Note: You'll see that Image is used all over this class instead of Texture2D. For some reason it's massively faster
	/// to use Image rather than Texture2D, so please keep it that way.
	/// Cursor images are exported scene dependencies to ensure they're loaded with the scene and won't be disposed.
	/// </summary>
	// Token: 0x020003D8 RID: 984
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/CommonUi/NCursorManager.cs")]
	public class NCursorManager : Node
	{
		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06003BAB RID: 15275 RVA: 0x001C8696 File Offset: 0x001C6896
		private Image CursorTilted
		{
			get
			{
				return this._overriddenCursorTilted ?? this._cursorTilted;
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06003BAC RID: 15276 RVA: 0x001C86A8 File Offset: 0x001C68A8
		private Image CursorNotTilted
		{
			get
			{
				return this._overriddenCursorNotTilted ?? this._cursorNotTilted;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06003BAD RID: 15277 RVA: 0x001C86BC File Offset: 0x001C68BC
		private Vector2 HotSpot
		{
			get
			{
				Vector2? overriddenHotSpot = this._overriddenHotSpot;
				if (overriddenHotSpot == null)
				{
					return NCursorManager._defaultHotSpot;
				}
				return overriddenHotSpot.GetValueOrDefault();
			}
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x001C86E6 File Offset: 0x001C68E6
		public override void _EnterTree()
		{
			Input.SetCustomMouseCursor(this._cursorInspect, 16L, new Vector2?(NCursorManager._inspectHotSpot));
			this.UpdateCursor();
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x001C8708 File Offset: 0x001C6908
		public override void _Ready()
		{
			NControllerManager.Instance.Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(delegate
			{
				this.SetIsUsingController(true);
			}), 0U);
			NControllerManager.Instance.Connect(NControllerManager.SignalName.MouseDetected, Callable.From(delegate
			{
				this.SetIsUsingController(false);
			}), 0U);
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x001C875C File Offset: 0x001C695C
		public override void _Input(InputEvent inputEvent)
		{
			InputEventMouseButton inputEventMouseButton = inputEvent as InputEventMouseButton;
			if (inputEventMouseButton != null && (inputEventMouseButton.ButtonIndex == 1L || inputEventMouseButton.ButtonIndex == 2L || inputEventMouseButton.ButtonIndex == 3L))
			{
				if (inputEventMouseButton.IsPressed() && !this._isDown)
				{
					this._isDown = true;
					this.UpdateCursor();
					return;
				}
				if (inputEventMouseButton.IsReleased() && this._isDown)
				{
					this._isDown = false;
					this.UpdateCursor();
				}
			}
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x001C87CC File Offset: 0x001C69CC
		public void StopOverridingCursor()
		{
			this._overriddenCursorTilted = null;
			this._overriddenCursorNotTilted = null;
			this._overriddenHotSpot = null;
			this.UpdateCursor();
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x001C87EE File Offset: 0x001C69EE
		public void OverrideCursor(Image cursorTilted, Image cursorNotTilted, Vector2 hotspot)
		{
			this._overriddenCursorTilted = cursorTilted;
			this._overriddenCursorNotTilted = cursorNotTilted;
			this._overriddenHotSpot = new Vector2?(hotspot);
			this.UpdateCursor();
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x001C8810 File Offset: 0x001C6A10
		private void UpdateCursor()
		{
			if (Input.MouseMode == 1L)
			{
				return;
			}
			Image image = (this._isDown ? this.CursorTilted : this.CursorNotTilted);
			if (image != this._lastSetCursor)
			{
				Input.SetCustomMouseCursor(image, 0L, new Vector2?(this.HotSpot));
				this._lastSetCursor = image;
			}
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x001C8861 File Offset: 0x001C6A61
		private void SetIsUsingController(bool isUsingController)
		{
			this._isUsingController = isUsingController;
			this.RefreshCursorShown();
		}

		/// <summary>
		/// Call this when you want to show or hide the cursor.
		/// Note that if the player is using a controller, the cursor will remain hidden even if show is true.
		/// </summary>
		// Token: 0x06003BB5 RID: 15285 RVA: 0x001C8870 File Offset: 0x001C6A70
		public void SetCursorShown(bool show)
		{
			this._shouldShowCursor = show;
			this.RefreshCursorShown();
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x001C8880 File Offset: 0x001C6A80
		private void RefreshCursorShown()
		{
			bool flag = !this._isUsingController && this._shouldShowCursor;
			Input.MouseMode = (flag ? 0L : 1L);
			if (!flag)
			{
				this._lastSetCursor = null;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003BB7 RID: 15287 RVA: 0x001C88B8 File Offset: 0x001C6AB8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NCursorManager.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCursorManager.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCursorManager.MethodName._Input, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "inputEvent", 0L, "", 6L, new StringName("InputEvent"), false)
				}, null),
				new MethodInfo(NCursorManager.MethodName.StopOverridingCursor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCursorManager.MethodName.OverrideCursor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cursorTilted", 0L, "", 6L, new StringName("Image"), false),
					new PropertyInfo(24L, "cursorNotTilted", 0L, "", 6L, new StringName("Image"), false),
					new PropertyInfo(5L, "hotspot", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCursorManager.MethodName.UpdateCursor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCursorManager.MethodName.SetIsUsingController, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isUsingController", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCursorManager.MethodName.SetCursorShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "show", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCursorManager.MethodName.RefreshCursorShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003BB8 RID: 15288 RVA: 0x001C8B6C File Offset: 0x001C6D6C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCursorManager.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCursorManager.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCursorManager.MethodName._Input && args.Count == 1)
			{
				this._Input(VariantUtils.ConvertTo<InputEvent>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCursorManager.MethodName.StopOverridingCursor && args.Count == 0)
			{
				this.StopOverridingCursor();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCursorManager.MethodName.OverrideCursor && args.Count == 3)
			{
				this.OverrideCursor(VariantUtils.ConvertTo<Image>(args[0]), VariantUtils.ConvertTo<Image>(args[1]), VariantUtils.ConvertTo<Vector2>(args[2]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCursorManager.MethodName.UpdateCursor && args.Count == 0)
			{
				this.UpdateCursor();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCursorManager.MethodName.SetIsUsingController && args.Count == 1)
			{
				this.SetIsUsingController(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCursorManager.MethodName.SetCursorShown && args.Count == 1)
			{
				this.SetCursorShown(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCursorManager.MethodName.RefreshCursorShown && args.Count == 0)
			{
				this.RefreshCursorShown();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003BB9 RID: 15289 RVA: 0x001C8D24 File Offset: 0x001C6F24
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCursorManager.MethodName._EnterTree || (ref method) == NCursorManager.MethodName._Ready || (ref method) == NCursorManager.MethodName._Input || (ref method) == NCursorManager.MethodName.StopOverridingCursor || (ref method) == NCursorManager.MethodName.OverrideCursor || (ref method) == NCursorManager.MethodName.UpdateCursor || (ref method) == NCursorManager.MethodName.SetIsUsingController || (ref method) == NCursorManager.MethodName.SetCursorShown || (ref method) == NCursorManager.MethodName.RefreshCursorShown || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003BBA RID: 15290 RVA: 0x001C8DC0 File Offset: 0x001C6FC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCursorManager.PropertyName._cursorTilted)
			{
				this._cursorTilted = VariantUtils.ConvertTo<Image>(ref value);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._cursorNotTilted)
			{
				this._cursorNotTilted = VariantUtils.ConvertTo<Image>(ref value);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._cursorInspect)
			{
				this._cursorInspect = VariantUtils.ConvertTo<Image>(ref value);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._overriddenCursorTilted)
			{
				this._overriddenCursorTilted = VariantUtils.ConvertTo<Image>(ref value);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._overriddenCursorNotTilted)
			{
				this._overriddenCursorNotTilted = VariantUtils.ConvertTo<Image>(ref value);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._lastSetCursor)
			{
				this._lastSetCursor = VariantUtils.ConvertTo<Image>(ref value);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._isDown)
			{
				this._isDown = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._isUsingController)
			{
				this._isUsingController = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._shouldShowCursor)
			{
				this._shouldShowCursor = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003BBB RID: 15291 RVA: 0x001C8EC8 File Offset: 0x001C70C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCursorManager.PropertyName.CursorTilted)
			{
				Image image = this.CursorTilted;
				value = VariantUtils.CreateFrom<Image>(ref image);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName.CursorNotTilted)
			{
				Image image = this.CursorNotTilted;
				value = VariantUtils.CreateFrom<Image>(ref image);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName.HotSpot)
			{
				Vector2 hotSpot = this.HotSpot;
				value = VariantUtils.CreateFrom<Vector2>(ref hotSpot);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._cursorTilted)
			{
				value = VariantUtils.CreateFrom<Image>(ref this._cursorTilted);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._cursorNotTilted)
			{
				value = VariantUtils.CreateFrom<Image>(ref this._cursorNotTilted);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._cursorInspect)
			{
				value = VariantUtils.CreateFrom<Image>(ref this._cursorInspect);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._overriddenCursorTilted)
			{
				value = VariantUtils.CreateFrom<Image>(ref this._overriddenCursorTilted);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._overriddenCursorNotTilted)
			{
				value = VariantUtils.CreateFrom<Image>(ref this._overriddenCursorNotTilted);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._lastSetCursor)
			{
				value = VariantUtils.CreateFrom<Image>(ref this._lastSetCursor);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._isDown)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDown);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._isUsingController)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isUsingController);
				return true;
			}
			if ((ref name) == NCursorManager.PropertyName._shouldShowCursor)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._shouldShowCursor);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003BBC RID: 15292 RVA: 0x001C9068 File Offset: 0x001C7268
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCursorManager.PropertyName._cursorTilted, 17L, "Image", 4102L, true),
				new PropertyInfo(24L, NCursorManager.PropertyName._cursorNotTilted, 17L, "Image", 4102L, true),
				new PropertyInfo(24L, NCursorManager.PropertyName._cursorInspect, 17L, "Image", 4102L, true),
				new PropertyInfo(24L, NCursorManager.PropertyName._overriddenCursorTilted, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCursorManager.PropertyName._overriddenCursorNotTilted, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCursorManager.PropertyName.CursorTilted, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCursorManager.PropertyName.CursorNotTilted, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCursorManager.PropertyName.HotSpot, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCursorManager.PropertyName._lastSetCursor, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCursorManager.PropertyName._isDown, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCursorManager.PropertyName._isUsingController, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCursorManager.PropertyName._shouldShowCursor, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003BBD RID: 15293 RVA: 0x001C9208 File Offset: 0x001C7408
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCursorManager.PropertyName._cursorTilted, Variant.From<Image>(ref this._cursorTilted));
			info.AddProperty(NCursorManager.PropertyName._cursorNotTilted, Variant.From<Image>(ref this._cursorNotTilted));
			info.AddProperty(NCursorManager.PropertyName._cursorInspect, Variant.From<Image>(ref this._cursorInspect));
			info.AddProperty(NCursorManager.PropertyName._overriddenCursorTilted, Variant.From<Image>(ref this._overriddenCursorTilted));
			info.AddProperty(NCursorManager.PropertyName._overriddenCursorNotTilted, Variant.From<Image>(ref this._overriddenCursorNotTilted));
			info.AddProperty(NCursorManager.PropertyName._lastSetCursor, Variant.From<Image>(ref this._lastSetCursor));
			info.AddProperty(NCursorManager.PropertyName._isDown, Variant.From<bool>(ref this._isDown));
			info.AddProperty(NCursorManager.PropertyName._isUsingController, Variant.From<bool>(ref this._isUsingController));
			info.AddProperty(NCursorManager.PropertyName._shouldShowCursor, Variant.From<bool>(ref this._shouldShowCursor));
		}

		/// <inheritdoc />
		// Token: 0x06003BBE RID: 15294 RVA: 0x001C92E4 File Offset: 0x001C74E4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCursorManager.PropertyName._cursorTilted, ref variant))
			{
				this._cursorTilted = variant.As<Image>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCursorManager.PropertyName._cursorNotTilted, ref variant2))
			{
				this._cursorNotTilted = variant2.As<Image>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCursorManager.PropertyName._cursorInspect, ref variant3))
			{
				this._cursorInspect = variant3.As<Image>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCursorManager.PropertyName._overriddenCursorTilted, ref variant4))
			{
				this._overriddenCursorTilted = variant4.As<Image>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCursorManager.PropertyName._overriddenCursorNotTilted, ref variant5))
			{
				this._overriddenCursorNotTilted = variant5.As<Image>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCursorManager.PropertyName._lastSetCursor, ref variant6))
			{
				this._lastSetCursor = variant6.As<Image>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCursorManager.PropertyName._isDown, ref variant7))
			{
				this._isDown = variant7.As<bool>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCursorManager.PropertyName._isUsingController, ref variant8))
			{
				this._isUsingController = variant8.As<bool>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCursorManager.PropertyName._shouldShowCursor, ref variant9))
			{
				this._shouldShowCursor = variant9.As<bool>();
			}
		}

		// Token: 0x04001752 RID: 5970
		private static readonly Vector2 _defaultHotSpot = new Vector2(14f, 5f);

		// Token: 0x04001753 RID: 5971
		private static readonly Vector2 _inspectHotSpot = new Vector2(12f, 12f);

		// Token: 0x04001754 RID: 5972
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Image _cursorTilted;

		// Token: 0x04001755 RID: 5973
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Image _cursorNotTilted;

		// Token: 0x04001756 RID: 5974
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		private Image _cursorInspect;

		// Token: 0x04001757 RID: 5975
		[Nullable(2)]
		private Image _overriddenCursorTilted;

		// Token: 0x04001758 RID: 5976
		[Nullable(2)]
		private Image _overriddenCursorNotTilted;

		// Token: 0x04001759 RID: 5977
		private Vector2? _overriddenHotSpot;

		// Token: 0x0400175A RID: 5978
		[Nullable(2)]
		private Image _lastSetCursor;

		// Token: 0x0400175B RID: 5979
		private bool _isDown;

		// Token: 0x0400175C RID: 5980
		private bool _isUsingController;

		// Token: 0x0400175D RID: 5981
		private bool _shouldShowCursor = true;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001836 RID: 6198
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005EBA RID: 24250
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005EBB RID: 24251
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Input' method.
			/// </summary>
			// Token: 0x04005EBC RID: 24252
			public static readonly StringName _Input = "_Input";

			/// <summary>
			/// Cached name for the 'StopOverridingCursor' method.
			/// </summary>
			// Token: 0x04005EBD RID: 24253
			public static readonly StringName StopOverridingCursor = "StopOverridingCursor";

			/// <summary>
			/// Cached name for the 'OverrideCursor' method.
			/// </summary>
			// Token: 0x04005EBE RID: 24254
			public static readonly StringName OverrideCursor = "OverrideCursor";

			/// <summary>
			/// Cached name for the 'UpdateCursor' method.
			/// </summary>
			// Token: 0x04005EBF RID: 24255
			public static readonly StringName UpdateCursor = "UpdateCursor";

			/// <summary>
			/// Cached name for the 'SetIsUsingController' method.
			/// </summary>
			// Token: 0x04005EC0 RID: 24256
			public static readonly StringName SetIsUsingController = "SetIsUsingController";

			/// <summary>
			/// Cached name for the 'SetCursorShown' method.
			/// </summary>
			// Token: 0x04005EC1 RID: 24257
			public static readonly StringName SetCursorShown = "SetCursorShown";

			/// <summary>
			/// Cached name for the 'RefreshCursorShown' method.
			/// </summary>
			// Token: 0x04005EC2 RID: 24258
			public static readonly StringName RefreshCursorShown = "RefreshCursorShown";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001837 RID: 6199
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CursorTilted' property.
			/// </summary>
			// Token: 0x04005EC3 RID: 24259
			public static readonly StringName CursorTilted = "CursorTilted";

			/// <summary>
			/// Cached name for the 'CursorNotTilted' property.
			/// </summary>
			// Token: 0x04005EC4 RID: 24260
			public static readonly StringName CursorNotTilted = "CursorNotTilted";

			/// <summary>
			/// Cached name for the 'HotSpot' property.
			/// </summary>
			// Token: 0x04005EC5 RID: 24261
			public static readonly StringName HotSpot = "HotSpot";

			/// <summary>
			/// Cached name for the '_cursorTilted' field.
			/// </summary>
			// Token: 0x04005EC6 RID: 24262
			public static readonly StringName _cursorTilted = "_cursorTilted";

			/// <summary>
			/// Cached name for the '_cursorNotTilted' field.
			/// </summary>
			// Token: 0x04005EC7 RID: 24263
			public static readonly StringName _cursorNotTilted = "_cursorNotTilted";

			/// <summary>
			/// Cached name for the '_cursorInspect' field.
			/// </summary>
			// Token: 0x04005EC8 RID: 24264
			public static readonly StringName _cursorInspect = "_cursorInspect";

			/// <summary>
			/// Cached name for the '_overriddenCursorTilted' field.
			/// </summary>
			// Token: 0x04005EC9 RID: 24265
			public static readonly StringName _overriddenCursorTilted = "_overriddenCursorTilted";

			/// <summary>
			/// Cached name for the '_overriddenCursorNotTilted' field.
			/// </summary>
			// Token: 0x04005ECA RID: 24266
			public static readonly StringName _overriddenCursorNotTilted = "_overriddenCursorNotTilted";

			/// <summary>
			/// Cached name for the '_lastSetCursor' field.
			/// </summary>
			// Token: 0x04005ECB RID: 24267
			public static readonly StringName _lastSetCursor = "_lastSetCursor";

			/// <summary>
			/// Cached name for the '_isDown' field.
			/// </summary>
			// Token: 0x04005ECC RID: 24268
			public static readonly StringName _isDown = "_isDown";

			/// <summary>
			/// Cached name for the '_isUsingController' field.
			/// </summary>
			// Token: 0x04005ECD RID: 24269
			public static readonly StringName _isUsingController = "_isUsingController";

			/// <summary>
			/// Cached name for the '_shouldShowCursor' field.
			/// </summary>
			// Token: 0x04005ECE RID: 24270
			public static readonly StringName _shouldShowCursor = "_shouldShowCursor";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001838 RID: 6200
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}

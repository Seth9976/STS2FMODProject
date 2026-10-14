using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Special Vfx to help players find offscreen revealable Epochs.
	/// </summary>
	// Token: 0x020001A4 RID: 420
	[ScriptPath("res://src/Core/Nodes/Vfx/NEpochOffscreenVfx.cs")]
	public class NEpochOffscreenVfx : Control
	{
		// Token: 0x060014C4 RID: 5316 RVA: 0x0007EC08 File Offset: 0x0007CE08
		[NullableContext(1)]
		public static NEpochOffscreenVfx Create(NEpochSlot slot)
		{
			NEpochOffscreenVfx nepochOffscreenVfx = PreloadManager.Cache.GetScene(NEpochOffscreenVfx.scenePath).Instantiate<NEpochOffscreenVfx>(0L);
			nepochOffscreenVfx._slot = slot;
			return nepochOffscreenVfx;
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0007EC34 File Offset: 0x0007CE34
		public override void _Ready()
		{
			this._viewportSizeX = base.GetViewportRect().Size.X;
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0007EC5C File Offset: 0x0007CE5C
		public override void _Process(double delta)
		{
			if (!this._showVfx)
			{
				if (this._slot.GlobalPosition.X < 0f)
				{
					this._showVfx = true;
					Tween tween = this._tween;
					if (tween != null)
					{
						tween.Kill();
					}
					this._tween = base.CreateTween();
					this._tween.TweenProperty(this, "modulate:a", 0.75f, 0.5);
					base.GlobalPosition = new Vector2(0f, this._slot.GlobalPosition.Y + 60f);
					return;
				}
				if (this._slot.GlobalPosition.X > this._viewportSizeX)
				{
					this._showVfx = true;
					Tween tween2 = this._tween;
					if (tween2 != null)
					{
						tween2.Kill();
					}
					this._tween = base.CreateTween();
					this._tween.TweenProperty(this, "modulate:a", 0.75f, 0.5);
					base.GlobalPosition = new Vector2(this._viewportSizeX, this._slot.GlobalPosition.Y + 60f);
					return;
				}
			}
			else if (this._slot.GlobalPosition.X > 0f && this._slot.GlobalPosition.X < this._viewportSizeX)
			{
				this._showVfx = false;
				Tween tween3 = this._tween;
				if (tween3 != null)
				{
					tween3.Kill();
				}
				this._tween = base.CreateTween();
				this._tween.TweenProperty(this, "modulate:a", 0f, 0.2);
			}
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0007EE0E File Offset: 0x0007D00E
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014C8 RID: 5320 RVA: 0x0007EE20 File Offset: 0x0007D020
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NEpochOffscreenVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "slot", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NEpochOffscreenVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEpochOffscreenVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEpochOffscreenVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014C9 RID: 5321 RVA: 0x0007EF54 File Offset: 0x0007D154
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEpochOffscreenVfx.MethodName.Create && args.Count == 1)
			{
				NEpochOffscreenVfx nepochOffscreenVfx = NEpochOffscreenVfx.Create(VariantUtils.ConvertTo<NEpochSlot>(args[0]));
				ret = VariantUtils.CreateFrom<NEpochOffscreenVfx>(ref nepochOffscreenVfx);
				return true;
			}
			if ((ref method) == NEpochOffscreenVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochOffscreenVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEpochOffscreenVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0007F020 File Offset: 0x0007D220
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEpochOffscreenVfx.MethodName.Create && args.Count == 1)
			{
				NEpochOffscreenVfx nepochOffscreenVfx = NEpochOffscreenVfx.Create(VariantUtils.ConvertTo<NEpochSlot>(args[0]));
				ret = VariantUtils.CreateFrom<NEpochOffscreenVfx>(ref nepochOffscreenVfx);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060014CB RID: 5323 RVA: 0x0007F070 File Offset: 0x0007D270
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEpochOffscreenVfx.MethodName.Create || (ref method) == NEpochOffscreenVfx.MethodName._Ready || (ref method) == NEpochOffscreenVfx.MethodName._Process || (ref method) == NEpochOffscreenVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060014CC RID: 5324 RVA: 0x0007F0C0 File Offset: 0x0007D2C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEpochOffscreenVfx.PropertyName._slot)
			{
				this._slot = VariantUtils.ConvertTo<NEpochSlot>(ref value);
				return true;
			}
			if ((ref name) == NEpochOffscreenVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEpochOffscreenVfx.PropertyName._showVfx)
			{
				this._showVfx = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEpochOffscreenVfx.PropertyName._viewportSizeX)
			{
				this._viewportSizeX = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060014CD RID: 5325 RVA: 0x0007F144 File Offset: 0x0007D344
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEpochOffscreenVfx.PropertyName._slot)
			{
				value = VariantUtils.CreateFrom<NEpochSlot>(ref this._slot);
				return true;
			}
			if ((ref name) == NEpochOffscreenVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NEpochOffscreenVfx.PropertyName._showVfx)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._showVfx);
				return true;
			}
			if ((ref name) == NEpochOffscreenVfx.PropertyName._viewportSizeX)
			{
				value = VariantUtils.CreateFrom<float>(ref this._viewportSizeX);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060014CE RID: 5326 RVA: 0x0007F1DC File Offset: 0x0007D3DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEpochOffscreenVfx.PropertyName._slot, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEpochOffscreenVfx.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEpochOffscreenVfx.PropertyName._showVfx, 0L, "", 4096L, false),
				new PropertyInfo(3L, NEpochOffscreenVfx.PropertyName._viewportSizeX, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060014CF RID: 5327 RVA: 0x0007F274 File Offset: 0x0007D474
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEpochOffscreenVfx.PropertyName._slot, Variant.From<NEpochSlot>(ref this._slot));
			info.AddProperty(NEpochOffscreenVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NEpochOffscreenVfx.PropertyName._showVfx, Variant.From<bool>(ref this._showVfx));
			info.AddProperty(NEpochOffscreenVfx.PropertyName._viewportSizeX, Variant.From<float>(ref this._viewportSizeX));
		}

		/// <inheritdoc />
		// Token: 0x060014D0 RID: 5328 RVA: 0x0007F2E0 File Offset: 0x0007D4E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEpochOffscreenVfx.PropertyName._slot, ref variant))
			{
				this._slot = variant.As<NEpochSlot>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEpochOffscreenVfx.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEpochOffscreenVfx.PropertyName._showVfx, ref variant3))
			{
				this._showVfx = variant3.As<bool>();
			}
			Variant variant4;
			if (info.TryGetProperty(NEpochOffscreenVfx.PropertyName._viewportSizeX, ref variant4))
			{
				this._viewportSizeX = variant4.As<float>();
			}
		}

		// Token: 0x04000775 RID: 1909
		[Nullable(1)]
		public static readonly string scenePath = SceneHelper.GetScenePath("timeline_screen/epoch_offscreen_vfx");

		// Token: 0x04000776 RID: 1910
		[Nullable(1)]
		private NEpochSlot _slot;

		// Token: 0x04000777 RID: 1911
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000778 RID: 1912
		private bool _showVfx;

		// Token: 0x04000779 RID: 1913
		private float _viewportSizeX;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001018 RID: 4120
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04003FE2 RID: 16354
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003FE3 RID: 16355
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04003FE4 RID: 16356
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003FE5 RID: 16357
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001019 RID: 4121
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_slot' field.
			/// </summary>
			// Token: 0x04003FE6 RID: 16358
			public static readonly StringName _slot = "_slot";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003FE7 RID: 16359
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_showVfx' field.
			/// </summary>
			// Token: 0x04003FE8 RID: 16360
			public static readonly StringName _showVfx = "_showVfx";

			/// <summary>
			/// Cached name for the '_viewportSizeX' field.
			/// </summary>
			// Token: 0x04003FE9 RID: 16361
			public static readonly StringName _viewportSizeX = "_viewportSizeX";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200101A RID: 4122
		public class SignalName : Control.SignalName
		{
		}
	}
}

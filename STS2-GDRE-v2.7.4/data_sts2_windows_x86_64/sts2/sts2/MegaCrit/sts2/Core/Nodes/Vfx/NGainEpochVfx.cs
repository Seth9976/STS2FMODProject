using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Creates a "toast" style popup. Used when you obtain an Epoch during gameplay.
	/// </summary>
	// Token: 0x020001B1 RID: 433
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NGainEpochVfx.cs")]
	public class NGainEpochVfx : Node
	{
		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x000830E0 File Offset: 0x000812E0
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NGainEpochVfx.ScenePath);
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x000830EC File Offset: 0x000812EC
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("vfx/vfx_gain_epoch");
			}
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x000830F8 File Offset: 0x000812F8
		public override void _Ready()
		{
			this._epoch = base.GetNode<Control>("%EpochContainer");
			this._portrait = base.GetNode<TextureRect>("%Portrait");
			this._portrait.Texture = this._model.Portrait;
			this._label = base.GetNode<MegaLabel>("%Label");
			this._label.SetTextAutoSize(new LocString("vfx", "EPOCH_GAIN").GetRawText());
			TaskHelper.RunSafely(this.AnimateVfx());
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x00083188 File Offset: 0x00081388
		private async Task AnimateVfx()
		{
			this._cts = new CancellationTokenSource();
			if (NGainEpochVfx._vfxCount > 1)
			{
				int num = 3000 * (NGainEpochVfx._vfxCount - 1);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Delaying Gain Epoch Vfx by: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral("ms");
				Log.Info(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
				await Task.Delay(num, this._cts.Token);
			}
			this._epoch.RotationDegrees = -30f;
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(this._epoch, "position:x", 164f, 0.5).SetEase(1L).SetTrans(10L);
			this._tween.TweenProperty(this._epoch, "rotation", 0f, 0.5).SetEase(1L).SetTrans(10L);
			this._tween.Chain();
			this._tween.TweenInterval(1.5);
			this._tween.Chain();
			this._tween.TweenProperty(this, "modulate:a", 0f, 1.0);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x000831CC File Offset: 0x000813CC
		public static NGainEpochVfx Create(EpochModel model)
		{
			NGainEpochVfx._vfxCount++;
			NGainEpochVfx ngainEpochVfx = PreloadManager.Cache.GetScene(NGainEpochVfx.ScenePath).Instantiate<NGainEpochVfx>(0L);
			ngainEpochVfx._model = model;
			return ngainEpochVfx;
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x00083204 File Offset: 0x00081404
		public override void _ExitTree()
		{
			CancellationTokenSource cts = this._cts;
			if (cts != null)
			{
				cts.Cancel();
			}
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			NGainEpochVfx._vfxCount--;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001578 RID: 5496 RVA: 0x00083234 File Offset: 0x00081434
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NGainEpochVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGainEpochVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001579 RID: 5497 RVA: 0x000832A8 File Offset: 0x000814A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGainEpochVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGainEpochVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600157A RID: 5498 RVA: 0x00083308 File Offset: 0x00081508
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGainEpochVfx.MethodName._Ready || (ref method) == NGainEpochVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600157B RID: 5499 RVA: 0x00083330 File Offset: 0x00081530
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGainEpochVfx.PropertyName._epoch)
			{
				this._epoch = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NGainEpochVfx.PropertyName._portrait)
			{
				this._portrait = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NGainEpochVfx.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NGainEpochVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600157C RID: 5500 RVA: 0x000833B4 File Offset: 0x000815B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGainEpochVfx.PropertyName._epoch)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._epoch);
				return true;
			}
			if ((ref name) == NGainEpochVfx.PropertyName._portrait)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._portrait);
				return true;
			}
			if ((ref name) == NGainEpochVfx.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NGainEpochVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600157D RID: 5501 RVA: 0x0008344C File Offset: 0x0008164C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGainEpochVfx.PropertyName._epoch, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGainEpochVfx.PropertyName._portrait, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGainEpochVfx.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NGainEpochVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600157E RID: 5502 RVA: 0x000834E4 File Offset: 0x000816E4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NGainEpochVfx.PropertyName._epoch, Variant.From<Control>(ref this._epoch));
			info.AddProperty(NGainEpochVfx.PropertyName._portrait, Variant.From<TextureRect>(ref this._portrait));
			info.AddProperty(NGainEpochVfx.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
			info.AddProperty(NGainEpochVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x0600157F RID: 5503 RVA: 0x00083550 File Offset: 0x00081750
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGainEpochVfx.PropertyName._epoch, ref variant))
			{
				this._epoch = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGainEpochVfx.PropertyName._portrait, ref variant2))
			{
				this._portrait = variant2.As<TextureRect>();
			}
			Variant variant3;
			if (info.TryGetProperty(NGainEpochVfx.PropertyName._label, ref variant3))
			{
				this._label = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NGainEpochVfx.PropertyName._tween, ref variant4))
			{
				this._tween = variant4.As<Tween>();
			}
		}

		/// <summary>
		/// Used to detect if this Vfx is already running. If so, delay it so we can see all of them one by one.
		/// </summary>
		// Token: 0x0400079E RID: 1950
		private static int _vfxCount;

		// Token: 0x0400079F RID: 1951
		private Control _epoch;

		// Token: 0x040007A0 RID: 1952
		private EpochModel _model;

		// Token: 0x040007A1 RID: 1953
		private TextureRect _portrait;

		// Token: 0x040007A2 RID: 1954
		private MegaLabel _label;

		// Token: 0x040007A3 RID: 1955
		[Nullable(2)]
		private CancellationTokenSource _cts;

		// Token: 0x040007A4 RID: 1956
		[Nullable(2)]
		private Tween _tween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001043 RID: 4163
		[NullableContext(0)]
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004036 RID: 16438
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004037 RID: 16439
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001044 RID: 4164
		[NullableContext(0)]
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the '_epoch' field.
			/// </summary>
			// Token: 0x04004038 RID: 16440
			public static readonly StringName _epoch = "_epoch";

			/// <summary>
			/// Cached name for the '_portrait' field.
			/// </summary>
			// Token: 0x04004039 RID: 16441
			public static readonly StringName _portrait = "_portrait";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x0400403A RID: 16442
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400403B RID: 16443
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001045 RID: 4165
		[NullableContext(0)]
		public class SignalName : Node.SignalName
		{
		}
	}
}

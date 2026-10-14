using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes
{
	// Token: 0x02000174 RID: 372
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/NActBanner.cs")]
	public class NActBanner : Control
	{
		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x000651BC File Offset: 0x000633BC
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NActBanner._path);
			}
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x000651C8 File Offset: 0x000633C8
		[return: Nullable(2)]
		public static NActBanner Create(ActModel act, int actIndex)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NActBanner nactBanner = PreloadManager.Cache.GetScene(NActBanner._path).Instantiate<NActBanner>(0L);
			nactBanner._act = act;
			nactBanner._actIndex = actIndex;
			return nactBanner;
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00065204 File Offset: 0x00063404
		public override void _Ready()
		{
			this._actNumber = base.GetNode<MegaLabel>("ActNumber");
			this._actName = base.GetNode<MegaLabel>("ActName");
			this._banner = base.GetNode<ColorRect>("%Banner");
			LocString locString = new LocString("gameplay_ui", "ACT_NUMBER");
			locString.Add("actNumber", this._actIndex + 1);
			this._actNumber.SetTextAutoSize(locString.GetFormattedText());
			this._actName.SetTextAutoSize(this._act.Title.GetFormattedText());
			TaskHelper.RunSafely(this.AnimateVfx());
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x000652B4 File Offset: 0x000634B4
		private async Task AnimateVfx()
		{
			this._banner.Modulate = StsColors.transparentBlack;
			this._actName.Modulate = StsColors.transparentWhite;
			this._actNumber.Modulate = StsColors.transparentWhite;
			Tween tween = base.CreateTween().SetParallel(true);
			tween.TweenProperty(this._banner, "modulate:a", 0.25f, 0.5).SetDelay(0.5);
			tween.TweenProperty(this._actName, "modulate:a", 1f, 1.0).SetDelay(0.25);
			tween.TweenProperty(this._actNumber, "modulate:a", 1f, 1.0).SetDelay(0.5);
			tween.TweenProperty(this._actNumber, "position:y", 440f, 1.25).SetDelay(0.5).SetEase(1L)
				.SetTrans(4L)
				.From(450f);
			tween.Chain();
			tween.TweenInterval((SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast) ? 0.5 : 2.0);
			tween.Chain();
			tween.TweenProperty(this, "modulate:a", 0f, 1.0).SetEase(1L).SetTrans(4L);
			bool flag = await tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600115B RID: 4443 RVA: 0x000652F8 File Offset: 0x000634F8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NActBanner.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600115C RID: 4444 RVA: 0x0006533C File Offset: 0x0006353C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NActBanner.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600115D RID: 4445 RVA: 0x0006536C File Offset: 0x0006356C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NActBanner.MethodName._Ready || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600115E RID: 4446 RVA: 0x00065384 File Offset: 0x00063584
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NActBanner.PropertyName._actNumber)
			{
				this._actNumber = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NActBanner.PropertyName._actName)
			{
				this._actName = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NActBanner.PropertyName._banner)
			{
				this._banner = VariantUtils.ConvertTo<ColorRect>(ref value);
				return true;
			}
			if ((ref name) == NActBanner.PropertyName._actIndex)
			{
				this._actIndex = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600115F RID: 4447 RVA: 0x00065408 File Offset: 0x00063608
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NActBanner.PropertyName._actNumber)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._actNumber);
				return true;
			}
			if ((ref name) == NActBanner.PropertyName._actName)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._actName);
				return true;
			}
			if ((ref name) == NActBanner.PropertyName._banner)
			{
				value = VariantUtils.CreateFrom<ColorRect>(ref this._banner);
				return true;
			}
			if ((ref name) == NActBanner.PropertyName._actIndex)
			{
				value = VariantUtils.CreateFrom<int>(ref this._actIndex);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001160 RID: 4448 RVA: 0x000654A0 File Offset: 0x000636A0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NActBanner.PropertyName._actNumber, 0L, "", 4096L, false),
				new PropertyInfo(24L, NActBanner.PropertyName._actName, 0L, "", 4096L, false),
				new PropertyInfo(24L, NActBanner.PropertyName._banner, 0L, "", 4096L, false),
				new PropertyInfo(2L, NActBanner.PropertyName._actIndex, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001161 RID: 4449 RVA: 0x00065538 File Offset: 0x00063738
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NActBanner.PropertyName._actNumber, Variant.From<MegaLabel>(ref this._actNumber));
			info.AddProperty(NActBanner.PropertyName._actName, Variant.From<MegaLabel>(ref this._actName));
			info.AddProperty(NActBanner.PropertyName._banner, Variant.From<ColorRect>(ref this._banner));
			info.AddProperty(NActBanner.PropertyName._actIndex, Variant.From<int>(ref this._actIndex));
		}

		/// <inheritdoc />
		// Token: 0x06001162 RID: 4450 RVA: 0x000655A4 File Offset: 0x000637A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NActBanner.PropertyName._actNumber, ref variant))
			{
				this._actNumber = variant.As<MegaLabel>();
			}
			Variant variant2;
			if (info.TryGetProperty(NActBanner.PropertyName._actName, ref variant2))
			{
				this._actName = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NActBanner.PropertyName._banner, ref variant3))
			{
				this._banner = variant3.As<ColorRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NActBanner.PropertyName._actIndex, ref variant4))
			{
				this._actIndex = variant4.As<int>();
			}
		}

		// Token: 0x0400062D RID: 1581
		private MegaLabel _actNumber;

		// Token: 0x0400062E RID: 1582
		private MegaLabel _actName;

		// Token: 0x0400062F RID: 1583
		private ColorRect _banner;

		// Token: 0x04000630 RID: 1584
		private static readonly string _path = SceneHelper.GetScenePath("ui/act_banner");

		// Token: 0x04000631 RID: 1585
		private ActModel _act;

		// Token: 0x04000632 RID: 1586
		private int _actIndex;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F50 RID: 3920
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003CEE RID: 15598
			public static readonly StringName _Ready = "_Ready";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F51 RID: 3921
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_actNumber' field.
			/// </summary>
			// Token: 0x04003CEF RID: 15599
			public static readonly StringName _actNumber = "_actNumber";

			/// <summary>
			/// Cached name for the '_actName' field.
			/// </summary>
			// Token: 0x04003CF0 RID: 15600
			public static readonly StringName _actName = "_actName";

			/// <summary>
			/// Cached name for the '_banner' field.
			/// </summary>
			// Token: 0x04003CF1 RID: 15601
			public static readonly StringName _banner = "_banner";

			/// <summary>
			/// Cached name for the '_actIndex' field.
			/// </summary>
			// Token: 0x04003CF2 RID: 15602
			public static readonly StringName _actIndex = "_actIndex";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000F52 RID: 3922
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}

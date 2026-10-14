using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Relics
{
	/// <summary>
	/// A basic relic display. In comparison with the top-bar relic holder, this cannot flash and never displays amounts.
	/// It automatically supports hovering tweens and showing hovertips, but clicks must be handled externally.
	/// Used in run history, multiplayer expanded state, the relic collection, and the relic unlock screen.
	/// </summary>
	// Token: 0x02000377 RID: 887
	[ScriptPath("res://src/Core/Nodes/Relics/NRelicBasicHolder.cs")]
	public class NRelicBasicHolder : NButton
	{
		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06003465 RID: 13413 RVA: 0x00186711 File Offset: 0x00184911
		[Nullable(1)]
		public NRelic Relic
		{
			[NullableContext(1)]
			get
			{
				return this._relic;
			}
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x0018671C File Offset: 0x0018491C
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NRelicBasicHolder Create(RelicModel relic)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NRelicBasicHolder nrelicBasicHolder = PreloadManager.Cache.GetScene(NRelicBasicHolder._scenePath).Instantiate<NRelicBasicHolder>(0L);
			Node node = nrelicBasicHolder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("NRelicBasicHolder-");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(relic.Id);
			node.Name = defaultInterpolatedStringHandler.ToStringAndClear();
			nrelicBasicHolder._model = relic;
			return nrelicBasicHolder;
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x00186786 File Offset: 0x00184986
		public override void _Ready()
		{
			this.ConnectSignals();
			this._relic = base.GetNode<NRelic>("%Relic");
			this._relic.Model = this._model;
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x001867B5 File Offset: 0x001849B5
		public override void _ExitTree()
		{
			base._ExitTree();
			Tween hoverTween = this._hoverTween;
			if (hoverTween == null)
			{
				return;
			}
			hoverTween.Kill();
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x001867D0 File Offset: 0x001849D0
		protected override void OnFocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._relic.Icon, "scale", Vector2.One * 1.25f, 0.05);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._relic.Model.HoverTips, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetAlignmentForRelic(this._relic);
			}
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x00186860 File Offset: 0x00184A60
		protected override void OnUnfocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._relic.Icon, "scale", Vector2.One, 1.0).SetEase(1L).SetTrans(5L);
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600346B RID: 13419 RVA: 0x001868D4 File Offset: 0x00184AD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NRelicBasicHolder.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicBasicHolder.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicBasicHolder.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicBasicHolder.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600346C RID: 13420 RVA: 0x001869A8 File Offset: 0x00184BA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelicBasicHolder.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicBasicHolder.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicBasicHolder.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicBasicHolder.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600346D RID: 13421 RVA: 0x00186A54 File Offset: 0x00184C54
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRelicBasicHolder.MethodName._Ready || (ref method) == NRelicBasicHolder.MethodName._ExitTree || (ref method) == NRelicBasicHolder.MethodName.OnFocus || (ref method) == NRelicBasicHolder.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600346E RID: 13422 RVA: 0x00186AA4 File Offset: 0x00184CA4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRelicBasicHolder.PropertyName._relic)
			{
				this._relic = VariantUtils.ConvertTo<NRelic>(ref value);
				return true;
			}
			if ((ref name) == NRelicBasicHolder.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600346F RID: 13423 RVA: 0x00186AE4 File Offset: 0x00184CE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRelicBasicHolder.PropertyName.Relic)
			{
				NRelic relic = this.Relic;
				value = VariantUtils.CreateFrom<NRelic>(ref relic);
				return true;
			}
			if ((ref name) == NRelicBasicHolder.PropertyName._relic)
			{
				value = VariantUtils.CreateFrom<NRelic>(ref this._relic);
				return true;
			}
			if ((ref name) == NRelicBasicHolder.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003470 RID: 13424 RVA: 0x00186B5C File Offset: 0x00184D5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRelicBasicHolder.PropertyName._relic, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicBasicHolder.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicBasicHolder.PropertyName.Relic, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003471 RID: 13425 RVA: 0x00186BD3 File Offset: 0x00184DD3
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRelicBasicHolder.PropertyName._relic, Variant.From<NRelic>(ref this._relic));
			info.AddProperty(NRelicBasicHolder.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
		}

		/// <inheritdoc />
		// Token: 0x06003472 RID: 13426 RVA: 0x00186C08 File Offset: 0x00184E08
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRelicBasicHolder.PropertyName._relic, ref variant))
			{
				this._relic = variant.As<NRelic>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRelicBasicHolder.PropertyName._hoverTween, ref variant2))
			{
				this._hoverTween = variant2.As<Tween>();
			}
		}

		// Token: 0x0400145A RID: 5210
		[Nullable(1)]
		private static readonly string _scenePath = SceneHelper.GetScenePath("relics/relic_basic_holder");

		// Token: 0x0400145B RID: 5211
		[Nullable(1)]
		private NRelic _relic;

		// Token: 0x0400145C RID: 5212
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x0400145D RID: 5213
		[Nullable(1)]
		private RelicModel _model;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016C8 RID: 5832
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040058CE RID: 22734
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040058CF RID: 22735
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040058D0 RID: 22736
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040058D1 RID: 22737
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016C9 RID: 5833
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Relic' property.
			/// </summary>
			// Token: 0x040058D2 RID: 22738
			public static readonly StringName Relic = "Relic";

			/// <summary>
			/// Cached name for the '_relic' field.
			/// </summary>
			// Token: 0x040058D3 RID: 22739
			public static readonly StringName _relic = "_relic";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x040058D4 RID: 22740
			public static readonly StringName _hoverTween = "_hoverTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016CA RID: 5834
		public new class SignalName : NButton.SignalName
		{
		}
	}
}

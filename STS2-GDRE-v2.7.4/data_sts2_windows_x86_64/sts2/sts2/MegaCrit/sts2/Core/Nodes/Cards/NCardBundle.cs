using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Cards
{
	// Token: 0x02000414 RID: 1044
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Cards/NCardBundle.cs")]
	public class NCardBundle : Control
	{
		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x0600420C RID: 16908 RVA: 0x00201098 File Offset: 0x001FF298
		// (set) Token: 0x0600420D RID: 16909 RVA: 0x002010A0 File Offset: 0x001FF2A0
		public NClickableControl Hitbox { get; private set; }

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x002010A9 File Offset: 0x001FF2A9
		// (set) Token: 0x0600420F RID: 16911 RVA: 0x002010B1 File Offset: 0x001FF2B1
		public IReadOnlyList<CardModel> Bundle { get; private set; }

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06004210 RID: 16912 RVA: 0x002010BA File Offset: 0x001FF2BA
		public IReadOnlyList<NCard> CardNodes
		{
			get
			{
				return this._cardNodes;
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06004211 RID: 16913 RVA: 0x002010C2 File Offset: 0x001FF2C2
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("/cards/card_bundle");
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x002010CE File Offset: 0x001FF2CE
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCardBundle.ScenePath);
			}
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x002010DC File Offset: 0x001FF2DC
		[return: Nullable(2)]
		public static NCardBundle Create(IReadOnlyList<CardModel> bundle)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NCardBundle ncardBundle = PreloadManager.Cache.GetScene(NCardBundle.ScenePath).Instantiate<NCardBundle>(0L);
			ncardBundle.Name = "NCardBundle";
			ncardBundle.Scale = ncardBundle.smallScale;
			ncardBundle.Bundle = bundle;
			return ncardBundle;
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x00201130 File Offset: 0x001FF330
		public override void _Ready()
		{
			this.Hitbox = base.GetNode<NButton>("%Hitbox");
			this._cardHolder = base.GetNode<Control>("%Cards");
			this.Hitbox.Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.OnFocused)), 0U);
			this.Hitbox.Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.OnUnfocused)), 0U);
			this.Hitbox.Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>(new Action<NClickableControl>(this.OnClicked)), 0U);
			for (int i = 0; i < this.Bundle.Count; i++)
			{
				NCard ncard = NCard.Create(this.Bundle[i], ModelVisibility.Visible);
				this._cardHolder = base.GetNode<Control>("%Cards");
				this._cardHolder.AddChildSafely(ncard);
				ncard.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
				ncard.Position += new Vector2(-1f, 1f) * 45f * ((float)i - (float)this.Bundle.Count / 2f);
				float num = 0.5f + (float)i / (float)(this.Bundle.Count - 1) * 0.5f;
				ncard.Modulate = new Color(num, num, num, 1f);
				this._cardNodes.Add(ncard);
			}
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x002012AC File Offset: 0x001FF4AC
		public IReadOnlyList<NCard> RemoveCardNodes()
		{
			Tween cardTween = this._cardTween;
			if (cardTween != null)
			{
				cardTween.Kill();
			}
			this._cardTween = base.CreateTween().SetParallel(true);
			foreach (NCard ncard in this._cardNodes)
			{
				this._cardTween.TweenProperty(ncard, "modulate", Colors.White, 0.15000000596046448);
			}
			return this.CardNodes;
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x0020134C File Offset: 0x001FF54C
		public void ReAddCardNodes()
		{
			Tween cardTween = this._cardTween;
			if (cardTween != null)
			{
				cardTween.Kill();
			}
			this._cardTween = base.CreateTween().SetParallel(true);
			for (int i = 0; i < this._cardNodes.Count; i++)
			{
				NCard ncard = this._cardNodes[i];
				Vector2 globalPosition = ncard.GlobalPosition;
				Node parent = ncard.GetParent();
				if (parent != null)
				{
					parent.RemoveChildSafely(ncard);
				}
				this._cardHolder = base.GetNode<Control>("%Cards");
				this._cardHolder.AddChildSafely(ncard);
				ncard.GlobalPosition = globalPosition;
				ncard.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
				this._cardTween.TweenProperty(ncard, "position", new Vector2(-1f, 1f) * 45f * ((float)i - (float)this._cardNodes.Count / 2f), 0.4000000059604645).SetEase(1L).SetTrans(5L);
				float num = 0.5f + (float)i / (float)(this._cardNodes.Count - 1) * 0.5f;
				this._cardTween.TweenProperty(ncard, "modulate", new Color(num, num, num, 1f), 0.4000000059604645);
			}
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x002014A5 File Offset: 0x001FF6A5
		private void OnClicked(NClickableControl _)
		{
			base.EmitSignal(NCardBundle.SignalName.Clicked, new Variant[] { this });
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x002014C6 File Offset: 0x001FF6C6
		private void OnFocused(NClickableControl _)
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			base.Scale = this._hoverScale;
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x002014E8 File Offset: 0x001FF6E8
		private void OnUnfocused(NClickableControl _)
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this, "scale", this.smallScale, 0.5).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x0600421A RID: 16922 RVA: 0x0020154C File Offset: 0x001FF74C
		public override void _ExitTree()
		{
			foreach (NCard ncard in this._cardNodes)
			{
				if (base.IsAncestorOf(ncard))
				{
					ncard.QueueFreeSafely();
				}
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600421B RID: 16923 RVA: 0x002015A8 File Offset: 0x001FF7A8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NCardBundle.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardBundle.MethodName.ReAddCardNodes, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardBundle.MethodName.OnClicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardBundle.MethodName.OnFocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardBundle.MethodName.OnUnfocused, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardBundle.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600421C RID: 16924 RVA: 0x00201768 File Offset: 0x001FF968
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardBundle.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardBundle.MethodName.ReAddCardNodes && args.Count == 0)
			{
				this.ReAddCardNodes();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardBundle.MethodName.OnClicked && args.Count == 1)
			{
				this.OnClicked(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardBundle.MethodName.OnFocused && args.Count == 1)
			{
				this.OnFocused(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardBundle.MethodName.OnUnfocused && args.Count == 1)
			{
				this.OnUnfocused(VariantUtils.ConvertTo<NClickableControl>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardBundle.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600421D RID: 16925 RVA: 0x00201888 File Offset: 0x001FFA88
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardBundle.MethodName._Ready || (ref method) == NCardBundle.MethodName.ReAddCardNodes || (ref method) == NCardBundle.MethodName.OnClicked || (ref method) == NCardBundle.MethodName.OnFocused || (ref method) == NCardBundle.MethodName.OnUnfocused || (ref method) == NCardBundle.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600421E RID: 16926 RVA: 0x002018F8 File Offset: 0x001FFAF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardBundle.PropertyName.Hitbox)
			{
				this.Hitbox = VariantUtils.ConvertTo<NClickableControl>(ref value);
				return true;
			}
			if ((ref name) == NCardBundle.PropertyName._cardHolder)
			{
				this._cardHolder = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCardBundle.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardBundle.PropertyName._cardTween)
			{
				this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600421F RID: 16927 RVA: 0x0020197C File Offset: 0x001FFB7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardBundle.PropertyName.Hitbox)
			{
				NClickableControl hitbox = this.Hitbox;
				value = VariantUtils.CreateFrom<NClickableControl>(ref hitbox);
				return true;
			}
			if ((ref name) == NCardBundle.PropertyName._hoverScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._hoverScale);
				return true;
			}
			if ((ref name) == NCardBundle.PropertyName.smallScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this.smallScale);
				return true;
			}
			if ((ref name) == NCardBundle.PropertyName._cardHolder)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cardHolder);
				return true;
			}
			if ((ref name) == NCardBundle.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NCardBundle.PropertyName._cardTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004220 RID: 16928 RVA: 0x00201A54 File Offset: 0x001FFC54
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NCardBundle.PropertyName._hoverScale, 0L, "", 4096L, false),
				new PropertyInfo(5L, NCardBundle.PropertyName.smallScale, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardBundle.PropertyName.Hitbox, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardBundle.PropertyName._cardHolder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardBundle.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardBundle.PropertyName._cardTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004221 RID: 16929 RVA: 0x00201B2C File Offset: 0x001FFD2C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName hitbox = NCardBundle.PropertyName.Hitbox;
			NClickableControl hitbox2 = this.Hitbox;
			info.AddProperty(hitbox, Variant.From<NClickableControl>(ref hitbox2));
			info.AddProperty(NCardBundle.PropertyName._cardHolder, Variant.From<Control>(ref this._cardHolder));
			info.AddProperty(NCardBundle.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NCardBundle.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
			info.AddSignalEventDelegate(NCardBundle.SignalName.Clicked, this.backing_Clicked);
		}

		/// <inheritdoc />
		// Token: 0x06004222 RID: 16930 RVA: 0x00201BAC File Offset: 0x001FFDAC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardBundle.PropertyName.Hitbox, ref variant))
			{
				this.Hitbox = variant.As<NClickableControl>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardBundle.PropertyName._cardHolder, ref variant2))
			{
				this._cardHolder = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardBundle.PropertyName._hoverTween, ref variant3))
			{
				this._hoverTween = variant3.As<Tween>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardBundle.PropertyName._cardTween, ref variant4))
			{
				this._cardTween = variant4.As<Tween>();
			}
			NCardBundle.ClickedEventHandler clickedEventHandler;
			if (info.TryGetSignalEventDelegate<NCardBundle.ClickedEventHandler>(NCardBundle.SignalName.Clicked, ref clickedEventHandler))
			{
				this.backing_Clicked = clickedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004223 RID: 16931 RVA: 0x00201C48 File Offset: 0x001FFE48
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NCardBundle.SignalName.Clicked, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardHolder", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Cards.NCardBundle.ClickedEventHandler" />
		// Token: 0x14000055 RID: 85
		// (add) Token: 0x06004224 RID: 16932 RVA: 0x00201CBC File Offset: 0x001FFEBC
		// (remove) Token: 0x06004225 RID: 16933 RVA: 0x00201CD5 File Offset: 0x001FFED5
		[Nullable(0)]
		public event NCardBundle.ClickedEventHandler Clicked
		{
			[NullableContext(0)]
			add
			{
				this.backing_Clicked = (NCardBundle.ClickedEventHandler)Delegate.Combine(this.backing_Clicked, value);
			}
			[NullableContext(0)]
			remove
			{
				this.backing_Clicked = (NCardBundle.ClickedEventHandler)Delegate.Remove(this.backing_Clicked, value);
			}
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x00201CEE File Offset: 0x001FFEEE
		[NullableContext(0)]
		protected void EmitSignalClicked(NCardBundle cardHolder)
		{
			base.EmitSignal(NCardBundle.SignalName.Clicked, new Variant[] { cardHolder });
		}

		/// <inheritdoc />
		// Token: 0x06004227 RID: 16935 RVA: 0x00201D0F File Offset: 0x001FFF0F
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NCardBundle.SignalName.Clicked) || args.Count != 1)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NCardBundle.ClickedEventHandler clickedEventHandler = this.backing_Clicked;
			if (clickedEventHandler == null)
			{
				return;
			}
			clickedEventHandler(VariantUtils.ConvertTo<NCardBundle>(args[0]));
		}

		/// <inheritdoc />
		// Token: 0x06004228 RID: 16936 RVA: 0x00201D4E File Offset: 0x001FFF4E
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NCardBundle.SignalName.Clicked || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x040019D1 RID: 6609
		private const float _cardSeparation = 45f;

		// Token: 0x040019D2 RID: 6610
		private readonly Vector2 _hoverScale = Vector2.One * 0.85f;

		// Token: 0x040019D3 RID: 6611
		public readonly Vector2 smallScale = Vector2.One * 0.8f;

		// Token: 0x040019D6 RID: 6614
		private Control _cardHolder;

		// Token: 0x040019D7 RID: 6615
		private readonly List<NCard> _cardNodes = new List<NCard>();

		// Token: 0x040019D8 RID: 6616
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x040019D9 RID: 6617
		[Nullable(2)]
		private Tween _cardTween;

		// Token: 0x040019DA RID: 6618
		[Nullable(0)]
		private NCardBundle.ClickedEventHandler backing_Clicked;

		// Token: 0x0200193C RID: 6460
		// (Invoke) Token: 0x0600AA4C RID: 43596
		[NullableContext(0)]
		[Signal]
		public delegate void ClickedEventHandler(NCardBundle cardHolder);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200193D RID: 6461
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400638E RID: 25486
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'ReAddCardNodes' method.
			/// </summary>
			// Token: 0x0400638F RID: 25487
			public static readonly StringName ReAddCardNodes = "ReAddCardNodes";

			/// <summary>
			/// Cached name for the 'OnClicked' method.
			/// </summary>
			// Token: 0x04006390 RID: 25488
			public static readonly StringName OnClicked = "OnClicked";

			/// <summary>
			/// Cached name for the 'OnFocused' method.
			/// </summary>
			// Token: 0x04006391 RID: 25489
			public static readonly StringName OnFocused = "OnFocused";

			/// <summary>
			/// Cached name for the 'OnUnfocused' method.
			/// </summary>
			// Token: 0x04006392 RID: 25490
			public static readonly StringName OnUnfocused = "OnUnfocused";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04006393 RID: 25491
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200193E RID: 6462
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hitbox' property.
			/// </summary>
			// Token: 0x04006394 RID: 25492
			public static readonly StringName Hitbox = "Hitbox";

			/// <summary>
			/// Cached name for the '_hoverScale' field.
			/// </summary>
			// Token: 0x04006395 RID: 25493
			public static readonly StringName _hoverScale = "_hoverScale";

			/// <summary>
			/// Cached name for the 'smallScale' field.
			/// </summary>
			// Token: 0x04006396 RID: 25494
			public static readonly StringName smallScale = "smallScale";

			/// <summary>
			/// Cached name for the '_cardHolder' field.
			/// </summary>
			// Token: 0x04006397 RID: 25495
			public static readonly StringName _cardHolder = "_cardHolder";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04006398 RID: 25496
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_cardTween' field.
			/// </summary>
			// Token: 0x04006399 RID: 25497
			public static readonly StringName _cardTween = "_cardTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200193F RID: 6463
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
			/// <summary>
			/// Cached name for the 'Clicked' signal.
			/// </summary>
			// Token: 0x0400639A RID: 25498
			public static readonly StringName Clicked = "Clicked";
		}
	}
}

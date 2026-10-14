using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;

namespace MegaCrit.Sts2.Core.Nodes.TopBar
{
	// Token: 0x02000244 RID: 580
	[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarDeckButton.cs")]
	public class NTopBarDeckButton : NTopBarButton
	{
		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001D99 RID: 7577 RVA: 0x000C09B1 File Offset: 0x000BEBB1
		[Nullable(1)]
		protected override string[] Hotkeys
		{
			[NullableContext(1)]
			get
			{
				return new string[] { MegaInput.viewDeckAndTabLeft };
			}
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x000C09C6 File Offset: 0x000BEBC6
		public override void _Ready()
		{
			base.InitTopBarButton();
			this._countLabel = base.GetNode<MegaLabel>("DeckCardCount");
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x000C09E4 File Offset: 0x000BEBE4
		public override void _ExitTree()
		{
			base._ExitTree();
			this._pile.CardAddFinished -= this.OnPileContentsChanged;
			this._pile.CardRemoveFinished -= this.OnPileContentsChanged;
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x000C0A1C File Offset: 0x000BEC1C
		[NullableContext(1)]
		public void Initialize(Player player)
		{
			this._player = player;
			this._pile = PileType.Deck.GetPile(player);
			this._pile.CardAddFinished += this.OnPileContentsChanged;
			this._pile.CardRemoveFinished += this.OnPileContentsChanged;
			this.OnPileContentsChanged();
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x000C0A74 File Offset: 0x000BEC74
		private void OnPileContentsChanged()
		{
			int count = this._pile.Cards.Count;
			if ((float)count > this._count)
			{
				Tween bumpTween = this._bumpTween;
				if (bumpTween != null)
				{
					bumpTween.Kill();
				}
				this._bumpTween = base.CreateTween();
				this._bumpTween.TweenProperty(this._countLabel, "scale", Vector2.One, 0.5).From(Vector2.One * 1.5f).SetEase(1L)
					.SetTrans(5L);
				this._countLabel.PivotOffset = this._countLabel.Size * 0.5f;
				this._count = (float)count;
			}
			this._countLabel.SetTextAutoSize(count.ToString());
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x000C0B4C File Offset: 0x000BED4C
		protected override void OnRelease()
		{
			base.OnRelease();
			if (this.IsOpen())
			{
				NCapstoneContainer.Instance.Close();
			}
			else
			{
				NDeckViewScreen.ShowScreen(this._player);
			}
			base.UpdateScreenOpen();
			ShaderMaterial hsv = this._hsv;
			if (hsv == null)
			{
				return;
			}
			hsv.SetShaderParameter(NTopBarDeckButton._v, 0.9f);
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x000C0BA4 File Offset: 0x000BEDA4
		protected override bool IsOpen()
		{
			return NCapstoneContainer.Instance.CurrentCapstoneScreen is NDeckViewScreen;
		}

		/// <summary>
		/// Animates rocking back and forth
		/// </summary>
		/// <param name="delta"></param>
		// Token: 0x06001DA0 RID: 7584 RVA: 0x000C0BB8 File Offset: 0x000BEDB8
		public override void _Process(double delta)
		{
			if (base.IsScreenOpen)
			{
				this._elapsedTime += (float)delta * 4f;
				this._icon.Rotation = this._rockBaseRotation + 0.12f * Mathf.Sin(this._elapsedTime);
				this._rockBaseRotation = (float)Mathf.Lerp((double)this._rockBaseRotation, 0.0, delta);
			}
		}

		/// <summary>
		/// Toggles the anim state of this button.
		/// Utilized when an external UI (ie BackButton) closes this screen.
		/// </summary>
		// Token: 0x06001DA1 RID: 7585 RVA: 0x000C0C22 File Offset: 0x000BEE22
		public void ToggleAnimState()
		{
			base.UpdateScreenOpen();
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x000C0C2C File Offset: 0x000BEE2C
		protected override void OnFocus()
		{
			base.OnFocus();
			LocString locString = new LocString("static_hover_tips", "DECK.title");
			locString.Add("Hotkey", NInputManager.Instance.GetShortcutKey(MegaInput.viewDeckAndTabLeft).ToString());
			HoverTip hoverTip = new HoverTip(locString, new LocString("static_hover_tips", "DECK.description"), null);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, hoverTip, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetGlobalPosition(base.GlobalPosition + new Vector2(base.Size.X - nhoverTipSet.Size.X, base.Size.Y + 20f), false);
			}
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x000C0CDF File Offset: 0x000BEEDF
		protected override void OnUnfocus()
		{
			base.OnUnfocus();
			NHoverTipSet.Remove(this);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001DA4 RID: 7588 RVA: 0x000C0CF0 File Offset: 0x000BEEF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NTopBarDeckButton.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarDeckButton.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarDeckButton.MethodName.OnPileContentsChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarDeckButton.MethodName.OnRelease, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarDeckButton.MethodName.IsOpen, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarDeckButton.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NTopBarDeckButton.MethodName.ToggleAnimState, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarDeckButton.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NTopBarDeckButton.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001DA5 RID: 7589 RVA: 0x000C0ED4 File Offset: 0x000BF0D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NTopBarDeckButton.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarDeckButton.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarDeckButton.MethodName.OnPileContentsChanged && args.Count == 0)
			{
				this.OnPileContentsChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarDeckButton.MethodName.OnRelease && args.Count == 0)
			{
				this.OnRelease();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarDeckButton.MethodName.IsOpen && args.Count == 0)
			{
				bool flag = this.IsOpen();
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == NTopBarDeckButton.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarDeckButton.MethodName.ToggleAnimState && args.Count == 0)
			{
				this.ToggleAnimState();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarDeckButton.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NTopBarDeckButton.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06001DA6 RID: 7590 RVA: 0x000C104C File Offset: 0x000BF24C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NTopBarDeckButton.MethodName._Ready || (ref method) == NTopBarDeckButton.MethodName._ExitTree || (ref method) == NTopBarDeckButton.MethodName.OnPileContentsChanged || (ref method) == NTopBarDeckButton.MethodName.OnRelease || (ref method) == NTopBarDeckButton.MethodName.IsOpen || (ref method) == NTopBarDeckButton.MethodName._Process || (ref method) == NTopBarDeckButton.MethodName.ToggleAnimState || (ref method) == NTopBarDeckButton.MethodName.OnFocus || (ref method) == NTopBarDeckButton.MethodName.OnUnfocus || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06001DA7 RID: 7591 RVA: 0x000C10E8 File Offset: 0x000BF2E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NTopBarDeckButton.PropertyName._elapsedTime)
			{
				this._elapsedTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NTopBarDeckButton.PropertyName._rockBaseRotation)
			{
				this._rockBaseRotation = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NTopBarDeckButton.PropertyName._countLabel)
			{
				this._countLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NTopBarDeckButton.PropertyName._count)
			{
				this._count = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NTopBarDeckButton.PropertyName._bumpTween)
			{
				this._bumpTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06001DA8 RID: 7592 RVA: 0x000C1184 File Offset: 0x000BF384
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NTopBarDeckButton.PropertyName.Hotkeys)
			{
				string[] hotkeys = this.Hotkeys;
				value = VariantUtils.CreateFrom<string[]>(ref hotkeys);
				return true;
			}
			if ((ref name) == NTopBarDeckButton.PropertyName._elapsedTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._elapsedTime);
				return true;
			}
			if ((ref name) == NTopBarDeckButton.PropertyName._rockBaseRotation)
			{
				value = VariantUtils.CreateFrom<float>(ref this._rockBaseRotation);
				return true;
			}
			if ((ref name) == NTopBarDeckButton.PropertyName._countLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._countLabel);
				return true;
			}
			if ((ref name) == NTopBarDeckButton.PropertyName._count)
			{
				value = VariantUtils.CreateFrom<float>(ref this._count);
				return true;
			}
			if ((ref name) == NTopBarDeckButton.PropertyName._bumpTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._bumpTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001DA9 RID: 7593 RVA: 0x000C125C File Offset: 0x000BF45C
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(34L, NTopBarDeckButton.PropertyName.Hotkeys, 0L, "", 4096L, false),
				new PropertyInfo(3L, NTopBarDeckButton.PropertyName._elapsedTime, 0L, "", 4096L, false),
				new PropertyInfo(3L, NTopBarDeckButton.PropertyName._rockBaseRotation, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBarDeckButton.PropertyName._countLabel, 0L, "", 4096L, false),
				new PropertyInfo(3L, NTopBarDeckButton.PropertyName._count, 0L, "", 4096L, false),
				new PropertyInfo(24L, NTopBarDeckButton.PropertyName._bumpTween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001DAA RID: 7594 RVA: 0x000C1334 File Offset: 0x000BF534
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NTopBarDeckButton.PropertyName._elapsedTime, Variant.From<float>(ref this._elapsedTime));
			info.AddProperty(NTopBarDeckButton.PropertyName._rockBaseRotation, Variant.From<float>(ref this._rockBaseRotation));
			info.AddProperty(NTopBarDeckButton.PropertyName._countLabel, Variant.From<MegaLabel>(ref this._countLabel));
			info.AddProperty(NTopBarDeckButton.PropertyName._count, Variant.From<float>(ref this._count));
			info.AddProperty(NTopBarDeckButton.PropertyName._bumpTween, Variant.From<Tween>(ref this._bumpTween));
		}

		/// <inheritdoc />
		// Token: 0x06001DAB RID: 7595 RVA: 0x000C13B8 File Offset: 0x000BF5B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NTopBarDeckButton.PropertyName._elapsedTime, ref variant))
			{
				this._elapsedTime = variant.As<float>();
			}
			Variant variant2;
			if (info.TryGetProperty(NTopBarDeckButton.PropertyName._rockBaseRotation, ref variant2))
			{
				this._rockBaseRotation = variant2.As<float>();
			}
			Variant variant3;
			if (info.TryGetProperty(NTopBarDeckButton.PropertyName._countLabel, ref variant3))
			{
				this._countLabel = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NTopBarDeckButton.PropertyName._count, ref variant4))
			{
				this._count = variant4.As<float>();
			}
			Variant variant5;
			if (info.TryGetProperty(NTopBarDeckButton.PropertyName._bumpTween, ref variant5))
			{
				this._bumpTween = variant5.As<Tween>();
			}
		}

		// Token: 0x04000B64 RID: 2916
		[Nullable(1)]
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000B65 RID: 2917
		private float _elapsedTime;

		// Token: 0x04000B66 RID: 2918
		private const float _rockSpeed = 4f;

		// Token: 0x04000B67 RID: 2919
		private const float _rockDist = 0.12f;

		// Token: 0x04000B68 RID: 2920
		private float _rockBaseRotation;

		// Token: 0x04000B69 RID: 2921
		private const float _defaultV = 0.9f;

		// Token: 0x04000B6A RID: 2922
		[Nullable(1)]
		private Player _player;

		// Token: 0x04000B6B RID: 2923
		[Nullable(1)]
		private CardPile _pile;

		// Token: 0x04000B6C RID: 2924
		[Nullable(1)]
		private MegaLabel _countLabel;

		// Token: 0x04000B6D RID: 2925
		private float _count;

		// Token: 0x04000B6E RID: 2926
		[Nullable(2)]
		private Tween _bumpTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001244 RID: 4676
		public new class MethodName : NTopBarButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040046B0 RID: 18096
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040046B1 RID: 18097
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnPileContentsChanged' method.
			/// </summary>
			// Token: 0x040046B2 RID: 18098
			public static readonly StringName OnPileContentsChanged = "OnPileContentsChanged";

			/// <summary>
			/// Cached name for the 'OnRelease' method.
			/// </summary>
			// Token: 0x040046B3 RID: 18099
			public new static readonly StringName OnRelease = "OnRelease";

			/// <summary>
			/// Cached name for the 'IsOpen' method.
			/// </summary>
			// Token: 0x040046B4 RID: 18100
			public new static readonly StringName IsOpen = "IsOpen";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040046B5 RID: 18101
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'ToggleAnimState' method.
			/// </summary>
			// Token: 0x040046B6 RID: 18102
			public static readonly StringName ToggleAnimState = "ToggleAnimState";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040046B7 RID: 18103
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040046B8 RID: 18104
			public new static readonly StringName OnUnfocus = "OnUnfocus";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001245 RID: 4677
		public new class PropertyName : NTopBarButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Hotkeys' property.
			/// </summary>
			// Token: 0x040046B9 RID: 18105
			public new static readonly StringName Hotkeys = "Hotkeys";

			/// <summary>
			/// Cached name for the '_elapsedTime' field.
			/// </summary>
			// Token: 0x040046BA RID: 18106
			public static readonly StringName _elapsedTime = "_elapsedTime";

			/// <summary>
			/// Cached name for the '_rockBaseRotation' field.
			/// </summary>
			// Token: 0x040046BB RID: 18107
			public static readonly StringName _rockBaseRotation = "_rockBaseRotation";

			/// <summary>
			/// Cached name for the '_countLabel' field.
			/// </summary>
			// Token: 0x040046BC RID: 18108
			public static readonly StringName _countLabel = "_countLabel";

			/// <summary>
			/// Cached name for the '_count' field.
			/// </summary>
			// Token: 0x040046BD RID: 18109
			public static readonly StringName _count = "_count";

			/// <summary>
			/// Cached name for the '_bumpTween' field.
			/// </summary>
			// Token: 0x040046BE RID: 18110
			public static readonly StringName _bumpTween = "_bumpTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001246 RID: 4678
		public new class SignalName : NTopBarButton.SignalName
		{
		}
	}
}

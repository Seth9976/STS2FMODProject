using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;

namespace MegaCrit.Sts2.Core.Nodes.Events
{
	// Token: 0x020003B9 RID: 953
	[ScriptPath("res://src/Core/Nodes/Events/NAncientDialogueLine.cs")]
	public class NAncientDialogueLine : NButton
	{
		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x0600393C RID: 14652 RVA: 0x001B1080 File Offset: 0x001AF280
		[Nullable(2)]
		protected override string HoveredSfx
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x0600393D RID: 14653 RVA: 0x001B1083 File Offset: 0x001AF283
		[Nullable(2)]
		protected override string ClickedSfx
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x001B1088 File Offset: 0x001AF288
		[NullableContext(1)]
		public static NAncientDialogueLine Create(AncientDialogueLine line, AncientEventModel ancient, CharacterModel character)
		{
			NAncientDialogueLine nancientDialogueLine = PreloadManager.Cache.GetScene("res://scenes/events/ancient_dialogue_line.tscn").Instantiate<NAncientDialogueLine>(0L);
			nancientDialogueLine._line = line;
			nancientDialogueLine._ancient = ancient;
			nancientDialogueLine._character = character;
			return nancientDialogueLine;
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x001B10C4 File Offset: 0x001AF2C4
		public override void _Ready()
		{
			this.ConnectSignals();
			LocString lineText = this._line.LineText;
			this._character.AddDetailsTo(lineText);
			this._ancient.DynamicVars.AddTo(lineText);
			base.GetNode<MegaRichTextLabel>("%Text").Text = lineText.GetFormattedText();
			AncientDialogueSpeaker speaker = this._line.Speaker;
			if (speaker != AncientDialogueSpeaker.Ancient)
			{
				if (speaker != AncientDialogueSpeaker.Character)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.SetCharacterAsSpeaker();
			}
			else
			{
				this.SetAncientAsSpeaker();
			}
			base.Modulate = StsColors.transparentWhite;
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x001B1152 File Offset: 0x001AF352
		public void PlaySfx()
		{
			SfxCmd.Play(this._line.GetSfxOrFallbackPath(), 1f);
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x001B116C File Offset: 0x001AF36C
		private void SetAncientAsSpeaker()
		{
			Control node = base.GetNode<Control>("%AncientIcon");
			node.GetNode<TextureRect>("Icon").Texture = this._ancient.RunHistoryIcon;
			node.GetNode<TextureRect>("Icon/Outline").Texture = this._ancient.RunHistoryIconOutline;
			this._iconNode = node;
			Control node2 = base.GetNode<Control>("%DialogueTailLeft");
			node2.Visible = true;
			MarginContainer node3 = base.GetNode<MarginContainer>("%TextContainer");
			node3.AddThemeConstantOverride(ThemeConstants.MarginContainer.MarginLeft, 48);
			base.GetNode<Control>("%Bubble").SelfModulate = this._ancient.DialogueColor;
			node2.SelfModulate = this._ancient.DialogueColor;
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x001B1238 File Offset: 0x001AF438
		private void SetCharacterAsSpeaker()
		{
			Control node = base.GetNode<Control>("%CharacterIcon");
			node.GetNode<TextureRect>("Icon").Texture = this._character.IconTexture;
			node.GetNode<TextureRect>("Icon/Outline").Texture = this._character.IconOutlineTexture;
			this._iconNode = node;
			Control node2 = base.GetNode<Control>("%DialogueTailRight");
			node2.Visible = true;
			MarginContainer node3 = base.GetNode<MarginContainer>("%TextContainer");
			node3.AddThemeConstantOverride(ThemeConstants.MarginContainer.MarginRight, 46);
			base.GetNode<Control>("%Bubble").SelfModulate = this._character.DialogueColor;
			node2.SelfModulate = this._character.DialogueColor;
		}

		/// <summary>
		/// Because the speaker's icon peeks through. This hack makes the icons visible when they animate in.
		/// </summary>
		// Token: 0x06003943 RID: 14659 RVA: 0x001B1304 File Offset: 0x001AF504
		public void SetSpeakerIconVisible()
		{
			this._iconNode.Visible = true;
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x001B1312 File Offset: 0x001AF512
		public void SetTransparency(float alpha)
		{
			this._targetAlpha = alpha;
			base.Modulate = new Color(1f, 1f, 1f, alpha);
		}

		/// <summary>
		/// Special method called when players hover older dialogue lines to view them.
		/// </summary>
		// Token: 0x06003945 RID: 14661 RVA: 0x001B1336 File Offset: 0x001AF536
		public void FadeInStaleDialogue()
		{
			this.OnAnimInSetVisible();
		}

		/// <summary>
		/// Opposite of FadeInStaleDialogue, hide these stale dialogues again.
		/// </summary>
		// Token: 0x06003946 RID: 14662 RVA: 0x001B1340 File Offset: 0x001AF540
		public void FadeOutStaleDialogue()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "modulate:a", 0.25f, 0.1);
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x001B1394 File Offset: 0x001AF594
		public void OnAnimInSetVisible()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween();
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.1);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003948 RID: 14664 RVA: 0x001B13E8 File Offset: 0x001AF5E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NAncientDialogueLine.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientDialogueLine.MethodName.PlaySfx, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientDialogueLine.MethodName.SetAncientAsSpeaker, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientDialogueLine.MethodName.SetCharacterAsSpeaker, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientDialogueLine.MethodName.SetSpeakerIconVisible, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientDialogueLine.MethodName.SetTransparency, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "alpha", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NAncientDialogueLine.MethodName.FadeInStaleDialogue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientDialogueLine.MethodName.FadeOutStaleDialogue, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAncientDialogueLine.MethodName.OnAnimInSetVisible, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003949 RID: 14665 RVA: 0x001B15CC File Offset: 0x001AF7CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAncientDialogueLine.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientDialogueLine.MethodName.PlaySfx && args.Count == 0)
			{
				this.PlaySfx();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientDialogueLine.MethodName.SetAncientAsSpeaker && args.Count == 0)
			{
				this.SetAncientAsSpeaker();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientDialogueLine.MethodName.SetCharacterAsSpeaker && args.Count == 0)
			{
				this.SetCharacterAsSpeaker();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientDialogueLine.MethodName.SetSpeakerIconVisible && args.Count == 0)
			{
				this.SetSpeakerIconVisible();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientDialogueLine.MethodName.SetTransparency && args.Count == 1)
			{
				this.SetTransparency(VariantUtils.ConvertTo<float>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientDialogueLine.MethodName.FadeInStaleDialogue && args.Count == 0)
			{
				this.FadeInStaleDialogue();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientDialogueLine.MethodName.FadeOutStaleDialogue && args.Count == 0)
			{
				this.FadeOutStaleDialogue();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAncientDialogueLine.MethodName.OnAnimInSetVisible && args.Count == 0)
			{
				this.OnAnimInSetVisible();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600394A RID: 14666 RVA: 0x001B1740 File Offset: 0x001AF940
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAncientDialogueLine.MethodName._Ready || (ref method) == NAncientDialogueLine.MethodName.PlaySfx || (ref method) == NAncientDialogueLine.MethodName.SetAncientAsSpeaker || (ref method) == NAncientDialogueLine.MethodName.SetCharacterAsSpeaker || (ref method) == NAncientDialogueLine.MethodName.SetSpeakerIconVisible || (ref method) == NAncientDialogueLine.MethodName.SetTransparency || (ref method) == NAncientDialogueLine.MethodName.FadeInStaleDialogue || (ref method) == NAncientDialogueLine.MethodName.FadeOutStaleDialogue || (ref method) == NAncientDialogueLine.MethodName.OnAnimInSetVisible || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600394B RID: 14667 RVA: 0x001B17DC File Offset: 0x001AF9DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAncientDialogueLine.PropertyName._iconNode)
			{
				this._iconNode = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NAncientDialogueLine.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NAncientDialogueLine.PropertyName._targetAlpha)
			{
				this._targetAlpha = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600394C RID: 14668 RVA: 0x001B1844 File Offset: 0x001AFA44
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAncientDialogueLine.PropertyName.HoveredSfx)
			{
				string text = this.HoveredSfx;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NAncientDialogueLine.PropertyName.ClickedSfx)
			{
				string text = this.ClickedSfx;
				value = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref name) == NAncientDialogueLine.PropertyName._iconNode)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._iconNode);
				return true;
			}
			if ((ref name) == NAncientDialogueLine.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NAncientDialogueLine.PropertyName._targetAlpha)
			{
				value = VariantUtils.CreateFrom<float>(ref this._targetAlpha);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600394D RID: 14669 RVA: 0x001B1900 File Offset: 0x001AFB00
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(4L, NAncientDialogueLine.PropertyName.HoveredSfx, 0L, "", 4096L, false),
				new PropertyInfo(4L, NAncientDialogueLine.PropertyName.ClickedSfx, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientDialogueLine.PropertyName._iconNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAncientDialogueLine.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(3L, NAncientDialogueLine.PropertyName._targetAlpha, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600394E RID: 14670 RVA: 0x001B19B8 File Offset: 0x001AFBB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAncientDialogueLine.PropertyName._iconNode, Variant.From<Control>(ref this._iconNode));
			info.AddProperty(NAncientDialogueLine.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NAncientDialogueLine.PropertyName._targetAlpha, Variant.From<float>(ref this._targetAlpha));
		}

		/// <inheritdoc />
		// Token: 0x0600394F RID: 14671 RVA: 0x001B1A10 File Offset: 0x001AFC10
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAncientDialogueLine.PropertyName._iconNode, ref variant))
			{
				this._iconNode = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAncientDialogueLine.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAncientDialogueLine.PropertyName._targetAlpha, ref variant3))
			{
				this._targetAlpha = variant3.As<float>();
			}
		}

		// Token: 0x04001640 RID: 5696
		[Nullable(1)]
		private const string _scenePath = "res://scenes/events/ancient_dialogue_line.tscn";

		// Token: 0x04001641 RID: 5697
		[Nullable(1)]
		private AncientDialogueLine _line;

		// Token: 0x04001642 RID: 5698
		[Nullable(1)]
		private AncientEventModel _ancient;

		// Token: 0x04001643 RID: 5699
		[Nullable(1)]
		private CharacterModel _character;

		// Token: 0x04001644 RID: 5700
		[Nullable(1)]
		private Control _iconNode;

		// Token: 0x04001645 RID: 5701
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04001646 RID: 5702
		private float _targetAlpha = 1f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017C4 RID: 6084
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005C87 RID: 23687
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'PlaySfx' method.
			/// </summary>
			// Token: 0x04005C88 RID: 23688
			public static readonly StringName PlaySfx = "PlaySfx";

			/// <summary>
			/// Cached name for the 'SetAncientAsSpeaker' method.
			/// </summary>
			// Token: 0x04005C89 RID: 23689
			public static readonly StringName SetAncientAsSpeaker = "SetAncientAsSpeaker";

			/// <summary>
			/// Cached name for the 'SetCharacterAsSpeaker' method.
			/// </summary>
			// Token: 0x04005C8A RID: 23690
			public static readonly StringName SetCharacterAsSpeaker = "SetCharacterAsSpeaker";

			/// <summary>
			/// Cached name for the 'SetSpeakerIconVisible' method.
			/// </summary>
			// Token: 0x04005C8B RID: 23691
			public static readonly StringName SetSpeakerIconVisible = "SetSpeakerIconVisible";

			/// <summary>
			/// Cached name for the 'SetTransparency' method.
			/// </summary>
			// Token: 0x04005C8C RID: 23692
			public static readonly StringName SetTransparency = "SetTransparency";

			/// <summary>
			/// Cached name for the 'FadeInStaleDialogue' method.
			/// </summary>
			// Token: 0x04005C8D RID: 23693
			public static readonly StringName FadeInStaleDialogue = "FadeInStaleDialogue";

			/// <summary>
			/// Cached name for the 'FadeOutStaleDialogue' method.
			/// </summary>
			// Token: 0x04005C8E RID: 23694
			public static readonly StringName FadeOutStaleDialogue = "FadeOutStaleDialogue";

			/// <summary>
			/// Cached name for the 'OnAnimInSetVisible' method.
			/// </summary>
			// Token: 0x04005C8F RID: 23695
			public static readonly StringName OnAnimInSetVisible = "OnAnimInSetVisible";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017C5 RID: 6085
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'HoveredSfx' property.
			/// </summary>
			// Token: 0x04005C90 RID: 23696
			public new static readonly StringName HoveredSfx = "HoveredSfx";

			/// <summary>
			/// Cached name for the 'ClickedSfx' property.
			/// </summary>
			// Token: 0x04005C91 RID: 23697
			public new static readonly StringName ClickedSfx = "ClickedSfx";

			/// <summary>
			/// Cached name for the '_iconNode' field.
			/// </summary>
			// Token: 0x04005C92 RID: 23698
			public static readonly StringName _iconNode = "_iconNode";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04005C93 RID: 23699
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_targetAlpha' field.
			/// </summary>
			// Token: 0x04005C94 RID: 23700
			public static readonly StringName _targetAlpha = "_targetAlpha";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017C6 RID: 6086
		public new class SignalName : NButton.SignalName
		{
		}
	}
}

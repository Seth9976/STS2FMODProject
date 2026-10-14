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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Thought bubble vfx used when creatures think during combat. Generally, this is the player character breaking the 4th wall.
	/// Out of energy! No more cards in the draw pile. Etc
	/// </summary>
	// Token: 0x02000208 RID: 520
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NThoughtBubbleVfx.cs")]
	public class NThoughtBubbleVfx : Control
	{
		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001ADE RID: 6878 RVA: 0x000ACEC0 File Offset: 0x000AB0C0
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/vfx/vfx_thought_bubble.tscn");
			}
		}

		/// <summary>
		/// Creates a thought bubble given a creature. Use this when you want creatures to think out loud during combat!
		/// </summary>
		// Token: 0x06001ADF RID: 6879 RVA: 0x000ACECC File Offset: 0x000AB0CC
		[return: Nullable(2)]
		public static NThoughtBubbleVfx Create(string text, Creature speaker, double? secondsToDisplay)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			CombatSide side = speaker.Side;
			DialogueSide dialogueSide;
			if (side != CombatSide.Player)
			{
				if (side != CombatSide.Enemy)
				{
					throw new ArgumentOutOfRangeException();
				}
				dialogueSide = DialogueSide.Right;
			}
			else
			{
				dialogueSide = DialogueSide.Left;
			}
			DialogueSide dialogueSide2 = dialogueSide;
			NThoughtBubbleVfx nthoughtBubbleVfx = NThoughtBubbleVfx.CreateInternal(text, null, dialogueSide2, secondsToDisplay);
			nthoughtBubbleVfx._startPos = new Vector2?(NThoughtBubbleVfx.GetCreatureSpeechPosition(speaker));
			return nthoughtBubbleVfx;
		}

		/// <summary>
		/// When we want to specify the absolute position of the thought bubble vfx.
		/// </summary>
		// Token: 0x06001AE0 RID: 6880 RVA: 0x000ACF20 File Offset: 0x000AB120
		[return: Nullable(2)]
		public static NThoughtBubbleVfx Create(string text, DialogueSide side, double? secondsToDisplay)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return NThoughtBubbleVfx.CreateInternal(text, null, side, secondsToDisplay);
		}

		/// <summary>
		/// When we want to specify the absolute position of the thought bubble vfx with an image.
		/// </summary>
		// Token: 0x06001AE1 RID: 6881 RVA: 0x000ACF44 File Offset: 0x000AB144
		[return: Nullable(2)]
		public static NThoughtBubbleVfx Create(Texture2D texture, DialogueSide side, double? secondsToDisplay)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return NThoughtBubbleVfx.CreateInternal(null, texture, side, secondsToDisplay);
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x000ACF68 File Offset: 0x000AB168
		public override void _Ready()
		{
			this._contents = base.GetNode<Node2D>("%Contents");
			this._container = base.GetNode<Control>("%Container");
			this._label = base.GetNode<MegaRichTextLabel>("%Text");
			this._textureRect = base.GetNode<TextureRect>("%Image");
			this._tail = base.GetNode<Node2D>("%Tail");
			if (this._startPos != null)
			{
				base.GlobalPosition = this._startPos.Value;
			}
			if (this._side == DialogueSide.Right)
			{
				this._container.Position = new Vector2(-this._container.Size.X - this._container.Position.X, this._container.Position.Y);
				this._tail.Scale = new Vector2(-1f, 1f);
			}
			TaskHelper.RunSafely(this.AnimateThoughtBubble());
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x000AD074 File Offset: 0x000AB274
		private async Task AnimateThoughtBubble()
		{
			this._tween = base.CreateTween().SetParallel(true);
			float num = 30f * ((this._side == DialogueSide.Left) ? (-1f) : 1f);
			if (this._text != null)
			{
				MegaRichTextLabel label = this._label;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[center][fly_in offset_x=");
				defaultInterpolatedStringHandler.AppendFormatted<float>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" offset_y=40]");
				defaultInterpolatedStringHandler.AppendFormatted(this._text);
				defaultInterpolatedStringHandler.AppendLiteral("[/fly_in][/center]");
				label.Text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			this._label.Visible = this._text != null;
			if (this._texture != null)
			{
				this._textureRect.Texture = this._texture;
			}
			this._textureRect.Visible = this._texture != null;
			base.Scale = Vector2.One * 0.75f;
			base.Modulate = StsColors.transparentWhite;
			this._tween.TweenProperty(this._label, "visible_ratio", 1f, 0.4).From(0f);
			this._tween.TweenProperty(this._textureRect, "modulate:a", 1f, 0.4).From(0f);
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.25).SetEase(1L).SetTrans(7L);
			this._tween.TweenProperty(this, "scale", Vector2.One, 0.25).SetEase(1L).SetTrans(10L);
			if (this._secondsToDisplay != null)
			{
				double num2 = Math.Max(this._secondsToDisplay.Value, 1.0);
				this._cts = new CancellationTokenSource();
				await Cmd.Wait((float)num2, this._cts.Token, false);
				await this.GoAway();
			}
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x000AD0B7 File Offset: 0x000AB2B7
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			CancellationTokenSource cts = this._cts;
			if (cts == null)
			{
				return;
			}
			cts.Cancel();
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x000AD0DC File Offset: 0x000AB2DC
		public async Task GoAway()
		{
			if (GodotObject.IsInstanceValid(this))
			{
				Tween tween = this._tween;
				if (tween != null)
				{
					tween.Kill();
				}
				this._tween = base.CreateTween();
				this._tween.TweenProperty(this, "modulate:a", 0f, 0.4).SetEase(1L).SetTrans(1L);
				bool flag = await this._tween.AwaitFinished(this);
				this.QueueFreeSafely();
			}
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x000AD120 File Offset: 0x000AB320
		[NullableContext(2)]
		[return: Nullable(1)]
		private static NThoughtBubbleVfx CreateInternal(string text, Texture2D texture, DialogueSide side, double? secondsToDisplay)
		{
			NThoughtBubbleVfx nthoughtBubbleVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/vfx_thought_bubble.tscn").Instantiate<NThoughtBubbleVfx>(0L);
			nthoughtBubbleVfx._side = side;
			nthoughtBubbleVfx._text = text;
			nthoughtBubbleVfx._texture = texture;
			nthoughtBubbleVfx._secondsToDisplay = secondsToDisplay;
			return nthoughtBubbleVfx;
		}

		/// <summary>
		/// Given a creature, dynamically calculates and returns the position of where the "mouth" should be.
		/// Used during combat, where we don't know which side a creature could be on.
		/// </summary>
		/// <param name="speaker"></param>
		/// <returns></returns>
		// Token: 0x06001AE7 RID: 6887 RVA: 0x000AD164 File Offset: 0x000AB364
		public static Vector2 GetCreatureSpeechPosition(Creature speaker)
		{
			NCreature creatureNode = speaker.GetCreatureNode();
			if (creatureNode == null)
			{
				return Vector2.Zero;
			}
			if (creatureNode.Visuals.TalkPosition != null)
			{
				return creatureNode.Visuals.TalkPosition.GlobalPosition;
			}
			Vector2 vector = creatureNode.VfxSpawnPosition + new Vector2(0f, -creatureNode.Hitbox.Size.Y * 0.5f * 0.75f);
			if (speaker.Side == CombatSide.Player)
			{
				vector.X += creatureNode.Hitbox.Size.X * 0.75f;
			}
			else
			{
				vector.X -= creatureNode.Hitbox.Size.X * 0.75f;
			}
			return vector;
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x000AD220 File Offset: 0x000AB420
		public void SetTexture(Texture2D texture)
		{
			if (this._texture == null)
			{
				throw new NotImplementedException("Can't set texture unless thought bubble was initialized with a texture");
			}
			this._texture = texture;
			this._textureRect.Texture = texture;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001AE9 RID: 6889 RVA: 0x000AD248 File Offset: 0x000AB448
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(NThoughtBubbleVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NThoughtBubbleVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NThoughtBubbleVfx.MethodName.SetTexture, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "texture", 0L, "", 6L, new StringName("Texture2D"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001AEA RID: 6890 RVA: 0x000AD31C File Offset: 0x000AB51C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NThoughtBubbleVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NThoughtBubbleVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NThoughtBubbleVfx.MethodName.SetTexture && args.Count == 1)
			{
				this.SetTexture(VariantUtils.ConvertTo<Texture2D>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001AEB RID: 6891 RVA: 0x000AD3AF File Offset: 0x000AB5AF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NThoughtBubbleVfx.MethodName._Ready || (ref method) == NThoughtBubbleVfx.MethodName._ExitTree || (ref method) == NThoughtBubbleVfx.MethodName.SetTexture || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001AEC RID: 6892 RVA: 0x000AD3E8 File Offset: 0x000AB5E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NThoughtBubbleVfx.PropertyName._container)
			{
				this._container = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._textureRect)
			{
				this._textureRect = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._contents)
			{
				this._contents = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._tail)
			{
				this._tail = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._style)
			{
				this._style = VariantUtils.ConvertTo<DialogueStyle>(ref value);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._side)
			{
				this._side = VariantUtils.ConvertTo<DialogueSide>(ref value);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._text)
			{
				this._text = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._texture)
			{
				this._texture = VariantUtils.ConvertTo<Texture2D>(ref value);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001AED RID: 6893 RVA: 0x000AD50C File Offset: 0x000AB70C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NThoughtBubbleVfx.PropertyName._container)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._container);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._textureRect)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._textureRect);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._contents)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._contents);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._tail)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._tail);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._style)
			{
				value = VariantUtils.CreateFrom<DialogueStyle>(ref this._style);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._side)
			{
				value = VariantUtils.CreateFrom<DialogueSide>(ref this._side);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._text)
			{
				value = VariantUtils.CreateFrom<string>(ref this._text);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._texture)
			{
				value = VariantUtils.CreateFrom<Texture2D>(ref this._texture);
				return true;
			}
			if ((ref name) == NThoughtBubbleVfx.PropertyName._tween)
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
		// Token: 0x06001AEE RID: 6894 RVA: 0x000AD664 File Offset: 0x000AB864
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NThoughtBubbleVfx.PropertyName._container, 0L, "", 4096L, false),
				new PropertyInfo(24L, NThoughtBubbleVfx.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NThoughtBubbleVfx.PropertyName._textureRect, 0L, "", 4096L, false),
				new PropertyInfo(24L, NThoughtBubbleVfx.PropertyName._contents, 0L, "", 4096L, false),
				new PropertyInfo(24L, NThoughtBubbleVfx.PropertyName._tail, 0L, "", 4096L, false),
				new PropertyInfo(2L, NThoughtBubbleVfx.PropertyName._style, 0L, "", 4096L, false),
				new PropertyInfo(2L, NThoughtBubbleVfx.PropertyName._side, 0L, "", 4096L, false),
				new PropertyInfo(4L, NThoughtBubbleVfx.PropertyName._text, 0L, "", 4096L, false),
				new PropertyInfo(24L, NThoughtBubbleVfx.PropertyName._texture, 0L, "", 4096L, false),
				new PropertyInfo(24L, NThoughtBubbleVfx.PropertyName._tween, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001AEF RID: 6895 RVA: 0x000AD7C0 File Offset: 0x000AB9C0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NThoughtBubbleVfx.PropertyName._container, Variant.From<Control>(ref this._container));
			info.AddProperty(NThoughtBubbleVfx.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NThoughtBubbleVfx.PropertyName._textureRect, Variant.From<TextureRect>(ref this._textureRect));
			info.AddProperty(NThoughtBubbleVfx.PropertyName._contents, Variant.From<Node2D>(ref this._contents));
			info.AddProperty(NThoughtBubbleVfx.PropertyName._tail, Variant.From<Node2D>(ref this._tail));
			info.AddProperty(NThoughtBubbleVfx.PropertyName._style, Variant.From<DialogueStyle>(ref this._style));
			info.AddProperty(NThoughtBubbleVfx.PropertyName._side, Variant.From<DialogueSide>(ref this._side));
			info.AddProperty(NThoughtBubbleVfx.PropertyName._text, Variant.From<string>(ref this._text));
			info.AddProperty(NThoughtBubbleVfx.PropertyName._texture, Variant.From<Texture2D>(ref this._texture));
			info.AddProperty(NThoughtBubbleVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
		}

		/// <inheritdoc />
		// Token: 0x06001AF0 RID: 6896 RVA: 0x000AD8B0 File Offset: 0x000ABAB0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._container, ref variant))
			{
				this._container = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._label, ref variant2))
			{
				this._label = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._textureRect, ref variant3))
			{
				this._textureRect = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._contents, ref variant4))
			{
				this._contents = variant4.As<Node2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._tail, ref variant5))
			{
				this._tail = variant5.As<Node2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._style, ref variant6))
			{
				this._style = variant6.As<DialogueStyle>();
			}
			Variant variant7;
			if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._side, ref variant7))
			{
				this._side = variant7.As<DialogueSide>();
			}
			Variant variant8;
			if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._text, ref variant8))
			{
				this._text = variant8.As<string>();
			}
			Variant variant9;
			if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._texture, ref variant9))
			{
				this._texture = variant9.As<Texture2D>();
			}
			Variant variant10;
			if (info.TryGetProperty(NThoughtBubbleVfx.PropertyName._tween, ref variant10))
			{
				this._tween = variant10.As<Tween>();
			}
		}

		// Token: 0x040009F7 RID: 2551
		private Control _container;

		// Token: 0x040009F8 RID: 2552
		private MegaRichTextLabel _label;

		// Token: 0x040009F9 RID: 2553
		private TextureRect _textureRect;

		// Token: 0x040009FA RID: 2554
		private Node2D _contents;

		// Token: 0x040009FB RID: 2555
		private Node2D _tail;

		// Token: 0x040009FC RID: 2556
		private const string _path = "res://scenes/vfx/vfx_thought_bubble.tscn";

		// Token: 0x040009FD RID: 2557
		private const float _spawnProportionToTopOfHitbox = 0.75f;

		// Token: 0x040009FE RID: 2558
		private const float _spawnProportionToEdgeOfHitbox = 0.75f;

		// Token: 0x040009FF RID: 2559
		private Vector2? _startPos;

		// Token: 0x04000A00 RID: 2560
		private DialogueStyle _style;

		// Token: 0x04000A01 RID: 2561
		private DialogueSide _side;

		// Token: 0x04000A02 RID: 2562
		[Nullable(2)]
		private string _text;

		// Token: 0x04000A03 RID: 2563
		[Nullable(2)]
		private Texture2D _texture;

		// Token: 0x04000A04 RID: 2564
		private double? _secondsToDisplay;

		// Token: 0x04000A05 RID: 2565
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x04000A06 RID: 2566
		[Nullable(2)]
		private CancellationTokenSource _cts;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200118B RID: 4491
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004461 RID: 17505
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004462 RID: 17506
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'SetTexture' method.
			/// </summary>
			// Token: 0x04004463 RID: 17507
			public static readonly StringName SetTexture = "SetTexture";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200118C RID: 4492
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_container' field.
			/// </summary>
			// Token: 0x04004464 RID: 17508
			public static readonly StringName _container = "_container";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x04004465 RID: 17509
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_textureRect' field.
			/// </summary>
			// Token: 0x04004466 RID: 17510
			public static readonly StringName _textureRect = "_textureRect";

			/// <summary>
			/// Cached name for the '_contents' field.
			/// </summary>
			// Token: 0x04004467 RID: 17511
			public static readonly StringName _contents = "_contents";

			/// <summary>
			/// Cached name for the '_tail' field.
			/// </summary>
			// Token: 0x04004468 RID: 17512
			public static readonly StringName _tail = "_tail";

			/// <summary>
			/// Cached name for the '_style' field.
			/// </summary>
			// Token: 0x04004469 RID: 17513
			public static readonly StringName _style = "_style";

			/// <summary>
			/// Cached name for the '_side' field.
			/// </summary>
			// Token: 0x0400446A RID: 17514
			public static readonly StringName _side = "_side";

			/// <summary>
			/// Cached name for the '_text' field.
			/// </summary>
			// Token: 0x0400446B RID: 17515
			public static readonly StringName _text = "_text";

			/// <summary>
			/// Cached name for the '_texture' field.
			/// </summary>
			// Token: 0x0400446C RID: 17516
			public static readonly StringName _texture = "_texture";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x0400446D RID: 17517
			public static readonly StringName _tween = "_tween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200118D RID: 4493
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}

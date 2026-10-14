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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	/// <summary>
	/// Speech bubble vfx used when creatures talk during combat or an NPC talks in a "room" style environment (i.e. Merchant)
	/// </summary>
	// Token: 0x020001F9 RID: 505
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NSpeechBubbleVfx.cs")]
	public class NSpeechBubbleVfx : Control
	{
		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x060019EC RID: 6636 RVA: 0x000A5B20 File Offset: 0x000A3D20
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>("res://scenes/vfx/vfx_speech_bubble.tscn");
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x000A5B2C File Offset: 0x000A3D2C
		// (set) Token: 0x060019EE RID: 6638 RVA: 0x000A5B34 File Offset: 0x000A3D34
		public double SecondsToDisplay { get; private set; }

		/// <summary>
		/// Creates a speech bubble given a creature. Use this when you want creatures to talk during combat!
		/// </summary>
		// Token: 0x060019EF RID: 6639 RVA: 0x000A5B40 File Offset: 0x000A3D40
		[return: Nullable(2)]
		public static NSpeechBubbleVfx Create(string text, Creature speaker, double secondsToDisplay, VfxColor vfxColor = VfxColor.White)
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
			NSpeechBubbleVfx nspeechBubbleVfx = NSpeechBubbleVfx.CreateInternal(text, dialogueSide2, secondsToDisplay);
			nspeechBubbleVfx._startPos = NSpeechBubbleVfx.GetCreatureSpeechPosition(speaker);
			nspeechBubbleVfx._vfxColor = vfxColor;
			return nspeechBubbleVfx;
		}

		/// <summary>
		/// When we want to specify the absolute position of the speech bubble vfx.
		/// </summary>
		// Token: 0x060019F0 RID: 6640 RVA: 0x000A5B94 File Offset: 0x000A3D94
		[return: Nullable(2)]
		public static NSpeechBubbleVfx Create(string text, DialogueSide side, Vector2 globalPosition, double secondsToDisplay, VfxColor vfxColor = VfxColor.White)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NSpeechBubbleVfx nspeechBubbleVfx = NSpeechBubbleVfx.CreateInternal(text, side, secondsToDisplay);
			nspeechBubbleVfx._startPos = globalPosition;
			nspeechBubbleVfx._vfxColor = vfxColor;
			return nspeechBubbleVfx;
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x000A5BC4 File Offset: 0x000A3DC4
		public override void _Ready()
		{
			this._contents = base.GetNode<Node2D>("%Contents");
			this._bubble = base.GetNode<Sprite2D>("%Bubble");
			this._shadow = base.GetNode<Sprite2D>("%Shadow");
			this._container = base.GetNode<Control>("%Container");
			this._label = base.GetNode<MegaRichTextLabel>("%Text");
			this._hsv = (ShaderMaterial)this._bubble.Material;
			this.SetSpeechBubbleColor();
			base.GlobalPosition = this._startPos;
			if (this._side == DialogueSide.Right)
			{
				this._container.Position = new Vector2(-this._container.Size.X - this._container.Position.X, this._container.Position.Y);
				this._bubble.FlipH = true;
				this._shadow.FlipH = true;
			}
			TaskHelper.RunSafely(this.AnimateSpeechBubble());
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x000A5CD8 File Offset: 0x000A3ED8
		private void SetSpeechBubbleColor()
		{
			switch (this._vfxColor)
			{
			case VfxColor.Red:
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, 0.49f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, 2.5f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, 0.38f);
				return;
			case VfxColor.Green:
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, 0.8f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, 1.5f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, 0.6f);
				return;
			case VfxColor.Blue:
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, 0.05f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, 1.3f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, 0.55f);
				return;
			case VfxColor.Purple:
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, 0.3f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, 0.6f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, 0.5f);
				return;
			case VfxColor.Black:
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, 1f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, 0.25f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, 0.3f);
				return;
			case VfxColor.Orange:
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, 0.6f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, 4f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, 0.5f);
				return;
			case VfxColor.Swamp:
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, 0.72f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, 1.7f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, 0.5f);
				return;
			case VfxColor.DarkGray:
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, 0.093f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, 0.2f);
				this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, 0.4f);
				return;
			}
			this._hsv.SetShaderParameter(NSpeechBubbleVfx._h, 1f);
			this._hsv.SetShaderParameter(NSpeechBubbleVfx._s, 0.9f);
			this._hsv.SetShaderParameter(NSpeechBubbleVfx._v, 0.5f);
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x000A5FE9 File Offset: 0x000A41E9
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween == null)
			{
				return;
			}
			tween.Kill();
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x000A5FFC File Offset: 0x000A41FC
		private async Task AnimateSpeechBubble()
		{
			this._tween = base.CreateTween().SetParallel(true);
			float num = 60f * ((this._side == DialogueSide.Left) ? (-1f) : 1f);
			MegaRichTextLabel label = this._label;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
			defaultInterpolatedStringHandler.AppendLiteral("[center][fly_in offset_x=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(num);
			defaultInterpolatedStringHandler.AppendLiteral(" offset_y=40]");
			defaultInterpolatedStringHandler.AppendFormatted(this._text);
			defaultInterpolatedStringHandler.AppendLiteral("[/fly_in][/center]");
			label.Text = defaultInterpolatedStringHandler.ToStringAndClear();
			this._tween.TweenProperty(this, "modulate:a", 1f, 0.4).From(0f);
			this._tween.TweenProperty(this._label, "visible_ratio", 1f, 0.4).From(0f);
			this._tween.TweenProperty(this._bubble, "scale", new Vector2(0.75f, 0.75f), 0.5).From(new Vector2(0.25f, 0.25f)).SetEase(1L)
				.SetTrans(5L);
			this._tween.TweenProperty(this, "rotation_degrees", 0f, 0.3).From(7f).SetEase(1L)
				.SetTrans(1L);
			this._tween.Chain();
			double num2 = Math.Max(this.SecondsToDisplay - 1.0, 1.0);
			this._tween.TweenInterval(num2);
			this._tween.Chain();
			await this.AnimOutInternal();
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x000A6040 File Offset: 0x000A4240
		public async Task AnimOut()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			await this.AnimOutInternal();
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x000A6084 File Offset: 0x000A4284
		private async Task AnimOutInternal()
		{
			this._tween.TweenProperty(this, "modulate", StsColors.transparentBlack, 0.4).SetEase(1L).SetTrans(1L);
			bool flag = await this._tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x000A60C8 File Offset: 0x000A42C8
		private static NSpeechBubbleVfx CreateInternal(string text, DialogueSide side, double secondsToDisplay)
		{
			NSpeechBubbleVfx nspeechBubbleVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/vfx_speech_bubble.tscn").Instantiate<NSpeechBubbleVfx>(0L);
			nspeechBubbleVfx._side = side;
			nspeechBubbleVfx._text = text;
			nspeechBubbleVfx.SecondsToDisplay = secondsToDisplay;
			return nspeechBubbleVfx;
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x000A6102 File Offset: 0x000A4302
		public override void _Process(double delta)
		{
			this._elapsedTime += (float)delta * 4.5f;
			this._contents.Position = new Vector2(0f, Mathf.Sin(this._elapsedTime) * 2f);
		}

		/// <summary>
		/// Given a creature, dynamically calculates and returns the position of where the "mouth" should be.
		/// Used during combat, where we don't know which side a creature could be on.
		/// </summary>
		// Token: 0x060019F9 RID: 6649 RVA: 0x000A6140 File Offset: 0x000A4340
		private static Vector2 GetCreatureSpeechPosition(Creature speaker)
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

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060019FA RID: 6650 RVA: 0x000A61FC File Offset: 0x000A43FC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(NSpeechBubbleVfx.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false),
					new PropertyInfo(2L, "side", 0L, "", 6L, false),
					new PropertyInfo(5L, "globalPosition", 0L, "", 6L, false),
					new PropertyInfo(3L, "secondsToDisplay", 0L, "", 6L, false),
					new PropertyInfo(2L, "vfxColor", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSpeechBubbleVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSpeechBubbleVfx.MethodName.SetSpeechBubbleColor, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSpeechBubbleVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NSpeechBubbleVfx.MethodName.CreateInternal, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "text", 0L, "", 6L, false),
					new PropertyInfo(2L, "side", 0L, "", 6L, false),
					new PropertyInfo(3L, "secondsToDisplay", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NSpeechBubbleVfx.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060019FB RID: 6651 RVA: 0x000A6478 File Offset: 0x000A4678
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSpeechBubbleVfx.MethodName.Create && args.Count == 5)
			{
				NSpeechBubbleVfx nspeechBubbleVfx = NSpeechBubbleVfx.Create(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<DialogueSide>(args[1]), VariantUtils.ConvertTo<Vector2>(args[2]), VariantUtils.ConvertTo<double>(args[3]), VariantUtils.ConvertTo<VfxColor>(args[4]));
				ret = VariantUtils.CreateFrom<NSpeechBubbleVfx>(ref nspeechBubbleVfx);
				return true;
			}
			if ((ref method) == NSpeechBubbleVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpeechBubbleVfx.MethodName.SetSpeechBubbleColor && args.Count == 0)
			{
				this.SetSpeechBubbleColor();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpeechBubbleVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NSpeechBubbleVfx.MethodName.CreateInternal && args.Count == 3)
			{
				NSpeechBubbleVfx nspeechBubbleVfx2 = NSpeechBubbleVfx.CreateInternal(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<DialogueSide>(args[1]), VariantUtils.ConvertTo<double>(args[2]));
				ret = VariantUtils.CreateFrom<NSpeechBubbleVfx>(ref nspeechBubbleVfx2);
				return true;
			}
			if ((ref method) == NSpeechBubbleVfx.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x000A65F0 File Offset: 0x000A47F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NSpeechBubbleVfx.MethodName.Create && args.Count == 5)
			{
				NSpeechBubbleVfx nspeechBubbleVfx = NSpeechBubbleVfx.Create(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<DialogueSide>(args[1]), VariantUtils.ConvertTo<Vector2>(args[2]), VariantUtils.ConvertTo<double>(args[3]), VariantUtils.ConvertTo<VfxColor>(args[4]));
				ret = VariantUtils.CreateFrom<NSpeechBubbleVfx>(ref nspeechBubbleVfx);
				return true;
			}
			if ((ref method) == NSpeechBubbleVfx.MethodName.CreateInternal && args.Count == 3)
			{
				NSpeechBubbleVfx nspeechBubbleVfx2 = NSpeechBubbleVfx.CreateInternal(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<DialogueSide>(args[1]), VariantUtils.ConvertTo<double>(args[2]));
				ret = VariantUtils.CreateFrom<NSpeechBubbleVfx>(ref nspeechBubbleVfx2);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060019FD RID: 6653 RVA: 0x000A66C8 File Offset: 0x000A48C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NSpeechBubbleVfx.MethodName.Create || (ref method) == NSpeechBubbleVfx.MethodName._Ready || (ref method) == NSpeechBubbleVfx.MethodName.SetSpeechBubbleColor || (ref method) == NSpeechBubbleVfx.MethodName._ExitTree || (ref method) == NSpeechBubbleVfx.MethodName.CreateInternal || (ref method) == NSpeechBubbleVfx.MethodName._Process || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060019FE RID: 6654 RVA: 0x000A6738 File Offset: 0x000A4938
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NSpeechBubbleVfx.PropertyName.SecondsToDisplay)
			{
				this.SecondsToDisplay = VariantUtils.ConvertTo<double>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._container)
			{
				this._container = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._label)
			{
				this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._contents)
			{
				this._contents = VariantUtils.ConvertTo<Node2D>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._bubble)
			{
				this._bubble = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._shadow)
			{
				this._shadow = VariantUtils.ConvertTo<Sprite2D>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._hsv)
			{
				this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._startPos)
			{
				this._startPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._vfxColor)
			{
				this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._style)
			{
				this._style = VariantUtils.ConvertTo<DialogueStyle>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._side)
			{
				this._side = VariantUtils.ConvertTo<DialogueSide>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._text)
			{
				this._text = VariantUtils.ConvertTo<string>(ref value);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._elapsedTime)
			{
				this._elapsedTime = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060019FF RID: 6655 RVA: 0x000A68C8 File Offset: 0x000A4AC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NSpeechBubbleVfx.PropertyName.SecondsToDisplay)
			{
				double secondsToDisplay = this.SecondsToDisplay;
				value = VariantUtils.CreateFrom<double>(ref secondsToDisplay);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._container)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._container);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._label)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._contents)
			{
				value = VariantUtils.CreateFrom<Node2D>(ref this._contents);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._bubble)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._bubble);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._shadow)
			{
				value = VariantUtils.CreateFrom<Sprite2D>(ref this._shadow);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._hsv)
			{
				value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._startPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._startPos);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._vfxColor)
			{
				value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._style)
			{
				value = VariantUtils.CreateFrom<DialogueStyle>(ref this._style);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._side)
			{
				value = VariantUtils.CreateFrom<DialogueSide>(ref this._side);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._text)
			{
				value = VariantUtils.CreateFrom<string>(ref this._text);
				return true;
			}
			if ((ref name) == NSpeechBubbleVfx.PropertyName._elapsedTime)
			{
				value = VariantUtils.CreateFrom<float>(ref this._elapsedTime);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001A00 RID: 6656 RVA: 0x000A6AA0 File Offset: 0x000A4CA0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NSpeechBubbleVfx.PropertyName._container, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSpeechBubbleVfx.PropertyName._label, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSpeechBubbleVfx.PropertyName._contents, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSpeechBubbleVfx.PropertyName._bubble, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSpeechBubbleVfx.PropertyName._shadow, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSpeechBubbleVfx.PropertyName._hsv, 0L, "", 4096L, false),
				new PropertyInfo(24L, NSpeechBubbleVfx.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NSpeechBubbleVfx.PropertyName._startPos, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSpeechBubbleVfx.PropertyName._vfxColor, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSpeechBubbleVfx.PropertyName._style, 0L, "", 4096L, false),
				new PropertyInfo(2L, NSpeechBubbleVfx.PropertyName._side, 0L, "", 4096L, false),
				new PropertyInfo(4L, NSpeechBubbleVfx.PropertyName._text, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSpeechBubbleVfx.PropertyName.SecondsToDisplay, 0L, "", 4096L, false),
				new PropertyInfo(3L, NSpeechBubbleVfx.PropertyName._elapsedTime, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001A01 RID: 6657 RVA: 0x000A6C7C File Offset: 0x000A4E7C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName secondsToDisplay = NSpeechBubbleVfx.PropertyName.SecondsToDisplay;
			double secondsToDisplay2 = this.SecondsToDisplay;
			info.AddProperty(secondsToDisplay, Variant.From<double>(ref secondsToDisplay2));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._container, Variant.From<Control>(ref this._container));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._contents, Variant.From<Node2D>(ref this._contents));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._bubble, Variant.From<Sprite2D>(ref this._bubble));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._shadow, Variant.From<Sprite2D>(ref this._shadow));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._startPos, Variant.From<Vector2>(ref this._startPos));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._style, Variant.From<DialogueStyle>(ref this._style));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._side, Variant.From<DialogueSide>(ref this._side));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._text, Variant.From<string>(ref this._text));
			info.AddProperty(NSpeechBubbleVfx.PropertyName._elapsedTime, Variant.From<float>(ref this._elapsedTime));
		}

		/// <inheritdoc />
		// Token: 0x06001A02 RID: 6658 RVA: 0x000A6DC8 File Offset: 0x000A4FC8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName.SecondsToDisplay, ref variant))
			{
				this.SecondsToDisplay = variant.As<double>();
			}
			Variant variant2;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._container, ref variant2))
			{
				this._container = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._label, ref variant3))
			{
				this._label = variant3.As<MegaRichTextLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._contents, ref variant4))
			{
				this._contents = variant4.As<Node2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._bubble, ref variant5))
			{
				this._bubble = variant5.As<Sprite2D>();
			}
			Variant variant6;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._shadow, ref variant6))
			{
				this._shadow = variant6.As<Sprite2D>();
			}
			Variant variant7;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._hsv, ref variant7))
			{
				this._hsv = variant7.As<ShaderMaterial>();
			}
			Variant variant8;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._tween, ref variant8))
			{
				this._tween = variant8.As<Tween>();
			}
			Variant variant9;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._startPos, ref variant9))
			{
				this._startPos = variant9.As<Vector2>();
			}
			Variant variant10;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._vfxColor, ref variant10))
			{
				this._vfxColor = variant10.As<VfxColor>();
			}
			Variant variant11;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._style, ref variant11))
			{
				this._style = variant11.As<DialogueStyle>();
			}
			Variant variant12;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._side, ref variant12))
			{
				this._side = variant12.As<DialogueSide>();
			}
			Variant variant13;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._text, ref variant13))
			{
				this._text = variant13.As<string>();
			}
			Variant variant14;
			if (info.TryGetProperty(NSpeechBubbleVfx.PropertyName._elapsedTime, ref variant14))
			{
				this._elapsedTime = variant14.As<float>();
			}
		}

		// Token: 0x04000995 RID: 2453
		private static readonly StringName _h = new StringName("h");

		// Token: 0x04000996 RID: 2454
		private static readonly StringName _s = new StringName("s");

		// Token: 0x04000997 RID: 2455
		private static readonly StringName _v = new StringName("v");

		// Token: 0x04000998 RID: 2456
		private Control _container;

		// Token: 0x04000999 RID: 2457
		private MegaRichTextLabel _label;

		// Token: 0x0400099A RID: 2458
		private Node2D _contents;

		// Token: 0x0400099B RID: 2459
		private Sprite2D _bubble;

		// Token: 0x0400099C RID: 2460
		private Sprite2D _shadow;

		// Token: 0x0400099D RID: 2461
		private ShaderMaterial _hsv;

		// Token: 0x0400099E RID: 2462
		private const string _path = "res://scenes/vfx/vfx_speech_bubble.tscn";

		// Token: 0x0400099F RID: 2463
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040009A0 RID: 2464
		private const float _spawnProportionToTopOfHitbox = 0.75f;

		// Token: 0x040009A1 RID: 2465
		private const float _spawnProportionToEdgeOfHitbox = 0.75f;

		// Token: 0x040009A2 RID: 2466
		private Vector2 _startPos;

		// Token: 0x040009A3 RID: 2467
		private VfxColor _vfxColor;

		// Token: 0x040009A4 RID: 2468
		private DialogueStyle _style;

		// Token: 0x040009A5 RID: 2469
		private DialogueSide _side;

		// Token: 0x040009A6 RID: 2470
		private string _text;

		// Token: 0x040009A8 RID: 2472
		private float _elapsedTime = 3.14f;

		// Token: 0x040009A9 RID: 2473
		private const float _waveFrequency = 4.5f;

		// Token: 0x040009AA RID: 2474
		private const float _waveAmplitude = 2f;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001150 RID: 4432
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x040043A2 RID: 17314
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040043A3 RID: 17315
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetSpeechBubbleColor' method.
			/// </summary>
			// Token: 0x040043A4 RID: 17316
			public static readonly StringName SetSpeechBubbleColor = "SetSpeechBubbleColor";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040043A5 RID: 17317
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'CreateInternal' method.
			/// </summary>
			// Token: 0x040043A6 RID: 17318
			public static readonly StringName CreateInternal = "CreateInternal";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x040043A7 RID: 17319
			public static readonly StringName _Process = "_Process";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001151 RID: 4433
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'SecondsToDisplay' property.
			/// </summary>
			// Token: 0x040043A8 RID: 17320
			public static readonly StringName SecondsToDisplay = "SecondsToDisplay";

			/// <summary>
			/// Cached name for the '_container' field.
			/// </summary>
			// Token: 0x040043A9 RID: 17321
			public static readonly StringName _container = "_container";

			/// <summary>
			/// Cached name for the '_label' field.
			/// </summary>
			// Token: 0x040043AA RID: 17322
			public static readonly StringName _label = "_label";

			/// <summary>
			/// Cached name for the '_contents' field.
			/// </summary>
			// Token: 0x040043AB RID: 17323
			public static readonly StringName _contents = "_contents";

			/// <summary>
			/// Cached name for the '_bubble' field.
			/// </summary>
			// Token: 0x040043AC RID: 17324
			public static readonly StringName _bubble = "_bubble";

			/// <summary>
			/// Cached name for the '_shadow' field.
			/// </summary>
			// Token: 0x040043AD RID: 17325
			public static readonly StringName _shadow = "_shadow";

			/// <summary>
			/// Cached name for the '_hsv' field.
			/// </summary>
			// Token: 0x040043AE RID: 17326
			public static readonly StringName _hsv = "_hsv";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040043AF RID: 17327
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_startPos' field.
			/// </summary>
			// Token: 0x040043B0 RID: 17328
			public static readonly StringName _startPos = "_startPos";

			/// <summary>
			/// Cached name for the '_vfxColor' field.
			/// </summary>
			// Token: 0x040043B1 RID: 17329
			public static readonly StringName _vfxColor = "_vfxColor";

			/// <summary>
			/// Cached name for the '_style' field.
			/// </summary>
			// Token: 0x040043B2 RID: 17330
			public static readonly StringName _style = "_style";

			/// <summary>
			/// Cached name for the '_side' field.
			/// </summary>
			// Token: 0x040043B3 RID: 17331
			public static readonly StringName _side = "_side";

			/// <summary>
			/// Cached name for the '_text' field.
			/// </summary>
			// Token: 0x040043B4 RID: 17332
			public static readonly StringName _text = "_text";

			/// <summary>
			/// Cached name for the '_elapsedTime' field.
			/// </summary>
			// Token: 0x040043B5 RID: 17333
			public static readonly StringName _elapsedTime = "_elapsedTime";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001152 RID: 4434
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}

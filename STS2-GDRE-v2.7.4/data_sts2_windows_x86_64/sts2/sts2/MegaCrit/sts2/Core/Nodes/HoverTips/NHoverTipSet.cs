using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.HoverTips
{
	// Token: 0x0200039A RID: 922
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/HoverTips/NHoverTipSet.cs")]
	public class NHoverTipSet : Control
	{
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x0600373A RID: 14138 RVA: 0x0019F224 File Offset: 0x0019D424
		private static Node HoverTipsContainer
		{
			get
			{
				return NGame.Instance.HoverTipsContainer;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x0600373B RID: 14139 RVA: 0x0019F230 File Offset: 0x0019D430
		private Vector2 TextHoverTipDimensions
		{
			get
			{
				return this._textHoverTipContainer.Size;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x0600373C RID: 14140 RVA: 0x0019F23D File Offset: 0x0019D43D
		private Vector2 CardHoverTipDimensions
		{
			get
			{
				return this._cardHoverTipContainer.Size;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x0600373D RID: 14141 RVA: 0x0019F24A File Offset: 0x0019D44A
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[] { "res://scenes/ui/hover_tip.tscn", "res://scenes/ui/hover_tip_set.tscn", "res://materials/ui/hover_tip_debuff.tres" });
			}
		}

		/// <summary>
		/// Allows the HoverTip to follow the node it's tied to.
		/// </summary>
		// Token: 0x0600373E RID: 14142 RVA: 0x0019F26F File Offset: 0x0019D46F
		public void SetFollowOwner()
		{
			this._followOwner = true;
			this._followOffset = this._owner.GlobalPosition - base.GlobalPosition;
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x0019F294 File Offset: 0x0019D494
		[return: Nullable(2)]
		public static NHoverTipSet CreateAndShow(Control owner, IHoverTip hoverTip, HoverTipAlignment alignment = HoverTipAlignment.None)
		{
			return NHoverTipSet.CreateAndShow(owner, new <>z__ReadOnlySingleElementList<IHoverTip>(hoverTip), alignment);
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x0019F2A4 File Offset: 0x0019D4A4
		[return: Nullable(2)]
		public static NHoverTipSet CreateAndShow(Control owner, IEnumerable<IHoverTip> hoverTips, HoverTipAlignment alignment = HoverTipAlignment.None)
		{
			if (NHoverTipSet.shouldBlockHoverTips)
			{
				return null;
			}
			NHoverTipSet nhoverTipSet = PreloadManager.Cache.GetScene("res://scenes/ui/hover_tip_set.tscn").Instantiate<NHoverTipSet>(0L);
			NHoverTipSet.HoverTipsContainer.AddChildSafely(nhoverTipSet);
			NHoverTipSet._activeHoverTips.Add(owner, nhoverTipSet);
			nhoverTipSet.Init(owner, hoverTips);
			if (NGame.IsDebugHidingHoverTips)
			{
				nhoverTipSet.Visible = false;
			}
			owner.Connect(Node.SignalName.TreeExiting, Callable.From(delegate
			{
				NHoverTipSet.Remove(owner);
			}), 0U);
			nhoverTipSet.SetAlignment(owner, alignment);
			return nhoverTipSet;
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x0019F348 File Offset: 0x0019D548
		public static NHoverTipSet CreateAndShowMapPointHistory(Control owner, NMapPointHistoryHoverTip historyHoverTip)
		{
			NHoverTipSet nhoverTipSet = PreloadManager.Cache.GetScene("res://scenes/ui/hover_tip_set.tscn").Instantiate<NHoverTipSet>(0L);
			nhoverTipSet._owner = owner;
			NHoverTipSet.HoverTipsContainer.AddChildSafely(nhoverTipSet);
			NHoverTipSet._activeHoverTips.Add(owner, nhoverTipSet);
			nhoverTipSet._textHoverTipContainer.AddChildSafely(historyHoverTip);
			if (NGame.IsDebugHidingHoverTips)
			{
				nhoverTipSet.Visible = false;
			}
			owner.Connect(Node.SignalName.TreeExiting, Callable.From(delegate
			{
				NHoverTipSet.Remove(owner);
			}), 0U);
			return nhoverTipSet;
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x0019F3E0 File Offset: 0x0019D5E0
		public override void _Ready()
		{
			this._textHoverTipContainer = new VFlowContainer();
			this._textHoverTipContainer.Name = NHoverTipSet._textHoverTipContainerStr;
			this._textHoverTipContainer.MouseFilter = 2L;
			this.AddChildSafely(this._textHoverTipContainer);
			this._cardHoverTipContainer = new NHoverTipCardContainer();
			this._cardHoverTipContainer.Name = NHoverTipSet._cardHoverTipContainerStr;
			this._cardHoverTipContainer.MouseFilter = 2L;
			this.AddChildSafely(this._cardHoverTipContainer);
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x0019F455 File Offset: 0x0019D655
		public override void _Process(double delta)
		{
			if (!this._followOwner)
			{
				return;
			}
			if (this._owner == null)
			{
				return;
			}
			base.GlobalPosition = this._owner.GlobalPosition - this._followOffset + this._extraOffset;
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x0019F490 File Offset: 0x0019D690
		private void Init(Control owner, IEnumerable<IHoverTip> hoverTips)
		{
			this._owner = owner;
			foreach (IHoverTip hoverTip in IHoverTip.RemoveDupes(hoverTips))
			{
				if (hoverTip is HoverTip)
				{
					HoverTip hoverTip2 = (HoverTip)hoverTip;
					Control control = PreloadManager.Cache.GetScene("res://scenes/ui/hover_tip.tscn").Instantiate<Control>(0L);
					this._textHoverTipContainer.AddChildSafely(control);
					MegaLabel node = control.GetNode<MegaLabel>("%Title");
					if (hoverTip2.Title == null)
					{
						node.Visible = false;
					}
					else
					{
						node.SetTextAutoSize(hoverTip2.Title);
					}
					control.GetNode<MegaRichTextLabel>("%Description").Text = hoverTip2.Description;
					control.GetNode<MegaRichTextLabel>("%Description").AutowrapMode = (hoverTip2.ShouldOverrideTextOverflow ? 0L : 3L);
					control.GetNode<TextureRect>("%Icon").Texture = hoverTip2.Icon;
					if (hoverTip2.IsDebuff)
					{
						control.GetNode<CanvasItem>("%Bg").Material = PreloadManager.Cache.GetMaterial("res://materials/ui/hover_tip_debuff.tres");
					}
					control.ResetSize();
					if (this._textHoverTipContainer.Size.Y + control.Size.Y + 5f < NGame.Instance.GetViewportRect().Size.Y - 50f)
					{
						this._textHoverTipContainer.Size = new Vector2(360f, this._textHoverTipContainer.Size.Y + control.Size.Y + 5f);
					}
					else
					{
						this._textHoverTipContainer.Alignment = 1L;
					}
				}
				else
				{
					this._cardHoverTipContainer.Add((CardHoverTip)hoverTip);
				}
				AbstractModel canonicalModel = hoverTip.CanonicalModel;
				CardModel cardModel = canonicalModel as CardModel;
				if (cardModel == null)
				{
					RelicModel relicModel = canonicalModel as RelicModel;
					if (relicModel == null)
					{
						PotionModel potionModel = canonicalModel as PotionModel;
						if (potionModel != null)
						{
							SaveManager.Instance.MarkPotionAsSeen(potionModel);
						}
					}
					else
					{
						SaveManager.Instance.MarkRelicAsSeen(relicModel);
					}
				}
				else
				{
					SaveManager.Instance.MarkCardAsSeen(cardModel);
				}
			}
		}

		/// <summary>
		/// Sets the position of the hover tip relative to the given node based on the alignment set.
		/// </summary>
		/// <param name="node">The control node that we are aligning the hover tip to</param>.
		/// <param name="alignment">Dictates the position of the hover tip relative to the given node.
		/// ie: top left side of the node, Top right side of the node</param>
		// Token: 0x06003745 RID: 14149 RVA: 0x0019F6DC File Offset: 0x0019D8DC
		public void SetAlignment(Control node, HoverTipAlignment alignment)
		{
			if (alignment != HoverTipAlignment.None)
			{
				this._textHoverTipContainer.Position = Vector2.Zero;
				if (alignment == HoverTipAlignment.Left)
				{
					this._textHoverTipContainer.GlobalPosition = node.GlobalPosition;
					this._textHoverTipContainer.Position += Vector2.Left * this._textHoverTipContainer.Size.X;
					this._textHoverTipContainer.ReverseFill = true;
					this._cardHoverTipContainer.LayoutResizeAndReposition(node.GlobalPosition + new Vector2(node.Size.X, 0f) * node.Scale, HoverTipAlignment.Right);
				}
				else if (alignment == HoverTipAlignment.Right)
				{
					this._cardHoverTipContainer.LayoutResizeAndReposition(node.GlobalPosition, HoverTipAlignment.Left);
					this._textHoverTipContainer.GlobalPosition = node.GlobalPosition + new Vector2(node.Size.X, 0f) * node.Scale;
				}
				else if (alignment == HoverTipAlignment.Center)
				{
					base.GlobalPosition = node.GlobalPosition + Vector2.Down * node.Size.Y * 1.5f;
					this._cardHoverTipContainer.GlobalPosition += Vector2.Down * this._textHoverTipContainer.Size.Y;
					this._cardHoverTipContainer.LayoutResizeAndReposition(this._cardHoverTipContainer.GlobalPosition, alignment);
				}
			}
			this.CorrectVerticalOverflow();
			this.CorrectHorizontalOverflow();
		}

		/// <summary>
		/// Align the hover tip for the given relic. We separate it because there are special positioning
		/// rules for relics.
		/// </summary>
		/// <param name="relic">The relic we are aligning too</param>
		// Token: 0x06003746 RID: 14150 RVA: 0x0019F864 File Offset: 0x0019DA64
		public void SetAlignmentForRelic(NRelic relic)
		{
			HoverTipAlignment hoverTipAlignment = HoverTip.GetHoverTipAlignment(relic, 0.75f);
			Vector2 vector = relic.Icon.Size * relic.GetGlobalTransform().Scale;
			this._textHoverTipContainer.GlobalPosition = relic.GlobalPosition + Vector2.Down * (vector.Y + 10f);
			if (hoverTipAlignment == HoverTipAlignment.Left)
			{
				this._textHoverTipContainer.Position += Vector2.Left * (this._textHoverTipContainer.Size.X - vector.X);
			}
			this._cardHoverTipContainer.LayoutResizeAndReposition(this._textHoverTipContainer.GlobalPosition + Vector2.Down * this._textHoverTipContainer.Size.Y, hoverTipAlignment);
			if (hoverTipAlignment == HoverTipAlignment.Left)
			{
				this._cardHoverTipContainer.GlobalPosition = new Vector2(this._textHoverTipContainer.GlobalPosition.X, this._cardHoverTipContainer.GlobalPosition.Y);
			}
			float y = NGame.Instance.GetViewportRect().Size.Y;
			if (relic.GlobalPosition.Y > y * 0.75f)
			{
				this._textHoverTipContainer.GlobalPosition = relic.GlobalPosition + Vector2.Up * this._textHoverTipContainer.Size.Y;
			}
			this.CorrectVerticalOverflow();
			this.CorrectHorizontalOverflow();
			if (this._textHoverTipContainer.GetRect().Intersects(this._cardHoverTipContainer.GetRect(), false))
			{
				if (hoverTipAlignment == HoverTipAlignment.Left)
				{
					this._cardHoverTipContainer.GlobalPosition = this._textHoverTipContainer.GlobalPosition + this._textHoverTipContainer.Size.X * Vector2.Right;
				}
				else
				{
					this._cardHoverTipContainer.GlobalPosition = this._textHoverTipContainer.GlobalPosition + this._cardHoverTipContainer.Size.X * Vector2.Left;
				}
				this.CorrectVerticalOverflow();
			}
		}

		/// <summary>
		/// Positions the hovertips given a Card. We separate it because there are special positioning
		/// rules for cards.
		/// </summary>
		/// <param name="holder">The holder that we are aligning to</param>
		// Token: 0x06003747 RID: 14151 RVA: 0x0019FA70 File Offset: 0x0019DC70
		public void SetAlignmentForCardHolder(NCardHolder holder)
		{
			HoverTipAlignment hoverTipAlignment = HoverTip.GetHoverTipAlignment(holder, 0.75f);
			this._textHoverTipContainer.Position = Vector2.Zero;
			Control hitbox = holder.Hitbox;
			if (hoverTipAlignment == HoverTipAlignment.Left)
			{
				this._textHoverTipContainer.GlobalPosition = hitbox.GlobalPosition;
				this._textHoverTipContainer.Position += Vector2.Left * this._textHoverTipContainer.Size.X - new Vector2(10f, 0f);
				this._textHoverTipContainer.ReverseFill = true;
				this._cardHoverTipContainer.LayoutResizeAndReposition(hitbox.GlobalPosition + new Vector2(hitbox.Size.X, 0f) * hitbox.Scale, HoverTipAlignment.Right);
			}
			else
			{
				Vector2 vector = hitbox.GlobalPosition;
				if (holder.CardModel != null && (holder.CardModel.CurrentStarCost > 0 || holder.CardModel.HasStarCostX))
				{
					vector += Vector2.Left * 15f;
				}
				this._cardHoverTipContainer.LayoutResizeAndReposition(vector, HoverTipAlignment.Left);
				this._textHoverTipContainer.GlobalPosition = hitbox.GlobalPosition + new Vector2(hitbox.Size.X + 10f, 0f) * hitbox.Scale * holder.Scale;
			}
			this.CorrectVerticalOverflow();
			this.CorrectHorizontalOverflow();
			this.SetFollowOwner();
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x0019FBEC File Offset: 0x0019DDEC
		private void CorrectVerticalOverflow()
		{
			float y = NGame.Instance.GetViewportRect().Size.Y;
			if (this._textHoverTipContainer.GlobalPosition.Y + this._textHoverTipContainer.Size.Y > y)
			{
				this._textHoverTipContainer.GlobalPosition = new Vector2(this._textHoverTipContainer.GlobalPosition.X, y - this._textHoverTipContainer.Size.Y);
			}
			if (this._cardHoverTipContainer.GlobalPosition.Y + this._cardHoverTipContainer.Size.Y > y)
			{
				this._cardHoverTipContainer.GlobalPosition = new Vector2(this._cardHoverTipContainer.GlobalPosition.X, y - this._cardHoverTipContainer.Size.Y);
			}
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x0019FCC0 File Offset: 0x0019DEC0
		private void CorrectHorizontalOverflow()
		{
			float x = NGame.Instance.GetViewportRect().Size.X;
			Vector2 globalPosition = this._cardHoverTipContainer.GlobalPosition;
			float x2 = this._cardHoverTipContainer.Size.X;
			Vector2 globalPosition2 = this._textHoverTipContainer.GlobalPosition;
			float x3 = this._textHoverTipContainer.Size.X;
			if (globalPosition.X + x2 <= x && globalPosition2.X + x3 > x)
			{
				float num = globalPosition.X - x3;
				this._textHoverTipContainer.GlobalPosition = new Vector2(num, globalPosition.Y);
				return;
			}
			if (globalPosition.X + x2 > x || globalPosition2.X + x3 > x)
			{
				float num2 = globalPosition2.X + x3 - x2;
				this._cardHoverTipContainer.GlobalPosition = new Vector2(num2, globalPosition.Y);
				this._textHoverTipContainer.GlobalPosition += Vector2.Left * x2;
				return;
			}
			if (globalPosition.X < 0f || globalPosition2.X < 0f)
			{
				float x4 = globalPosition2.X;
				this._cardHoverTipContainer.GlobalPosition = new Vector2(x4, globalPosition.Y);
				this._textHoverTipContainer.GlobalPosition += Vector2.Right * x2;
			}
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x0019FE18 File Offset: 0x0019E018
		public static void Clear()
		{
			foreach (Control control in NHoverTipSet._activeHoverTips.Keys)
			{
				NHoverTipSet.Remove(control);
			}
		}

		// Token: 0x0600374B RID: 14155 RVA: 0x0019FE70 File Offset: 0x0019E070
		public static void Remove(Control owner)
		{
			NHoverTipSet nhoverTipSet;
			if (NHoverTipSet._activeHoverTips.TryGetValue(owner, out nhoverTipSet))
			{
				nhoverTipSet.QueueFreeSafely();
				NHoverTipSet._activeHoverTips.Remove(owner);
			}
		}

		/// <summary>
		/// Adds a static offset to a HoverTip when it's following a node.
		/// Used when the default offset doesn't have enough padding.
		/// See NLabPotionHolder for example.
		/// </summary>
		// Token: 0x0600374C RID: 14156 RVA: 0x0019FE9E File Offset: 0x0019E09E
		public void SetExtraFollowOffset(Vector2 offset)
		{
			this._extraOffset = offset;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600374D RID: 14157 RVA: 0x0019FEA8 File Offset: 0x0019E0A8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(12)
			{
				new MethodInfo(NHoverTipSet.MethodName.SetFollowOwner, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHoverTipSet.MethodName.CreateAndShowMapPointHistory, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "owner", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(24L, "historyHoverTip", 0L, "", 6L, new StringName("MarginContainer"), false)
				}, null),
				new MethodInfo(NHoverTipSet.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHoverTipSet.MethodName._Process, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(3L, "delta", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHoverTipSet.MethodName.SetAlignment, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Control"), false),
					new PropertyInfo(2L, "alignment", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NHoverTipSet.MethodName.SetAlignmentForRelic, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "relic", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NHoverTipSet.MethodName.SetAlignmentForCardHolder, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "holder", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NHoverTipSet.MethodName.CorrectVerticalOverflow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHoverTipSet.MethodName.CorrectHorizontalOverflow, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NHoverTipSet.MethodName.Clear, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(NHoverTipSet.MethodName.Remove, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "owner", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NHoverTipSet.MethodName.SetExtraFollowOffset, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "offset", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600374E RID: 14158 RVA: 0x001A0288 File Offset: 0x0019E488
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHoverTipSet.MethodName.SetFollowOwner && args.Count == 0)
			{
				this.SetFollowOwner();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName.CreateAndShowMapPointHistory && args.Count == 2)
			{
				NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShowMapPointHistory(VariantUtils.ConvertTo<Control>(args[0]), VariantUtils.ConvertTo<NMapPointHistoryHoverTip>(args[1]));
				ret = VariantUtils.CreateFrom<NHoverTipSet>(ref nhoverTipSet);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName._Process && args.Count == 1)
			{
				this._Process(VariantUtils.ConvertTo<double>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName.SetAlignment && args.Count == 2)
			{
				this.SetAlignment(VariantUtils.ConvertTo<Control>(args[0]), VariantUtils.ConvertTo<HoverTipAlignment>(args[1]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName.SetAlignmentForRelic && args.Count == 1)
			{
				this.SetAlignmentForRelic(VariantUtils.ConvertTo<NRelic>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName.SetAlignmentForCardHolder && args.Count == 1)
			{
				this.SetAlignmentForCardHolder(VariantUtils.ConvertTo<NCardHolder>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName.CorrectVerticalOverflow && args.Count == 0)
			{
				this.CorrectVerticalOverflow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName.CorrectHorizontalOverflow && args.Count == 0)
			{
				this.CorrectHorizontalOverflow();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName.Clear && args.Count == 0)
			{
				NHoverTipSet.Clear();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName.Remove && args.Count == 1)
			{
				NHoverTipSet.Remove(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName.SetExtraFollowOffset && args.Count == 1)
			{
				this.SetExtraFollowOffset(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x001A04DC File Offset: 0x0019E6DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NHoverTipSet.MethodName.CreateAndShowMapPointHistory && args.Count == 2)
			{
				NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShowMapPointHistory(VariantUtils.ConvertTo<Control>(args[0]), VariantUtils.ConvertTo<NMapPointHistoryHoverTip>(args[1]));
				ret = VariantUtils.CreateFrom<NHoverTipSet>(ref nhoverTipSet);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName.Clear && args.Count == 0)
			{
				NHoverTipSet.Clear();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NHoverTipSet.MethodName.Remove && args.Count == 1)
			{
				NHoverTipSet.Remove(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06003750 RID: 14160 RVA: 0x001A0590 File Offset: 0x0019E790
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NHoverTipSet.MethodName.SetFollowOwner || (ref method) == NHoverTipSet.MethodName.CreateAndShowMapPointHistory || (ref method) == NHoverTipSet.MethodName._Ready || (ref method) == NHoverTipSet.MethodName._Process || (ref method) == NHoverTipSet.MethodName.SetAlignment || (ref method) == NHoverTipSet.MethodName.SetAlignmentForRelic || (ref method) == NHoverTipSet.MethodName.SetAlignmentForCardHolder || (ref method) == NHoverTipSet.MethodName.CorrectVerticalOverflow || (ref method) == NHoverTipSet.MethodName.CorrectHorizontalOverflow || (ref method) == NHoverTipSet.MethodName.Clear || (ref method) == NHoverTipSet.MethodName.Remove || (ref method) == NHoverTipSet.MethodName.SetExtraFollowOffset || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003751 RID: 14161 RVA: 0x001A0658 File Offset: 0x0019E858
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NHoverTipSet.PropertyName._textHoverTipContainer)
			{
				this._textHoverTipContainer = VariantUtils.ConvertTo<VFlowContainer>(ref value);
				return true;
			}
			if ((ref name) == NHoverTipSet.PropertyName._cardHoverTipContainer)
			{
				this._cardHoverTipContainer = VariantUtils.ConvertTo<NHoverTipCardContainer>(ref value);
				return true;
			}
			if ((ref name) == NHoverTipSet.PropertyName._owner)
			{
				this._owner = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NHoverTipSet.PropertyName._followOwner)
			{
				this._followOwner = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NHoverTipSet.PropertyName._followOffset)
			{
				this._followOffset = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NHoverTipSet.PropertyName._extraOffset)
			{
				this._extraOffset = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003752 RID: 14162 RVA: 0x001A0710 File Offset: 0x0019E910
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NHoverTipSet.PropertyName.TextHoverTipDimensions)
			{
				Vector2 vector = this.TextHoverTipDimensions;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NHoverTipSet.PropertyName.CardHoverTipDimensions)
			{
				Vector2 vector = this.CardHoverTipDimensions;
				value = VariantUtils.CreateFrom<Vector2>(ref vector);
				return true;
			}
			if ((ref name) == NHoverTipSet.PropertyName._textHoverTipContainer)
			{
				value = VariantUtils.CreateFrom<VFlowContainer>(ref this._textHoverTipContainer);
				return true;
			}
			if ((ref name) == NHoverTipSet.PropertyName._cardHoverTipContainer)
			{
				value = VariantUtils.CreateFrom<NHoverTipCardContainer>(ref this._cardHoverTipContainer);
				return true;
			}
			if ((ref name) == NHoverTipSet.PropertyName._owner)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._owner);
				return true;
			}
			if ((ref name) == NHoverTipSet.PropertyName._followOwner)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._followOwner);
				return true;
			}
			if ((ref name) == NHoverTipSet.PropertyName._followOffset)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._followOffset);
				return true;
			}
			if ((ref name) == NHoverTipSet.PropertyName._extraOffset)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._extraOffset);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003753 RID: 14163 RVA: 0x001A082C File Offset: 0x0019EA2C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NHoverTipSet.PropertyName._textHoverTipContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHoverTipSet.PropertyName._cardHoverTipContainer, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHoverTipSet.PropertyName.TextHoverTipDimensions, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHoverTipSet.PropertyName.CardHoverTipDimensions, 0L, "", 4096L, false),
				new PropertyInfo(24L, NHoverTipSet.PropertyName._owner, 0L, "", 4096L, false),
				new PropertyInfo(1L, NHoverTipSet.PropertyName._followOwner, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHoverTipSet.PropertyName._followOffset, 0L, "", 4096L, false),
				new PropertyInfo(5L, NHoverTipSet.PropertyName._extraOffset, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003754 RID: 14164 RVA: 0x001A0944 File Offset: 0x0019EB44
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NHoverTipSet.PropertyName._textHoverTipContainer, Variant.From<VFlowContainer>(ref this._textHoverTipContainer));
			info.AddProperty(NHoverTipSet.PropertyName._cardHoverTipContainer, Variant.From<NHoverTipCardContainer>(ref this._cardHoverTipContainer));
			info.AddProperty(NHoverTipSet.PropertyName._owner, Variant.From<Control>(ref this._owner));
			info.AddProperty(NHoverTipSet.PropertyName._followOwner, Variant.From<bool>(ref this._followOwner));
			info.AddProperty(NHoverTipSet.PropertyName._followOffset, Variant.From<Vector2>(ref this._followOffset));
			info.AddProperty(NHoverTipSet.PropertyName._extraOffset, Variant.From<Vector2>(ref this._extraOffset));
		}

		/// <inheritdoc />
		// Token: 0x06003755 RID: 14165 RVA: 0x001A09DC File Offset: 0x0019EBDC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NHoverTipSet.PropertyName._textHoverTipContainer, ref variant))
			{
				this._textHoverTipContainer = variant.As<VFlowContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NHoverTipSet.PropertyName._cardHoverTipContainer, ref variant2))
			{
				this._cardHoverTipContainer = variant2.As<NHoverTipCardContainer>();
			}
			Variant variant3;
			if (info.TryGetProperty(NHoverTipSet.PropertyName._owner, ref variant3))
			{
				this._owner = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NHoverTipSet.PropertyName._followOwner, ref variant4))
			{
				this._followOwner = variant4.As<bool>();
			}
			Variant variant5;
			if (info.TryGetProperty(NHoverTipSet.PropertyName._followOffset, ref variant5))
			{
				this._followOffset = variant5.As<Vector2>();
			}
			Variant variant6;
			if (info.TryGetProperty(NHoverTipSet.PropertyName._extraOffset, ref variant6))
			{
				this._extraOffset = variant6.As<Vector2>();
			}
		}

		/// <summary>
		/// Blocks hover tips from displaying. Useful for situations such as while you are using card/potion targeting
		/// WARNING: Be very careful when modifying this!!! Make sure that if you set it to true, that you set it back to false!
		/// </summary>
		// Token: 0x04001568 RID: 5480
		public static bool shouldBlockHoverTips = false;

		// Token: 0x04001569 RID: 5481
		private static readonly StringName _cardHoverTipContainerStr = new StringName("cardHoverTipContainer");

		// Token: 0x0400156A RID: 5482
		private static readonly StringName _textHoverTipContainerStr = new StringName("textHoverTipContainer");

		// Token: 0x0400156B RID: 5483
		private const float _hoverTipSpacing = 5f;

		// Token: 0x0400156C RID: 5484
		private const float _hoverTipWidth = 360f;

		// Token: 0x0400156D RID: 5485
		private const string _tipScenePath = "res://scenes/ui/hover_tip.tscn";

		// Token: 0x0400156E RID: 5486
		private const string _tipSetScenePath = "res://scenes/ui/hover_tip_set.tscn";

		// Token: 0x0400156F RID: 5487
		private const string _debuffMatPath = "res://materials/ui/hover_tip_debuff.tres";

		// Token: 0x04001570 RID: 5488
		private static readonly Dictionary<Control, NHoverTipSet> _activeHoverTips = new Dictionary<Control, NHoverTipSet>();

		// Token: 0x04001571 RID: 5489
		private VFlowContainer _textHoverTipContainer;

		// Token: 0x04001572 RID: 5490
		private NHoverTipCardContainer _cardHoverTipContainer;

		// Token: 0x04001573 RID: 5491
		private Control _owner;

		// Token: 0x04001574 RID: 5492
		private bool _followOwner;

		// Token: 0x04001575 RID: 5493
		private Vector2 _followOffset;

		// Token: 0x04001576 RID: 5494
		private Vector2 _extraOffset = Vector2.Zero;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001759 RID: 5977
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the 'SetFollowOwner' method.
			/// </summary>
			// Token: 0x04005B0F RID: 23311
			public static readonly StringName SetFollowOwner = "SetFollowOwner";

			/// <summary>
			/// Cached name for the 'CreateAndShowMapPointHistory' method.
			/// </summary>
			// Token: 0x04005B10 RID: 23312
			public static readonly StringName CreateAndShowMapPointHistory = "CreateAndShowMapPointHistory";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005B11 RID: 23313
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_Process' method.
			/// </summary>
			// Token: 0x04005B12 RID: 23314
			public static readonly StringName _Process = "_Process";

			/// <summary>
			/// Cached name for the 'SetAlignment' method.
			/// </summary>
			// Token: 0x04005B13 RID: 23315
			public static readonly StringName SetAlignment = "SetAlignment";

			/// <summary>
			/// Cached name for the 'SetAlignmentForRelic' method.
			/// </summary>
			// Token: 0x04005B14 RID: 23316
			public static readonly StringName SetAlignmentForRelic = "SetAlignmentForRelic";

			/// <summary>
			/// Cached name for the 'SetAlignmentForCardHolder' method.
			/// </summary>
			// Token: 0x04005B15 RID: 23317
			public static readonly StringName SetAlignmentForCardHolder = "SetAlignmentForCardHolder";

			/// <summary>
			/// Cached name for the 'CorrectVerticalOverflow' method.
			/// </summary>
			// Token: 0x04005B16 RID: 23318
			public static readonly StringName CorrectVerticalOverflow = "CorrectVerticalOverflow";

			/// <summary>
			/// Cached name for the 'CorrectHorizontalOverflow' method.
			/// </summary>
			// Token: 0x04005B17 RID: 23319
			public static readonly StringName CorrectHorizontalOverflow = "CorrectHorizontalOverflow";

			/// <summary>
			/// Cached name for the 'Clear' method.
			/// </summary>
			// Token: 0x04005B18 RID: 23320
			public static readonly StringName Clear = "Clear";

			/// <summary>
			/// Cached name for the 'Remove' method.
			/// </summary>
			// Token: 0x04005B19 RID: 23321
			public static readonly StringName Remove = "Remove";

			/// <summary>
			/// Cached name for the 'SetExtraFollowOffset' method.
			/// </summary>
			// Token: 0x04005B1A RID: 23322
			public static readonly StringName SetExtraFollowOffset = "SetExtraFollowOffset";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200175A RID: 5978
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'TextHoverTipDimensions' property.
			/// </summary>
			// Token: 0x04005B1B RID: 23323
			public static readonly StringName TextHoverTipDimensions = "TextHoverTipDimensions";

			/// <summary>
			/// Cached name for the 'CardHoverTipDimensions' property.
			/// </summary>
			// Token: 0x04005B1C RID: 23324
			public static readonly StringName CardHoverTipDimensions = "CardHoverTipDimensions";

			/// <summary>
			/// Cached name for the '_textHoverTipContainer' field.
			/// </summary>
			// Token: 0x04005B1D RID: 23325
			public static readonly StringName _textHoverTipContainer = "_textHoverTipContainer";

			/// <summary>
			/// Cached name for the '_cardHoverTipContainer' field.
			/// </summary>
			// Token: 0x04005B1E RID: 23326
			public static readonly StringName _cardHoverTipContainer = "_cardHoverTipContainer";

			/// <summary>
			/// Cached name for the '_owner' field.
			/// </summary>
			// Token: 0x04005B1F RID: 23327
			public static readonly StringName _owner = "_owner";

			/// <summary>
			/// Cached name for the '_followOwner' field.
			/// </summary>
			// Token: 0x04005B20 RID: 23328
			public static readonly StringName _followOwner = "_followOwner";

			/// <summary>
			/// Cached name for the '_followOffset' field.
			/// </summary>
			// Token: 0x04005B21 RID: 23329
			public static readonly StringName _followOffset = "_followOffset";

			/// <summary>
			/// Cached name for the '_extraOffset' field.
			/// </summary>
			// Token: 0x04005B22 RID: 23330
			public static readonly StringName _extraOffset = "_extraOffset";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200175B RID: 5979
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}

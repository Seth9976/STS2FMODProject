using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Combat
{
	/// <summary>
	/// An abstract base class in charge of managing the logic for playing a card from your hand.
	/// Inherited classes manage card play behavior for different types of inputs.
	/// Is created and destroyed on demand.
	/// </summary>
	// Token: 0x020003F1 RID: 1009
	[ScriptPath("res://src/Core/Nodes/Combat/NCardPlay.cs")]
	public abstract class NCardPlay : Node
	{
		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06003DD6 RID: 15830 RVA: 0x001D9D01 File Offset: 0x001D7F01
		// (set) Token: 0x06003DD7 RID: 15831 RVA: 0x001D9D09 File Offset: 0x001D7F09
		[Nullable(1)]
		public NHandCardHolder Holder
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			protected set;
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06003DD8 RID: 15832 RVA: 0x001D9D12 File Offset: 0x001D7F12
		[Nullable(2)]
		protected NCard CardNode
		{
			[NullableContext(2)]
			get
			{
				return this.Holder.CardNode;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x001D9D1F File Offset: 0x001D7F1F
		[Nullable(2)]
		protected CardModel Card
		{
			[NullableContext(2)]
			get
			{
				NCard cardNode = this.CardNode;
				if (cardNode == null)
				{
					return null;
				}
				return cardNode.Model;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06003DDA RID: 15834 RVA: 0x001D9D32 File Offset: 0x001D7F32
		[Nullable(2)]
		protected NCreature CardOwnerNode
		{
			[NullableContext(2)]
			get
			{
				NCombatRoom instance = NCombatRoom.Instance;
				CardModel card = this.Card;
				return instance.GetCreatureNode((card != null) ? card.Owner.Creature : null);
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06003DDB RID: 15835 RVA: 0x001D9D55 File Offset: 0x001D7F55
		// (set) Token: 0x06003DDC RID: 15836 RVA: 0x001D9D5D File Offset: 0x001D7F5D
		[Nullable(1)]
		public Player Player
		{
			[NullableContext(1)]
			get;
			[NullableContext(1)]
			protected set;
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x001D9D66 File Offset: 0x001D7F66
		public override void _Ready()
		{
			this._viewport = base.GetViewport();
		}

		// Token: 0x06003DDE RID: 15838
		public abstract void Start();

		// Token: 0x06003DDF RID: 15839 RVA: 0x001D9D74 File Offset: 0x001D7F74
		[NullableContext(2)]
		protected void TryPlayCard(Creature target)
		{
			if (this.Card == null)
			{
				return;
			}
			if (this.Card.TargetType == TargetType.AnyEnemy && target == null)
			{
				this.CancelPlayCard();
				return;
			}
			if (this.Card.TargetType == TargetType.AnyAlly && target == null)
			{
				this.CancelPlayCard();
				return;
			}
			if (!this.Holder.CardModel.CanPlayTargeting(target))
			{
				this.CannotPlayThisCardFtueCheck(this.Holder.CardModel);
				this.CancelPlayCard();
				return;
			}
			CardModel card = this.Card;
			this._isTryingToPlayCard = true;
			TargetType targetType = card.TargetType;
			bool flag = targetType == TargetType.AnyEnemy || targetType == TargetType.AnyAlly;
			bool flag2;
			if (flag)
			{
				if (NCombatRoom.Instance != null)
				{
					NCombatRoom.Instance.LastTargetedCreature = target;
				}
				flag2 = card.TryManualPlay(target);
			}
			else
			{
				bool flag3 = NCombatRoom.Instance != null;
				bool flag4 = flag3;
				if (flag4)
				{
					targetType = card.TargetType;
					flag = targetType == TargetType.AllEnemies || targetType == TargetType.AllAllies;
					flag4 = flag;
				}
				if (flag4)
				{
					NCombatRoom.Instance.LastTargetedCreature = null;
				}
				flag2 = card.TryManualPlay(null);
			}
			this._isTryingToPlayCard = false;
			if (flag2)
			{
				this.AutoDisableCannotPlayCardFtueCheck();
				targetType = card.TargetType;
				flag = targetType == TargetType.AnyEnemy || targetType == TargetType.AnyAlly;
				if (flag && this.Holder.IsInsideTree())
				{
					Vector2 size = base.GetViewport().GetVisibleRect().Size;
					this.Holder.SetTargetPosition(new Vector2(size.X / 2f, size.Y - this.Holder.Size.Y));
				}
				this.Cleanup(true);
				ActiveScreenContext.Instance.FocusOnDefaultControl();
				return;
			}
			this.CancelPlayCard();
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x001D9F03 File Offset: 0x001D8103
		public void CancelPlayCard()
		{
			if (this._isTryingToPlayCard)
			{
				return;
			}
			this.ClearTarget();
			this.Cleanup(false);
			this.OnCancelPlayCard();
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x001D9F21 File Offset: 0x001D8121
		protected virtual void OnCancelPlayCard()
		{
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x001D9F23 File Offset: 0x001D8123
		protected void Cleanup(bool isFinished)
		{
			this.HideTargetingVisuals();
			this.HideEvokingOrbs();
			if (GodotObject.IsInstanceValid(this))
			{
				base.EmitSignal(NCardPlay.SignalName.Finished, new Variant[] { isFinished });
				this.QueueFreeSafely();
			}
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x001D9F5E File Offset: 0x001D815E
		[NullableContext(1)]
		protected void OnCreatureHover(NCreature creature)
		{
			NCard cardNode = this.CardNode;
			if (cardNode == null)
			{
				return;
			}
			cardNode.SetPreviewTarget(creature.Entity);
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x001D9F76 File Offset: 0x001D8176
		[NullableContext(1)]
		protected void OnCreatureUnhover(NCreature _)
		{
			this.ClearTarget();
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x001D9F80 File Offset: 0x001D8180
		protected void CenterCard()
		{
			Vector2 size = this._viewport.GetVisibleRect().Size;
			this.Holder.SetTargetPosition(new Vector2(size.X / 2f, size.Y - this.Holder.Hitbox.Size.Y * 0.75f / 2f));
			this.Holder.SetTargetScale(Vector2.One * 0.75f);
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x001DA000 File Offset: 0x001D8200
		[NullableContext(1)]
		protected void CannotPlayThisCardFtueCheck(CardModel card)
		{
			UnplayableReason unplayableReason;
			AbstractModel abstractModel;
			if (!SaveManager.Instance.SeenFtue("cannot_play_card_ftue") && !card.CanPlay(out unplayableReason, out abstractModel) && unplayableReason == UnplayableReason.EnergyCostTooHigh)
			{
				NModalContainer.Instance.Add(NCannotPlayCardFtue.Create(), true);
				SaveManager.Instance.MarkFtueAsComplete("cannot_play_card_ftue");
			}
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x001DA050 File Offset: 0x001D8250
		protected void HideTargetingVisuals()
		{
			foreach (NCreature ncreature in NCombatRoom.Instance.CreatureNodes)
			{
				ncreature.HideMultiselectReticle();
			}
			NCard cardNode = this.CardNode;
			if (cardNode != null)
			{
				cardNode.SetPreviewTarget(null);
			}
			NCard cardNode2 = this.CardNode;
			if (cardNode2 == null)
			{
				return;
			}
			CardModel card = this.Card;
			PileType? pileType;
			if (card == null)
			{
				pileType = null;
			}
			else
			{
				CardPile pile = card.Pile;
				pileType = ((pile != null) ? new PileType?(pile.Type) : null);
			}
			PileType? pileType2 = pileType;
			cardNode2.UpdateVisuals(pileType2.GetValueOrDefault(), CardPreviewMode.Normal);
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x001DA100 File Offset: 0x001D8300
		protected void ShowMultiCreatureTargetingVisuals()
		{
			if (this.CardNode != null)
			{
				CardModel card = this.Card;
				if (((card != null) ? card.CombatState : null) != null)
				{
					TargetType targetType = this.Card.TargetType;
					bool flag = targetType - TargetType.AllEnemies <= 1;
					if (flag)
					{
						IReadOnlyList<Creature> hittableEnemies = this.Card.CombatState.HittableEnemies;
						if (hittableEnemies.Count == 1)
						{
							this.CardNode.SetPreviewTarget(hittableEnemies[0]);
						}
						NCard cardNode = this.CardNode;
						CardModel model = this.CardNode.Model;
						PileType? pileType;
						if (model == null)
						{
							pileType = null;
						}
						else
						{
							CardPile pile = model.Pile;
							pileType = ((pile != null) ? new PileType?(pile.Type) : null);
						}
						PileType? pileType2 = pileType;
						cardNode.UpdateVisuals(pileType2.GetValueOrDefault(), CardPreviewMode.MultiCreatureTargeting);
						foreach (Creature creature in hittableEnemies)
						{
							NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(creature);
							if (creatureNode != null)
							{
								creatureNode.ShowMultiselectReticle();
							}
						}
					}
					if (this.Card.TargetType == TargetType.AllAllies)
					{
						IEnumerable<Creature> enumerable = this.Card.CombatState.PlayerCreatures.Where<Creature>((Creature c) => c.IsAlive);
						using (IEnumerator<Creature> enumerator2 = enumerable.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Creature creature2 = enumerator2.Current;
								NCreature creatureNode2 = NCombatRoom.Instance.GetCreatureNode(creature2);
								if (creatureNode2 != null)
								{
									creatureNode2.ShowMultiselectReticle();
								}
							}
							return;
						}
					}
					if (this.Card.TargetType == TargetType.Self)
					{
						NCreature creatureNode3 = NCombatRoom.Instance.GetCreatureNode(this.Card.Owner.Creature);
						if (creatureNode3 == null)
						{
							return;
						}
						creatureNode3.ShowMultiselectReticle();
						return;
					}
					else if (this.Card.TargetType == TargetType.Osty)
					{
						NCreature creatureNode4 = NCombatRoom.Instance.GetCreatureNode(this.Card.Owner.Osty);
						if (creatureNode4 == null)
						{
							return;
						}
						creatureNode4.ShowMultiselectReticle();
					}
					return;
				}
			}
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x001DA30C File Offset: 0x001D850C
		private void AutoDisableCannotPlayCardFtueCheck()
		{
			NCardPlay._totalCardsPlayedForFtue++;
			if (NCardPlay._totalCardsPlayedForFtue == 8 && !SaveManager.Instance.SeenFtue("cannot_play_card_ftue"))
			{
				Log.Info("Cannot play cards FTUE was disabled, the player never saw it!!", 2);
				SaveManager.Instance.MarkFtueAsComplete("cannot_play_card_ftue");
			}
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x001DA358 File Offset: 0x001D8558
		protected void TryShowEvokingOrbs()
		{
			if (this.Card == null)
			{
				return;
			}
			NCreature cardOwnerNode = this.CardOwnerNode;
			if (cardOwnerNode == null)
			{
				return;
			}
			NOrbManager orbManager = cardOwnerNode.OrbManager;
			if (orbManager == null)
			{
				return;
			}
			orbManager.UpdateVisuals(this.Card.OrbEvokeType);
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x001DA388 File Offset: 0x001D8588
		private void HideEvokingOrbs()
		{
			NCreature cardOwnerNode = this.CardOwnerNode;
			if (cardOwnerNode == null)
			{
				return;
			}
			NOrbManager orbManager = cardOwnerNode.OrbManager;
			if (orbManager == null)
			{
				return;
			}
			orbManager.UpdateVisuals(OrbEvokeType.None);
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x001DA3A5 File Offset: 0x001D85A5
		private void ClearTarget()
		{
			NCard cardNode = this.CardNode;
			if (cardNode == null)
			{
				return;
			}
			cardNode.SetPreviewTarget(null);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003DED RID: 15853 RVA: 0x001DA3B8 File Offset: 0x001D85B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NCardPlay.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlay.MethodName.Start, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlay.MethodName.CancelPlayCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlay.MethodName.OnCancelPlayCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlay.MethodName.Cleanup, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "isFinished", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NCardPlay.MethodName.OnCreatureHover, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "creature", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardPlay.MethodName.OnCreatureUnhover, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCardPlay.MethodName.CenterCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlay.MethodName.HideTargetingVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlay.MethodName.ShowMultiCreatureTargetingVisuals, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlay.MethodName.AutoDisableCannotPlayCardFtueCheck, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlay.MethodName.TryShowEvokingOrbs, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlay.MethodName.HideEvokingOrbs, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardPlay.MethodName.ClearTarget, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003DEE RID: 15854 RVA: 0x001DA6E8 File Offset: 0x001D88E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardPlay.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.Start && args.Count == 0)
			{
				this.Start();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.CancelPlayCard && args.Count == 0)
			{
				this.CancelPlayCard();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.OnCancelPlayCard && args.Count == 0)
			{
				this.OnCancelPlayCard();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.Cleanup && args.Count == 1)
			{
				this.Cleanup(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.OnCreatureHover && args.Count == 1)
			{
				this.OnCreatureHover(VariantUtils.ConvertTo<NCreature>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.OnCreatureUnhover && args.Count == 1)
			{
				this.OnCreatureUnhover(VariantUtils.ConvertTo<NCreature>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.CenterCard && args.Count == 0)
			{
				this.CenterCard();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.HideTargetingVisuals && args.Count == 0)
			{
				this.HideTargetingVisuals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.ShowMultiCreatureTargetingVisuals && args.Count == 0)
			{
				this.ShowMultiCreatureTargetingVisuals();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.AutoDisableCannotPlayCardFtueCheck && args.Count == 0)
			{
				this.AutoDisableCannotPlayCardFtueCheck();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.TryShowEvokingOrbs && args.Count == 0)
			{
				this.TryShowEvokingOrbs();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.HideEvokingOrbs && args.Count == 0)
			{
				this.HideEvokingOrbs();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardPlay.MethodName.ClearTarget && args.Count == 0)
			{
				this.ClearTarget();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003DEF RID: 15855 RVA: 0x001DA930 File Offset: 0x001D8B30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardPlay.MethodName._Ready || (ref method) == NCardPlay.MethodName.Start || (ref method) == NCardPlay.MethodName.CancelPlayCard || (ref method) == NCardPlay.MethodName.OnCancelPlayCard || (ref method) == NCardPlay.MethodName.Cleanup || (ref method) == NCardPlay.MethodName.OnCreatureHover || (ref method) == NCardPlay.MethodName.OnCreatureUnhover || (ref method) == NCardPlay.MethodName.CenterCard || (ref method) == NCardPlay.MethodName.HideTargetingVisuals || (ref method) == NCardPlay.MethodName.ShowMultiCreatureTargetingVisuals || (ref method) == NCardPlay.MethodName.AutoDisableCannotPlayCardFtueCheck || (ref method) == NCardPlay.MethodName.TryShowEvokingOrbs || (ref method) == NCardPlay.MethodName.HideEvokingOrbs || (ref method) == NCardPlay.MethodName.ClearTarget || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003DF0 RID: 15856 RVA: 0x001DAA18 File Offset: 0x001D8C18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardPlay.PropertyName.Holder)
			{
				this.Holder = VariantUtils.ConvertTo<NHandCardHolder>(ref value);
				return true;
			}
			if ((ref name) == NCardPlay.PropertyName._viewport)
			{
				this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
				return true;
			}
			if ((ref name) == NCardPlay.PropertyName._isTryingToPlayCard)
			{
				this._isTryingToPlayCard = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003DF1 RID: 15857 RVA: 0x001DAA80 File Offset: 0x001D8C80
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardPlay.PropertyName.Holder)
			{
				NHandCardHolder holder = this.Holder;
				value = VariantUtils.CreateFrom<NHandCardHolder>(ref holder);
				return true;
			}
			if ((ref name) == NCardPlay.PropertyName.CardNode)
			{
				NCard cardNode = this.CardNode;
				value = VariantUtils.CreateFrom<NCard>(ref cardNode);
				return true;
			}
			if ((ref name) == NCardPlay.PropertyName.CardOwnerNode)
			{
				NCreature cardOwnerNode = this.CardOwnerNode;
				value = VariantUtils.CreateFrom<NCreature>(ref cardOwnerNode);
				return true;
			}
			if ((ref name) == NCardPlay.PropertyName._viewport)
			{
				value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
				return true;
			}
			if ((ref name) == NCardPlay.PropertyName._isTryingToPlayCard)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isTryingToPlayCard);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003DF2 RID: 15858 RVA: 0x001DAB40 File Offset: 0x001D8D40
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardPlay.PropertyName.Holder, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardPlay.PropertyName.CardNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardPlay.PropertyName.CardOwnerNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardPlay.PropertyName._viewport, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCardPlay.PropertyName._isTryingToPlayCard, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003DF3 RID: 15859 RVA: 0x001DABF8 File Offset: 0x001D8DF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName holder = NCardPlay.PropertyName.Holder;
			NHandCardHolder holder2 = this.Holder;
			info.AddProperty(holder, Variant.From<NHandCardHolder>(ref holder2));
			info.AddProperty(NCardPlay.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
			info.AddProperty(NCardPlay.PropertyName._isTryingToPlayCard, Variant.From<bool>(ref this._isTryingToPlayCard));
			info.AddSignalEventDelegate(NCardPlay.SignalName.Finished, this.backing_Finished);
		}

		/// <inheritdoc />
		// Token: 0x06003DF4 RID: 15860 RVA: 0x001DAC64 File Offset: 0x001D8E64
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardPlay.PropertyName.Holder, ref variant))
			{
				this.Holder = variant.As<NHandCardHolder>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardPlay.PropertyName._viewport, ref variant2))
			{
				this._viewport = variant2.As<Viewport>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardPlay.PropertyName._isTryingToPlayCard, ref variant3))
			{
				this._isTryingToPlayCard = variant3.As<bool>();
			}
			NCardPlay.FinishedEventHandler finishedEventHandler;
			if (info.TryGetSignalEventDelegate<NCardPlay.FinishedEventHandler>(NCardPlay.SignalName.Finished, ref finishedEventHandler))
			{
				this.backing_Finished = finishedEventHandler;
			}
		}

		/// <summary>
		/// Get the signal information for all the signals declared in this class.
		/// This method is used by Godot to register the available signals in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003DF5 RID: 15861 RVA: 0x001DACE4 File Offset: 0x001D8EE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotSignalList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(NCardPlay.SignalName.Finished, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "success", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc cref="T:MegaCrit.Sts2.Core.Nodes.Combat.NCardPlay.FinishedEventHandler" />
		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06003DF6 RID: 15862 RVA: 0x001DAD4D File Offset: 0x001D8F4D
		// (remove) Token: 0x06003DF7 RID: 15863 RVA: 0x001DAD66 File Offset: 0x001D8F66
		public event NCardPlay.FinishedEventHandler Finished
		{
			add
			{
				this.backing_Finished = (NCardPlay.FinishedEventHandler)Delegate.Combine(this.backing_Finished, value);
			}
			remove
			{
				this.backing_Finished = (NCardPlay.FinishedEventHandler)Delegate.Remove(this.backing_Finished, value);
			}
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x001DAD7F File Offset: 0x001D8F7F
		protected void EmitSignalFinished(bool success)
		{
			base.EmitSignal(NCardPlay.SignalName.Finished, new Variant[] { success });
		}

		/// <inheritdoc />
		// Token: 0x06003DF9 RID: 15865 RVA: 0x001DADA0 File Offset: 0x001D8FA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
		{
			if (!((ref signal) == NCardPlay.SignalName.Finished) || args.Count != 1)
			{
				base.RaiseGodotClassSignalCallbacks(ref signal, args);
				return;
			}
			NCardPlay.FinishedEventHandler finishedEventHandler = this.backing_Finished;
			if (finishedEventHandler == null)
			{
				return;
			}
			finishedEventHandler(VariantUtils.ConvertTo<bool>(args[0]));
		}

		/// <inheritdoc />
		// Token: 0x06003DFA RID: 15866 RVA: 0x001DADDF File Offset: 0x001D8FDF
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassSignal(in godot_string_name signal)
		{
			return (ref signal) == NCardPlay.SignalName.Finished || base.HasGodotClassSignal(ref signal);
		}

		// Token: 0x0400181B RID: 6171
		private static int _totalCardsPlayedForFtue;

		// Token: 0x0400181C RID: 6172
		private const int _numCardPlayedUntilDisableFtue = 8;

		// Token: 0x0400181F RID: 6175
		[Nullable(1)]
		protected Viewport _viewport;

		// Token: 0x04001820 RID: 6176
		private bool _isTryingToPlayCard;

		// Token: 0x04001821 RID: 6177
		private NCardPlay.FinishedEventHandler backing_Finished;

		// Token: 0x02001898 RID: 6296
		// (Invoke) Token: 0x0600A908 RID: 43272
		[Signal]
		public delegate void FinishedEventHandler(bool success);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001899 RID: 6297
		public class MethodName : Node.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x0400602F RID: 24623
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'Start' method.
			/// </summary>
			// Token: 0x04006030 RID: 24624
			public static readonly StringName Start = "Start";

			/// <summary>
			/// Cached name for the 'CancelPlayCard' method.
			/// </summary>
			// Token: 0x04006031 RID: 24625
			public static readonly StringName CancelPlayCard = "CancelPlayCard";

			/// <summary>
			/// Cached name for the 'OnCancelPlayCard' method.
			/// </summary>
			// Token: 0x04006032 RID: 24626
			public static readonly StringName OnCancelPlayCard = "OnCancelPlayCard";

			/// <summary>
			/// Cached name for the 'Cleanup' method.
			/// </summary>
			// Token: 0x04006033 RID: 24627
			public static readonly StringName Cleanup = "Cleanup";

			/// <summary>
			/// Cached name for the 'OnCreatureHover' method.
			/// </summary>
			// Token: 0x04006034 RID: 24628
			public static readonly StringName OnCreatureHover = "OnCreatureHover";

			/// <summary>
			/// Cached name for the 'OnCreatureUnhover' method.
			/// </summary>
			// Token: 0x04006035 RID: 24629
			public static readonly StringName OnCreatureUnhover = "OnCreatureUnhover";

			/// <summary>
			/// Cached name for the 'CenterCard' method.
			/// </summary>
			// Token: 0x04006036 RID: 24630
			public static readonly StringName CenterCard = "CenterCard";

			/// <summary>
			/// Cached name for the 'HideTargetingVisuals' method.
			/// </summary>
			// Token: 0x04006037 RID: 24631
			public static readonly StringName HideTargetingVisuals = "HideTargetingVisuals";

			/// <summary>
			/// Cached name for the 'ShowMultiCreatureTargetingVisuals' method.
			/// </summary>
			// Token: 0x04006038 RID: 24632
			public static readonly StringName ShowMultiCreatureTargetingVisuals = "ShowMultiCreatureTargetingVisuals";

			/// <summary>
			/// Cached name for the 'AutoDisableCannotPlayCardFtueCheck' method.
			/// </summary>
			// Token: 0x04006039 RID: 24633
			public static readonly StringName AutoDisableCannotPlayCardFtueCheck = "AutoDisableCannotPlayCardFtueCheck";

			/// <summary>
			/// Cached name for the 'TryShowEvokingOrbs' method.
			/// </summary>
			// Token: 0x0400603A RID: 24634
			public static readonly StringName TryShowEvokingOrbs = "TryShowEvokingOrbs";

			/// <summary>
			/// Cached name for the 'HideEvokingOrbs' method.
			/// </summary>
			// Token: 0x0400603B RID: 24635
			public static readonly StringName HideEvokingOrbs = "HideEvokingOrbs";

			/// <summary>
			/// Cached name for the 'ClearTarget' method.
			/// </summary>
			// Token: 0x0400603C RID: 24636
			public static readonly StringName ClearTarget = "ClearTarget";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200189A RID: 6298
		public class PropertyName : Node.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Holder' property.
			/// </summary>
			// Token: 0x0400603D RID: 24637
			public static readonly StringName Holder = "Holder";

			/// <summary>
			/// Cached name for the 'CardNode' property.
			/// </summary>
			// Token: 0x0400603E RID: 24638
			public static readonly StringName CardNode = "CardNode";

			/// <summary>
			/// Cached name for the 'CardOwnerNode' property.
			/// </summary>
			// Token: 0x0400603F RID: 24639
			public static readonly StringName CardOwnerNode = "CardOwnerNode";

			/// <summary>
			/// Cached name for the '_viewport' field.
			/// </summary>
			// Token: 0x04006040 RID: 24640
			public static readonly StringName _viewport = "_viewport";

			/// <summary>
			/// Cached name for the '_isTryingToPlayCard' field.
			/// </summary>
			// Token: 0x04006041 RID: 24641
			public static readonly StringName _isTryingToPlayCard = "_isTryingToPlayCard";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200189B RID: 6299
		public class SignalName : Node.SignalName
		{
			/// <summary>
			/// Cached name for the 'Finished' signal.
			/// </summary>
			// Token: 0x04006042 RID: 24642
			public static readonly StringName Finished = "Finished";
		}
	}
}

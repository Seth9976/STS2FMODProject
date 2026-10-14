using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Events
{
	// Token: 0x02000828 RID: 2088
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AbyssalBaths : EventModel
	{
		// Token: 0x1700195F RID: 6495
		// (get) Token: 0x0600675F RID: 26463 RVA: 0x002726BC File Offset: 0x002708BC
		// (set) Token: 0x06006760 RID: 26464 RVA: 0x002726C4 File Offset: 0x002708C4
		private int LingerCount
		{
			get
			{
				return this._lingerCount;
			}
			set
			{
				base.AssertMutable();
				this._lingerCount = value;
			}
		}

		// Token: 0x17001960 RID: 6496
		// (get) Token: 0x06006761 RID: 26465 RVA: 0x002726D3 File Offset: 0x002708D3
		protected override IEnumerable<DynamicVar> CanonicalVars
		{
			get
			{
				return new <>z__ReadOnlyArray<DynamicVar>(new DynamicVar[]
				{
					new MaxHpVar(2m),
					new DamageVar(3m, ValueProp.Unblockable | ValueProp.Unpowered),
					new HealVar(10m)
				});
			}
		}

		// Token: 0x06006762 RID: 26466 RVA: 0x0027270C File Offset: 0x0027090C
		protected override IReadOnlyList<EventOption> GenerateInitialOptions()
		{
			return new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Immerse), "ABYSSAL_BATHS.pages.INITIAL.options.IMMERSE", Array.Empty<IHoverTip>()).ThatDoesDamage(base.DynamicVars.Damage.IntValue - base.DynamicVars.MaxHp.BaseValue),
				new EventOption(this, new Func<Task>(this.Abstain), "ABYSSAL_BATHS.pages.INITIAL.options.ABSTAIN", Array.Empty<IHoverTip>())
			});
		}

		// Token: 0x06006763 RID: 26467 RVA: 0x00272794 File Offset: 0x00270994
		private async Task Immerse()
		{
			await this.OnImmerse();
			this.SetEventState(base.L10NLookup("ABYSSAL_BATHS.pages.IMMERSE.description"), new <>z__ReadOnlyArray<EventOption>(new EventOption[]
			{
				new EventOption(this, new Func<Task>(this.Linger), "ABYSSAL_BATHS.pages.ALL.options.LINGER", Array.Empty<IHoverTip>()).ThatDoesDamage(base.DynamicVars.Damage.IntValue - base.DynamicVars.MaxHp.BaseValue),
				new EventOption(this, new Func<Task>(this.ExitBaths), "ABYSSAL_BATHS.pages.ALL.options.EXIT_BATHS", Array.Empty<IHoverTip>())
			}));
		}

		// Token: 0x06006764 RID: 26468 RVA: 0x002727D8 File Offset: 0x002709D8
		private async Task Abstain()
		{
			await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.IntValue, true);
			base.SetEventFinished(base.L10NLookup("ABYSSAL_BATHS.pages.ABSTAIN.description"));
		}

		// Token: 0x06006765 RID: 26469 RVA: 0x0027281C File Offset: 0x00270A1C
		private async Task Linger()
		{
			int lingerCount = this.LingerCount;
			this.LingerCount = lingerCount + 1;
			if (this.LingerCount > 9)
			{
				this.LingerCount = 9;
			}
			await this.OnImmerse();
			decimal num = base.DynamicVars.Damage.IntValue - base.DynamicVars.MaxHp.BaseValue;
			bool flag = this.WillKillPlayer(num);
			if (flag)
			{
				this.SetEventState(base.L10NLookup("ABYSSAL_BATHS.pages.DEATH_WARNING.description"), new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					new EventOption(this, new Func<Task>(this.Linger), "ABYSSAL_BATHS.pages.ALL.options.LINGER", Array.Empty<IHoverTip>()).ThatDoesDamage(num),
					new EventOption(this, new Func<Task>(this.ExitBaths), "ABYSSAL_BATHS.pages.ALL.options.EXIT_BATHS", Array.Empty<IHoverTip>())
				}));
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ABYSSAL_BATHS.pages.LINGER");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.LingerCount);
				defaultInterpolatedStringHandler.AppendLiteral(".description");
				this.SetEventState(base.L10NLookup(defaultInterpolatedStringHandler.ToStringAndClear()), new <>z__ReadOnlyArray<EventOption>(new EventOption[]
				{
					new EventOption(this, new Func<Task>(this.Linger), "ABYSSAL_BATHS.pages.ALL.options.LINGER", Array.Empty<IHoverTip>()).ThatDoesDamage(num),
					new EventOption(this, new Func<Task>(this.ExitBaths), "ABYSSAL_BATHS.pages.ALL.options.EXIT_BATHS", Array.Empty<IHoverTip>())
				}));
			}
		}

		// Token: 0x06006766 RID: 26470 RVA: 0x0027285F File Offset: 0x00270A5F
		private bool WillKillPlayer(decimal damage)
		{
			return base.Owner.Creature.CurrentHp <= damage;
		}

		// Token: 0x06006767 RID: 26471 RVA: 0x0027287C File Offset: 0x00270A7C
		private Task ExitBaths()
		{
			base.SetEventFinished(base.L10NLookup("ABYSSAL_BATHS.pages.EXIT_BATHS.description"));
			return Task.CompletedTask;
		}

		// Token: 0x06006768 RID: 26472 RVA: 0x00272894 File Offset: 0x00270A94
		private async Task OnImmerse()
		{
			await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.BaseValue);
			await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Damage, null, null, null);
			base.DynamicVars.Damage.BaseValue += 1m;
		}

		// Token: 0x04002753 RID: 10067
		private const int _baseDamage = 3;

		// Token: 0x04002754 RID: 10068
		private const int _damageScaling = 1;

		// Token: 0x04002755 RID: 10069
		private int _lingerCount;
	}
}

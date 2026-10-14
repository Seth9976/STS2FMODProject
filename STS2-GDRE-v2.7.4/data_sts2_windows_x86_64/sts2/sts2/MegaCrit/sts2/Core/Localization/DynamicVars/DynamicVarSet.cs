using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace MegaCrit.Sts2.Core.Localization.DynamicVars
{
	// Token: 0x02000BE2 RID: 3042
	[NullableContext(1)]
	[Nullable(0)]
	public class DynamicVarSet : IReadOnlyDictionary<string, DynamicVar>, IEnumerable<KeyValuePair<string, DynamicVar>>, IEnumerable, IReadOnlyCollection<KeyValuePair<string, DynamicVar>>
	{
		// Token: 0x06007E30 RID: 32304 RVA: 0x002ADB29 File Offset: 0x002ABD29
		[return: Nullable(new byte[] { 1, 0, 1, 1 })]
		public IEnumerator<KeyValuePair<string, DynamicVar>> GetEnumerator()
		{
			return this._vars.GetEnumerator();
		}

		// Token: 0x06007E31 RID: 32305 RVA: 0x002ADB3B File Offset: 0x002ABD3B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x170022CE RID: 8910
		// (get) Token: 0x06007E32 RID: 32306 RVA: 0x002ADB43 File Offset: 0x002ABD43
		public int Count
		{
			get
			{
				return this._vars.Count;
			}
		}

		// Token: 0x06007E33 RID: 32307 RVA: 0x002ADB50 File Offset: 0x002ABD50
		public bool ContainsKey(string key)
		{
			return this._vars.ContainsKey(key);
		}

		// Token: 0x06007E34 RID: 32308 RVA: 0x002ADB5E File Offset: 0x002ABD5E
		public bool TryGetValue(string key, [MaybeNullWhen(false)] out DynamicVar value)
		{
			return this._vars.TryGetValue(key, out value);
		}

		// Token: 0x170022CF RID: 8911
		public DynamicVar this[string key]
		{
			get
			{
				return this._vars[key];
			}
		}

		// Token: 0x170022D0 RID: 8912
		// (get) Token: 0x06007E36 RID: 32310 RVA: 0x002ADB7B File Offset: 0x002ABD7B
		public IEnumerable<string> Keys
		{
			get
			{
				return this._vars.Keys;
			}
		}

		// Token: 0x170022D1 RID: 8913
		// (get) Token: 0x06007E37 RID: 32311 RVA: 0x002ADB88 File Offset: 0x002ABD88
		public IEnumerable<DynamicVar> Values
		{
			get
			{
				return this._vars.Values;
			}
		}

		// Token: 0x170022D2 RID: 8914
		// (get) Token: 0x06007E38 RID: 32312 RVA: 0x002ADB95 File Offset: 0x002ABD95
		public BlockVar Block
		{
			get
			{
				return (BlockVar)this._vars["Block"];
			}
		}

		// Token: 0x170022D3 RID: 8915
		// (get) Token: 0x06007E39 RID: 32313 RVA: 0x002ADBAC File Offset: 0x002ABDAC
		public CalculatedBlockVar CalculatedBlock
		{
			get
			{
				return (CalculatedBlockVar)this._vars["CalculatedBlock"];
			}
		}

		// Token: 0x170022D4 RID: 8916
		// (get) Token: 0x06007E3A RID: 32314 RVA: 0x002ADBC3 File Offset: 0x002ABDC3
		public CalculatedDamageVar CalculatedDamage
		{
			get
			{
				return (CalculatedDamageVar)this._vars["CalculatedDamage"];
			}
		}

		// Token: 0x170022D5 RID: 8917
		// (get) Token: 0x06007E3B RID: 32315 RVA: 0x002ADBDA File Offset: 0x002ABDDA
		public CalculationBaseVar CalculationBase
		{
			get
			{
				return (CalculationBaseVar)this._vars["CalculationBase"];
			}
		}

		// Token: 0x170022D6 RID: 8918
		// (get) Token: 0x06007E3C RID: 32316 RVA: 0x002ADBF1 File Offset: 0x002ABDF1
		public CalculationExtraVar CalculationExtra
		{
			get
			{
				return (CalculationExtraVar)this._vars["CalculationExtra"];
			}
		}

		// Token: 0x170022D7 RID: 8919
		// (get) Token: 0x06007E3D RID: 32317 RVA: 0x002ADC08 File Offset: 0x002ABE08
		public CardsVar Cards
		{
			get
			{
				return (CardsVar)this._vars["Cards"];
			}
		}

		// Token: 0x170022D8 RID: 8920
		// (get) Token: 0x06007E3E RID: 32318 RVA: 0x002ADC1F File Offset: 0x002ABE1F
		public DamageVar Damage
		{
			get
			{
				return (DamageVar)this._vars["Damage"];
			}
		}

		// Token: 0x170022D9 RID: 8921
		// (get) Token: 0x06007E3F RID: 32319 RVA: 0x002ADC36 File Offset: 0x002ABE36
		public PowerVar<DexterityPower> Dexterity
		{
			get
			{
				return (PowerVar<DexterityPower>)this._vars["DexterityPower"];
			}
		}

		// Token: 0x170022DA RID: 8922
		// (get) Token: 0x06007E40 RID: 32320 RVA: 0x002ADC4D File Offset: 0x002ABE4D
		public PowerVar<DoomPower> Doom
		{
			get
			{
				return (PowerVar<DoomPower>)this._vars["DoomPower"];
			}
		}

		// Token: 0x170022DB RID: 8923
		// (get) Token: 0x06007E41 RID: 32321 RVA: 0x002ADC64 File Offset: 0x002ABE64
		public EnergyVar Energy
		{
			get
			{
				return (EnergyVar)this._vars["Energy"];
			}
		}

		// Token: 0x170022DC RID: 8924
		// (get) Token: 0x06007E42 RID: 32322 RVA: 0x002ADC7B File Offset: 0x002ABE7B
		public ExtraDamageVar ExtraDamage
		{
			get
			{
				return (ExtraDamageVar)this._vars["ExtraDamage"];
			}
		}

		// Token: 0x170022DD RID: 8925
		// (get) Token: 0x06007E43 RID: 32323 RVA: 0x002ADC92 File Offset: 0x002ABE92
		public ForgeVar Forge
		{
			get
			{
				return (ForgeVar)this._vars["Forge"];
			}
		}

		// Token: 0x170022DE RID: 8926
		// (get) Token: 0x06007E44 RID: 32324 RVA: 0x002ADCA9 File Offset: 0x002ABEA9
		public GoldVar Gold
		{
			get
			{
				return (GoldVar)this._vars["Gold"];
			}
		}

		// Token: 0x170022DF RID: 8927
		// (get) Token: 0x06007E45 RID: 32325 RVA: 0x002ADCC0 File Offset: 0x002ABEC0
		public HealVar Heal
		{
			get
			{
				return (HealVar)this._vars["Heal"];
			}
		}

		// Token: 0x170022E0 RID: 8928
		// (get) Token: 0x06007E46 RID: 32326 RVA: 0x002ADCD7 File Offset: 0x002ABED7
		public HpLossVar HpLoss
		{
			get
			{
				return (HpLossVar)this._vars["HpLoss"];
			}
		}

		// Token: 0x170022E1 RID: 8929
		// (get) Token: 0x06007E47 RID: 32327 RVA: 0x002ADCEE File Offset: 0x002ABEEE
		public MaxHpVar MaxHp
		{
			get
			{
				return (MaxHpVar)this._vars["MaxHp"];
			}
		}

		// Token: 0x170022E2 RID: 8930
		// (get) Token: 0x06007E48 RID: 32328 RVA: 0x002ADD05 File Offset: 0x002ABF05
		public OstyDamageVar OstyDamage
		{
			get
			{
				return (OstyDamageVar)this._vars["OstyDamage"];
			}
		}

		// Token: 0x170022E3 RID: 8931
		// (get) Token: 0x06007E49 RID: 32329 RVA: 0x002ADD1C File Offset: 0x002ABF1C
		public PowerVar<PoisonPower> Poison
		{
			get
			{
				return (PowerVar<PoisonPower>)this._vars["PoisonPower"];
			}
		}

		// Token: 0x170022E4 RID: 8932
		// (get) Token: 0x06007E4A RID: 32330 RVA: 0x002ADD33 File Offset: 0x002ABF33
		public RepeatVar Repeat
		{
			get
			{
				return (RepeatVar)this._vars["Repeat"];
			}
		}

		// Token: 0x170022E5 RID: 8933
		// (get) Token: 0x06007E4B RID: 32331 RVA: 0x002ADD4A File Offset: 0x002ABF4A
		public StarsVar Stars
		{
			get
			{
				return (StarsVar)this._vars["Stars"];
			}
		}

		// Token: 0x170022E6 RID: 8934
		// (get) Token: 0x06007E4C RID: 32332 RVA: 0x002ADD61 File Offset: 0x002ABF61
		public PowerVar<StrengthPower> Strength
		{
			get
			{
				return (PowerVar<StrengthPower>)this._vars["StrengthPower"];
			}
		}

		// Token: 0x170022E7 RID: 8935
		// (get) Token: 0x06007E4D RID: 32333 RVA: 0x002ADD78 File Offset: 0x002ABF78
		public SummonVar Summon
		{
			get
			{
				return (SummonVar)this._vars["Summon"];
			}
		}

		// Token: 0x170022E8 RID: 8936
		// (get) Token: 0x06007E4E RID: 32334 RVA: 0x002ADD8F File Offset: 0x002ABF8F
		public PowerVar<VulnerablePower> Vulnerable
		{
			get
			{
				return (PowerVar<VulnerablePower>)this._vars["VulnerablePower"];
			}
		}

		// Token: 0x170022E9 RID: 8937
		// (get) Token: 0x06007E4F RID: 32335 RVA: 0x002ADDA6 File Offset: 0x002ABFA6
		public PowerVar<WeakPower> Weak
		{
			get
			{
				return (PowerVar<WeakPower>)this._vars["WeakPower"];
			}
		}

		// Token: 0x06007E50 RID: 32336 RVA: 0x002ADDC0 File Offset: 0x002ABFC0
		public DynamicVarSet(IEnumerable<DynamicVar> vars)
		{
			foreach (DynamicVar dynamicVar in vars)
			{
				if (this._vars.ContainsKey(dynamicVar.Name))
				{
					throw new ArgumentException(StringHelper.CompactText("DynamicVarSet contains duplicate key '" + dynamicVar.Name + "'. If you're using two of the same variable\ntype (like 2 BlockVars for Halt), please specify a name for each one instead of using the default."));
				}
				this._vars[dynamicVar.Name] = dynamicVar;
			}
		}

		/// <summary>
		/// Initialize this DynamicVarSet with a model that "owns" the vars.
		/// This must be called in a separate method, because some DynamicVars are dependent on the model's DynamicVarSet,
		/// so we can't call this until the model's DynamicVarSet property has been set.
		/// </summary>
		// Token: 0x06007E51 RID: 32337 RVA: 0x002ADE58 File Offset: 0x002AC058
		public void InitializeWithOwner(AbstractModel model)
		{
			foreach (DynamicVar dynamicVar in this.Values)
			{
				dynamicVar.SetOwner(model);
			}
		}

		// Token: 0x06007E52 RID: 32338 RVA: 0x002ADEA8 File Offset: 0x002AC0A8
		public void AddTo(LocString str)
		{
			foreach (KeyValuePair<string, DynamicVar> keyValuePair in this)
			{
				str.Add(keyValuePair.Value);
			}
		}

		// Token: 0x06007E53 RID: 32339 RVA: 0x002ADEF8 File Offset: 0x002AC0F8
		public void ClearPreview()
		{
			foreach (DynamicVar dynamicVar in this.Values)
			{
				dynamicVar.ResetToBase();
			}
		}

		// Token: 0x06007E54 RID: 32340 RVA: 0x002ADF44 File Offset: 0x002AC144
		public void FinalizeUpgrade()
		{
			foreach (DynamicVar dynamicVar in this.Values)
			{
				dynamicVar.FinalizeUpgrade();
			}
		}

		/// <summary>
		/// Recalculate all calculated vars to determine if any of the dependent values have been upgraded or enchanted.
		/// </summary>
		// Token: 0x06007E55 RID: 32341 RVA: 0x002ADF90 File Offset: 0x002AC190
		public void RecalculateForUpgradeOrEnchant()
		{
			foreach (CalculatedVar calculatedVar in this.Values.OfType<CalculatedVar>())
			{
				calculatedVar.RecalculateForUpgradeOrEnchant();
			}
		}

		// Token: 0x06007E56 RID: 32342 RVA: 0x002ADFE4 File Offset: 0x002AC1E4
		public DynamicVarSet Clone(AbstractModel model)
		{
			DynamicVarSet dynamicVarSet = new DynamicVarSet(this.Values.Select<DynamicVar, DynamicVar>((DynamicVar v) => v.Clone()));
			dynamicVarSet.InitializeWithOwner(model);
			return dynamicVarSet;
		}

		// Token: 0x04002A31 RID: 10801
		private readonly Dictionary<string, DynamicVar> _vars = new Dictionary<string, DynamicVar>();
	}
}

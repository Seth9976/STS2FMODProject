using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities
{
	// Token: 0x02000222 RID: 546
	public class ScreenTraumaRumble
	{
		// Token: 0x06001BCF RID: 7119 RVA: 0x000B4A58 File Offset: 0x000B2C58
		public ScreenTraumaRumble()
		{
			this._noise.Frequency = 0.01f;
			this._noise.Seed = Rng.Chaotic.NextInt(999999);
			this._noise.NoiseType = 3L;
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x000B4AB0 File Offset: 0x000B2CB0
		public void AddTrauma(ShakeStrength amount)
		{
			float trauma = this._trauma;
			float num;
			switch (amount)
			{
			case ShakeStrength.VeryWeak:
				num = 0.07f;
				break;
			case ShakeStrength.Weak:
				num = 0.2f;
				break;
			case ShakeStrength.Medium:
				num = 0.4f;
				break;
			case ShakeStrength.Strong:
				num = 0.6f;
				break;
			case ShakeStrength.TooMuch:
				num = 1f;
				break;
			default:
				throw new ArgumentOutOfRangeException("amount", amount, null);
			}
			this._trauma = trauma + num;
			this._trauma = Mathf.Min(this._trauma, 1f);
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x000B4B3C File Offset: 0x000B2D3C
		public Vector2 Update(double delta)
		{
			if (this._trauma <= 0f)
			{
				return Vector2.Zero;
			}
			this._duration += delta * 1000.0;
			float num = Mathf.Pow(this._trauma, 1.5f) * 200f * this._multiplier;
			float noise1D = this._noise.GetNoise1D((float)this._duration + 100f);
			float noise1D2 = this._noise.GetNoise1D((float)this._duration + 300f);
			Vector2 vector = (new Vector2(noise1D, noise1D2) * num).LimitLength(50f);
			this._trauma = Mathf.Max(this._trauma - (float)(2.0 * delta), 0f);
			return vector;
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x000B4C03 File Offset: 0x000B2E03
		public void SetMultiplier(float multiplier)
		{
			this._multiplier = multiplier;
		}

		// Token: 0x04000A82 RID: 2690
		private const float _rumbleAmount = 200f;

		// Token: 0x04000A83 RID: 2691
		private const float _speed = 1000f;

		// Token: 0x04000A84 RID: 2692
		private const float _decayRate = 2f;

		// Token: 0x04000A85 RID: 2693
		private const float _maxShake = 50f;

		// Token: 0x04000A86 RID: 2694
		[Nullable(1)]
		private readonly FastNoiseLite _noise = new FastNoiseLite();

		// Token: 0x04000A87 RID: 2695
		private float _trauma;

		// Token: 0x04000A88 RID: 2696
		private double _duration;

		// Token: 0x04000A89 RID: 2697
		private float _multiplier;
	}
}

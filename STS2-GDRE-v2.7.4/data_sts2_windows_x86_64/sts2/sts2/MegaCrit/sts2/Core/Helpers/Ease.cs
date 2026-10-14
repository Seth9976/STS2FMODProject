using System;
using Godot;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C05 RID: 3077
	public static class Ease
	{
		/// <summary>
		/// Interpolate using the specified function.
		/// </summary>
		// Token: 0x06007F81 RID: 32641 RVA: 0x002B49A0 File Offset: 0x002B2BA0
		public static float Interpolate(float p, Ease.Functions function)
		{
			float num;
			switch (function)
			{
			case Ease.Functions.QuadIn:
				num = Ease.QuadIn(p);
				break;
			case Ease.Functions.QuadOut:
				num = Ease.QuadOut(p);
				break;
			case Ease.Functions.QuadInOut:
				num = Ease.QuadInOut(p);
				break;
			case Ease.Functions.CubicIn:
				num = Ease.CubicIn(p);
				break;
			case Ease.Functions.CubicOut:
				num = Ease.CubicOut(p);
				break;
			case Ease.Functions.CubicInOut:
				num = Ease.CubicInOut(p);
				break;
			case Ease.Functions.QuartIn:
				num = Ease.QuartIn(p);
				break;
			case Ease.Functions.QuartOut:
				num = Ease.QuartOut(p);
				break;
			case Ease.Functions.QuartInOut:
				num = Ease.QuartInOut(p);
				break;
			case Ease.Functions.QuintIn:
				num = Ease.QuintIn(p);
				break;
			case Ease.Functions.QuinOut:
				num = Ease.QuintOut(p);
				break;
			case Ease.Functions.QuinInOut:
				num = Ease.QuintInOut(p);
				break;
			case Ease.Functions.SineIn:
				num = Ease.SineIn(p);
				break;
			case Ease.Functions.SineOut:
				num = Ease.SineOut(p);
				break;
			case Ease.Functions.SineInOut:
				num = Ease.SineInOut(p);
				break;
			case Ease.Functions.CircIn:
				num = Ease.CircIn(p);
				break;
			case Ease.Functions.CircOut:
				num = Ease.CircOut(p);
				break;
			case Ease.Functions.CircInOut:
				num = Ease.CircInOut(p);
				break;
			case Ease.Functions.ExpoIn:
				num = Ease.ExpoIn(p);
				break;
			case Ease.Functions.ExpoOut:
				num = Ease.ExpoOut(p);
				break;
			case Ease.Functions.ExpoInOut:
				num = Ease.ExpoInOut(p);
				break;
			case Ease.Functions.ElasticIn:
				num = Ease.ElasticIn(p);
				break;
			case Ease.Functions.ElasticOut:
				num = Ease.ElasticOut(p);
				break;
			case Ease.Functions.ElasticInOut:
				num = Ease.ElasticInOut(p);
				break;
			case Ease.Functions.BackIn:
				num = Ease.BackIn(p, 1f);
				break;
			case Ease.Functions.BackOut:
				num = Ease.BackOut(p, 1f);
				break;
			case Ease.Functions.BackInOut:
				num = Ease.BackInOut(p, 1f);
				break;
			case Ease.Functions.BounceIn:
				num = Ease.BounceIn(p);
				break;
			case Ease.Functions.BounceOut:
				num = Ease.BounceOut(p);
				break;
			case Ease.Functions.BounceInOut:
				num = Ease.BounceInOut(p);
				break;
			default:
				num = Ease.Linear(p);
				break;
			}
			return num;
		}

		/// <summary>
		/// Modeled after the line y = x
		/// </summary>
		// Token: 0x06007F82 RID: 32642 RVA: 0x002B4B8B File Offset: 0x002B2D8B
		public static float Linear(float p)
		{
			return p;
		}

		/// <summary>
		/// Modeled after the parabola y = x^2
		/// </summary>
		// Token: 0x06007F83 RID: 32643 RVA: 0x002B4B8E File Offset: 0x002B2D8E
		public static float QuadIn(float p)
		{
			return p * p;
		}

		/// <summary>
		/// Modeled after the parabola y = -x^2 + 2x
		/// </summary>
		// Token: 0x06007F84 RID: 32644 RVA: 0x002B4B93 File Offset: 0x002B2D93
		public static float QuadOut(float p)
		{
			return -(p * (p - 2f));
		}

		/// <summary>
		/// Modeled after the piecewise quadratic
		/// y = (1/2)((2x)^2)             ; [0, 0.5)
		/// y = -(1/2)((2x-1)*(2x-3) - 1) ; [0.5, 1]
		/// </summary>
		// Token: 0x06007F85 RID: 32645 RVA: 0x002B4B9F File Offset: 0x002B2D9F
		public static float QuadInOut(float p)
		{
			if (p < 0.5f)
			{
				return 2f * p * p;
			}
			return -2f * p * p + 4f * p - 1f;
		}

		/// <summary>
		/// Modeled after the cubic y = x^3
		/// </summary>
		// Token: 0x06007F86 RID: 32646 RVA: 0x002B4BCA File Offset: 0x002B2DCA
		public static float CubicIn(float p)
		{
			return p * p * p;
		}

		/// <summary>
		/// Modeled after the cubic y = (x - 1)^3 + 1
		/// </summary>
		// Token: 0x06007F87 RID: 32647 RVA: 0x002B4BD4 File Offset: 0x002B2DD4
		public static float CubicOut(float p)
		{
			float num = p - 1f;
			return num * num * num + 1f;
		}

		/// <summary>
		/// Modeled after the piecewise cubic
		/// y = (1/2)((2x)^3)       ; [0, 0.5)
		/// y = (1/2)((2x-2)^3 + 2) ; [0.5, 1]
		/// </summary>
		// Token: 0x06007F88 RID: 32648 RVA: 0x002B4BF4 File Offset: 0x002B2DF4
		public static float CubicInOut(float p)
		{
			if (p < 0.5f)
			{
				return 4f * p * p * p;
			}
			float num = 2f * p - 2f;
			return 0.5f * num * num * num + 1f;
		}

		/// <summary>
		/// Modeled after the quartic x^4
		/// </summary>
		// Token: 0x06007F89 RID: 32649 RVA: 0x002B4C34 File Offset: 0x002B2E34
		public static float QuartIn(float p)
		{
			return p * p * p * p;
		}

		/// <summary>
		/// Modeled after the quartic y = 1 - (x - 1)^4
		/// </summary>
		// Token: 0x06007F8A RID: 32650 RVA: 0x002B4C40 File Offset: 0x002B2E40
		public static float QuartOut(float p)
		{
			float num = p - 1f;
			return num * num * num * (1f - p) + 1f;
		}

		/// <summary>
		/// Modeled after the piecewise quartic
		/// y = (1/2)((2x)^4)        ; [0, 0.5)
		/// y = -(1/2)((2x-2)^4 - 2) ; [0.5, 1]
		/// </summary>
		// Token: 0x06007F8B RID: 32651 RVA: 0x002B4C68 File Offset: 0x002B2E68
		public static float QuartInOut(float p)
		{
			if (p < 0.5f)
			{
				return 8f * p * p * p * p;
			}
			float num = p - 1f;
			return -8f * num * num * num * num + 1f;
		}

		/// <summary>
		/// Modeled after the quintic y = x^5
		/// </summary>
		// Token: 0x06007F8C RID: 32652 RVA: 0x002B4CA6 File Offset: 0x002B2EA6
		public static float QuintIn(float p)
		{
			return p * p * p * p * p;
		}

		/// <summary>
		/// Modeled after the quintic y = (x - 1)^5 + 1
		/// </summary>
		// Token: 0x06007F8D RID: 32653 RVA: 0x002B4CB4 File Offset: 0x002B2EB4
		public static float QuintOut(float p)
		{
			float num = p - 1f;
			return num * num * num * num * num + 1f;
		}

		/// <summary>
		/// Modeled after the piecewise quintic
		/// y = (1/2)((2x)^5)       ; [0, 0.5)
		/// y = (1/2)((2x-2)^5 + 2) ; [0.5, 1]
		/// </summary>
		// Token: 0x06007F8E RID: 32654 RVA: 0x002B4CD8 File Offset: 0x002B2ED8
		public static float QuintInOut(float p)
		{
			if (p < 0.5f)
			{
				return 16f * p * p * p * p * p;
			}
			float num = 2f * p - 2f;
			return 0.5f * num * num * num * num * num + 1f;
		}

		/// <summary>
		/// Modeled after quarter-cycle of sine wave
		/// </summary>
		// Token: 0x06007F8F RID: 32655 RVA: 0x002B4D20 File Offset: 0x002B2F20
		public static float SineIn(float p)
		{
			return Mathf.Sin((p - 1f) * 1.5707964f) + 1f;
		}

		/// <summary>
		/// Modeled after quarter-cycle of sine wave (different phase)
		/// </summary>
		// Token: 0x06007F90 RID: 32656 RVA: 0x002B4D3A File Offset: 0x002B2F3A
		public static float SineOut(float p)
		{
			return Mathf.Sin(p * 1.5707964f);
		}

		/// <summary>
		/// Modeled after half sine wave
		/// </summary>
		// Token: 0x06007F91 RID: 32657 RVA: 0x002B4D48 File Offset: 0x002B2F48
		public static float SineInOut(float p)
		{
			return 0.5f * (1f - Mathf.Cos(p * 3.1415927f));
		}

		/// <summary>
		/// Modeled after shifted quadrant IV of unit circle
		/// </summary>
		// Token: 0x06007F92 RID: 32658 RVA: 0x002B4D62 File Offset: 0x002B2F62
		public static float CircIn(float p)
		{
			return 1f - Mathf.Sqrt(1f - p * p);
		}

		/// <summary>
		/// Modeled after shifted quadrant II of unit circle
		/// </summary>
		// Token: 0x06007F93 RID: 32659 RVA: 0x002B4D78 File Offset: 0x002B2F78
		public static float CircOut(float p)
		{
			return Mathf.Sqrt((2f - p) * p);
		}

		/// <summary>
		/// Modeled after the piecewise circular function
		/// y = (1/2)(1 - Mathf.Sqrt(1 - 4x^2))           ; [0, 0.5)
		/// y = (1/2)(Mathf.Sqrt(-(2x - 3)*(2x - 1)) + 1) ; [0.5, 1]
		/// </summary>
		// Token: 0x06007F94 RID: 32660 RVA: 0x002B4D88 File Offset: 0x002B2F88
		public static float CircInOut(float p)
		{
			if (p < 0.5f)
			{
				return 0.5f * (1f - Mathf.Sqrt(1f - 4f * (p * p)));
			}
			return 0.5f * (Mathf.Sqrt(-(2f * p - 3f) * (2f * p - 1f)) + 1f);
		}

		/// <summary>
		/// Modeled after the exponential function y = 2^(10(x - 1))
		/// </summary>
		// Token: 0x06007F95 RID: 32661 RVA: 0x002B4DEB File Offset: 0x002B2FEB
		public static float ExpoIn(float p)
		{
			if (p != 0f)
			{
				return Mathf.Pow(2f, 10f * (p - 1f));
			}
			return p;
		}

		/// <summary>
		/// Modeled after the exponential function y = -2^(-10x) + 1
		/// </summary>
		// Token: 0x06007F96 RID: 32662 RVA: 0x002B4E0E File Offset: 0x002B300E
		public static float ExpoOut(float p)
		{
			if (Math.Abs(p - 1f) >= 0.001f)
			{
				return 1f - Mathf.Pow(2f, -10f * p);
			}
			return p;
		}

		/// <summary>
		/// Modeled after the piecewise exponential
		/// y = (1/2)2^(10(2x - 1))         ; [0,0.5)
		/// y = -(1/2)*2^(-10(2x - 1))) + 1 ; [0.5,1]
		/// </summary>
		// Token: 0x06007F97 RID: 32663 RVA: 0x002B4E3C File Offset: 0x002B303C
		public static float ExpoInOut(float p)
		{
			if (p == 0f || Math.Abs(p - 1f) < 0.001f)
			{
				return p;
			}
			if (p < 0.5f)
			{
				return 0.5f * Mathf.Pow(2f, 20f * p - 10f);
			}
			return -0.5f * Mathf.Pow(2f, -20f * p + 10f) + 1f;
		}

		/// <summary>
		/// Modeled after the damped sine wave y = sin(13pi/2*x)*Mathf.Pow(2, 10 * (x - 1))
		/// </summary>
		// Token: 0x06007F98 RID: 32664 RVA: 0x002B4EAF File Offset: 0x002B30AF
		public static float ElasticIn(float p)
		{
			return Mathf.Sin(20.420353f * p) * Mathf.Pow(2f, 10f * (p - 1f));
		}

		/// <summary>
		/// Modeled after the damped sine wave y = sin(-13pi/2*(x + 1))*Mathf.Pow(2, -10x) + 1
		/// </summary>
		// Token: 0x06007F99 RID: 32665 RVA: 0x002B4ED5 File Offset: 0x002B30D5
		public static float ElasticOut(float p)
		{
			return Mathf.Sin(-20.420353f * (p + 1f)) * Mathf.Pow(2f, -10f * p) + 1f;
		}

		/// <summary>
		/// Modeled after the piecewise exponentially-damped sine wave:
		/// y = (1/2)*sin(13pi/2*(2*x))*Mathf.Pow(2, 10 * ((2*x) - 1))      ; [0,0.5)
		/// y = (1/2)*(sin(-13pi/2*((2x-1)+1))*Mathf.Pow(2,-10(2*x-1)) + 2) ; [0.5, 1]
		/// </summary>
		// Token: 0x06007F9A RID: 32666 RVA: 0x002B4F04 File Offset: 0x002B3104
		public static float ElasticInOut(float p)
		{
			if (p < 0.5f)
			{
				return 0.5f * Mathf.Sin(20.420353f * (2f * p)) * Mathf.Pow(2f, 10f * (2f * p - 1f));
			}
			return 0.5f * (Mathf.Sin(-20.420353f * (2f * p - 1f + 1f)) * Mathf.Pow(2f, -10f * (2f * p - 1f)) + 2f);
		}

		/// <summary>
		/// Lerps an object beyond the target destination and brings it back
		/// </summary>
		// Token: 0x06007F9B RID: 32667 RVA: 0x002B4F98 File Offset: 0x002B3198
		public static float BackIn(float p, float strength = 1f)
		{
			float num = strength * 1.70158f;
			return p * p * ((num + 1f) * p - num);
		}

		/// <summary>
		/// Lerps an object beyond the target destination and brings it back
		/// </summary>
		// Token: 0x06007F9C RID: 32668 RVA: 0x002B4FBC File Offset: 0x002B31BC
		public static float BackOut(float p, float strength = 1f)
		{
			float num = strength * 1.70158f;
			return 1f + num * (p - 1f) * (p - 1f) * ((num + 1f) * (p - 1f) + num);
		}

		/// <summary>
		/// Lerps an object beyond the target destination and brings it back
		/// </summary>
		// Token: 0x06007F9D RID: 32669 RVA: 0x002B4FFC File Offset: 0x002B31FC
		public static float BackInOut(float p, float strength = 1f)
		{
			float num = strength * 1.70158f;
			p *= 2f;
			if (p < 1f)
			{
				return 0.5f * (p * p * (((num *= 1.525f) + 1f) * p - num));
			}
			return 0.5f * ((p -= 2f) * p * (((num *= 1.525f) + 1f) * p + num) + 2f);
		}

		// Token: 0x06007F9E RID: 32670 RVA: 0x002B506C File Offset: 0x002B326C
		public static float BounceIn(float p)
		{
			return 1f - Ease.BounceOut(1f - p);
		}

		// Token: 0x06007F9F RID: 32671 RVA: 0x002B5080 File Offset: 0x002B3280
		public static float BounceOut(float p)
		{
			if (p < 0.36363637f)
			{
				return 121f * p * p / 16f;
			}
			if (p < 0.72727275f)
			{
				return 9.075f * p * p - 9.9f * p + 3.4f;
			}
			if (p < 0.9f)
			{
				return 12.066482f * p * p - 19.635458f * p + 8.898061f;
			}
			return 10.8f * p * p - 20.52f * p + 10.72f;
		}

		// Token: 0x06007FA0 RID: 32672 RVA: 0x002B50FC File Offset: 0x002B32FC
		public static float BounceInOut(float p)
		{
			if (p < 0.5f)
			{
				return 0.5f * Ease.BounceIn(p * 2f);
			}
			return 0.5f * Ease.BounceOut(p * 2f - 1f) + 0.5f;
		}

		/// <summary>
		/// Used for safe floating point comparisons with pixel values on a 4K screen.
		/// </summary>
		// Token: 0x04002A81 RID: 10881
		private const float _tolerance = 0.001f;

		/// <summary>
		/// Constant Pi.
		/// </summary>
		// Token: 0x04002A82 RID: 10882
		private const float _pi = 3.1415927f;

		/// <summary>
		/// Constant Pi / 2.
		/// </summary>
		// Token: 0x04002A83 RID: 10883
		private const float _halfPi = 1.5707964f;

		/// <summary>
		/// Easing function enums
		/// </summary>
		// Token: 0x0200234E RID: 9038
		public enum Functions
		{
			// Token: 0x04009598 RID: 38296
			QuadIn,
			// Token: 0x04009599 RID: 38297
			QuadOut,
			// Token: 0x0400959A RID: 38298
			QuadInOut,
			// Token: 0x0400959B RID: 38299
			CubicIn,
			// Token: 0x0400959C RID: 38300
			CubicOut,
			// Token: 0x0400959D RID: 38301
			CubicInOut,
			// Token: 0x0400959E RID: 38302
			QuartIn,
			// Token: 0x0400959F RID: 38303
			QuartOut,
			// Token: 0x040095A0 RID: 38304
			QuartInOut,
			// Token: 0x040095A1 RID: 38305
			QuintIn,
			// Token: 0x040095A2 RID: 38306
			QuinOut,
			// Token: 0x040095A3 RID: 38307
			QuinInOut,
			// Token: 0x040095A4 RID: 38308
			SineIn,
			// Token: 0x040095A5 RID: 38309
			SineOut,
			// Token: 0x040095A6 RID: 38310
			SineInOut,
			// Token: 0x040095A7 RID: 38311
			CircIn,
			// Token: 0x040095A8 RID: 38312
			CircOut,
			// Token: 0x040095A9 RID: 38313
			CircInOut,
			// Token: 0x040095AA RID: 38314
			ExpoIn,
			// Token: 0x040095AB RID: 38315
			ExpoOut,
			// Token: 0x040095AC RID: 38316
			ExpoInOut,
			// Token: 0x040095AD RID: 38317
			ElasticIn,
			// Token: 0x040095AE RID: 38318
			ElasticOut,
			// Token: 0x040095AF RID: 38319
			ElasticInOut,
			// Token: 0x040095B0 RID: 38320
			BackIn,
			// Token: 0x040095B1 RID: 38321
			BackOut,
			// Token: 0x040095B2 RID: 38322
			BackInOut,
			// Token: 0x040095B3 RID: 38323
			BounceIn,
			// Token: 0x040095B4 RID: 38324
			BounceOut,
			// Token: 0x040095B5 RID: 38325
			BounceInOut
		}
	}
}

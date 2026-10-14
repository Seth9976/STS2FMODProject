using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Helpers;

namespace MegaCrit.Sts2.Core.Entities.Potions
{
	// Token: 0x02000C75 RID: 3189
	[NullableContext(1)]
	[Nullable(0)]
	public static class PotionBodyExtensions
	{
		// Token: 0x0600829F RID: 33439 RVA: 0x002C1CF0 File Offset: 0x002BFEF0
		public static string GetBodyPath(this PotionBody body)
		{
			string text;
			switch (body)
			{
			case PotionBody.None:
				throw new ArgumentOutOfRangeException("body", body, null);
			case PotionBody.Anvil:
				text = "potion_anvil_body.png";
				break;
			case PotionBody.Bolt:
				text = "potion_bolt_body.png";
				break;
			case PotionBody.Card:
				text = "potion_card_body.png";
				break;
			case PotionBody.Cube:
				text = "potion_cube_body.png";
				break;
			case PotionBody.Diamond:
				text = "potion_diamond_body.png";
				break;
			case PotionBody.Eye:
				text = "potion_eye_body.png";
				break;
			case PotionBody.Fairy:
				text = "potion_fairy_body.png";
				break;
			case PotionBody.Fat:
				text = "potion_fat_body.png";
				break;
			case PotionBody.FatDiamond:
				text = "potion_fat_diamond_body.png";
				break;
			case PotionBody.Flask:
				text = "potion_flask_body.png";
				break;
			case PotionBody.Ghost:
				text = "potion_ghost_body.png";
				break;
			case PotionBody.Heart:
				text = "potion_heart_body.png";
				break;
			case PotionBody.Moon:
				text = "potion_moon_body.png";
				break;
			case PotionBody.Shield:
				text = "potion_shield_body.png";
				break;
			case PotionBody.Snecko:
				text = "potion_snecko_body.png";
				break;
			case PotionBody.Sphere:
				text = "potion_sphere_body.png";
				break;
			case PotionBody.Spiky:
				text = "potion_spiky_body.png";
				break;
			case PotionBody.Thin:
				text = "potion_thin_body.png";
				break;
			default:
				throw new ArgumentOutOfRangeException("body", body, null);
			}
			string text2 = text;
			return ImageHelper.GetImagePath("packed/potion/body/" + text2);
		}

		// Token: 0x060082A0 RID: 33440 RVA: 0x002C1E28 File Offset: 0x002C0028
		[NullableContext(2)]
		public static string GetGradientPath(this PotionBody body)
		{
			string text;
			if (body != PotionBody.Anvil)
			{
				if (body != PotionBody.Card)
				{
					if (body != PotionBody.Sphere)
					{
						text = null;
					}
					else
					{
						text = "potion_sphere_gradient.png";
					}
				}
				else
				{
					text = "potion_card_gradient.png";
				}
			}
			else
			{
				text = "potion_anvil_gradient.png";
			}
			string text2 = text;
			if (text2 != null)
			{
				return ImageHelper.GetImagePath("packed/potion/gradient/" + text2);
			}
			return null;
		}

		// Token: 0x060082A1 RID: 33441 RVA: 0x002C1E78 File Offset: 0x002C0078
		public static string GetJuicePath(this PotionBody body)
		{
			string text;
			switch (body)
			{
			case PotionBody.None:
				throw new ArgumentOutOfRangeException("body", body, null);
			case PotionBody.Anvil:
				text = "potion_anvil_juice.png";
				break;
			case PotionBody.Bolt:
				text = "potion_bolt_juice.png";
				break;
			case PotionBody.Card:
				text = "potion_card_juice.png";
				break;
			case PotionBody.Cube:
				text = "potion_cube_juice.png";
				break;
			case PotionBody.Diamond:
				text = "potion_diamond_juice.png";
				break;
			case PotionBody.Eye:
				text = "potion_eye_juice.png";
				break;
			case PotionBody.Fairy:
				text = "potion_fairy_juice.png";
				break;
			case PotionBody.Fat:
				text = "potion_fat_juice.png";
				break;
			case PotionBody.FatDiamond:
				text = "potion_fat_diamond_juice.png";
				break;
			case PotionBody.Flask:
				text = "potion_flask_juice.png";
				break;
			case PotionBody.Ghost:
				text = "potion_ghost_juice.png";
				break;
			case PotionBody.Heart:
				text = "potion_heart_juice.png";
				break;
			case PotionBody.Moon:
				text = "potion_moon_juice.png";
				break;
			case PotionBody.Shield:
				text = "potion_shield_juice.png";
				break;
			case PotionBody.Snecko:
				text = "potion_snecko_juice.png";
				break;
			case PotionBody.Sphere:
				text = "potion_sphere_juice.png";
				break;
			case PotionBody.Spiky:
				text = "potion_spiky_juice.png";
				break;
			case PotionBody.Thin:
				text = "potion_thin_juice.png";
				break;
			default:
				throw new ArgumentOutOfRangeException("body", body, null);
			}
			string text2 = text;
			return ImageHelper.GetImagePath("packed/potion/juice/" + text2);
		}

		// Token: 0x060082A2 RID: 33442 RVA: 0x002C1FB0 File Offset: 0x002C01B0
		[NullableContext(2)]
		public static string GetOverlayPath(this PotionBody body, PotionOverlay overlay)
		{
			string text;
			if (PotionBodyExtensions._potionOverlayMap.TryGetValue(new ValueTuple<PotionBody, PotionOverlay>(body, overlay), out text))
			{
				return ImageHelper.GetImagePath("packed/potion/overlay/" + text);
			}
			return null;
		}

		// Token: 0x060082A3 RID: 33443 RVA: 0x002C1FE4 File Offset: 0x002C01E4
		// Note: this type is marked as 'beforefieldinit'.
		static PotionBodyExtensions()
		{
			Dictionary<ValueTuple<PotionBody, PotionOverlay>, string> dictionary = new Dictionary<ValueTuple<PotionBody, PotionOverlay>, string>();
			ValueTuple<PotionBody, PotionOverlay> valueTuple = new ValueTuple<PotionBody, PotionOverlay>(PotionBody.Cube, PotionOverlay.Bubbles);
			dictionary[valueTuple] = "potion_cube_bubbles.png";
			ValueTuple<PotionBody, PotionOverlay> valueTuple2 = new ValueTuple<PotionBody, PotionOverlay>(PotionBody.Cube, PotionOverlay.Curve);
			dictionary[valueTuple2] = "potion_cube_curve.png";
			ValueTuple<PotionBody, PotionOverlay> valueTuple3 = new ValueTuple<PotionBody, PotionOverlay>(PotionBody.Diamond, PotionOverlay.Bubbles);
			dictionary[valueTuple3] = "potion_diamond_bubbles.png";
			ValueTuple<PotionBody, PotionOverlay> valueTuple4 = new ValueTuple<PotionBody, PotionOverlay>(PotionBody.Fairy, PotionOverlay.Sparkle);
			dictionary[valueTuple4] = "potion_fairy_sparkle.png";
			ValueTuple<PotionBody, PotionOverlay> valueTuple5 = new ValueTuple<PotionBody, PotionOverlay>(PotionBody.FatDiamond, PotionOverlay.Curve);
			dictionary[valueTuple5] = "potion_fat_diamond_curve.png";
			ValueTuple<PotionBody, PotionOverlay> valueTuple6 = new ValueTuple<PotionBody, PotionOverlay>(PotionBody.Heart, PotionOverlay.Curve);
			dictionary[valueTuple6] = "potion_heart_curve.png";
			ValueTuple<PotionBody, PotionOverlay> valueTuple7 = new ValueTuple<PotionBody, PotionOverlay>(PotionBody.Spiky, PotionOverlay.Curve);
			dictionary[valueTuple7] = "potion_spiky_curve.png";
			PotionBodyExtensions._potionOverlayMap = dictionary;
		}

		// Token: 0x04002C1A RID: 11290
		[Nullable(new byte[] { 1, 0, 1 })]
		private static readonly Dictionary<ValueTuple<PotionBody, PotionOverlay>, string> _potionOverlayMap;
	}
}

using System;
using System.Runtime.CompilerServices;
using Godot;

namespace MegaCrit.Sts2.Core.Helpers
{
	// Token: 0x02000C0B RID: 3083
	[NullableContext(1)]
	[Nullable(0)]
	public static class HandPosHelper
	{
		/// <summary>
		/// Returns the position of a card when given the total number of cards in
		/// the hand (handSize) and the index of the card you want the position of.
		/// </summary>
		/// <param name="handSize"></param>
		/// <param name="cardIndex"></param>
		/// <returns></returns>
		// Token: 0x06007FBB RID: 32699 RVA: 0x002B5824 File Offset: 0x002B3A24
		public static Vector2 GetPosition(int handSize, int cardIndex)
		{
			if (handSize - 1 >= HandPosHelper._cardPositionData.Length)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Hand size ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(handSize);
				defaultInterpolatedStringHandler.AppendLiteral(" is greater than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(HandPosHelper._cardPositionData.Length + 1);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (cardIndex >= HandPosHelper._cardPositionData[handSize - 1].Length)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Card index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(cardIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" is greater than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(HandPosHelper._cardPositionData[handSize - 1].Length);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return HandPosHelper._cardPositionData[handSize - 1][cardIndex];
		}

		/// <summary>
		/// Returns the angle of a card when given the total number of cards in
		/// the hand (handSize) and the index of the card you want the position of.
		/// </summary>
		/// <param name="handSize"></param>
		/// <param name="cardIndex"></param>
		/// <returns></returns>
		// Token: 0x06007FBC RID: 32700 RVA: 0x002B5904 File Offset: 0x002B3B04
		public static float GetAngle(int handSize, int cardIndex)
		{
			if (handSize - 1 >= HandPosHelper._cardPositionData.Length)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Hand size ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(handSize);
				defaultInterpolatedStringHandler.AppendLiteral(" is greater than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(HandPosHelper._cardPositionData.Length + 1);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (cardIndex >= HandPosHelper._cardPositionData[handSize - 1].Length)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Card index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(cardIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" is greater than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(HandPosHelper._cardPositionData[handSize - 1].Length);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return HandPosHelper._cardAngleData[handSize - 1][cardIndex];
		}

		/// <summary>
		/// Returns the expected Scale of a card should be based on how many cards are in your hand.
		/// </summary>
		/// <param name="handSize"></param>
		/// <returns></returns>
		// Token: 0x06007FBD RID: 32701 RVA: 0x002B59E0 File Offset: 0x002B3BE0
		public static Vector2 GetScale(int handSize)
		{
			float num;
			switch (handSize)
			{
			case 8:
				num = 0.95f;
				break;
			case 9:
				num = 0.9f;
				break;
			case 10:
				num = 0.85f;
				break;
			case 11:
				num = 0.8f;
				break;
			case 12:
				num = 0.75f;
				break;
			default:
				num = 1f;
				break;
			}
			float num2 = num;
			return HandPosHelper._baseScale * num2;
		}

		// Token: 0x04002A86 RID: 10886
		private const float _offsetY = -50f;

		// Token: 0x04002A87 RID: 10887
		private static readonly Vector2[][] _cardPositionData = new Vector2[][]
		{
			new Vector2[]
			{
				new Vector2(0f, -50f)
			},
			new Vector2[]
			{
				new Vector2(-100f, -50f),
				new Vector2(100f, -50f)
			},
			new Vector2[]
			{
				new Vector2(-180f, -50f),
				new Vector2(0f, -59f),
				new Vector2(180f, -50f)
			},
			new Vector2[]
			{
				new Vector2(-240f, -25f),
				new Vector2(-80f, -50f),
				new Vector2(80f, -50f),
				new Vector2(240f, -25f)
			},
			new Vector2[]
			{
				new Vector2(-340f, 10f),
				new Vector2(-170f, -30f),
				new Vector2(0f, -50f),
				new Vector2(170f, -30f),
				new Vector2(340f, 10f)
			},
			new Vector2[]
			{
				new Vector2(-460f, 13f),
				new Vector2(-273f, -25f),
				new Vector2(-90f, -50f),
				new Vector2(90f, -50f),
				new Vector2(273f, -25f),
				new Vector2(460f, 13f)
			},
			new Vector2[]
			{
				new Vector2(-534f, 18f),
				new Vector2(-365f, -14f),
				new Vector2(-189f, -39f),
				new Vector2(0f, -50f),
				new Vector2(189f, -39f),
				new Vector2(365f, -14f),
				new Vector2(534f, 18f)
			},
			new Vector2[]
			{
				new Vector2(-565f, 28f),
				new Vector2(-400f, -14f),
				new Vector2(-231f, -39f),
				new Vector2(-80f, -50f),
				new Vector2(80f, -50f),
				new Vector2(231f, -39f),
				new Vector2(400f, -14f),
				new Vector2(565f, 28f)
			},
			new Vector2[]
			{
				new Vector2(-600f, 37f),
				new Vector2(-445f, -2f),
				new Vector2(-300f, -29f),
				new Vector2(-150f, -45f),
				new Vector2(0f, -50f),
				new Vector2(150f, -45f),
				new Vector2(300f, -29f),
				new Vector2(445f, -2f),
				new Vector2(600f, 37f)
			},
			new Vector2[]
			{
				new Vector2(-610f, 38f),
				new Vector2(-472f, 5f),
				new Vector2(-340f, -21f),
				new Vector2(-200f, -41f),
				new Vector2(-64f, -50f),
				new Vector2(64f, -50f),
				new Vector2(200f, -41f),
				new Vector2(340f, -21f),
				new Vector2(472f, 5f),
				new Vector2(610f, 38f)
			}
		};

		// Token: 0x04002A88 RID: 10888
		private static readonly float[][] _cardAngleData = new float[][]
		{
			new float[1],
			new float[] { -2f, 2f },
			new float[] { -3f, 0f, 3f },
			new float[] { -8f, -4f, 4f, 8f },
			new float[] { -8f, -4f, 0f, 4f, 8f },
			new float[] { -9f, -6f, -3f, 3f, 6f, 9f },
			new float[] { -9f, -6f, -3f, 0f, 3f, 6f, 9f },
			new float[] { -12f, -9f, -6f, -3f, 3f, 6f, 9f, 12f },
			new float[] { -12f, -9f, -6f, -3f, 0f, 3f, 6f, 9f, 12f },
			new float[] { -15f, -12f, -9f, -6f, -3f, 3f, 6f, 9f, 12f, 15f }
		};

		// Token: 0x04002A89 RID: 10889
		private static readonly Vector2 _baseScale = Vector2.One * 0.8f;
	}
}

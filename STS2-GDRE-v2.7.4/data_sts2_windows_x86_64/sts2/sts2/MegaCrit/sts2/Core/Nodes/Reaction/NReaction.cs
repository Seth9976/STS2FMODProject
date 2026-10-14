using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Nodes.Reaction
{
	// Token: 0x0200037A RID: 890
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Reaction/NReaction.cs")]
	public class NReaction : TextureRect
	{
		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x060034B8 RID: 13496 RVA: 0x00188A70 File Offset: 0x00186C70
		public ReactionType Type
		{
			get
			{
				return NReaction.TextureToType(base.Texture);
			}
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x00188A80 File Offset: 0x00186C80
		public static NReaction Create(Texture2D reactionTexture)
		{
			NReaction nreaction = PreloadManager.Cache.GetScene(NReaction._scenePath).Instantiate<NReaction>(0L);
			nreaction.Texture = reactionTexture;
			return nreaction;
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x00188AAC File Offset: 0x00186CAC
		public static NReaction Create(ReactionType type)
		{
			return NReaction.Create(NReaction.TypeToTexture(type));
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x00188AB9 File Offset: 0x00186CB9
		public void BeginAnim()
		{
			TaskHelper.RunSafely(this.DoAnim());
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x00188AC8 File Offset: 0x00186CC8
		private async Task DoAnim()
		{
			Color modulate = base.Modulate;
			modulate.A = 0f;
			base.Modulate = modulate;
			float num = Rng.Chaotic.NextFloat(40f, 60f);
			float num2 = Rng.Chaotic.NextFloat(-30f, 30f);
			Vector2 vector = base.Position + Vector2.Up.Rotated(Mathf.DegToRad(num2)) * num;
			Tween tween = base.CreateTween();
			tween.SetParallel(true);
			tween.TweenProperty(this, "position", vector, 0.30000001192092896).SetEase(1L).SetTrans(5L);
			tween.TweenProperty(this, "modulate:a", 1f, 0.30000001192092896).SetEase(1L).SetTrans(5L);
			tween.SetParallel(false);
			tween.TweenProperty(this, "modulate:a", 0f, 0.20000000298023224).SetDelay(0.6000000238418579).SetEase(0L)
				.SetTrans(5L);
			bool flag = await tween.AwaitFinished(this);
			this.QueueFreeSafely();
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x00188B0C File Offset: 0x00186D0C
		private static Texture2D TypeToTexture(ReactionType type)
		{
			AssetCache cache = PreloadManager.Cache;
			string text;
			switch (type)
			{
			case ReactionType.Exclamation:
				text = "res://images/ui/emote/exclaim.png";
				break;
			case ReactionType.Skull:
				text = "res://images/ui/emote/skull.png";
				break;
			case ReactionType.ThumbDown:
				text = "res://images/ui/emote/thumb_down.png";
				break;
			case ReactionType.SadSlime:
				text = "res://images/ui/emote/slime_sad.png";
				break;
			case ReactionType.QuestionMark:
				text = "res://images/ui/emote/question.png";
				break;
			case ReactionType.Heart:
				text = "res://images/ui/emote/heart.png";
				break;
			case ReactionType.ThumbUp:
				text = "res://images/ui/emote/thumb_up.png";
				break;
			case ReactionType.HappyCultist:
				text = "res://images/ui/emote/happy_cultist.png";
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			return cache.GetTexture2D(text);
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x00188BA4 File Offset: 0x00186DA4
		private static ReactionType TextureToType(Texture2D texture)
		{
			string resourcePath = texture.ResourcePath;
			if (resourcePath != null)
			{
				ReactionType reactionType;
				switch (resourcePath.Length)
				{
				case 31:
				{
					char c = resourcePath[22];
					if (c != 'h')
					{
						if (c != 's')
						{
							goto IL_0111;
						}
						if (!(resourcePath == "res://images/ui/emote/skull.png"))
						{
							goto IL_0111;
						}
						reactionType = ReactionType.Skull;
					}
					else
					{
						if (!(resourcePath == "res://images/ui/emote/heart.png"))
						{
							goto IL_0111;
						}
						reactionType = ReactionType.Heart;
					}
					break;
				}
				case 32:
				case 37:
				case 38:
					goto IL_0111;
				case 33:
					if (!(resourcePath == "res://images/ui/emote/exclaim.png"))
					{
						goto IL_0111;
					}
					reactionType = ReactionType.Exclamation;
					break;
				case 34:
				{
					char c = resourcePath[22];
					if (c != 'q')
					{
						if (c != 't')
						{
							goto IL_0111;
						}
						if (!(resourcePath == "res://images/ui/emote/thumb_up.png"))
						{
							goto IL_0111;
						}
						reactionType = ReactionType.ThumbUp;
					}
					else
					{
						if (!(resourcePath == "res://images/ui/emote/question.png"))
						{
							goto IL_0111;
						}
						reactionType = ReactionType.QuestionMark;
					}
					break;
				}
				case 35:
					if (!(resourcePath == "res://images/ui/emote/slime_sad.png"))
					{
						goto IL_0111;
					}
					reactionType = ReactionType.SadSlime;
					break;
				case 36:
					if (!(resourcePath == "res://images/ui/emote/thumb_down.png"))
					{
						goto IL_0111;
					}
					reactionType = ReactionType.ThumbDown;
					break;
				case 39:
					if (!(resourcePath == "res://images/ui/emote/happy_cultist.png"))
					{
						goto IL_0111;
					}
					reactionType = ReactionType.HappyCultist;
					break;
				default:
					goto IL_0111;
				}
				return reactionType;
			}
			IL_0111:
			throw new ArgumentOutOfRangeException("texture", texture, null);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060034BF RID: 13503 RVA: 0x00188CD0 File Offset: 0x00186ED0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NReaction.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("TextureRect"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "reactionTexture", 0L, "", 6L, new StringName("Texture2D"), false)
				}, null),
				new MethodInfo(NReaction.MethodName.BeginAnim, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NReaction.MethodName.TypeToTexture, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Texture2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "type", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NReaction.MethodName.TextureToType, new PropertyInfo(2L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "texture", 0L, "", 6L, new StringName("Texture2D"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060034C0 RID: 13504 RVA: 0x00188E40 File Offset: 0x00187040
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NReaction.MethodName.Create && args.Count == 1)
			{
				NReaction nreaction = NReaction.Create(VariantUtils.ConvertTo<Texture2D>(args[0]));
				ret = VariantUtils.CreateFrom<NReaction>(ref nreaction);
				return true;
			}
			if ((ref method) == NReaction.MethodName.BeginAnim && args.Count == 0)
			{
				this.BeginAnim();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NReaction.MethodName.TypeToTexture && args.Count == 1)
			{
				Texture2D texture2D = NReaction.TypeToTexture(VariantUtils.ConvertTo<ReactionType>(args[0]));
				ret = VariantUtils.CreateFrom<Texture2D>(ref texture2D);
				return true;
			}
			if ((ref method) == NReaction.MethodName.TextureToType && args.Count == 1)
			{
				ReactionType reactionType = NReaction.TextureToType(VariantUtils.ConvertTo<Texture2D>(args[0]));
				ret = VariantUtils.CreateFrom<ReactionType>(ref reactionType);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x00188F28 File Offset: 0x00187128
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NReaction.MethodName.Create && args.Count == 1)
			{
				NReaction nreaction = NReaction.Create(VariantUtils.ConvertTo<Texture2D>(args[0]));
				ret = VariantUtils.CreateFrom<NReaction>(ref nreaction);
				return true;
			}
			if ((ref method) == NReaction.MethodName.TypeToTexture && args.Count == 1)
			{
				Texture2D texture2D = NReaction.TypeToTexture(VariantUtils.ConvertTo<ReactionType>(args[0]));
				ret = VariantUtils.CreateFrom<Texture2D>(ref texture2D);
				return true;
			}
			if ((ref method) == NReaction.MethodName.TextureToType && args.Count == 1)
			{
				ReactionType reactionType = NReaction.TextureToType(VariantUtils.ConvertTo<Texture2D>(args[0]));
				ret = VariantUtils.CreateFrom<ReactionType>(ref reactionType);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060034C2 RID: 13506 RVA: 0x00188FE8 File Offset: 0x001871E8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NReaction.MethodName.Create || (ref method) == NReaction.MethodName.BeginAnim || (ref method) == NReaction.MethodName.TypeToTexture || (ref method) == NReaction.MethodName.TextureToType || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060034C3 RID: 13507 RVA: 0x00189038 File Offset: 0x00187238
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NReaction.PropertyName.Type)
			{
				ReactionType type = this.Type;
				value = VariantUtils.CreateFrom<ReactionType>(ref type);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060034C4 RID: 13508 RVA: 0x00189070 File Offset: 0x00187270
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(2L, NReaction.PropertyName.Type, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060034C5 RID: 13509 RVA: 0x001890A4 File Offset: 0x001872A4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x060034C6 RID: 13510 RVA: 0x001890AD File Offset: 0x001872AD
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04001473 RID: 5235
		private static readonly string _scenePath = SceneHelper.GetScenePath("ui/reaction");

		// Token: 0x04001474 RID: 5236
		private const string _exclamationPath = "res://images/ui/emote/exclaim.png";

		// Token: 0x04001475 RID: 5237
		private const string _skullPath = "res://images/ui/emote/skull.png";

		// Token: 0x04001476 RID: 5238
		private const string _thumbDownPath = "res://images/ui/emote/thumb_down.png";

		// Token: 0x04001477 RID: 5239
		private const string _sadSlimePath = "res://images/ui/emote/slime_sad.png";

		// Token: 0x04001478 RID: 5240
		private const string _questionMarkPath = "res://images/ui/emote/question.png";

		// Token: 0x04001479 RID: 5241
		private const string _heartPath = "res://images/ui/emote/heart.png";

		// Token: 0x0400147A RID: 5242
		private const string _thumbUpPath = "res://images/ui/emote/thumb_up.png";

		// Token: 0x0400147B RID: 5243
		private const string _happyCultistPath = "res://images/ui/emote/happy_cultist.png";

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016D7 RID: 5847
		[NullableContext(0)]
		public class MethodName : TextureRect.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04005906 RID: 22790
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'BeginAnim' method.
			/// </summary>
			// Token: 0x04005907 RID: 22791
			public static readonly StringName BeginAnim = "BeginAnim";

			/// <summary>
			/// Cached name for the 'TypeToTexture' method.
			/// </summary>
			// Token: 0x04005908 RID: 22792
			public static readonly StringName TypeToTexture = "TypeToTexture";

			/// <summary>
			/// Cached name for the 'TextureToType' method.
			/// </summary>
			// Token: 0x04005909 RID: 22793
			public static readonly StringName TextureToType = "TextureToType";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016D8 RID: 5848
		[NullableContext(0)]
		public class PropertyName : TextureRect.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Type' property.
			/// </summary>
			// Token: 0x0400590A RID: 22794
			public static readonly StringName Type = "Type";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016D9 RID: 5849
		[NullableContext(0)]
		public class SignalName : TextureRect.SignalName
		{
		}
	}
}

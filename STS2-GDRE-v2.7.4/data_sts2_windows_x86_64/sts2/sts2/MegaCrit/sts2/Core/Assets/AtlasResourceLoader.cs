using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;

namespace MegaCrit.Sts2.Core.Assets
{
	/// <summary>
	/// ResourceFormatLoader that intercepts .sprites/ paths and returns AtlasTextures from AtlasManager.
	/// Handles paths like: res://images/atlases/relic_atlas.sprites/anchor.tres
	/// </summary>
	// Token: 0x02000D92 RID: 3474
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Assets/AtlasResourceLoader.cs")]
	public class AtlasResourceLoader : ResourceFormatLoader
	{
		// Token: 0x06008BB4 RID: 35764 RVA: 0x002EA5F2 File Offset: 0x002E87F2
		public override string[] _GetRecognizedExtensions()
		{
			return new string[] { "tres" };
		}

		// Token: 0x06008BB5 RID: 35765 RVA: 0x002EA602 File Offset: 0x002E8802
		public override bool _HandlesType(StringName type)
		{
			return type == AtlasResourceLoader._typeAtlasTexture || type == AtlasResourceLoader._typeTexture2D || type == AtlasResourceLoader._typeResource;
		}

		// Token: 0x06008BB6 RID: 35766 RVA: 0x002EA62B File Offset: 0x002E882B
		public override string _GetResourceType(string path)
		{
			if (AtlasResourceLoader.IsSpritePath(path))
			{
				return "AtlasTexture";
			}
			return "";
		}

		// Token: 0x06008BB7 RID: 35767 RVA: 0x002EA640 File Offset: 0x002E8840
		public override bool _RecognizePath(string path, StringName type)
		{
			return AtlasResourceLoader.IsSpritePath(path);
		}

		// Token: 0x06008BB8 RID: 35768 RVA: 0x002EA648 File Offset: 0x002E8848
		public override bool _Exists(string path)
		{
			if (!AtlasResourceLoader.IsSpritePath(path))
			{
				return false;
			}
			ValueTuple<string, string> valueTuple = AtlasResourceLoader.ParsePath(path);
			string item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			if (item == null || item2 == null)
			{
				return false;
			}
			if (!AtlasManager.IsAtlasLoaded(item))
			{
				AtlasManager.LoadAtlas(item);
			}
			return AtlasManager.HasSprite(item, item2) || AtlasResourceLoader.HasFallback(item, item2);
		}

		// Token: 0x06008BB9 RID: 35769 RVA: 0x002EA69C File Offset: 0x002E889C
		public override Variant _Load(string path, string originalPath, bool useSubThreads, int cacheMode)
		{
			if (!AtlasResourceLoader.IsSpritePath(path))
			{
				return default(Variant);
			}
			ValueTuple<string, string> valueTuple = AtlasResourceLoader.ParsePath(path);
			string item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			if (item == null || item2 == null)
			{
				Log.Warn("AtlasResourceLoader: Failed to parse path: " + path, 2);
				return 7L;
			}
			if (!AtlasManager.IsAtlasLoaded(item))
			{
				AtlasManager.LoadAtlas(item);
			}
			AtlasTexture sprite = AtlasManager.GetSprite(item, item2);
			if (sprite != null)
			{
				return sprite;
			}
			Texture2D texture2D = AtlasResourceLoader.LoadFallback(item, item2);
			if (texture2D != null)
			{
				return texture2D;
			}
			if (!item2.StartsWith("mock_"))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 3);
				defaultInterpolatedStringHandler.AppendLiteral("AtlasResourceLoader: Missing sprite '");
				defaultInterpolatedStringHandler.AppendFormatted(item2);
				defaultInterpolatedStringHandler.AppendLiteral("' in ");
				defaultInterpolatedStringHandler.AppendFormatted(item);
				defaultInterpolatedStringHandler.AppendLiteral(" (requested: ");
				defaultInterpolatedStringHandler.AppendFormatted(path);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				Log.Warn(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			}
			return AtlasResourceLoader.GetMissingTexture(item);
		}

		// Token: 0x06008BBA RID: 35770 RVA: 0x002EA796 File Offset: 0x002E8996
		public override string[] _GetDependencies(string path, bool addTypes)
		{
			return Array.Empty<string>();
		}

		// Token: 0x06008BBB RID: 35771 RVA: 0x002EA79D File Offset: 0x002E899D
		private static bool IsSpritePath(string path)
		{
			return path.StartsWith("res://images/atlases/") && path.Contains(".sprites/") && path.EndsWith(".tres");
		}

		/// <summary>
		/// Parses a sprite path to extract atlas and sprite names.
		/// Example: res://images/atlases/relic_atlas.sprites/anchor.tres
		/// Returns: ("relic_atlas", "anchor")
		/// </summary>
		// Token: 0x06008BBC RID: 35772 RVA: 0x002EA7C8 File Offset: 0x002E89C8
		[return: TupleElementNames(new string[] { "AtlasName", "SpriteName" })]
		[return: Nullable(new byte[] { 0, 2, 2 })]
		public static ValueTuple<string, string> ParsePath(string path)
		{
			Match match = AtlasResourceLoader._pathPattern.Match(path);
			if (!match.Success)
			{
				return new ValueTuple<string, string>(null, null);
			}
			return new ValueTuple<string, string>(match.Groups[1].Value, match.Groups[2].Value);
		}

		// Token: 0x06008BBD RID: 35773 RVA: 0x002EA818 File Offset: 0x002E8A18
		private static bool HasFallback(string atlasName, string spriteName)
		{
			string fallbackPath = AtlasResourceLoader.GetFallbackPath(atlasName, spriteName);
			return fallbackPath != null && ResourceLoader.Exists(fallbackPath, "");
		}

		// Token: 0x06008BBE RID: 35774 RVA: 0x002EA840 File Offset: 0x002E8A40
		[return: Nullable(2)]
		private static Texture2D LoadFallback(string atlasName, string spriteName)
		{
			string fallbackPath = AtlasResourceLoader.GetFallbackPath(atlasName, spriteName);
			if (fallbackPath == null)
			{
				return null;
			}
			if (!ResourceLoader.Exists(fallbackPath, ""))
			{
				return null;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 3);
			defaultInterpolatedStringHandler.AppendLiteral("AtlasResourceLoader: Using fallback for ");
			defaultInterpolatedStringHandler.AppendFormatted(atlasName);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted(spriteName);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(fallbackPath);
			Log.Debug(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
			return ResourceLoader.Load<Texture2D>(fallbackPath, null, 1L);
		}

		/// <summary>
		/// Returns the fallback path for a sprite not found in the atlas.
		/// Different atlases have different fallback conventions.
		/// </summary>
		// Token: 0x06008BBF RID: 35775 RVA: 0x002EA8C8 File Offset: 0x002E8AC8
		[return: Nullable(2)]
		private static string GetFallbackPath(string atlasName, string spriteName)
		{
			string text;
			if (!(atlasName == "relic_atlas") && !(atlasName == "relic_outline_atlas"))
			{
				if (!(atlasName == "power_atlas"))
				{
					if (!(atlasName == "card_atlas"))
					{
						if (!(atlasName == "potion_atlas") && !(atlasName == "potion_outline_atlas"))
						{
							text = null;
						}
						else
						{
							text = AtlasResourceLoader.GetPotionFallbackPath(spriteName);
						}
					}
					else
					{
						text = AtlasResourceLoader.GetCardFallbackPath(spriteName);
					}
				}
				else
				{
					text = AtlasResourceLoader.GetPowerFallbackPath(spriteName);
				}
			}
			else
			{
				text = AtlasResourceLoader.GetRelicFallbackPath(spriteName);
			}
			return text;
		}

		// Token: 0x06008BC0 RID: 35776 RVA: 0x002EA94C File Offset: 0x002E8B4C
		[return: Nullable(2)]
		private static string GetRelicFallbackPath(string spriteName)
		{
			string text = "res://images/relics/" + spriteName + ".png";
			if (ResourceLoader.Exists(text, ""))
			{
				return text;
			}
			string text2 = "res://images/relics/beta/" + spriteName + ".png";
			if (ResourceLoader.Exists(text2, ""))
			{
				return text2;
			}
			return null;
		}

		// Token: 0x06008BC1 RID: 35777 RVA: 0x002EA99C File Offset: 0x002E8B9C
		[return: Nullable(2)]
		private static string GetPowerFallbackPath(string spriteName)
		{
			string text = "res://images/powers/" + spriteName + ".png";
			if (ResourceLoader.Exists(text, ""))
			{
				return text;
			}
			string text2 = "res://images/powers/beta/" + spriteName + ".png";
			if (ResourceLoader.Exists(text2, ""))
			{
				return text2;
			}
			return null;
		}

		// Token: 0x06008BC2 RID: 35778 RVA: 0x002EA9EC File Offset: 0x002E8BEC
		[return: Nullable(2)]
		private static string GetCardFallbackPath(string spriteName)
		{
			string text = "res://images/packed/card_portraits/" + spriteName + ".png";
			if (ResourceLoader.Exists(text, ""))
			{
				return text;
			}
			int num = spriteName.LastIndexOf('/');
			if (num > 0)
			{
				string text2 = spriteName.Substring(0, num);
				int num2 = num + 1;
				string text3 = spriteName.Substring(num2, spriteName.Length - num2);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 2);
				defaultInterpolatedStringHandler.AppendLiteral("res://images/packed/card_portraits/");
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				defaultInterpolatedStringHandler.AppendLiteral("/beta/");
				defaultInterpolatedStringHandler.AppendFormatted(text3);
				defaultInterpolatedStringHandler.AppendLiteral(".png");
				string text4 = defaultInterpolatedStringHandler.ToStringAndClear();
				if (ResourceLoader.Exists(text4, ""))
				{
					return text4;
				}
			}
			return null;
		}

		// Token: 0x06008BC3 RID: 35779 RVA: 0x002EAAA4 File Offset: 0x002E8CA4
		[return: Nullable(2)]
		private static string GetPotionFallbackPath(string spriteName)
		{
			string text = "res://images/potions/" + spriteName + ".png";
			if (ResourceLoader.Exists(text, ""))
			{
				return text;
			}
			return null;
		}

		// Token: 0x06008BC4 RID: 35780 RVA: 0x002EAAD4 File Offset: 0x002E8CD4
		private static Variant GetMissingTexture(string atlasName)
		{
			string text;
			if (!(atlasName == "card_atlas"))
			{
				if (!(atlasName == "potion_atlas") && !(atlasName == "potion_outline_atlas"))
				{
					text = "res://images/powers/missing_power.png";
				}
				else
				{
					text = "res://images/potions/missing_potion.png";
				}
			}
			else
			{
				text = "res://images/packed/card_portraits/beta.png";
			}
			string text2 = text;
			if (ResourceLoader.Exists(text2, ""))
			{
				Texture2D texture2D = ResourceLoader.Load<Texture2D>(text2, null, 1L);
				if (texture2D != null)
				{
					return texture2D;
				}
			}
			return 7L;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06008BC5 RID: 35781 RVA: 0x002EAB4C File Offset: 0x002E8D4C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(16)
			{
				new MethodInfo(AtlasResourceLoader.MethodName._GetRecognizedExtensions, new PropertyInfo(34L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(AtlasResourceLoader.MethodName._HandlesType, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(21L, "type", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName._GetResourceType, new PropertyInfo(4L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName._RecognizePath, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false),
					new PropertyInfo(21L, "type", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName._Exists, new PropertyInfo(1L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName._Load, new PropertyInfo(0L, "", 0L, "", 131078L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false),
					new PropertyInfo(4L, "originalPath", 0L, "", 6L, false),
					new PropertyInfo(1L, "useSubThreads", 0L, "", 6L, false),
					new PropertyInfo(2L, "cacheMode", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName._GetDependencies, new PropertyInfo(34L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false),
					new PropertyInfo(1L, "addTypes", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName.IsSpritePath, new PropertyInfo(1L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "path", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName.HasFallback, new PropertyInfo(1L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "atlasName", 0L, "", 6L, false),
					new PropertyInfo(4L, "spriteName", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName.LoadFallback, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Texture2D"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "atlasName", 0L, "", 6L, false),
					new PropertyInfo(4L, "spriteName", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName.GetFallbackPath, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "atlasName", 0L, "", 6L, false),
					new PropertyInfo(4L, "spriteName", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName.GetRelicFallbackPath, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "spriteName", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName.GetPowerFallbackPath, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "spriteName", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName.GetCardFallbackPath, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "spriteName", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName.GetPotionFallbackPath, new PropertyInfo(4L, "", 0L, "", 6L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "spriteName", 0L, "", 6L, false)
				}, null),
				new MethodInfo(AtlasResourceLoader.MethodName.GetMissingTexture, new PropertyInfo(0L, "", 0L, "", 131078L, false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(4L, "atlasName", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06008BC6 RID: 35782 RVA: 0x002EB1A8 File Offset: 0x002E93A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == AtlasResourceLoader.MethodName._GetRecognizedExtensions && args.Count == 0)
			{
				string[] array = this._GetRecognizedExtensions();
				ret = VariantUtils.CreateFrom<string[]>(ref array);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName._HandlesType && args.Count == 1)
			{
				bool flag = this._HandlesType(VariantUtils.ConvertTo<StringName>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName._GetResourceType && args.Count == 1)
			{
				string text = this._GetResourceType(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref text);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName._RecognizePath && args.Count == 2)
			{
				bool flag2 = this._RecognizePath(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<StringName>(args[1]));
				ret = VariantUtils.CreateFrom<bool>(ref flag2);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName._Exists && args.Count == 1)
			{
				bool flag3 = this._Exists(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag3);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName._Load && args.Count == 4)
			{
				Variant variant = this._Load(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]), VariantUtils.ConvertTo<bool>(args[2]), VariantUtils.ConvertTo<int>(args[3]));
				ret = VariantUtils.CreateFrom<Variant>(ref variant);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName._GetDependencies && args.Count == 2)
			{
				string[] array2 = this._GetDependencies(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<bool>(args[1]));
				ret = VariantUtils.CreateFrom<string[]>(ref array2);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.IsSpritePath && args.Count == 1)
			{
				bool flag4 = AtlasResourceLoader.IsSpritePath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag4);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.HasFallback && args.Count == 2)
			{
				bool flag5 = AtlasResourceLoader.HasFallback(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = VariantUtils.CreateFrom<bool>(ref flag5);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.LoadFallback && args.Count == 2)
			{
				Texture2D texture2D = AtlasResourceLoader.LoadFallback(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = VariantUtils.CreateFrom<Texture2D>(ref texture2D);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.GetFallbackPath && args.Count == 2)
			{
				string fallbackPath = AtlasResourceLoader.GetFallbackPath(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = VariantUtils.CreateFrom<string>(ref fallbackPath);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.GetRelicFallbackPath && args.Count == 1)
			{
				string relicFallbackPath = AtlasResourceLoader.GetRelicFallbackPath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref relicFallbackPath);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.GetPowerFallbackPath && args.Count == 1)
			{
				string powerFallbackPath = AtlasResourceLoader.GetPowerFallbackPath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref powerFallbackPath);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.GetCardFallbackPath && args.Count == 1)
			{
				string cardFallbackPath = AtlasResourceLoader.GetCardFallbackPath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref cardFallbackPath);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.GetPotionFallbackPath && args.Count == 1)
			{
				string potionFallbackPath = AtlasResourceLoader.GetPotionFallbackPath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref potionFallbackPath);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.GetMissingTexture && args.Count == 1)
			{
				Variant missingTexture = AtlasResourceLoader.GetMissingTexture(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<Variant>(ref missingTexture);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		// Token: 0x06008BC7 RID: 35783 RVA: 0x002EB5BC File Offset: 0x002E97BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == AtlasResourceLoader.MethodName.IsSpritePath && args.Count == 1)
			{
				bool flag = AtlasResourceLoader.IsSpritePath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<bool>(ref flag);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.HasFallback && args.Count == 2)
			{
				bool flag2 = AtlasResourceLoader.HasFallback(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = VariantUtils.CreateFrom<bool>(ref flag2);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.LoadFallback && args.Count == 2)
			{
				Texture2D texture2D = AtlasResourceLoader.LoadFallback(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = VariantUtils.CreateFrom<Texture2D>(ref texture2D);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.GetFallbackPath && args.Count == 2)
			{
				string fallbackPath = AtlasResourceLoader.GetFallbackPath(VariantUtils.ConvertTo<string>(args[0]), VariantUtils.ConvertTo<string>(args[1]));
				ret = VariantUtils.CreateFrom<string>(ref fallbackPath);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.GetRelicFallbackPath && args.Count == 1)
			{
				string relicFallbackPath = AtlasResourceLoader.GetRelicFallbackPath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref relicFallbackPath);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.GetPowerFallbackPath && args.Count == 1)
			{
				string powerFallbackPath = AtlasResourceLoader.GetPowerFallbackPath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref powerFallbackPath);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.GetCardFallbackPath && args.Count == 1)
			{
				string cardFallbackPath = AtlasResourceLoader.GetCardFallbackPath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref cardFallbackPath);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.GetPotionFallbackPath && args.Count == 1)
			{
				string potionFallbackPath = AtlasResourceLoader.GetPotionFallbackPath(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<string>(ref potionFallbackPath);
				return true;
			}
			if ((ref method) == AtlasResourceLoader.MethodName.GetMissingTexture && args.Count == 1)
			{
				Variant missingTexture = AtlasResourceLoader.GetMissingTexture(VariantUtils.ConvertTo<string>(args[0]));
				ret = VariantUtils.CreateFrom<Variant>(ref missingTexture);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06008BC8 RID: 35784 RVA: 0x002EB800 File Offset: 0x002E9A00
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == AtlasResourceLoader.MethodName._GetRecognizedExtensions || (ref method) == AtlasResourceLoader.MethodName._HandlesType || (ref method) == AtlasResourceLoader.MethodName._GetResourceType || (ref method) == AtlasResourceLoader.MethodName._RecognizePath || (ref method) == AtlasResourceLoader.MethodName._Exists || (ref method) == AtlasResourceLoader.MethodName._Load || (ref method) == AtlasResourceLoader.MethodName._GetDependencies || (ref method) == AtlasResourceLoader.MethodName.IsSpritePath || (ref method) == AtlasResourceLoader.MethodName.HasFallback || (ref method) == AtlasResourceLoader.MethodName.LoadFallback || (ref method) == AtlasResourceLoader.MethodName.GetFallbackPath || (ref method) == AtlasResourceLoader.MethodName.GetRelicFallbackPath || (ref method) == AtlasResourceLoader.MethodName.GetPowerFallbackPath || (ref method) == AtlasResourceLoader.MethodName.GetCardFallbackPath || (ref method) == AtlasResourceLoader.MethodName.GetPotionFallbackPath || (ref method) == AtlasResourceLoader.MethodName.GetMissingTexture || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06008BC9 RID: 35785 RVA: 0x002EB904 File Offset: 0x002E9B04
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06008BCA RID: 35786 RVA: 0x002EB90D File Offset: 0x002E9B0D
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x040030C4 RID: 12484
		private const string _atlasBasePath = "res://images/atlases/";

		// Token: 0x040030C5 RID: 12485
		private const string _spritesSuffix = ".sprites/";

		// Token: 0x040030C6 RID: 12486
		private static readonly StringName _typeAtlasTexture = new StringName("AtlasTexture");

		// Token: 0x040030C7 RID: 12487
		private static readonly StringName _typeTexture2D = new StringName("Texture2D");

		// Token: 0x040030C8 RID: 12488
		private static readonly StringName _typeResource = new StringName("Resource");

		// Token: 0x040030C9 RID: 12489
		private static readonly Regex _pathPattern = new Regex("^res://images/atlases/([^/]+)\\.sprites/(.+)\\.tres$", RegexOptions.Compiled);

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002561 RID: 9569
		[NullableContext(0)]
		public class MethodName : ResourceFormatLoader.MethodName
		{
			/// <summary>
			/// Cached name for the '_GetRecognizedExtensions' method.
			/// </summary>
			// Token: 0x04009FEC RID: 40940
			public static readonly StringName _GetRecognizedExtensions = "_GetRecognizedExtensions";

			/// <summary>
			/// Cached name for the '_HandlesType' method.
			/// </summary>
			// Token: 0x04009FED RID: 40941
			public static readonly StringName _HandlesType = "_HandlesType";

			/// <summary>
			/// Cached name for the '_GetResourceType' method.
			/// </summary>
			// Token: 0x04009FEE RID: 40942
			public static readonly StringName _GetResourceType = "_GetResourceType";

			/// <summary>
			/// Cached name for the '_RecognizePath' method.
			/// </summary>
			// Token: 0x04009FEF RID: 40943
			public static readonly StringName _RecognizePath = "_RecognizePath";

			/// <summary>
			/// Cached name for the '_Exists' method.
			/// </summary>
			// Token: 0x04009FF0 RID: 40944
			public static readonly StringName _Exists = "_Exists";

			/// <summary>
			/// Cached name for the '_Load' method.
			/// </summary>
			// Token: 0x04009FF1 RID: 40945
			public static readonly StringName _Load = "_Load";

			/// <summary>
			/// Cached name for the '_GetDependencies' method.
			/// </summary>
			// Token: 0x04009FF2 RID: 40946
			public static readonly StringName _GetDependencies = "_GetDependencies";

			/// <summary>
			/// Cached name for the 'IsSpritePath' method.
			/// </summary>
			// Token: 0x04009FF3 RID: 40947
			public static readonly StringName IsSpritePath = "IsSpritePath";

			/// <summary>
			/// Cached name for the 'HasFallback' method.
			/// </summary>
			// Token: 0x04009FF4 RID: 40948
			public static readonly StringName HasFallback = "HasFallback";

			/// <summary>
			/// Cached name for the 'LoadFallback' method.
			/// </summary>
			// Token: 0x04009FF5 RID: 40949
			public static readonly StringName LoadFallback = "LoadFallback";

			/// <summary>
			/// Cached name for the 'GetFallbackPath' method.
			/// </summary>
			// Token: 0x04009FF6 RID: 40950
			public static readonly StringName GetFallbackPath = "GetFallbackPath";

			/// <summary>
			/// Cached name for the 'GetRelicFallbackPath' method.
			/// </summary>
			// Token: 0x04009FF7 RID: 40951
			public static readonly StringName GetRelicFallbackPath = "GetRelicFallbackPath";

			/// <summary>
			/// Cached name for the 'GetPowerFallbackPath' method.
			/// </summary>
			// Token: 0x04009FF8 RID: 40952
			public static readonly StringName GetPowerFallbackPath = "GetPowerFallbackPath";

			/// <summary>
			/// Cached name for the 'GetCardFallbackPath' method.
			/// </summary>
			// Token: 0x04009FF9 RID: 40953
			public static readonly StringName GetCardFallbackPath = "GetCardFallbackPath";

			/// <summary>
			/// Cached name for the 'GetPotionFallbackPath' method.
			/// </summary>
			// Token: 0x04009FFA RID: 40954
			public static readonly StringName GetPotionFallbackPath = "GetPotionFallbackPath";

			/// <summary>
			/// Cached name for the 'GetMissingTexture' method.
			/// </summary>
			// Token: 0x04009FFB RID: 40955
			public static readonly StringName GetMissingTexture = "GetMissingTexture";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002562 RID: 9570
		[NullableContext(0)]
		public class PropertyName : ResourceFormatLoader.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02002563 RID: 9571
		[NullableContext(0)]
		public class SignalName : ResourceFormatLoader.SignalName
		{
		}
	}
}

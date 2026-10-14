using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

namespace MegaCrit.Sts2.Core.Rooms
{
	// Token: 0x02000126 RID: 294
	[NullableContext(1)]
	[Nullable(0)]
	public class BackgroundAssets
	{
		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x0005B65F File Offset: 0x0005985F
		public string BackgroundScenePath { get; }

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x0005B667 File Offset: 0x00059867
		public List<string> BgLayers { get; }

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06000E5C RID: 3676 RVA: 0x0005B66F File Offset: 0x0005986F
		[Nullable(2)]
		public string FgLayer
		{
			[NullableContext(2)]
			get;
		}

		/// Creates the combat background for a particular encounter. If this encounter doesn't have a unique background,
		/// automatically pull the one for the act. Otherwise scrub through the the corresponding filepath and set up the
		/// background dynamically.
		/// file structure:
		/// -&gt;scenes/backgrounds/{title}
		/// --&gt; layers
		/// ---&gt; {title}_bg_00_a.tscn
		/// ---&gt; {title}_bg_01_a.tscn
		/// ---&gt; {title}_bg_01_b.tscn
		/// ---&gt; {title}_bg_02_a.tscn
		/// ---&gt; {title}_bg_02_b.tscn
		/// ---&gt; {title}_fg_a.tscn
		/// --&gt; {title}_background.tscn
		///
		/// notes:
		/// - the {title}_background.tscn file helps visually define how the layers are set up
		/// - the layer scenes (in the layer folder) are an asset optimization.
		/// - {title} must be exactly the same as the class name
		/// - layer files that share the same prefix (_bg_##, _fg_) are grouped together, and will be randomly chosen for the combat's background
		/// - you can have as many _bg_ layers as you want.
		/// - _bg_ layers are stacked from least to most
		/// - there is only 1 fg layer currently
		// Token: 0x06000E5D RID: 3677 RVA: 0x0005B678 File Offset: 0x00059878
		public BackgroundAssets(string title, Rng rng)
		{
			string text = "res://scenes/backgrounds/" + title + "/layers";
			using (DirAccess dirAccess = DirAccess.Open(text))
			{
				if (dirAccess == null)
				{
					throw new InvalidOperationException("could not find directory " + text);
				}
				Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
				List<string> list = new List<string>();
				dirAccess.ListDirBegin();
				string text2 = dirAccess.GetNext();
				while (text2 != "")
				{
					if (dirAccess.CurrentIsDir())
					{
						throw new InvalidOperationException("there should be no other directories within the layers directory");
					}
					if (text2.Contains("_fg_"))
					{
						list.Add(text + "/" + text2);
					}
					else
					{
						if (!text2.Contains("_bg_"))
						{
							throw new InvalidOperationException("files must either contain '_fg_' or '_bg_'");
						}
						string text3 = text2.Split("_bg_", StringSplitOptions.None)[1];
						string text4 = text3.Split("_", StringSplitOptions.None)[0];
						if (!dictionary.ContainsKey(text4))
						{
							dictionary.Add(text4, new List<string>());
						}
						dictionary[text4].Add(text + "/" + text2);
					}
					text2 = dirAccess.GetNext();
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
				defaultInterpolatedStringHandler.AppendLiteral("backgrounds/");
				defaultInterpolatedStringHandler.AppendFormatted(title);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(title);
				defaultInterpolatedStringHandler.AppendLiteral("_background");
				string scenePath = SceneHelper.GetScenePath(defaultInterpolatedStringHandler.ToStringAndClear());
				this.BackgroundScenePath = scenePath;
				this.BgLayers = BackgroundAssets.SelectRandomBackgroundAssetLayers(rng, dictionary);
				this.FgLayer = BackgroundAssets.SelectRandomForegroundAssetLayer(rng, list.ToArray());
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x0005B830 File Offset: 0x00059A30
		public IEnumerable<string> AssetPaths
		{
			get
			{
				return from s in new string[]
					{
						this.BackgroundScenePath,
						this.FgLayer ?? string.Empty
					}.Concat<string>(this.BgLayers)
					where !string.IsNullOrWhiteSpace(s)
					select s;
			}
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0005B890 File Offset: 0x00059A90
		private static List<string> SelectRandomBackgroundAssetLayers(Rng rng, Dictionary<string, List<string>> bgLayers)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, List<string>> keyValuePair in bgLayers.OrderBy<KeyValuePair<string, List<string>>, string>((KeyValuePair<string, List<string>> kv) => kv.Key))
			{
				string text;
				List<string> list2;
				keyValuePair.Deconstruct(out text, out list2);
				List<string> list3 = list2;
				string text2 = rng.NextItem<string>(list3);
				list.Add(text2);
			}
			return list;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0005B91C File Offset: 0x00059B1C
		[return: Nullable(2)]
		private static string SelectRandomForegroundAssetLayer(Rng rng, IEnumerable<string> fgLayer)
		{
			return rng.NextItem<string>(fgLayer);
		}
	}
}

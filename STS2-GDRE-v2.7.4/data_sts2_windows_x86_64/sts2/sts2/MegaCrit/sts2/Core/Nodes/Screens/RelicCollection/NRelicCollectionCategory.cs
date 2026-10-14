using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection
{
	// Token: 0x020002D7 RID: 727
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/RelicCollection/NRelicCollectionCategory.cs")]
	public class NRelicCollectionCategory : VBoxContainer
	{
		// Token: 0x060026B4 RID: 9908 RVA: 0x001077CC File Offset: 0x001059CC
		private NRelicCollectionCategory CreateForSubcategory()
		{
			return PreloadManager.Cache.GetScene(NRelicCollectionCategory.scenePath).Instantiate<NRelicCollectionCategory>(0L);
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x001077F4 File Offset: 0x001059F4
		public override void _Ready()
		{
			this._headerLabel = base.GetNode<MegaRichTextLabel>("%Header");
			this._icon = base.GetNode<TextureRect>("%Icon");
			this._relicsContainer = base.GetNode<GridContainer>("%RelicsContainer");
			this._spacer = base.GetNode<Control>("Spacer");
			this._icon.Visible = false;
		}

		/// <summary>
		/// Instantiates a relic category by sorting and categorizing relics by ancient, character specific, and unlock/seen status.
		/// </summary>
		// Token: 0x060026B6 RID: 9910 RVA: 0x00107868 File Offset: 0x00105A68
		public void LoadRelics(RelicRarity relicRarity, NRelicCollection collection, LocString header, HashSet<RelicModel> seenRelics, UnlockState unlockState, HashSet<RelicModel> allUnlockedRelics)
		{
			this._subCategories.Clear();
			this._headerLabel.Text = header.GetFormattedText();
			this._collection = collection;
			NRelicCollectionCategory._relicModelCache.Clear();
			NRelicCollectionCategory._relicModelCache.AddRange(ModelDb.AllRelics.Where<RelicModel>((RelicModel relic) => relic.Rarity == relicRarity));
			if (relicRarity == RelicRarity.Starter)
			{
				List<RelicModel> list = ModelDb.AllCharacters.SelectMany<CharacterModel, RelicModel>((CharacterModel c) => c.StartingRelics).ToList<RelicModel>();
				NRelicCollectionCategory nrelicCollectionCategory = this.CreateForSubcategory();
				this._subCategories.Add(nrelicCollectionCategory);
				this.AddChildSafely(nrelicCollectionCategory);
				this.MoveChildSafely(nrelicCollectionCategory, this._headerLabel.GetIndex(false) + 1);
				nrelicCollectionCategory._spacer.Visible = false;
				nrelicCollectionCategory._headerLabel.Visible = false;
				nrelicCollectionCategory.LoadSubcategory(this._collection, null, list, seenRelics, allUnlockedRelics);
				IEnumerable<RelicModel> enumerable = list.Select<RelicModel, RelicModel>((RelicModel r) => ModelDb.Relic<TouchOfOrobas>().GetUpgradedStarterRelic(r));
				NRelicCollectionCategory nrelicCollectionCategory2 = this.CreateForSubcategory();
				this._subCategories.Add(nrelicCollectionCategory2);
				this.AddChildSafely(nrelicCollectionCategory2);
				this.MoveChildSafely(nrelicCollectionCategory2, this._headerLabel.GetIndex(false) + 2);
				nrelicCollectionCategory2._headerLabel.Visible = false;
				nrelicCollectionCategory2.LoadSubcategory(this._collection, null, enumerable, seenRelics, allUnlockedRelics);
				return;
			}
			if (relicRarity == RelicRarity.Ancient)
			{
				List<ActModel> list2 = ModelDb.Acts.ToList<ActModel>();
				List<AncientEventModel> list3 = list2.Select<ActModel, IEnumerable<AncientEventModel>>((ActModel a) => a.AllAncients).SelectMany<IEnumerable<AncientEventModel>, AncientEventModel>((IEnumerable<AncientEventModel> a) => a).Concat<AncientEventModel>(ModelDb.AllSharedAncients)
					.Distinct<AncientEventModel>()
					.ToList<AncientEventModel>();
				HashSet<AncientEventModel> hashSet = list2.Select<ActModel, IEnumerable<AncientEventModel>>((ActModel a) => a.GetUnlockedAncients(unlockState)).SelectMany<IEnumerable<AncientEventModel>, AncientEventModel>((IEnumerable<AncientEventModel> a) => a).Concat<AncientEventModel>(unlockState.SharedAncients)
					.Distinct<AncientEventModel>()
					.ToHashSet<AncientEventModel>();
				IReadOnlyDictionary<ModelId, AncientStats> ancientStats = SaveManager.Instance.Progress.AncientStats;
				LocString locString = new LocString("relic_collection", "UNKNOWN_ANCIENT");
				Func<RelicModel, bool> <>9__9;
				for (int i = 0; i < list3.Count; i++)
				{
					AncientEventModel ancientEventModel = list3[i];
					if (hashSet.Contains(ancientEventModel))
					{
						NRelicCollectionCategory nrelicCollectionCategory3 = this.CreateForSubcategory();
						this._subCategories.Add(nrelicCollectionCategory3);
						this.AddChildSafely(nrelicCollectionCategory3);
						this.MoveChildSafely(nrelicCollectionCategory3, this._headerLabel.GetIndex(false) + i + 1);
						RelicModel[] array = ancientEventModel.AllPossibleOptions.Select<EventOption, RelicModel>(delegate(EventOption o)
						{
							RelicModel relic = o.Relic;
							if (relic == null)
							{
								return null;
							}
							return relic.CanonicalInstance;
						}).OfType<RelicModel>().Intersect<RelicModel>(NRelicCollectionCategory._relicModelCache)
							.OrderBy<RelicModel, string>((RelicModel r) => r.Title.GetFormattedText(), LocManager.Instance.StringComparer)
							.ToArray<RelicModel>();
						bool flag;
						if (!ancientStats.ContainsKey(ancientEventModel.Id))
						{
							IEnumerable<RelicModel> enumerable2 = array;
							Func<RelicModel, bool> func;
							if ((func = <>9__9) == null)
							{
								func = (<>9__9 = (RelicModel r) => seenRelics.Contains(r));
							}
							flag = enumerable2.Any<RelicModel>(func);
						}
						else
						{
							flag = true;
						}
						bool flag2 = flag;
						LocString locString2 = new LocString("relic_collection", "ANCIENT_SUBCATEGORY");
						locString2.Add("Ancient", flag2 ? ancientEventModel.Title : locString);
						nrelicCollectionCategory3.LoadSubcategory(this._collection, locString2, array, seenRelics, allUnlockedRelics);
						nrelicCollectionCategory3.LoadIcon(ancientEventModel.RunHistoryIcon);
					}
				}
				return;
			}
			List<RelicModel> list4 = new List<RelicModel>();
			List<RelicModel> list5 = new List<RelicModel>();
			foreach (RelicPoolModel relicPoolModel in ModelDb.AllCharacterRelicPools)
			{
				foreach (RelicModel relicModel in NRelicCollectionCategory._relicModelCache)
				{
					if (relicPoolModel.AllRelicIds.Contains(relicModel.Id))
					{
						list4.Add(relicModel);
					}
				}
			}
			foreach (RelicModel relicModel2 in NRelicCollectionCategory._relicModelCache)
			{
				if (!list4.Contains(relicModel2))
				{
					list5.Add(relicModel2);
				}
			}
			list5.Sort((RelicModel p1, RelicModel p2) => LocManager.Instance.StringComparer.Compare(p1.Title.GetFormattedText(), p2.Title.GetFormattedText()));
			this.LoadRelicNodes(list5.Concat<RelicModel>(list4), seenRelics, allUnlockedRelics);
			this._collection.AddRelics(list5);
			this._collection.AddRelics(list4);
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x00107D9C File Offset: 0x00105F9C
		private void LoadSubcategory(NRelicCollection collection, [Nullable(2)] LocString header, IEnumerable<RelicModel> relics, HashSet<RelicModel> seenRelics, HashSet<RelicModel> unlockedRelics)
		{
			this._headerLabel.Text = ((header != null) ? header.GetFormattedText() : null) ?? "";
			this._collection = collection;
			this._collection.AddRelics(relics);
			this.LoadRelicNodes(relics, seenRelics, unlockedRelics);
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x00107DDC File Offset: 0x00105FDC
		private void LoadIcon(Texture2D tex)
		{
			this._icon.Texture = tex;
			this._icon.Visible = true;
		}

		/// <summary>
		/// Helper method to hook up relic nodes.
		/// </summary>
		// Token: 0x060026B9 RID: 9913 RVA: 0x00107DF8 File Offset: 0x00105FF8
		private void LoadRelicNodes(IEnumerable<RelicModel> relics, HashSet<RelicModel> seenRelics, HashSet<RelicModel> unlockedRelics)
		{
			foreach (Node node in this._relicsContainer.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
			foreach (RelicModel relicModel in relics)
			{
				ModelVisibility modelVisibility = ((!unlockedRelics.Contains(relicModel)) ? ModelVisibility.Locked : ((!seenRelics.Contains(relicModel)) ? ModelVisibility.NotSeen : ModelVisibility.Visible));
				NRelicCollectionEntry nrelicCollectionEntry = NRelicCollectionEntry.Create(relicModel, modelVisibility);
				this._relicsContainer.AddChildSafely(nrelicCollectionEntry);
				nrelicCollectionEntry.Connect(NClickableControl.SignalName.Released, Callable.From<NRelicCollectionEntry>(new Action<NRelicCollectionEntry>(this.OnRelicEntryPressed)), 0U);
			}
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x00107ECC File Offset: 0x001060CC
		public void ClearRelics()
		{
			foreach (Node node in this._relicsContainer.GetChildren(false))
			{
				node.QueueFreeSafely();
			}
			foreach (NRelicCollectionCategory nrelicCollectionCategory in base.GetChildren(false).OfType<NRelicCollectionCategory>())
			{
				nrelicCollectionCategory.QueueFreeSafely();
			}
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x00107F60 File Offset: 0x00106160
		private void OnRelicEntryPressed(NRelicCollectionEntry entry)
		{
			NGame.Instance.GetInspectRelicScreen().Open(this._collection.Relics, entry.relic);
			this._collection.SetLastFocusedRelic(entry);
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060026BC RID: 9916 RVA: 0x00107F90 File Offset: 0x00106190
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				if (this._subCategories.Any<NRelicCollectionCategory>())
				{
					return this._subCategories.First<NRelicCollectionCategory>().DefaultFocusedControl;
				}
				if (GodotObject.IsInstanceValid(this._relicsContainer) && this._relicsContainer.GetChildCount(false) > 0)
				{
					return this._relicsContainer.GetChild<Control>(0, false);
				}
				return null;
			}
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x00107FE8 File Offset: 0x001061E8
		public List<IReadOnlyList<Control>> GetGridItems()
		{
			List<IReadOnlyList<Control>> list = new List<IReadOnlyList<Control>>();
			if (this._subCategories.Any<NRelicCollectionCategory>())
			{
				using (List<NRelicCollectionCategory>.Enumerator enumerator = this._subCategories.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NRelicCollectionCategory nrelicCollectionCategory = enumerator.Current;
						list.AddRange(nrelicCollectionCategory.GetGridItems());
					}
					return list;
				}
			}
			for (int i = 0; i < this._relicsContainer.GetChildren(false).Count; i += this._relicsContainer.Columns)
			{
				list.Add(this._relicsContainer.GetChildren(false).OfType<Control>().Skip<Control>(i)
					.Take<Control>(this._relicsContainer.Columns)
					.ToList<Control>());
			}
			return list;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060026BE RID: 9918 RVA: 0x001080AC File Offset: 0x001062AC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NRelicCollectionCategory.MethodName.CreateForSubcategory, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("VBoxContainer"), false), 1L, null, null),
				new MethodInfo(NRelicCollectionCategory.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollectionCategory.MethodName.LoadIcon, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "tex", 0L, "", 6L, new StringName("Texture2D"), false)
				}, null),
				new MethodInfo(NRelicCollectionCategory.MethodName.ClearRelics, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollectionCategory.MethodName.OnRelicEntryPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "entry", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060026BF RID: 9919 RVA: 0x00108218 File Offset: 0x00106418
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelicCollectionCategory.MethodName.CreateForSubcategory && args.Count == 0)
			{
				NRelicCollectionCategory nrelicCollectionCategory = this.CreateForSubcategory();
				ret = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref nrelicCollectionCategory);
				return true;
			}
			if ((ref method) == NRelicCollectionCategory.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollectionCategory.MethodName.LoadIcon && args.Count == 1)
			{
				this.LoadIcon(VariantUtils.ConvertTo<Texture2D>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollectionCategory.MethodName.ClearRelics && args.Count == 0)
			{
				this.ClearRelics();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollectionCategory.MethodName.OnRelicEntryPressed && args.Count == 1)
			{
				this.OnRelicEntryPressed(VariantUtils.ConvertTo<NRelicCollectionEntry>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x060026C0 RID: 9920 RVA: 0x0010830C File Offset: 0x0010650C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRelicCollectionCategory.MethodName.CreateForSubcategory || (ref method) == NRelicCollectionCategory.MethodName._Ready || (ref method) == NRelicCollectionCategory.MethodName.LoadIcon || (ref method) == NRelicCollectionCategory.MethodName.ClearRelics || (ref method) == NRelicCollectionCategory.MethodName.OnRelicEntryPressed || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x060026C1 RID: 9921 RVA: 0x0010836C File Offset: 0x0010656C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRelicCollectionCategory.PropertyName._collection)
			{
				this._collection = VariantUtils.ConvertTo<NRelicCollection>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollectionCategory.PropertyName._headerLabel)
			{
				this._headerLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollectionCategory.PropertyName._relicsContainer)
			{
				this._relicsContainer = VariantUtils.ConvertTo<GridContainer>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollectionCategory.PropertyName._spacer)
			{
				this._spacer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollectionCategory.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x060026C2 RID: 9922 RVA: 0x00108408 File Offset: 0x00106608
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRelicCollectionCategory.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NRelicCollectionCategory.PropertyName._collection)
			{
				value = VariantUtils.CreateFrom<NRelicCollection>(ref this._collection);
				return true;
			}
			if ((ref name) == NRelicCollectionCategory.PropertyName._headerLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._headerLabel);
				return true;
			}
			if ((ref name) == NRelicCollectionCategory.PropertyName._relicsContainer)
			{
				value = VariantUtils.CreateFrom<GridContainer>(ref this._relicsContainer);
				return true;
			}
			if ((ref name) == NRelicCollectionCategory.PropertyName._spacer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._spacer);
				return true;
			}
			if ((ref name) == NRelicCollectionCategory.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060026C3 RID: 9923 RVA: 0x001084E0 File Offset: 0x001066E0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRelicCollectionCategory.PropertyName._collection, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollectionCategory.PropertyName._headerLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollectionCategory.PropertyName._relicsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollectionCategory.PropertyName._spacer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollectionCategory.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollectionCategory.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060026C4 RID: 9924 RVA: 0x001085BC File Offset: 0x001067BC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRelicCollectionCategory.PropertyName._collection, Variant.From<NRelicCollection>(ref this._collection));
			info.AddProperty(NRelicCollectionCategory.PropertyName._headerLabel, Variant.From<MegaRichTextLabel>(ref this._headerLabel));
			info.AddProperty(NRelicCollectionCategory.PropertyName._relicsContainer, Variant.From<GridContainer>(ref this._relicsContainer));
			info.AddProperty(NRelicCollectionCategory.PropertyName._spacer, Variant.From<Control>(ref this._spacer));
			info.AddProperty(NRelicCollectionCategory.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
		}

		/// <inheritdoc />
		// Token: 0x060026C5 RID: 9925 RVA: 0x00108640 File Offset: 0x00106840
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRelicCollectionCategory.PropertyName._collection, ref variant))
			{
				this._collection = variant.As<NRelicCollection>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRelicCollectionCategory.PropertyName._headerLabel, ref variant2))
			{
				this._headerLabel = variant2.As<MegaRichTextLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRelicCollectionCategory.PropertyName._relicsContainer, ref variant3))
			{
				this._relicsContainer = variant3.As<GridContainer>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRelicCollectionCategory.PropertyName._spacer, ref variant4))
			{
				this._spacer = variant4.As<Control>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRelicCollectionCategory.PropertyName._icon, ref variant5))
			{
				this._icon = variant5.As<TextureRect>();
			}
		}

		// Token: 0x04000E67 RID: 3687
		public static readonly string scenePath = SceneHelper.GetScenePath("screens/relic_collection/relic_collection_subcategory");

		// Token: 0x04000E68 RID: 3688
		private static readonly List<RelicModel> _relicModelCache = new List<RelicModel>();

		// Token: 0x04000E69 RID: 3689
		private NRelicCollection _collection;

		// Token: 0x04000E6A RID: 3690
		private MegaRichTextLabel _headerLabel;

		// Token: 0x04000E6B RID: 3691
		private GridContainer _relicsContainer;

		// Token: 0x04000E6C RID: 3692
		private readonly List<NRelicCollectionCategory> _subCategories = new List<NRelicCollectionCategory>();

		// Token: 0x04000E6D RID: 3693
		private Control _spacer;

		// Token: 0x04000E6E RID: 3694
		private TextureRect _icon;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001452 RID: 5202
		[NullableContext(0)]
		public class MethodName : VBoxContainer.MethodName
		{
			/// <summary>
			/// Cached name for the 'CreateForSubcategory' method.
			/// </summary>
			// Token: 0x04004D24 RID: 19748
			public static readonly StringName CreateForSubcategory = "CreateForSubcategory";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004D25 RID: 19749
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'LoadIcon' method.
			/// </summary>
			// Token: 0x04004D26 RID: 19750
			public static readonly StringName LoadIcon = "LoadIcon";

			/// <summary>
			/// Cached name for the 'ClearRelics' method.
			/// </summary>
			// Token: 0x04004D27 RID: 19751
			public static readonly StringName ClearRelics = "ClearRelics";

			/// <summary>
			/// Cached name for the 'OnRelicEntryPressed' method.
			/// </summary>
			// Token: 0x04004D28 RID: 19752
			public static readonly StringName OnRelicEntryPressed = "OnRelicEntryPressed";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001453 RID: 5203
		[NullableContext(0)]
		public class PropertyName : VBoxContainer.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04004D29 RID: 19753
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_collection' field.
			/// </summary>
			// Token: 0x04004D2A RID: 19754
			public static readonly StringName _collection = "_collection";

			/// <summary>
			/// Cached name for the '_headerLabel' field.
			/// </summary>
			// Token: 0x04004D2B RID: 19755
			public static readonly StringName _headerLabel = "_headerLabel";

			/// <summary>
			/// Cached name for the '_relicsContainer' field.
			/// </summary>
			// Token: 0x04004D2C RID: 19756
			public static readonly StringName _relicsContainer = "_relicsContainer";

			/// <summary>
			/// Cached name for the '_spacer' field.
			/// </summary>
			// Token: 0x04004D2D RID: 19757
			public static readonly StringName _spacer = "_spacer";

			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x04004D2E RID: 19758
			public static readonly StringName _icon = "_icon";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001454 RID: 5204
		[NullableContext(0)]
		public class SignalName : VBoxContainer.SignalName
		{
		}
	}
}

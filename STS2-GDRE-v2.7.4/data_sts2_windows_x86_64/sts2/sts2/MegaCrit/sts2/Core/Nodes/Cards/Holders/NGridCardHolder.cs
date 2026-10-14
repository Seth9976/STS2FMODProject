using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Pooling;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Cards.Holders
{
	// Token: 0x0200041D RID: 1053
	[ScriptPath("res://src/Core/Nodes/Cards/Holders/NGridCardHolder.cs")]
	public class NGridCardHolder : NCardHolder, IPoolable
	{
		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06004308 RID: 17160 RVA: 0x00208E46 File Offset: 0x00207046
		[Nullable(1)]
		private static string ScenePath
		{
			[NullableContext(1)]
			get
			{
				return SceneHelper.GetScenePath("cards/holders/grid_card_holder");
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06004309 RID: 17161 RVA: 0x00208E52 File Offset: 0x00207052
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NGridCardHolder.ScenePath);
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x0600430A RID: 17162 RVA: 0x00208E5E File Offset: 0x0020705E
		// (set) Token: 0x0600430B RID: 17163 RVA: 0x00208E66 File Offset: 0x00207066
		[Nullable(2)]
		public NCardLibraryStats CardLibraryStats
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			private set;
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x0600430C RID: 17164 RVA: 0x00208E6F File Offset: 0x0020706F
		[Nullable(1)]
		public override CardModel CardModel
		{
			[NullableContext(1)]
			get
			{
				return this._baseCard;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x0600430D RID: 17165 RVA: 0x00208E77 File Offset: 0x00207077
		public override bool IsShowingUpgradedCard
		{
			get
			{
				return this._isPreviewingUpgrade || base.IsShowingUpgradedCard;
			}
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x00208E89 File Offset: 0x00207089
		public static void InitPool()
		{
			NodePool.Init<NGridCardHolder>(NGridCardHolder.ScenePath, 30);
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x00208E98 File Offset: 0x00207098
		[NullableContext(1)]
		[return: Nullable(2)]
		public static NGridCardHolder Create(NCard cardNode)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NGridCardHolder ngridCardHolder = NodePool.Get<NGridCardHolder>();
			ngridCardHolder.SetCard(cardNode);
			ngridCardHolder.UpdateCardModel();
			ngridCardHolder.UpdateName();
			ngridCardHolder.Scale = ngridCardHolder.SmallScale;
			return ngridCardHolder;
		}

		// Token: 0x06004310 RID: 17168 RVA: 0x00208ED4 File Offset: 0x002070D4
		private void UpdateCardModel()
		{
			CardModel model = base.CardNode.Model;
			this._baseCard = model;
			if (model.IsUpgradable)
			{
				this._upgradedCard = (CardModel)model.MutableClone();
				this._upgradedCard.UpgradeInternal();
				if (base.IsNodeReady())
				{
					bool isPreviewingUpgrade = this._isPreviewingUpgrade;
					this._isPreviewingUpgrade = false;
					this.SetIsPreviewingUpgrade(isPreviewingUpgrade);
				}
			}
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x00208F35 File Offset: 0x00207135
		public void OnInstantiated()
		{
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x00208F38 File Offset: 0x00207138
		public override void _Ready()
		{
			bool isPreviewingUpgrade = this._isPreviewingUpgrade;
			this._isPreviewingUpgrade = false;
			this.SetIsPreviewingUpgrade(isPreviewingUpgrade);
			base.ConnectSignals();
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x00208F60 File Offset: 0x00207160
		public void EnsureCardLibraryStatsExists()
		{
			if (this.CardLibraryStats != null)
			{
				return;
			}
			this.CardLibraryStats = NCardLibraryStats.Create();
			this.AddChildSafely(this.CardLibraryStats);
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x00208F82 File Offset: 0x00207182
		protected override void OnCardReassigned()
		{
			this.UpdateCardModel();
			this.UpdateName();
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x00208F90 File Offset: 0x00207190
		[NullableContext(1)]
		protected override void SetCard(NCard node)
		{
			base.SetCard(node);
			if (this.CardLibraryStats != null)
			{
				this.MoveChildSafely(this.CardLibraryStats, base.GetChildCount(false) - 1);
			}
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x00208FB8 File Offset: 0x002071B8
		private void UpdateName()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("GridCardHolder-");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(base.CardNode.Model.Id);
			base.Name = defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06004317 RID: 17175 RVA: 0x00209004 File Offset: 0x00207204
		protected override void OnFocus()
		{
			base.OnFocus();
			this.MoveToFrontSafely();
		}

		// Token: 0x06004318 RID: 17176 RVA: 0x00209014 File Offset: 0x00207214
		public void SetIsPreviewingUpgrade(bool showUpgradePreview)
		{
			if (!base.Visible)
			{
				return;
			}
			if (!this._baseCard.IsUpgradable && showUpgradePreview)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendFormatted<ModelId>(this._baseCard.Id);
				defaultInterpolatedStringHandler.AppendLiteral(" is not upgradable.");
				throw new InvalidExpressionException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (this._isPreviewingUpgrade != showUpgradePreview)
			{
				if (showUpgradePreview && this._upgradedCard != null)
				{
					base.CardNode.Model = this._upgradedCard;
					base.CardNode.ShowUpgradePreview();
				}
				else
				{
					base.CardNode.Model = this._baseCard;
					base.CardNode.UpdateVisuals(base.CardNode.DisplayingPile, CardPreviewMode.Normal);
				}
				this._isPreviewingUpgrade = showUpgradePreview;
			}
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x002090D2 File Offset: 0x002072D2
		public override void _ExitTree()
		{
			if (base.IsAncestorOf(base.CardNode))
			{
				NCard cardNode = base.CardNode;
				if (cardNode != null)
				{
					cardNode.QueueFreeSafely();
				}
			}
			base.CardNode = null;
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x002090FC File Offset: 0x002072FC
		public void OnReturnedFromPool()
		{
			if (base.IsNodeReady())
			{
				base.Position = Vector2.Zero;
				base.Rotation = 0f;
				base.Scale = Vector2.One;
				base.Modulate = Colors.White;
				base.Visible = true;
				base.SetClickable(true);
				base.Hitbox.MouseDefaultCursorShape = 0L;
				this._isPreviewingUpgrade = false;
				if (this.CardLibraryStats != null)
				{
					this.CardLibraryStats.Visible = false;
					this.CardLibraryStats.Modulate = Colors.White;
				}
			}
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x00209183 File Offset: 0x00207383
		public void OnFreedToPool()
		{
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600431C RID: 17180 RVA: 0x00209188 File Offset: 0x00207388
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NGridCardHolder.MethodName.InitPool, new PropertyInfo(0L, "", 0L, "", 6L, false), 33L, null, null),
				new MethodInfo(NGridCardHolder.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cardNode", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NGridCardHolder.MethodName.UpdateCardModel, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGridCardHolder.MethodName.OnInstantiated, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGridCardHolder.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGridCardHolder.MethodName.EnsureCardLibraryStatsExists, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGridCardHolder.MethodName.OnCardReassigned, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGridCardHolder.MethodName.SetCard, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "node", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NGridCardHolder.MethodName.UpdateName, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGridCardHolder.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGridCardHolder.MethodName.SetIsPreviewingUpgrade, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(1L, "showUpgradePreview", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NGridCardHolder.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGridCardHolder.MethodName.OnReturnedFromPool, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NGridCardHolder.MethodName.OnFreedToPool, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600431D RID: 17181 RVA: 0x002094C4 File Offset: 0x002076C4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGridCardHolder.MethodName.InitPool && args.Count == 0)
			{
				NGridCardHolder.InitPool();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName.Create && args.Count == 1)
			{
				NGridCardHolder ngridCardHolder = NGridCardHolder.Create(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = VariantUtils.CreateFrom<NGridCardHolder>(ref ngridCardHolder);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName.UpdateCardModel && args.Count == 0)
			{
				this.UpdateCardModel();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName.OnInstantiated && args.Count == 0)
			{
				this.OnInstantiated();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName.EnsureCardLibraryStatsExists && args.Count == 0)
			{
				this.EnsureCardLibraryStatsExists();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName.OnCardReassigned && args.Count == 0)
			{
				this.OnCardReassigned();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName.SetCard && args.Count == 1)
			{
				this.SetCard(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName.UpdateName && args.Count == 0)
			{
				this.UpdateName();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName.SetIsPreviewingUpgrade && args.Count == 1)
			{
				this.SetIsPreviewingUpgrade(VariantUtils.ConvertTo<bool>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName.OnReturnedFromPool && args.Count == 0)
			{
				this.OnReturnedFromPool();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName.OnFreedToPool && args.Count == 0)
			{
				this.OnFreedToPool();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x0600431E RID: 17182 RVA: 0x00209710 File Offset: 0x00207910
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NGridCardHolder.MethodName.InitPool && args.Count == 0)
			{
				NGridCardHolder.InitPool();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NGridCardHolder.MethodName.Create && args.Count == 1)
			{
				NGridCardHolder ngridCardHolder = NGridCardHolder.Create(VariantUtils.ConvertTo<NCard>(args[0]));
				ret = VariantUtils.CreateFrom<NGridCardHolder>(ref ngridCardHolder);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x0600431F RID: 17183 RVA: 0x00209784 File Offset: 0x00207984
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NGridCardHolder.MethodName.InitPool || (ref method) == NGridCardHolder.MethodName.Create || (ref method) == NGridCardHolder.MethodName.UpdateCardModel || (ref method) == NGridCardHolder.MethodName.OnInstantiated || (ref method) == NGridCardHolder.MethodName._Ready || (ref method) == NGridCardHolder.MethodName.EnsureCardLibraryStatsExists || (ref method) == NGridCardHolder.MethodName.OnCardReassigned || (ref method) == NGridCardHolder.MethodName.SetCard || (ref method) == NGridCardHolder.MethodName.UpdateName || (ref method) == NGridCardHolder.MethodName.OnFocus || (ref method) == NGridCardHolder.MethodName.SetIsPreviewingUpgrade || (ref method) == NGridCardHolder.MethodName._ExitTree || (ref method) == NGridCardHolder.MethodName.OnReturnedFromPool || (ref method) == NGridCardHolder.MethodName.OnFreedToPool || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06004320 RID: 17184 RVA: 0x0020986A File Offset: 0x00207A6A
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NGridCardHolder.PropertyName.CardLibraryStats)
			{
				this.CardLibraryStats = VariantUtils.ConvertTo<NCardLibraryStats>(ref value);
				return true;
			}
			if ((ref name) == NGridCardHolder.PropertyName._isPreviewingUpgrade)
			{
				this._isPreviewingUpgrade = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06004321 RID: 17185 RVA: 0x002098AC File Offset: 0x00207AAC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NGridCardHolder.PropertyName.CardLibraryStats)
			{
				NCardLibraryStats cardLibraryStats = this.CardLibraryStats;
				value = VariantUtils.CreateFrom<NCardLibraryStats>(ref cardLibraryStats);
				return true;
			}
			if ((ref name) == NGridCardHolder.PropertyName.IsShowingUpgradedCard)
			{
				bool isShowingUpgradedCard = this.IsShowingUpgradedCard;
				value = VariantUtils.CreateFrom<bool>(ref isShowingUpgradedCard);
				return true;
			}
			if ((ref name) == NGridCardHolder.PropertyName._isPreviewingUpgrade)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isPreviewingUpgrade);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06004322 RID: 17186 RVA: 0x00209928 File Offset: 0x00207B28
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NGridCardHolder.PropertyName.CardLibraryStats, 0L, "", 4096L, false),
				new PropertyInfo(1L, NGridCardHolder.PropertyName._isPreviewingUpgrade, 0L, "", 4096L, false),
				new PropertyInfo(1L, NGridCardHolder.PropertyName.IsShowingUpgradedCard, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06004323 RID: 17187 RVA: 0x002099A0 File Offset: 0x00207BA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName cardLibraryStats = NGridCardHolder.PropertyName.CardLibraryStats;
			NCardLibraryStats cardLibraryStats2 = this.CardLibraryStats;
			info.AddProperty(cardLibraryStats, Variant.From<NCardLibraryStats>(ref cardLibraryStats2));
			info.AddProperty(NGridCardHolder.PropertyName._isPreviewingUpgrade, Variant.From<bool>(ref this._isPreviewingUpgrade));
		}

		/// <inheritdoc />
		// Token: 0x06004324 RID: 17188 RVA: 0x002099E4 File Offset: 0x00207BE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NGridCardHolder.PropertyName.CardLibraryStats, ref variant))
			{
				this.CardLibraryStats = variant.As<NCardLibraryStats>();
			}
			Variant variant2;
			if (info.TryGetProperty(NGridCardHolder.PropertyName._isPreviewingUpgrade, ref variant2))
			{
				this._isPreviewingUpgrade = variant2.As<bool>();
			}
		}

		// Token: 0x04001A18 RID: 6680
		[Nullable(1)]
		private CardModel _baseCard;

		// Token: 0x04001A19 RID: 6681
		[Nullable(2)]
		private CardModel _upgradedCard;

		// Token: 0x04001A1A RID: 6682
		private bool _isPreviewingUpgrade;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001963 RID: 6499
		public new class MethodName : NCardHolder.MethodName
		{
			/// <summary>
			/// Cached name for the 'InitPool' method.
			/// </summary>
			// Token: 0x04006443 RID: 25667
			public static readonly StringName InitPool = "InitPool";

			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04006444 RID: 25668
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the 'UpdateCardModel' method.
			/// </summary>
			// Token: 0x04006445 RID: 25669
			public static readonly StringName UpdateCardModel = "UpdateCardModel";

			/// <summary>
			/// Cached name for the 'OnInstantiated' method.
			/// </summary>
			// Token: 0x04006446 RID: 25670
			public static readonly StringName OnInstantiated = "OnInstantiated";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04006447 RID: 25671
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'EnsureCardLibraryStatsExists' method.
			/// </summary>
			// Token: 0x04006448 RID: 25672
			public static readonly StringName EnsureCardLibraryStatsExists = "EnsureCardLibraryStatsExists";

			/// <summary>
			/// Cached name for the 'OnCardReassigned' method.
			/// </summary>
			// Token: 0x04006449 RID: 25673
			public new static readonly StringName OnCardReassigned = "OnCardReassigned";

			/// <summary>
			/// Cached name for the 'SetCard' method.
			/// </summary>
			// Token: 0x0400644A RID: 25674
			public new static readonly StringName SetCard = "SetCard";

			/// <summary>
			/// Cached name for the 'UpdateName' method.
			/// </summary>
			// Token: 0x0400644B RID: 25675
			public static readonly StringName UpdateName = "UpdateName";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x0400644C RID: 25676
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'SetIsPreviewingUpgrade' method.
			/// </summary>
			// Token: 0x0400644D RID: 25677
			public static readonly StringName SetIsPreviewingUpgrade = "SetIsPreviewingUpgrade";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x0400644E RID: 25678
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnReturnedFromPool' method.
			/// </summary>
			// Token: 0x0400644F RID: 25679
			public static readonly StringName OnReturnedFromPool = "OnReturnedFromPool";

			/// <summary>
			/// Cached name for the 'OnFreedToPool' method.
			/// </summary>
			// Token: 0x04006450 RID: 25680
			public static readonly StringName OnFreedToPool = "OnFreedToPool";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001964 RID: 6500
		public new class PropertyName : NCardHolder.PropertyName
		{
			/// <summary>
			/// Cached name for the 'CardLibraryStats' property.
			/// </summary>
			// Token: 0x04006451 RID: 25681
			public static readonly StringName CardLibraryStats = "CardLibraryStats";

			/// <summary>
			/// Cached name for the 'IsShowingUpgradedCard' property.
			/// </summary>
			// Token: 0x04006452 RID: 25682
			public new static readonly StringName IsShowingUpgradedCard = "IsShowingUpgradedCard";

			/// <summary>
			/// Cached name for the '_isPreviewingUpgrade' field.
			/// </summary>
			// Token: 0x04006453 RID: 25683
			public static readonly StringName _isPreviewingUpgrade = "_isPreviewingUpgrade";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001965 RID: 6501
		public new class SignalName : NCardHolder.SignalName
		{
		}
	}
}

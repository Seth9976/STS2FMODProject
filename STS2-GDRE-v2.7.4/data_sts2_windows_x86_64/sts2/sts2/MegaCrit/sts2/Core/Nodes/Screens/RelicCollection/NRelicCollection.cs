using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection
{
	// Token: 0x020002D6 RID: 726
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/RelicCollection/NRelicCollection.cs")]
	public class NRelicCollection : NSubmenu
	{
		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x0600269A RID: 9882 RVA: 0x00106B06 File Offset: 0x00104D06
		public IReadOnlyList<RelicModel> Relics
		{
			get
			{
				return this._relics;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x0600269B RID: 9883 RVA: 0x00106B0E File Offset: 0x00104D0E
		public static string[] AssetPaths
		{
			get
			{
				return new string[]
				{
					NRelicCollection._scenePath,
					NRelicCollectionEntry.scenePath,
					NRelicCollectionCategory.scenePath,
					NRelicCollectionEntry.lockedIconPath
				};
			}
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x00106B36 File Offset: 0x00104D36
		[NullableContext(2)]
		public static NRelicCollection Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NRelicCollection._scenePath).Instantiate<NRelicCollection>(0L);
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x00106B58 File Offset: 0x00104D58
		public override void _Ready()
		{
			this.ConnectSignals();
			this._screenContents = base.GetNode<NScrollableContainer>("%ScreenContents");
			this._starter = base.GetNode<NRelicCollectionCategory>("%Starter");
			this._common = base.GetNode<NRelicCollectionCategory>("%Common");
			this._uncommon = base.GetNode<NRelicCollectionCategory>("%Uncommon");
			this._rare = base.GetNode<NRelicCollectionCategory>("%Rare");
			this._shop = base.GetNode<NRelicCollectionCategory>("%Shop");
			this._ancient = base.GetNode<NRelicCollectionCategory>("%Ancient");
			this._event = base.GetNode<NRelicCollectionCategory>("%Event");
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x00106C1B File Offset: 0x00104E1B
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x00106C28 File Offset: 0x00104E28
		public override void _ExitTree()
		{
			this._cts.Cancel();
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x00106C35 File Offset: 0x00104E35
		public override void OnSubmenuOpened()
		{
			base.OnSubmenuOpened();
			this._relics.Clear();
			this._loadTask = TaskHelper.RunSafely(this.LoadRelics());
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x00106C59 File Offset: 0x00104E59
		public override void OnSubmenuClosed()
		{
			base.OnSubmenuClosed();
			Tween screenTween = this._screenTween;
			if (screenTween != null)
			{
				screenTween.Kill();
			}
			this.ClearRelics();
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x00106C78 File Offset: 0x00104E78
		protected override void OnSubmenuShown()
		{
			base.OnSubmenuShown();
			TaskHelper.RunSafely(this.TweenAfterLoading());
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x00106C8C File Offset: 0x00104E8C
		private async Task TweenAfterLoading()
		{
			this._screenContents.Modulate = new Color(1f, 1f, 1f, 0f);
			if (this._loadTask != null)
			{
				await this._loadTask;
			}
			Tween screenTween = this._screenTween;
			if (screenTween != null)
			{
				screenTween.Kill();
			}
			this._screenTween = base.CreateTween();
			this._screenTween.TweenProperty(this._screenContents, "modulate:a", 1f, 0.4).From(0f);
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x00106CD0 File Offset: 0x00104ED0
		private async Task LoadRelics()
		{
			this._starter.Modulate = Colors.Transparent;
			this._common.Modulate = Colors.Transparent;
			this._uncommon.Modulate = Colors.Transparent;
			this._rare.Modulate = Colors.Transparent;
			this._shop.Modulate = Colors.Transparent;
			this._ancient.Modulate = Colors.Transparent;
			this._event.Modulate = Colors.Transparent;
			IEnumerable<ModelId> discoveredRelics = SaveManager.Instance.Progress.DiscoveredRelics;
			Func<ModelId, RelicModel> func;
			if ((func = NRelicCollection.<>O.<0>__GetByIdOrNull) == null)
			{
				func = (NRelicCollection.<>O.<0>__GetByIdOrNull = new Func<ModelId, RelicModel>(ModelDb.GetByIdOrNull<RelicModel>));
			}
			HashSet<RelicModel> hashSet = discoveredRelics.Select<ModelId, RelicModel>(func).OfType<RelicModel>().ToHashSet<RelicModel>();
			UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
			HashSet<RelicModel> hashSet2 = unlockState.Relics.ToHashSet<RelicModel>();
			this._starter.LoadRelics(RelicRarity.Starter, this, new LocString("relic_collection", "STARTER"), hashSet, unlockState, hashSet2);
			this._common.LoadRelics(RelicRarity.Common, this, new LocString("relic_collection", "COMMON"), hashSet, unlockState, hashSet2);
			this._uncommon.LoadRelics(RelicRarity.Uncommon, this, new LocString("relic_collection", "UNCOMMON"), hashSet, unlockState, hashSet2);
			this._rare.LoadRelics(RelicRarity.Rare, this, new LocString("relic_collection", "RARE"), hashSet, unlockState, hashSet2);
			this._shop.LoadRelics(RelicRarity.Shop, this, new LocString("relic_collection", "SHOP"), hashSet, unlockState, hashSet2);
			this._ancient.LoadRelics(RelicRarity.Ancient, this, new LocString("relic_collection", "ANCIENT"), hashSet, unlockState, hashSet2);
			this._event.LoadRelics(RelicRarity.Event, this, new LocString("relic_collection", "EVENT"), hashSet, unlockState, hashSet2);
			List<IReadOnlyList<Control>> list = new List<IReadOnlyList<Control>>();
			list.AddRange(this._starter.GetGridItems());
			list.AddRange(this._common.GetGridItems());
			list.AddRange(this._uncommon.GetGridItems());
			list.AddRange(this._rare.GetGridItems());
			list.AddRange(this._shop.GetGridItems());
			list.AddRange(this._ancient.GetGridItems());
			list.AddRange(this._event.GetGridItems());
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list[i].Count; j++)
				{
					Control control = list[i][j];
					Control control2 = control;
					NodePath nodePath;
					if (j <= 0)
					{
						IReadOnlyList<Control> readOnlyList = list[i];
						nodePath = readOnlyList[readOnlyList.Count - 1].GetPath();
					}
					else
					{
						nodePath = list[i][j - 1].GetPath();
					}
					control2.FocusNeighborLeft = nodePath;
					control.FocusNeighborRight = ((j < list[i].Count - 1) ? list[i][j + 1].GetPath() : list[i][0].GetPath());
					if (i > 0)
					{
						control.FocusNeighborTop = ((j < list[i - 1].Count) ? list[i - 1][j].GetPath() : list[i - 1][list[i - 1].Count - 1].GetPath());
					}
					else
					{
						control.FocusNeighborTop = list[i][j].GetPath();
					}
					if (i < list.Count - 1)
					{
						control.FocusNeighborBottom = ((j < list[i + 1].Count) ? list[i + 1][j].GetPath() : list[i + 1][list[i + 1].Count - 1].GetPath());
					}
					else
					{
						control.FocusNeighborBottom = list[i][j].GetPath();
					}
				}
			}
			await this.AwaitProcessFrame(this._cts.Token);
			this._starter.Modulate = Colors.White;
			this._common.Modulate = Colors.White;
			this._uncommon.Modulate = Colors.White;
			this._rare.Modulate = Colors.White;
			this._shop.Modulate = Colors.White;
			this._ancient.Modulate = Colors.White;
			this._event.Modulate = Colors.White;
			this._screenContents.InstantlyScrollToTop();
			Control initialFocusedControl = this.InitialFocusedControl;
			if (initialFocusedControl != null)
			{
				initialFocusedControl.TryGrabFocus();
			}
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x00106D13 File Offset: 0x00104F13
		public void AddRelics(IEnumerable<RelicModel> relics)
		{
			this._relics.AddRange(relics);
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x00106D24 File Offset: 0x00104F24
		private void ClearRelics()
		{
			this._starter.ClearRelics();
			this._common.ClearRelics();
			this._uncommon.ClearRelics();
			this._rare.ClearRelics();
			this._shop.ClearRelics();
			this._ancient.ClearRelics();
			this._event.ClearRelics();
			this._relics.Clear();
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x00106D89 File Offset: 0x00104F89
		public void SetLastFocusedRelic(NRelicCollectionEntry relic)
		{
			this._lastFocusedControl = relic;
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x060026A8 RID: 9896 RVA: 0x00106D92 File Offset: 0x00104F92
		[Nullable(2)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this._starter.DefaultFocusedControl;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060026A9 RID: 9897 RVA: 0x00106DA0 File Offset: 0x00104FA0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NRelicCollection.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NRelicCollection.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollection.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollection.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollection.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollection.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollection.MethodName.OnSubmenuShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollection.MethodName.ClearRelics, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicCollection.MethodName.SetLastFocusedRelic, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "relic", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060026AA RID: 9898 RVA: 0x00106F9C File Offset: 0x0010519C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelicCollection.MethodName.Create && args.Count == 0)
			{
				NRelicCollection nrelicCollection = NRelicCollection.Create();
				ret = VariantUtils.CreateFrom<NRelicCollection>(ref nrelicCollection);
				return true;
			}
			if ((ref method) == NRelicCollection.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollection.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollection.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollection.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollection.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollection.MethodName.OnSubmenuShown && args.Count == 0)
			{
				this.OnSubmenuShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollection.MethodName.ClearRelics && args.Count == 0)
			{
				this.ClearRelics();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicCollection.MethodName.SetLastFocusedRelic && args.Count == 1)
			{
				this.SetLastFocusedRelic(VariantUtils.ConvertTo<NRelicCollectionEntry>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x00107114 File Offset: 0x00105314
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelicCollection.MethodName.Create && args.Count == 0)
			{
				NRelicCollection nrelicCollection = NRelicCollection.Create();
				ret = VariantUtils.CreateFrom<NRelicCollection>(ref nrelicCollection);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x060026AC RID: 9900 RVA: 0x00107154 File Offset: 0x00105354
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRelicCollection.MethodName.Create || (ref method) == NRelicCollection.MethodName._Ready || (ref method) == NRelicCollection.MethodName._EnterTree || (ref method) == NRelicCollection.MethodName._ExitTree || (ref method) == NRelicCollection.MethodName.OnSubmenuOpened || (ref method) == NRelicCollection.MethodName.OnSubmenuClosed || (ref method) == NRelicCollection.MethodName.OnSubmenuShown || (ref method) == NRelicCollection.MethodName.ClearRelics || (ref method) == NRelicCollection.MethodName.SetLastFocusedRelic || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060026AD RID: 9901 RVA: 0x001071F0 File Offset: 0x001053F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRelicCollection.PropertyName._screenContents)
			{
				this._screenContents = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._starter)
			{
				this._starter = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._common)
			{
				this._common = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._uncommon)
			{
				this._uncommon = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._rare)
			{
				this._rare = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._shop)
			{
				this._shop = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._ancient)
			{
				this._ancient = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._event)
			{
				this._event = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._screenTween)
			{
				this._screenTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060026AE RID: 9902 RVA: 0x001072F8 File Offset: 0x001054F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRelicCollection.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._screenContents)
			{
				value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._screenContents);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._starter)
			{
				value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._starter);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._common)
			{
				value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._common);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._uncommon)
			{
				value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._uncommon);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._rare)
			{
				value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._rare);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._shop)
			{
				value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._shop);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._ancient)
			{
				value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._ancient);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._event)
			{
				value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._event);
				return true;
			}
			if ((ref name) == NRelicCollection.PropertyName._screenTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._screenTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060026AF RID: 9903 RVA: 0x00107450 File Offset: 0x00105650
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRelicCollection.PropertyName._screenContents, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollection.PropertyName._starter, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollection.PropertyName._common, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollection.PropertyName._uncommon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollection.PropertyName._rare, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollection.PropertyName._shop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollection.PropertyName._ancient, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollection.PropertyName._event, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollection.PropertyName._screenTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicCollection.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060026B0 RID: 9904 RVA: 0x001075B0 File Offset: 0x001057B0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRelicCollection.PropertyName._screenContents, Variant.From<NScrollableContainer>(ref this._screenContents));
			info.AddProperty(NRelicCollection.PropertyName._starter, Variant.From<NRelicCollectionCategory>(ref this._starter));
			info.AddProperty(NRelicCollection.PropertyName._common, Variant.From<NRelicCollectionCategory>(ref this._common));
			info.AddProperty(NRelicCollection.PropertyName._uncommon, Variant.From<NRelicCollectionCategory>(ref this._uncommon));
			info.AddProperty(NRelicCollection.PropertyName._rare, Variant.From<NRelicCollectionCategory>(ref this._rare));
			info.AddProperty(NRelicCollection.PropertyName._shop, Variant.From<NRelicCollectionCategory>(ref this._shop));
			info.AddProperty(NRelicCollection.PropertyName._ancient, Variant.From<NRelicCollectionCategory>(ref this._ancient));
			info.AddProperty(NRelicCollection.PropertyName._event, Variant.From<NRelicCollectionCategory>(ref this._event));
			info.AddProperty(NRelicCollection.PropertyName._screenTween, Variant.From<Tween>(ref this._screenTween));
		}

		/// <inheritdoc />
		// Token: 0x060026B1 RID: 9905 RVA: 0x0010768C File Offset: 0x0010588C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRelicCollection.PropertyName._screenContents, ref variant))
			{
				this._screenContents = variant.As<NScrollableContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRelicCollection.PropertyName._starter, ref variant2))
			{
				this._starter = variant2.As<NRelicCollectionCategory>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRelicCollection.PropertyName._common, ref variant3))
			{
				this._common = variant3.As<NRelicCollectionCategory>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRelicCollection.PropertyName._uncommon, ref variant4))
			{
				this._uncommon = variant4.As<NRelicCollectionCategory>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRelicCollection.PropertyName._rare, ref variant5))
			{
				this._rare = variant5.As<NRelicCollectionCategory>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRelicCollection.PropertyName._shop, ref variant6))
			{
				this._shop = variant6.As<NRelicCollectionCategory>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRelicCollection.PropertyName._ancient, ref variant7))
			{
				this._ancient = variant7.As<NRelicCollectionCategory>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRelicCollection.PropertyName._event, ref variant8))
			{
				this._event = variant8.As<NRelicCollectionCategory>();
			}
			Variant variant9;
			if (info.TryGetProperty(NRelicCollection.PropertyName._screenTween, ref variant9))
			{
				this._screenTween = variant9.As<Tween>();
			}
		}

		// Token: 0x04000E5A RID: 3674
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/relic_collection/relic_collection");

		// Token: 0x04000E5B RID: 3675
		private NScrollableContainer _screenContents;

		// Token: 0x04000E5C RID: 3676
		private NRelicCollectionCategory _starter;

		// Token: 0x04000E5D RID: 3677
		private NRelicCollectionCategory _common;

		// Token: 0x04000E5E RID: 3678
		private NRelicCollectionCategory _uncommon;

		// Token: 0x04000E5F RID: 3679
		private NRelicCollectionCategory _rare;

		// Token: 0x04000E60 RID: 3680
		private NRelicCollectionCategory _shop;

		// Token: 0x04000E61 RID: 3681
		private NRelicCollectionCategory _ancient;

		// Token: 0x04000E62 RID: 3682
		private NRelicCollectionCategory _event;

		// Token: 0x04000E63 RID: 3683
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x04000E64 RID: 3684
		[Nullable(2)]
		private Tween _screenTween;

		// Token: 0x04000E65 RID: 3685
		[Nullable(2)]
		private Task _loadTask;

		// Token: 0x04000E66 RID: 3686
		private readonly List<RelicModel> _relics = new List<RelicModel>();

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200144C RID: 5196
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004D08 RID: 19720
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004D09 RID: 19721
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04004D0A RID: 19722
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004D0B RID: 19723
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04004D0C RID: 19724
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x04004D0D RID: 19725
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'OnSubmenuShown' method.
			/// </summary>
			// Token: 0x04004D0E RID: 19726
			public new static readonly StringName OnSubmenuShown = "OnSubmenuShown";

			/// <summary>
			/// Cached name for the 'ClearRelics' method.
			/// </summary>
			// Token: 0x04004D0F RID: 19727
			public static readonly StringName ClearRelics = "ClearRelics";

			/// <summary>
			/// Cached name for the 'SetLastFocusedRelic' method.
			/// </summary>
			// Token: 0x04004D10 RID: 19728
			public static readonly StringName SetLastFocusedRelic = "SetLastFocusedRelic";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200144D RID: 5197
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04004D11 RID: 19729
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_screenContents' field.
			/// </summary>
			// Token: 0x04004D12 RID: 19730
			public static readonly StringName _screenContents = "_screenContents";

			/// <summary>
			/// Cached name for the '_starter' field.
			/// </summary>
			// Token: 0x04004D13 RID: 19731
			public static readonly StringName _starter = "_starter";

			/// <summary>
			/// Cached name for the '_common' field.
			/// </summary>
			// Token: 0x04004D14 RID: 19732
			public static readonly StringName _common = "_common";

			/// <summary>
			/// Cached name for the '_uncommon' field.
			/// </summary>
			// Token: 0x04004D15 RID: 19733
			public static readonly StringName _uncommon = "_uncommon";

			/// <summary>
			/// Cached name for the '_rare' field.
			/// </summary>
			// Token: 0x04004D16 RID: 19734
			public static readonly StringName _rare = "_rare";

			/// <summary>
			/// Cached name for the '_shop' field.
			/// </summary>
			// Token: 0x04004D17 RID: 19735
			public static readonly StringName _shop = "_shop";

			/// <summary>
			/// Cached name for the '_ancient' field.
			/// </summary>
			// Token: 0x04004D18 RID: 19736
			public static readonly StringName _ancient = "_ancient";

			/// <summary>
			/// Cached name for the '_event' field.
			/// </summary>
			// Token: 0x04004D19 RID: 19737
			public static readonly StringName _event = "_event";

			/// <summary>
			/// Cached name for the '_screenTween' field.
			/// </summary>
			// Token: 0x04004D1A RID: 19738
			public static readonly StringName _screenTween = "_screenTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200144E RID: 5198
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}

		// Token: 0x0200144F RID: 5199
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04004D1B RID: 19739
			[Nullable(0)]
			public static Func<ModelId, RelicModel> <0>__GetByIdOrNull;
		}
	}
}

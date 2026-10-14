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
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;

namespace MegaCrit.Sts2.Core.Nodes.Screens.PotionLab
{
	// Token: 0x020002DE RID: 734
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/PotionLab/NPotionLab.cs")]
	public class NPotionLab : NSubmenu
	{
		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x0010BC0C File Offset: 0x00109E0C
		public static string[] AssetPaths
		{
			get
			{
				return new string[]
				{
					NPotionLab._scenePath,
					NLabPotionHolder.scenePath,
					NLabPotionHolder.lockedIconPath
				};
			}
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x0010BC2C File Offset: 0x00109E2C
		[NullableContext(2)]
		public static NPotionLab Create()
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			return PreloadManager.Cache.GetScene(NPotionLab._scenePath).Instantiate<NPotionLab>(0L);
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x0010BC50 File Offset: 0x00109E50
		public override void _Ready()
		{
			this.ConnectSignals();
			this._screenContents = base.GetNode<NScrollableContainer>("%ScreenContents");
			this._common = base.GetNode<NPotionLabCategory>("%Common");
			this._uncommon = base.GetNode<NPotionLabCategory>("%Uncommon");
			this._rare = base.GetNode<NPotionLabCategory>("%Rare");
			this._special = base.GetNode<NPotionLabCategory>("%Special");
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x0010BCD1 File Offset: 0x00109ED1
		public override void _EnterTree()
		{
			this._cts = new CancellationTokenSource();
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x0010BCDE File Offset: 0x00109EDE
		public override void _ExitTree()
		{
			this._cts.Cancel();
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x0010BCEB File Offset: 0x00109EEB
		public override void OnSubmenuOpened()
		{
			base.OnSubmenuOpened();
			this._loadTask = TaskHelper.RunSafely(this.LoadPotions());
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x0010BD04 File Offset: 0x00109F04
		public override void OnSubmenuClosed()
		{
			base.OnSubmenuClosed();
			Tween screenTween = this._screenTween;
			if (screenTween != null)
			{
				screenTween.Kill();
			}
			this.ClearPotions();
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x0010BD23 File Offset: 0x00109F23
		protected override void OnSubmenuShown()
		{
			base.OnSubmenuShown();
			TaskHelper.RunSafely(this.TweenAfterLoading());
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x0010BD38 File Offset: 0x00109F38
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
			this._screenTween.TweenProperty(this._screenContents, "modulate:a", 1f, 0.25).From(0f);
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x0010BD7C File Offset: 0x00109F7C
		private async Task LoadPotions()
		{
			this._common.Modulate = Colors.Transparent;
			this._uncommon.Modulate = Colors.Transparent;
			this._rare.Modulate = Colors.Transparent;
			this._special.Modulate = Colors.Transparent;
			UnlockState unlockState = SaveManager.Instance.GenerateUnlockStateFromProgress();
			HashSet<PotionModel> hashSet = unlockState.Potions.ToHashSet<PotionModel>();
			IEnumerable<ModelId> discoveredPotions = SaveManager.Instance.Progress.DiscoveredPotions;
			Func<ModelId, PotionModel> func;
			if ((func = NPotionLab.<>O.<0>__GetByIdOrNull) == null)
			{
				func = (NPotionLab.<>O.<0>__GetByIdOrNull = new Func<ModelId, PotionModel>(ModelDb.GetByIdOrNull<PotionModel>));
			}
			HashSet<PotionModel> hashSet2 = discoveredPotions.Select<ModelId, PotionModel>(func).OfType<PotionModel>().ToHashSet<PotionModel>();
			this._common.LoadPotions(PotionRarity.Common, new LocString("potion_lab", "COMMON"), hashSet2, unlockState, hashSet, null);
			this._uncommon.LoadPotions(PotionRarity.Uncommon, new LocString("potion_lab", "UNCOMMON"), hashSet2, unlockState, hashSet, null);
			this._rare.LoadPotions(PotionRarity.Rare, new LocString("potion_lab", "RARE"), hashSet2, unlockState, hashSet, null);
			this._special.LoadPotions(PotionRarity.Event, new LocString("potion_lab", "SPECIAL"), hashSet2, unlockState, hashSet, new PotionRarity?(PotionRarity.Token));
			List<IReadOnlyList<Control>> list = new List<IReadOnlyList<Control>>();
			list.AddRange(this._common.GetGridItems());
			list.AddRange(this._uncommon.GetGridItems());
			list.AddRange(this._rare.GetGridItems());
			list.AddRange(this._special.GetGridItems());
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
			this._common.Modulate = Colors.White;
			this._uncommon.Modulate = Colors.White;
			this._rare.Modulate = Colors.White;
			this._special.Modulate = Colors.White;
			this._screenContents.InstantlyScrollToTop();
			Control initialFocusedControl = this.InitialFocusedControl;
			if (initialFocusedControl != null)
			{
				initialFocusedControl.TryGrabFocus();
			}
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x0010BDBF File Offset: 0x00109FBF
		private void ClearPotions()
		{
			this._common.ClearPotions();
			this._uncommon.ClearPotions();
			this._rare.ClearPotions();
			this._special.ClearPotions();
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002741 RID: 10049 RVA: 0x0010BDED File Offset: 0x00109FED
		[Nullable(2)]
		protected override Control InitialFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this._common.DefaultFocusedControl;
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002742 RID: 10050 RVA: 0x0010BDFC File Offset: 0x00109FFC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NPotionLab.MethodName.Create, new PropertyInfo(24L, "", 0L, "", 6L, new StringName("Control"), false), 33L, null, null),
				new MethodInfo(NPotionLab.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionLab.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionLab.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionLab.MethodName.OnSubmenuOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionLab.MethodName.OnSubmenuClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionLab.MethodName.OnSubmenuShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NPotionLab.MethodName.ClearPotions, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002743 RID: 10051 RVA: 0x0010BF98 File Offset: 0x0010A198
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotionLab.MethodName.Create && args.Count == 0)
			{
				NPotionLab npotionLab = NPotionLab.Create();
				ret = VariantUtils.CreateFrom<NPotionLab>(ref npotionLab);
				return true;
			}
			if ((ref method) == NPotionLab.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionLab.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionLab.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionLab.MethodName.OnSubmenuOpened && args.Count == 0)
			{
				this.OnSubmenuOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionLab.MethodName.OnSubmenuClosed && args.Count == 0)
			{
				this.OnSubmenuClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionLab.MethodName.OnSubmenuShown && args.Count == 0)
			{
				this.OnSubmenuShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NPotionLab.MethodName.ClearPotions && args.Count == 0)
			{
				this.ClearPotions();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x0010C0DC File Offset: 0x0010A2DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NPotionLab.MethodName.Create && args.Count == 0)
			{
				NPotionLab npotionLab = NPotionLab.Create();
				ret = VariantUtils.CreateFrom<NPotionLab>(ref npotionLab);
				return true;
			}
			ret = default(godot_variant);
			return false;
		}

		/// <inheritdoc />
		// Token: 0x06002745 RID: 10053 RVA: 0x0010C11C File Offset: 0x0010A31C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NPotionLab.MethodName.Create || (ref method) == NPotionLab.MethodName._Ready || (ref method) == NPotionLab.MethodName._EnterTree || (ref method) == NPotionLab.MethodName._ExitTree || (ref method) == NPotionLab.MethodName.OnSubmenuOpened || (ref method) == NPotionLab.MethodName.OnSubmenuClosed || (ref method) == NPotionLab.MethodName.OnSubmenuShown || (ref method) == NPotionLab.MethodName.ClearPotions || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06002746 RID: 10054 RVA: 0x0010C1A8 File Offset: 0x0010A3A8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NPotionLab.PropertyName._screenContents)
			{
				this._screenContents = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
				return true;
			}
			if ((ref name) == NPotionLab.PropertyName._common)
			{
				this._common = VariantUtils.ConvertTo<NPotionLabCategory>(ref value);
				return true;
			}
			if ((ref name) == NPotionLab.PropertyName._uncommon)
			{
				this._uncommon = VariantUtils.ConvertTo<NPotionLabCategory>(ref value);
				return true;
			}
			if ((ref name) == NPotionLab.PropertyName._rare)
			{
				this._rare = VariantUtils.ConvertTo<NPotionLabCategory>(ref value);
				return true;
			}
			if ((ref name) == NPotionLab.PropertyName._special)
			{
				this._special = VariantUtils.ConvertTo<NPotionLabCategory>(ref value);
				return true;
			}
			if ((ref name) == NPotionLab.PropertyName._screenTween)
			{
				this._screenTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x06002747 RID: 10055 RVA: 0x0010C260 File Offset: 0x0010A460
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NPotionLab.PropertyName.InitialFocusedControl)
			{
				Control initialFocusedControl = this.InitialFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
				return true;
			}
			if ((ref name) == NPotionLab.PropertyName._screenContents)
			{
				value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._screenContents);
				return true;
			}
			if ((ref name) == NPotionLab.PropertyName._common)
			{
				value = VariantUtils.CreateFrom<NPotionLabCategory>(ref this._common);
				return true;
			}
			if ((ref name) == NPotionLab.PropertyName._uncommon)
			{
				value = VariantUtils.CreateFrom<NPotionLabCategory>(ref this._uncommon);
				return true;
			}
			if ((ref name) == NPotionLab.PropertyName._rare)
			{
				value = VariantUtils.CreateFrom<NPotionLabCategory>(ref this._rare);
				return true;
			}
			if ((ref name) == NPotionLab.PropertyName._special)
			{
				value = VariantUtils.CreateFrom<NPotionLabCategory>(ref this._special);
				return true;
			}
			if ((ref name) == NPotionLab.PropertyName._screenTween)
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
		// Token: 0x06002748 RID: 10056 RVA: 0x0010C358 File Offset: 0x0010A558
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NPotionLab.PropertyName._screenContents, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionLab.PropertyName._common, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionLab.PropertyName._uncommon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionLab.PropertyName._rare, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionLab.PropertyName._special, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionLab.PropertyName._screenTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NPotionLab.PropertyName.InitialFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002749 RID: 10057 RVA: 0x0010C454 File Offset: 0x0010A654
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NPotionLab.PropertyName._screenContents, Variant.From<NScrollableContainer>(ref this._screenContents));
			info.AddProperty(NPotionLab.PropertyName._common, Variant.From<NPotionLabCategory>(ref this._common));
			info.AddProperty(NPotionLab.PropertyName._uncommon, Variant.From<NPotionLabCategory>(ref this._uncommon));
			info.AddProperty(NPotionLab.PropertyName._rare, Variant.From<NPotionLabCategory>(ref this._rare));
			info.AddProperty(NPotionLab.PropertyName._special, Variant.From<NPotionLabCategory>(ref this._special));
			info.AddProperty(NPotionLab.PropertyName._screenTween, Variant.From<Tween>(ref this._screenTween));
		}

		/// <inheritdoc />
		// Token: 0x0600274A RID: 10058 RVA: 0x0010C4EC File Offset: 0x0010A6EC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NPotionLab.PropertyName._screenContents, ref variant))
			{
				this._screenContents = variant.As<NScrollableContainer>();
			}
			Variant variant2;
			if (info.TryGetProperty(NPotionLab.PropertyName._common, ref variant2))
			{
				this._common = variant2.As<NPotionLabCategory>();
			}
			Variant variant3;
			if (info.TryGetProperty(NPotionLab.PropertyName._uncommon, ref variant3))
			{
				this._uncommon = variant3.As<NPotionLabCategory>();
			}
			Variant variant4;
			if (info.TryGetProperty(NPotionLab.PropertyName._rare, ref variant4))
			{
				this._rare = variant4.As<NPotionLabCategory>();
			}
			Variant variant5;
			if (info.TryGetProperty(NPotionLab.PropertyName._special, ref variant5))
			{
				this._special = variant5.As<NPotionLabCategory>();
			}
			Variant variant6;
			if (info.TryGetProperty(NPotionLab.PropertyName._screenTween, ref variant6))
			{
				this._screenTween = variant6.As<Tween>();
			}
		}

		// Token: 0x04000E98 RID: 3736
		private static readonly string _scenePath = SceneHelper.GetScenePath("screens/potion_lab/potion_lab");

		// Token: 0x04000E99 RID: 3737
		private NScrollableContainer _screenContents;

		// Token: 0x04000E9A RID: 3738
		private NPotionLabCategory _common;

		// Token: 0x04000E9B RID: 3739
		private NPotionLabCategory _uncommon;

		// Token: 0x04000E9C RID: 3740
		private NPotionLabCategory _rare;

		// Token: 0x04000E9D RID: 3741
		private NPotionLabCategory _special;

		// Token: 0x04000E9E RID: 3742
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x04000E9F RID: 3743
		[Nullable(2)]
		private Tween _screenTween;

		// Token: 0x04000EA0 RID: 3744
		[Nullable(2)]
		private Task _loadTask;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200146B RID: 5227
		[NullableContext(0)]
		public new class MethodName : NSubmenu.MethodName
		{
			/// <summary>
			/// Cached name for the 'Create' method.
			/// </summary>
			// Token: 0x04004D7A RID: 19834
			public static readonly StringName Create = "Create";

			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004D7B RID: 19835
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04004D7C RID: 19836
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04004D7D RID: 19837
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnSubmenuOpened' method.
			/// </summary>
			// Token: 0x04004D7E RID: 19838
			public new static readonly StringName OnSubmenuOpened = "OnSubmenuOpened";

			/// <summary>
			/// Cached name for the 'OnSubmenuClosed' method.
			/// </summary>
			// Token: 0x04004D7F RID: 19839
			public new static readonly StringName OnSubmenuClosed = "OnSubmenuClosed";

			/// <summary>
			/// Cached name for the 'OnSubmenuShown' method.
			/// </summary>
			// Token: 0x04004D80 RID: 19840
			public new static readonly StringName OnSubmenuShown = "OnSubmenuShown";

			/// <summary>
			/// Cached name for the 'ClearPotions' method.
			/// </summary>
			// Token: 0x04004D81 RID: 19841
			public static readonly StringName ClearPotions = "ClearPotions";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200146C RID: 5228
		[NullableContext(0)]
		public new class PropertyName : NSubmenu.PropertyName
		{
			/// <summary>
			/// Cached name for the 'InitialFocusedControl' property.
			/// </summary>
			// Token: 0x04004D82 RID: 19842
			public new static readonly StringName InitialFocusedControl = "InitialFocusedControl";

			/// <summary>
			/// Cached name for the '_screenContents' field.
			/// </summary>
			// Token: 0x04004D83 RID: 19843
			public static readonly StringName _screenContents = "_screenContents";

			/// <summary>
			/// Cached name for the '_common' field.
			/// </summary>
			// Token: 0x04004D84 RID: 19844
			public static readonly StringName _common = "_common";

			/// <summary>
			/// Cached name for the '_uncommon' field.
			/// </summary>
			// Token: 0x04004D85 RID: 19845
			public static readonly StringName _uncommon = "_uncommon";

			/// <summary>
			/// Cached name for the '_rare' field.
			/// </summary>
			// Token: 0x04004D86 RID: 19846
			public static readonly StringName _rare = "_rare";

			/// <summary>
			/// Cached name for the '_special' field.
			/// </summary>
			// Token: 0x04004D87 RID: 19847
			public static readonly StringName _special = "_special";

			/// <summary>
			/// Cached name for the '_screenTween' field.
			/// </summary>
			// Token: 0x04004D88 RID: 19848
			public static readonly StringName _screenTween = "_screenTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200146D RID: 5229
		[NullableContext(0)]
		public new class SignalName : NSubmenu.SignalName
		{
		}

		// Token: 0x0200146E RID: 5230
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04004D89 RID: 19849
			[Nullable(0)]
			public static Func<ModelId, PotionModel> <0>__GetByIdOrNull;
		}
	}
}

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
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen
{
	/// <summary>
	/// Animates the Run Summary portion of the Death Screen.
	/// </summary>
	// Token: 0x0200031D RID: 797
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/GameOverScreen/NRunSummary.cs")]
	public class NRunSummary : Control
	{
		// Token: 0x06002C80 RID: 11392 RVA: 0x0013DBD4 File Offset: 0x0013BDD4
		public override void _Ready()
		{
			this._discoveryContainer = base.GetNode<Control>("%DiscoveryContainer");
			this._discoveryHeader = base.GetNode<Control>("%DiscoveryHeader");
			this._discoveredContents = base.GetNode<Control>("%DiscoveredContents");
			this._discoveredCards = base.GetNode<NDiscoveredItem>("%DiscoveredCards");
			this._discoveredRelics = base.GetNode<NDiscoveredItem>("%DiscoveredRelics");
			this._discoveredPotions = base.GetNode<NDiscoveredItem>("%DiscoveredPotions");
			this._discoveredEnemies = base.GetNode<NDiscoveredItem>("%DiscoveredEnemies");
			this._discoveredEpochs = base.GetNode<NDiscoveredItem>("%DiscoveredEpochs");
			this._discoveredCards.Visible = false;
			this._discoveredRelics.Visible = false;
			this._discoveredPotions.Visible = false;
			this._discoveredEnemies.Visible = false;
			this._discoveredEpochs.Visible = false;
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x0013DCD0 File Offset: 0x0013BED0
		public async Task AnimateInDiscoveries(RunState runState, CancellationToken ct)
		{
			Player player = LocalContext.GetMe(runState);
			if (player.DiscoveredCards.Count + player.DiscoveredRelics.Count + player.DiscoveredPotions.Count + player.DiscoveredEnemies.Count + player.DiscoveredEpochs.Count == 0)
			{
				Log.Info("No discoveries this time. Very sad", 2);
			}
			else
			{
				Tween tween = base.CreateTween();
				tween.TweenProperty(this._discoveryHeader, "modulate:a", 1f, 0.25);
				await Task.Delay(100, ct);
				if (this.IsValid())
				{
					if (player.DiscoveredCards.Count > 0)
					{
						string discoveryBodyText = NRunSummary.GetDiscoveryBodyText<ModelId>(player.DiscoveredCards, (ModelId id) => SaveUtil.CardOrDeprecated(id).Title, "game_over_screen", "DISCOVERY_BODY_CARD", "CardCount");
						this._discoveredCards.SetHoverTip(new HoverTip(new LocString("game_over_screen", "DISCOVERY_HEADER_CARD"), discoveryBodyText, null));
						this._discoveredCards.Visible = true;
						this._discoveredCards.Modulate = StsColors.transparentBlack;
					}
					if (player.DiscoveredRelics.Count > 0)
					{
						string discoveryBodyText2 = NRunSummary.GetDiscoveryBodyText<ModelId>(player.DiscoveredRelics, (ModelId id) => SaveUtil.RelicOrDeprecated(id).Title.GetFormattedText(), "game_over_screen", "DISCOVERY_BODY_RELIC", "RelicCount");
						this._discoveredRelics.SetHoverTip(new HoverTip(new LocString("game_over_screen", "DISCOVERY_HEADER_RELIC"), discoveryBodyText2, null));
						this._discoveredRelics.Visible = true;
						this._discoveredRelics.Modulate = StsColors.transparentBlack;
					}
					if (player.DiscoveredPotions.Count > 0)
					{
						string discoveryBodyText3 = NRunSummary.GetDiscoveryBodyText<ModelId>(player.DiscoveredPotions, (ModelId id) => SaveUtil.PotionOrDeprecated(id).Title.GetFormattedText(), "game_over_screen", "DISCOVERY_BODY_POTION", "PotionCount");
						this._discoveredPotions.SetHoverTip(new HoverTip(new LocString("game_over_screen", "DISCOVERY_HEADER_POTION"), discoveryBodyText3, null));
						this._discoveredPotions.Visible = true;
						this._discoveredPotions.Modulate = StsColors.transparentBlack;
					}
					if (player.DiscoveredEnemies.Count > 0)
					{
						string discoveryBodyText4 = NRunSummary.GetDiscoveryBodyText<ModelId>(player.DiscoveredEnemies, (ModelId id) => SaveUtil.MonsterOrDeprecated(id).Title.GetFormattedText(), "game_over_screen", "DISCOVERY_BODY_ENEMY", "EnemyCount");
						this._discoveredEnemies.SetHoverTip(new HoverTip(new LocString("game_over_screen", "DISCOVERY_HEADER_ENEMY"), discoveryBodyText4, null));
						this._discoveredEnemies.Visible = true;
						this._discoveredEnemies.Modulate = StsColors.transparentBlack;
					}
					if (player.DiscoveredEpochs.Count > 0)
					{
						LocString locString = new LocString("game_over_screen", "DISCOVERY_HEADER_EPOCH");
						LocString locString2 = new LocString("game_over_screen", "DISCOVERY_BODY_EPOCH");
						locString2.Add("EpochCount", player.DiscoveredEpochs.Count);
						HoverTip hoverTip = new HoverTip(locString, locString2, null);
						this._discoveredEpochs.SetHoverTip(hoverTip);
						this._discoveredEpochs.Visible = true;
						this._discoveredEpochs.Modulate = StsColors.transparentBlack;
					}
					if (this._discoveredCards.Visible)
					{
						NDiscoveredItem discoveredCards = this._discoveredCards;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
						defaultInterpolatedStringHandler.AppendFormatted<int>(player.DiscoveredCards.Count);
						discoveredCards.SetText(defaultInterpolatedStringHandler.ToStringAndClear());
						await TaskHelper.RunSafely(this.DiscoveryAnimHelper(this._discoveredCards));
					}
					if (this._discoveredRelics.Visible)
					{
						NDiscoveredItem discoveredRelics = this._discoveredRelics;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
						defaultInterpolatedStringHandler.AppendFormatted<int>(player.DiscoveredRelics.Count);
						discoveredRelics.SetText(defaultInterpolatedStringHandler.ToStringAndClear());
						await TaskHelper.RunSafely(this.DiscoveryAnimHelper(this._discoveredRelics));
					}
					if (this._discoveredPotions.Visible)
					{
						NDiscoveredItem discoveredPotions = this._discoveredPotions;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
						defaultInterpolatedStringHandler.AppendFormatted<int>(player.DiscoveredPotions.Count);
						discoveredPotions.SetText(defaultInterpolatedStringHandler.ToStringAndClear());
						await TaskHelper.RunSafely(this.DiscoveryAnimHelper(this._discoveredPotions));
					}
					if (this._discoveredEnemies.Visible)
					{
						NDiscoveredItem discoveredEnemies = this._discoveredEnemies;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
						defaultInterpolatedStringHandler.AppendFormatted<int>(player.DiscoveredEnemies.Count);
						discoveredEnemies.SetText(defaultInterpolatedStringHandler.ToStringAndClear());
						await TaskHelper.RunSafely(this.DiscoveryAnimHelper(this._discoveredEnemies));
					}
					if (this._discoveredEpochs.Visible)
					{
						NDiscoveredItem discoveredEpochs = this._discoveredEpochs;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
						defaultInterpolatedStringHandler.AppendFormatted<int>(player.DiscoveredEpochs.Count);
						discoveredEpochs.SetText(defaultInterpolatedStringHandler.ToStringAndClear());
						await TaskHelper.RunSafely(this.DiscoveryAnimHelper(this._discoveredEpochs));
					}
				}
			}
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x0013DD24 File Offset: 0x0013BF24
		private async Task DiscoveryAnimHelper(Control node)
		{
			node.Modulate = StsColors.transparentBlack;
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			this._tween = base.CreateTween().SetParallel(true);
			this._tween.TweenProperty(node, "modulate", Colors.White, 0.3);
			this._tween.TweenProperty(node, "position:y", 0f, 0.3).SetEase(1L).SetTrans(10L)
				.From(100f);
			bool flag = await this._tween.AwaitFinished(this);
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x0013DD70 File Offset: 0x0013BF70
		private static string GetDiscoveryBodyText<[Nullable(2)] T>(List<T> discoveredIds, Func<T, string> getTitle, string locTable, string locKey, string countParam)
		{
			LocString locString = new LocString(locTable, locKey);
			locString.Add(countParam, discoveredIds.Count);
			string text = string.Join("\n", discoveredIds.Take<T>(10).Select<T, string>(getTitle));
			if (discoveredIds.Count > 10)
			{
				text += "\n....";
			}
			return locString.GetFormattedText() + "\n\n" + text;
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002C84 RID: 11396 RVA: 0x0013DDD8 File Offset: 0x0013BFD8
		[Nullable(2)]
		public Control DefaultFocusedControl
		{
			[NullableContext(2)]
			get
			{
				return this._discoveredContents.GetChildren(false).OfType<Control>().FirstOrDefault<Control>((Control c) => c.IsVisible());
			}
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x0013DE10 File Offset: 0x0013C010
		[NullableContext(2)]
		public void SetControllerNav(Control focusNeighborTop)
		{
			Control[] array = (from c in this._discoveredContents.GetChildren(false).OfType<Control>()
				where c.IsVisible()
				select c).ToArray<Control>();
			for (int i = 0; i < array.Length; i++)
			{
				Control control = array[i];
				control.FocusNeighborTop = ((focusNeighborTop != null) ? focusNeighborTop.GetPath() : control.GetPath());
				control.FocusNeighborBottom = control.GetPath();
				Control control2 = control;
				NodePath nodePath;
				if (i <= 0)
				{
					Control[] array2 = array;
					nodePath = array2[array2.Length - 1].GetPath();
				}
				else
				{
					nodePath = array[i - 1].GetPath();
				}
				control2.FocusNeighborLeft = nodePath;
				control.FocusNeighborRight = ((i < array.Length - 1) ? array[i + 1].GetPath() : array[0].GetPath());
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002C86 RID: 11398 RVA: 0x0013DED4 File Offset: 0x0013C0D4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NRunSummary.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRunSummary.MethodName.SetControllerNav, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "focusNeighborTop", 0L, "", 6L, new StringName("Control"), false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C87 RID: 11399 RVA: 0x0013DF78 File Offset: 0x0013C178
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRunSummary.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRunSummary.MethodName.SetControllerNav && args.Count == 1)
			{
				this.SetControllerNav(VariantUtils.ConvertTo<Control>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002C88 RID: 11400 RVA: 0x0013DFE6 File Offset: 0x0013C1E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRunSummary.MethodName._Ready || (ref method) == NRunSummary.MethodName.SetControllerNav || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002C89 RID: 11401 RVA: 0x0013E010 File Offset: 0x0013C210
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRunSummary.PropertyName._discoveryContainer)
			{
				this._discoveryContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveryHeader)
			{
				this._discoveryHeader = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveredContents)
			{
				this._discoveredContents = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveredCards)
			{
				this._discoveredCards = VariantUtils.ConvertTo<NDiscoveredItem>(ref value);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveredRelics)
			{
				this._discoveredRelics = VariantUtils.ConvertTo<NDiscoveredItem>(ref value);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveredPotions)
			{
				this._discoveredPotions = VariantUtils.ConvertTo<NDiscoveredItem>(ref value);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveredEnemies)
			{
				this._discoveredEnemies = VariantUtils.ConvertTo<NDiscoveredItem>(ref value);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveredEpochs)
			{
				this._discoveredEpochs = VariantUtils.ConvertTo<NDiscoveredItem>(ref value);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._waitTween)
			{
				this._waitTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06002C8A RID: 11402 RVA: 0x0013E134 File Offset: 0x0013C334
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRunSummary.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveryContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._discoveryContainer);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveryHeader)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._discoveryHeader);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveredContents)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._discoveredContents);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveredCards)
			{
				value = VariantUtils.CreateFrom<NDiscoveredItem>(ref this._discoveredCards);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveredRelics)
			{
				value = VariantUtils.CreateFrom<NDiscoveredItem>(ref this._discoveredRelics);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveredPotions)
			{
				value = VariantUtils.CreateFrom<NDiscoveredItem>(ref this._discoveredPotions);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveredEnemies)
			{
				value = VariantUtils.CreateFrom<NDiscoveredItem>(ref this._discoveredEnemies);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._discoveredEpochs)
			{
				value = VariantUtils.CreateFrom<NDiscoveredItem>(ref this._discoveredEpochs);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NRunSummary.PropertyName._waitTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._waitTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002C8B RID: 11403 RVA: 0x0013E2AC File Offset: 0x0013C4AC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRunSummary.PropertyName._discoveryContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSummary.PropertyName._discoveryHeader, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSummary.PropertyName._discoveredContents, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSummary.PropertyName._discoveredCards, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSummary.PropertyName._discoveredRelics, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSummary.PropertyName._discoveredPotions, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSummary.PropertyName._discoveredEnemies, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSummary.PropertyName._discoveredEpochs, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSummary.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSummary.PropertyName._waitTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRunSummary.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002C8C RID: 11404 RVA: 0x0013E42C File Offset: 0x0013C62C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NRunSummary.PropertyName._discoveryContainer, Variant.From<Control>(ref this._discoveryContainer));
			info.AddProperty(NRunSummary.PropertyName._discoveryHeader, Variant.From<Control>(ref this._discoveryHeader));
			info.AddProperty(NRunSummary.PropertyName._discoveredContents, Variant.From<Control>(ref this._discoveredContents));
			info.AddProperty(NRunSummary.PropertyName._discoveredCards, Variant.From<NDiscoveredItem>(ref this._discoveredCards));
			info.AddProperty(NRunSummary.PropertyName._discoveredRelics, Variant.From<NDiscoveredItem>(ref this._discoveredRelics));
			info.AddProperty(NRunSummary.PropertyName._discoveredPotions, Variant.From<NDiscoveredItem>(ref this._discoveredPotions));
			info.AddProperty(NRunSummary.PropertyName._discoveredEnemies, Variant.From<NDiscoveredItem>(ref this._discoveredEnemies));
			info.AddProperty(NRunSummary.PropertyName._discoveredEpochs, Variant.From<NDiscoveredItem>(ref this._discoveredEpochs));
			info.AddProperty(NRunSummary.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NRunSummary.PropertyName._waitTween, Variant.From<Tween>(ref this._waitTween));
		}

		/// <inheritdoc />
		// Token: 0x06002C8D RID: 11405 RVA: 0x0013E51C File Offset: 0x0013C71C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRunSummary.PropertyName._discoveryContainer, ref variant))
			{
				this._discoveryContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRunSummary.PropertyName._discoveryHeader, ref variant2))
			{
				this._discoveryHeader = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRunSummary.PropertyName._discoveredContents, ref variant3))
			{
				this._discoveredContents = variant3.As<Control>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRunSummary.PropertyName._discoveredCards, ref variant4))
			{
				this._discoveredCards = variant4.As<NDiscoveredItem>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRunSummary.PropertyName._discoveredRelics, ref variant5))
			{
				this._discoveredRelics = variant5.As<NDiscoveredItem>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRunSummary.PropertyName._discoveredPotions, ref variant6))
			{
				this._discoveredPotions = variant6.As<NDiscoveredItem>();
			}
			Variant variant7;
			if (info.TryGetProperty(NRunSummary.PropertyName._discoveredEnemies, ref variant7))
			{
				this._discoveredEnemies = variant7.As<NDiscoveredItem>();
			}
			Variant variant8;
			if (info.TryGetProperty(NRunSummary.PropertyName._discoveredEpochs, ref variant8))
			{
				this._discoveredEpochs = variant8.As<NDiscoveredItem>();
			}
			Variant variant9;
			if (info.TryGetProperty(NRunSummary.PropertyName._tween, ref variant9))
			{
				this._tween = variant9.As<Tween>();
			}
			Variant variant10;
			if (info.TryGetProperty(NRunSummary.PropertyName._waitTween, ref variant10))
			{
				this._waitTween = variant10.As<Tween>();
			}
		}

		// Token: 0x040010DB RID: 4315
		private Control _discoveryContainer;

		// Token: 0x040010DC RID: 4316
		private Control _discoveryHeader;

		// Token: 0x040010DD RID: 4317
		private Control _discoveredContents;

		// Token: 0x040010DE RID: 4318
		private NDiscoveredItem _discoveredCards;

		// Token: 0x040010DF RID: 4319
		private NDiscoveredItem _discoveredRelics;

		// Token: 0x040010E0 RID: 4320
		private NDiscoveredItem _discoveredPotions;

		// Token: 0x040010E1 RID: 4321
		private NDiscoveredItem _discoveredEnemies;

		// Token: 0x040010E2 RID: 4322
		private NDiscoveredItem _discoveredEpochs;

		// Token: 0x040010E3 RID: 4323
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040010E4 RID: 4324
		[Nullable(2)]
		private Tween _waitTween;

		// Token: 0x040010E5 RID: 4325
		private const int _maxItemsToList = 10;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200155E RID: 5470
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040051EC RID: 20972
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetControllerNav' method.
			/// </summary>
			// Token: 0x040051ED RID: 20973
			public static readonly StringName SetControllerNav = "SetControllerNav";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200155F RID: 5471
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040051EE RID: 20974
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_discoveryContainer' field.
			/// </summary>
			// Token: 0x040051EF RID: 20975
			public static readonly StringName _discoveryContainer = "_discoveryContainer";

			/// <summary>
			/// Cached name for the '_discoveryHeader' field.
			/// </summary>
			// Token: 0x040051F0 RID: 20976
			public static readonly StringName _discoveryHeader = "_discoveryHeader";

			/// <summary>
			/// Cached name for the '_discoveredContents' field.
			/// </summary>
			// Token: 0x040051F1 RID: 20977
			public static readonly StringName _discoveredContents = "_discoveredContents";

			/// <summary>
			/// Cached name for the '_discoveredCards' field.
			/// </summary>
			// Token: 0x040051F2 RID: 20978
			public static readonly StringName _discoveredCards = "_discoveredCards";

			/// <summary>
			/// Cached name for the '_discoveredRelics' field.
			/// </summary>
			// Token: 0x040051F3 RID: 20979
			public static readonly StringName _discoveredRelics = "_discoveredRelics";

			/// <summary>
			/// Cached name for the '_discoveredPotions' field.
			/// </summary>
			// Token: 0x040051F4 RID: 20980
			public static readonly StringName _discoveredPotions = "_discoveredPotions";

			/// <summary>
			/// Cached name for the '_discoveredEnemies' field.
			/// </summary>
			// Token: 0x040051F5 RID: 20981
			public static readonly StringName _discoveredEnemies = "_discoveredEnemies";

			/// <summary>
			/// Cached name for the '_discoveredEpochs' field.
			/// </summary>
			// Token: 0x040051F6 RID: 20982
			public static readonly StringName _discoveredEpochs = "_discoveredEpochs";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x040051F7 RID: 20983
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_waitTween' field.
			/// </summary>
			// Token: 0x040051F8 RID: 20984
			public static readonly StringName _waitTween = "_waitTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001560 RID: 5472
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}

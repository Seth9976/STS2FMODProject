using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Relics
{
	/// <summary>
	/// Shows the relic in a relic container. The full relic experience; shows the amount, can flash, and can animate in
	/// from an arbitrary position.
	/// This is used in the relic display under the top bar during a run.
	/// </summary>
	// Token: 0x02000379 RID: 889
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Relics/NRelicInventoryHolder.cs")]
	public class NRelicInventoryHolder : NButton
	{
		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x0600349D RID: 13469 RVA: 0x00187D38 File Offset: 0x00185F38
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlyArray<string>(new string[]
				{
					NRelicInventoryHolder._scenePath,
					NRelicInventoryHolder._flashPath
				});
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x0600349E RID: 13470 RVA: 0x00187D55 File Offset: 0x00185F55
		public NRelic Relic
		{
			get
			{
				return this._relic;
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x0600349F RID: 13471 RVA: 0x00187D5D File Offset: 0x00185F5D
		// (set) Token: 0x060034A0 RID: 13472 RVA: 0x00187D65 File Offset: 0x00185F65
		public NRelicInventory Inventory { get; set; }

		// Token: 0x060034A1 RID: 13473 RVA: 0x00187D70 File Offset: 0x00185F70
		[return: Nullable(2)]
		public static NRelicInventoryHolder Create(RelicModel relic)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			NRelicInventoryHolder nrelicInventoryHolder = PreloadManager.Cache.GetScene(NRelicInventoryHolder._scenePath).Instantiate<NRelicInventoryHolder>(0L);
			Node node = nrelicInventoryHolder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("NRelicContainerHolder-");
			defaultInterpolatedStringHandler.AppendFormatted<ModelId>(relic.Id);
			node.Name = defaultInterpolatedStringHandler.ToStringAndClear();
			nrelicInventoryHolder._model = relic;
			return nrelicInventoryHolder;
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x00187DDC File Offset: 0x00185FDC
		public override void _Ready()
		{
			this.ConnectSignals();
			this._relic = base.GetNode<NRelic>("%Relic");
			this._amountLabel = base.GetNode<MegaLabel>("%AmountLabel");
			this._originalIconPosition = this._relic.Icon.Position;
			this._relic.ModelChanged += this.OnModelChanged;
			this._relic.Model = this._model;
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x00187E5C File Offset: 0x0018605C
		public override void _ExitTree()
		{
			base._ExitTree();
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			CancellationTokenSource cancellationTokenSource = this._cancellationTokenSource;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			if (this._subscribedRelic != null)
			{
				this._subscribedRelic.DisplayAmountChanged -= this.OnDisplayAmountChanged;
				this._subscribedRelic.StatusChanged -= this.OnStatusChanged;
				this._subscribedRelic.Flashed -= this.OnRelicFlashed;
			}
			this._subscribedRelic = null;
			this._relic.ModelChanged -= this.OnModelChanged;
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x00187EFC File Offset: 0x001860FC
		[NullableContext(2)]
		private void OnModelChanged(RelicModel oldModel, RelicModel newModel)
		{
			if (oldModel != null)
			{
				oldModel.DisplayAmountChanged -= this.OnDisplayAmountChanged;
				oldModel.StatusChanged -= this.OnStatusChanged;
				oldModel.Flashed -= this.OnRelicFlashed;
			}
			if (newModel != null)
			{
				newModel.DisplayAmountChanged += this.OnDisplayAmountChanged;
				newModel.StatusChanged += this.OnStatusChanged;
				newModel.Flashed += this.OnRelicFlashed;
			}
			this.RefreshAmount();
			this.RefreshStatus();
			this._subscribedRelic = newModel;
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x00187F90 File Offset: 0x00186190
		private void RefreshAmount()
		{
			if (this._relic.Model.ShowCounter && RunManager.Instance.IsInProgress)
			{
				this._amountLabel.Visible = true;
				this._amountLabel.SetTextAutoSize(this._relic.Model.DisplayAmount.ToString());
				return;
			}
			this._amountLabel.Visible = false;
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x00187FF8 File Offset: 0x001861F8
		private void RefreshStatus()
		{
			if (!RunManager.Instance.IsInProgress)
			{
				this._relic.Icon.Modulate = Colors.White;
				return;
			}
			this._relic.Model.UpdateTexture(this._relic.Icon);
			TextureRect icon = this._relic.Icon;
			RelicStatus status = this._relic.Model.Status;
			Color color;
			if (status > RelicStatus.Active)
			{
				if (status != RelicStatus.Disabled)
				{
					throw new ArgumentOutOfRangeException();
				}
				color = new Color("#808080");
			}
			else
			{
				color = Colors.White;
			}
			icon.Modulate = color;
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x0018808C File Offset: 0x0018628C
		public async Task PlayNewlyAcquiredAnimation(Vector2? startLocation, Vector2? startScale)
		{
			if (this._cancellationTokenSource != null)
			{
				await this._cancellationTokenSource.CancelAsync();
			}
			CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
			this._cancellationTokenSource = cancelTokenSource;
			await this.AwaitProcessFrame(cancelTokenSource.Token);
			if (!cancelTokenSource.IsCancellationRequested)
			{
				Tween obtainedTween = this._obtainedTween;
				if (obtainedTween != null)
				{
					obtainedTween.Kill();
				}
				if (startLocation == null)
				{
					Control icon = this._relic.Icon;
					Vector2 position = this._relic.Icon.Position;
					position.Y = this._relic.Icon.Position.Y + 40f;
					icon.Position = position;
					CanvasItem icon2 = this._relic.Icon;
					Color color = this._relic.Icon.Modulate;
					color.A = 0f;
					icon2.Modulate = color;
					this._obtainedTween = base.GetTree().CreateTween();
					this._obtainedTween.TweenProperty(this._relic.Icon, "modulate:a", 1f, 0.10000000149011612);
					this._obtainedTween.Parallel();
					this._obtainedTween.SetEase(1L);
					this._obtainedTween.SetTrans(10L);
					this._obtainedTween.TweenProperty(this._relic.Icon, "position:y", this._originalIconPosition.Y, 0.3499999940395355);
					this._obtainedTween.TweenCallback(Callable.From(new Action(this.DoFlash)));
				}
				else
				{
					this._relic.Icon.GlobalPosition = startLocation.Value;
					this._relic.Icon.Scale = startScale ?? Vector2.One;
					CanvasItem icon3 = this._relic.Icon;
					Color color = this._relic.Icon.Modulate;
					color.A = 1f;
					icon3.Modulate = color;
					this._obtainedTween = base.GetTree().CreateTween();
					this._obtainedTween.SetEase(1L);
					this._obtainedTween.SetTrans(1L);
					this._obtainedTween.TweenProperty(this._relic.Icon, "position", this._originalIconPosition, 0.3499999940395355);
					this._obtainedTween.Parallel().TweenProperty(this._relic.Icon, "scale", Vector2.One, 0.3499999940395355);
					this._obtainedTween.TweenCallback(Callable.From(new Action(this.DoFlash)));
				}
			}
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x001880E0 File Offset: 0x001862E0
		protected override void OnFocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._relic.Icon, "scale", Vector2.One * 1.25f, 0.05);
			NHoverTipSet nhoverTipSet = NHoverTipSet.CreateAndShow(this, this._relic.Model.HoverTips, HoverTipAlignment.None);
			if (nhoverTipSet != null)
			{
				nhoverTipSet.SetAlignmentForRelic(this._relic);
			}
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x00188170 File Offset: 0x00186370
		protected override void OnUnfocus()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween();
			this._hoverTween.TweenProperty(this._relic.Icon, "scale", Vector2.One, 1.0).SetEase(1L).SetTrans(5L);
			NHoverTipSet.Remove(this);
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x001881E2 File Offset: 0x001863E2
		private void OnRelicFlashed(RelicModel _, IEnumerable<Creature> __)
		{
			this.DoFlash();
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x001881EC File Offset: 0x001863EC
		private void DoFlash()
		{
			Node2D node2D = PreloadManager.Cache.GetScene(NRelicInventoryHolder._flashPath).Instantiate<Node2D>(0L);
			Node aboveTopBarVfxContainer = NRun.Instance.GlobalUi.AboveTopBarVfxContainer;
			node2D.GetNode<GpuParticles2D>("Particles").Texture = this._relic.Model.Icon;
			node2D.GlobalPosition = base.GlobalPosition + base.Size * 0.5f;
			aboveTopBarVfxContainer.AddChildSafely(node2D);
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x0018826D File Offset: 0x0018646D
		private void OnDisplayAmountChanged()
		{
			this.RefreshAmount();
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x00188275 File Offset: 0x00186475
		private void OnStatusChanged()
		{
			this.RefreshStatus();
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060034AE RID: 13486 RVA: 0x00188280 File Offset: 0x00186480
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(9)
			{
				new MethodInfo(NRelicInventoryHolder.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventoryHolder.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventoryHolder.MethodName.RefreshAmount, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventoryHolder.MethodName.RefreshStatus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventoryHolder.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventoryHolder.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventoryHolder.MethodName.DoFlash, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventoryHolder.MethodName.OnDisplayAmountChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NRelicInventoryHolder.MethodName.OnStatusChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x060034AF RID: 13487 RVA: 0x00188440 File Offset: 0x00186640
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NRelicInventoryHolder.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventoryHolder.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventoryHolder.MethodName.RefreshAmount && args.Count == 0)
			{
				this.RefreshAmount();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventoryHolder.MethodName.RefreshStatus && args.Count == 0)
			{
				this.RefreshStatus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventoryHolder.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventoryHolder.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventoryHolder.MethodName.DoFlash && args.Count == 0)
			{
				this.DoFlash();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventoryHolder.MethodName.OnDisplayAmountChanged && args.Count == 0)
			{
				this.OnDisplayAmountChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NRelicInventoryHolder.MethodName.OnStatusChanged && args.Count == 0)
			{
				this.OnStatusChanged();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x060034B0 RID: 13488 RVA: 0x001885A4 File Offset: 0x001867A4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NRelicInventoryHolder.MethodName._Ready || (ref method) == NRelicInventoryHolder.MethodName._ExitTree || (ref method) == NRelicInventoryHolder.MethodName.RefreshAmount || (ref method) == NRelicInventoryHolder.MethodName.RefreshStatus || (ref method) == NRelicInventoryHolder.MethodName.OnFocus || (ref method) == NRelicInventoryHolder.MethodName.OnUnfocus || (ref method) == NRelicInventoryHolder.MethodName.DoFlash || (ref method) == NRelicInventoryHolder.MethodName.OnDisplayAmountChanged || (ref method) == NRelicInventoryHolder.MethodName.OnStatusChanged || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x060034B1 RID: 13489 RVA: 0x00188640 File Offset: 0x00186840
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NRelicInventoryHolder.PropertyName.Inventory)
			{
				this.Inventory = VariantUtils.ConvertTo<NRelicInventory>(ref value);
				return true;
			}
			if ((ref name) == NRelicInventoryHolder.PropertyName._relic)
			{
				this._relic = VariantUtils.ConvertTo<NRelic>(ref value);
				return true;
			}
			if ((ref name) == NRelicInventoryHolder.PropertyName._amountLabel)
			{
				this._amountLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NRelicInventoryHolder.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NRelicInventoryHolder.PropertyName._obtainedTween)
			{
				this._obtainedTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NRelicInventoryHolder.PropertyName._originalIconPosition)
			{
				this._originalIconPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x060034B2 RID: 13490 RVA: 0x001886F8 File Offset: 0x001868F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NRelicInventoryHolder.PropertyName.Relic)
			{
				NRelic relic = this.Relic;
				value = VariantUtils.CreateFrom<NRelic>(ref relic);
				return true;
			}
			if ((ref name) == NRelicInventoryHolder.PropertyName.Inventory)
			{
				NRelicInventory inventory = this.Inventory;
				value = VariantUtils.CreateFrom<NRelicInventory>(ref inventory);
				return true;
			}
			if ((ref name) == NRelicInventoryHolder.PropertyName._relic)
			{
				value = VariantUtils.CreateFrom<NRelic>(ref this._relic);
				return true;
			}
			if ((ref name) == NRelicInventoryHolder.PropertyName._amountLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._amountLabel);
				return true;
			}
			if ((ref name) == NRelicInventoryHolder.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NRelicInventoryHolder.PropertyName._obtainedTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._obtainedTween);
				return true;
			}
			if ((ref name) == NRelicInventoryHolder.PropertyName._originalIconPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._originalIconPosition);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x060034B3 RID: 13491 RVA: 0x001887F4 File Offset: 0x001869F4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NRelicInventoryHolder.PropertyName._relic, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicInventoryHolder.PropertyName._amountLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicInventoryHolder.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicInventoryHolder.PropertyName._obtainedTween, 0L, "", 4096L, false),
				new PropertyInfo(5L, NRelicInventoryHolder.PropertyName._originalIconPosition, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicInventoryHolder.PropertyName.Relic, 0L, "", 4096L, false),
				new PropertyInfo(24L, NRelicInventoryHolder.PropertyName.Inventory, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x060034B4 RID: 13492 RVA: 0x001888F0 File Offset: 0x00186AF0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName inventory = NRelicInventoryHolder.PropertyName.Inventory;
			NRelicInventory inventory2 = this.Inventory;
			info.AddProperty(inventory, Variant.From<NRelicInventory>(ref inventory2));
			info.AddProperty(NRelicInventoryHolder.PropertyName._relic, Variant.From<NRelic>(ref this._relic));
			info.AddProperty(NRelicInventoryHolder.PropertyName._amountLabel, Variant.From<MegaLabel>(ref this._amountLabel));
			info.AddProperty(NRelicInventoryHolder.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NRelicInventoryHolder.PropertyName._obtainedTween, Variant.From<Tween>(ref this._obtainedTween));
			info.AddProperty(NRelicInventoryHolder.PropertyName._originalIconPosition, Variant.From<Vector2>(ref this._originalIconPosition));
		}

		/// <inheritdoc />
		// Token: 0x060034B5 RID: 13493 RVA: 0x0018898C File Offset: 0x00186B8C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NRelicInventoryHolder.PropertyName.Inventory, ref variant))
			{
				this.Inventory = variant.As<NRelicInventory>();
			}
			Variant variant2;
			if (info.TryGetProperty(NRelicInventoryHolder.PropertyName._relic, ref variant2))
			{
				this._relic = variant2.As<NRelic>();
			}
			Variant variant3;
			if (info.TryGetProperty(NRelicInventoryHolder.PropertyName._amountLabel, ref variant3))
			{
				this._amountLabel = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NRelicInventoryHolder.PropertyName._hoverTween, ref variant4))
			{
				this._hoverTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NRelicInventoryHolder.PropertyName._obtainedTween, ref variant5))
			{
				this._obtainedTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NRelicInventoryHolder.PropertyName._originalIconPosition, ref variant6))
			{
				this._originalIconPosition = variant6.As<Vector2>();
			}
		}

		// Token: 0x04001465 RID: 5221
		private static readonly string _scenePath = SceneHelper.GetScenePath("relics/relic_inventory_holder");

		// Token: 0x04001466 RID: 5222
		private static readonly string _flashPath = SceneHelper.GetScenePath("vfx/relic_inventory_flash_vfx");

		// Token: 0x04001467 RID: 5223
		private const float _newlyAcquiredPopDuration = 0.35f;

		// Token: 0x04001468 RID: 5224
		private const float _newlyAcquiredFadeInDuration = 0.1f;

		// Token: 0x04001469 RID: 5225
		private const float _newlyAcquiredPopDistance = 40f;

		// Token: 0x0400146A RID: 5226
		private NRelic _relic;

		// Token: 0x0400146B RID: 5227
		[Nullable(2)]
		private RelicModel _subscribedRelic;

		// Token: 0x0400146C RID: 5228
		private MegaLabel _amountLabel;

		// Token: 0x0400146D RID: 5229
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x0400146E RID: 5230
		[Nullable(2)]
		private Tween _obtainedTween;

		// Token: 0x0400146F RID: 5231
		[Nullable(2)]
		private CancellationTokenSource _cancellationTokenSource;

		// Token: 0x04001470 RID: 5232
		private Vector2 _originalIconPosition;

		// Token: 0x04001471 RID: 5233
		private RelicModel _model;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016D3 RID: 5843
		[NullableContext(0)]
		public new class MethodName : NButton.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040058EE RID: 22766
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040058EF RID: 22767
			public new static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'RefreshAmount' method.
			/// </summary>
			// Token: 0x040058F0 RID: 22768
			public static readonly StringName RefreshAmount = "RefreshAmount";

			/// <summary>
			/// Cached name for the 'RefreshStatus' method.
			/// </summary>
			// Token: 0x040058F1 RID: 22769
			public static readonly StringName RefreshStatus = "RefreshStatus";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x040058F2 RID: 22770
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x040058F3 RID: 22771
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'DoFlash' method.
			/// </summary>
			// Token: 0x040058F4 RID: 22772
			public static readonly StringName DoFlash = "DoFlash";

			/// <summary>
			/// Cached name for the 'OnDisplayAmountChanged' method.
			/// </summary>
			// Token: 0x040058F5 RID: 22773
			public static readonly StringName OnDisplayAmountChanged = "OnDisplayAmountChanged";

			/// <summary>
			/// Cached name for the 'OnStatusChanged' method.
			/// </summary>
			// Token: 0x040058F6 RID: 22774
			public static readonly StringName OnStatusChanged = "OnStatusChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016D4 RID: 5844
		[NullableContext(0)]
		public new class PropertyName : NButton.PropertyName
		{
			/// <summary>
			/// Cached name for the 'Relic' property.
			/// </summary>
			// Token: 0x040058F7 RID: 22775
			public static readonly StringName Relic = "Relic";

			/// <summary>
			/// Cached name for the 'Inventory' property.
			/// </summary>
			// Token: 0x040058F8 RID: 22776
			public static readonly StringName Inventory = "Inventory";

			/// <summary>
			/// Cached name for the '_relic' field.
			/// </summary>
			// Token: 0x040058F9 RID: 22777
			public static readonly StringName _relic = "_relic";

			/// <summary>
			/// Cached name for the '_amountLabel' field.
			/// </summary>
			// Token: 0x040058FA RID: 22778
			public static readonly StringName _amountLabel = "_amountLabel";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x040058FB RID: 22779
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_obtainedTween' field.
			/// </summary>
			// Token: 0x040058FC RID: 22780
			public static readonly StringName _obtainedTween = "_obtainedTween";

			/// <summary>
			/// Cached name for the '_originalIconPosition' field.
			/// </summary>
			// Token: 0x040058FD RID: 22781
			public static readonly StringName _originalIconPosition = "_originalIconPosition";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020016D5 RID: 5845
		[NullableContext(0)]
		public new class SignalName : NButton.SignalName
		{
		}
	}
}

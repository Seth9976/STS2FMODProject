using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere
{
	/// <summary>
	/// The UI node for the Crystal Sphere minigame.
	/// </summary>
	// Token: 0x020003C4 RID: 964
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Events/Custom/CrystalSphere/NCrystalSphereScreen.cs")]
	public class NCrystalSphereScreen : Control, IOverlayScreen, IScreenContext
	{
		// Token: 0x06003A3D RID: 14909 RVA: 0x001BA974 File Offset: 0x001B8B74
		public static NCrystalSphereScreen ShowScreen(CrystalSphereMinigame grid)
		{
			NCrystalSphereScreen ncrystalSphereScreen = PreloadManager.Cache.GetScene("res://scenes/events/custom/crystal_sphere/crystal_sphere_screen.tscn").Instantiate<NCrystalSphereScreen>(0L);
			ncrystalSphereScreen._entity = grid;
			NOverlayStack.Instance.Push(ncrystalSphereScreen);
			return ncrystalSphereScreen;
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x001BA9AC File Offset: 0x001B8BAC
		public override void _Ready()
		{
			this._itemsContainer = base.GetNode<Control>("%Items");
			this._cellContainer = base.GetNode<Control>("%Cells");
			this._bigDivinationButton = base.GetNode<NDivinationButton>("%BigDivinationButton");
			this._smallDivinationButton = base.GetNode<NDivinationButton>("%SmallDivinationButton");
			this._bigDivinationButton.SetLabel(new LocString("events", "CRYSTAL_SPHERE.button.DIVINATION_LABEL_BIG"));
			this._smallDivinationButton.SetLabel(new LocString("events", "CRYSTAL_SPHERE.button.DIVINATION_LABEL_SMALL"));
			this._divinationsLeftLabel = base.GetNode<MegaRichTextLabel>("%DivinationsLeft");
			this._mask = base.GetNode<NCrystalSphereMask>("%ScryMask");
			this._proceedButton = base.GetNode<NProceedButton>("%ProceedButton");
			this._instructionsTitleLabel = base.GetNode<MegaRichTextLabel>("%InstructionsTitle");
			this._instructionsDescriptionLabel = base.GetNode<MegaRichTextLabel>("%InstructionsDescription");
			this._instructionsContainer = base.GetNode<Control>("%Instructions");
			this._instructionsTitleLabel.SetTextAutoSize(this._instructionsTitleLoc.GetFormattedText());
			this._instructionsDescriptionLabel.SetTextAutoSize(this._instructionsDescriptionLoc.GetFormattedText());
			this._dialogue = base.GetNode<NCrystalSphereDialogue>("%Dialogue");
			Vector2 vector = Vector2.One * (float)(-(float)(57 * this._entity.GridSize.X)) * 0.5f;
			NCrystalSphereCell[,] array = new NCrystalSphereCell[this._entity.GridSize.X, this._entity.GridSize.Y];
			for (int i = 0; i < this._entity.GridSize.X; i++)
			{
				for (int j = 0; j < this._entity.GridSize.Y; j++)
				{
					NCrystalSphereCell cell = NCrystalSphereCell.Create(this._entity.cells[i, j], this._mask);
					this._cellContainer.AddChildSafely(cell);
					array[i, j] = cell;
					cell.Position = vector + 57f * new Vector2((float)i, (float)j);
					cell.Connect(Control.SignalName.MouseEntered, Callable.From(delegate
					{
						this.OnHoverCell(cell);
					}), 0U);
					cell.Connect(Control.SignalName.MouseExited, Callable.From(delegate
					{
						this.OnUnhoverCell(cell);
					}), 0U);
					cell.Connect(Control.SignalName.FocusEntered, Callable.From(delegate
					{
						this.OnHoverCell(cell);
					}), 0U);
					cell.Connect(Control.SignalName.FocusExited, Callable.From(delegate
					{
						this.OnUnhoverCell(cell);
					}), 0U);
					cell.Connect(NClickableControl.SignalName.MouseReleased, Callable.From<InputEvent>(delegate(InputEvent _)
					{
						TaskHelper.RunSafely(this.OnCellClicked(cell));
					}), 0U);
					cell.Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>(delegate(NClickableControl _)
					{
						TaskHelper.RunSafely(this.OnCellClicked(cell));
					}), 0U);
				}
			}
			foreach (CrystalSphereItem crystalSphereItem in this._entity.Items)
			{
				NCrystalSphereItem ncrystalSphereItem = NCrystalSphereItem.Create(crystalSphereItem);
				ncrystalSphereItem.Size = crystalSphereItem.Size * 57;
				this._itemsContainer.AddChildSafely(ncrystalSphereItem);
				ncrystalSphereItem.Position = vector + 57f * new Vector2((float)crystalSphereItem.Position.X, (float)crystalSphereItem.Position.Y);
				crystalSphereItem.Revealed += this.OnItemRevealed;
			}
			this._bigDivinationButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.SetBigDivination)), 0U);
			this._smallDivinationButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.SetSmallDivination)), 0U);
			this._proceedButton.Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnProceedButtonPressed)), 0U);
			this._smallDivinationButton.SetHotkeys(new string[] { MegaInput.viewDrawPile });
			this._bigDivinationButton.SetHotkeys(new string[] { MegaInput.viewDiscardPile });
			this.UpdateDivinationsLeft();
			this._proceedButton.Disable();
			this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
			for (int k = 0; k < array.GetLength(0); k++)
			{
				for (int l = 0; l < array.GetLength(1); l++)
				{
					Control control = array[k, l];
					control.FocusNeighborTop = ((l > 0) ? array[k, l - 1].GetPath() : array[k, l].GetPath());
					control.FocusNeighborBottom = ((l < array.GetLength(1) - 1) ? array[k, l + 1].GetPath() : array[k, l].GetPath());
					control.FocusNeighborLeft = ((k > 0) ? array[k - 1, l].GetPath() : array[k, l].GetPath());
					control.FocusNeighborRight = ((k < array.GetLength(0) - 1) ? array[k + 1, l].GetPath() : array[k, l].GetPath());
				}
			}
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x001BAF8C File Offset: 0x001B918C
		private void SetBigDivination(NButton obj)
		{
			this._bigDivinationButton.SetActive(true);
			this._smallDivinationButton.SetActive(false);
			this._entity.SetTool(CrystalSphereMinigame.CrystalSphereToolType.Big);
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x001BAFB2 File Offset: 0x001B91B2
		private void SetSmallDivination(NButton obj)
		{
			this._smallDivinationButton.SetActive(true);
			this._bigDivinationButton.SetActive(false);
			this._entity.SetTool(CrystalSphereMinigame.CrystalSphereToolType.Small);
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x001BAFD8 File Offset: 0x001B91D8
		public override void _EnterTree()
		{
			this._entity.DivinationCountChanged += this.UpdateDivinationsLeft;
			this._entity.Finished += this.OnMinigameFinished;
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x001BB008 File Offset: 0x001B9208
		public override void _ExitTree()
		{
			this._entity.DivinationCountChanged -= this.UpdateDivinationsLeft;
			this._entity.Finished -= this.OnMinigameFinished;
			Tween fadeTween = this._fadeTween;
			if (fadeTween != null)
			{
				fadeTween.Kill();
			}
			foreach (CrystalSphereItem crystalSphereItem in this._entity.Items)
			{
				crystalSphereItem.Revealed -= this.OnItemRevealed;
			}
			this._entity.ForceMinigameEnd();
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x001BB0B0 File Offset: 0x001B92B0
		private void OnItemRevealed(CrystalSphereItem item)
		{
			if (item.IsGood)
			{
				this._dialogue.PlayGood();
				return;
			}
			this._dialogue.PlayBad();
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x001BB0D4 File Offset: 0x001B92D4
		private async Task OnCellClicked(NCrystalSphereCell cell)
		{
			if (this._entity.DivinationCount > 0)
			{
				this.UpdateDivinationsLeft();
				await this._entity.CellClicked(cell.Entity);
				List<NCrystalSphereCell> list = (from c in this._cellContainer.GetChildren(false).OfType<NCrystalSphereCell>()
					where c.Entity.IsHidden
					select c).ToList<NCrystalSphereCell>();
				list.OrderBy<NCrystalSphereCell, float>((NCrystalSphereCell c1) => new Vector2I(cell.Entity.X, cell.Entity.Y).DistanceTo(new Vector2I(c1.Entity.X, c1.Entity.Y))).First<NCrystalSphereCell>().TryGrabFocus();
			}
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x001BB11F File Offset: 0x001B931F
		private void OnHoverCell(NCrystalSphereCell cell)
		{
			if (!this._entity.IsFinished)
			{
				this._entity.SetHoveredCell(cell.Entity);
			}
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x001BB13F File Offset: 0x001B933F
		private void OnUnhoverCell(NCrystalSphereCell cell)
		{
			this._entity.UnsetHoveredCell();
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x001BB14C File Offset: 0x001B934C
		private void UpdateDivinationsLeft()
		{
			this._divinationsRemainLoc.Add("Count", this._entity.DivinationCount);
			this._divinationsLeftLabel.Text = this._divinationsRemainLoc.GetFormattedText() ?? "";
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x001BB198 File Offset: 0x001B9398
		private void OnMinigameFinished()
		{
			this._bigDivinationButton.Visible = false;
			this._smallDivinationButton.Visible = false;
			this._divinationsLeftLabel.Visible = false;
			this._instructionsContainer.Visible = false;
			this._proceedButton.Visible = true;
			this._dialogue.PlayEnd();
			this._proceedButton.Enable();
			NMapScreen.Instance.SetTravelEnabled(true);
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x001BB202 File Offset: 0x001B9402
		private void OnProceedButtonPressed(NButton _)
		{
			TaskHelper.RunSafely(RunManager.Instance.ProceedFromTerminalRewardsScreen());
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06003A4A RID: 14922 RVA: 0x001BB214 File Offset: 0x001B9414
		public NetScreenType ScreenType
		{
			get
			{
				return NetScreenType.None;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06003A4B RID: 14923 RVA: 0x001BB217 File Offset: 0x001B9417
		public bool UseSharedBackstop
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x001BB21C File Offset: 0x001B941C
		public void AfterOverlayOpened()
		{
			this._itemsContainer.Visible = false;
			Tween fadeTween = this._fadeTween;
			if (fadeTween != null)
			{
				fadeTween.FastForwardToCompletion();
			}
			this._fadeTween = base.CreateTween();
			this._fadeTween.TweenProperty(this, "modulate:a", 1.0, 0.5).From(0f);
			this._fadeTween.Chain().TweenCallback(Callable.From(delegate
			{
				this._dialogue.PlayStart();
				this._itemsContainer.Visible = true;
			}));
		}

		// Token: 0x06003A4D RID: 14925 RVA: 0x001BB2B1 File Offset: 0x001B94B1
		public void AfterOverlayClosed()
		{
			Tween fadeTween = this._fadeTween;
			if (fadeTween != null)
			{
				fadeTween.FastForwardToCompletion();
			}
			this.QueueFreeSafely();
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x001BB2CA File Offset: 0x001B94CA
		public void AfterOverlayShown()
		{
			if (this._entity.IsFinished)
			{
				this._proceedButton.Enable();
			}
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x001BB2E4 File Offset: 0x001B94E4
		public void AfterOverlayHidden()
		{
			this._proceedButton.Disable();
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06003A50 RID: 14928 RVA: 0x001BB2F4 File Offset: 0x001B94F4
		public Control DefaultFocusedControl
		{
			get
			{
				List<NCrystalSphereCell> list = (from c in this._cellContainer.GetChildren(false).OfType<NCrystalSphereCell>()
					where c.Entity.IsHidden
					select c).ToList<NCrystalSphereCell>();
				return list[list.Count / 2];
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A51 RID: 14929 RVA: 0x001BB34C File Offset: 0x001B954C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(14)
			{
				new MethodInfo(NCrystalSphereScreen.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereScreen.MethodName.SetBigDivination, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "obj", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCrystalSphereScreen.MethodName.SetSmallDivination, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "obj", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCrystalSphereScreen.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereScreen.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereScreen.MethodName.OnHoverCell, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cell", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCrystalSphereScreen.MethodName.OnUnhoverCell, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "cell", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCrystalSphereScreen.MethodName.UpdateDivinationsLeft, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereScreen.MethodName.OnMinigameFinished, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereScreen.MethodName.OnProceedButtonPressed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(24L, "_", 0L, "", 6L, new StringName("Control"), false)
				}, null),
				new MethodInfo(NCrystalSphereScreen.MethodName.AfterOverlayOpened, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereScreen.MethodName.AfterOverlayClosed, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereScreen.MethodName.AfterOverlayShown, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCrystalSphereScreen.MethodName.AfterOverlayHidden, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A52 RID: 14930 RVA: 0x001BB6E4 File Offset: 0x001B98E4
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCrystalSphereScreen.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName.SetBigDivination && args.Count == 1)
			{
				this.SetBigDivination(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName.SetSmallDivination && args.Count == 1)
			{
				this.SetSmallDivination(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName.OnHoverCell && args.Count == 1)
			{
				this.OnHoverCell(VariantUtils.ConvertTo<NCrystalSphereCell>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName.OnUnhoverCell && args.Count == 1)
			{
				this.OnUnhoverCell(VariantUtils.ConvertTo<NCrystalSphereCell>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName.UpdateDivinationsLeft && args.Count == 0)
			{
				this.UpdateDivinationsLeft();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName.OnMinigameFinished && args.Count == 0)
			{
				this.OnMinigameFinished();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName.OnProceedButtonPressed && args.Count == 1)
			{
				this.OnProceedButtonPressed(VariantUtils.ConvertTo<NButton>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName.AfterOverlayOpened && args.Count == 0)
			{
				this.AfterOverlayOpened();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName.AfterOverlayClosed && args.Count == 0)
			{
				this.AfterOverlayClosed();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName.AfterOverlayShown && args.Count == 0)
			{
				this.AfterOverlayShown();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCrystalSphereScreen.MethodName.AfterOverlayHidden && args.Count == 0)
			{
				this.AfterOverlayHidden();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06003A53 RID: 14931 RVA: 0x001BB948 File Offset: 0x001B9B48
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCrystalSphereScreen.MethodName._Ready || (ref method) == NCrystalSphereScreen.MethodName.SetBigDivination || (ref method) == NCrystalSphereScreen.MethodName.SetSmallDivination || (ref method) == NCrystalSphereScreen.MethodName._EnterTree || (ref method) == NCrystalSphereScreen.MethodName._ExitTree || (ref method) == NCrystalSphereScreen.MethodName.OnHoverCell || (ref method) == NCrystalSphereScreen.MethodName.OnUnhoverCell || (ref method) == NCrystalSphereScreen.MethodName.UpdateDivinationsLeft || (ref method) == NCrystalSphereScreen.MethodName.OnMinigameFinished || (ref method) == NCrystalSphereScreen.MethodName.OnProceedButtonPressed || (ref method) == NCrystalSphereScreen.MethodName.AfterOverlayOpened || (ref method) == NCrystalSphereScreen.MethodName.AfterOverlayClosed || (ref method) == NCrystalSphereScreen.MethodName.AfterOverlayShown || (ref method) == NCrystalSphereScreen.MethodName.AfterOverlayHidden || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06003A54 RID: 14932 RVA: 0x001BBA30 File Offset: 0x001B9C30
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCrystalSphereScreen.PropertyName._itemsContainer)
			{
				this._itemsContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._cellContainer)
			{
				this._cellContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._bigDivinationButton)
			{
				this._bigDivinationButton = VariantUtils.ConvertTo<NDivinationButton>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._smallDivinationButton)
			{
				this._smallDivinationButton = VariantUtils.ConvertTo<NDivinationButton>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._divinationsLeftLabel)
			{
				this._divinationsLeftLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._mask)
			{
				this._mask = VariantUtils.ConvertTo<NCrystalSphereMask>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._proceedButton)
			{
				this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._instructionsTitleLabel)
			{
				this._instructionsTitleLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._instructionsDescriptionLabel)
			{
				this._instructionsDescriptionLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._instructionsContainer)
			{
				this._instructionsContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._dialogue)
			{
				this._dialogue = VariantUtils.ConvertTo<NCrystalSphereDialogue>(ref value);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._fadeTween)
			{
				this._fadeTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06003A55 RID: 14933 RVA: 0x001BBB8C File Offset: 0x001B9D8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCrystalSphereScreen.PropertyName.ScreenType)
			{
				NetScreenType screenType = this.ScreenType;
				value = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName.UseSharedBackstop)
			{
				bool useSharedBackstop = this.UseSharedBackstop;
				value = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._itemsContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._itemsContainer);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._cellContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._cellContainer);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._bigDivinationButton)
			{
				value = VariantUtils.CreateFrom<NDivinationButton>(ref this._bigDivinationButton);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._smallDivinationButton)
			{
				value = VariantUtils.CreateFrom<NDivinationButton>(ref this._smallDivinationButton);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._divinationsLeftLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._divinationsLeftLabel);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._mask)
			{
				value = VariantUtils.CreateFrom<NCrystalSphereMask>(ref this._mask);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._proceedButton)
			{
				value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._instructionsTitleLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._instructionsTitleLabel);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._instructionsDescriptionLabel)
			{
				value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._instructionsDescriptionLabel);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._instructionsContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._instructionsContainer);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._dialogue)
			{
				value = VariantUtils.CreateFrom<NCrystalSphereDialogue>(ref this._dialogue);
				return true;
			}
			if ((ref name) == NCrystalSphereScreen.PropertyName._fadeTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._fadeTween);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003A56 RID: 14934 RVA: 0x001BBD8C File Offset: 0x001B9F8C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName._itemsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName._cellContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName._bigDivinationButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName._smallDivinationButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName._divinationsLeftLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName._mask, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName._proceedButton, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName._instructionsTitleLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName._instructionsDescriptionLabel, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName._instructionsContainer, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName._dialogue, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName._fadeTween, 0L, "", 4096L, false),
				new PropertyInfo(2L, NCrystalSphereScreen.PropertyName.ScreenType, 0L, "", 4096L, false),
				new PropertyInfo(1L, NCrystalSphereScreen.PropertyName.UseSharedBackstop, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCrystalSphereScreen.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003A57 RID: 14935 RVA: 0x001BBF90 File Offset: 0x001BA190
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NCrystalSphereScreen.PropertyName._itemsContainer, Variant.From<Control>(ref this._itemsContainer));
			info.AddProperty(NCrystalSphereScreen.PropertyName._cellContainer, Variant.From<Control>(ref this._cellContainer));
			info.AddProperty(NCrystalSphereScreen.PropertyName._bigDivinationButton, Variant.From<NDivinationButton>(ref this._bigDivinationButton));
			info.AddProperty(NCrystalSphereScreen.PropertyName._smallDivinationButton, Variant.From<NDivinationButton>(ref this._smallDivinationButton));
			info.AddProperty(NCrystalSphereScreen.PropertyName._divinationsLeftLabel, Variant.From<MegaRichTextLabel>(ref this._divinationsLeftLabel));
			info.AddProperty(NCrystalSphereScreen.PropertyName._mask, Variant.From<NCrystalSphereMask>(ref this._mask));
			info.AddProperty(NCrystalSphereScreen.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
			info.AddProperty(NCrystalSphereScreen.PropertyName._instructionsTitleLabel, Variant.From<MegaRichTextLabel>(ref this._instructionsTitleLabel));
			info.AddProperty(NCrystalSphereScreen.PropertyName._instructionsDescriptionLabel, Variant.From<MegaRichTextLabel>(ref this._instructionsDescriptionLabel));
			info.AddProperty(NCrystalSphereScreen.PropertyName._instructionsContainer, Variant.From<Control>(ref this._instructionsContainer));
			info.AddProperty(NCrystalSphereScreen.PropertyName._dialogue, Variant.From<NCrystalSphereDialogue>(ref this._dialogue));
			info.AddProperty(NCrystalSphereScreen.PropertyName._fadeTween, Variant.From<Tween>(ref this._fadeTween));
		}

		/// <inheritdoc />
		// Token: 0x06003A58 RID: 14936 RVA: 0x001BC0AC File Offset: 0x001BA2AC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._itemsContainer, ref variant))
			{
				this._itemsContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._cellContainer, ref variant2))
			{
				this._cellContainer = variant2.As<Control>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._bigDivinationButton, ref variant3))
			{
				this._bigDivinationButton = variant3.As<NDivinationButton>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._smallDivinationButton, ref variant4))
			{
				this._smallDivinationButton = variant4.As<NDivinationButton>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._divinationsLeftLabel, ref variant5))
			{
				this._divinationsLeftLabel = variant5.As<MegaRichTextLabel>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._mask, ref variant6))
			{
				this._mask = variant6.As<NCrystalSphereMask>();
			}
			Variant variant7;
			if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._proceedButton, ref variant7))
			{
				this._proceedButton = variant7.As<NProceedButton>();
			}
			Variant variant8;
			if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._instructionsTitleLabel, ref variant8))
			{
				this._instructionsTitleLabel = variant8.As<MegaRichTextLabel>();
			}
			Variant variant9;
			if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._instructionsDescriptionLabel, ref variant9))
			{
				this._instructionsDescriptionLabel = variant9.As<MegaRichTextLabel>();
			}
			Variant variant10;
			if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._instructionsContainer, ref variant10))
			{
				this._instructionsContainer = variant10.As<Control>();
			}
			Variant variant11;
			if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._dialogue, ref variant11))
			{
				this._dialogue = variant11.As<NCrystalSphereDialogue>();
			}
			Variant variant12;
			if (info.TryGetProperty(NCrystalSphereScreen.PropertyName._fadeTween, ref variant12))
			{
				this._fadeTween = variant12.As<Tween>();
			}
		}

		// Token: 0x040016B7 RID: 5815
		private readonly LocString _instructionsTitleLoc = new LocString("events", "CRYSTAL_SPHERE.minigame.instructions.title");

		// Token: 0x040016B8 RID: 5816
		private readonly LocString _instructionsDescriptionLoc = new LocString("events", "CRYSTAL_SPHERE.minigame.instructions.description");

		// Token: 0x040016B9 RID: 5817
		private readonly LocString _divinationsRemainLoc = new LocString("events", "CRYSTAL_SPHERE.minigame.divinationsRemain");

		// Token: 0x040016BA RID: 5818
		private const string _scenePath = "res://scenes/events/custom/crystal_sphere/crystal_sphere_screen.tscn";

		// Token: 0x040016BB RID: 5819
		private CrystalSphereMinigame _entity;

		// Token: 0x040016BC RID: 5820
		private Control _itemsContainer;

		// Token: 0x040016BD RID: 5821
		private Control _cellContainer;

		// Token: 0x040016BE RID: 5822
		private NDivinationButton _bigDivinationButton;

		// Token: 0x040016BF RID: 5823
		private NDivinationButton _smallDivinationButton;

		// Token: 0x040016C0 RID: 5824
		private MegaRichTextLabel _divinationsLeftLabel;

		// Token: 0x040016C1 RID: 5825
		private NCrystalSphereMask _mask;

		// Token: 0x040016C2 RID: 5826
		private NProceedButton _proceedButton;

		// Token: 0x040016C3 RID: 5827
		private MegaRichTextLabel _instructionsTitleLabel;

		// Token: 0x040016C4 RID: 5828
		private MegaRichTextLabel _instructionsDescriptionLabel;

		// Token: 0x040016C5 RID: 5829
		private Control _instructionsContainer;

		// Token: 0x040016C6 RID: 5830
		private NCrystalSphereDialogue _dialogue;

		// Token: 0x040016C7 RID: 5831
		[Nullable(2)]
		private Tween _fadeTween;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017ED RID: 6125
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005D5D RID: 23901
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SetBigDivination' method.
			/// </summary>
			// Token: 0x04005D5E RID: 23902
			public static readonly StringName SetBigDivination = "SetBigDivination";

			/// <summary>
			/// Cached name for the 'SetSmallDivination' method.
			/// </summary>
			// Token: 0x04005D5F RID: 23903
			public static readonly StringName SetSmallDivination = "SetSmallDivination";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x04005D60 RID: 23904
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04005D61 RID: 23905
			public static readonly StringName _ExitTree = "_ExitTree";

			/// <summary>
			/// Cached name for the 'OnHoverCell' method.
			/// </summary>
			// Token: 0x04005D62 RID: 23906
			public static readonly StringName OnHoverCell = "OnHoverCell";

			/// <summary>
			/// Cached name for the 'OnUnhoverCell' method.
			/// </summary>
			// Token: 0x04005D63 RID: 23907
			public static readonly StringName OnUnhoverCell = "OnUnhoverCell";

			/// <summary>
			/// Cached name for the 'UpdateDivinationsLeft' method.
			/// </summary>
			// Token: 0x04005D64 RID: 23908
			public static readonly StringName UpdateDivinationsLeft = "UpdateDivinationsLeft";

			/// <summary>
			/// Cached name for the 'OnMinigameFinished' method.
			/// </summary>
			// Token: 0x04005D65 RID: 23909
			public static readonly StringName OnMinigameFinished = "OnMinigameFinished";

			/// <summary>
			/// Cached name for the 'OnProceedButtonPressed' method.
			/// </summary>
			// Token: 0x04005D66 RID: 23910
			public static readonly StringName OnProceedButtonPressed = "OnProceedButtonPressed";

			/// <summary>
			/// Cached name for the 'AfterOverlayOpened' method.
			/// </summary>
			// Token: 0x04005D67 RID: 23911
			public static readonly StringName AfterOverlayOpened = "AfterOverlayOpened";

			/// <summary>
			/// Cached name for the 'AfterOverlayClosed' method.
			/// </summary>
			// Token: 0x04005D68 RID: 23912
			public static readonly StringName AfterOverlayClosed = "AfterOverlayClosed";

			/// <summary>
			/// Cached name for the 'AfterOverlayShown' method.
			/// </summary>
			// Token: 0x04005D69 RID: 23913
			public static readonly StringName AfterOverlayShown = "AfterOverlayShown";

			/// <summary>
			/// Cached name for the 'AfterOverlayHidden' method.
			/// </summary>
			// Token: 0x04005D6A RID: 23914
			public static readonly StringName AfterOverlayHidden = "AfterOverlayHidden";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017EE RID: 6126
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScreenType' property.
			/// </summary>
			// Token: 0x04005D6B RID: 23915
			public static readonly StringName ScreenType = "ScreenType";

			/// <summary>
			/// Cached name for the 'UseSharedBackstop' property.
			/// </summary>
			// Token: 0x04005D6C RID: 23916
			public static readonly StringName UseSharedBackstop = "UseSharedBackstop";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x04005D6D RID: 23917
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_itemsContainer' field.
			/// </summary>
			// Token: 0x04005D6E RID: 23918
			public static readonly StringName _itemsContainer = "_itemsContainer";

			/// <summary>
			/// Cached name for the '_cellContainer' field.
			/// </summary>
			// Token: 0x04005D6F RID: 23919
			public static readonly StringName _cellContainer = "_cellContainer";

			/// <summary>
			/// Cached name for the '_bigDivinationButton' field.
			/// </summary>
			// Token: 0x04005D70 RID: 23920
			public static readonly StringName _bigDivinationButton = "_bigDivinationButton";

			/// <summary>
			/// Cached name for the '_smallDivinationButton' field.
			/// </summary>
			// Token: 0x04005D71 RID: 23921
			public static readonly StringName _smallDivinationButton = "_smallDivinationButton";

			/// <summary>
			/// Cached name for the '_divinationsLeftLabel' field.
			/// </summary>
			// Token: 0x04005D72 RID: 23922
			public static readonly StringName _divinationsLeftLabel = "_divinationsLeftLabel";

			/// <summary>
			/// Cached name for the '_mask' field.
			/// </summary>
			// Token: 0x04005D73 RID: 23923
			public static readonly StringName _mask = "_mask";

			/// <summary>
			/// Cached name for the '_proceedButton' field.
			/// </summary>
			// Token: 0x04005D74 RID: 23924
			public static readonly StringName _proceedButton = "_proceedButton";

			/// <summary>
			/// Cached name for the '_instructionsTitleLabel' field.
			/// </summary>
			// Token: 0x04005D75 RID: 23925
			public static readonly StringName _instructionsTitleLabel = "_instructionsTitleLabel";

			/// <summary>
			/// Cached name for the '_instructionsDescriptionLabel' field.
			/// </summary>
			// Token: 0x04005D76 RID: 23926
			public static readonly StringName _instructionsDescriptionLabel = "_instructionsDescriptionLabel";

			/// <summary>
			/// Cached name for the '_instructionsContainer' field.
			/// </summary>
			// Token: 0x04005D77 RID: 23927
			public static readonly StringName _instructionsContainer = "_instructionsContainer";

			/// <summary>
			/// Cached name for the '_dialogue' field.
			/// </summary>
			// Token: 0x04005D78 RID: 23928
			public static readonly StringName _dialogue = "_dialogue";

			/// <summary>
			/// Cached name for the '_fadeTween' field.
			/// </summary>
			// Token: 0x04005D79 RID: 23929
			public static readonly StringName _fadeTween = "_fadeTween";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020017EF RID: 6127
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}

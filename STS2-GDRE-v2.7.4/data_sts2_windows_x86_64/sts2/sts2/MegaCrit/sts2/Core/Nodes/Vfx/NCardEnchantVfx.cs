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
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx
{
	// Token: 0x0200018C RID: 396
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Vfx/NCardEnchantVfx.cs")]
	public class NCardEnchantVfx : Node2D
	{
		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001334 RID: 4916 RVA: 0x000733DA File Offset: 0x000715DA
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("vfx/vfx_card_enchant");
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001335 RID: 4917 RVA: 0x000733E6 File Offset: 0x000715E6
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return new <>z__ReadOnlySingleElementList<string>(NCardEnchantVfx.ScenePath);
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001336 RID: 4918 RVA: 0x000733F2 File Offset: 0x000715F2
		// (set) Token: 0x06001337 RID: 4919 RVA: 0x000733FA File Offset: 0x000715FA
		[Nullable(2)]
		[Export(/*Failed to decode CustomAttribute blob!*/)]
		public Curve EmbossCurve
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x00073404 File Offset: 0x00071604
		[return: Nullable(2)]
		public static NCardEnchantVfx Create(CardModel card)
		{
			if (TestMode.IsOn)
			{
				return null;
			}
			if (!LocalContext.IsMine(card))
			{
				return null;
			}
			NCardEnchantVfx ncardEnchantVfx = PreloadManager.Cache.GetScene(NCardEnchantVfx.ScenePath).Instantiate<NCardEnchantVfx>(0L);
			ncardEnchantVfx._cardModel = card;
			return ncardEnchantVfx;
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00073444 File Offset: 0x00071644
		public override void _Ready()
		{
			this._enchantmentSparkles = base.GetNode<GpuParticles2D>("%EnchantmentAppearSparkles");
			this._enchantmentIcon = base.GetNode<TextureRect>("%EnchantmentInViewport/Icon");
			this._enchantmentLabel = base.GetNode<MegaLabel>("%EnchantmentInViewport/Label");
			this._enchantmentIcon.Texture = this._cardModel.Enchantment.Icon;
			this._enchantmentLabel.SetTextAutoSize(this._cardModel.Enchantment.DisplayAmount.ToString());
			this._enchantmentLabel.Visible = this._cardModel.Enchantment.ShowAmount;
			this._cardNode = NCard.Create(this._cardModel, ModelVisibility.Visible);
			this.AddChildSafely(this._cardNode);
			this.MoveChildSafely(this._cardNode, 0);
			this._cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
			this._cardNode.EnchantmentTab.Visible = false;
			this._cardNode.EnchantmentVfxOverride.Visible = true;
			Viewport node = base.GetNode<Viewport>("%EnchantmentViewport");
			this._cardNode.EnchantmentVfxOverride.Texture = node.GetTexture();
			TaskHelper.RunSafely(this.PlayAnimation());
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0007357C File Offset: 0x0007177C
		public override void _ExitTree()
		{
			Tween tween = this._tween;
			if (tween != null)
			{
				tween.Kill();
			}
			CancellationTokenSource cts = this._cts;
			if (cts != null)
			{
				cts.Cancel();
			}
			if (this._cardNode.IsValid() && base.IsAncestorOf(this._cardNode))
			{
				this._cardNode.QueueFreeSafely();
			}
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x000735D4 File Offset: 0x000717D4
		private async Task PlayAnimation()
		{
			this._cts = new CancellationTokenSource();
			((ShaderMaterial)this._cardNode.EnchantmentVfxOverride.Material).SetShaderParameter(NCardEnchantVfx._progress, 0f);
			this._tween = base.CreateTween();
			SfxCmd.Play("event:/sfx/ui/enchant_shimmer", 1f);
			this._tween.TweenProperty(this._cardNode.EnchantmentVfxOverride, "material:shader_parameter/progress", 1f, 1.0).SetEase(2L).SetTrans(4L);
			this._tween.Parallel().TweenCallback(Callable.From<bool>(() => this._enchantmentSparkles.Emitting = true)).SetDelay(0.20000000298023224);
			this._tween.Parallel().TweenProperty(this._enchantmentSparkles, "position:x", this._enchantmentSparkles.Position.X + 72f, 0.4000000059604645).SetDelay(0.20000000298023224);
			TaskAwaiter<bool> taskAwaiter = this._tween.AwaitFinished(this).GetAwaiter();
			TaskAwaiter<bool> taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				await Cmd.Wait(1f, this._cts.Token, false);
				CardModel model = this._cardNode.Model;
				if (this._cardNode.IsInsideTree() && model.Pile == null)
				{
					this._tween = base.CreateTween();
					this._tween.TweenProperty(this, "scale", Vector2.Zero, 0.15000000596046448);
					TaskAwaiter<bool> taskAwaiter3 = this._tween.AwaitFinished(this).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						await taskAwaiter3;
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter3.GetResult())
					{
						return;
					}
				}
				else if (this._cardNode.IsInsideTree())
				{
					NCardFlyVfx ncardFlyVfx = NCardFlyVfx.Create(this._cardNode, model.Pile.Type, false, model.Owner.Character.TrailPath);
					NRun instance = NRun.Instance;
					if (instance != null)
					{
						instance.GlobalUi.TopBar.TrailContainer.AddChildSafely(ncardFlyVfx);
					}
					if (ncardFlyVfx.SwooshAwayCompletion != null)
					{
						await ncardFlyVfx.SwooshAwayCompletion.Task;
					}
				}
				this.QueueFreeSafely();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600133C RID: 4924 RVA: 0x00073618 File Offset: 0x00071818
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NCardEnchantVfx.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NCardEnchantVfx.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600133D RID: 4925 RVA: 0x0007368C File Offset: 0x0007188C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NCardEnchantVfx.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NCardEnchantVfx.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x0600133E RID: 4926 RVA: 0x000736EC File Offset: 0x000718EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NCardEnchantVfx.MethodName._Ready || (ref method) == NCardEnchantVfx.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x0600133F RID: 4927 RVA: 0x00073714 File Offset: 0x00071914
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NCardEnchantVfx.PropertyName.EmbossCurve)
			{
				this.EmbossCurve = VariantUtils.ConvertTo<Curve>(ref value);
				return true;
			}
			if ((ref name) == NCardEnchantVfx.PropertyName._tween)
			{
				this._tween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NCardEnchantVfx.PropertyName._cardNode)
			{
				this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
				return true;
			}
			if ((ref name) == NCardEnchantVfx.PropertyName._enchantmentSparkles)
			{
				this._enchantmentSparkles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
				return true;
			}
			if ((ref name) == NCardEnchantVfx.PropertyName._enchantmentIcon)
			{
				this._enchantmentIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NCardEnchantVfx.PropertyName._enchantmentLabel)
			{
				this._enchantmentLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001340 RID: 4928 RVA: 0x000737CC File Offset: 0x000719CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NCardEnchantVfx.PropertyName.EmbossCurve)
			{
				Curve embossCurve = this.EmbossCurve;
				value = VariantUtils.CreateFrom<Curve>(ref embossCurve);
				return true;
			}
			if ((ref name) == NCardEnchantVfx.PropertyName._tween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._tween);
				return true;
			}
			if ((ref name) == NCardEnchantVfx.PropertyName._cardNode)
			{
				value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
				return true;
			}
			if ((ref name) == NCardEnchantVfx.PropertyName._enchantmentSparkles)
			{
				value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._enchantmentSparkles);
				return true;
			}
			if ((ref name) == NCardEnchantVfx.PropertyName._enchantmentIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._enchantmentIcon);
				return true;
			}
			if ((ref name) == NCardEnchantVfx.PropertyName._enchantmentLabel)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._enchantmentLabel);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001341 RID: 4929 RVA: 0x000738A4 File Offset: 0x00071AA4
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NCardEnchantVfx.PropertyName._tween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardEnchantVfx.PropertyName.EmbossCurve, 17L, "Curve", 4102L, true),
				new PropertyInfo(24L, NCardEnchantVfx.PropertyName._cardNode, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardEnchantVfx.PropertyName._enchantmentSparkles, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardEnchantVfx.PropertyName._enchantmentIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NCardEnchantVfx.PropertyName._enchantmentLabel, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001342 RID: 4930 RVA: 0x00073980 File Offset: 0x00071B80
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName embossCurve = NCardEnchantVfx.PropertyName.EmbossCurve;
			Curve embossCurve2 = this.EmbossCurve;
			info.AddProperty(embossCurve, Variant.From<Curve>(ref embossCurve2));
			info.AddProperty(NCardEnchantVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
			info.AddProperty(NCardEnchantVfx.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
			info.AddProperty(NCardEnchantVfx.PropertyName._enchantmentSparkles, Variant.From<GpuParticles2D>(ref this._enchantmentSparkles));
			info.AddProperty(NCardEnchantVfx.PropertyName._enchantmentIcon, Variant.From<TextureRect>(ref this._enchantmentIcon));
			info.AddProperty(NCardEnchantVfx.PropertyName._enchantmentLabel, Variant.From<MegaLabel>(ref this._enchantmentLabel));
		}

		/// <inheritdoc />
		// Token: 0x06001343 RID: 4931 RVA: 0x00073A1C File Offset: 0x00071C1C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NCardEnchantVfx.PropertyName.EmbossCurve, ref variant))
			{
				this.EmbossCurve = variant.As<Curve>();
			}
			Variant variant2;
			if (info.TryGetProperty(NCardEnchantVfx.PropertyName._tween, ref variant2))
			{
				this._tween = variant2.As<Tween>();
			}
			Variant variant3;
			if (info.TryGetProperty(NCardEnchantVfx.PropertyName._cardNode, ref variant3))
			{
				this._cardNode = variant3.As<NCard>();
			}
			Variant variant4;
			if (info.TryGetProperty(NCardEnchantVfx.PropertyName._enchantmentSparkles, ref variant4))
			{
				this._enchantmentSparkles = variant4.As<GpuParticles2D>();
			}
			Variant variant5;
			if (info.TryGetProperty(NCardEnchantVfx.PropertyName._enchantmentIcon, ref variant5))
			{
				this._enchantmentIcon = variant5.As<TextureRect>();
			}
			Variant variant6;
			if (info.TryGetProperty(NCardEnchantVfx.PropertyName._enchantmentLabel, ref variant6))
			{
				this._enchantmentLabel = variant6.As<MegaLabel>();
			}
		}

		// Token: 0x040006CB RID: 1739
		private static readonly StringName _progress = new StringName("progress");

		// Token: 0x040006CC RID: 1740
		[Nullable(2)]
		private Tween _tween;

		// Token: 0x040006CD RID: 1741
		[Nullable(2)]
		private CancellationTokenSource _cts;

		// Token: 0x040006CF RID: 1743
		private CardModel _cardModel;

		// Token: 0x040006D0 RID: 1744
		private NCard _cardNode;

		// Token: 0x040006D1 RID: 1745
		private GpuParticles2D _enchantmentSparkles;

		// Token: 0x040006D2 RID: 1746
		private TextureRect _enchantmentIcon;

		// Token: 0x040006D3 RID: 1747
		private MegaLabel _enchantmentLabel;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FB9 RID: 4025
		[NullableContext(0)]
		public class MethodName : Node2D.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04003E95 RID: 16021
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x04003E96 RID: 16022
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FBA RID: 4026
		[NullableContext(0)]
		public class PropertyName : Node2D.PropertyName
		{
			/// <summary>
			/// Cached name for the 'EmbossCurve' property.
			/// </summary>
			// Token: 0x04003E97 RID: 16023
			public static readonly StringName EmbossCurve = "EmbossCurve";

			/// <summary>
			/// Cached name for the '_tween' field.
			/// </summary>
			// Token: 0x04003E98 RID: 16024
			public static readonly StringName _tween = "_tween";

			/// <summary>
			/// Cached name for the '_cardNode' field.
			/// </summary>
			// Token: 0x04003E99 RID: 16025
			public static readonly StringName _cardNode = "_cardNode";

			/// <summary>
			/// Cached name for the '_enchantmentSparkles' field.
			/// </summary>
			// Token: 0x04003E9A RID: 16026
			public static readonly StringName _enchantmentSparkles = "_enchantmentSparkles";

			/// <summary>
			/// Cached name for the '_enchantmentIcon' field.
			/// </summary>
			// Token: 0x04003E9B RID: 16027
			public static readonly StringName _enchantmentIcon = "_enchantmentIcon";

			/// <summary>
			/// Cached name for the '_enchantmentLabel' field.
			/// </summary>
			// Token: 0x04003E9C RID: 16028
			public static readonly StringName _enchantmentLabel = "_enchantmentLabel";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02000FBB RID: 4027
		[NullableContext(0)]
		public class SignalName : Node2D.SignalName
		{
		}
	}
}

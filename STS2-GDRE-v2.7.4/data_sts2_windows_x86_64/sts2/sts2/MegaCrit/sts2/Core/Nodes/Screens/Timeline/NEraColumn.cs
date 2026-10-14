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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Timeline;

namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline
{
	/// <summary>
	/// Represents a single column in the Timeline.
	/// Node which has a list of NEpochSlots, an Era icon, its name, and year.
	/// </summary>
	// Token: 0x02000261 RID: 609
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NEraColumn.cs")]
	public class NEraColumn : Control
	{
		// Token: 0x06001FD3 RID: 8147 RVA: 0x000D6854 File Offset: 0x000D4A54
		public static NEraColumn Create(EpochSlotData data)
		{
			NEraColumn neraColumn = PreloadManager.Cache.GetScene("res://scenes/timeline_screen/era_column.tscn").Instantiate<NEraColumn>(0L);
			neraColumn._data = data;
			return neraColumn;
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x000D6880 File Offset: 0x000D4A80
		public override void _Ready()
		{
			this._icon = base.GetNode<TextureRect>("%Icon");
			this._name = base.GetNode<MegaLabel>("%Name");
			this._year = base.GetNode<MegaLabel>("%Year");
			this.era = this._data.Era;
			base.SetName(this._data.Era.ToString());
			this.Init(this._data);
			base.ItemRectChanged += this.RectChange;
		}

		/// <summary>
		/// Creates this Era Column and also the first slot for it
		/// </summary>
		// Token: 0x06001FD5 RID: 8149 RVA: 0x000D6920 File Offset: 0x000D4B20
		public void Init(EpochSlotData epochSlot)
		{
			ValueTuple<Texture2D, string> eraIcon = NTimelineScreen.GetEraIcon(epochSlot.Era);
			this._icon.Texture = eraIcon.Item1;
			this._name.SetTextAutoSize(new LocString("eras", eraIcon.Item2 + ".name").GetFormattedText());
			this._year.SetTextAutoSize(new LocString("eras", eraIcon.Item2 + ".year").GetFormattedText());
			this.AddSlot(epochSlot);
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x000D69A8 File Offset: 0x000D4BA8
		public void AddSlot(EpochSlotData epochSlotData)
		{
			NEpochSlot nepochSlot = NEpochSlot.Create(epochSlotData);
			this.AddChildSafely(nepochSlot);
			Node node = nepochSlot;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Slot");
			defaultInterpolatedStringHandler.AppendFormatted<int>(epochSlotData.EraPosition);
			node.Name = defaultInterpolatedStringHandler.ToStringAndClear();
			this.MoveChildSafely(nepochSlot, 0);
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x000D6A00 File Offset: 0x000D4C00
		public void SpawnIcon()
		{
			this._iconTween = base.CreateTween().SetParallel(true);
			this._iconTween.TweenProperty(this._icon, "modulate:a", 1f, 0.5);
			this._iconTween.TweenProperty(this._icon, "scale", Vector2.One, 0.5).From(Vector2.One * 0.1f).SetEase(1L)
				.SetTrans(10L);
		}

		/// <summary>
		/// Epoch slot pop-up effect!
		/// </summary>
		// Token: 0x06001FD8 RID: 8152 RVA: 0x000D6AA8 File Offset: 0x000D4CA8
		public async Task SpawnSlots(bool isAnimated)
		{
			foreach (Node node in base.GetChildren(false))
			{
				NEpochSlot nepochSlot = node as NEpochSlot;
				if (nepochSlot != null && !nepochSlot.HasSpawned)
				{
					if (isAnimated)
					{
						await nepochSlot.SpawnSlot();
					}
					else
					{
						TaskHelper.RunSafely(nepochSlot.SpawnSlot());
					}
				}
			}
			IEnumerator<Node> enumerator = null;
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x000D6AF4 File Offset: 0x000D4CF4
		public async Task SpawnNameAndYear()
		{
			if (!this._labelSpawned)
			{
				this._labelSpawned = true;
				this._labelTween = base.CreateTween().SetParallel(true);
				this._name.SelfModulate = new Color(this._name.SelfModulate.R, this._name.SelfModulate.G, this._name.SelfModulate.B, 0f);
				this._year.Modulate = new Color(this._year.Modulate.R, this._year.Modulate.G, this._year.Modulate.B, 0f);
				this._labelTween.TweenProperty(this._name, "self_modulate:a", 1f, 1.0);
				this._labelTween.TweenProperty(this._name, "position:y", 28f, 1.0).From(-36f).SetEase(1L)
					.SetTrans(7L);
				this._labelTween.TweenProperty(this._year, "modulate:a", 1f, 1.0).SetDelay(0.5);
				this._labelTween.TweenProperty(this._year, "position:y", 20f, 1.0).SetDelay(0.5).From(0f)
					.SetEase(1L)
					.SetTrans(7L);
				bool flag = await this._labelTween.AwaitFinished(this);
				await Task.Delay(500);
			}
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x000D6B38 File Offset: 0x000D4D38
		public async Task SaveBeforeAnimationPosition()
		{
			this._isAnimated = true;
			this._prevLocalPos = base.Position;
			this._prevGlobalPos = base.GlobalPosition;
			await this.AwaitProcessFrame(default(CancellationToken));
			this._isAnimated = false;
			this._targetPosition = this._predictedPosition;
			base.GlobalPosition = this._prevGlobalPos;
			Tween tween = base.CreateTween().SetParallel(true);
			tween.TweenProperty(this, "position", this._targetPosition, 2.0).SetEase(2L).SetTrans(7L);
		}

		/// <summary>
		/// Cache the position of where we predict the Position this column will be.
		/// There's no way to forcefully refresh the layout in Godot so PredictHBoxLayout()
		/// was created and that passes in the expected position.
		/// </summary>
		// Token: 0x06001FDB RID: 8155 RVA: 0x000D6B7B File Offset: 0x000D4D7B
		public void SetPredictedPosition(Vector2 setPredictedPosition)
		{
			if (!this._isAnimated)
			{
				return;
			}
			this._predictedPosition = setPredictedPosition;
		}

		/// <summary>
		/// Behold the hack. Every time this Node's size is updated we force the position to be where it should.
		/// Note that this is called several times per frame as many children are added to the list of columns
		/// so the _isAnimated bool is actually disabled next frame.
		/// </summary>
		// Token: 0x06001FDC RID: 8156 RVA: 0x000D6B8D File Offset: 0x000D4D8D
		private void RectChange()
		{
			if (this._isAnimated)
			{
				base.GlobalPosition = this._prevGlobalPos;
			}
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000D6BA3 File Offset: 0x000D4DA3
		public override void _ExitTree()
		{
			base.ItemRectChanged -= this.RectChange;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001FDE RID: 8158 RVA: 0x000D6BB8 File Offset: 0x000D4DB8
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(5)
			{
				new MethodInfo(NEraColumn.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEraColumn.MethodName.SpawnIcon, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEraColumn.MethodName.SetPredictedPosition, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(5L, "setPredictedPosition", 0L, "", 6L, false)
				}, null),
				new MethodInfo(NEraColumn.MethodName.RectChange, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NEraColumn.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001FDF RID: 8159 RVA: 0x000D6CE0 File Offset: 0x000D4EE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NEraColumn.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEraColumn.MethodName.SpawnIcon && args.Count == 0)
			{
				this.SpawnIcon();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEraColumn.MethodName.SetPredictedPosition && args.Count == 1)
			{
				this.SetPredictedPosition(VariantUtils.ConvertTo<Vector2>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEraColumn.MethodName.RectChange && args.Count == 0)
			{
				this.RectChange();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NEraColumn.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06001FE0 RID: 8160 RVA: 0x000D6DC0 File Offset: 0x000D4FC0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NEraColumn.MethodName._Ready || (ref method) == NEraColumn.MethodName.SpawnIcon || (ref method) == NEraColumn.MethodName.SetPredictedPosition || (ref method) == NEraColumn.MethodName.RectChange || (ref method) == NEraColumn.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06001FE1 RID: 8161 RVA: 0x000D6E20 File Offset: 0x000D5020
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NEraColumn.PropertyName._icon)
			{
				this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._name)
			{
				this._name = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._year)
			{
				this._year = VariantUtils.ConvertTo<MegaLabel>(ref value);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._iconTween)
			{
				this._iconTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._labelTween)
			{
				this._labelTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._labelSpawned)
			{
				this._labelSpawned = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName.era)
			{
				this.era = VariantUtils.ConvertTo<EpochEra>(ref value);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._prevLocalPos)
			{
				this._prevLocalPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._prevGlobalPos)
			{
				this._prevGlobalPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._predictedPosition)
			{
				this._predictedPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._targetPosition)
			{
				this._targetPosition = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._isAnimated)
			{
				this._isAnimated = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x06001FE2 RID: 8162 RVA: 0x000D6F7C File Offset: 0x000D517C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NEraColumn.PropertyName._icon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._name)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._name);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._year)
			{
				value = VariantUtils.CreateFrom<MegaLabel>(ref this._year);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._iconTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._iconTween);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._labelTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._labelTween);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._labelSpawned)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._labelSpawned);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName.era)
			{
				value = VariantUtils.CreateFrom<EpochEra>(ref this.era);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._prevLocalPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._prevLocalPos);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._prevGlobalPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._prevGlobalPos);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._predictedPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._predictedPosition);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._targetPosition)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetPosition);
				return true;
			}
			if ((ref name) == NEraColumn.PropertyName._isAnimated)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isAnimated);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06001FE3 RID: 8163 RVA: 0x000D7114 File Offset: 0x000D5314
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NEraColumn.PropertyName._icon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEraColumn.PropertyName._name, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEraColumn.PropertyName._year, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEraColumn.PropertyName._iconTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NEraColumn.PropertyName._labelTween, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEraColumn.PropertyName._labelSpawned, 0L, "", 4096L, false),
				new PropertyInfo(2L, NEraColumn.PropertyName.era, 0L, "", 4096L, false),
				new PropertyInfo(5L, NEraColumn.PropertyName._prevLocalPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NEraColumn.PropertyName._prevGlobalPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NEraColumn.PropertyName._predictedPosition, 0L, "", 4096L, false),
				new PropertyInfo(5L, NEraColumn.PropertyName._targetPosition, 0L, "", 4096L, false),
				new PropertyInfo(1L, NEraColumn.PropertyName._isAnimated, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06001FE4 RID: 8164 RVA: 0x000D72B0 File Offset: 0x000D54B0
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NEraColumn.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
			info.AddProperty(NEraColumn.PropertyName._name, Variant.From<MegaLabel>(ref this._name));
			info.AddProperty(NEraColumn.PropertyName._year, Variant.From<MegaLabel>(ref this._year));
			info.AddProperty(NEraColumn.PropertyName._iconTween, Variant.From<Tween>(ref this._iconTween));
			info.AddProperty(NEraColumn.PropertyName._labelTween, Variant.From<Tween>(ref this._labelTween));
			info.AddProperty(NEraColumn.PropertyName._labelSpawned, Variant.From<bool>(ref this._labelSpawned));
			info.AddProperty(NEraColumn.PropertyName.era, Variant.From<EpochEra>(ref this.era));
			info.AddProperty(NEraColumn.PropertyName._prevLocalPos, Variant.From<Vector2>(ref this._prevLocalPos));
			info.AddProperty(NEraColumn.PropertyName._prevGlobalPos, Variant.From<Vector2>(ref this._prevGlobalPos));
			info.AddProperty(NEraColumn.PropertyName._predictedPosition, Variant.From<Vector2>(ref this._predictedPosition));
			info.AddProperty(NEraColumn.PropertyName._targetPosition, Variant.From<Vector2>(ref this._targetPosition));
			info.AddProperty(NEraColumn.PropertyName._isAnimated, Variant.From<bool>(ref this._isAnimated));
		}

		/// <inheritdoc />
		// Token: 0x06001FE5 RID: 8165 RVA: 0x000D73CC File Offset: 0x000D55CC
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NEraColumn.PropertyName._icon, ref variant))
			{
				this._icon = variant.As<TextureRect>();
			}
			Variant variant2;
			if (info.TryGetProperty(NEraColumn.PropertyName._name, ref variant2))
			{
				this._name = variant2.As<MegaLabel>();
			}
			Variant variant3;
			if (info.TryGetProperty(NEraColumn.PropertyName._year, ref variant3))
			{
				this._year = variant3.As<MegaLabel>();
			}
			Variant variant4;
			if (info.TryGetProperty(NEraColumn.PropertyName._iconTween, ref variant4))
			{
				this._iconTween = variant4.As<Tween>();
			}
			Variant variant5;
			if (info.TryGetProperty(NEraColumn.PropertyName._labelTween, ref variant5))
			{
				this._labelTween = variant5.As<Tween>();
			}
			Variant variant6;
			if (info.TryGetProperty(NEraColumn.PropertyName._labelSpawned, ref variant6))
			{
				this._labelSpawned = variant6.As<bool>();
			}
			Variant variant7;
			if (info.TryGetProperty(NEraColumn.PropertyName.era, ref variant7))
			{
				this.era = variant7.As<EpochEra>();
			}
			Variant variant8;
			if (info.TryGetProperty(NEraColumn.PropertyName._prevLocalPos, ref variant8))
			{
				this._prevLocalPos = variant8.As<Vector2>();
			}
			Variant variant9;
			if (info.TryGetProperty(NEraColumn.PropertyName._prevGlobalPos, ref variant9))
			{
				this._prevGlobalPos = variant9.As<Vector2>();
			}
			Variant variant10;
			if (info.TryGetProperty(NEraColumn.PropertyName._predictedPosition, ref variant10))
			{
				this._predictedPosition = variant10.As<Vector2>();
			}
			Variant variant11;
			if (info.TryGetProperty(NEraColumn.PropertyName._targetPosition, ref variant11))
			{
				this._targetPosition = variant11.As<Vector2>();
			}
			Variant variant12;
			if (info.TryGetProperty(NEraColumn.PropertyName._isAnimated, ref variant12))
			{
				this._isAnimated = variant12.As<bool>();
			}
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000D7538 File Offset: 0x000D5738
		// Note: this type is marked as 'beforefieldinit'.
		static NEraColumn()
		{
			List<string> list = new List<string>();
			list.Add("res://scenes/timeline_screen/era_column.tscn");
			list.AddRange(NEpochSlot.assetPaths);
			NEraColumn.assetPaths = new <>z__ReadOnlyList<string>(list);
		}

		// Token: 0x04000C70 RID: 3184
		private const string _scenePath = "res://scenes/timeline_screen/era_column.tscn";

		// Token: 0x04000C71 RID: 3185
		public static readonly IEnumerable<string> assetPaths;

		// Token: 0x04000C72 RID: 3186
		private TextureRect _icon;

		// Token: 0x04000C73 RID: 3187
		private MegaLabel _name;

		// Token: 0x04000C74 RID: 3188
		private MegaLabel _year;

		// Token: 0x04000C75 RID: 3189
		private Tween _iconTween;

		// Token: 0x04000C76 RID: 3190
		private Tween _labelTween;

		// Token: 0x04000C77 RID: 3191
		private bool _labelSpawned;

		// Token: 0x04000C78 RID: 3192
		public EpochEra era;

		// Token: 0x04000C79 RID: 3193
		private Vector2 _prevLocalPos;

		// Token: 0x04000C7A RID: 3194
		private Vector2 _prevGlobalPos;

		// Token: 0x04000C7B RID: 3195
		private Vector2 _predictedPosition;

		// Token: 0x04000C7C RID: 3196
		private Vector2 _targetPosition;

		// Token: 0x04000C7D RID: 3197
		private bool _isAnimated;

		// Token: 0x04000C7E RID: 3198
		private EpochSlotData _data;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012B1 RID: 4785
		[NullableContext(0)]
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040048C5 RID: 18629
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'SpawnIcon' method.
			/// </summary>
			// Token: 0x040048C6 RID: 18630
			public static readonly StringName SpawnIcon = "SpawnIcon";

			/// <summary>
			/// Cached name for the 'SetPredictedPosition' method.
			/// </summary>
			// Token: 0x040048C7 RID: 18631
			public static readonly StringName SetPredictedPosition = "SetPredictedPosition";

			/// <summary>
			/// Cached name for the 'RectChange' method.
			/// </summary>
			// Token: 0x040048C8 RID: 18632
			public static readonly StringName RectChange = "RectChange";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040048C9 RID: 18633
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012B2 RID: 4786
		[NullableContext(0)]
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the '_icon' field.
			/// </summary>
			// Token: 0x040048CA RID: 18634
			public static readonly StringName _icon = "_icon";

			/// <summary>
			/// Cached name for the '_name' field.
			/// </summary>
			// Token: 0x040048CB RID: 18635
			public static readonly StringName _name = "_name";

			/// <summary>
			/// Cached name for the '_year' field.
			/// </summary>
			// Token: 0x040048CC RID: 18636
			public static readonly StringName _year = "_year";

			/// <summary>
			/// Cached name for the '_iconTween' field.
			/// </summary>
			// Token: 0x040048CD RID: 18637
			public static readonly StringName _iconTween = "_iconTween";

			/// <summary>
			/// Cached name for the '_labelTween' field.
			/// </summary>
			// Token: 0x040048CE RID: 18638
			public static readonly StringName _labelTween = "_labelTween";

			/// <summary>
			/// Cached name for the '_labelSpawned' field.
			/// </summary>
			// Token: 0x040048CF RID: 18639
			public static readonly StringName _labelSpawned = "_labelSpawned";

			/// <summary>
			/// Cached name for the 'era' field.
			/// </summary>
			// Token: 0x040048D0 RID: 18640
			public static readonly StringName era = "era";

			/// <summary>
			/// Cached name for the '_prevLocalPos' field.
			/// </summary>
			// Token: 0x040048D1 RID: 18641
			public static readonly StringName _prevLocalPos = "_prevLocalPos";

			/// <summary>
			/// Cached name for the '_prevGlobalPos' field.
			/// </summary>
			// Token: 0x040048D2 RID: 18642
			public static readonly StringName _prevGlobalPos = "_prevGlobalPos";

			/// <summary>
			/// Cached name for the '_predictedPosition' field.
			/// </summary>
			// Token: 0x040048D3 RID: 18643
			public static readonly StringName _predictedPosition = "_predictedPosition";

			/// <summary>
			/// Cached name for the '_targetPosition' field.
			/// </summary>
			// Token: 0x040048D4 RID: 18644
			public static readonly StringName _targetPosition = "_targetPosition";

			/// <summary>
			/// Cached name for the '_isAnimated' field.
			/// </summary>
			// Token: 0x040048D5 RID: 18645
			public static readonly StringName _isAnimated = "_isAnimated";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012B3 RID: 4787
		[NullableContext(0)]
		public class SignalName : Control.SignalName
		{
		}
	}
}

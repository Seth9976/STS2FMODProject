using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Platform;

namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen
{
	// Token: 0x02000272 RID: 626
	[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NAchievementsGrid.cs")]
	public class NAchievementsGrid : Control
	{
		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x0600210F RID: 8463 RVA: 0x000E021F File Offset: 0x000DE41F
		[Nullable(1)]
		public static IEnumerable<string> AssetPaths
		{
			[NullableContext(1)]
			get
			{
				return from a in Enum.GetValues<Achievement>()
					select NAchievementHolder.GetPathForAchievement(a);
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06002110 RID: 8464 RVA: 0x000E024A File Offset: 0x000DE44A
		private float ScrollLimitBottom
		{
			get
			{
				return -this._achievementsContainer.Size.Y + base.Size.Y;
			}
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000E026C File Offset: 0x000DE46C
		public override void _Ready()
		{
			this._achievementsContainer = base.GetNode<Control>("%AchievementsContainer");
			List<NAchievementHolder> list = new List<NAchievementHolder>();
			foreach (Achievement achievement in Enum.GetValues<Achievement>())
			{
				NAchievementHolder nachievementHolder = NAchievementHolder.Create(achievement);
				if (nachievementHolder.IsUnlocked)
				{
					this._achievementsContainer.AddChildSafely(nachievementHolder);
				}
				else
				{
					list.Add(nachievementHolder);
				}
			}
			foreach (NAchievementHolder nachievementHolder2 in list)
			{
				this._achievementsContainer.AddChildSafely(nachievementHolder2);
			}
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x000E0320 File Offset: 0x000DE520
		private void OnAchievementsChanged()
		{
			foreach (NAchievementHolder nachievementHolder in this._achievementsContainer.GetChildren(false).OfType<NAchievementHolder>())
			{
				nachievementHolder.RefreshUnlocked();
			}
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000E0378 File Offset: 0x000DE578
		public override void _EnterTree()
		{
			AchievementsUtil.AchievementsChanged += this.OnAchievementsChanged;
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000E038B File Offset: 0x000DE58B
		public override void _ExitTree()
		{
			AchievementsUtil.AchievementsChanged -= this.OnAchievementsChanged;
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06002115 RID: 8469 RVA: 0x000E039E File Offset: 0x000DE59E
		[Nullable(1)]
		public Control DefaultFocusedControl
		{
			[NullableContext(1)]
			get
			{
				return this._achievementsContainer.GetChildren(false).OfType<NAchievementHolder>().First<NAchievementHolder>();
			}
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06002116 RID: 8470 RVA: 0x000E03B8 File Offset: 0x000DE5B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(4)
			{
				new MethodInfo(NAchievementsGrid.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAchievementsGrid.MethodName.OnAchievementsChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAchievementsGrid.MethodName._EnterTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NAchievementsGrid.MethodName._ExitTree, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002117 RID: 8471 RVA: 0x000E048C File Offset: 0x000DE68C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NAchievementsGrid.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAchievementsGrid.MethodName.OnAchievementsChanged && args.Count == 0)
			{
				this.OnAchievementsChanged();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAchievementsGrid.MethodName._EnterTree && args.Count == 0)
			{
				this._EnterTree();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NAchievementsGrid.MethodName._ExitTree && args.Count == 0)
			{
				this._ExitTree();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(ref method, args, ref ret);
		}

		/// <inheritdoc />
		// Token: 0x06002118 RID: 8472 RVA: 0x000E0538 File Offset: 0x000DE738
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NAchievementsGrid.MethodName._Ready || (ref method) == NAchievementsGrid.MethodName.OnAchievementsChanged || (ref method) == NAchievementsGrid.MethodName._EnterTree || (ref method) == NAchievementsGrid.MethodName._ExitTree || base.HasGodotClassMethod(ref method);
		}

		/// <inheritdoc />
		// Token: 0x06002119 RID: 8473 RVA: 0x000E0588 File Offset: 0x000DE788
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NAchievementsGrid.PropertyName._achievementsContainer)
			{
				this._achievementsContainer = VariantUtils.ConvertTo<Control>(ref value);
				return true;
			}
			if ((ref name) == NAchievementsGrid.PropertyName._scrollbarPressed)
			{
				this._scrollbarPressed = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			if ((ref name) == NAchievementsGrid.PropertyName._startDragPos)
			{
				this._startDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NAchievementsGrid.PropertyName._targetDragPos)
			{
				this._targetDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NAchievementsGrid.PropertyName._isDragging)
			{
				this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(ref name, ref value);
		}

		/// <inheritdoc />
		// Token: 0x0600211A RID: 8474 RVA: 0x000E0624 File Offset: 0x000DE824
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NAchievementsGrid.PropertyName.ScrollLimitBottom)
			{
				float scrollLimitBottom = this.ScrollLimitBottom;
				value = VariantUtils.CreateFrom<float>(ref scrollLimitBottom);
				return true;
			}
			if ((ref name) == NAchievementsGrid.PropertyName.DefaultFocusedControl)
			{
				Control defaultFocusedControl = this.DefaultFocusedControl;
				value = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
				return true;
			}
			if ((ref name) == NAchievementsGrid.PropertyName._achievementsContainer)
			{
				value = VariantUtils.CreateFrom<Control>(ref this._achievementsContainer);
				return true;
			}
			if ((ref name) == NAchievementsGrid.PropertyName._scrollbarPressed)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._scrollbarPressed);
				return true;
			}
			if ((ref name) == NAchievementsGrid.PropertyName._startDragPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._startDragPos);
				return true;
			}
			if ((ref name) == NAchievementsGrid.PropertyName._targetDragPos)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._targetDragPos);
				return true;
			}
			if ((ref name) == NAchievementsGrid.PropertyName._isDragging)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
				return true;
			}
			return base.GetGodotClassPropertyValue(ref name, ref value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600211B RID: 8475 RVA: 0x000E0720 File Offset: 0x000DE920
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(24L, NAchievementsGrid.PropertyName._achievementsContainer, 0L, "", 4096L, false),
				new PropertyInfo(1L, NAchievementsGrid.PropertyName._scrollbarPressed, 0L, "", 4096L, false),
				new PropertyInfo(5L, NAchievementsGrid.PropertyName._startDragPos, 0L, "", 4096L, false),
				new PropertyInfo(5L, NAchievementsGrid.PropertyName._targetDragPos, 0L, "", 4096L, false),
				new PropertyInfo(1L, NAchievementsGrid.PropertyName._isDragging, 0L, "", 4096L, false),
				new PropertyInfo(3L, NAchievementsGrid.PropertyName.ScrollLimitBottom, 0L, "", 4096L, false),
				new PropertyInfo(24L, NAchievementsGrid.PropertyName.DefaultFocusedControl, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600211C RID: 8476 RVA: 0x000E0818 File Offset: 0x000DEA18
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(NAchievementsGrid.PropertyName._achievementsContainer, Variant.From<Control>(ref this._achievementsContainer));
			info.AddProperty(NAchievementsGrid.PropertyName._scrollbarPressed, Variant.From<bool>(ref this._scrollbarPressed));
			info.AddProperty(NAchievementsGrid.PropertyName._startDragPos, Variant.From<Vector2>(ref this._startDragPos));
			info.AddProperty(NAchievementsGrid.PropertyName._targetDragPos, Variant.From<Vector2>(ref this._targetDragPos));
			info.AddProperty(NAchievementsGrid.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
		}

		/// <inheritdoc />
		// Token: 0x0600211D RID: 8477 RVA: 0x000E089C File Offset: 0x000DEA9C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NAchievementsGrid.PropertyName._achievementsContainer, ref variant))
			{
				this._achievementsContainer = variant.As<Control>();
			}
			Variant variant2;
			if (info.TryGetProperty(NAchievementsGrid.PropertyName._scrollbarPressed, ref variant2))
			{
				this._scrollbarPressed = variant2.As<bool>();
			}
			Variant variant3;
			if (info.TryGetProperty(NAchievementsGrid.PropertyName._startDragPos, ref variant3))
			{
				this._startDragPos = variant3.As<Vector2>();
			}
			Variant variant4;
			if (info.TryGetProperty(NAchievementsGrid.PropertyName._targetDragPos, ref variant4))
			{
				this._targetDragPos = variant4.As<Vector2>();
			}
			Variant variant5;
			if (info.TryGetProperty(NAchievementsGrid.PropertyName._isDragging, ref variant5))
			{
				this._isDragging = variant5.As<bool>();
			}
		}

		// Token: 0x04000CE1 RID: 3297
		[Nullable(1)]
		private Control _achievementsContainer;

		// Token: 0x04000CE2 RID: 3298
		private bool _scrollbarPressed;

		// Token: 0x04000CE3 RID: 3299
		private Vector2 _startDragPos;

		// Token: 0x04000CE4 RID: 3300
		private Vector2 _targetDragPos;

		// Token: 0x04000CE5 RID: 3301
		private bool _isDragging;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012F7 RID: 4855
		public class MethodName : Control.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x040049CF RID: 18895
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnAchievementsChanged' method.
			/// </summary>
			// Token: 0x040049D0 RID: 18896
			public static readonly StringName OnAchievementsChanged = "OnAchievementsChanged";

			/// <summary>
			/// Cached name for the '_EnterTree' method.
			/// </summary>
			// Token: 0x040049D1 RID: 18897
			public static readonly StringName _EnterTree = "_EnterTree";

			/// <summary>
			/// Cached name for the '_ExitTree' method.
			/// </summary>
			// Token: 0x040049D2 RID: 18898
			public static readonly StringName _ExitTree = "_ExitTree";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012F8 RID: 4856
		public class PropertyName : Control.PropertyName
		{
			/// <summary>
			/// Cached name for the 'ScrollLimitBottom' property.
			/// </summary>
			// Token: 0x040049D3 RID: 18899
			public static readonly StringName ScrollLimitBottom = "ScrollLimitBottom";

			/// <summary>
			/// Cached name for the 'DefaultFocusedControl' property.
			/// </summary>
			// Token: 0x040049D4 RID: 18900
			public static readonly StringName DefaultFocusedControl = "DefaultFocusedControl";

			/// <summary>
			/// Cached name for the '_achievementsContainer' field.
			/// </summary>
			// Token: 0x040049D5 RID: 18901
			public static readonly StringName _achievementsContainer = "_achievementsContainer";

			/// <summary>
			/// Cached name for the '_scrollbarPressed' field.
			/// </summary>
			// Token: 0x040049D6 RID: 18902
			public static readonly StringName _scrollbarPressed = "_scrollbarPressed";

			/// <summary>
			/// Cached name for the '_startDragPos' field.
			/// </summary>
			// Token: 0x040049D7 RID: 18903
			public static readonly StringName _startDragPos = "_startDragPos";

			/// <summary>
			/// Cached name for the '_targetDragPos' field.
			/// </summary>
			// Token: 0x040049D8 RID: 18904
			public static readonly StringName _targetDragPos = "_targetDragPos";

			/// <summary>
			/// Cached name for the '_isDragging' field.
			/// </summary>
			// Token: 0x040049D9 RID: 18905
			public static readonly StringName _isDragging = "_isDragging";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x020012F9 RID: 4857
		public class SignalName : Control.SignalName
		{
		}
	}
}

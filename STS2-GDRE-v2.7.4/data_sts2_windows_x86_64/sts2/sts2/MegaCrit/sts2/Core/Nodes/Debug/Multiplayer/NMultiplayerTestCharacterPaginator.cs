using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer
{
	// Token: 0x020003CF RID: 975
	[ScriptPath("res://src/Core/Nodes/Debug/Multiplayer/NMultiplayerTestCharacterPaginator.cs")]
	public class NMultiplayerTestCharacterPaginator : NPaginator
	{
		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06003B0E RID: 15118 RVA: 0x001C30A8 File Offset: 0x001C12A8
		// (remove) Token: 0x06003B0F RID: 15119 RVA: 0x001C30E0 File Offset: 0x001C12E0
		[Nullable(new byte[] { 2, 1 })]
		[field: Nullable(new byte[] { 2, 1 })]
		public event Action<CharacterModel> CharacterChanged;

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06003B10 RID: 15120 RVA: 0x001C3115 File Offset: 0x001C1315
		[Nullable(1)]
		public CharacterModel Character
		{
			[NullableContext(1)]
			get
			{
				return this._characters[this._currentIndex];
			}
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x001C3124 File Offset: 0x001C1324
		public override void _Ready()
		{
			base.ConnectSignals();
			foreach (CharacterModel characterModel in this._characters)
			{
				this._options.Add(new LocString("characters", characterModel.Id.Entry + ".title").GetFormattedText());
			}
			this._label.Text = this._options[this._currentIndex];
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x001C319B File Offset: 0x001C139B
		protected override void OnIndexChanged(int index)
		{
			this._currentIndex = index;
			this._label.Text = this._characters[index].Title.GetFormattedText();
			Action<CharacterModel> characterChanged = this.CharacterChanged;
			if (characterChanged == null)
			{
				return;
			}
			characterChanged(this._characters[index]);
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x06003B13 RID: 15123 RVA: 0x001C31DC File Offset: 0x001C13DC
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(NMultiplayerTestCharacterPaginator.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMultiplayerTestCharacterPaginator.MethodName.OnIndexChanged, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, new List<PropertyInfo>
				{
					new PropertyInfo(2L, "index", 0L, "", 6L, false)
				}, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x06003B14 RID: 15124 RVA: 0x001C3274 File Offset: 0x001C1474
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMultiplayerTestCharacterPaginator.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMultiplayerTestCharacterPaginator.MethodName.OnIndexChanged && args.Count == 1)
			{
				this.OnIndexChanged(VariantUtils.ConvertTo<int>(args[0]));
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x06003B15 RID: 15125 RVA: 0x001C32E2 File Offset: 0x001C14E2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMultiplayerTestCharacterPaginator.MethodName._Ready || (ref method) == NMultiplayerTestCharacterPaginator.MethodName.OnIndexChanged || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x06003B16 RID: 15126 RVA: 0x001C3309 File Offset: 0x001C1509
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
		}

		/// <inheritdoc />
		// Token: 0x06003B17 RID: 15127 RVA: 0x001C3312 File Offset: 0x001C1512
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
		}

		// Token: 0x04001705 RID: 5893
		[Nullable(1)]
		private readonly CharacterModel[] _characters = new CharacterModel[]
		{
			ModelDb.Character<Ironclad>(),
			ModelDb.Character<Silent>(),
			ModelDb.Character<Regent>(),
			ModelDb.Character<Necrobinder>(),
			ModelDb.Character<Defect>()
		};

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001816 RID: 6166
		public new class MethodName : NPaginator.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04005E37 RID: 24119
			public new static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnIndexChanged' method.
			/// </summary>
			// Token: 0x04005E38 RID: 24120
			public new static readonly StringName OnIndexChanged = "OnIndexChanged";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001817 RID: 6167
		public new class PropertyName : NPaginator.PropertyName
		{
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x02001818 RID: 6168
		public new class SignalName : NPaginator.SignalName
		{
		}
	}
}

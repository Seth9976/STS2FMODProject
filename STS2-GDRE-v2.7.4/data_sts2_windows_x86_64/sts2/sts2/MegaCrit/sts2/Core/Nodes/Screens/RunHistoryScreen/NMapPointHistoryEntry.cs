using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;

namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen
{
	// Token: 0x020002D1 RID: 721
	[NullableContext(1)]
	[Nullable(0)]
	[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NMapPointHistoryEntry.cs")]
	public class NMapPointHistoryEntry : NClickableControl
	{
		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002625 RID: 9765 RVA: 0x00101797 File Offset: 0x000FF997
		private static string ScenePath
		{
			get
			{
				return SceneHelper.GetScenePath("screens/run_history_screen/map_point_history_entry");
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002626 RID: 9766 RVA: 0x001017A3 File Offset: 0x000FF9A3
		public static IEnumerable<string> AssetPaths
		{
			get
			{
				return NMapPointHistoryEntry.GetAssetPaths();
			}
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x001017AA File Offset: 0x000FF9AA
		private static IEnumerable<string> GetAssetPaths()
		{
			return new NMapPointHistoryEntry.<GetAssetPaths>d__4(-2);
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002628 RID: 9768 RVA: 0x001017B3 File Offset: 0x000FF9B3
		// (set) Token: 0x06002629 RID: 9769 RVA: 0x001017BB File Offset: 0x000FF9BB
		public int FloorNum { get; private set; }

		// Token: 0x0600262A RID: 9770 RVA: 0x001017C4 File Offset: 0x000FF9C4
		public override void _Ready()
		{
			this._baseAngle = Rng.Chaotic.NextGaussianFloat(0f, 1f, 0f, 5f);
			this._texture = base.GetNode<TextureRect>("%Icon");
			this._outline = base.GetNode<TextureRect>("%Outline");
			this._questIcon = base.GetNode<TextureRect>("%QuestIcon");
			this._texture.RotationDegrees = (Rng.Chaotic.NextBool() ? this._baseAngle : (-this._baseAngle));
			MapPointType mapPointType = this._entry.MapPointType;
			RoomType roomType = this._entry.Rooms.First<MapPointRoomHistoryEntry>().RoomType;
			MapPointType mapPointType2 = this._entry.MapPointType;
			bool flag = mapPointType2 - MapPointType.Boss <= 1;
			string roomIconPath = ImageHelper.GetRoomIconPath(mapPointType, roomType, flag ? this._entry.Rooms.First<MapPointRoomHistoryEntry>().ModelId : null);
			MapPointType mapPointType3 = this._entry.MapPointType;
			RoomType roomType2 = this._entry.Rooms.First<MapPointRoomHistoryEntry>().RoomType;
			mapPointType2 = this._entry.MapPointType;
			flag = mapPointType2 - MapPointType.Boss <= 1;
			string roomIconOutlinePath = ImageHelper.GetRoomIconOutlinePath(mapPointType3, roomType2, flag ? this._entry.Rooms.First<MapPointRoomHistoryEntry>().ModelId : null);
			if (roomIconPath != null)
			{
				this._texture.Visible = true;
				this._texture.Texture = PreloadManager.Cache.GetCompressedTexture2D(roomIconPath);
			}
			else
			{
				this._texture.Visible = false;
			}
			if (roomIconOutlinePath != null)
			{
				this._outline.Visible = true;
				this._outline.Texture = PreloadManager.Cache.GetCompressedTexture2D(roomIconOutlinePath);
			}
			else
			{
				this._outline.Visible = false;
			}
			this._questIcon.Visible = false;
			Color modulate = base.Modulate;
			modulate.A = 1f;
			base.Modulate = modulate;
			this.ConnectSignals();
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x001019BC File Offset: 0x000FFBBC
		public static NMapPointHistoryEntry Create(RunHistory history, MapPointHistoryEntry entry, int floorNum)
		{
			NMapPointHistoryEntry nmapPointHistoryEntry = PreloadManager.Cache.GetScene(NMapPointHistoryEntry.ScenePath).Instantiate<NMapPointHistoryEntry>(0L);
			nmapPointHistoryEntry._runHistory = history;
			nmapPointHistoryEntry._entry = entry;
			nmapPointHistoryEntry.FloorNum = floorNum;
			return nmapPointHistoryEntry;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x001019F8 File Offset: 0x000FFBF8
		public void SetPlayer(RunHistoryPlayer player)
		{
			this._player = player;
			PlayerMapPointHistoryEntry entry = this._entry.GetEntry(this._player.Id);
			this._questIcon.Visible = entry.CompletedQuests.Count > 0 || entry.IsAffectedByFurCoat;
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x00101A48 File Offset: 0x000FFC48
		protected override void OnFocus()
		{
			if (this._player == null)
			{
				throw new InvalidOperationException("Player has not been set!");
			}
			this.Highlight();
			HoverTipAlignment alignment = HoverTip.GetHoverTipAlignment(this, 0.75f);
			NHoverTipSet tip = NHoverTipSet.CreateAndShowMapPointHistory(this, NMapPointHistoryHoverTip.Create(this.FloorNum, this._player.Id, this._entry));
			Callable.From(delegate
			{
				tip.SetAlignment(this, alignment);
			}).CallDeferred(Array.Empty<Variant>());
			tip.GlobalPosition += Vector2.Down * 96f;
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x00101AF7 File Offset: 0x000FFCF7
		protected override void OnUnfocus()
		{
			this.Unhighlight();
			NHoverTipSet.Remove(this);
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x00101B08 File Offset: 0x000FFD08
		public void Highlight()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this._texture, "scale", this._baseScale * 1.5f, 0.05).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._texture, "rotation_degrees", 0f, 0.05).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._outline, "modulate", StsColors.halfTransparentWhite, 0.05).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x00101BFC File Offset: 0x000FFDFC
		public void Unhighlight()
		{
			Tween hoverTween = this._hoverTween;
			if (hoverTween != null)
			{
				hoverTween.Kill();
			}
			this._hoverTween = base.CreateTween().SetParallel(true);
			this._hoverTween.TweenProperty(this._texture, "scale", this._baseScale, 1.0).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._texture, "rotation_degrees", this._baseAngle, 1.0).SetEase(1L).SetTrans(5L);
			this._hoverTween.TweenProperty(this._outline, "modulate", StsColors.quarterTransparentBlack, 1.0).SetEase(1L).SetTrans(5L);
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x00101CE8 File Offset: 0x000FFEE8
		public async Task AnimateIn(int index)
		{
			base.Visible = true;
			base.Scale = Vector2.One * 0.01f;
			Tween animateInTween = this._animateInTween;
			if (animateInTween != null)
			{
				animateInTween.Kill();
			}
			this._animateInTween = base.CreateTween().SetParallel(true);
			this._animateInTween.TweenProperty(this, "scale", Vector2.One, this._hurryUp ? 0.05 : 0.4).SetEase(1L).SetTrans(11L);
			this._animateInTween.TweenProperty(this, "modulate", Colors.White, this._hurryUp ? 0.05 : 0.1);
			TaskHelper.RunSafely(this.DoAnimateInEffects());
			if (this._hurryUp)
			{
				await Cmd.Wait(0.05f, false);
			}
			else
			{
				float num = ((SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast) ? 0.4f : 0.5f);
				float num2 = 0.2f;
				int num3 = ((SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast) ? 5 : 9);
				await Cmd.Wait(Mathf.Lerp(num, num2, (float)index / (float)(index + num3)), false);
			}
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x00101D34 File Offset: 0x000FFF34
		private async Task DoAnimateInEffects()
		{
			PlayerMapPointHistoryEntry entry = this._entry.GetEntry(this._player.Id);
			RoomType roomType = this._entry.Rooms.Last<MapPointRoomHistoryEntry>().RoomType;
			if (this._runHistory.MapPointHistory.Last<List<MapPointHistoryEntry>>().Last<MapPointHistoryEntry>() == this._entry && !this._runHistory.Win)
			{
				SfxCmd.Play("event:/sfx/block_break", 1f);
				NDebugAudioManager instance = NDebugAudioManager.Instance;
				if (instance != null)
				{
					instance.Play("STS_DeathStinger_v4_Short_SFX.mp3", 0.75f * this.GetSfxVolume(), PitchVariance.None);
				}
			}
			else
			{
				bool flag = roomType - RoomType.Monster <= 2;
				if (flag)
				{
					await this.DoCombatAnimateInEffects(roomType);
				}
				else if (roomType == RoomType.Shop)
				{
					SfxCmd.Play("event:/sfx/npcs/merchant/merchant_welcome", 1f);
				}
				else if (roomType == RoomType.Treasure)
				{
					SfxCmd.Play("event:/sfx/ui/gold/gold_2", this.GetSfxVolume());
				}
				else if (roomType == RoomType.RestSite)
				{
					if (entry.RestSiteChoices.Contains("SMITH"))
					{
						NGame.Instance.ScreenRumble(ShakeStrength.Medium, ShakeDuration.Short, RumbleStyle.Rumble);
						NDebugAudioManager instance2 = NDebugAudioManager.Instance;
						if (instance2 != null)
						{
							instance2.Play("card_smith.mp3", this.GetSfxVolume(), PitchVariance.Small);
						}
					}
					else if (entry.RestSiteChoices.Contains("HEAL"))
					{
						NDebugAudioManager instance3 = NDebugAudioManager.Instance;
						if (instance3 != null)
						{
							instance3.Play("SOTE_SFX_SleepBlanket_v1.mp3", this.GetSfxVolume(), PitchVariance.Medium);
						}
					}
					else if (entry.RestSiteChoices.Contains("DIG"))
					{
						NDebugAudioManager.Instance.Play("sts_sfx_shovel_v1.mp3", this.GetSfxVolume(), PitchVariance.Small);
					}
					else if (entry.RestSiteChoices.Contains("HATCH"))
					{
						SfxCmd.Play("event:/sfx/byrdpip/byrdpip_attack", 1f);
					}
					else if (entry.RestSiteChoices.Contains("LIFT"))
					{
						NGame instance4 = NGame.Instance;
						if (instance4 != null)
						{
							instance4.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short, -1f);
						}
					}
					else if (entry.RestSiteChoices.Contains("MEND"))
					{
					}
				}
				else if (roomType == RoomType.Event)
				{
					SfxCmd.Play("event:/sfx/ui/clicks/ui_hover", 1f);
				}
			}
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x00101D78 File Offset: 0x000FFF78
		private async Task DoCombatAnimateInEffects(RoomType roomType)
		{
			CharacterModel characterModel = SaveUtil.CharacterOrDeprecated(this._player.Character);
			ShakeStrength? shakeStrength = null;
			if (roomType == RoomType.Monster)
			{
				shakeStrength = new ShakeStrength?(ShakeStrength.Weak);
				await this.PlaySfx(this.GetSmallHitSfx(characterModel));
			}
			else if (roomType == RoomType.Elite)
			{
				shakeStrength = new ShakeStrength?(ShakeStrength.Medium);
				await this.PlaySfx(this.GetBigHitSfx(characterModel));
			}
			else if (roomType == RoomType.Boss)
			{
				shakeStrength = new ShakeStrength?(ShakeStrength.Strong);
				await this.PlaySfx(this.GetBigHitSfx(characterModel));
			}
			if (shakeStrength != null)
			{
				NGame.Instance.ScreenRumble(shakeStrength.Value, ShakeDuration.Normal, RumbleStyle.Rumble);
			}
			await Cmd.Wait(0.25f, false);
			foreach (ModelId modelId in this._entry.Rooms.Last<MapPointRoomHistoryEntry>().MonsterIds)
			{
				MonsterModel monsterModel = SaveUtil.MonsterOrDeprecated(modelId);
				if (monsterModel.HasDeathSfx)
				{
					SfxCmd.Play(monsterModel.DeathSfx, 1f);
					await Cmd.Wait(0.25f, false);
				}
			}
			List<ModelId>.Enumerator enumerator = default(List<ModelId>.Enumerator);
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x00101DC4 File Offset: 0x000FFFC4
		private unsafe List<string> GetSmallHitSfx(CharacterModel character)
		{
			List<string> list;
			if (!(character is Defect))
			{
				if (!(character is Ironclad))
				{
					if (!(character is Necrobinder))
					{
						if (!(character is Regent))
						{
							if (!(character is Silent))
							{
								list = new List<string>();
							}
							else
							{
								int num = 1;
								List<string> list2 = new List<string>(num);
								CollectionsMarshal.SetCount<string>(list2, num);
								Span<string> span = CollectionsMarshal.AsSpan<string>(list2);
								int num2 = 0;
								*span[num2] = "slash_attack.mp3";
								list = list2;
							}
						}
						else
						{
							int num2 = 1;
							List<string> list3 = new List<string>(num2);
							CollectionsMarshal.SetCount<string>(list3, num2);
							Span<string> span = CollectionsMarshal.AsSpan<string>(list3);
							int num = 0;
							*span[num] = "slash_attack.mp3";
							list = list3;
						}
					}
					else
					{
						int num = 1;
						List<string> list4 = new List<string>(num);
						CollectionsMarshal.SetCount<string>(list4, num);
						Span<string> span = CollectionsMarshal.AsSpan<string>(list4);
						int num2 = 0;
						*span[num2] = "slash_attack.mp3";
						list = list4;
					}
				}
				else
				{
					int num2 = 1;
					List<string> list5 = new List<string>(num2);
					CollectionsMarshal.SetCount<string>(list5, num2);
					Span<string> span = CollectionsMarshal.AsSpan<string>(list5);
					int num = 0;
					*span[num] = "blunt_attack.mp3";
					list = list5;
				}
			}
			else
			{
				int num = 1;
				List<string> list6 = new List<string>(num);
				CollectionsMarshal.SetCount<string>(list6, num);
				Span<string> span = CollectionsMarshal.AsSpan<string>(list6);
				int num2 = 0;
				*span[num2] = "slash_attack.mp3";
				list = list6;
			}
			return list;
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x00101EE0 File Offset: 0x001000E0
		private unsafe List<string> GetBigHitSfx(CharacterModel character)
		{
			List<string> list;
			if (!(character is Defect))
			{
				if (!(character is Ironclad))
				{
					if (!(character is Necrobinder))
					{
						if (!(character is Regent))
						{
							if (!(character is Silent))
							{
								list = new List<string>();
							}
							else
							{
								int num = 2;
								List<string> list2 = new List<string>(num);
								CollectionsMarshal.SetCount<string>(list2, num);
								Span<string> span = CollectionsMarshal.AsSpan<string>(list2);
								int num2 = 0;
								*span[num2] = "dagger_throw.mp3";
								num2++;
								*span[num2] = "dagger_throw.mp3";
								list = list2;
							}
						}
						else
						{
							int num2 = 1;
							List<string> list3 = new List<string>(num2);
							CollectionsMarshal.SetCount<string>(list3, num2);
							Span<string> span = CollectionsMarshal.AsSpan<string>(list3);
							int num = 0;
							*span[num] = "heavy_attack.mp3";
							list = list3;
						}
					}
					else
					{
						int num = 1;
						List<string> list4 = new List<string>(num);
						CollectionsMarshal.SetCount<string>(list4, num);
						Span<string> span = CollectionsMarshal.AsSpan<string>(list4);
						int num2 = 0;
						*span[num2] = "heavy_attack.mp3";
						list = list4;
					}
				}
				else
				{
					int num2 = 1;
					List<string> list5 = new List<string>(num2);
					CollectionsMarshal.SetCount<string>(list5, num2);
					Span<string> span = CollectionsMarshal.AsSpan<string>(list5);
					int num = 0;
					*span[num] = "heavy_attack.mp3";
					list = list5;
				}
			}
			else
			{
				int num = 1;
				List<string> list6 = new List<string>(num);
				CollectionsMarshal.SetCount<string>(list6, num);
				Span<string> span = CollectionsMarshal.AsSpan<string>(list6);
				int num2 = 0;
				*span[num2] = "lightning_orb_evoke.mp3";
				list = list6;
			}
			return list;
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x0010200C File Offset: 0x0010020C
		private async Task PlaySfx(List<string> sfxPaths)
		{
			for (int i = 0; i < sfxPaths.Count; i++)
			{
				string text = sfxPaths[i];
				if (text.StartsWith("event:"))
				{
					SfxCmd.Play(text, this.GetSfxVolume());
				}
				else
				{
					NDebugAudioManager instance = NDebugAudioManager.Instance;
					if (instance != null)
					{
						instance.Play(text, this.GetSfxVolume(), PitchVariance.Medium);
					}
				}
				if (i < sfxPaths.Count - 1)
				{
					await Cmd.Wait(0.1f, false);
				}
			}
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x00102057 File Offset: 0x00100257
		private float GetSfxVolume()
		{
			if (!this._hurryUp)
			{
				return 1f;
			}
			return 0.5f;
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x0010206C File Offset: 0x0010026C
		public void HurryUp()
		{
			this._hurryUp = true;
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x00102075 File Offset: 0x00100275
		public void SetupForAnimation()
		{
			base.Visible = false;
			base.Modulate = StsColors.transparentBlack;
		}

		/// <summary>
		/// Get the method information for all the methods declared in this class.
		/// This method is used by Godot to register the available methods in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600263A RID: 9786 RVA: 0x0010208C File Offset: 0x0010028C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(8)
			{
				new MethodInfo(NMapPointHistoryEntry.MethodName._Ready, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPointHistoryEntry.MethodName.OnFocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPointHistoryEntry.MethodName.OnUnfocus, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPointHistoryEntry.MethodName.Highlight, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPointHistoryEntry.MethodName.Unhighlight, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPointHistoryEntry.MethodName.GetSfxVolume, new PropertyInfo(3L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPointHistoryEntry.MethodName.HurryUp, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null),
				new MethodInfo(NMapPointHistoryEntry.MethodName.SetupForAnimation, new PropertyInfo(0L, "", 0L, "", 6L, false), 1L, null, null)
			};
		}

		/// <inheritdoc />
		// Token: 0x0600263B RID: 9787 RVA: 0x0010221C File Offset: 0x0010041C
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if ((ref method) == NMapPointHistoryEntry.MethodName._Ready && args.Count == 0)
			{
				this._Ready();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPointHistoryEntry.MethodName.OnFocus && args.Count == 0)
			{
				this.OnFocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPointHistoryEntry.MethodName.OnUnfocus && args.Count == 0)
			{
				this.OnUnfocus();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPointHistoryEntry.MethodName.Highlight && args.Count == 0)
			{
				this.Highlight();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPointHistoryEntry.MethodName.Unhighlight && args.Count == 0)
			{
				this.Unhighlight();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPointHistoryEntry.MethodName.GetSfxVolume && args.Count == 0)
			{
				float sfxVolume = this.GetSfxVolume();
				ret = VariantUtils.CreateFrom<float>(ref sfxVolume);
				return true;
			}
			if ((ref method) == NMapPointHistoryEntry.MethodName.HurryUp && args.Count == 0)
			{
				this.HurryUp();
				ret = default(godot_variant);
				return true;
			}
			if ((ref method) == NMapPointHistoryEntry.MethodName.SetupForAnimation && args.Count == 0)
			{
				this.SetupForAnimation();
				ret = default(godot_variant);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		/// <inheritdoc />
		// Token: 0x0600263C RID: 9788 RVA: 0x00102364 File Offset: 0x00100564
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			return (ref method) == NMapPointHistoryEntry.MethodName._Ready || (ref method) == NMapPointHistoryEntry.MethodName.OnFocus || (ref method) == NMapPointHistoryEntry.MethodName.OnUnfocus || (ref method) == NMapPointHistoryEntry.MethodName.Highlight || (ref method) == NMapPointHistoryEntry.MethodName.Unhighlight || (ref method) == NMapPointHistoryEntry.MethodName.GetSfxVolume || (ref method) == NMapPointHistoryEntry.MethodName.HurryUp || (ref method) == NMapPointHistoryEntry.MethodName.SetupForAnimation || base.HasGodotClassMethod(in method);
		}

		/// <inheritdoc />
		// Token: 0x0600263D RID: 9789 RVA: 0x001023F0 File Offset: 0x001005F0
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if ((ref name) == NMapPointHistoryEntry.PropertyName.FloorNum)
			{
				this.FloorNum = VariantUtils.ConvertTo<int>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._baseScale)
			{
				this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._texture)
			{
				this._texture = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._outline)
			{
				this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._questIcon)
			{
				this._questIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._animateInTween)
			{
				this._animateInTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._hoverTween)
			{
				this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._baseAngle)
			{
				this._baseAngle = VariantUtils.ConvertTo<float>(ref value);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._hurryUp)
			{
				this._hurryUp = VariantUtils.ConvertTo<bool>(ref value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		/// <inheritdoc />
		// Token: 0x0600263E RID: 9790 RVA: 0x001024F8 File Offset: 0x001006F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if ((ref name) == NMapPointHistoryEntry.PropertyName.FloorNum)
			{
				int floorNum = this.FloorNum;
				value = VariantUtils.CreateFrom<int>(ref floorNum);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._baseScale)
			{
				value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._texture)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._texture);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._outline)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._questIcon)
			{
				value = VariantUtils.CreateFrom<TextureRect>(ref this._questIcon);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._animateInTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._animateInTween);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._hoverTween)
			{
				value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._baseAngle)
			{
				value = VariantUtils.CreateFrom<float>(ref this._baseAngle);
				return true;
			}
			if ((ref name) == NMapPointHistoryEntry.PropertyName._hurryUp)
			{
				value = VariantUtils.CreateFrom<bool>(ref this._hurryUp);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		/// <summary>
		/// Get the property information for all the properties declared in this class.
		/// This method is used by Godot to register the available properties in the editor.
		/// Do not call this method.
		/// </summary>
		// Token: 0x0600263F RID: 9791 RVA: 0x00102630 File Offset: 0x00100830
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal new static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(5L, NMapPointHistoryEntry.PropertyName._baseScale, 0L, "", 4096L, false),
				new PropertyInfo(2L, NMapPointHistoryEntry.PropertyName.FloorNum, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPointHistoryEntry.PropertyName._texture, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPointHistoryEntry.PropertyName._outline, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPointHistoryEntry.PropertyName._questIcon, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPointHistoryEntry.PropertyName._animateInTween, 0L, "", 4096L, false),
				new PropertyInfo(24L, NMapPointHistoryEntry.PropertyName._hoverTween, 0L, "", 4096L, false),
				new PropertyInfo(3L, NMapPointHistoryEntry.PropertyName._baseAngle, 0L, "", 4096L, false),
				new PropertyInfo(1L, NMapPointHistoryEntry.PropertyName._hurryUp, 0L, "", 4096L, false)
			};
		}

		/// <inheritdoc />
		// Token: 0x06002640 RID: 9792 RVA: 0x0010276C File Offset: 0x0010096C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			StringName floorNum = NMapPointHistoryEntry.PropertyName.FloorNum;
			int floorNum2 = this.FloorNum;
			info.AddProperty(floorNum, Variant.From<int>(ref floorNum2));
			info.AddProperty(NMapPointHistoryEntry.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
			info.AddProperty(NMapPointHistoryEntry.PropertyName._texture, Variant.From<TextureRect>(ref this._texture));
			info.AddProperty(NMapPointHistoryEntry.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
			info.AddProperty(NMapPointHistoryEntry.PropertyName._questIcon, Variant.From<TextureRect>(ref this._questIcon));
			info.AddProperty(NMapPointHistoryEntry.PropertyName._animateInTween, Variant.From<Tween>(ref this._animateInTween));
			info.AddProperty(NMapPointHistoryEntry.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
			info.AddProperty(NMapPointHistoryEntry.PropertyName._baseAngle, Variant.From<float>(ref this._baseAngle));
			info.AddProperty(NMapPointHistoryEntry.PropertyName._hurryUp, Variant.From<bool>(ref this._hurryUp));
		}

		/// <inheritdoc />
		// Token: 0x06002641 RID: 9793 RVA: 0x0010284C File Offset: 0x00100A4C
		[NullableContext(0)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			Variant variant;
			if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName.FloorNum, ref variant))
			{
				this.FloorNum = variant.As<int>();
			}
			Variant variant2;
			if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._baseScale, ref variant2))
			{
				this._baseScale = variant2.As<Vector2>();
			}
			Variant variant3;
			if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._texture, ref variant3))
			{
				this._texture = variant3.As<TextureRect>();
			}
			Variant variant4;
			if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._outline, ref variant4))
			{
				this._outline = variant4.As<TextureRect>();
			}
			Variant variant5;
			if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._questIcon, ref variant5))
			{
				this._questIcon = variant5.As<TextureRect>();
			}
			Variant variant6;
			if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._animateInTween, ref variant6))
			{
				this._animateInTween = variant6.As<Tween>();
			}
			Variant variant7;
			if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._hoverTween, ref variant7))
			{
				this._hoverTween = variant7.As<Tween>();
			}
			Variant variant8;
			if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._baseAngle, ref variant8))
			{
				this._baseAngle = variant8.As<float>();
			}
			Variant variant9;
			if (info.TryGetProperty(NMapPointHistoryEntry.PropertyName._hurryUp, ref variant9))
			{
				this._hurryUp = variant9.As<bool>();
			}
		}

		// Token: 0x04000E18 RID: 3608
		private Vector2 _baseScale = Vector2.One * 0.7f;

		// Token: 0x04000E19 RID: 3609
		private RunHistory _runHistory;

		// Token: 0x04000E1A RID: 3610
		private MapPointHistoryEntry _entry;

		// Token: 0x04000E1C RID: 3612
		[Nullable(2)]
		private RunHistoryPlayer _player;

		// Token: 0x04000E1D RID: 3613
		private TextureRect _texture;

		// Token: 0x04000E1E RID: 3614
		private TextureRect _outline;

		// Token: 0x04000E1F RID: 3615
		private TextureRect _questIcon;

		// Token: 0x04000E20 RID: 3616
		[Nullable(2)]
		private Tween _animateInTween;

		// Token: 0x04000E21 RID: 3617
		[Nullable(2)]
		private Tween _hoverTween;

		// Token: 0x04000E22 RID: 3618
		private float _baseAngle;

		// Token: 0x04000E23 RID: 3619
		private bool _hurryUp;

		/// <summary>
		/// Cached StringNames for the methods contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200142D RID: 5165
		[NullableContext(0)]
		public new class MethodName : NClickableControl.MethodName
		{
			/// <summary>
			/// Cached name for the '_Ready' method.
			/// </summary>
			// Token: 0x04004C8D RID: 19597
			public static readonly StringName _Ready = "_Ready";

			/// <summary>
			/// Cached name for the 'OnFocus' method.
			/// </summary>
			// Token: 0x04004C8E RID: 19598
			public new static readonly StringName OnFocus = "OnFocus";

			/// <summary>
			/// Cached name for the 'OnUnfocus' method.
			/// </summary>
			// Token: 0x04004C8F RID: 19599
			public new static readonly StringName OnUnfocus = "OnUnfocus";

			/// <summary>
			/// Cached name for the 'Highlight' method.
			/// </summary>
			// Token: 0x04004C90 RID: 19600
			public static readonly StringName Highlight = "Highlight";

			/// <summary>
			/// Cached name for the 'Unhighlight' method.
			/// </summary>
			// Token: 0x04004C91 RID: 19601
			public static readonly StringName Unhighlight = "Unhighlight";

			/// <summary>
			/// Cached name for the 'GetSfxVolume' method.
			/// </summary>
			// Token: 0x04004C92 RID: 19602
			public static readonly StringName GetSfxVolume = "GetSfxVolume";

			/// <summary>
			/// Cached name for the 'HurryUp' method.
			/// </summary>
			// Token: 0x04004C93 RID: 19603
			public static readonly StringName HurryUp = "HurryUp";

			/// <summary>
			/// Cached name for the 'SetupForAnimation' method.
			/// </summary>
			// Token: 0x04004C94 RID: 19604
			public static readonly StringName SetupForAnimation = "SetupForAnimation";
		}

		/// <summary>
		/// Cached StringNames for the properties and fields contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200142E RID: 5166
		[NullableContext(0)]
		public new class PropertyName : NClickableControl.PropertyName
		{
			/// <summary>
			/// Cached name for the 'FloorNum' property.
			/// </summary>
			// Token: 0x04004C95 RID: 19605
			public static readonly StringName FloorNum = "FloorNum";

			/// <summary>
			/// Cached name for the '_baseScale' field.
			/// </summary>
			// Token: 0x04004C96 RID: 19606
			public static readonly StringName _baseScale = "_baseScale";

			/// <summary>
			/// Cached name for the '_texture' field.
			/// </summary>
			// Token: 0x04004C97 RID: 19607
			public static readonly StringName _texture = "_texture";

			/// <summary>
			/// Cached name for the '_outline' field.
			/// </summary>
			// Token: 0x04004C98 RID: 19608
			public static readonly StringName _outline = "_outline";

			/// <summary>
			/// Cached name for the '_questIcon' field.
			/// </summary>
			// Token: 0x04004C99 RID: 19609
			public static readonly StringName _questIcon = "_questIcon";

			/// <summary>
			/// Cached name for the '_animateInTween' field.
			/// </summary>
			// Token: 0x04004C9A RID: 19610
			public static readonly StringName _animateInTween = "_animateInTween";

			/// <summary>
			/// Cached name for the '_hoverTween' field.
			/// </summary>
			// Token: 0x04004C9B RID: 19611
			public static readonly StringName _hoverTween = "_hoverTween";

			/// <summary>
			/// Cached name for the '_baseAngle' field.
			/// </summary>
			// Token: 0x04004C9C RID: 19612
			public static readonly StringName _baseAngle = "_baseAngle";

			/// <summary>
			/// Cached name for the '_hurryUp' field.
			/// </summary>
			// Token: 0x04004C9D RID: 19613
			public static readonly StringName _hurryUp = "_hurryUp";
		}

		/// <summary>
		/// Cached StringNames for the signals contained in this class, for fast lookup.
		/// </summary>
		// Token: 0x0200142F RID: 5167
		[NullableContext(0)]
		public new class SignalName : NClickableControl.SignalName
		{
		}
	}
}

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace MegaCrit.Sts2.Core.Debug
{
	// Token: 0x02000D0B RID: 3339
	[NullableContext(1)]
	[Nullable(0)]
	public static class FileDropHandler
	{
		// Token: 0x060086B9 RID: 34489 RVA: 0x002D35B8 File Offset: 0x002D17B8
		public static void OnFilesDropped(string[] files)
		{
			if (!OS.HasFeature("editor") && !ModManager.IsRunningModded() && !SaveManager.Instance.SettingsSave.FullConsole)
			{
				return;
			}
			if (files.Length > 1)
			{
				return;
			}
			string text = files[0];
			if (!text.EndsWith(".run"))
			{
				Log.Error("We only support dropping .run files. You dropped " + text + ".", 2);
				return;
			}
			TaskHelper.RunSafely(FileDropHandler.OnRunHistoryDropped(text));
		}

		// Token: 0x060086BA RID: 34490 RVA: 0x002D362C File Offset: 0x002D182C
		private static async Task OnRunHistoryDropped(string file)
		{
			if (!RunManager.Instance.IsInProgress)
			{
				Log.Error("Can only load run history while run is in progress.", 2);
			}
			else
			{
				RunState runState = RunManager.Instance.DebugOnlyGetState();
				if (runState.Players.Count > 1)
				{
					Log.Error("Only singleplayer supported for now.", 2);
				}
				else
				{
					using (FileAccess fileAccess = FileAccess.Open(file, 1L))
					{
						if (fileAccess == null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Couldn't open file ");
							defaultInterpolatedStringHandler.AppendFormatted(file);
							defaultInterpolatedStringHandler.AppendLiteral(": ");
							defaultInterpolatedStringHandler.AppendFormatted<Error>(FileAccess.GetOpenError());
							Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
						}
						else
						{
							ReadSaveResult<RunHistory> readSaveResult = JsonSerializationUtility.FromJson<RunHistory>(fileAccess.GetAsText(false));
							if (!readSaveResult.Success)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Couldn't read ");
								defaultInterpolatedStringHandler.AppendFormatted(file);
								defaultInterpolatedStringHandler.AppendLiteral(": ");
								defaultInterpolatedStringHandler.AppendFormatted(readSaveResult.ErrorMessage);
								defaultInterpolatedStringHandler.AppendLiteral(" (");
								defaultInterpolatedStringHandler.AppendFormatted<ReadSaveStatus>(readSaveResult.Status);
								defaultInterpolatedStringHandler.AppendLiteral(")");
								Log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), 2);
							}
							else
							{
								RunHistory saveData = readSaveResult.SaveData;
								if (saveData.Players.Count > 1)
								{
									Log.Error("Only singleplayer supported for now.", 2);
								}
								else
								{
									RunHistoryPlayer runHistoryPlayer = saveData.Players[0];
									Log.Info("Successfully loaded file " + file, 2);
									SerializableRun savedRun = RunManager.Instance.ToSave(null);
									SerializablePlayer serializablePlayer = savedRun.Players[0];
									serializablePlayer.Deck.Clear();
									serializablePlayer.Deck.AddRange(runHistoryPlayer.Deck);
									serializablePlayer.Relics.Clear();
									serializablePlayer.Relics.AddRange(runHistoryPlayer.Relics);
									serializablePlayer.Potions.Clear();
									serializablePlayer.Potions.AddRange(runHistoryPlayer.Potions);
									RunManager.Instance.CleanUp(true);
									RunState loadedState = RunState.FromSerializable(savedRun);
									await RunManager.Instance.SetUpSavedSingleplayer(loadedState, savedRun);
									NGame.Instance.ReactionContainer.InitializeNetworking(new NetSingleplayerGameService());
									await NGame.Instance.LoadRun(loadedState, savedRun.PreFinishedRoom);
								}
							}
						}
					}
				}
			}
		}
	}
}

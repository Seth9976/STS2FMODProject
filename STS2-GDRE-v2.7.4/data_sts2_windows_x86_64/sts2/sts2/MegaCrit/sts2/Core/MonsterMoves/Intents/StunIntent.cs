using System;
using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace MegaCrit.Sts2.Core.MonsterMoves.Intents
{
	// Token: 0x020004D2 RID: 1234
	[NullableContext(1)]
	[Nullable(0)]
	public class StunIntent : AbstractIntent
	{
		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06004964 RID: 18788 RVA: 0x002260D5 File Offset: 0x002242D5
		public override IntentType IntentType
		{
			get
			{
				return IntentType.Stun;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06004965 RID: 18789 RVA: 0x002260D9 File Offset: 0x002242D9
		protected override string IntentPrefix
		{
			get
			{
				return "STUN";
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06004966 RID: 18790 RVA: 0x002260E0 File Offset: 0x002242E0
		protected override string SpritePath
		{
			get
			{
				return "atlases/intent_atlas.sprites/intent_stun.tres";
			}
		}

		/// <summary>
		/// A special HoverTip override for the StunIntent so that it can be used in a static context
		/// ie: for the Whistle card.
		/// </summary>
		/// <remarks>
		/// The intent sprite is fetched straight from the resident intent atlas rather than through
		/// <see cref="P:MegaCrit.Sts2.Core.Assets.PreloadManager.Cache" />. This tip is stored on the canonical Whistle card model
		/// and re-shown across room transitions (e.g. Tanx's Whistle boss relic preview). Loading it
		/// through the cache registers it as a missed-cache asset, which the next room transition
		/// disposes out from under the stored tip, throwing ObjectDisposedException on hover (PRG-7151).
		/// The intent atlas is loaded at startup and never unloaded, and AtlasManager self-heals
		/// disposed sprite wrappers, so the reference stays valid for the process lifetime.
		/// </remarks>
		// Token: 0x06004967 RID: 18791 RVA: 0x002260E8 File Offset: 0x002242E8
		public static HoverTip GetStaticHoverTip()
		{
			if (!AtlasManager.IsAtlasLoaded("intent_atlas"))
			{
				AtlasManager.LoadAtlas("intent_atlas");
			}
			Texture2D sprite = AtlasManager.GetSprite("intent_atlas", "intent_stun");
			LocString locString = new LocString("intents", "STUN.description");
			return new HoverTip(new LocString("intents", "STUN.title"), locString, sprite);
		}

		// Token: 0x04001CC8 RID: 7368
		private const string _intentPrefix = "STUN";

		// Token: 0x04001CC9 RID: 7369
		private const string _atlasName = "intent_atlas";

		// Token: 0x04001CCA RID: 7370
		private const string _spriteName = "intent_stun";

		// Token: 0x04001CCB RID: 7371
		private const string _spritePath = "atlases/intent_atlas.sprites/intent_stun.tres";
	}
}

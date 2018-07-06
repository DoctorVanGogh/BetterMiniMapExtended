﻿using UnityEngine;
using Verse;

namespace BetterMiniMap
{
    [StaticConstructorOnStartup]
    class MiniMapTextures
    {
        public static readonly Texture2D config;
        public static readonly Texture2D dragA;
        public static readonly Texture2D dragD;
        public static readonly Texture2D homeA;
        public static readonly Texture2D resizeA;
        public static readonly Texture2D resizeD;
		public static readonly Texture2D find;

        static MiniMapTextures()
        {
            MiniMapTextures.config = ContentFinder<Texture2D>.Get("UI/config", true);
            MiniMapTextures.dragA = ContentFinder<Texture2D>.Get("UI/dragA", true);
            MiniMapTextures.dragD = ContentFinder<Texture2D>.Get("UI/dragD", true);
            MiniMapTextures.homeA = ContentFinder<Texture2D>.Get("UI/homeA", true);
            MiniMapTextures.resizeA = ContentFinder<Texture2D>.Get("UI/resizeA", true);
            MiniMapTextures.resizeD = ContentFinder<Texture2D>.Get("UI/resizeD", true);
			MiniMapTextures.find = ContentFinder<Texture2D>.Get("UI/find", true);
		}

    }
}

﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using UAT_MS539.Core.Code;

namespace UAT_MS539.Code
{
    public static class AssetLoadUtility
    {
        private const string CRYPTID_PATH = "/Assets/Cryptids";
        private const string FOOD_PATH = "/Assets/Foods";
        private const string RUNE_PATH = "/Assets/UI/Runes";

        private static string GetRuneName(uint value)
        {
            Debug.Assert(value < NumberEncoder.Base24Encoding.Length);

            var runeCode = NumberEncoder.Base24Encoding[(int) value];

            return $"icon_rune_{runeCode}.png";
        }

        private static string GetCryptidName(uint value)
        {
            Debug.Assert(value < 100);

            return $"cryptid_{value.ToString().PadLeft(2, '0')}.png";
        }

        public static BitmapImage LoadRune(uint value)
        {
            var path = Path.Combine(RUNE_PATH, GetRuneName(value));
            var pathUri = new Uri(path, UriKind.Relative);
            return new BitmapImage(pathUri);
        }

        public static BitmapImage LoadCryptid(uint value)
        {
            var path = Path.Combine(CRYPTID_PATH, GetCryptidName(value));
            var pathUri = new Uri(path, UriKind.Relative);
            return new BitmapImage(pathUri);
        }
    }
}
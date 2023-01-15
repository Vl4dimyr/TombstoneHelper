using UnityEngine;

namespace TombstoneHelper
{
    internal class Assets
    {
        public static Sprite Tombstone { get; private set; }

        public static string English { get; private set; }
        public static string German { get; private set; }

        public static void Init()
        {
            Tombstone = LoadSpriteFromTexture(LoadTextureFromRaw(Resources.Tombstone));

            English = BytesToString(Resources.English);
            German = BytesToString(Resources.German);
        }

        internal static Texture2D LoadTextureFromRaw(byte[] bytes)
        {
            Texture2D texture = new Texture2D(2, 2);

            texture.LoadImage(bytes);

            return texture;
        }

        internal static Sprite LoadSpriteFromTexture(Texture2D SpriteTexture, float PixelsPerUnit = 100f)
        {
            if (!(bool)(Object)SpriteTexture)
            {
                return (Sprite)null;
            }

            return Sprite.Create(
                SpriteTexture,
                new Rect(0.0f, 0.0f, (float)SpriteTexture.width, (float)SpriteTexture.height),
                new Vector2(0.0f, 0.0f),
                PixelsPerUnit
            );
        }

        internal static string BytesToString(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}

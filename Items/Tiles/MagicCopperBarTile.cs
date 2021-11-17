using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Localization;

namespace breadyMod.Items.Tiles
{
    public class MagicCopperBarTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.MagicMetalBar"));
        }

        public override bool Drop(int i, int j)
        {
            Tile t = Main.tile[i, j];
            Item.NewItem(i * 16, j * 16, 16, 16, ModContent.ItemType<Items.InvItems.MagicCopperBar>());
            return base.Drop(i, j);
        }
    }
}
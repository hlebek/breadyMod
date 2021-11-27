using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;

namespace breadyMod.Items.Tiles
{
    public class MagicCatalystTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(200, 200, 0));
            drop = ModContent.ItemType<Items.InvItems.MagicCatalyst>();
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 48, 32, ModContent.ItemType<Items.InvItems.MagicCatalyst>());
        }

        public override bool NewRightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.manaSick = true;
            player.statMana -= 100;
            Item.NewItem(i * 16, j * 16, 16, 16, ModContent.ItemType<Items.InvItems.ManaEssence>());

            return true;
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace breadyMod.Items.Tiles
{
    public class MagicCopperTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            drop = ModContent.ItemType<Items.InvItems.MagicCopperOre>();
            AddMapEntry(new Color(200, 200, 200));
            dustType = DustID.RedTorch;
        }
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace breadyMod.Items.Tiles
{
    public class StarwayBlockTile : ModTile
    {
        int[] arr = { -3, -2, -1, 1, 2, 3 };
        float random;

        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            drop = ModContent.ItemType<Items.InvItems.MagicCopperOre>();
            AddMapEntry(new Color(200, 200, 0));
            dustType = DustID.YellowTorch;
        }

        public override void FloorVisuals(Player player)
        {
            random = Main.rand.NextFloat(0, 100);
            if (random <= 0.1f && player.velocity != new Vector2(0))
            {
                Projectile.NewProjectile(new Vector2(player.position.X, player.position.Y - 500), new Vector2(Main.rand.Next(arr), 10), ModContent.ProjectileType<Items.Projectiles.FakeFallingStar>(), 999, 0, player.whoAmI);
            }
        }
    }
}
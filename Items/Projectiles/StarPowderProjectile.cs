using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.Projectiles
{
    class StarPowderProjectile : ModProjectile // TODO: Warunek sprawdzający czy ktoś na krańcu mapy tego nie rzuca, żeby index poza wymiarami nie był
    {
        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.friendly = true;
            drawOffsetX = -9;
            drawOriginOffsetY = -3;
            projectile.penetrate = -1;
            projectile.timeLeft = 60;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            Vector2 dustPosition = projectile.Center + Main.rand.NextVector2Circular(-30, 31);
            //Vector2 dustPosition = projectile.Center + (Main.rand.NextVector2Unit(projectile.velocity.Normalize() - (float)MathHelper.Pi/2, projectile.velocity.ToRotation() + (float)MathHelper.Pi / 2) * Main.rand.NextFloat(1, 40));
            //Dust dust = Dust.NewDustDirect(dustPosition, 48, 48, 44, projectile.velocity.X, projectile.velocity.Y, 100, Color.Gold, 1f);
            Dust dust = Dust.NewDustPerfect(dustPosition, 44, null, 100, Color.Gold, 1f);
            //dust.velocity *= 0.2f;
            dust.noGravity = true;
            
            projectile.velocity *= 0.75f;  // higher value makes projectile slow slower. 1 for no drag

            /* Calculate offsets from the center of the projectile (projectile.Center divided by 16
             * due to conversion from world coordinates to tile coordinates. Then substract
             * width/height of hitbox divided by it's value. For adding divide only by half of
             * width/height (got to be this way propably due to the fact numbers are rounded to int
             * or because center is misplaced? Dunno for sure.
             */
            int xOffsetLeft = (int)(projectile.Center.X / 16f - ((float)projectile.width / 48f));
            int xOffsetRight = (int)(projectile.Center.X / 16f + ((float)projectile.width / 24f));
            int yOffsetUp = (int)(projectile.Center.Y / 16f - ((float)projectile.height / 48f));
            int yOffsetDown = (int)(projectile.Center.Y / 16f + ((float)projectile.height / 24f));

            for (int i = xOffsetLeft; i < xOffsetRight; i++)
            {
                for (int j = yOffsetUp; j < yOffsetDown; j++)
                {
                    if (Main.tile[i, j].type == TileID.Copper || Main.tile[i, j].type == TileID.Tin)
                    {
                        WorldGen.KillTile(i, j, false, false, true);
                        WorldGen.PlaceTile(i, j, ModContent.TileType<Items.Tiles.MagicCopperTile>(), true, false, -1, 0);
                    }
                }
            }
        }
    }
}

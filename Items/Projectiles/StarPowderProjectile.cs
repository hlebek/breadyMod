using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace breadyMod.Items.Projectiles
{
    class StarPowderProjectile : ModProjectile // TODO: Zamiana bloków, które uderzy w inne bloki
    {
        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.friendly = true;
            drawOffsetX = -9;
            drawOriginOffsetY = -3;
            projectile.penetrate = -1;
            projectile.timeLeft = 180;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            Vector2 dustPosition = projectile.Center + new Vector2(Main.rand.Next(-40, 41), Main.rand.Next(-40, 41));
            Dust dust = Dust.NewDustPerfect(dustPosition, 44, null, 100, Color.Gold, 1f);
            dust.velocity *= 0.2f;
            dust.noGravity = true;

            // Set the rotation so the projectile points towards where it's going.
            if (projectile.velocity != Vector2.Zero)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {


            return false; // return false because we are handling collision
        }
    }
}

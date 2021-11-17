using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;


namespace breadyMod.Items.Projectiles
{
    class SparkProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 6;
            projectile.friendly = true;
            projectile.light = 0.2f;
            projectile.magic = true;
            drawOffsetX = -1;
            drawOriginOffsetY = -1;
            projectile.penetrate = 2;
            projectile.timeLeft = 30;
            projectile.ignoreWater = true;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(200, 0, 30, 0);

        public override void AI()
        {
            if (projectile.ai[0] == 3)
            {
                Main.PlaySound(SoundID.Item25, projectile.position);
            }

            if(projectile.ai[0] % 10 == 0)
            {
                Vector2 dustPosition = projectile.Center + new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, 2));
                Dust dust = Dust.NewDustPerfect(dustPosition, 96, null, 100, Color.Brown, 1f);
                dust.noGravity = true;
            }

            if (projectile.ai[0] > 15)
                projectile.velocity *= 0.3f;

            // Set the rotation so the projectile points towards where it's going.
            if (projectile.velocity != Vector2.Zero)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();

            return false;
        }
    }
}

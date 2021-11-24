using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;


namespace breadyMod.Items.Projectiles
{
    class OrbProjectile : ModProjectile // TODO: Chyba shardy się nie odwracają, gdy pocisk zniszczy się na bloku
    {
        Vector2 oldVelocity;

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.light = 0.8f;
            projectile.magic = true;
            drawOffsetX = -3;
            drawOriginOffsetY = -3;
            projectile.penetrate = 3;
            projectile.timeLeft = 60;
            projectile.ignoreWater = true;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(126, 126, 255, 0);

        public override void AI()
        {
            oldVelocity = projectile.velocity;

            // This part makes the projectile do a shime sound every 20 ticks as long as it is moving.
            if (projectile.soundDelay == 0 && Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) > 2f)
            {
                projectile.soundDelay = 20;
                Main.PlaySound(SoundID.Item20, projectile.position);
            }

            Vector2 dustPosition = projectile.Center + new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
            Dust dust = Dust.NewDustPerfect(dustPosition, 7, null, 100, Color.Lime, 0.8f);
            dust.velocity *= 0.3f;
            dust.noGravity = true;

            // Set the rotation so the projectile points towards where it's going.
            if (projectile.velocity != Vector2.Zero)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // If statement for how many enemies/tiles projectile can hit
            if (projectile.penetrate == 0)
                projectile.Kill();
            projectile.penetrate--;

            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
            }

            return false; // return false because we are handling collision
        }

        public override void Kill(int timeLeft)
        {
            // Makes the projectile hit all enemies as it circunvents the penetrate limit.
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            int explosionArea = 100;
            Vector2 oldSize = projectile.Size;

            // Resize the projectile hitbox to be bigger.
            projectile.position = projectile.Center;
            projectile.Size += new Vector2(explosionArea);
            projectile.Center = projectile.position;

            // Spawn dust particles
            for (int i = 0; i < 80; i++)
            {
                Vector2 dustPosition = projectile.Center + Main.rand.NextVector2Circular(-50, 50);
                Dust dust = Dust.NewDustPerfect(dustPosition, 137, Main.rand.NextVector2Circular(-2.5f, 2.5f), 100, Color.AliceBlue, 1f);
                dust.noGravity = true;
            }

            projectile.tileCollide = false;
            oldVelocity.Normalize();
            projectile.velocity *= 0.01f;

            // Damage enemies inside the hitbox area
            projectile.Damage();
            projectile.scale = 0.01f;

            // Resize the hitbox to its original size
            projectile.position = projectile.Center;
            projectile.Size = oldSize;
            projectile.Center = projectile.position;
            
            Main.PlaySound(SoundID.Item10, projectile.position);

            // Spawn 1-3 child projectiles
            for (int i = 0; i < Main.rand.Next(1, 4); i++)
            {
                Projectile.NewProjectile(projectile.position, (oldVelocity + Main.rand.NextVector2Unit(projectile.rotation - 2, 2))*8f, ModContent.ProjectileType<Projectiles.OrbProjectileChild>(), 14, Main.myPlayer, 0, 0, 0);
            }

        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

// TODO: Projectile is not affecte to manasickness due to fact it's damage is changing inside the code
// CHANGE THAT ^^^

namespace breadyMod.Items.Projectiles
{
    class AquaHeartiaProjectile : ModProjectile
    {
        private const float scaleStep = 0.01f;
        private int oldDamage;
        private int x = 0, y = 0;
        private Vector2 direction;

        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 44;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 3600;
            projectile.ignoreWater = false;
            projectile.alpha = 160;
            projectile.damage = 30;  //normally set to 30
            oldDamage = projectile.damage;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            // While charging the rowning noise is played.
            if (projectile.soundDelay == 0 && projectile.ai[0] == 0f)
            {
                projectile.soundDelay = 50;
                Main.PlaySound(SoundID.Drown, projectile.position);
            }

            // Shot projectile plays water spray sound
            if (projectile.soundDelay == 0 && Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) > 2f && projectile.ai[0] > 0)
            {
                projectile.soundDelay = 30;
                Main.PlaySound(SoundID.Splash, projectile.position);
            }

            // In Multi Player this code only runs on the client of the projectile's owner, this is because it relies on mouse position, which isn't the same across all clients.
            if (Main.myPlayer == projectile.owner && projectile.ai[0] == 0f)
            {
                Player player = Main.player[projectile.owner];

                if (player.channel)
                {
                    projectile.velocity = new Vector2(0);

                    y = x * x / 5000 + 1;
                    projectile.damage = oldDamage * y;
                    projectile.position = player.Center;
                    projectile.position.X -= projectile.width / 2;
                    projectile.position.Y -= 100 + projectile.height / 2;
                    projectile.rotation += MathHelper.PiOver4 / 60 + projectile.scale;

                    direction = Main.MouseWorld - projectile.Center;
                    direction.Normalize();

                    // the damage will increase for 150 frames using square function
                    //(not advised to icrease it much over 150 due to rapid value change above 150th frame)
                    //(unless you change the formula for y). After 150 frames damage of projectiles is 4.5 times bigger
                    if (x < 150)
                    {
                        projectile.scale += scaleStep;
                        x++;
                    }
                }
                else if (projectile.ai[0] == 0f)
                {
                    projectile.netUpdate = true;
                    projectile.tileCollide = true;
                    projectile.velocity = direction * 25f;
                    projectile.rotation += MathHelper.PiOver4 / 60 + projectile.scale;

                    projectile.ai[0] = 1f;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Coins, projectile.position);

            for (int i = 0; i < 30 + x; i++)
            {
                Vector2 dustPosition = projectile.Center + Main.rand.NextVector2Circular(-50, 50);
                Dust dust = Dust.NewDustPerfect(dustPosition, 1, 3* Main.rand.NextVector2Circular(-2.5f, 2.5f), 128, Color.SkyBlue, 0.7f);
                dust.noGravity = false;
            }
        }
    }
}

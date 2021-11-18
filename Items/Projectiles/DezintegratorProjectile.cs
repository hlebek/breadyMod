using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

//TODO: Projectile traktuje każdego tile'a jak blok stały i dlatego jak przeleci przez np. drzewo albo platformę
//to potem przy następnej platformie się zabija... zmienić, żeby sprawdzić czy blok jest solidny czy nie.
//Do tego trzeba sprawić, aby wyświetlała się grafika lasera, ale jak?
//Można zrobić wiązkę lasera tak, że broń wypuszczałaby najpierw projectile, który bije 0 dmg i może przelecieć
//przez jedną ścianę, a po drodze respi za sobą inne pociski, które mają teksturę lasera i się łączą i zadają dmg
//Do tego może dać maksymalny zasięg lasera, żeby za dużo projectili nie respić i nie obciążać pamięci i procesora

namespace breadyMod.Items.Projectiles
{
    class DezintegratorProjectile : ModProjectile
    {
        bool didPierceWall = false;
        bool anotherWall = false;
        int oldPositionX = 0;
        int oldPositionY = 0;
        Vector2 startingPosition;
        Vector2 endingPosition;

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 11;
            projectile.friendly = true;
            projectile.light = 0.6f;
            projectile.magic = true;
            //drawOffsetX = -15;
            //drawOriginOffsetY = -3;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 10;
            projectile.tileCollide = false;
            projectile.damage = 0;
            startingPosition = projectile.position;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(230, 0, 0, 0);

        public override void AI()
        {
            int positionX = (int)(projectile.Center.X / 16f);
            int positionY = (int)(projectile.Center.Y / 16f);

            if (Main.tileSolid[Main.tile[positionX, positionY].type])

            if (projectile.ai[0] % 20 ==0)
            {
                Projectile.NewProjectile(projectile.position, projectile.velocity * 0.0001f, ModContent.ProjectileType<Items.Projectiles.DezintegratorProjectileReal>(), 60, 0, Main.myPlayer, 0, 0);
            }
            //Dust dust = Dust.NewDustPerfect(projectile.Center, ModContent.DustType<Items.Dusts.DezintegratorDust>(), projectile.velocity*0.01f, 0, Color.Red, 1f);

            if (!Main.tileTable[Main.tile[positionX, positionY].type] && Main.tile[positionX, positionY].active() && Main.tileSolid[Main.tile[positionX, positionY].type] && (oldPositionX != positionX && oldPositionY != positionY))   //if projectile hits the tile first time (is in it)
            {
                didPierceWall = true;
                oldPositionX = positionX;
                oldPositionY = positionY;
            }
            if (didPierceWall)                              //if projectile hit the wall
            {
                if (!Main.tileTable[Main.tile[positionX, positionY].type] && !Main.tile[positionX, positionY].active() && Main.tileSolid[Main.tile[positionX, positionY].type] && (oldPositionX != positionX && oldPositionY != positionY))   //if projectile did exit the wall
                    anotherWall = true;
            }

            if ((anotherWall && !Main.tileTable[Main.tile[positionX, positionY].type] && Main.tile[positionX, positionY].active() && Main.tileSolid[Main.tile[positionX, positionY].type]) || projectile.ai[0] >= 60)    //if projectile is hitting a wall 2nd time
            {
                DrawLaser(startingPosition, endingPosition);
                projectile.Kill();
            }
        }

        public void DrawLaser(Vector2 start, Vector2 end)
        {
            float betweenX = end.X - start.X;
            float deltaX = betweenX / 20;
            float betweenY = end.Y - start.Y;
            float deltaY = betweenY / 20;
            float between = Vector2.Distance(end, start);
            for (int i = 0; i < between; i += 20)
            {
                Projectile.NewProjectile(start.X + deltaX, start.Y + deltaY, projectile.velocity.X * 0.0001f, projectile.velocity.Y * 0.0001f, ModContent.ProjectileType<Items.Projectiles.DezintegratorProjectileReal>(), 60, 0, Main.myPlayer, 0, 0);
            }
        }
    }
}

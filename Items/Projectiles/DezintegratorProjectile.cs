using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

//Jak narazie jest ciekawie. Jedyne czego brakuje to wyliczania punkta końcowego (albo dwóch punktów, bo wiązka
//ma szerokość, także wyznaczałbym 2 punkty końcowe dla obu krawędzi najlepiej). Po dodaniu wyliczenia punkta
//końcowego lasera będzie trzeba zmienić rysowanie spriteBatchy - cały na raz ma się rysować, a nie dorysowywać
//w raz z pokonaną odległością projectile'a (dodanie punkta końcowego rozwiąże ten problem). Końcówka ma się
// rysować (a jakże by inaczej) na końcu.
//Możliwe, że spriteBatche będą o wiele za szybko znikać gdy już broń będzie skończona. Aby uniknąć efektów
//stroboskopowych czy wiązki lasera pojawiającej się na 2 klatki, można dodać niewidzialnego projectile'a, który
//będzie miał podpiętego pod siebie spriteBatcha i będzie żył np. przez 3 sekundy, a następnie umierał.

namespace breadyMod.Items.Projectiles
{
    class DezintegratorProjectile : ModProjectile
    {
        private Vector2 laserEndPoint;
        private Vector2 laserStartPoint;
        private Vector2 predictedPosition;
        private float laserDrawStep = 500f;
        private bool foundCollisionPoint = false;

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.light = 0.6f;
            projectile.magic = true;
            //drawOffsetX = -15;
            //drawOriginOffsetY = -3;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.timeLeft = 90;
            projectile.ignoreWater = true;
            //projectile.tileCollide = false;
            projectile.extraUpdates = 10;
        }

        public override void PostAI()
        {
            float k = projectile.Center.X / 16f;
            float j = projectile.Center.Y / 16f;
            int helper = 0;
            while ((!((Main.tileSolid[Main.tile[(int)k, (int)j].type] && Main.tile[(int)k, (int)j].active()) || foundCollisionPoint) || Main.tile[(int)k, (int)j].type == 19) && helper < 90)
            {
                k += projectile.velocity.X / 16f;
                j += projectile.velocity.Y / 16f;
                predictedPosition.X = k * 16f;
                predictedPosition.Y = j * 16f;
                helper++;
                // delete spawn of the new projectile - it's for tests only
                //Projectile.NewProjectile(predictedPosition, projectile.velocity*0, ModContent.ProjectileType<Projectiles.OrbProjectileChild>(), 14, Main.myPlayer, 0, 0, 0);
            }
            if (Main.tileSolid[Main.tile[(int)k, (int)j].type])
                foundCollisionPoint = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 toEndPointVector = laserEndPoint - Main.player[projectile.owner].Center;
            toEndPointVector.Normalize();
            DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center, toEndPointVector, 10, projectile.damage, -1.57f, 1f, 1000f, Color.White, 75);// this is the projectile sprite draw, 45 = the distance of where the projectile starts from the player

            return false;
        }

        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 toEndPointVector, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int projStartDistFromThePlayer = 50)
        {
            Vector2 origin = start;
            Vector2 tailOffset = toEndPointVector * 10; ;
            float r = toEndPointVector.ToRotation() + rotation;
            int distance = (int)Vector2.Distance(start, predictedPosition);

            //laserStartPoint = projectile.position;






            #region Draw laser body
            for (float i = projStartDistFromThePlayer; i <= projStartDistFromThePlayer + distance - step; i += step)
            {
                Color c = Color.White;
                origin = start + tailOffset + i * toEndPointVector;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                new Rectangle(0, 0, 30, 30), c, r,
                new Vector2(28 / 2, 26 / 2), scale, 0, 0);
            }
            #endregion

            #region Draw laser tail
            spriteBatch.Draw(texture, start + tailOffset - Main.screenPosition,
            new Rectangle(0, 63, 30, 30), Color.White, r+(float)Math.PI,
            new Vector2(28 / 2, 26 / 2), scale, 0, 0);
            #endregion

            #region Draw laser head
            spriteBatch.Draw(texture, predictedPosition + (laserDrawStep + step) * toEndPointVector - Main.screenPosition,//predictedPosition + tailOffset - Main.screenPosition,
            new Rectangle(0, 32, 30, 30), Color.White, r, new Vector2(28 / 2, 26 / 2), scale, 0, 0);
            #endregion


            //#region Draw laser tail
            //spriteBatch.Draw(texture, start + tailOffset - Main.screenPosition,
            //new Rectangle(0, 63, 30, 30), Color.White, r + (float)Math.PI,
            //new Vector2(28 / 2, 26 / 2), scale, 0, 0);
            //#endregion

            //#region Draw laser head
            //spriteBatch.Draw(texture, start + (laserDrawStep + step) * toEndPointVector - Main.screenPosition,
            //new Rectangle(0, 32, 30, 30), Color.White, r, new Vector2(28 / 2, 26 / 2), scale, 0, 0);
            //#endregion
        }

        public override void AI()
        {
            //laserCurrPoint = projectile.position;

            //TUTAJ CHYBA JEDNA PĘTLA STARCZY, BO TAK ITERUJEMY SIĘ W DZIWNY SPOSÓB PO MIEJSCACH, KTÓRE
            //NIE NALEŻĄ DO ŚCIEŻKI NASZEGO POCISKU
            // We iterate through each point our projectile will be in. We start with i&j being equal to projectile
            // coordinates. Then in our for loop(s) we check if our i&j arent bigger then last position they ever get
            // which is equal to projectile starting position + 90 frames of projectile's life multiplied by projectile
            // velocity. We only need to do it once in projectile's life time

            // Delete if statement - used for tests only
            if (!Main.tileSolid[Main.tile[(int)((projectile.Center.X / 16f)+(projectile.velocity.X/16f)*90), (int)((projectile.Center.Y / 16f) + (projectile.velocity.Y/16f) * 90)].type])
            {
                //Projectile.NewProjectile(projectile.position, (projectile.velocity + Main.rand.NextVector2Unit(projectile.rotation - 2, 2)) * 8f, ModContent.ProjectileType<Projectiles.OrbProjectileChild>(), 14, Main.myPlayer, 0, 0, 0);
            }

            //float i = projectile.Center.X / 16f;
            //float j = projectile.Center.Y / 16f;
            //int helper = 0;

            
            // Check if tile doen't exist. If it exist in given i&j then check if it is solid. Also check helper bool state so this loop
            // executes only once in projectiles lifetime. In loop condition there's also helper variable "helper" used to determine maximum
            // length of the laser beam (it is used cuz otherwise if we shoot into the air the game will check every tile up to the world boundry
            // which is waste of compute resources)
            //while ((!((Main.tileSolid[Main.tile[(int)i, (int)j].type] && Main.tile[(int)i, (int)j].active()) || foundCollisionPoint) || Main.tile[(int)i, (int)j].type == TileID.Platforms) && helper < 90)
            //{
            //    i += projectile.velocity.X/16f;
            //    j += projectile.velocity.Y/16f;
            //    predictedPosition.X = i * 16f;
            //    predictedPosition.Y = j * 16f;
            //    helper++;
            //    // delete spawn of the new projectile - it's for tests only
            //    //Projectile.NewProjectile(predictedPosition, projectile.velocity*0, ModContent.ProjectileType<Projectiles.OrbProjectileChild>(), 14, Main.myPlayer, 0, 0, 0);
            ////}
            //if (Main.tileSolid[Main.tile[(int)i, (int)j].type])
            //    foundCollisionPoint = true;

            //Projectile.NewProjectile(predictedPosition, (projectile.velocity + Main.rand.NextVector2Unit(projectile.rotation - 2, 2)) * 8f, ModContent.ProjectileType<Projectiles.OrbProjectileChild>(), 14, Main.myPlayer, 0, 0, 0);

            //for (float i = projectile.position.X; i < projectile.position.X + (90 * projectile.velocity.X); i += projectile.velocity.X)    
            //{
            //    Vector2 predictedPosition = projectile.position;
            //    for (float j = projectile.position.Y; j < projectile.position.Y + (90 * projectile.velocity.Y); j += projectile.velocity.Y)
            //    {
            //        if (Main.tileSolid[Main.tile[(int)i, (int)j].type])

            //    }
            //}

            Vector2 step = Main.player[projectile.owner].Center - Main.MouseWorld;
            step.Normalize();
            step *= -1;
            laserEndPoint = Main.player[projectile.owner].Center + step * laserDrawStep;

            //laserEndPoint.X = i;
            //laserEndPoint.Y = j;


        }
    }
}

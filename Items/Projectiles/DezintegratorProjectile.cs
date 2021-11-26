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
//rysować (a jakże by inaczej) na końcu.
//Możliwe, że spriteBatche będą o wiele za szybko znikać gdy już broń będzie skończona. Aby uniknąć efektów
//stroboskopowych czy wiązki lasera pojawiającej się na 2 klatki, można dodać niewidzialnego projectile'a, który
//będzie miał podpiętego pod siebie spriteBatcha i będzie żył np. przez 3 sekundy, a następnie umierał.
//The code can be edited using - lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
//to save resources and increase accuracy and quality. Now Im tired with it and it works fine so I'll it for now as it is.
//using Collision.CanHitLine we do not have to iterate manually through every step + it takes into accound width and height
//of the projectile while present method does not and can cause a bug that shooting at blocks edge with 45 degree angle or shooting
//at crack between two block that almost connected by the eges (imagine them forming a matrix like 1st row -[1, 0], 2nd row -[0, 1])
//the sprite renders with maximum length while projectile is killed somewhere in the middle of the sprite
//or sprite does not render at all but projectile spawns behind the crack (if you stand right next to the crack and aim at it)


namespace breadyMod.Items.Projectiles
{
    class DezintegratorProjectile : ModProjectile
    {
        private Vector2 laserEndPoint;
        //private Vector2 laserStartPoint;
        private Vector2 predictedPosition;
        private float laserDrawStep = 500f; // it's not really a DrawStep. this variable is used once so it may be wise to just delete it and hard code the value
        private float dustDistance = 0;
        private bool foundCollisionPoint = false;
        private bool setup = false;
        int helper = 0; // This propably could be replaced in code with projectile.timeleft

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.friendly = true;
            projectile.light = 0.6f;
            projectile.magic = true;
            //drawOffsetX = -15;
            //drawOriginOffsetY = -3;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.timeLeft = 30;
            projectile.ignoreWater = true;
            //projectile.tileCollide = false;
            projectile.extraUpdates = 4;
        }

        public override void PostAI()
        {
            if (!foundCollisionPoint)
            {
                float k, j;
                k = projectile.Center.X / 16f;
                j = projectile.Center.Y / 16f;
                Tile tile = Main.tile[(int)k, (int)j];
                while (!((tile.type != 19 && tile.active() && Main.tileSolid[tile.type]) || foundCollisionPoint) && helper < 30)
                {
                    k += projectile.velocity.X / 16f;    // divide by 16f by default. Lower value gives worse resolution and might cause bugs but is faster
                    j += projectile.velocity.Y / 16f;    // higher values give better resolution (but higher than 16f is basically pointless) but is more
                    predictedPosition.X = k * 16f;       // time and resource consuming and may be laggy (and that might also cause bugs)
                    predictedPosition.Y = j * 16f;
                    helper++;
                    tile = Main.tile[(int)k, (int)j];
                    // delete spawn of the new projectile - it's for tests only
                    //Projectile.NewProjectile(predictedPosition, projectile.velocity * 0, ModContent.ProjectileType<Projectiles.OrbProjectileChild>(), 14, Main.myPlayer, 0, 0, 0);
                }
                //if ((Main.tile[(int)k, (int)j].type != 19 && Main.tile[(int)k, (int)j].active()) || helper >= 90)
                foundCollisionPoint = true;
                //Projectile.NewProjectile(predictedPosition, projectile.velocity * 0, ModContent.ProjectileType<Projectiles.OrbProjectile>(), 14, Main.myPlayer, 0, 0, 0);

            }

            dustDistance = Vector2.Distance(Main.player[projectile.owner].Center, predictedPosition);
            int i = 0;

            while (i < dustDistance)
            {
                Vector2 direction = projectile.velocity;
                direction.Normalize();
                if (i < 80 && i % 25 == 0) // TailDust
                {
                    Vector2 dustPosition = Main.player[projectile.owner].Center + direction * 70 + Main.rand.NextVector2Circular(-30, 30);
                    Dust dust = Dust.NewDustPerfect(dustPosition, ModContent.DustType<Items.Dusts.DezintegratorDustTail>(), projectile.velocity * 0.09f + Main.rand.NextVector2Circular(0.5f, 0.5f), 160, default(Color), 1f);
                }
                else if (i >= 120 && i < dustDistance - 40 && i % 30 == 0) // BodyDust
                {
                    int[] arr = { -1, 1 };
                    int plusMinus = Main.rand.Next(arr);
                    Vector2 dustPosition = Main.player[projectile.owner].Center + direction * i + Main.rand.NextVector2Circular(-15, 15);
                    Dust dust = Dust.NewDustPerfect(dustPosition, ModContent.DustType<Items.Dusts.DezintegratorDustBody>(), projectile.velocity * 0.01f + Main.rand.NextVector2Unit(projectile.velocity.ToRotation() + MathHelper.PiOver4*plusMinus, MathHelper.PiOver2), 160, default(Color), 1f);
                }
                else if (i >= dustDistance - 40 && i < dustDistance && i % 25 == 0) // HeadDust
                {
                    int inverse;
                    int timeLeft = projectile.timeLeft;
                    Vector2 dustPosition = Main.player[projectile.owner].Center + direction * i + Main.rand.NextVector2Circular(-15, 15);

                    if (dustDistance > timeLeft * projectile.velocity.Length() - 10)
                    {
                        inverse = 1;
                        Dust dust = Dust.NewDustPerfect(dustPosition, ModContent.DustType<Items.Dusts.DezintegratorDustBody>(), inverse * projectile.velocity * 0.2f + inverse * 3 * Main.rand.NextVector2Unit(projectile.velocity.ToRotation() - MathHelper.PiOver4, MathHelper.PiOver2), 160, default(Color), 1f);
                    }
                    else
                    {
                        inverse = -1;
                        Dust dust = Dust.NewDustPerfect(dustPosition, ModContent.DustType<Items.Dusts.DezintegratorDustBody>(), inverse * projectile.velocity * 0.2f + inverse * 3 * Main.rand.NextVector2Unit(projectile.velocity.ToRotation() - MathHelper.PiOver4, MathHelper.PiOver2), 160, default(Color), 1f);
                    }
                }

                i++;
            }

            //int distance = 0;
            //distance = (int)Vector2.Distance(dustOrigin, predictedPosition);

            //if (!setup)
            //{
            //    dustOrigin = Main.player[projectile.owner].Center;
            //}
            ////laserCurrPoint = projectile.position;

            //for (float i = 95; i <= 20 + distance - 50; i += 5)
            //{
            //    Vector2 dustPosition = dustOrigin - Main.screenPosition + Main.rand.NextVector2Circular(-50, 50);
            //    Dust dust = Dust.NewDustPerfect(dustPosition, ModContent.DustType<Items.Dusts.DezintegratorDustBody>(), Main.rand.NextVector2Circular(-2.5f, 2.5f), 100, Color.AliceBlue, 1f);
            //}
            //if (!foundCollisionPoint)
            //{
            //    float k, j;
            //    k = projectile.Center.X / 16f;
            //    j = projectile.Center.Y / 16f;
            //    Tile tile = Main.tile[(int)k, (int)j];
            //    while (!((Main.tile[(int)k, (int)j].type != 19 && Main.tile[(int)k, (int)j].active()) || foundCollisionPoint) && helper < 90)
            //    {
            //        k += projectile.velocity.X / 16f;    // divide by 16f by default. Lower value gives worse resolution and might cause bugs but is faster
            //        j += projectile.velocity.Y / 16f;    // higher values give better resolution (but higher than 16f is basically pointless) but is more
            //        predictedPosition.X = k * 16f;      // time and resource consuming and may be laggy (and that might also cause bugs)
            //        predictedPosition.Y = j * 16f;
            //        helper++;
            //        // delete spawn of the new projectile - it's for tests only
            //        Projectile.NewProjectile(predictedPosition, projectile.velocity*0, ModContent.ProjectileType<Projectiles.OrbProjectileChild>(), 14, Main.myPlayer, 0, 0, 0);
            //    }
            //    //if ((Main.tile[(int)k, (int)j].type != 19 && Main.tile[(int)k, (int)j].active()) || helper >= 90)
            //        foundCollisionPoint = true;
            //    Projectile.NewProjectile(predictedPosition, projectile.velocity * 0, ModContent.ProjectileType<Projectiles.OrbProjectile>(), 14, Main.myPlayer, 0, 0, 0);

            //}
            //if (!(!((Main.tile[(int)k, (int)j].type != 19 && Main.tile[(int)k, (int)j].active()) || foundCollisionPoint) && helper < 90))
            //foundCollisionPoint = true;

            //while ((!((Main.tileSolid[Main.tile[(int)k, (int)j].type] && Main.tile[(int)k, (int)j].active()) || foundCollisionPoint) || Main.tile[(int)k, (int)j].type == 19) && helper < 90)
            //{
            //    k += projectile.velocity.X / 16f;
            //    j += projectile.velocity.Y / 16f;
            //    predictedPosition.X = k * 16f;
            //    predictedPosition.Y = j * 16f;
            //    helper++;
            //    // delete spawn of the new projectile - it's for tests only
            //    //Projectile.NewProjectile(predictedPosition, projectile.velocity*0, ModContent.ProjectileType<Projectiles.OrbProjectileChild>(), 14, Main.myPlayer, 0, 0, 0);
            //}
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 toEndPointVector = predictedPosition - Main.player[projectile.owner].Center;
            toEndPointVector.Normalize();
            DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center, toEndPointVector, 5, projectile.damage, -1.57f, 1f, 1000f, Color.White, 65);// this is the projectile sprite draw, 45 = the distance of where the projectile starts from the player

            return false;
        }

        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 toEndPointVector, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int projStartDistFromThePlayer = 50)
        {
            Vector2 origin = start;
            Vector2 tailOffset = toEndPointVector * 70;
            float r = toEndPointVector.ToRotation() + rotation;
            int distance = (int)Vector2.Distance(start, predictedPosition);

            //laserStartPoint = projectile.position;



            //WSZYSTKO GIT, DODAĆ TYLKO, ŻEBY WIAZKA PRZEZ SEKUNDĘ POZOSTAWAŁA W MIEJSCU WYSTRZAŁU (dodać np. zmienną globalną do skalowania, która się zmienia w kodzie AI), ZMIENIĆ TEKSTURY I DODAĆ DUSTY NA POCZĄTKU I KOŃCU WIĄZKI

            #region Draw laser body
            for (float i = projStartDistFromThePlayer + 30; i <= projStartDistFromThePlayer - 40 + distance - step * 10; i += step)
            {
                Color c = Color.White;
                origin = start + i * toEndPointVector;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                new Rectangle(0, 0, 30, 30), c, r,
                new Vector2(28 / 2, 26 / 2), scale, 0, 0);

            }
            #endregion

            #region Draw laser tail
            spriteBatch.Draw(texture, start + tailOffset - Main.screenPosition,
            new Rectangle(0, 63, 30, 30), Color.White, r,
            new Vector2(28 / 2, 26 / 2), scale, 0, 0);

            #endregion

            #region Draw laser head
            spriteBatch.Draw(texture, predictedPosition + step * toEndPointVector / 5 - toEndPointVector * 5 - Main.screenPosition,//predictedPosition + tailOffset - Main.screenPosition,
            new Rectangle(0, 32, 30, 29), Color.White, r, new Vector2(28 / 2, 26 / 2), scale, 0, 0);

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
        //    int distance = 0;
        //    distance = (int)Vector2.Distance(dustOrigin, predictedPosition);

        //    if (!setup)
        //    {
        //        dustOrigin = Main.player[projectile.owner].Center;
        //    }
        //    //laserCurrPoint = projectile.position;

        //    for (float i = 95; i <= 20 + distance - 50; i += 5)
        //    {
        //        Vector2 dustPosition = dustOrigin - Main.screenPosition + Main.rand.NextVector2Circular(-50, 50);
        //        Dust dust = Dust.NewDustPerfect(dustPosition, ModContent.DustType<Items.Dusts.DezintegratorDustBody>(), Main.rand.NextVector2Circular(-2.5f, 2.5f), 100, Color.AliceBlue, 1f);
        //    }

            //for (int l = 0; l < 6; l++)
            //{
            //    Vector2 dustPosition = dustOrigin - Main.screenPosition + Main.rand.NextVector2Circular(-50, 50);
            //    Dust dust = Dust.NewDustPerfect(dustPosition, ModContent.DustType<Items.Dusts.DezintegratorDustBody>(), Main.rand.NextVector2Circular(-2.5f, 2.5f), 100, Color.AliceBlue, 1f);
            //}
            //TUTAJ CHYBA JEDNA PĘTLA STARCZY, BO TAK ITERUJEMY SIĘ W DZIWNY SPOSÓB PO MIEJSCACH, KTÓRE
            //NIE NALEŻĄ DO ŚCIEŻKI NASZEGO POCISKU
            // We iterate through each point our projectile will be in. We start with i&j being equal to projectile
            // coordinates. Then in our for loop(s) we check if our i&j arent bigger then last position they ever get
            // which is equal to projectile starting position + 90 frames of projectile's life multiplied by projectile
            // velocity. We only need to do it once in projectile's life time

            //for (int i = 0; i < 6; i++)
            //{
            //    Vector2 dustPosition = start + tailOffset - Main.screenPosition + Main.rand.NextVector2Circular(-50, 50);
            //    Dust dust = Dust.NewDustPerfect(dustPosition, ModContent.DustType<Items.Dusts.DezintegratorDustBody>(), Main.rand.NextVector2Circular(-2.5f, 2.5f), 100, Color.AliceBlue, 1f);
            //}
            // Delete if statement - used for tests only
            //if (!Main.tileSolid[Main.tile[(int)((projectile.Center.X / 16f)+(projectile.velocity.X/16f)*90), (int)((projectile.Center.Y / 16f) + (projectile.velocity.Y/16f) * 90)].type])
            //{
            //    //Projectile.NewProjectile(projectile.position, (projectile.velocity + Main.rand.NextVector2Unit(projectile.rotation - 2, 2)) * 8f, ModContent.ProjectileType<Projectiles.OrbProjectileChild>(), 14, Main.myPlayer, 0, 0, 0);
            //}

            //for (int i = 0; i < 6; i++)
            //{
            //    Vector2 dustPosition = predictedPosition + Main.rand.NextVector2Circular(-50, 50);
            //    Dust dust = Dust.NewDustPerfect(dustPosition, ModContent.DustType<Items.Dusts.DezintegratorDustBody>(), Main.rand.NextVector2Circular(-2.5f, 2.5f), 100, Color.AliceBlue, 1f);
            //}
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

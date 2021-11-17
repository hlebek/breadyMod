using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.MagicWeapons
{
    class ManaWideShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Shots a powerful mana manifestations in wide arc in front of you." +
                                "\nGood at close distance.");

            DisplayName.AddTranslation(GameCulture.Polish, "Szerokostrzał Many");
            Tooltip.AddTranslation(GameCulture.Polish, "Strzela przed Tobą POTĘŻNĄ MANIFESTACJĄ MANY pod szerokim kątem." +
                                                        "\nDobre na krótki dystans.");
        }

        public override void SetDefaults()
        {
            item.damage = 52;
            item.magic = true;
            item.mana = 8;
            item.width = 40;
            item.height = 40;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 7f;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item34;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Projectiles.SparkProjectile>();
            item.shootSpeed = 16f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 5 + Main.rand.Next(3); // 5, 6, 7
			for (int i = 0; i < numberProjectiles; i++)
			{
                Vector2 perturbedSpeed;
                if (i < 4)
                {
                    perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30)); // 30 degree spread for first 4 shots
                }
                else
                {
                    perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(70)); // 70 degree spread for last shots.
                }
                // If you want to randomize the speed to stagger the projectiles
                float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale; 
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false; // return false because we don't want tmodloader to shoot projectile
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.InvItems.MagicCopperBar>(), 10);
            recipe.AddIngredient(ItemID.Shotgun, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

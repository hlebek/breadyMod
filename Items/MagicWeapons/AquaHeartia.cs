using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace breadyMod.Items.MagicWeapons
{
    class AquaHeartia : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Arrogant Water Dragon King");

            DisplayName.AddTranslation(GameCulture.Polish, "Aqua Heartia");
            Tooltip.AddTranslation(GameCulture.Polish, "Arogancki Smoczy Król Wody");
        }

        public override void SetDefaults()
        {
            item.damage = 30;
            item.magic = true;
            item.mana = 8;
            item.width = 40;
            item.height = 40;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.channel = true;
            item.knockBack = 4f;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Expert;
            item.UseSound = SoundID.Item21;
            item.shoot = ModContent.ProjectileType<Items.Projectiles.AquaHeartiaProjectile>();
            item.shootSpeed = 20f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.InvItems.MagicCopperBar>(), 2);
            recipe.AddIngredient(ItemID.Sapphire, 5);
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

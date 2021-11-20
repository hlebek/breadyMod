using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace breadyMod.Items.MagicWeapons
{
    class Dezintegrator : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Shots a precise laser that's powerful enough to pierce through a thick wall." +
                                "\nUses compressed mana as an ammo");

            DisplayName.AddTranslation(GameCulture.Polish, "Dezintegrator");
            Tooltip.AddTranslation(GameCulture.Polish, "Strzela precyzyjnie laserem, który jest tak potężny, że rozjebie nawet kogoś za grubą ścianą." +
                                                        "\nUżywa skompresowanej many jako amunicji.");
        }

        public override void SetDefaults()
        {
            item.damage = 62;
            item.magic = true;
            item.mana = 0;
            item.width = 40;
            item.height = 40;
            item.useTime = 4;
            item.useAnimation = 4;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 4f;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item12;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Items.Projectiles.DezintegratorProjectile>();
            item.shootSpeed = 10f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.InvItems.MagicCopperBar>(), 10);
            recipe.AddIngredient(ItemID.SniperRifle, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.InvItems.Ammo
{
    public class CompressedManaAmmo : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Ammunition used in some magic weapons");

            DisplayName.AddTranslation(GameCulture.Polish, "Amunicja ze Skompresowanej Many");
            Tooltip.AddTranslation(GameCulture.Polish, "Amunicja używana w niektórych magicznych broniach");
        }

        public override void SetDefaults()
        {
            item.damage = 1;
            item.magic = true;
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.consumable = true;
            item.knockBack = 1f;
            item.value = Item.sellPrice(0, 0, 1, 0);
            item.rare = ItemRarityID.Yellow;
            item.shoot = ModContent.ProjectileType<Items.Projectiles.DezintegratorProjectile>();
            item.ammo = item.type; // The first item in an ammo class sets the AmmoID to it's type
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CompressedMana>());
            recipe.AddIngredient(ItemID.SilverBullet, 50);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
}
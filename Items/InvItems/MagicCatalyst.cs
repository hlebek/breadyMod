using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.InvItems
{
    class MagicCatalyst : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Infuses blood with mana to get mana essence." +
                                "\nNo worries. Noone's gonna cut it from your car.");

            DisplayName.AddTranslation(GameCulture.Polish, "Magiczny Katalizator");
            Tooltip.AddTranslation(GameCulture.Polish, "Natchniewa krew maną, aby stworzyć esencję many" +
                                                        "\nBez obaw. Nikt Ci go nie wytnie z auta.");
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 1;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.consumable = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.createTile = ModContent.TileType<Items.Tiles.MagicCatalystTile>();
            item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Furnace, 1);
            recipe.AddIngredient(ItemID.ManaCrystal, 5);
            recipe.AddIngredient(ItemID.Sapphire, 5);
            recipe.AddIngredient(ItemID.Ruby, 3);
            recipe.AddIngredient(ItemID.Diamond, 1);
            recipe.AddIngredient(ModContent.ItemType<Items.InvItems.MagicCopperBar>(), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
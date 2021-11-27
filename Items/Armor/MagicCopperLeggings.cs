using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;

namespace breadyMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class MagicCopperLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Glossy!" +
                                "\nSlightly increases movement speed.");

            DisplayName.AddTranslation(GameCulture.Polish, "Nogawice z Magicznej Miedzi");
            Tooltip.AddTranslation(GameCulture.Polish, "Lśniący!" +
                                                        "\nNieznacznie zwiększają prękość poruszania się.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.InvItems.MagicCopperBar>(), 25);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;

namespace breadyMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class MagicCopperHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Glowy!" +
                                "\n+20 mana");

            DisplayName.AddTranslation(GameCulture.Polish, "Hełm z Magicznej Miedzi");
            Tooltip.AddTranslation(GameCulture.Polish, "Świecący!" +
                                                        "\n+20 many");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 20;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<MagicCopperBreastplate>() && legs.type == ModContent.ItemType<MagicCopperLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "You feel power during night!" +
                                "\n+15% magic damage during night" +
                                "\n+40 mana during night";
            
            if (!Main.dayTime)
            {
                player.magicDamage += 0.15f;
                player.statManaMax2 += 40;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.InvItems.MagicCopperBar>(), 20);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
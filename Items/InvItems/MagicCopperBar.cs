using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace breadyMod.Items.InvItems
{
    class MagicCopperBar : ModItem  // Dodać ceny (NIE TYLKO TUTAJ! WSZĘDZIE)
    {
        int randomizer = 0;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("This is bar of magic copper!");

            DisplayName.AddTranslation(GameCulture.Polish, "Sztabka Magicznej Miedzi");
            Tooltip.AddTranslation(GameCulture.Polish, "Toż to sztabka magicznej miedzi!");

            ItemID.Sets.SortingPriorityMaterials[item.type] = 56;
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.consumable = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.createTile = ModContent.TileType<Items.Tiles.MagicCopperBarTile>();
            item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType <MagicCopperOre>(), 3);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override void PostUpdate()
        {
            randomizer = Main.rand.Next(0, 1001);
            if(randomizer > 950)
            {
                Dust dust = Dust.NewDustPerfect(item.position + new Vector2(10, 10) + Main.rand.NextVector2Circular(16, 8), 133, null, 100, Color.Gold, 1f);
                dust.noGravity = true;
                dust.velocity *= 0;
            }
        }
    }
}

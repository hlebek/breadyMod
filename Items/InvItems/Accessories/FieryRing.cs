using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.InvItems.Accessories
{
    public class FieryRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Burns enemies around you");

            DisplayName.AddTranslation(GameCulture.Polish, "Ognisty Pierścień");
            Tooltip.AddTranslation(GameCulture.Polish, "Podpala przeciwników dookoła Ciebie");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.value = Item.sellPrice(gold: 1);
            item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Sets enemies on fire

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                float between = Vector2.Distance(npc.Center, player.Center);
                if (between < 16 * 4 * 2.5f && npc.CanBeChasedBy())
                {
                    npc.AddBuff(BuffID.OnFire, 1, false);
                }
            }
        }

        public override void AddRecipes()
        {
            // Delete recipe. It is supposed to be found in surface chests (maybe underground chests too)
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock, 2);
            recipe.AddIngredient(ItemID.Shackle, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
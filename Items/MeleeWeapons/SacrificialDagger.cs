using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace breadyMod.Items.MeleeWeapons
{
    public class SacrificialDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used on pure creatures for dark rituals");

            DisplayName.AddTranslation(GameCulture.Polish, "Sztylet Ofiarny");
            Tooltip.AddTranslation(GameCulture.Polish, "Używany na czystych stworzeniach na potrzeby czarnych rytuałów");
        }

        public override void SetDefaults()
        {
            item.damage = 5;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 10;
            item.useAnimation = 10;
            item.knockBack = 3;
            item.value = Item.buyPrice(gold: 1);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 10;

            //The useStyle of the item. 
            //Use useStyle 1 for normal swinging or for throwing
            //use useStyle 2 for an item that drinks such as a potion,
            //use useStyle 3 to make the sword act like a shortsword
            //use useStyle 4 for use like a life crystal,
            //and use useStyle 5 for staffs or guns
            item.useStyle = ItemUseStyleID.Stabbing; // 1 is the useStyle
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 5);
            recipe.AddIngredient(ItemID.Wood, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LeadBar, 5);
            recipe.AddIngredient(ItemID.Wood, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 109, 0, 0, 0, default, 1.5f);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (!target.SpawnedFromStatue && target.catchItem > 0)
            {
                Item.NewItem(target.position, 16, 16, ModContent.ItemType<Items.InvItems.Blood>());
            }
        }
    }
}
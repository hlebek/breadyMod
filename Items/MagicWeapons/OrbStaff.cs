using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

//Pociski niech skręcają delikatnie do celu, a nie ostro - trudne do ogarnięcia, może nie na teraz
//W pliku z ExampleLastPrism może być kod potrzebny do gładkiego namierzania, ponieważ last prism
//też się gładko namierza w stronę kursora
//
//Namierzanie można też zrobić tak, że zamiast zmieniać wektor prędkości, aby wskazywał od razu na przeciwnika
//to niech wektor prędkości pozostanie taki jaki jest, tylko niech projectile się obraca w stronę przeciwnika
//Zmienić recepturę

namespace breadyMod.Items.MagicWeapons
{
    class OrbStaff : ModItem
    {
        int freeShot = 1;

        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Shoots and Orb that upon 3rd impact shatters into small pieces that seek & penetrate enemies." +
                                "\nEvery fifth shot costs 0 mana.");

            DisplayName.AddTranslation(GameCulture.Polish, "Kostur Sferyczny");
            Tooltip.AddTranslation(GameCulture.Polish, "Strzela sferą, która po trzecim uderzeniu rozpada się na małe kawałki, które namierzają i penetrują przeciwników." +
                                                        "\nKażdy piąty strzał kosztuje 0 many.");
        }

        public override void SetDefaults()
        {
            item.damage = 34;
            item.magic = true;
            item.mana = 5;
            item.width = 40;
            item.height = 40;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 4f;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Green;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Wooo");
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Projectiles.OrbProjectile>();
            item.shootSpeed = 8f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (freeShot == 5)
                freeShot = 1;
            else
                freeShot++;
            return true;
        }

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (freeShot == 5)
                mult *= 0;
            else
                mult = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.InvItems.MagicCopperBar>(), 10);
            recipe.AddIngredient(ItemID.WandofSparking, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

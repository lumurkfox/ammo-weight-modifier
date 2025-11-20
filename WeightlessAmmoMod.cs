using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Services;
using System.Text.Json;

namespace AmmoWeightModifier;

[Injectable(InjectionType.Singleton, TypePriority = OnLoadOrder.PostSptModLoader + 1)]
public class WeightlessAmmoMod(DatabaseService databaseService) : IOnLoad
{
    // Parent IDs for different types of ammo
    private static readonly string[] AMMO_PARENT_IDS = new[]
    {
        "5485a8684bdc2da71d8b4567", // Ammo
        "543be5cb4bdc2deb348b4568", // Ammo box
    };

    public Task OnLoad()
    {
        try
        {
            // Load config
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user", "mods", "AmmoWeightModifier", "config", "config.json");
            var config = LoadConfig(configPath);

            // Get database tables
            var tables = databaseService.GetTables();
            dynamic items = tables.Templates.Items;

            int modifiedCount = 0;

            // Get all keys from the dictionary
            var keys = new List<string>();
            foreach (var key in items.Keys)
            {
                keys.Add(key);
            }

            // Loop through all items
            foreach (var itemId in keys)
            {
                dynamic item = items[itemId];

                // Check if this item is ammo
                bool isAmmo = false;
                foreach (var ammoParentId in AMMO_PARENT_IDS)
                {
                    if (item.Parent == ammoParentId)
                    {
                        isAmmo = true;
                        break;
                    }
                }

                if (isAmmo)
                {
                    // Access properties using reflection
                    var itemType = item.GetType();
                    var propsProperty = itemType.GetProperty("Properties") ?? itemType.GetProperty("_props") ?? itemType.GetProperty("Props");

                    if (propsProperty != null)
                    {
                        dynamic props = propsProperty.GetValue(item);
                        var propsType = props.GetType();
                        var weightProperty = propsType.GetProperty("Weight");

                        if (weightProperty != null && weightProperty.CanWrite)
                        {
                            var currentWeight = weightProperty.GetValue(props);
                            if (currentWeight != null)
                            {
                                double originalWeight = Convert.ToDouble(currentWeight);
                                double newWeight = originalWeight * config.WeightPercentage;

                                // Set new weight
                                weightProperty.SetValue(props, newWeight);

                                modifiedCount++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("========================================");
            Console.WriteLine("[Ammo Weight Modifier] SUCCESS!");
            Console.WriteLine($"[Ammo Weight Modifier] Modified {modifiedCount} ammo items");
            Console.WriteLine($"[Ammo Weight Modifier] Weight multiplier: {config.WeightPercentage:P0}");
            Console.WriteLine("========================================");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Ammo Weight Modifier] ERROR: {ex.Message}");
            Console.WriteLine($"[Ammo Weight Modifier] Stack: {ex.StackTrace}");
        }

        return Task.CompletedTask;
    }

    private ModConfig LoadConfig(string configPath)
    {
        var defaultConfig = new ModConfig { WeightPercentage = 0.0f };

        try
        {
            if (!File.Exists(configPath))
            {
                Console.WriteLine("[Ammo Weight Modifier] Could not find config, using default value (0% weight)");
                // Create default config file
                var configDir = Path.GetDirectoryName(configPath);
                if (configDir != null && !Directory.Exists(configDir))
                {
                    Directory.CreateDirectory(configDir);
                }
                File.WriteAllText(configPath, JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true }));
                return defaultConfig;
            }

            var configJson = File.ReadAllText(configPath);
            var config = JsonSerializer.Deserialize<ModConfig>(configJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (config == null)
            {
                Console.WriteLine("[Ammo Weight Modifier] Failed to parse config, using default value (0% weight)");
                return defaultConfig;
            }

            Console.WriteLine($"[Ammo Weight Modifier] Config loaded: {config.WeightPercentage:P0} weight");
            return config;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Ammo Weight Modifier] Could not load config: {ex.Message}, using default value (0% weight)");
            return defaultConfig;
        }
    }
}

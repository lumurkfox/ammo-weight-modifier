using SPTarkov.Server.Core.Models.Spt.Mod;

namespace AmmoWeightModifier;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "ammo-weight-modifier";
    public override string Name { get; init; } = "Ammo Weight Modifier";
    public override string Author { get; init; } = "LumurkFox";
    public override List<string>? Contributors { get; init; } = new();
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
    public override List<string>? Incompatibilities { get; init; } = new();
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; } = new();
    public override string Url { get; init; } = "";
    public override bool? IsBundleMod { get; init; } = false;
    public override string License { get; init; } = "MIT";
}

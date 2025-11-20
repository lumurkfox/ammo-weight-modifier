# Ammo Weight Modifier

A server-side mod for SPT 4.0+ that allows you to modify the weight of all ammunition in the game.

## Features

- Adjustable ammo weight via percentage multiplier
- Server-side mod that syncs with all clients (Fika compatible)
- Easy configuration via JSON file

## Installation

1. Download the latest release
2. Extract the `WeightlessAmmo` folder to `SPT/user/mods/`
3. Configure the weight percentage in `config/config.json`
4. Start your SPT server

## Configuration

Edit `config/config.json`:

```json
{
  "weightPercentage": 0.0
}
```

### Weight Percentage Values

- `0.0` = 0% weight (completely weightless)
- `0.1` = 10% of original weight
- `0.5` = 50% of original weight
- `1.0` = 100% of original weight (unchanged)

## Requirements

- SPT 4.0.x
- .NET 9.0 (included with SPT)

## Building from Source

1. Clone the repository
2. Open the project in your preferred IDE or use the command line
3. Run `dotnet build -c Release`
4. The compiled DLL will be in `bin/Release/net9.0/`
5. Copy `WeightlessAmmo.dll` to the mod root directory

## License

MIT License

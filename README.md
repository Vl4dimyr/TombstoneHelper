# TombstoneHelper

## Description

This mod allows you to access a nearby tombstone, its intended purpose is to access glitched tombstones in dungeons which are not reachable otherwise.

When you are close to a tombstone you will receive a status effect "Tombstone nearby",
in oder to interact with the tombstone you can press `Left Shift` + `E`.

![Tombstone nearby status effect](https://raw.githubusercontent.com/Vl4dimyr/TombstoneHelper/master/images/sc_status_effect.jpg)

## Help me out

[![Patreon](https://cdn.iconscout.com/icon/free/png-64/patreon-2752105-2284922.png)](https://www.patreon.com/vl4dimyr)

It is by no means required, but if you would like to support me head over to [my Patreon](https://www.patreon.com/vl4dimyr).


## Config

### TL;DR

Use [Official BepInEx ConfigurationManager](https://valheim.thunderstore.io/package/Azumatt/Official_BepInEx_ConfigurationManager/) for in-game settings!

### Manual Config

The config file (`\BepInEx\config\com.userstorm.tombstonehelper.cfg`) will be crated automatically when the mod is loaded.
You need to restart the game for changes to apply in game.

#### Example config

```ini
## Settings file was created by plugin TombstoneHelper v1.0.0
## Plugin GUID: com.userstorm.tombstonehelper

[General]

## Open tombstone key combination
# Setting type: KeyboardShortcut
# Default value: E + LeftShift
Open tombstone = E + LeftShift

## Interaction range
# Setting type: Single
# Default value: 5
# Acceptable value range: From 0.1 to 100
Interaction range = 5
```

> Higher values for `Interaction range` may cause issues when trying to interact with the tombstone from afar as the game does not recognize the owner of the tombstone. Getting closer to the tombstone usually resolves the issue.

## Manual Install

- Install [BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/) and [Jotunn](https://valheim.thunderstore.io/package/ValheimModding/Jotunn/)
- Download the latest `Vl4dimyr-TombstoneHelper-x.y.z.zip` [here](https://thunderstore.io/package/Vl4dimyr/TombstoneHelper/)
- Extract and move contents of the ZIP file into the `\BepInEx\plugins\TombstoneHelper\` folder
- (optional) Install [Official BepInEx ConfigurationManager](https://valheim.thunderstore.io/package/Azumatt/Official_BepInEx_ConfigurationManager/)

## Changelog

The [Changelog](https://github.com/Vl4dimyr/TombstoneHelper/blob/master/CHANGELOG.md) can be found on GitHub.

## Bugs/Feedback

For bugs or feedback please use [GitHub Issues](https://github.com/Vl4dimyr/TombstoneHelper/issues).

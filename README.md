# [![](Images/Logo.gif)](https://archidog1998.github.io/RotationSolver/#/) **Rotation Solver**

Repo:

```
https://puni.sh/api/repository/croizat
```

## Brief Notice

Rotation Solver's original developer, [ArchiTed](https://github.com/ArchiDog1998/), has quit XIV. The previous repository is located [here](https://github.com/ArchiDog1998/RotationSolver).

Development of the plugin is now community maintained here. PRs are always welcome. My immediate goal is to overhaul the codebase so that it is easier to maintain and work on in the longterm since the original developer will be gone now. At the very least, I intend on keeping the plugin updated.

The discord situation is unknown at the time of writing but it's still up so the current ReadMe links ArchiTed set up will stay active here in the meantime.

## Plugin Info

> Analyses combat information every frame and finds the best action.

This means almost all the information available in one frame in combat, including the status of all players in the party, the status of any hostile targets, skill cooldowns, the MP and HP of characters, the location of characters, casting status of the hostile target, combo, combat duration, player level, etc.

The best action can be highlighted as a recommendation or automatically executed.

It is designed for `general combat`, not for savage or ultimate. Use it carefully.

## Compatibility

Plugins that affect the targeting system or action system have varying levels of incompatibility with Rotation Solver. Some may interfer with the action execution and some may prevent operation entirely. The following list is not _guaranteed_ to cause issues, but there are reports, nor is this list exhaustive.

- [XIVCombo](https://github.com/daemitus/XIVComboPlugin)
- [ReAction](https://github.com/UnknownX7/ReAction)
- [Simple Tweaks](https://github.com/Caraxi/SimpleTweaksPlugin): Specifically the [`Block Targeting Treasure Hunt Enemies`](https://github.com/Caraxi/SimpleTweaksPlugin/blob/7e94915afa17ea873d48be2c469ebdaddd2e5200/Tweaks/TreasureHuntTargets.cs) tweak.

## Contributions

- This repository is open to any PRs

## Links

If you have any questions about usage, please check the [Wiki](https://archidog1998.github.io/RotationSolver/#/).

The rotations definitions are [here](https://github.com/ArchiDog1998/FFXIVRotations)

[![Discord](https://discordapp.com/api/guilds/1064448004498653245/embed.png?style=banner2)](https://discord.gg/4fECHunam9)

[![Crowdin](https://badges.crowdin.net/badge/light/crowdin-on-dark.png)](https://crowdin.com/project/rotationsolver)

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/B0B0IN5DX)

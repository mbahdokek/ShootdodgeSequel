[![](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExYW92amI4ZDgzZTQ4dnBweWUzeWp4ZTgyZmNwaDY4c2FiN2dsdG1yYSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/OY6vcGPeFPiNw6liN9/giphy-downsized-large.gif)](http://www.youtube.com/watch?v=kEQCRlDJv-w "Click to play on Youtube.com")
  
  This GIF is from early v1.0 version, now it's much better!

Thanks to jedijosh920 for the <a href="https://www.gta5-mods.com/scripts/shootdodge">original mod</a>  <br />
<b>Credit him and me if you use this code for your project </b>

I like Max Payne and Shootdodge's great mod. Unfortunately, it's outdated. I decided to revive it and added stuffs to make it better. 
Showcase video above is from v0.9, lot of changes since then.

Features:
<ul type="square">
<li>Bullet Meter HUD (texture by <a href="https://www.nexusmods.com/maxpayne/mods/19">Dragononandon</a>) acts as a limit to balance gameplay, forces you to think strategically</li>
<li>Weapon drains bullet meter differently, Pistols drain slower compared to Rifle</li>
<li>Bullet meter will be filled very slowly, but killing people will fill the time meter faster</li>
<li>Sound effects implemented</li>
<li>Better controller\gamepad support (read Usage below)</li>
<li>Bullet meter HUD and Sound can be deactivated inside .ini config</li>
<li>Config for changing time scale and dodging forces (in case the jump is too high for you, read incompatibility below)</li>
<li>Character will stay prone on the floor in aiming mode on landing unless move keys are pressed</li></ul>

<b>Requirement:</b>
ScriptHookVDotNet3. I strongly recommend the <a href="https://github.com/scripthookvdotnet/scripthookvdotnet-nightly">Nightly Version</a>
This script was made on game version 3179

<b>Changelog:</b>  
1.3
<ul type="disc">
<li>Now works with Dual Wield Reboot 1.1 with limitation (see Dual Wield page)</li>
<li>Added wider camera view (Thanks to JoyLucien), you can turn it off in the config</li>
<li>Added faster gun fire rate (shoot normal speed on slow-mo), you can turn it off in the config</li>
<li>INI config now moved inside Shootdodge folder. Added proper error handling when textures or INI are missing</li>
<li>Better hud textures, now in dds instead of png format</ul>1.2b<ul type="disc">
<li>Better controller support, now shootdodge by double-clicking Reload button instead of Aim first then Jump. It means no more overlapping with the player rolling, and the button is more reachable</li>
<li>Changed diagonal leaping direction, char facing correctly</li>
<li>Default config changed, forces reduced a bit more</li>
<li>Small code refactoring</li></ul>1.1
<ul type="disc">
<li>Changed the forces delivery, now unpredictable forces mostly fixed, leaping-landing curve is smoother</li>
<li>Fixed the endless floating bug on slopes (slopes still not recommended tho)</li>
<li>Height limit to ragdoll raised back. You can jump from higher places</li>
<li>Now if health is below 50% the time will slow a little bit more (read config)</li>
<li>Default config adjusted closer to what Max Payne 3 values (jump is not far but high, same level as enemy's head)</li>
<li>Removed hud delay, configs and codes refactoring</li></ul>1.0.
<ul type="disc">
<li>Tightened the height limit to ragdoll to reduce floating bugs around stairs/slopes</li>
<li>Sound overhaul: added heartbeat sfx, ambient sounds reduced when shootdodging, removed loud gun bang on the intro</li>
<li>Configs overhaul: forces and timescale can now be adjusted, added adjustable hud update delay to lighten script load</li>
<li>Running vs walking will produce a slightly different boost unless the inconsistency bug breaks the forces</li>
</ul>

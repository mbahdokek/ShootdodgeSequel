[![](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExYW92amI4ZDgzZTQ4dnBweWUzeWp4ZTgyZmNwaDY4c2FiN2dsdG1yYSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/OY6vcGPeFPiNw6liN9/giphy-downsized-large.gif)](http://www.youtube.com/watch?v=kEQCRlDJv-w "Click to play on Youtube.com")

Thanks to jedijosh920 for the <a href="https://www.gta5-mods.com/scripts/shootdodge">original mod</a> 

I like action movies and Shootdodge's great mod. Unfortunately, it's outdated so I decided to revive it, and added stuffs that I think the mod needed to balance it, to make it work like a skill.
Showcase video above is from v0.9, new changes below, read till the end. If you ever have a problem installing, please chat me on my discord.

Features:
<ul type="square">
<li>Bullet Meter HUD (texture by <a href="https://www.nexusmods.com/maxpayne/mods/19">Dragononandon</a>) acts as a limit to balance gameplay, forces you to think strategically</li>
<li>Weapon drains bullet meter differently, Pistols drain slower compared to Rifle</li>
<li>Bullet meter will be filled very slowly, but killing people will fill the time meter faster</li>
<li>Sound effects implemented</li>
<li>Better controller support on v1.2 (read Usage below)</li>
<li>Bullet meter HUD and Sound can be deactivated inside .ini config</li>
<li>Config for changing time scale and dodging forces (in case the jump is too high for you, read incompatibility below)</li>
<li>Character will stay prone on the floor in aiming mode on landing unless move keys are pressed</li>
</ul>

<b>Requirement:</b>
ScriptHookVDotNet3. I strongly recommend the <a href="https://github.com/scripthookvdotnet/scripthookvdotnet-nightly">Nightly Version</a>
This script was made on game version 3179

Bugs: 
<ul>
<li>Mistakenly choosing melee weapons in the middle of diving will break animation</li>
<li>Legs will get through the ground when landing on slopes or uneven ground. It looks weird but not important so I'll ignore this</li>
<li>Inconsistent forces. This bug makes flying sometimes very low and short (Mostly fixed on v1.1)</li></ul>
<b>Incompatibility</b>: <b>Slow-mo mods</b> will conflict if activated simultaneously. Tested SilverFinish's <b>KillCam</b>, it works but after Kill Cam, the dodging will end too because the time reverted. This also includes when choosing weapons, so don't change weapons when you've started the dodge, it will stop the slow-mo prematurely
<b>Physics mod or ragdoll mod </b> will affect this mod. The jump height & push forward force when diving are indeed affected, you need to adjust the forward & upward forces in the config to suit your game if you use one.  
<br />
<br />
Changelog:
<br />
1.2b
<ul type="disc">
<li>Better controller support, now shootdodge by double-clicking Reload button instead of Aim first then Jump. It means no more overlapping with the player rolling, and the button is more reachable</li>
<li>Changed diagonal leaping direction, char facing correctly</li>
<li>Default config changed, forces reduced a bit more</li>
<li>Small code refactoring</li></ul>
1.1
<ul type="disc">
<li>Changed how the force's delivered, I've tested it and now unpredictable forces almost never happen</li>
<li>Fixed the endless floating bug on slopes (slopes still not recommended tho)</li>
<li>Height limit to ragdoll raised back, You can jump from high places as long as the height still makes sense (tall buildings = suicide), there's a health penalty for this</li>
<li>Now if health is below 50% the time will slow a little bit more (read config)</li>
<li>Default config adjusted closer to what Max Payne 3 values (jump is not far but high, same level as enemy's head)</li>
<li>Removed the hud draw delay, tidied up the config and some codes</li></ul>
1.0.
<ul type="disc">
<li>Tightened the height limit to ragdoll and some limitations to reduce bugs around stairs/slopes</li>
<li>Sound overhaul: added heartbeat sfx, ambient sounds reduced when shootdodging, removed loud gun bang on the intro</li>
<li>Configs overhaul: forces and timescale can now be adjusted, added adjustable hud update delay to lighten script load</li>
<li>Running vs walking will produce a slightly different boost unless the inconsistency bug breaks the forces</li>
</ul>

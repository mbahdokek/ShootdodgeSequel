[![](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExYW92amI4ZDgzZTQ4dnBweWUzeWp4ZTgyZmNwaDY4c2FiN2dsdG1yYSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/OY6vcGPeFPiNw6liN9/giphy-downsized-large.gif)](http://www.youtube.com/watch?v=kEQCRlDJv-w "Click to play on Youtube.com")

Thanks to jedijosh920 for the <a href="https://www.gta5-mods.com/scripts/shootdodge">original mod</a>  <br />
<b>Please credit him if you use this code for your project </b>

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

Bugs: 
<ul>
<li>Mistakenly choosing melee weapons in the middle of diving will break animation</li>
<li>Legs will get through the ground when landing on slopes or uneven ground. It looks weird but not important so I'll ignore this</li>
<li>Faster fire rate (v1.3) doesn't affect the gun sounds, the gun still sounds like it shoots at slowed fire rate, but if you look at the bullet holes it shoots at normal speed</li>
<li>Physics/ragdoll mods affect this mod, adjust forward/upward forces accordingly in the .ini config to prevent jumping too far/high</ul><b>Incompatibility</b>: 
<b>Slow-mo mods</b> will conflict if activated simultaneously. SilverFinish's <b>KillCam</b> works but after Kill Cam-ing, the dodging will end too
<b>Physics/ragdoll mods</b> will affect this mod. The jump height & push forward force when diving are indeed affected, you need to adjust the forward & upward forces in the config to suit your game if you use one.

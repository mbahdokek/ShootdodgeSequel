using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Shootdodge
{
    public class Main : Script
    {
        private int currentScriptStatus;
        private string animName;
        private string animDict;
        private Ped player = Game.Player.Character;
        private Vector3 forceDirection;
        private Vector3 playerVelocity;
        private Color barColor;
        private uint playerWeap;
        private bool DoNothing = false;
        private bool EnemyNerfed = false;
        private bool highGround = false;
        private bool onTopVehicle = false;
        private float dodgeEnergy = 100.0f;
        private float lastEnergy = 100.0f;
        private float VehSpeed;
        private float energyMod;
        private float stanceModifier;
        private float groundHeight2;
        private readonly float timeScaleCfg = Configs.timeScale;
        private readonly float hudScaleDiv = Configs.hudScaleDiv;
        private readonly float hudPosX = Configs.hudPosX;
        private readonly float hudPosY = Configs.hudPosY;
        private readonly float xyForce = Configs.forceFwd;
        private readonly float zForce = Configs.forceUp;
        private float slomoSpeed;
        private ScriptSound soundCue1;
        private ScriptSound slomoSound;
        private ScriptSound soundCue2;
        private const float maxEnergy = 100.0f;
        private const float energyCost = 10.0f;
        private const float rechargeRate = 0.50f;
        private readonly Dictionary<float, string> textureNames = new Dictionary<float, string>
        {
            {15.0f, "fg12.5.png"}, {30.0f, "fg25.0.png"},{45.0f, "fg37.5.png"}, {60.0f, "fg50.0.png"},
            {75.0f, "fg62.5.png"}, {83.5f, "fg75.0.png"}, {93.5f, "fg87.5.png"}, {99.0f, "fg100.png"}
        };
        private string currentTextureName = "fg100.png";
        private List<Ped> enemies = new List<Ped>();
        private List<Ped> victims = new List<Ped>();
        private List<int> oldEnemyAccuracy = new List<int>();

        public Main()
        {
            Tick += new EventHandler(OnTick);
            KeyDown += new KeyEventHandler(OnKeyDown);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Configs.key)
                return;
            ActivateShootdodge();
        }

        private void ActivateShootdodge()
        {
            if (DoNothing || Game.TimeScale < 1.0f || dodgeEnergy < energyCost + energyMod)
                return;

            dodgeEnergy -= energyCost + energyMod;
            float health = player.Health;
            if(health > 0.5 * player.MaxHealth)
            slomoSpeed = timeScaleCfg / 1.125f;
            else slomoSpeed = timeScaleCfg;
            onTopVehicle = false;

            if (Configs.enableSfx)
            {
                slomoSound = Audio.GetSoundId();
                soundCue1 = Audio.GetSoundId();
                soundCue2 = Audio.GetSoundId();
            }

            if (currentScriptStatus != 0)
                return;

            float LR = Game.GetControlValueNormalized(GTA.Control.ScriptLeftAxisX);
            float UD = Game.GetControlValueNormalized(GTA.Control.ScriptLeftAxisY);
            bool flag = player.IsInCover;
            if (player.Speed > 9f)
            {
                onTopVehicle = true;
                VehSpeed = player.Speed;
            }
            player.Task.ClearAllImmediately();
            player.IsCollisionProof = false;
            player.FacePosition(GameplayCamera.Position + GameplayCamera.Direction * 1000f);
            Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, false);

            if (flag)
            {
                animName = "dive_start_run";
                forceDirection = player.ForwardVector.Normalized * -xyForce + Vector3.RelativeTop * zForce;
                player.FacePosition(GameplayCamera.Position + GameplayCamera.Direction * -1000f);
                player.Task.PlayAnimation("move_jump", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
            }
            if (LR == 0.0f && UD == -1.0f)
            {
                animName = "dive_start_run";
                forceDirection = player.ForwardVector.Normalized * xyForce + Vector3.RelativeTop * zForce;
                player.Task.PlayAnimation("move_jump", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
            }
            else if (LR == 0.0f && UD == 1.0f)
            {
                animName = "dive_start_run";
                forceDirection = player.ForwardVector.Normalized * -xyForce + Vector3.RelativeTop * zForce;
                player.FacePosition(GameplayCamera.Position + GameplayCamera.Direction * -1000f);
                player.Task.PlayAnimation("move_jump", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
            }
            //LR
            else if (LR == -1.0f && UD == 0.0f)
            {
                animName = "react_front_dive_left";
                forceDirection = player.RightVector.Normalized * -xyForce + Vector3.RelativeTop * zForce;
                player.Task.PlayAnimation("move_avoidance@generic_m", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
            }
            else if (LR == 1.0f && UD == 0.0f)
            {
                animName = "react_front_dive_right";
                forceDirection = player.RightVector.Normalized * xyForce + Vector3.RelativeTop * zForce;
                player.Task.PlayAnimation("move_avoidance@generic_m", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);

            }
            //Diagonal
            else if (LR == -1.0f && UD == -1.0f)
            {
                animName = "react_front_dive_left";
                forceDirection = player.RightVector.Normalized * -xyForce + player.ForwardVector.Normalized * xyForce + Vector3.RelativeTop * zForce;
                player.Task.PlayAnimation("move_avoidance@generic_m", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);

            }
            else if (LR == -1.0f && UD == 1.0f)
            {
                animName = "react_front_dive_left";
                forceDirection = player.RightVector.Normalized * -xyForce + player.ForwardVector.Normalized * -xyForce + Vector3.RelativeTop * zForce;
                player.Task.PlayAnimation("move_avoidance@generic_m", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
            }
            else if (LR == 1.0f && UD == -1.0f)
            {
                animName = "react_front_dive_right";
                forceDirection = player.RightVector.Normalized * xyForce + player.ForwardVector.Normalized * xyForce + Vector3.RelativeTop * zForce;
                player.Task.PlayAnimation("move_avoidance@generic_m", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
            }
            else if (LR == 1.0f && UD == 1.0f)
            {
                animName = "react_front_dive_right";
                forceDirection = player.RightVector.Normalized * xyForce + player.ForwardVector.Normalized * -xyForce + Vector3.RelativeTop * zForce;
                player.Task.PlayAnimation("move_avoidance@generic_m", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
            }
            else
            {
                animName = "dive_start_run";
                forceDirection = player.ForwardVector.Normalized * xyForce + Vector3.RelativeTop * zForce;
                player.Task.PlayAnimation("move_jump", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
            }
            Audio.SetAudioFlag(AudioFlags.AllowScriptedSpeechInSlowMo, true);
            Function.Call(Hash.PLAY_PAIN, player, 22, 0, 0);
            Function.Call(Hash.REQUEST_MISSION_AUDIO_BANK, "HUNTING_MAIN_A", false, -1);
            if (animName == "dive_start_run") animDict = "move_jump";
            else animDict = "move_avoidance@generic_m";
            ++currentScriptStatus;
            DiveSound();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if ((currentScriptStatus == 1 || currentScriptStatus == 2) && Game.TimeScale < 1.0f && Vector3.Distance(World.Raycast(Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, player, Bone.SkelSpine0, 0f, 0f, 0f), 
                Vector3.RelativeBottom, 1000f, IntersectFlags.Peds | IntersectFlags.Map | IntersectFlags.Vehicles | IntersectFlags.Objects, player).HitPosition, player.Position) < 1.2f) 
                ApplyForcesUpw();
            if ((currentScriptStatus == 1 || currentScriptStatus == 2) && Game.TimeScale < 1.0f && player.Speed < 9f)
                ApplyForcesFwd();
            if ((currentScriptStatus == 1 || currentScriptStatus == 2) && Game.TimeScale < 1.0f && onTopVehicle)
                ApplyForcesFwdHi();

            if (EnemyNerfed && Game.TimeScale == 1.0f)
                ResetEnemy();

            if (player.IsDead
                            || Game.IsPaused
                            || Game.IsCutsceneActive
                            || Game.IsLoading
                            || player.IsRagdoll
                            || player.IsOnFire
                            || player.IsJumping
                            || player.IsFalling
                            || player.IsInWaterStrict
                            || player.IsInAir
                            || player.IsInVehicle()
                            || !Function.Call<bool>(Hash.IS_PED_ARMED, player, 4)
                            || player.IsPlayingAnimation(new CrClipAsset("anim@sports@ballgame@handball@", "ball_get_up"))
                            || player.IsGettingUp)
                DoNothing = true;
            else DoNothing = false;

            player = Game.Player.Character;
            playerVelocity = player.Velocity;

            if (Game.Player.IsAiming && Game.IsEnabledControlJustPressed(GTA.Control.Jump) && Game.LastInputMethod == InputMethod.GamePad && Configs.controllerOn)
                ActivateShootdodge();

            if (dodgeEnergy < maxEnergy && currentScriptStatus == 0)
                dodgeEnergy = Math.Min(maxEnergy, dodgeEnergy + rechargeRate * Game.LastFrameTime);

            if (dodgeEnergy < maxEnergy)
                KillNFill();

            if (player.IsDead)
            { dodgeEnergy = maxEnergy; lastEnergy = maxEnergy - 5f; }

            EnergyModifier();
            UpdateTexture(); DrawHUD();

            if (player.IsFalling || player.MaterialCollidingWith == MaterialHash.CarMetal)
                player.IsCollisionProof = false;

            if (player.IsRunning) stanceModifier = 1.2f;
            else stanceModifier = 1.0f;

            if (currentScriptStatus == 1 || currentScriptStatus == 2)
            {
                Hud.HideComponentThisFrame(HudComponent.WeaponWheel);
                Function.Call(Hash.HUD_SUPPRESS_WEAPON_WHEEL_RESULTS_THIS_FRAME);
            }

            if (currentScriptStatus >= 1 && (player.IsDead || player.IsInWaterStrict))
            {
                Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
                player.CanRagdoll = true;
                player.IsCollisionEnabled = true;
                Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, false);
                player.Task.ClearAll();
                EndAudio();
                Game.TimeScale = 1f;
                currentScriptStatus = 0;
            }

            if (currentScriptStatus == 2)
            World.GetGroundHeight(player.Position, out groundHeight2, GetGroundHeightMode.Normal);
            if (currentScriptStatus == 2 && (player.Position.Z > groundHeight2 + 7.0f))
            {
                player.IsCollisionProof = false;
                player.CanRagdoll = true;
                player.IsCollisionEnabled = true;
                player.SendNMMessage(Euphoria.NMMessage.highFall, 3000);
                Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, false);
                player.Velocity = new Vector3(playerVelocity.X / 2f, playerVelocity.Y / 2f, playerVelocity.Z / 2f);
                player.Task.ClearAll();
                EndAudio();
                Game.TimeScale = 1f;
                Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
                currentScriptStatus = 0;
            }

            if (currentScriptStatus > 2 && player.IsRagdoll)
            {
                player.IsCollisionProof = true;
                player.CanRagdoll = true;
                player.IsCollisionEnabled = true;
                Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, false);
                player.Task.ClearAll();
                EndAudio();
                Game.TimeScale = 1f;
                Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
                currentScriptStatus = 0;
            }

            else if ((currentScriptStatus == 1 || currentScriptStatus == 2 || currentScriptStatus == 3)
                     && Capsule1(Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, player, Bone.SkelHead, 0f, 0f, 0f), 0.105f, 0f, 0f, 0.075f).DidHit)
            {
                Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
                player.CanRagdoll = true;
                player.IsCollisionProof = true;
                player.IsCollisionEnabled = true;
                Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, false);
                player.Task.ClearAll();
                EndAudio();
                player.SendNMMessage(Euphoria.NMMessage.braceForImpact, 1500);
                Game.TimeScale = 1f;
                currentScriptStatus = 0;
            }
            else
            {
                switch (currentScriptStatus)
                {
                    case 1:
                        if (Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, player, animDict, animName) < 0.4f) //if (Game.GameTime < gameTimer)
                            break;
                        if (DoNothing || Game.TimeScale < 1.0f)
                            break;
                        Function.Call(Hash.PLAY_PAIN, player, 11, 0, 0);
                        Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, true);
                        player.Task.ClearAll();
                        Function.Call(Hash.TASK_AIM_GUN_SCRIPTED, player, Function.Call<Hash>(Hash.GET_HASH_KEY, "SCRIPTED_GUN_TASK_PLANE_WING"), 1, 1);
                        Game.TimeScale = slomoSpeed;
                        NerfEnemy();
                        player.CanRagdoll = false;
                        player.IsCollisionEnabled = false;
                        Function.Call(Hash.ANIMPOSTFX_PLAY, "FocusIn", 0, false);
                        Slo_moSound();
                        ++currentScriptStatus;
                        break;
                    case 2:
                        if (player.Position.Z > groundHeight2 + 3.35f) highGround = true;
                        if (!Capsule2(Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, player, Bone.SkelPelvis, 0f, 0f, 0f), 0.290f, 0.0f, 0f, 0f).DidHit)
                            break;
                        Function.Call(Hash.ANIMPOSTFX_STOP, "FocusIn");
                        Function.Call(Hash.ANIMPOSTFX_PLAY, "FocusOut", 0, false);
                        player.Velocity = new Vector3(playerVelocity.X / 2f, playerVelocity.Y / 2f, playerVelocity.Z / 2f);
                        player.Task.ClearAll();
                        Function.Call(Hash.TASK_AIM_GUN_SCRIPTED, player, Function.Call<Hash>(Hash.GET_HASH_KEY, "SCRIPTED_GUN_TASK_PLANE_WING"), 1, 1);
                        Game.TimeScale = 1f;
                        Function.Call(Hash.PLAY_PAIN, player, 33, 0, 0);
                        player.CanRagdoll = false;
                        player.IsCollisionEnabled = true;
                        LandSound();
                        if (highGround) player.ApplyDamage(15);
                        ++currentScriptStatus;
                        highGround = false;
                        break;
                    case 3:
                        float LR = Game.GetControlValueNormalized(GTA.Control.ScriptLeftAxisX);
                        float UD = Game.GetControlValueNormalized(GTA.Control.ScriptLeftAxisY);
                        if (soundCue2.HasFinished() || soundCue1.HasFinished())
                            EndAudio();
                        if (!Game.IsEnabledControlJustPressed(GTA.Control.Jump) && LR == 0.0f && UD == 0.0f)
                            break;
                        Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, true);
                        Game.TimeScale = 1f;
                        EndAudio();
                        player.IsCollisionEnabled = true;
                        ++currentScriptStatus;
                        break;
                    case 4:
                        if (!Game.IsControlJustPressed(GTA.Control.MoveUpOnly) && !Game.IsControlJustPressed(GTA.Control.MoveDownOnly) && !Game.IsControlJustPressed(GTA.Control.MoveLeftOnly) && !Game.IsControlJustPressed(GTA.Control.MoveRightOnly))
                            break;
                        player.Task.ClearAll();
                        Game.TimeScale = 1f;
                        Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, false);
                        Function.Call(Hash.PLAY_PAIN, player, 23, 0, 0);
                        player.Task.PlayAnimation("anim@sports@ballgame@handball@", "ball_get_up", -8f, 1f, 1250, AnimationFlags.UseKinematicPhysics, 0.0f);
                        player.IsCollisionEnabled = true;
                        player.CanRagdoll = true;
                        currentScriptStatus = 0;
                        break;
                }
            }
        }

        private void UpdateTexture()
        {
            if (Configs.enableHud == false)
                return;
            foreach (var level in textureNames)
            {
                if ((dodgeEnergy >= level.Key && lastEnergy < level.Key) || (dodgeEnergy < level.Key && lastEnergy >= level.Key))
                {
                    currentTextureName = level.Value;
                    break;
                }
            }
        }

        private void DrawHUD()
        {
            if (Configs.enableHud == false || DoNothing)
                return;
            // new TextElement("Current speed = " + player.Speed, new PointF(50f, 50f), 0.5f).ScaledDraw();;
            if (dodgeEnergy < energyCost + energyMod)
                barColor = Color.Red;
            else barColor = Color.White;
            string texturePath = "scripts/Shootdodge/" + currentTextureName;
            new CustomSprite("scripts/Shootdodge/bg.png", new SizeF(70f / hudScaleDiv, 262f / hudScaleDiv),
                new PointF(hudPosX, hudPosY), barColor).Draw();
            if (dodgeEnergy > energyCost)
                new CustomSprite(texturePath, new SizeF(70f / hudScaleDiv, 262f / hudScaleDiv),
                    new PointF(hudPosX, hudPosY)).Draw();
        }

        private void EnergyModifier()
        {
            if (!DoNothing)
            {
                playerWeap = Function.Call<uint>(Hash.GET_SELECTED_PED_WEAPON, player);
                uint weapType = Function.Call<uint>(Hash.GET_WEAPONTYPE_GROUP, playerWeap);
                string weapGroup = Utils.GetWeaponGroupFromHash(weapType);
                if (weapGroup == "Pistol")
                    energyMod = 2.45f;
                else if (weapGroup == "Shotgun" || weapGroup == "Sniper")
                    energyMod = 10.0f;
                else energyMod = 20f;
            }
        }

        private void KillNFill()
        {
            WeaponHash weapon = (WeaponHash)playerWeap;
            foreach (Ped pedKilled in World.GetAllPeds())
            {
                if (pedKilled.Killer == player && pedKilled.CauseOfDeath == weapon && !victims.Contains(pedKilled))
                {
                    dodgeEnergy = Math.Min(dodgeEnergy + 10f, maxEnergy);
                    victims.Add(pedKilled);
                }
            }
        }

        private void ApplyForcesFwd()
        {
            Function.Call(Hash.APPLY_FORCE_TO_ENTITY, player, 1,
            (forceDirection.X * stanceModifier) * Game.LastFrameTime,
            (forceDirection.Y * stanceModifier) * Game.LastFrameTime,
            0f,
            0, 0, 0, 8, false, false, true, false, true);
        }

        private void ApplyForcesFwdHi()
        {
            if (player.Speed < VehSpeed * 1.2f)
            {
                Function.Call(Hash.APPLY_FORCE_TO_ENTITY, player, 1,
                (forceDirection.X * stanceModifier) * Game.LastFrameTime,
                (forceDirection.Y * stanceModifier) * Game.LastFrameTime,
                0f,
                0, 0, 0, 8, false, false, true, false, true);
            }
        }

        private void ApplyForcesUpw()
        {
            Function.Call(Hash.APPLY_FORCE_TO_ENTITY, player, 1,
            0f,
            0f,
            (forceDirection.Z * stanceModifier) * Game.LastFrameTime,
            0, 0, 0, 7, false, false, true, false, true);
        }

        private void NerfEnemy()
        {
            if (!Configs.nerfAcc || EnemyNerfed) // Set Ped Accuracy to 0
               return;
                EnemyNerfed = true;
                foreach (Ped allPed in World.GetAllPeds())
                {
                    if (allPed != player)
                    {
                        enemies.Add(allPed);
                        oldEnemyAccuracy.Add(allPed.Accuracy);
                        allPed.Accuracy = 0;
                    }
                }
        }

        private void ResetEnemy()
        {
            if (!Configs.nerfAcc)
               return;
                EnemyNerfed = false;
                for (int index = 0; index < enemies.Count; index = index - 1 + 1)
                {
                    enemies[index].Accuracy = oldEnemyAccuracy[index];
                    enemies.RemoveAt(index);
                    oldEnemyAccuracy.RemoveAt(index);
                }
        }

        private void DiveSound()
        {
            if (Configs.enableSfx == false) return;
            soundCue2.PlaySound("ROUND_ENDING_STINGER_CUSTOM", "CELEBRATION_SOUNDSET");
            soundCue1.PlaySound("Short_Transition_Out", "PLAYER_SWITCH_CUSTOM_SOUNDSET");
        }

        private void LandSound()
        {
            if (Configs.enableSfx == false) return;
            soundCue1.PlaySound("Short_Transition_Out", "PLAYER_SWITCH_CUSTOM_SOUNDSET");
            soundCue2.PlaySound("CHECKPOINT_UNDER_THE_BRIDGE", "HUD_MINI_GAME_SOUNDSET");
        }

        private void Slo_moSound()
        {
            if (Configs.enableSfx == false) return;
            Function.Call(Hash.START_AUDIO_SCENE, "HUNTING_02_SETTINGS");
            Function.Call(Hash.SET_AUDIO_SCENE_VARIABLE, "HUNTING_02_SETTINGS", "Concentration", 1.0f);
            Function.Call(Hash.SET_AUDIO_SCENE_VARIABLE, "HUNTING_02_SETTINGS", "Breathing", 1.0f);
            slomoSound.PlaySoundFromEntity(player, "Heart_Breathing");
            Function.Call(Hash.SET_VARIABLE_ON_SOUND, slomoSound, "Concentration", 20f);
        }

        private void EndAudio()
        {
            if (Configs.enableSfx == false) return;
            soundCue1.Stop();
            slomoSound.Stop();
            soundCue2.Stop();
            Function.Call(Hash.STOP_AUDIO_SCENE, "HUNTING_02_SETTINGS");
            soundCue1.Release();
            slomoSound.Release();
            soundCue2.Release();
            Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "HUNTING_MAIN_A");
        }

        public ShapeTestResult Capsule1(Vector3 position, float radius, float protrudeX, float protrudeY, float protrudeZ)
        {
            ShapeTestHandle capsule1 = ShapeTest.StartTestCapsule(
              position,
              position + new Vector3(protrudeX, protrudeY, protrudeZ),
              radius,
              IntersectFlags.Peds | IntersectFlags.Map | IntersectFlags.Vehicles | IntersectFlags.Objects, player);

            capsule1.GetResult(out ShapeTestResult result);
            return result;
        }

        public ShapeTestResult Capsule2(Vector3 position, float radius, float protrudeX, float protrudeY, float protrudeZ)
        {
            ShapeTestHandle capsule2 = ShapeTest.StartTestCapsule(
            position + new Vector3(protrudeX, protrudeY, protrudeZ),
            position + new Vector3(protrudeX, protrudeY, protrudeZ),
            radius,
            IntersectFlags.Peds | IntersectFlags.Map | IntersectFlags.Vehicles | IntersectFlags.Objects, player);

            capsule2.GetResult(out ShapeTestResult result);
            return result;
        }
    }
}

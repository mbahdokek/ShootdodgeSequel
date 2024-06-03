using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Shootdodge
{
  public class Main : Script
  {
    private int currentScriptStatus;
    private string animName;
    private string animDict;    
    private Ped player = Game.Player.Character;
    private readonly int hudWaitMs = Configs.updateHudMs;
    private Vector3 forceDirection;
    private Vector3 playerVelocity;
    private Vector3 playerPos;
    private Color barColor;
    private uint playerWeap;
    private bool DoNothing = false;
    private float dodgeEnergy = 100.0f;
    private float lastEnergy = 100.0f;
    private float energyMod;
    private float playerStance;
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
      if (e.KeyCode != Configs.toggleKey)
        return;
      ActivateShootdodge();
    }

    private void ActivateShootdodge()
    {
      if (DoNothing || Game.TimeScale < 1.0f || dodgeEnergy < energyCost + energyMod)
        return;

      if (Configs.lowerEnemyAccuracy) // Set Ped Accuracy to 0
      {
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
      dodgeEnergy -= energyCost + energyMod;
      
      if (Configs.enableSfx)
      slomoSound = Audio.GetSoundId(); 
      soundCue1 = Audio.GetSoundId();
      soundCue2 = Audio.GetSoundId();

      if (currentScriptStatus != 0)
        return;
      float LR = Game.GetControlValueNormalized(GTA.Control.ScriptLeftAxisX);
      float UD = Game.GetControlValueNormalized(GTA.Control.ScriptLeftAxisY);
      bool flag = player.IsInCover;
      player.Task.ClearAllImmediately();
      player.IsCollisionProof = false;
      player.FacePosition(GameplayCamera.Position + GameplayCamera.Direction * 1000f);
      Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, false);
      playerPos = player.Position;

      if (flag)
      {
        animName = "dive_start_run";
        forceDirection = player.ForwardVector.Normalized * -Configs.jumpForceForward + Vector3.RelativeTop * Configs.jumpForceUpward;
        player.FacePosition(GameplayCamera.Position + GameplayCamera.Direction * -1000f);
        player.Task.PlayAnimation("move_jump", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
      }
      if (LR == 0.0f && UD == -1.0f)
      {
        animName = "dive_start_run";
        forceDirection = player.ForwardVector.Normalized * Configs.jumpForceForward + Vector3.RelativeTop * Configs.jumpForceUpward;
        player.Task.PlayAnimation("move_jump", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
      }
      else if (LR == 0.0f && UD == 1.0f)
      {
        animName = "dive_start_run";
        forceDirection = player.ForwardVector.Normalized * -Configs.jumpForceForward + Vector3.RelativeTop * Configs.jumpForceUpward; 
        player.FacePosition(GameplayCamera.Position + GameplayCamera.Direction * -1000f);
        player.Task.PlayAnimation("move_jump", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
      }
      else if (LR == -1.0f && UD == 0.0f)
      {
        animName = "react_front_dive_left";
        forceDirection = player.RightVector.Normalized * -Configs.jumpForceForward + Vector3.RelativeTop * Configs.jumpForceUpward;
        player.Task.PlayAnimation("move_avoidance@generic_m", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
      }
      else if (LR == 1.0f && UD == 0.0f)
      {
        animName = "react_front_dive_right";
        forceDirection = player.RightVector.Normalized * Configs.jumpForceForward + Vector3.RelativeTop * Configs.jumpForceUpward;
        player.Task.PlayAnimation("move_avoidance@generic_m", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
            
      }
      else if (LR == -1.0f && UD == -1.0f)
      {
        animName = "react_front_dive_left";
        forceDirection = player.RightVector.Normalized * -Configs.jumpForceForward + player.ForwardVector.Normalized * Configs.jumpForceForward + Vector3.RelativeTop * Configs.jumpForceUpward;
        player.Task.PlayAnimation("move_avoidance@generic_m", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
        
      }
      else if (LR == -1.0f && UD == 1.0f)
      {
        animName = "react_front_dive_left";
        forceDirection = player.RightVector.Normalized * -Configs.jumpForceForward + player.ForwardVector.Normalized * -Configs.jumpForceForward + Vector3.RelativeTop * Configs.jumpForceUpward;
        player.Task.PlayAnimation("move_avoidance@generic_m", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
      }
      else if (LR == 1.0f && UD == -1.0f)
      {
        animName = "react_front_dive_right"; 
        forceDirection = player.RightVector.Normalized * Configs.jumpForceForward + player.ForwardVector.Normalized * Configs.jumpForceForward + Vector3.RelativeTop * Configs.jumpForceUpward;
        player.Task.PlayAnimation("move_avoidance@generic_m", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
      }
      else if (LR == 1.0f && UD == 1.0f)
      {
        animName = "react_front_dive_right"; 
        forceDirection = player.RightVector.Normalized * Configs.jumpForceForward + player.ForwardVector.Normalized * -Configs.jumpForceForward + Vector3.RelativeTop * Configs.jumpForceUpward;
        player.Task.PlayAnimation("move_avoidance@generic_m", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
      }
      else
      {
        animName = "dive_start_run";
        forceDirection = player.ForwardVector.Normalized * Configs.jumpForceForward + Vector3.RelativeTop * Configs.jumpForceUpward;
        player.Task.PlayAnimation("move_jump", animName, 8f, 1f, -1, AnimationFlags.None, 0.0f);
      }
      Function.Call(Hash.PLAY_PAIN, player, 32, 0.0, false);
      Function.Call(Hash.REQUEST_MISSION_AUDIO_BANK, "HUNTING_MAIN_A", false, -1);
      if (animName == "dive_start_run") animDict = "move_jump";
      else animDict = "move_avoidance@generic_m";
      ++currentScriptStatus;
      DiveSound();
    }

   private void OnTick(object sender, EventArgs e)
    {
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
        
      if (Game.Player.IsAiming && Game.IsEnabledControlJustPressed(GTA.Control.Jump) && Game.LastInputMethod == InputMethod.GamePad && Configs.controllerEnabled)
        ActivateShootdodge();

      if (dodgeEnergy < maxEnergy && currentScriptStatus == 0)
      dodgeEnergy = Math.Min(maxEnergy, dodgeEnergy + rechargeRate * Game.LastFrameTime);

      if(dodgeEnergy < maxEnergy)
        KillNFill();

      if(player.IsDead) 
      { dodgeEnergy = maxEnergy; lastEnergy = maxEnergy - 5f; }

      EnergyModifier();
      UpdateTexture(); DrawHUD();

      if (player.IsFalling || player.MaterialCollidingWith == MaterialHash.CarMetal)
      player.IsCollisionProof = false;

      if(player.IsRunning) playerStance = 1.10f;
      else playerStance = 1.0f;
                
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
            if (currentScriptStatus == 2 && player.Position.Z > World.GetGroundHeight(player.Position) + 3.3f)
            {
                player.IsCollisionProof = false;
                player.CanRagdoll = true;
                player.IsCollisionEnabled = true;
                player.SendNMMessage(Euphoria.NMMessage.highFall, 3000);
                Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, false);
                player.Velocity = new Vector3 (playerVelocity.X / 2f, playerVelocity.Y / 2f, playerVelocity.Z / 2f);
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
               && RaycastSphere(Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, player, Bone.SkelHead, 0f, 0f, 0f), 0.105f, 0f, 0f, 0.075f).DidHit)  // Check for slope/walls in front of
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
            player.Position = new Vector3 (player.Position.X, player.Position.Y, playerPos.Z + 0.025f);
            ApplyForces();
            player.Task.ClearAll();
            Function.Call(Hash.TASK_AIM_GUN_SCRIPTED, player, Function.Call<Hash>(Hash.GET_HASH_KEY, "SCRIPTED_GUN_TASK_PLANE_WING"), 1, 1);
            Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, true);
            player.CanRagdoll = false;
            player.IsCollisionEnabled = false;
            Function.Call(Hash.ANIMPOSTFX_PLAY, "FocusIn", 0, false);
            Game.TimeScale = Configs.timeScale;
            Slo_moSound();
            ++currentScriptStatus;
          break;
          case 2:
            if (!RaycastSphere1(Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, player, Bone.SkelSpine1, 0f, 0f, 0f), 0.170f, 0.0f, 0f, 0f).DidHit) // Scan ground vs body
                break;
            Function.Call(Hash.ANIMPOSTFX_STOP, "FocusIn");
            Function.Call(Hash.ANIMPOSTFX_PLAY, "FocusOut", 0, false);
            player.Velocity = new Vector3 (playerVelocity.X / 2f, playerVelocity.Y / 2f, playerVelocity.Z / 2f);
            player.Task.ClearAll();
            Function.Call(Hash.TASK_AIM_GUN_SCRIPTED, player, Function.Call<Hash>(Hash.GET_HASH_KEY, "SCRIPTED_GUN_TASK_PLANE_WING"), 1, 1);
            Game.TimeScale = 1f;
            player.CanRagdoll = false;
            player.IsCollisionEnabled = true;
            Function.Call(Hash.SET_PED_CAN_ARM_IK, player, true);
            Function.Call(Hash.SET_PED_CAN_LEG_IK, player, true);
            Function.Call(Hash.SET_PED_CAN_TORSO_IK, player, true);
            LandSound();
            ++currentScriptStatus;
          break;
          case 3:
            float LR = Game.GetControlValueNormalized(GTA.Control.ScriptLeftAxisX);
            float UD = Game.GetControlValueNormalized(GTA.Control.ScriptLeftAxisY);
            if(soundCue2.HasFinished() || soundCue1.HasFinished())
            EndAudio();
            if (!Game.IsEnabledControlJustPressed(GTA.Control.Jump) && LR == 0.0f && UD == 0.0f)  // Check movement to break
                 break;
            if (Configs.lowerEnemyAccuracy)
            {
              for (int index = 0; index < enemies.Count; index = index - 1 + 1)
              {
                enemies[index].Accuracy = oldEnemyAccuracy[index];
                enemies.RemoveAt(index);
                oldEnemyAccuracy.RemoveAt(index);
              }
            }
            Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, true);
            Game.TimeScale = 1f;
            EndAudio();
            player.IsCollisionEnabled = true;
            ++currentScriptStatus;
          break;
          case 4:
            player.Task.ClearAll();
            Game.TimeScale = 1f;
            Function.Call(Hash.SET_PLAYER_FORCED_AIM, Game.Player, false);
            Function.Call(Hash.PLAY_PAIN, player, 23, 0.0, false);
            player.Task.PlayAnimation("anim@sports@ballgame@handball@", "ball_get_up", -8f, 1f, 1250, AnimationFlags.None, 0.0f);
            player.IsCollisionEnabled = true;
            player.CanRagdoll = true;
            currentScriptStatus = 0;
          break;
        }
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
        
        private void KillNFill()
        {
            WeaponHash weapon = (WeaponHash) playerWeap;
            foreach (Ped pedKilled in World.GetAllPeds())
            {
                if (pedKilled.Killer == player && pedKilled.CauseOfDeath == weapon && !victims.Contains(pedKilled))
                {
                    dodgeEnergy = Math.Min(dodgeEnergy + 10f, maxEnergy);
                    victims.Add(pedKilled);
                }
            }
        }

        private void DrawHUD()
        {
            if (Configs.enableHud == false || DoNothing)
                return;
            // new TextElement("Current vectorFwd = " + player.ForwardVector, new PointF(50f, 50f), 0.5f).ScaledDraw();
            if (dodgeEnergy < energyCost + energyMod)
                barColor = Color.Red; else barColor = Color.White;
            string texturePath = "scripts/shootdodge/" + currentTextureName;
            new CustomSprite("scripts/shootdodge/bg.png", new SizeF(70f / Configs.hudScaleDiv, 262f / Configs.hudScaleDiv), 
                new PointF(Configs.hudPosX, Configs.hudPosY), barColor).Draw();
            Wait(hudWaitMs);
            if (dodgeEnergy > energyCost)
            new CustomSprite(texturePath, new SizeF(70f / Configs.hudScaleDiv, 262f / Configs.hudScaleDiv), 
                new PointF(Configs.hudPosX, Configs.hudPosY)).Draw();
        }

        private void ApplyForces()
        {
                Function.Call(Hash.APPLY_FORCE_TO_ENTITY_CENTER_OF_MASS, player, 1,
                (forceDirection.X * playerStance * 224.8089f) * Game.LastFrameTime,
                (forceDirection.Y * playerStance * 224.8089f) * Game.LastFrameTime,
                (forceDirection.Z * playerStance * 224.8089f) * Game.LastFrameTime,
                 10, false, true, false);
        }

        public ShapeTestResult RaycastSphere(Vector3 position, float radius, float protrudeX, float protrudeY, float protrudeZ)
        {
        ShapeTestHandle sphereHandle = ShapeTest.StartTestCapsule(
          position,
          position + new Vector3(protrudeX, protrudeY, protrudeZ), 
          radius, 
          IntersectFlags.Peds | IntersectFlags.Map | IntersectFlags.Vehicles | IntersectFlags.Objects, player);

         sphereHandle.GetResult(out ShapeTestResult result);
         return result;
        }

        public ShapeTestResult RaycastSphere1(Vector3 position, float radius, float protrudeX, float protrudeY, float protrudeZ)
        {
            ShapeTestHandle sphereHandle1 = ShapeTest.StartTestCapsule(
            position + new Vector3(protrudeX, protrudeY, protrudeZ),
            position + new Vector3 (protrudeX, protrudeY, protrudeZ),
            radius, 
            IntersectFlags.Peds | IntersectFlags.Map | IntersectFlags.Vehicles | IntersectFlags.Objects, player);

            sphereHandle1.GetResult(out ShapeTestResult result);
            return result;
        }
    }
}

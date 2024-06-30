using GTA;
using GTA.Native;
using System;

namespace Shootdodge
{
    public static class Cam
    {
        public static Camera camera;
        public static DateTime Timer;
        public static bool Active = false;
        private static float fov;

        public static void Create()
        {
            switch ((int)GameplayCamera.FollowPedCamViewMode)
            {
                case 0:
                    fov = 70.0f;
                    break;
                case 1:
                    fov = 75.0f;
                    break;
                case 2:
                    fov = 80.0f;
                    break;
            }
            camera = Camera.Create(ScriptedCameraNameHash.DefaultScriptedCamera, GameplayCamera.Position, GameplayCamera.Rotation, fov, true);
            ScriptCameraDirector.StartRendering(false);
            Timer = DateTime.Now;
            Active = true;
        }

        public static void Destroy()
        {
            if (camera != null)
            {
                camera.IsActive = false;
                camera.Delete();
            }
            ScriptCameraDirector.StopRendering();
            Active = false;
        }

        public static void Update()
        {
            if (Main.player.IsAiming && ScriptCameraDirector.RenderingCam == camera)
            {
                Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, 14);
                Function.Call(Hash.SET_HUD_COMPONENT_POSITION, 14, 0f, 0f);
            }
            camera.Position = GameplayCamera.Position;
            camera.Direction = GameplayCamera.Direction;
        }
    }
}

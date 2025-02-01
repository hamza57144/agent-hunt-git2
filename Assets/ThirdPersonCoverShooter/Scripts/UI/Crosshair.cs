using UnityEngine;

namespace CoverShooter
{
    [RequireComponent(typeof(ThirdPersonCamera))]
    public class Crosshair : MonoBehaviour
    {
       
        private CharacterOutline characterOutline;
        private CharacterMotor enemyMotor;
       
        // Crosshair colors
        public Color defaultColor = Color.white;
        public Color targetColor = Color.red;

        // Manual size control (use a value less than 1 to reduce size)
        [Tooltip("Manual multiplier to adjust crosshair size.")]
        public float manualSizeMultiplier = 0.5f;
        /// <summary>
        /// Settings to use when aiming a weapon of a type not covered by other properties.
        /// </summary>
        [Tooltip("Settings to use when aiming a weapon of a type not covered by other properties.")]
        public CrosshairSettings Default = CrosshairSettings.Default();

        /// <summary>
        /// Settings to use when aiming a pistol.
        /// </summary>
        [Tooltip("Settings to use when aiming a pistol.")]
        public CrosshairSettings Pistol = CrosshairSettings.Default();

        /// <summary>
        /// Settings to use when aiming a rifle.
        /// </summary>
        [Tooltip("Settings to use when aiming a rifle.")]
        public CrosshairSettings Rifle = CrosshairSettings.Default();

        /// <summary>
        /// Settings to use when aiming a shotgun.
        /// </summary>
        [Tooltip("Settings to use when aiming a shotgun.")]
        public CrosshairSettings Shotgun = CrosshairSettings.Default();

        /// <summary>
        /// Settings to use when aiming a sniper.
        /// </summary>
        [Tooltip("Settings to use when aiming a sniper.")]
        public CrosshairSettings Sniper = CrosshairSettings.Default();

        private ThirdPersonCamera _thirdPersonCamera;
        private Camera _camera;

        private float _fov;
        private CrosshairSettings _settings = CrosshairSettings.Default();
        private float _previousAlpha;

        private void Awake()
        {
            _thirdPersonCamera = GetComponent<ThirdPersonCamera>();
            _camera = GetComponent<Camera>();
        }

        /// <summary>
        /// Draws the crosshair.
        /// </summary>
        private void OnGUI()
        {
            var settings = _settings;

            if (_thirdPersonCamera.Target != null)
            {
                var weapon = _thirdPersonCamera.Target.ActiveWeapon;

                if (!_thirdPersonCamera.Target.IsChangingWeapon || (_thirdPersonCamera.CrosshairAlpha > _previousAlpha))
                {
                    if (weapon.Gun != null && weapon.Gun.UseCustomCrosshair)
                        settings = weapon.Gun.CustomCrosshair;
                    else if (weapon.Gun != null && weapon.Gun.Type == WeaponType.Pistol)
                        settings = Pistol;
                    else if (weapon.Gun != null && weapon.Gun.Type == WeaponType.Rifle)
                        settings = Rifle;
                    else
                        settings = Default;
                }
            }
            else
                settings = Default;

            _previousAlpha = _thirdPersonCamera.CrosshairAlpha;

            if (settings.Sprites == null || settings.Sprites.Length == 0)
                return;

            var targetFOV = _thirdPersonCamera.ShakeOffset * settings.ShakeMultiplier;
            targetFOV += _thirdPersonCamera.Target.Bloom * settings.RecoilMultiplier * 3.14f;

            if (_thirdPersonCamera.Target != null)
            {
                var gun = _thirdPersonCamera.Target.ActiveWeapon.Gun;

                if (gun != null)
                {
                    if (_thirdPersonCamera.Target.IsZooming)
                        targetFOV += gun.Error * _thirdPersonCamera.Target.ZoomErrorMultiplier;
                    else
                        targetFOV += gun.Error;
                }
            }

            if (_thirdPersonCamera.Target != null)
                targetFOV += _thirdPersonCamera.Target.MovementError;

            Util.Lerp(ref _fov, targetFOV, settings.Adaptation);

            if (_thirdPersonCamera.Target != null && _thirdPersonCamera.Target.IsScoping)
                return;
            
            var lerp = settings.LowAngle < settings.HighAngle ? Mathf.Clamp01((_fov - settings.LowAngle) / (settings.HighAngle - settings.LowAngle)) : 0;
            var sprite = settings.Sprites[(int)(lerp * (settings.Sprites.Length - 1))];

            if (sprite == null)
                return;

            var aimFOV = Mathf.Lerp(settings.LowAngle, settings.HighAngle, lerp);

            var size = settings.Scale * Screen.height * aimFOV / _camera.fieldOfView * manualSizeMultiplier;
            var point = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);

            var texture = sprite.texture;
            var textureRect = sprite.textureRect;
            var textureRectOffset = sprite.textureRectOffset;
            var pivot = sprite.pivot;

            var source = new Rect(textureRect.x / texture.width, textureRect.y / texture.height, textureRect.width / texture.width, textureRect.height / texture.height);
            var aspectRatio = textureRect.width / textureRect.height;

            Vector2 offset;
            offset.x = (pivot.x - textureRectOffset.x) / textureRect.width;
            offset.y = (textureRect.height - pivot.y + textureRectOffset.y) / textureRect.height;

            var dest = new Rect(point.x - size * offset.x * aspectRatio, point.y - size * offset.y, size * aspectRatio, size);

            // Check if crosshair is on target
            Color crosshairColor = defaultColor;
            
            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
               
                // Check if the object hit is an enemy or a target
                if (hit.collider.CompareTag(TagsHandler.Enemy))
                {
                    crosshairColor = targetColor;                    
                    characterOutline = hit.collider.GetComponent<CharacterOutline>();
                    enemyMotor = hit.collider.GetComponent<CharacterMotor>();
                    EnableHealthBar(enemyMotor,true);
                    if (GameData.CompletedLevelIndex < 3)
                        ChangeOutline(characterOutline, true);
                }                             
                else
                {
                    ChangeOutline(characterOutline, false);
                    EnableHealthBar(enemyMotor, false);
                }


            }

            // Apply color and draw crosshair
            var previous = GUI.color;
            GUI.color = new Color(crosshairColor.r, crosshairColor.g, crosshairColor.b, _thirdPersonCamera.CrosshairAlpha);
            GUI.DrawTextureWithTexCoords(dest, texture, source, true);
            GUI.color = previous;
        }
        private void ChangeOutline(CharacterOutline characterOutline,bool change)
        {
            if (characterOutline != null)
                characterOutline.enabled = change;
        } 
        private void EnableHealthBar(CharacterMotor characterMotor,bool enable)
        {
            if (characterMotor != null)
              characterMotor.GetHealthBar.EnableHealthBar(enable);
        }
    }
}

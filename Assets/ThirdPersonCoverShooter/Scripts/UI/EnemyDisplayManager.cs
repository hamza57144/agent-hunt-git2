using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CoverShooter
{
    /// <summary>
    /// Creates and manages objects with Health Bar for all enemies visible on screen. 
    /// </summary>
    public class EnemyDisplayManager : MonoBehaviour
    {

        /// <summary>
        /// Prototype of an arrow to be shown for any active enemy that is away.
        /// </summary>
        [Tooltip("Prototype of an arrow to be shown for any active enemy that is away.")]
        public RectTransform ArrowPrototype;

        /// <summary>
        /// Player that is used to determine who is an enemy.
        /// </summary>
        [HideInInspector]
        public BaseActor Player;

        private void Start()
        {
            Player = GameManager.instance.PlayerMotor.gameObject.GetComponent<BaseActor>();
        }

        /// <summary>
        /// Offset of the health bar relative to the screen height.
        /// </summary>
        [Tooltip("Offset of the health bar relative to the screen height.")]
        public Vector2 Offset = new Vector2(0, 0.1f);

        private Dictionary<GameObject, GameObject> _bars = new Dictionary<GameObject, GameObject>();
        private Dictionary<GameObject, GameObject> _away = new Dictionary<GameObject, GameObject>();
        private List<GameObject> _keep = new List<GameObject>();
        bool IsEnemyVisible(Transform enemyTransform, Camera camera)
        {
            // Step 1: Check if the enemy is within the viewport
            Vector3 viewportPos = camera.WorldToViewportPoint(enemyTransform.position);

            // Consider visible if inside the viewport with a small margin
            const float margin = 0.02f; // Allows for slight overflows near edges
            bool isInViewport = viewportPos.z > 0 &&
                                viewportPos.x > 0 - margin && viewportPos.x < 1 + margin &&
                                viewportPos.y > 0 - margin && viewportPos.y < 1 + margin;

            if (!isInViewport)
                return false;

            // Step 2: Perform a raycast to verify direct line of sight
            Vector3 rayStart = camera.transform.position;
            Vector3 direction = enemyTransform.position - rayStart;

            if (Physics.Raycast(rayStart, direction, out RaycastHit hit))
            {
                return hit.transform == enemyTransform; // Ensure the ray hits the enemy
            }

            return false; // Occluded or off-screen
        }

        private void LateUpdate()
        {
            /// Manage arrows
            {
                _keep.Clear();

                if (ArrowPrototype != null)
                {
                    foreach (var character in Characters.AllAlive)
                    {
                        if (character.Actor != null && (Player == null || character.Actor.Side != Player.Side))
                        {
                            var enemyTransform = character.Object.transform;
                            var camera = Camera.main;
                            var characterMotor = character.Object.GetComponent<CharacterMotor>();
                            if (characterMotor == null)
                            {
                                continue; // Skip this character if no CharacterMotor exists
                            }

                            bool isEnemyVisible = characterMotor.isVisible;

                            // Fixed height offset above the enemy
                            const float arrowHeightOffset = 2.5f; // Adjust this value to raise the arrow higher

                            if (IsEnemyVisible(enemyTransform, camera)) // Assuming you have visibility check
                            {
                                // Calculate the position directly above the enemy
                                Vector3 arrowWorldPosition = enemyTransform.position + Vector3.up * arrowHeightOffset;

                                // Convert the world position to screen position
                                Vector3 screenPos = camera.WorldToScreenPoint(arrowWorldPosition);

                                if (!_away.ContainsKey(character.Object))
                                {
                                    var clone = GameObject.Instantiate(ArrowPrototype.gameObject);
                                    clone.transform.SetParent(transform, false);
                                    _away.Add(character.Object, clone);
                                }

                                var t = _away[character.Object].GetComponent<RectTransform>();
                                if (isEnemyVisible)
                                {
                                    t.gameObject.SetActive(true);
                                    t.position = new Vector3(screenPos.x, screenPos.y, t.position.z);
                                    t.eulerAngles = new Vector3(0, 0, -180);
                                }
                            }
                            else
                            {
                                // Handle the case when the enemy is not visible
                                Vector3 viewportPos = camera.WorldToViewportPoint(enemyTransform.position);
                                const float edgeThreshold = 0.05f;

                                if (viewportPos.z < 0)
                                {
                                    viewportPos.x = 1 - viewportPos.x;
                                    viewportPos.y = 1 - viewportPos.y;
                                }

                                float angle = 0;
                                bool isLeft = viewportPos.x < edgeThreshold;
                                bool isRight = viewportPos.x > 1 - edgeThreshold;
                                bool isDown = viewportPos.y < edgeThreshold;
                                bool isUp = viewportPos.y > 1 - edgeThreshold;

                                if (isUp) angle = isLeft ? 45 : isRight ? -45 : 0;
                                else if (isDown) angle = isLeft ? 135 : isRight ? -135 : 180;
                                else angle = isLeft ? 90 : -90;

                                if (isLeft) viewportPos.x = edgeThreshold;
                                if (isDown) viewportPos.y = edgeThreshold;
                                if (isRight) viewportPos.x = 1 - edgeThreshold;
                                if (isUp) viewportPos.y = 1 - edgeThreshold;

                                if (!_away.ContainsKey(character.Object))
                                {
                                    var clone = GameObject.Instantiate(ArrowPrototype.gameObject);
                                    clone.transform.SetParent(transform, false);
                                    _away.Add(character.Object, clone);
                                }

                                var t = _away[character.Object].GetComponent<RectTransform>();
                                if (isEnemyVisible)
                                {
                                    t.gameObject.SetActive(true);
                                    // Position the arrow based on viewport position
                                    t.position = new Vector3(viewportPos.x * Screen.width, (viewportPos.y * Screen.height) + arrowHeightOffset * 100, t.position.z); // Adjust this value to raise the arrow higher
                                    t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, angle);
                                }
                            }

                            _keep.Add(character.Object);
                        }
                    }

                    // Cleanup unused arrows
                    var toRemove = _away.Keys.Except(_keep).ToList();
                    foreach (var key in toRemove)
                    {
                        GameObject.Destroy(_away[key]);
                        _away.Remove(key);
                    }
                }
            }
        }
    }

}
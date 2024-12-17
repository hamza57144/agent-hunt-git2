using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace CoverShooter
{
    /// <summary>
    /// Creates and manages objects with Health Bar for all enemies visible on screen. 
    /// </summary>
    public class EnemyDisplayManager : MonoBehaviour
    {
        /// <summary>
        /// Prototype of a health bar to be shown on any visible enemy.
        /// </summary>
        [Tooltip("Prototype of a health bar to be shown on any visible enemy.")]
        public RectTransform HealthPrototype;

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
            Player = GameManager.instance.GetPlayer().gameObject.GetComponent<BaseActor>();
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
            _keep.Clear();

            if (ArrowPrototype != null)
            {
                foreach (var character in Characters.AllAlive)
                {
                    if (character.Actor != null && (Player == null || character.Actor.Side != Player.Side))
                    {
                        var enemyTransform = character.Object.transform;
                        var camera = Camera.main;

                        // Access the CharacterMotor script of the enemy to check the visibility flag
                        var characterMotor = character.Object.GetComponent<CharacterMotor>();

                        if (characterMotor != null && characterMotor.isVisible) // Only show arrow if isVisible is true
                        {
                            // Place arrow above the enemy (head position)
                            Vector3 screenPos = camera.WorldToScreenPoint(enemyTransform.position + Vector3.up * 2); // Adjust offset

                            // Increase the arrow height further when visible
                            screenPos.y += 50f;  // Adjust Y-position for more height (adjust as needed)

                            if (!_away.ContainsKey(character.Object))
                            {
                                var clone = GameObject.Instantiate(ArrowPrototype.gameObject);
                                clone.transform.SetParent(transform, false);
                                _away.Add(character.Object, clone);
                            }

                            var t = _away[character.Object].GetComponent<RectTransform>();
                            t.gameObject.SetActive(true);
                            t.position = new Vector3(screenPos.x, screenPos.y, t.position.z);
                            t.eulerAngles = Vector3.zero;
                        }
                        else
                        {
                            // Remove arrow if isVisible is false
                            if (_away.ContainsKey(character.Object))
                            {
                                _away[character.Object].SetActive(false);
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

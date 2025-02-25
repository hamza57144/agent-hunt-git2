﻿using UnityEngine;
using UnityEngine.UI;

namespace CoverShooter
{
    /// <summary>
    /// Takes a relative health  from a Character Health component and displays a bar that displays the value.
    /// </summary>
    [ExecuteInEditMode]
    public class HealthBar : MonoBehaviour
    {

        /// <summary>
        /// Object whose health is displayed on the health bar.
        /// </summary>
        [Tooltip("Object whose health is displayed on the health bar.")]
        public GameObject Target;

        /// <summary>
        /// Current value of the health bar.
        /// </summary>
        [Range(0, 1)]
        [Tooltip("Current value of the health bar.")]
        public float Value = 1.0f;

        /// <summary>
        /// Determines if the health bar is hidden when the target has no health.
        /// </summary>
        [Tooltip("Determines if the health bar is hidden when the target has no health.")]
        public bool HideWhenDead = true;

        /// <summary>
        /// Determines if the health bar is hidden when there is no target.
        /// </summary>
        [Tooltip("Determines if the health bar is hidden when there is no target.")]
        public bool HideWhenNone = false;

        /// <summary>
        /// Link to the object that draws the background of the health bar.
        /// </summary>
        [Tooltip("Link to the object that draws the background of the health bar.")]
        public RectTransform BackgroundRect;

        /// <summary>
        /// Link to the object that draws the health bar.
        /// </summary>
        [Tooltip("Link to the object that draws the health bar.")]
        public RectTransform FillRect;

        /// <summary>
        /// Link to the object that will be used to display a character's name.
        /// </summary>
        [Tooltip("Link to the object that will be used to display a character's name.")]
        public Text Name;

        private GameObject _cachedTarget;
        private CharacterHealth _cachedCharacterHealth;
        private CharacterName _cachedCharacterName;

        [Header("Health Bar")]
        public Transform healthTransform;
        public Vector3 topPosition;
        public Vector3 bottomPosition;

        private void LateUpdate()
        {
            // Make the health bar face the camera
            healthTransform.rotation = Quaternion.LookRotation(healthTransform.position - Camera.main.transform.position);

            // Keep the health bar upright (ignore X & Z rotation)
            healthTransform.rotation = Quaternion.Euler(0, healthTransform.rotation.eulerAngles.y, 0);
            /*if (isHealthbarEnabled) 
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    HideHealthBar();
                }
            }*/
            /*Vector3 targetPosition = Target.transform.position + Vector3.up * 6.0f; // Adjust height as needed
            transform.position = targetPosition;*/


            if (Target != _cachedTarget)
            {
                _cachedTarget = Target;

                if (Target != null)
                {
                    _cachedCharacterHealth = Target.GetComponent<CharacterHealth>();
                    _cachedCharacterName = Target.GetComponent<CharacterName>();
                }
                else
                {
                    _cachedCharacterHealth = null;
                    _cachedCharacterName = null;
                }
            }

            if (_cachedCharacterHealth != null)
                Value = _cachedCharacterHealth.Health / _cachedCharacterHealth.MaxHealth;

            var isVisible = true;
           

          

            if (isVisible)
            {
                if (Name != null)
                {
                    if (_cachedCharacterName == null)
                    {
                        if (Target != null)
                            Name.text = Target.name;
                    }
                    else
                        Name.text = _cachedCharacterName.Name;
                }

                if (FillRect != null)
                {
                   // FillRect.anchorMax = new Vector2(Value, 1);
                    FillRect.gameObject.GetComponent<Image>().fillAmount = Value;
                }
                  
            }
            
        }

        public void EnableHealthBar(bool enable)
        {

            FillRect.gameObject.SetActive(enable);
            BackgroundRect.gameObject.SetActive(enable);

        }

   
        public void SetHealthPositionTop()
        {
          
            healthTransform.transform.localPosition = topPosition;
        }
        public void SetHealthPositionBottom()
        {
            healthTransform.transform.localPosition = bottomPosition;
        }

    }
}
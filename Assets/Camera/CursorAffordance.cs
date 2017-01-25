﻿using UnityEngine;

namespace RPG
{
    public class CursorAffordance : MonoBehaviour
    {
        public Material highlightMaterial;
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D targetCursor = null;
        [SerializeField] Texture2D unknownCursor = null;
        [SerializeField] Vector2 cursorCenterFromTopLeft = new Vector2(96, 96);

        CameraRaycaster cameraRaycaster;

        // TODO consider factoring-out once enemies highlight by gamepad
        MeshRenderer previousEnemyRenderer;
        Material previousEnemyMaterial;

        void Start()
        {
            cameraRaycaster = GetComponent<CameraRaycaster>();
        }

        void FixedUpdate()
        {
            if (PlayerMovement.isInDirectMode)
            {
                Cursor.lockState = CursorLockMode.Locked;
                return;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }

            var hit = cameraRaycaster.hit; // Read current state
            UnhighlightEnemy();

            switch (cameraRaycaster.layerHit)
            {
                case Layer.Walkable:
                    Cursor.SetCursor(walkCursor, cursorCenterFromTopLeft, CursorMode.Auto);
                    break;
                case Layer.Enemy:
                    HighlightEnemy(cameraRaycaster.hit.collider.gameObject);
                    Cursor.SetCursor(targetCursor, cursorCenterFromTopLeft, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(unknownCursor, cursorCenterFromTopLeft, CursorMode.Auto);
                    break;
            }
        }

        private void UnhighlightEnemy()
        {
            if (previousEnemyRenderer != null)
            {
                previousEnemyRenderer.material = previousEnemyMaterial;
                previousEnemyRenderer = null;
            }
        }

        private void HighlightEnemy(GameObject enemy)
        {
            if (previousEnemyRenderer == null)
            {
                var previousEnemy = enemy;
                previousEnemyRenderer = previousEnemy.GetComponentInChildren<MeshRenderer>();
                previousEnemyMaterial = previousEnemyRenderer.material;
                previousEnemyRenderer.material = highlightMaterial;
            }
        }
    }
}
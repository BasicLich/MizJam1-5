﻿using Microsoft.Xna.Framework;
using System;

namespace MizJam1.Rendering
{
    /// <summary>
    /// Thanks to RogueSharp
    /// https://roguesharp.wordpress.com/2014/07/13/tutorial-5-creating-a-2d-camera-with-pan-and-zoom-in-monogame/
    /// </summary>
    public class Camera
    {
        // Construct a new Camera class with standard zoom (no scaling)
        public Camera(int viewportWidth, int viewportHeight, int levelCellWidth, int levelCellHeight)
        {
            ViewportWidth = viewportWidth;
            ViewportHeight = viewportHeight;
            LevelCellWidth = levelCellWidth;
            LevelCellHeight = levelCellHeight;
            zoom = MinZoom;
            MoveCamera(Vector2.Zero, true);
        }

        public int LevelCellWidth { get; set; }
        public int LevelCellHeight { get; set; }
        public Point LevelCellSize => new Point(LevelCellWidth, LevelCellHeight);
        public Point LevelSize => LevelCellSize * Global.SpriteSize;

        // Centered Position of the Camera in pixels.
        public Vector2 Position { get; set; }
        private float zoom;
        // Current Zoom level with 1.0f being standard
        public float Zoom
        {
            get => zoom;
            set
            {
                if (value < MinZoom) zoom = MinZoom;
                else if (value > 8f) zoom = 8f;
                else
                {
                    zoom = value;
                }
                MoveCamera(Vector2.Zero, true);
            }
        }
        public float MinZoom => Math.Max(((float)ViewportWidth) / LevelSize.X, ((float)ViewportHeight) / LevelSize.Y);
        // Current Rotation amount with 0.0f being standard orientation
        public float Rotation { get; set; }

        // Height and width of the viewport window which we need to adjust
        // any time the player resizes the game window.
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        // Center of the Viewport which does not account for scale
        public Vector2 ViewportCenter
        {
            get
            {
                return new Vector2(ViewportWidth / 2, ViewportHeight / 2);
            }
        }

        // Create a matrix for the camera to offset everything we draw,
        // the map and our objects. since the camera coordinates are where
        // the camera is, we offset everything by the negative of that to simulate
        // a camera moving. We also cast to integers to avoid filtering artifacts.
        public Matrix TransformationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)Position.X,
                   -(int)Position.Y, 0) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                   Matrix.CreateTranslation(new Vector3(ViewportCenter + new Vector2(420, 0), 0));
            }
        }

        // Move the camera in an X and Y amount based on the cameraMovement param.
        // if clampToMap is true the camera will try not to pan outside of the
        // bounds of the map.
        public void MoveCamera(Vector2 cameraMovement, bool clampToMap = false)
        {
            Vector2 newPosition = Position + cameraMovement;

            if (clampToMap)
            {
                Position = MapClampedPosition(newPosition);
            }
            else
            {
                Position = newPosition;
            }
        }

        public Rectangle ViewportWorldBoundary()
        {
            Vector2 viewPortCorner = ScreenToWorld(new Vector2(0, 0));
            Vector2 viewPortBottomCorner =
               ScreenToWorld(new Vector2(ViewportWidth, ViewportHeight));

            return new Rectangle((int)viewPortCorner.X,
               (int)viewPortCorner.Y,
               (int)(viewPortBottomCorner.X - viewPortCorner.X),
               (int)(viewPortBottomCorner.Y - viewPortCorner.Y));
        }

        // Center the camera on specific pixel coordinates
        public void CenterOn(Vector2 position)
        {
            Position = position;
        }

        // Clamp the camera so it never leaves the visible area of the map.
        private Vector2 MapClampedPosition(Vector2 position)
        {
            var cameraMax = new Vector2(LevelCellWidth * Global.SpriteWidth -
                (ViewportWidth / Zoom / 2),
                LevelCellHeight * Global.SpriteHeight -
                (ViewportHeight / Zoom / 2));
            cameraMax.Floor();
            var cameraMin = new Vector2(ViewportWidth / Zoom / 2, ViewportHeight / Zoom / 2);
            cameraMin.Ceiling();
            Vector2 res = Vector2.Clamp(position,
               cameraMin,
               cameraMax);
            return res;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TransformationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition,
                Matrix.Invert(TransformationMatrix));
        }
    }
}
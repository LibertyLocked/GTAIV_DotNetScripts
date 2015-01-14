using System;
using System.Drawing;
using System.Collections.Generic;
using GTA;

namespace GTAIV_DotNetScripts
{
    class NumericHealth : Script
    {
        Color healthNormalColor;
        Color healthLowColor;
        Color armorColor;
        Color healthNormalTbogtColor;
        Color armorTbogtColor;
        Color speedColor;
        Color vehHealthColor;

        public NumericHealth()
        {
            this.PerFrameDrawing += new GTA.GraphicsEventHandler(NumericHealth_PerFrameDrawing);
            // Player.Character.Armor = 100;
            healthNormalColor = Color.FromArgb(86, 124, 85);
            healthLowColor = Color.FromArgb(153, 61, 60);
            armorColor = Color.FromArgb(72, 149, 159);
            healthNormalTbogtColor = Color.White;
            armorTbogtColor = Color.FromArgb(132, 66, 148);
            speedColor = Color.White;
            vehHealthColor = Color.Silver;
        }

        private void NumericHealth_PerFrameDrawing(object sender, GraphicsEventArgs e)
        {
            RectangleF radar = e.Graphics.GetRadarRectangle(FontScaling.Pixel);  // this retrieves the rectangle of the radar on screen

            // calculate the center of the radar
            float radarCenterX = radar.X + radar.Width * 0.5f;
            float radarCenterY = radar.Y + radar.Height * 0.5f;

            e.Graphics.Scaling = FontScaling.Pixel;  // Pixel is the default setting, but you could also use any other scaling instead

            // Create a rectangle to display health and armor
            RectangleF healthArmorBox = new RectangleF(radar.X, radar.Y, radar.Width, radar.Height);

            // Get player's health and armor
            int health = Player.Character.Health;
            int armor = Player.Character.Armor;

            // Draw numeric health and armor if player isn't dead
            if (Player.Character.isAlive && Player.CanControlCharacter)
            {
                Color healthNormalColorToDraw;
                Color armorColorToDraw;

                // Set different colors for different episodes
                if (Game.CurrentEpisode == GameEpisode.TBOGT)
                {
                    healthNormalColorToDraw = healthNormalTbogtColor;
                    armorColorToDraw = armorTbogtColor;
                }
                else if (Game.CurrentEpisode == GameEpisode.TLAD)
                {
                    healthNormalColorToDraw = healthNormalTbogtColor;   // TLAD uses white health bar
                    armorColorToDraw = armorColor;
                }
                else
                {
                    healthNormalColorToDraw = healthNormalColor;
                    armorColorToDraw = armorColor;
                }

                // Draw numeric health text
                e.Graphics.DrawText(health.ToString(), healthArmorBox, TextAlignment.Left, health > 25 ? healthNormalColorToDraw : healthLowColor);
                // Draw numeric armor if there is one
                if (armor > 0) e.Graphics.DrawText(armor.ToString(), healthArmorBox, TextAlignment.Right, armorColorToDraw);

                // Draw numeric vehicle speed and health if player is in vechicle
                if (Player.Character.isInVehicle())
                {
                    Vehicle vehicle = Player.Character.CurrentVehicle;
                    float speedKph = vehicle.Speed * 3600 / 1000;   // convert from m/s to km/h
                    int vehHealth = vehicle.Health / 10;    // convert it from 1000 to 100 scale

                    // Draw the vehicle health on the bottom left
                    e.Graphics.DrawText(vehHealth.ToString(), healthArmorBox, TextAlignment.Bottom | TextAlignment.Left, vehHealthColor);

                    // Draw the converted speed on bottom right of the radar
                    e.Graphics.DrawText(speedKph.ToString("0"), healthArmorBox, TextAlignment.Bottom | TextAlignment.Right, speedColor);
                }
            }
        }
    }
}

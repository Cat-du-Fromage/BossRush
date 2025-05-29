using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using BossRush.Scenes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush;

public static class Globals
{
    public static Point ScreenSize()
    {
        return new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
    }

    public static GraphicsDevice GraphicsDevice { get; set; }
    public static ContentManager Content { get; set; }

    public static Game1 GameInstance { get; set; }

    public static SpriteFont Font { get; private set; }

    public static Texture2D WhitePixel { get; private set; }

    public static Dictionary<string, List<Texture2D>> ParticleTextures { get; private set; }

    public static void LoadContent()
    {
        Font = Content.Load<SpriteFont>("default");

        WhitePixel = new Texture2D(GraphicsDevice, 1, 1);
        WhitePixel.SetData([Microsoft.Xna.Framework.Color.White]);

        LoadParticleTextures();

        // Print the number of particle textures loaded for debugging
        foreach (var particleTexturesKey in ParticleTextures.Keys)
        {  
            Console.WriteLine($"Loaded {ParticleTextures[particleTexturesKey].Count} textures for particle type '{particleTexturesKey}'");
        }
    }

    public static RenderTarget2D GetNewRenderTarget()
    {
        return new RenderTarget2D(GraphicsDevice, ScreenSize().X, ScreenSize().Y);
    }

    private static void LoadParticleTextures()
    {
        ParticleTextures = new Dictionary<string, List<Texture2D>>();

        const string PARTICLE_PATH = "Particles";

        DirectoryInfo dir = new DirectoryInfo(Path.Combine(Content.RootDirectory, PARTICLE_PATH));
        // print the directory path for debugging
        if (!dir.Exists)
            throw new DirectoryNotFoundException("Particle folder not found: " + dir.FullName);

        // Loop through each file
        foreach (FileInfo file in dir.GetFiles("*.xnb"))
        {
            string fileName = Path.GetFileNameWithoutExtension(file.Name);

            // Extract the base name (e.g., "muzzle_01" → "muzzle")
            string key = fileName.Split('_')[0]; // Split by underscore and take the first part

            // Load the texture
            Texture2D texture = Content.Load<Texture2D>($"{PARTICLE_PATH}/{fileName}");

            // Add to dictionary
            if (!ParticleTextures.ContainsKey(key))
                ParticleTextures[key] = new List<Texture2D>();

            ParticleTextures[key].Add(texture);
        }
    }
}
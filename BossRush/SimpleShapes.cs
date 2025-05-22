using System;
using System.Data;
using BossRush;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class SimpleShapes
{ private static BasicEffect getBasicEffect()
    {
        return new BasicEffect(Globals.GraphicsDevice)
        {
            VertexColorEnabled = true,
            Projection = Matrix.CreateOrthographicOffCenter(
                0, Globals.GraphicsDevice.Viewport.Width,
                Globals.GraphicsDevice.Viewport.Height, 0,
                0, 1
            )
        };
    }

    public static void Rectangle(Vector2 position, Vector2 size, Color color)
    {
        VertexPositionColor[] vertices = new VertexPositionColor[6];

        // First triangle
        vertices[0].Position = new Vector3(position.X, position.Y, 0);
        vertices[1].Position = new Vector3(position.X + size.X, position.Y, 0);
        vertices[2].Position = new Vector3(position.X, position.Y + size.Y, 0);

        // Second triangle
        vertices[3].Position = new Vector3(position.X + size.X, position.Y, 0);
        vertices[4].Position = new Vector3(position.X + size.X, position.Y + size.Y, 0);
        vertices[5].Position = new Vector3(position.X, position.Y + size.Y, 0);

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].Color = color;
        }

        foreach (EffectPass pass in getBasicEffect().CurrentTechnique.Passes)
        {
            pass.Apply();
            Globals.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 2);
        }
    }

    public static void Circle(Vector2 position, float radius, Color color, int resolution = 15)
    {
        VertexPositionColor[] vertices = new VertexPositionColor[3 * resolution];
        double angleIncrement = 2 * Math.PI / resolution;
        
        Vector3 center = new Vector3(position.X, position.Y, 0);
        Vector3 p1;
        Vector3 p2 = new Vector3(position.X + radius, position.Y, 0);

        for (int i = 0; i < resolution; i++)
        {
            double angle = (i+1) * angleIncrement;
            p1 = p2;
            p2 = new Vector3(center.X + radius * (float)Math.Cos(angle),center.Y + radius * (float)Math.Sin(angle), 0);

            vertices[3*i] = new VertexPositionColor(center, color);
            vertices[3*i + 1] = new VertexPositionColor(p1, color);
            vertices[3*i + 2] = new VertexPositionColor(p2, color);
        }
        
        foreach (EffectPass pass in getBasicEffect().CurrentTechnique.Passes)
        {
            pass.Apply();
            Globals.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, resolution);
        }
    }

    public static void CircleOutline(Vector2 position, float radius, Color color, int resolution = 15)
    {
        VertexPositionColor[] vertices = new VertexPositionColor[resolution + 1];
        double angleIncrement = 2 * Math.PI / resolution;

        for (int i = 0; i <= resolution; i++)
        {
            double angle = (i+1) * angleIncrement;
            vertices[i].Position = new Vector3(position.X + radius * (float)Math.Cos(angle),position.Y + radius * (float)Math.Sin(angle), 0);
            vertices[i].Color = color;
        }
        
        foreach (EffectPass pass in getBasicEffect().CurrentTechnique.Passes)
        {
            pass.Apply();
            Globals.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, resolution);
        }
    }
    
}
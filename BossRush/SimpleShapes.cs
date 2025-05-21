using System;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SimpleShapes
{
    private GraphicsDevice graphicsDevice;
    private BasicEffect basicEffect;

    private static SimpleShapes _instance;

    public SimpleShapes(GraphicsDevice graphicsDevice)
    {
        if (_instance != null)
            throw new Exception("Only one instance should be created");
        
        this.graphicsDevice = graphicsDevice;

        basicEffect = new BasicEffect(graphicsDevice)
        {
            VertexColorEnabled = true,
            Projection = Matrix.CreateOrthographicOffCenter(
                0, graphicsDevice.Viewport.Width,
                graphicsDevice.Viewport.Height, 0,
                0, 1
            )
        };
        
        _instance = this;
    }

    private void DrawRectangle(Vector2 position, Vector2 size, Color color)
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

        foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 2);
        }
    }

    private void DrawCircle(Vector2 position, float radius, Color color, int resolution)
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
        
        foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, resolution);
        }
    }
    
    private void DrawCircleOutline(Vector2 position, float radius, Color color, int resolution)
    {
        VertexPositionColor[] vertices = new VertexPositionColor[resolution + 1];
        double angleIncrement = 2 * Math.PI / resolution;

        for (int i = 0; i <= resolution; i++)
        {
            double angle = (i+1) * angleIncrement;
            vertices[i].Position = new Vector3(position.X + radius * (float)Math.Cos(angle),position.Y + radius * (float)Math.Sin(angle), 0);
            vertices[i].Color = color;
        }
        
        foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, resolution);
        }
    }

    public static void Rectangle(Vector2 position, Vector2 size, Color color)
    {
        _instance.DrawRectangle(position, size, color);
    }

    public static void Circle(Vector2 position, float radius, Color color, int resolution = 15)
    {
        _instance.DrawCircle(position, radius, color, resolution);
    }

    public static void CircleOutline(Vector2 position, float radius, Color color, int resolution = 15)
    {
        _instance.DrawCircleOutline(position, radius, color, resolution);
    }
    
}
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

    public static void Rectangle(Vector2 position, Vector2 size, Color color)
    {
        _instance.DrawRectangle(position, size, color);
    }
    
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CGaG.Lab07 {
    public static class SimpleUtils {

        public static void DrawPrimitive(this Game thread, VertexPositionColor[ ] vertexList, PrimitiveType type, short[ ] indices) {
            VertexBuffer vertexBuffer = new VertexBuffer(thread.GraphicsDevice, typeof(VertexPositionColor), vertexList.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertexList);
            thread.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            uint primitiveCount;
            switch (type) {
            case PrimitiveType.TriangleList:
                primitiveCount = (uint)indices.Length / 3;
                break;
            case PrimitiveType.LineList:
                primitiveCount = (uint)indices.Length / 2;
                break;
            default:
                throw new Exception( );
            }

            thread.GraphicsDevice.DrawUserIndexedPrimitives(type, vertexList, 0, vertexList.Length, indices, 0, (int)primitiveCount);
        }

        public static Vector3 SphereToCart(this Vector3 v) {
            float cos = (float)Math.Cos(MathHelper.ToRadians(v.Z));
            return v.X * new Vector3(
                (float)Math.Cos(MathHelper.ToRadians(v.Y)) * cos,
                (float)Math.Sin(MathHelper.ToRadians(v.Z)),
                (float)Math.Sin(MathHelper.ToRadians(v.Y)) * cos);
        }

        public static void Median(ref float value, float min, float max) {
            if (min > max) {
                throw new Exception( );
            }
            if (value < min) {
                value = min;
            }
            if (value > max) {
                value = max;
            }
        }

    }
}

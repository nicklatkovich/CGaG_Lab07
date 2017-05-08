using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CGaG.Lab07 {
    public class MainThread : Game {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;

        KeyboardState keyboard;
        KeyboardState keyboardPrev = Keyboard.GetState( );

        BasicEffect Effect;

        float AxesLight = 0.7f;

        VertexPositionColor[ ] Points;
        Tuple<short, short, short>[ ] Faces = new Tuple<short, short, short>[ ] {
            new Tuple<short, short, short>(0, 2, 1),
            new Tuple<short, short, short>(0, 1, 3),
            new Tuple<short, short, short>(4, 3, 1),
            new Tuple<short, short, short>(1, 2, 5),
            new Tuple<short, short, short>(5, 4, 1),
            new Tuple<short, short, short>(2, 0, 3),
            new Tuple<short, short, short>(3, 5, 2),
            new Tuple<short, short, short>(3, 4, 5),
        };
        Vector3 SphereCameraPosition = new Vector3(10f, 315f, 45f);
        Vector3 SphereLightPosition = new Vector3(10f, 315f, 45f);

        Color[ ] AxesColors;
        Color PyramidColor = Color.White;
        Color LightColor = Color.Red;

        Color BackColor = new Color(1f, 1f, 1f);

        public MainThread( ) {
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnWindowResized;
            AxesColors = new Color[ ] {
                new Color(1f, AxesLight, AxesLight),
                new Color(AxesLight, 1f, AxesLight),
                new Color(AxesLight, AxesLight, 1f),
            };
            {
                float baseSize = 3f;
                float baseHeight = -2f;
                float baseDx = baseSize * (float)Math.Cos(MathHelper.ToRadians(30f));
                float baseDy = baseSize / 2f;
                float topSize = 1f;
                float topHeight = 2f;
                float topDx = topSize * (float)Math.Cos(MathHelper.ToRadians(30f));
                float topDy = topSize / 2f;
                Points = new VertexPositionColor[ ] {
                    new VertexPositionColor(new Vector3(0f, baseHeight, -baseSize), PyramidColor),
                    new VertexPositionColor(new Vector3(baseDx, baseHeight, baseDy), PyramidColor),
                    new VertexPositionColor(new Vector3(-baseDx, baseHeight, baseDy), PyramidColor),
                    new VertexPositionColor(new Vector3(0f, topHeight, -topSize), PyramidColor),
                    new VertexPositionColor(new Vector3(topDx, topHeight, topDy), PyramidColor),
                    new VertexPositionColor(new Vector3(-topDx, topHeight, topDy), PyramidColor),
                };
            }
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreparingDeviceSettings += SetMultiSampling;
            //Graphics.PreferMultiSampling = true;
            Window.Title = "CGaG Lab 5 by NickLatkovich";
            base.IsMouseVisible = true;
            Content.RootDirectory = "Content";

        }

        private void OnWindowResized(Object sender, EventArgs e) {
            Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            Graphics.ApplyChanges( );
        }

        private void SetMultiSampling(Object sender, PreparingDeviceSettingsEventArgs e) {
            e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 4;
        }

        protected override void Initialize( ) {
            // TODO: Initialization logic
            Effect = new BasicEffect(Graphics.GraphicsDevice);
            Effect.World = Matrix.Identity;
            Effect.VertexColorEnabled = true;

            base.Initialize( );
        }

        protected override void LoadContent( ) {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: Use this.Content to load game content
        }

        protected override void UnloadContent( ) {
            // TODO: Unload any non ContentManager content
        }

        protected override void Update(GameTime Time) {

            keyboard = Keyboard.GetState( );
            if (keyboardPrev == null) {
                keyboardPrev = keyboard;
            }

            if (keyboard.IsKeyDown(Keys.Escape)) {
                Exit( );
            }

            // TODO: Update logic

            SphereCameraPosition.Y +=
                (keyboard.IsKeyDown(Keys.Left) ? 1 : 0) -
                (keyboard.IsKeyDown(Keys.Right) ? 1 : 0);
            SphereCameraPosition.Z +=
                (keyboard.IsKeyDown(Keys.Up) ? 1 : 0) -
                (keyboard.IsKeyDown(Keys.Down) ? 1 : 0);
            SimpleUtils.Median(ref SphereCameraPosition.Z, -89f, 89f);
            SphereLightPosition.Y +=
                (keyboard.IsKeyDown(Keys.A) ? 1 : 0) -
                (keyboard.IsKeyDown(Keys.D) ? 1 : 0);
            SphereLightPosition.Z +=
                (keyboard.IsKeyDown(Keys.W) ? 1 : 0) -
                (keyboard.IsKeyDown(Keys.S) ? 1 : 0);
            Effect.View = Matrix.CreateLookAt(SphereCameraPosition.SphereToCart( ), Vector3.Zero, Vector3.Up);
            if (Graphics.PreferredBackBufferWidth > Graphics.PreferredBackBufferHeight) {
                Effect.Projection = Matrix.CreateOrthographic(SphereCameraPosition.X * Graphics.PreferredBackBufferWidth / Graphics.PreferredBackBufferHeight, SphereCameraPosition.X, 0.1f, 100.0f);
            } else {
                Effect.Projection = Matrix.CreateOrthographic(SphereCameraPosition.X, SphereCameraPosition.X * Graphics.PreferredBackBufferHeight / Graphics.PreferredBackBufferWidth, 0.1f, 100.0f);
            }

            keyboardPrev = keyboard;
            base.Update(Time);
        }

        protected override void Draw(GameTime Time) {
            GraphicsDevice.Clear(this.BackColor);

            // TODO: Drawing code
            RasterizerState rasterizerState1 = new RasterizerState( );
            rasterizerState1.CullMode = CullMode.None;
            Graphics.GraphicsDevice.RasterizerState = rasterizerState1;
            foreach (EffectPass pass in Effect.CurrentTechnique.Passes) {
                pass.Apply( );
                this.DrawPrimitive(new VertexPositionColor[6] {
                    new VertexPositionColor(new Vector3(-1024f, 0f, 0f), AxesColors[0]),
                    new VertexPositionColor(new Vector3(1024f, 0f, 0f), AxesColors[0]),
                    new VertexPositionColor(new Vector3(0f, 1024f, 0f), AxesColors[1]),
                    new VertexPositionColor(new Vector3(0f, -1024f, 0f), AxesColors[1]),
                    new VertexPositionColor(new Vector3(0f, 0f, 1024f), AxesColors[2]),
                    new VertexPositionColor(new Vector3(0f, 0f, -1024f), AxesColors[2]),
                }, PrimitiveType.LineList, new short[ ] {
                    0, 1, 2, 3, 4, 5,
                });
                bool[ ] facesVisible = new bool[Faces.Length];
                Color[ ] facesColors = new Color[Faces.Length];
                for (uint i = 0; i < Faces.Length; i++) {
                    Vector3 V1 = Points[Faces[i].Item2].Position - Points[Faces[i].Item1].Position;
                    Vector3 V2 = Points[Faces[i].Item3].Position - Points[Faces[i].Item1].Position;
                    Vector3 normal = Vector3.Cross(V2, V1);
                    Vector3 toCam = SimpleUtils.SphereToCart(SphereCameraPosition);
                    float angle = (float)Math.Abs(Math.Acos(Vector3.Dot(normal, toCam) / (normal.Length( ) * toCam.Length( ))));
                    if (facesVisible[i] = angle < MathHelper.ToRadians(90f)) {
                        Vector3 toLight = SimpleUtils.SphereToCart(SphereLightPosition);
                        float colorAngle = (float)Math.Abs(Math.Acos(Vector3.Dot(normal, toLight) / (normal.Length( ) * toLight.Length( ))));
                        float light = -2f / (float)Math.PI * colorAngle + 1f;
                        Vector3 pyramidColor = PyramidColor.ToVector3( );
                        Vector3 lightColor = LightColor.ToVector3( );
                        facesColors[i] = new Color(
                            pyramidColor.X * lightColor.X * light,
                            pyramidColor.Y * lightColor.Y * light,
                            pyramidColor.Z * lightColor.Z * light);
                        Points[Faces[i].Item1].Color = facesColors[i];
                        Points[Faces[i].Item2].Color = facesColors[i];
                        Points[Faces[i].Item3].Color = facesColors[i];
                        this.DrawPrimitive(new VertexPositionColor[ ] {
                            Points[Faces[i].Item1],
                            Points[Faces[i].Item2],
                            Points[Faces[i].Item3],
                        }, PrimitiveType.TriangleList, new short[ ] { 0, 1, 2 });
                    }
                }

                /*
                switch (Style) {
                case DrawingStyle.Lines:
                    this.DrawLineList(Points, Indices);
                    break;
                case DrawingStyle.Edges:
                    this.DrawLineList(Points, Indices, visibleLines);
                    break;
                case DrawingStyle.ColorBases:
                    this.DrawLineList(Points, Indices, visibleLines);
                    if (facesVisible[0]) {
                        this.DrawTriangle(Points[0].Position, Points[1].Position, Points[2].Position, Color.Red);
                    }
                    if (facesVisible[4]) {
                        this.DrawTriangle(Points[3].Position, Points[4].Position, Points[5].Position, Color.Blue);
                    }
                    break;
                }
                */
            }

            base.Draw(Time);
        }

    }
}

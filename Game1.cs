using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Eratosthenes
{
    /// <summary>
    /// This is an animation that will create an image of a sieve of eratosthenes up to a maximum value set
    /// by n
    /// </summary>
    public class Game1 : Game
    {
        // Change this to change how large the graph is.
        int n = 100;

        Texture2D circleTex;
        Texture2D pixel;
        SpriteFont ActionMan;
        List<int> PrimeList;
        List<Circle> CircleList;

        int screen_width = 1280;
        int screen_height = 720;
        int x_zero;
        int y_zero;
        int space;
        int x_loc;
        int y_loc;

        int r;
        int g;
        int b;
        int a;
        int colour_step = 64;

        string msg = "Press Space to Begin Animation";
        
        // For the logic to add circles to the list
        int current_diameter;
        int current_prime_index;
        int current_x_loc;
        int delay = 10;
        int current_delta;

        bool done = false;
        bool begin = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = screen_width;
            graphics.PreferredBackBufferHeight = screen_height;
            graphics.SynchronizeWithVerticalRetrace = true; 
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            x_zero = 20;
            y_zero = screen_height - 20;
            space = (screen_width - 20) / n;

            r = 0;
            g = 0;
            b = 255;
            a = 128;

            PrimeList = primeFinder();
            CircleList = new List<Circle>();

            current_prime_index = 0;
            current_x_loc = PrimeList[current_prime_index];
            current_diameter = PrimeList[current_prime_index];
            current_delta = 0;

            CircleList.Add(new Eratosthenes.Circle(current_diameter, current_x_loc, space, circleTex, new Color(r, g, b, a)));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            circleTex = this.Content.Load<Texture2D>("Semicircle");
            pixel = this.Content.Load<Texture2D>("Pixel");
            ActionMan = this.Content.Load<SpriteFont>("File");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space))
                begin = true;

            if (begin)
            {
                current_delta += 1;

                if (current_delta >= delay && !done)
                {
                    current_delta = 0;

                    current_x_loc += current_diameter;

                    if (current_x_loc > n + current_diameter)
                    {
                        current_prime_index++;

                        if (current_prime_index >= PrimeList.Count)
                        {
                            current_prime_index--;
                            done = true;
                        }

                        current_x_loc = PrimeList[current_prime_index];
                        current_diameter = PrimeList[current_prime_index];

                        if (b != 0 && r == 0)
                        {
                            b -= colour_step;
                            g += colour_step;

                            if (b < 0)
                                b = 0;
                            if (g > 255)
                                g = 255;
                        }
                        else if (g != 0 && b == 0)
                        {
                            g -= colour_step;
                            r += colour_step;

                            if (g < 0)
                                g = 0;
                            if (r > 255)
                                r = 255;
                        }
                        else
                        {
                            r -= colour_step;
                            b += colour_step;

                            if (r < 0)
                                r = 0;
                            if (b > 255)
                                b = 255;
                        }
                    }

                    if (!done)
                        CircleList.Add(new Circle(current_diameter, current_x_loc, space, circleTex, new Color(r, g, b, a)));
                }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (!begin)
            {
                Vector2 msgLocation = new Vector2();
                msgLocation.X = (screen_width / 2) - (ActionMan.MeasureString(msg).X / 2);
                msgLocation.Y = (screen_height / 2) - (ActionMan.MeasureString(msg).Y / 2);

                spriteBatch.DrawString(ActionMan, msg, msgLocation, Color.White);
            }


            else
            {
                // TODO: Draw the Circles
                CircleList.Reverse();
                foreach (Circle c in CircleList)
                {
                    c.DrawCircle(spriteBatch, x_zero, y_zero);
                }
                CircleList.Reverse();

                // Draw the Axes
                spriteBatch.Draw(pixel, new Rectangle(0, y_zero, screen_width, 1), Color.White);
                spriteBatch.Draw(pixel, new Rectangle(x_zero, 0, 1, screen_height), Color.White);


                // Draw the Dashes
                x_loc = x_zero;
                y_loc = y_zero;
                while (x_loc < screen_width)
                {
                    x_loc += space;
                    spriteBatch.Draw(pixel, new Rectangle(x_loc, y_zero - 2, 1, 4), Color.White);
                }
                while (y_loc > 0)
                {
                    y_loc -= space;
                    spriteBatch.Draw(pixel, new Rectangle(x_zero - 2, y_loc, 4, 1), Color.White);
                }

                // Nice Black Frame
                spriteBatch.Draw(pixel, new Rectangle(0, y_zero + 1, screen_width, 20), Color.Black);
                spriteBatch.Draw(pixel, new Rectangle(0, 0, 19, screen_height), Color.Black);
            }

            spriteBatch.End();

            base.Draw(gameTime);
            
        }

        public List<int> primeFinder()
        {
            int size = n;
            int num = 0;

            // Create a list of ints that will ONLY contain primes but initially contains every int up to n
            // Create an array for each element in initial list
            List<int> return_list = new List<int>();
            bool[] is_prime = new bool[size + 1];
            is_prime[0] = false;

            // Initialize the lists
            for (int i = 1; i <= size; i++)
            {
                return_list.Add(i);

                if (i == 1)
                    is_prime[i] = false;
                else
                    is_prime[i] = true;
            }

            while (num <= (int)Math.Ceiling(Math.Sqrt(size)))
            {
                if (!is_prime[num])
                {
                    num++;
                }
                else
                {
                    for (int i = 2 * num; i <= size; i += num)
                    {
                        is_prime[i] = false;
                    }

                    num++;
                }
            }

            for (int i = 0; i <= size; i++)
            {
                if (!is_prime[i])
                    return_list.Remove(i);
            }

            return return_list;
        }
    }
}

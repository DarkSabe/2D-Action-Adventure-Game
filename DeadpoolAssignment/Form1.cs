//Jaden Chang
//May 5, 2018
//Assignment 3

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace DeadpoolAssignment
{
    public partial class GameForm : Form
    {
        // Declares audio variable
        WMPLib.WindowsMediaPlayer playerOpening = new WMPLib.WindowsMediaPlayer();
        WMPLib.WindowsMediaPlayer playerRunning = new WMPLib.WindowsMediaPlayer();
        WMPLib.WindowsMediaPlayer playerProjectile = new WMPLib.WindowsMediaPlayer();
        WMPLib.WindowsMediaPlayer playerAttack = new WMPLib.WindowsMediaPlayer();
        WMPLib.WindowsMediaPlayer playerLose = new WMPLib.WindowsMediaPlayer();
        WMPLib.WindowsMediaPlayer playerWin = new WMPLib.WindowsMediaPlayer();

        // declares counters 
        int counter = 0;
        int weaponCounter = 0;
        int bossCounter = 0;

        int playerHealth = 300;
        int enemyHealth = 600;

        // timer interval controls how much time needs to pass before running the loop code
        int interval = 20;

        // state variable to exit loop
        int loopState = NOT_RUNNING;

        //state variable values
        const int NOT_RUNNING = 0, RUNNING = 1, FRONT = 2, BACK = 3, RIGHT = 4, LEFT = 5;
        const int WEAPON_UP = 6, WEAPON_DOWN = 7, WEAPON_RIGHT = 8, WEAPON_LEFT = 9, NO_WEAPON = 10;
        

        // the last time that the loop ran its code
        int lastRunTime;

        // sets the defult direction for player and weapon
        int direction = FRONT;
        int weaponDirection = NO_WEAPON;

        //Stores if player presses arrow keys
        bool moveUp = false, moveDown = false, moveRight = false, moveLeft = false;

        //Stores if player attacks enemy
        bool attack = false;
         
        //Stores if title screen is shown
        bool titlescreen = true;

        //Stores if lose and win screen is shown
        bool loseScreen = false, winScreen = false;

        // to set up "picture boxes"
        RectangleF playerBox;
        RectangleF weaponBox;
        RectangleF enemyBox;
        RectangleF projectileBox;

        // Projectile's speeds
        float projectileXSpeed, projectileYSpeed;
        const float TOTAL_SPEED = 10;

        // Temporary variables used for the projectile calculation
        float rise, run, hypotenuse;

        // Used to recreate a projectile
        bool isInAir = false; 

        // Stores text fonts and colours
        Font enemyFont = new Font("Arial", 13.0f);
        Font playerFont = new Font("Arial", 13.0f);

        //Stores the location of the enemy health terxt
        PointF enemyHealthLocation = new PointF(240, 20);


        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // makes the variable exit the loop
            loopState = NOT_RUNNING;
            // Plays music when the program loads
            playerOpening.controls.stop();
        }

        public GameForm()
        {
            InitializeComponent();
            

            // Identifies each sound file
            playerOpening.URL = "Opening.mp3";
            playerRunning.URL = "Running.mp3";
            playerProjectile.URL = "keemstar.mp3";
            playerAttack.URL = "noway.mp3";
            playerLose.URL = "headshot.mp3";
            playerWin.URL = "fusrodah.mp3";

            // Stops music when the program loads
            playerProjectile.controls.stop();
            playerWin.controls.stop();
            playerLose.controls.stop();
            playerAttack.controls.stop();
            playerRunning.controls.stop();
            // Plays music when the program loads
            playerOpening.controls.play();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (loopState == RUNNING)
            {
                // Draws the background
                e.Graphics.DrawImage(Properties.Resources.deadpoolbackground, 0, 0, ClientSize.Width, ClientSize.Height);

                // Checks if player box is on the left side of the boss
                if (playerBox.X < 180)
                {
                    //Determines which animation to use for the boss 
                    if (bossCounter < 20 && bossCounter >= 10)
                    {
                        //Changes boss picture
                        e.Graphics.DrawImage(Properties.Resources.bossleft, enemyBox);
                    }
                    //Determines which animation to use for the boss 
                    else if (bossCounter >= 0 && bossCounter < 10)
                    {
                        //Changes boss picture
                        e.Graphics.DrawImage(Properties.Resources.bossdownleft, enemyBox);
                    }
                }
                // Checks if player box is on the right side of the boss
                else
                {
                    //Determines which animation to use for the boss 
                    if (bossCounter < 20 && bossCounter >= 10)
                    {
                        //Changes boss picture
                        e.Graphics.DrawImage(Properties.Resources.bossright, enemyBox);
                    }
                    //Determines which animation to use for the boss 
                    else if (bossCounter >= 0 && bossCounter < 10)
                    {
                        //Changes boss picture
                        e.Graphics.DrawImage(Properties.Resources.bossdownright, enemyBox);
                    }
                }

                // Draws a fireball as the projectile
                e.Graphics.DrawImage(Properties.Resources.fireball, projectileBox);

                // Checks which direction is being used
                if (weaponDirection == WEAPON_UP)
                {
                    // draws the scythe facing up
                    e.Graphics.DrawImage(Properties.Resources.scytheup, weaponBox);
                }
                else if (weaponDirection == WEAPON_DOWN)
                {
                    // draws the scythe facing down
                    e.Graphics.DrawImage(Properties.Resources.scythedown, weaponBox);
                }
                else if (weaponDirection == WEAPON_RIGHT)
                {
                    // draws the scythe facing right
                    e.Graphics.DrawImage(Properties.Resources.scytheright, weaponBox);
                }
                else if (weaponDirection == WEAPON_LEFT)
                {
                    // draws the scythe facing left
                    e.Graphics.DrawImage(Properties.Resources.scytheleft, weaponBox);
                }


                if (direction == FRONT)
                {
                    if (counter == 20)
                    {
                        //sets counter to 1
                        counter = 1;
                        // Draws player walking down animation
                        e.Graphics.DrawImage(Properties.Resources.walkingdown2, playerBox);
                    }
                    else if (counter >= 10 && counter < 20)
                    {
                        // Draws player walking down animation
                        e.Graphics.DrawImage(Properties.Resources.walkingdown2, playerBox);
                    }
                    else if (counter > 0 && counter < 10)
                    {
                        // Draws player walking down animation
                        e.Graphics.DrawImage(Properties.Resources.walkingdown, playerBox);
                        // Plays a sounds as the player walks
                        playerRunning.controls.play();
                    }
                    else
                    {
                        // Draws player in defult stance
                        e.Graphics.DrawImage(Properties.Resources.downstance, playerBox);
                    }
                }


                if (direction == BACK)
                {
                    if (counter == 20)
                    {
                        //sets counter to 1
                        counter = 1;
                        // Draws player walking down animation
                        e.Graphics.DrawImage(Properties.Resources.walkingup2, playerBox);
                    }
                    else if (counter >= 10 && counter < 20)
                    {
                        // Draws player walking down animation
                        e.Graphics.DrawImage(Properties.Resources.walkingup2, playerBox);
                    }
                    else if (counter > 0 && counter < 10)
                    {
                        // Draws player walking down animation
                        e.Graphics.DrawImage(Properties.Resources.walkingup, playerBox);
                        // Plays a sounds as the player walks
                        playerRunning.controls.play();
                    }
                    else
                    {
                        // Draws player in defult stance
                        e.Graphics.DrawImage(Properties.Resources.upstance, playerBox);
                    }
                }

                if (direction == RIGHT)
                {
                    if (counter == 20)
                    {
                        //sets counter to 1
                        counter = 1;
                        // Draws player walking down animation
                        e.Graphics.DrawImage(Properties.Resources.walkingright2, playerBox);
                    }
                    else if (counter >= 10 && counter < 20)
                    {
                        // Draws player walking down animation
                        e.Graphics.DrawImage(Properties.Resources.walkingright2, playerBox);
                    }
                    else if (counter > 0 && counter < 10)
                    {
                        // Draws player walking down animation
                        e.Graphics.DrawImage(Properties.Resources.walkingright, playerBox);
                        // Plays a sounds as the player walks
                        playerRunning.controls.play();
                    }
                    else
                    {
                        // Draws player in defult stance
                        e.Graphics.DrawImage(Properties.Resources.rightstance, playerBox);
                    }
                }

                if (direction == LEFT)
                {
                    if (counter == 20)
                    {
                        //sets counter to 1
                        counter = 1;
                        // Draws player walking down animation
                        e.Graphics.DrawImage(Properties.Resources.walkingleft2, playerBox);
                    }
                    else if (counter >= 10 && counter < 20)
                    {
                        // Draws player walking down animation
                        e.Graphics.DrawImage(Properties.Resources.walkingleft2, playerBox);
                    }
                    else if (counter > 0 && counter < 10)
                    {
                        // Draws player walking down animation
                        e.Graphics.DrawImage(Properties.Resources.walkingleft, playerBox);
                        // Plays a sounds as the player walks
                        playerRunning.controls.play();
                    }
                    else
                    {
                        // Draws player in defult stance
                        e.Graphics.DrawImage(Properties.Resources.downstance, playerBox);
                    }
                }
                // draw the enemy's health
                e.Graphics.DrawString(enemyHealth.ToString(), enemyFont, Brushes.Red, enemyHealthLocation);
                // draw the player's health
                e.Graphics.DrawString(playerHealth.ToString(), playerFont, Brushes.Red, playerBox.X + 35, playerBox.Y - 5);

                if (winScreen == true)
                {
                    // Draws win screen
                    e.Graphics.DrawImage(Properties.Resources.winscreen, 0, 0, ClientSize.Width, ClientSize.Height);

                    // Plays win sounds
                    playerWin.controls.play();
                    //Stops all sounds
                    playerRunning.controls.stop();
                    playerProjectile.controls.stop();
                    playerLose.controls.stop();
                    playerAttack.controls.stop();
                    playerOpening.controls.stop();
                }
                else if (loseScreen == true)
                {
                    // Draws lose screen
                    e.Graphics.DrawImage(Properties.Resources.losescreen, 0, 0, ClientSize.Width, ClientSize.Height);
                    // Plays win sounds
                    playerLose.controls.play();
                    //Stops all sounds
                    playerWin.controls.stop();
                    playerRunning.controls.stop();
                    playerProjectile.controls.stop();
                    playerAttack.controls.stop();
                    playerOpening.controls.stop();
                }
            }
            else
            {
                if (titlescreen == true)
                {
                    // draw the background
                    e.Graphics.DrawImage(Properties.Resources.startscreen, 0, 0, ClientSize.Width, ClientSize.Height);
                }
            }



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GameSetup();
        }

        // setup the graphics geometry
        void GameSetup()
        {
            // setup the weapon 'picturebox'
            weaponBox = new RectangleF(715, 510, 50, 50);

            // setup the player 'picturebox'
            playerBox = new RectangleF(715, 510, 100, 100);

            // setup the enemy 'picturebox'
            enemyBox = new RectangleF(135, 5, 270, 270);

            // setup the projectile 'picturebox'
            projectileBox = new RectangleF(153, 3, 50, 50);
        }



        void CustomTimer()
        {
            // setup the timer
            loopState = RUNNING;
            // the lastRunTime is 'right now'
            lastRunTime = Environment.TickCount;

            // create a loop to do 'something' -- it counts up a label once per second
            while (loopState == RUNNING)
            {
                if (Environment.TickCount - lastRunTime >= interval)
                {

                    // updatee the last run time to 'right now'
                    lastRunTime = Environment.TickCount;

                    CreateProjectile();

                    MoveProjectile();

                    // boss counter counts up by 1
                    bossCounter = bossCounter + 1;

                    //Counter counts up by 1
                    counter = counter + 1;

                    // checks if player health or boss health is less than or equal to zero
                    if (enemyHealth <= 0)
                    {
                        winScreen = true;
                    }
                    else if (playerHealth <= 0)
                    {
                        loseScreen = true;

                    }

                    // Checks if weapon intersects with enemy and that the attack variable is ture
                    if (weaponBox.IntersectsWith(enemyBox) && attack == true)
                    {
                        //Subtracts 5 hp from the enemy's health
                        enemyHealth = enemyHealth - 5;
                        //plays a sounds when the enemy loses health
                        playerAttack.controls.play();
                    }

                    // Checks if projectile intersects with player 
                    if (projectileBox.IntersectsWith(playerBox))
                    {
                        //Subtracts 50 hp from the player's health
                        playerHealth = playerHealth - 50;
                        // Plays a sound when the player gets hit
                        playerProjectile.controls.play();
                    }

                    // Checks if the boss counter is equal to 20
                    if (bossCounter == 20)
                    {
                        //sets counter to 0
                        bossCounter = 0;
                    }

                    // Checks if there is no movement
                    if (moveUp == false && moveDown == false && moveRight == false && moveLeft == false)
                    {
                        //Sets counter to 0
                        counter = 0;
                    }

                    //When up is on, it decreases the player's y value
                    if (moveUp == true)
                    {
                        playerBox.Y = playerBox.Y - 10;
                        weaponBox.Y = weaponBox.Y - 10;
                    }
                    //When down is on, it increases the player's y value
                    if (moveDown == true)
                    {
                        playerBox.Y = playerBox.Y + 10;
                        weaponBox.Y = weaponBox.Y + 10;
                    }
                    //When up is on, it decreases the player's x value
                    if (moveLeft == true)
                    {
                        playerBox.X = playerBox.X - 10;
                        weaponBox.X = weaponBox.X - 10;
                    }
                    //When up is on, it increases the player's x value
                    if (moveRight == true)
                    {
                        playerBox.X = playerBox.X + 10;
                        weaponBox.X = weaponBox.X + 10;
                    }

                    //Checks if weapon is in the up direction
                    if (weaponDirection == WEAPON_UP) 
                    {
                            // Checks if weapon counter equals 0
                            if (weaponCounter == 0)
                            {
                                // weapon counter counts up by one
                                weaponCounter = weaponCounter + 1;
                                //Sets weapon location
                                weaponBox.Location = new PointF(playerBox.X, playerBox.Y + 40);
                            }
                            else if (weaponCounter == 10)
                            {
                            // sets weaponcounter to 0
                                weaponCounter = 0;
                            // Sets weapon location
                            weaponBox.Location = new PointF(playerBox.X, playerBox.Y - 40);
                            }
                    }
                    //Checks if weapon is in the down direction
                    if (weaponDirection == WEAPON_DOWN)
                    {

                        if (weaponCounter == 0)
                        {
                            // weapon counter counts up by one 
                            weaponCounter = weaponCounter + 1;
                            //Sets weapon location
                            weaponBox.Location = new PointF(playerBox.X, playerBox.Y - 40);
                        }
                        else if (weaponCounter == 10)
                        {
                            // sets weaponcounter to 0
                            weaponCounter = 0;
                            // Sets weapon location
                            weaponBox.Location = new PointF(playerBox.X, playerBox.Y + 40);
                        }
                    }
                    //Checks which direction for the weapon is facing
                    if (weaponDirection == WEAPON_UP)
                    {
                        //sets new weapon location
                        weaponBox = new RectangleF(playerBox.X, playerBox.Y - 20, 30, 80);
                    }
                    else if (weaponDirection == WEAPON_DOWN)
                    {
                        //sets new weapon location
                        weaponBox = new RectangleF(playerBox.X, playerBox.Y + 50, 30, 80);
                    }
                    else if (weaponDirection == WEAPON_RIGHT)
                    {
                        //sets new weapon location
                        weaponBox = new RectangleF(playerBox.X + 85, playerBox.Y + 40, 80, 30);
                    }
                    else if (weaponDirection == WEAPON_LEFT)
                    {
                        //sets new weapon location
                        weaponBox = new RectangleF(playerBox.X - 65, playerBox.Y + 40, 80, 30);
                    }
                    BoundaryDetection();
                    // Redraws the screen
                    Refresh();
                }

                // prevent freezing
                Application.DoEvents();
            }
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            //Checks if player presses enter
            if (e.KeyCode == Keys.Enter)
            {
                loopState = RUNNING;
                titlescreen = false;
                CustomTimer();
                CreateProjectile();
            }
            
            // Checks if player presses Up key
            if (e.KeyCode == Keys.Up)
            {
                moveUp = true;

                direction = BACK;
            }
            // Checks if player presses Down key
            else if (e.KeyCode == Keys.Down)
            {
                moveDown = true;

                direction = FRONT;
            }
            // Checks if player presses left key
            else if (e.KeyCode == Keys.Left)
            {
                moveLeft = true;

                direction = LEFT;
            }
            // Checks if player presses right key
            else if (e.KeyCode == Keys.Right)
            {
                moveRight = true;

                direction = RIGHT;
            }


            // Programing the WASD keys, W is up, A is left, S is down, D is right
            // Checks if player presses W
            if (e.KeyCode == Keys.W)
            {
                weaponDirection = WEAPON_UP;
                direction = BACK;
                attack = true;
            }
            // Checks if player presses S
            else if (e.KeyCode == Keys.S)
            {
                weaponDirection = WEAPON_DOWN;
                direction = FRONT;
                attack = true;
            }
            // Checks if player presses A
            else if (e.KeyCode == Keys.A)
            {
                weaponDirection = WEAPON_LEFT;
                direction = LEFT;
                attack = true;
            }
            // Checks if player presses D
            else if (e.KeyCode == Keys.D)
            {
                weaponDirection = WEAPON_RIGHT;
                direction = RIGHT;
                attack = true;
            }

        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            {
                // Programing the movement to stop when the user lets go of the keys

                // When the user lets go of the up key, it turns the up movement off
                if (e.KeyCode == Keys.Up)
                {
                    moveUp = false;
                    
                }
                // When the user lets go of the down key, it turns the down movement off
                else if (e.KeyCode == Keys.Down)
                {
                    moveDown = false;
                    
                }
                // When the user lets go of the left key, it turns the left movement off
                else if (e.KeyCode == Keys.Left)
                {
                    moveLeft = false;
                    
                }
                // When the user lets go of the right key, it turns the right movement off
                else if (e.KeyCode == Keys.Right)
                {
                    moveRight = false;
                }

                // Checks if player presses W
                if (e.KeyCode == Keys.W)
                {
                    weaponDirection = NO_WEAPON;
                    attack = false;
                }
                // Checks if player presses S
                else if (e.KeyCode == Keys.S)
                {
                    weaponDirection = NO_WEAPON;
                    attack = false;
                }
                // Checks if player presses A
                else if (e.KeyCode == Keys.A)
                {
                    weaponDirection = NO_WEAPON;
                    attack = false;
                }
                // Checks if player presses D
                else if (e.KeyCode == Keys.D)
                {
                    weaponDirection = NO_WEAPON;
                    attack = false;
                }
            }
        }

        // Stop the user from going past the left boundary
        void CheckLeftBoundary()
        {
            // stop the x value from being negative
            if (playerBox.X < 0)
            {
                playerBox.X = 0;

            }
        }

        void CheckTopBoundary()
        {
            // stop the y value from being negative
            if (playerBox.Y < 0)
            {
                playerBox.Y = 0;
            }
        }

        void CheckRightBoundary()
        {
            // stop the player's right x value from being bigger than the screen's width
            if (playerBox.X > this.ClientSize.Width - playerBox.Width)
            {
                playerBox.X = this.ClientSize.Width - playerBox.Width;
            }
        }

        void CheckBottomBoundary()
        {
            // stop the player's bottom y value from being bigger than the screen's height
            if (playerBox.Y > this.ClientSize.Height - playerBox.Height)
            {
                playerBox.Y = this.ClientSize.Height - playerBox.Height;
            }
        }

        // stops the user from going past any of the edges of the form
        void BoundaryDetection()
        {
            CheckLeftBoundary();
            CheckTopBoundary();
            CheckRightBoundary();
            CheckBottomBoundary();

            playerBox.Location = new PointF (playerBox.X, playerBox.Y);
        }

        // calcuatex the slope from the enemy to the player for the projectile's direction and speed
        void CreateProjectile()
        {
            if (isInAir == false)
            {
                // the projectile starts off at the enemy
                projectileBox.Location = new PointF(enemyBox.X + 180, enemyBox.Y + 50);

                // rise = y2 - y1
                rise = playerBox.Y - enemyBox.Y - 50;

                // run = x2 - x1
                run = playerBox.X - enemyBox.X - 130;

                // c^2 = a^2 + b^2
                // need to convert the Math.Sqrt into a float
                hypotenuse = (float)Math.Sqrt(Math.Pow(rise, 2) + Math.Pow(run, 2));

                // calculate the projectile's x and y speed coordinates
                projectileYSpeed = rise / hypotenuse * TOTAL_SPEED;
                projectileXSpeed = run / hypotenuse * TOTAL_SPEED;

                isInAir = true;
            }
        }

        // Move the projectile according to its calculated speeds
        void MoveProjectile()
        {
            // calculate the new x and y alocations for the projectile - original location + speed
            int x = (int)(projectileBox.Location.X + projectileXSpeed);
            int y = (int)(projectileBox.Location.Y + projectileYSpeed);

            projectileBox.Location = new Point(x, y);

            // if the projectiele hits the player or the edge of the client size, create  a new one
            if (projectileBox.IntersectsWith(playerBox) == true || projectileBox.X > ClientSize.Width || projectileBox.Y > ClientSize.Height || projectileBox.X < 0 || projectileBox.Y < 0)
            {
                isInAir = false; 
            }
        }
    }
}

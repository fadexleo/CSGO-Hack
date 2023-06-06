using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ezOverLay;
using hazedumper;
using Swed32;
using static System.Windows.Forms.DataFormats;

namespace CSGO_Aimbot
{
    public partial class Form2 : Form
    {
        const int localplayer = signatures.dwLocalPlayer;
        const int entitylist = signatures.dwEntityList;
        //const int glowobjectmanager = ;
        const int viewmatrix = signatures.dwViewMatrix;
        const int xyz = netvars.m_vecOrigin; //m_vecOrigin on hazedumper
        const int team = netvars.m_iTeamNum;
        const int dormant = signatures.m_bDormant;
        const int health = netvars.m_iHealth;

        Pen FriendlyPen = new Pen(Color.Blue, 1);
        Pen EnemyPen = new Pen(Color.White, 1);

        ez ez = new ez();
        Swed swed;

        entity player = new entity();
        public List<entity> list = new List<entity>();

        IntPtr client;

        public static Form2 instance;

        public bool espboxesp;


        public Form2()
        {
            InitializeComponent();
            instance = this;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            swed = new Swed("csgo");
            client = swed.GetModuleBase("client.dll");

            ez.SetInvi(this);
            ez.DoStuff("Counter-Strike: Global Offensive - Direct3D 9", this);

            Thread thread = new Thread(main) { IsBackground = true };
            thread.Start();

        }

        void main()
        {
            while (true)
            {
                updatelocal();
                updateentities();
                panel1.Refresh();
                Thread.Sleep(1);
            }
        }

        void updatelocal()
        {
            // get current team
            var buffer = swed.ReadPointer(client, localplayer);
            player.team = BitConverter.ToInt32(swed.ReadBytes(buffer, team, 4));

            // get _
        }

        void updateentities()
        {
            list.Clear(); // empty the current entitylist

            for (int i = 0; i < 32; i++)
            {
                var buffer = swed.ReadPointer(client, entitylist + i * 0x10); // reads from current entity
                var entityteam = BitConverter.ToInt32(swed.ReadBytes(buffer, team, 4), 0);
                var entitydormant = BitConverter.ToInt32(swed.ReadBytes(buffer, dormant, 4), 0);
                var entityhealth = BitConverter.ToInt32(swed.ReadBytes(buffer, health, 4), 0);

                HealthPen(entityhealth);


                // check if enemy is dead
                if (entityhealth < 2 || entitydormant != 0)
                    continue;
                // if still alive, do the other things

                var coords = swed.ReadBytes(buffer, xyz, 12);

                var ent = new entity
                {
                    x = BitConverter.ToSingle(coords, 0),
                    y = BitConverter.ToSingle(coords, 4),
                    z = BitConverter.ToSingle(coords, 8),
                    team = entityteam,
                    health = entityhealth
                };

                ent.bot = WorldToScreen(readmatrix(), ent.x, ent.y, ent.z, Width, Height);

                ent.top = WorldToScreen(readmatrix(), ent.x, ent.y, ent.z + 58, Width, Height);


                list.Add(ent);
            }
        }

        viewmatrix readmatrix()
        {
            var matrix = new viewmatrix();

            var buffer = new byte[16 * 4];

            buffer = swed.ReadBytes(client, viewmatrix, buffer.Length);


            //replacing the matrix properties
            matrix.m11 = BitConverter.ToSingle(buffer, 0 * 4);
            matrix.m12 = BitConverter.ToSingle(buffer, 1 * 4);
            matrix.m13 = BitConverter.ToSingle(buffer, 2 * 4);
            matrix.m14 = BitConverter.ToSingle(buffer, 3 * 4);

            matrix.m21 = BitConverter.ToSingle(buffer, 4 * 4);
            matrix.m22 = BitConverter.ToSingle(buffer, 5 * 4);
            matrix.m23 = BitConverter.ToSingle(buffer, 6 * 4);
            matrix.m24 = BitConverter.ToSingle(buffer, 7 * 4);

            matrix.m31 = BitConverter.ToSingle(buffer, 8 * 4);
            matrix.m32 = BitConverter.ToSingle(buffer, 9 * 4);
            matrix.m33 = BitConverter.ToSingle(buffer, 10 * 4);
            matrix.m34 = BitConverter.ToSingle(buffer, 11 * 4);

            matrix.m41 = BitConverter.ToSingle(buffer, 12 * 4);
            matrix.m42 = BitConverter.ToSingle(buffer, 13 * 4);
            matrix.m43 = BitConverter.ToSingle(buffer, 14 * 4);
            matrix.m44 = BitConverter.ToSingle(buffer, 15 * 4);
            return matrix;

        }

        Point WorldToScreen(viewmatrix mtx, float x, float y, float z, int width, int height)
        {
            var twoD = new Point();

            float screenW = (mtx.m41 * x) + (mtx.m42 * y) + (mtx.m43 * z) + mtx.m44;
            if (screenW > 0.001f)
            {
                float screenX = (mtx.m11 * x) + (mtx.m12 * y) + (mtx.m13 * z) + mtx.m14;
                float screenY = (mtx.m21 * x) + (mtx.m22 * y) + (mtx.m23 * z) + mtx.m24;

                float camX = width / 2f;
                float camY = height / 2f;


                float X = camX + (camX * screenX / screenW);

                float Y = camY - (camY * screenY / screenW);

                twoD.X = (int)X;
                twoD.Y = (int)Y;
            }
            else
            {
                return new Point(-99, -99); // new point outside of bounds, not drawn
            }


            return twoD;
        }

        Pen HealthPen(int hp)
        {
            if (hp >= 100)
                return new Pen(Color.FromArgb(16, 255, 0), 1);
            else if (hp > 60)
                return new Pen(Color.FromArgb(64, 204, 0), 1);
            else if (hp > 40)
                return new Pen(Color.FromArgb(112, 153, 0), 1);
            else if (hp > 20)
                return new Pen(Color.FromArgb(159, 102, 0), 1);
            else if (hp > 1)
                return new Pen(Color.FromArgb(207, 51, 0), 1);
            else if (hp == 1)
                return new Pen(Color.FromArgb(255, 0, 0), 1);
            return new Pen(Color.Black, 1);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (list.Count > 0)
            {

                try
                {
                    // for each entity in entitylist, draw a a colored rectangle at the position of the entity (depending on team for color)
                    foreach (var ent in list)
                    {
                        if (Form1.instance.boxespcheck.Checked && Form1.instance.teamespcheck.Checked && Form1.instance.tracersespcheck.Checked && ent.team == player.team && ent.bot.X > 0 && ent.bot.X < Width && ent.bot.Y > 0 && ent.bot.Y < Height)
                        {
                            g.DrawRectangle(FriendlyPen, ent.rect()); // team esp + tracers
                            g.DrawLine(FriendlyPen, Width / 2, Height, ent.bot.X, ent.bot.Y);
                        }
                        else if (Form1.instance.boxespcheck.Checked && Form1.instance.tracersespcheck.Checked && ent.team != player.team && ent.bot.X > 0 && ent.bot.X < Width && ent.bot.Y > 0 && ent.bot.Y < Height)
                        {
                            g.DrawRectangle(HealthPen(ent.health), ent.rect()); // enemy esp + tracers
                            g.DrawLine(EnemyPen, Width / 2, Height, ent.bot.X, ent.bot.Y);
                        } 
                        else if (Form1.instance.boxespcheck.Checked && Form1.instance.teamespcheck.Checked && ent.team == player.team && ent.bot.X > 0 && ent.bot.X < Width && ent.bot.Y > 0 && ent.bot.Y < Height)
                        {
                            g.DrawRectangle(FriendlyPen, ent.rect()); // team esp
                        }
                        else if (Form1.instance.boxespcheck.Checked && ent.team != player.team && ent.bot.X > 0 && ent.bot.X < Width && ent.bot.Y > 0 && ent.bot.Y < Height)
                        {
                            g.DrawRectangle(HealthPen(ent.health), ent.rect()); // enemy esp
                        }
                        else if (Form1.instance.teamespcheck.Checked && Form1.instance.tracersespcheck.Checked && ent.team == player.team && ent.bot.X > 0 && ent.bot.X < Width && ent.bot.Y > 0 && ent.bot.Y < Height)
                        {
                            g.DrawLine(FriendlyPen, Width / 2, Height, ent.bot.X, ent.bot.Y); // team tracers
                        }
                        else if (Form1.instance.tracersespcheck.Checked && ent.team != player.team && ent.bot.X > 0 && ent.bot.X < Width && ent.bot.Y > 0 && ent.bot.Y < Height)
                        {
                            g.DrawLine(EnemyPen, Width / 2, Height, ent.bot.X, ent.bot.Y); // enemy tracers
                        }
                    }

                }
                catch { }
            }
        }
  
    }
}

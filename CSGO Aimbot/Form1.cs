using System;
using Swed32;
using System.Runtime.InteropServices;
using hazedumper;
using System.Numerics;
using System.Resources;

namespace CSGO_Aimbot
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        extern static short GetAsyncKeyState(int vKey); // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=windowsdesktop-6.0

        public static Form1 instance;
        public CheckBox boxespcheck, teamespcheck, tracersespcheck, skelespcheck;

        public Form1()
        {
            InitializeComponent();
            instance = this;
            boxespcheck = boxespbox;
            teamespcheck = teamespbox;
            tracersespcheck = tracersespbox;
            skelespcheck = skelespbox;
        }

        Swed swed;
        IntPtr clientModule, engineModule;
        entity player;
        int targetBone = 8;
        bool vischeck = false;
        int hotkey = 0x1;
        public List<entity> entities;

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            try
            {
                swed = new Swed("csgo");
            }
            catch (Exception)
            {
                MessageBox.Show("Open CS:GO first.", "Error");
                Environment.Exit(0);
            }

            clientModule = swed.GetModuleBase("client.dll");
            engineModule = swed.GetModuleBase("engine.dll");
            player = new entity(); // localplayer
            entities = new List<entity>(); // all other players (entities)

            Thread aimbot1 = new Thread(main) { IsBackground = true };
            Thread bhop1 = new Thread(bhop) { IsBackground = true };
            aimbot1.Start();
            bhop1.Start();
            new Form2().Show();

        }

        void bhop()
        {
            while (true)
            {
                if (GetAsyncKeyState(0x20) < 0 && bunnyhopbox.Checked)
                {
                    var buffer = swed.ReadPointer(clientModule, signatures.dwLocalPlayer);
                    var flag = swed.ReadInt(buffer, netvars.m_fFlags);

                    if (flag == 257 || flag == 263)
                    {
                        swed.WriteBytes(clientModule, signatures.dwForceJump, BitConverter.GetBytes(5));
                    }
                    else
                    {
                        swed.WriteBytes(clientModule, signatures.dwForceJump, BitConverter.GetBytes(4));
                    }
                }

                Thread.Sleep(1);
            }
        }

        void UpdateLocalPlayer()
        {
            var entPointer = swed.ReadPointer(clientModule, signatures.dwLocalPlayer); // get player pointer
            var coords = swed.ReadBytes(entPointer, netvars.m_vecOrigin, 12);
            var viewVector = BitConverter.ToSingle(swed.ReadBytes(entPointer, netvars.m_vecViewOffset + 0x8, 4), 0);

            player.feet.X = BitConverter.ToSingle(coords, 0);
            player.feet.Y = BitConverter.ToSingle(coords, 4);
            player.feet.Z = BitConverter.ToSingle(coords, 8);

            player.team = BitConverter.ToInt32(swed.ReadBytes(entPointer, netvars.m_iTeamNum, 4), 0);
            player.feet.Z += viewVector; // add view vector

        }

        void UpdateEntities()
        {
            entities.Clear();

            for (int i = 1; i < 32; i++) // loop through entity list
            {
                var entPointer = swed.ReadPointer(clientModule, signatures.dwEntityList + i * 0x10); // current entity pointer

                var team = BitConverter.ToInt32(swed.ReadBytes(entPointer, netvars.m_iTeamNum, 4), 0);
                var dormant = BitConverter.ToInt32(swed.ReadBytes(entPointer, signatures.m_bDormant, 4), 0);
                var hp = BitConverter.ToInt32(swed.ReadBytes(entPointer, netvars.m_iHealth, 4), 0);
                var visible = BitConverter.ToInt32(swed.ReadBytes(entPointer, netvars.m_bSpotted, 4), 0);

                if (hp < 2 || dormant != 0) // filter out dead & dormant ones
                    continue;

                var ent = new entity // new instance in our list
                {
                    head = RecieveHead(entPointer),
                    team = team,
                    health = hp,
                    visible = visible
                };

                ent.mag = CalcMag(player.feet, ent.head); // calc dist from player to nearest enemy

                entities.Add(ent);
            }
        }

        float CalcMag(Vector3 player, Vector3 enemy)
        {
            return (float)(Math.Sqrt(
                Math.Pow(enemy.X - player.X, 2) +
                Math.Pow(enemy.Y - player.Y, 2) +
                Math.Pow(enemy.Z - player.Z, 2)
                ));
        }

        Vector3 RecieveHead(IntPtr entPointer)
        {
            var bones = swed.ReadPointer(entPointer, netvars.m_dwBoneMatrix);
            var bone = swed.ReadBytes(bones, 0x30 * targetBone, 0x30); // bone 8 = head

            return new Vector3
            {
                X = BitConverter.ToSingle(bone, 0xC),
                Y = BitConverter.ToSingle(bone, 0x1C),
                Z = BitConverter.ToSingle(bone, 0x2C)

            };
        }

        void Aim(entity ent)
        {
            // x

            float deltaX = ent.head.X - player.feet.X;
            float deltaY = ent.head.Y - player.feet.Y;
            float X = (float)(Math.Atan2(deltaY, deltaX) * 180 / Math.PI);

            // y

            float deltaZ = ent.head.Z - player.feet.Z;

            double dist = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

            float Y = -(float)(Math.Atan2(deltaZ, dist) * 180 / Math.PI); // math.atan2(deltaZ, dist) -> degrees

            var buffer = swed.ReadPointer(engineModule, signatures.dwClientState);
            var localBuffer = swed.ReadPointer(clientModule, signatures.dwLocalPlayer);

            float cX = BitConverter.ToSingle(swed.ReadBytes(buffer, signatures.dwClientState_ViewAngles + 0x4, 4), 0); // current angle
            float cY = BitConverter.ToSingle(swed.ReadBytes(buffer, signatures.dwClientState_ViewAngles, 4), 0);

            float rY = 2 * BitConverter.ToSingle(swed.ReadBytes(localBuffer, netvars.m_aimPunchAngle, 4), 0); // recoil
            float rX = 2 * BitConverter.ToSingle(swed.ReadBytes(localBuffer, netvars.m_aimPunchAngle + 0x4, 4), 0);

            X -= rX; // aimbot angle - recoil angle
            Y -= rY;

            float dX = X - cX; // distance between current angle and target angle
            float dY = Y - cY;

            dist = Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));

            float sX = cX + (dX / (float)smoothing.Value); // f angle (smoothed)
            float sY = cY + (dY / (float)smoothing.Value);

            if (dist < (double)aimbotfovd.Value)
            {
                swed.WriteBytes(buffer, signatures.dwClientState_ViewAngles, BitConverter.GetBytes(sY)); // smoothed angle
                swed.WriteBytes(buffer, signatures.dwClientState_ViewAngles + 0x4, BitConverter.GetBytes(sX));
            }





        }
        void main()
        {
            while (true)
            {
                aimbot();
                Thread.Sleep(1);
            }
        }

        void aimbot()
        {

            if (aimbot_checkbox.Checked)
            {
                UpdateLocalPlayer();
                UpdateEntities();

                entities = entities.OrderBy(o => o.mag).ToList();


                if (entities.Count > 0 && GetAsyncKeyState(hotkey) < 0)
                {
                    foreach (var ent in entities.ToList())
                    {
                        if (ent.team == player.team && team_checkbox.Checked)
                        {
                            entities.Remove(ent);
                        }
                        else if (ent.visible == 0 && vischeck == true)
                        {
                            entities.Remove(ent);
                        }
                    }

                    if (entities.Count > 0)
                        Aim(entities[0]);
                }


            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (targetBoneBox.Text == "head")
            { targetBone = 8; }
            else if (targetBoneBox.Text == "neck")
            { targetBone = 7; }
            else if (targetBoneBox.Text == "chest")
            { targetBone = 6; }
            else if (targetBoneBox.Text == "stomach")
            { targetBone = 4; }
            else if (targetBoneBox.Text == "pelvis")
            { targetBone = 0; }
        }

        private void visible_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (visible_checkbox.Checked)
            {
                vischeck = true;
            }
            else
            {
                vischeck = false;
            }
        }

        private void keybox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (keybox.Text == "mouse1")
            { hotkey = 0x1; }
            else if (keybox.Text == "mouse2")
            { hotkey = 0x2; }
            else if (keybox.Text == "mouse3")
            { hotkey = 0x4; }
            else if (keybox.Text == "mouse4")
            { hotkey = 0x5; }
            else if (keybox.Text == "mouse5")
            { hotkey = 0x6; }
            else if (keybox.Text == "shift")
            { hotkey = 0x10; }
        }

        private void boxespbox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void teamespbox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void skelespbox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tracersespbox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }


}
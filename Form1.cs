using DarkSoulsScripting;
using System.Collections;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Metadata;

namespace DSR_BossRush
{

    public partial class Form1 : Form
    {
        Process DS = new Process();

        EzDrawHook.Box? titlebox = null;
        EzDrawHook.Text? titletext = null;
        Vector2 offScreen = new Vector2(9999, 9999);

        List<boss> bosses = null;




        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            bosses = boss.getBosses();

            foreach (boss b in bosses)
            {
                cmbBoss.Items.Add(b.Name);
            }
        }



        void SetNoLogo()
        {
            //output($"Setting NoLogo\n");
            //ProcessLogoCurrentState function cmp
            Hook.WByte(0x14070f269, 1);  //DSR1310
        }
        void WaitFrpgSysInit()
        {
            while ((Hook.RIntPtr(FrpgSystem.Address) == IntPtr.Zero) && (Hook.RInt32(0x140000000) == 0x905a4d)) { }
        }
        void NukeServerNames()
        {
            //output($"Nuking FromSoft server names.\n");
            //dsr1310
            Hook.WAsciiStr(0x141403a58, "tcp://nope.nope\0");
            Hook.WAsciiStr(0x141403a90, "tcp://nope.nope\0");
            Hook.WAsciiStr(0x141403ac8, "tcp://nope.nope\0");
            Hook.WAsciiStr(0x141403b00, "tcp://nope.nope\0");
            Hook.WAsciiStr(0x141403b38, "tcp://nope.nope\0");
            Hook.WAsciiStr(0x141403b70, "tcp://nope.nope\0");
            Hook.WAsciiStr(0x141403ba8, "tcp://nope.nope\0");
            Hook.WAsciiStr(0x141403be0, "tcp://nope.nope\0");
            Hook.WAsciiStr(0x141403c18, "tcp://nope.nope\0");
        }

        void Launch()
        {
            //If the path doesn't exist in the registry, this will fail.
            //Of course, that's probably only going to happen with pirated copies.
            //No judgment, but they can troubleshoot themselves.
            string currDir = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 570940", "InstallLocation", null).ToString();
            string Application = $"{currDir}\\DarkSoulsRemastered.exe";



            //Create the steam_appid.txt file, which lets the EXE launch directly instead of needing Steam to trigger it.
            try
            {
                System.IO.File.WriteAllText($@"{currDir}\steam_appid.txt", "570940");
            }
            catch { };


            DS.StartInfo.FileName = Application;
            DS.StartInfo.RedirectStandardError = true;
            DS.StartInfo.RedirectStandardOutput = true;
            DS.StartInfo.UseShellExecute = false;
            DS.StartInfo.WorkingDirectory = currDir;

            DS.Start();


            //Launch and hook.
            Hook.DARKSOULS.TryAttachToDarkSouls(DS.Id);
        }

        private void btnLaunchDSR_Click(object sender, EventArgs e)
        {
            //Do all the launchy stuff in its own thread, so the form's UI doesn't lock up.
            new Thread(LaunchThread).Start();
        }



        public int gameMode = 0;
        public bool gameStarted = false;
        public bool brStarted = false;


        void LaunchThread()
        {
            Launch();



            //PrototypeSelect instead of normal TitleStep.
            Hook.WBytes(0x140278ca6, new byte[] { 0xE8, 0x65, 0x2D, 0xFE, 0xFF });

            //SetNoLogo shouldn't matter here, but what the heck, disable them anyway.
            SetNoLogo();
            //Prevent DSR from knowing the FromSoft server names, avoid communicating any data.
            NukeServerNames();

            //Wait until things have initialized enough for the important structures to exist.
            WaitFrpgSysInit();
            EzDrawHook.Hook3();

            new Thread(titleThread).Start();

            //Lets load our custom textures.
            Hook.WUnicodeStr(0x1413fda40, "other:/BossRush.tpf");
            IntPtr tpf = IngameFuncs.LoadTpfFileCap(0x1413fda40);
            Thread.Sleep(33);

            //PrototypeSelectStep   [[[[[141b68e18]+8]+20]+58]+20]
            //IngameStep            [[[[[[141b68e18]+8]+20]+58]+20]+88]
            //MoveMapStep           [[[[[[[141b68e18]+8]+20]+58]+20]+88]+20]


            //Overwrite the texture names that the normal PrototypeSelect step uses, to the ones in our TPF.
            Hook.WUnicodeStr(0x1412f6030, "brush_title");
            Hook.WUnicodeStr(0x1412f6050, "brush_press_start");
            Hook.WUnicodeStr(0x1412f6050, "brush_press_start");
            Hook.WUnicodeStr(0x1412f6078, "brush_select_character");
            Hook.WUnicodeStr(0x1412f60a8, "brush_select_character_%02d");


            



            //Fix title_prototype pos and height
            //Without this, the textures don't scale to window size.
            Vector2 scrRatio = FrpgWindow.DisplaySize / new Vector2(1920, 1080);
            Hook.WFloat(0x1412f6138, HgMan.ScreenHeight);
            Hook.WFloat(0x1412e1574, HgMan.ScreenWidth);
            Hook.WFloat(0x1412f6150, HgMan.ScreenWidth / 2);
            Hook.WFloat(0x1412f6154, HgMan.ScreenHeight / 2);


            //press start, width 0x1412f6128
            //height, 0x1412b2330
            //pos, 0x1412f6160
            Hook.WFloat(0x1412f6160, 960 * scrRatio.X);
            Hook.WFloat(0x1412f6164, 792 * scrRatio.Y);



            //After pressing start, jump to Step 9 instead of 12
            //This gives us both the basic vertical menu, and the horizontal char select.
            Hook.WByte(0x14025cae2, 9);

            //After step 10, go to 12 instead of 15.
            Hook.WByte(0x14025cd0d, 12);

            //0x1412f6120 = Vertical select menu, text x offset
            Hook.WFloat(0x1412f6120, 30 * scrRatio.X);
            //0x1412f6130 = Vertical select menu, text y pos
            Hook.WFloat(0x1412f6130, 560 * scrRatio.Y);
            //0x1412f612c = Vertical select menu, box width
            Hook.WFloat(0x1412f612c, 280 * scrRatio.X);
            //0x14025C0BB = Vertical select menu, box y pos
            Hook.WFloat(0x14025c0bb, 500 * scrRatio.Y);
            Hook.WUnicodeStr(0x1412f5c00, "Boss Rush");
            Hook.WUnicodeStr(0x1412f5c28, "Story");
            Hook.WUnicodeStr(0x1412f5c50, "SURVIVE");
            Hook.WUnicodeStr(0x1412f5c70, "");
            Hook.WUnicodeStr(0x1412f5ca8, "");
            Hook.WUnicodeStr(0x1412f5cf0, "");

            



            //Further Scaling correction
            //Selected Char, x offset per char = 1412f6124 (192)
            //Selected Char, width = 1412F6128 (384)
            //Selected Char, xy offset of grid start = 0x1412f6140 (192, 384)
            Hook.WFloat(0x1412e86f8, 32 * scrRatio.X); //Addition to x offset per char
            Hook.WFloat(0x14025c5ef, 192 * scrRatio.Y); //Addition to y offset per char
            Hook.WFloat(0x1412f6124, 192 * scrRatio.X); //x offset per char
            Hook.WFloat(0x1412f6128, 384 * scrRatio.X); //char width
            Hook.WFloat(0x1412f6134, 768 * scrRatio.Y); //char height
            Hook.WFloat(0x1412f6140, 192 * scrRatio.X);
            Hook.WFloat(0x1412f6144, 384 * scrRatio.Y);







        }


        void titleThread()
        {
            Boolean cont = true;
            int sleepDur = 5;

            while (cont)
            {
                if (FrpgSystem.Title.Step == 16)
                {
                    sleepDur = 500;
                    if (!gameStarted)
                    {
                        switch (gameMode)
                        {
                            case 0:
                                gameStarted = true;
                                if (!brStarted)
                                {
                                    new Thread(brThread).Start();
                                    brStarted = true;
                                }
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            default:
                                break;
                        }
                    }
                } 
                else {
                    sleepDur = 10;
                    gameStarted = false;
                }

                if (FrpgSystem.Title.Step == 10)
                {
                    gameMode = Hook.RInt32(FrpgSystem.Title.Address + 0x44);
                    if (gameMode > 2)
                    {
                        gameMode = 0;
                        Hook.WInt32(FrpgSystem.Title.Address + 0x44, 0);
                    }
                }
                if (FrpgSystem.Title.Step == 12)
                {
                    FadeMan.A = 0;
                }


                if (Hook.RUInt32(0x140000000) == 0) { cont = false; };
                Thread.Sleep(sleepDur);
            }//end while cont
        }//end titleThread

        void brThread()
        {
            Boolean cont = true;
            int sleepDur = 5;
            bool fighting = false;
            int bossNum = 0;



            while (cont)
            {
                if (FrpgSystem.InGame.Step == 7)
                {
                    if ((WorldChrMan.LocalPlayer.MaxHP > 0) && (WorldChrMan.LocalPlayer.HP < 1))
                    {
                        Thread.Sleep(1000);
                        GameMan.RequestToEnding = true;
                        cont = false;
                    }
                    if (!fighting)
                    {
                        if (MenuMan.LoadingState  == 0)
                        {
                            bosses[bossNum].Fight();
                            fighting = true;
                        }

                    }


                }


                if (Hook.RUInt32(0x140000000) == 0) { cont = false; };
                Thread.Sleep(sleepDur);
            }//end while cont
            brStarted = false;
        }//end brThread



        private void btnDSRDebug_Click(object sender, EventArgs e)
        {
            new Thread(LaunchDbg).Start();

        }

        void LaunchDbg()
        {
            //Launch();
            //Thread.Sleep(1000);

            //DbgMenuStep_New
            Hook.WBytes(0x140278ca6, new byte[] { 0xE9, 0x85, 0x67, 0xFE, 0xFF });

            //Jump to fix DbgMenuMan_DbgNode
            Hook.WBytes(0x140154253, new byte[] { 0xe9, 0xa8, 0x95, 0x04, 0x03 });
            //Fix DbgMenuMan_DbgNode
            Hook.WBytes(0x14319D800, new byte[] { 0x48, 0xb9, 0x60, 0x37, 0x4b, 0x41, 0x01, 0x00,
        0x00, 0x00, 0x48, 0xba, 0x60, 0x37, 0x4b, 0x41,
        0x01, 0x00, 0x00, 0x00, 0x48, 0xb8, 0x80, 0x65,
        0x47, 0x40, 0x01, 0x00, 0x00, 0x00, 0xff, 0xd0,
        0x48, 0xb9, 0xb8, 0x8c, 0xb6, 0x41, 0x01, 0x00, 0x00, 0x00,
        0x48, 0x8b, 0x09, 0x48, 0x83, 0xc1, 0x08, 0x48,
        0x89, 0x01, 0x48, 0x83, 0xc4, 0x28, 0xc3 });
            //temporary Nopping of frpgsystemdbg lookup
            Hook.WBytes(0x14015c957, new byte[] { 0x90, 0x90,0x90,0x90,0x90,0x90,0x90,0x90,
                0x90,0x90,0x90,0x90,0x90,0x90,0x90 });




        }

        private void btnLoadTpf_Click(object sender, EventArgs e)
        {
            //Hook.WUnicodeStr(0x1413fda40, "other:/BossRush.tpf");
            //IntPtr tpf = IngameFuncs.LoadTpfFileCap(0x1413fda40);
            //Thread.Sleep(66);



            Hook.WUnicodeStr(0x1413fda40, "brush_menubox");
            IntPtr tex = IngameFuncs.GetTexHdlResCap(0x1413fda40);
            Thread.Sleep(66);

            Vector2 scrRatio = FrpgWindow.DisplaySize / new Vector2(1280, 720);

            if (titlebox == null)
            {
                titlebox = new EzDrawHook.Box();
                Thread.Sleep(33);
            }
            if (titletext == null)
            {
                titletext = new EzDrawHook.Text();
                Thread.Sleep(33);
            }

            FrpgSystem.MoveMap.pause = 1;

            titlebox.Size = new Vector2(1920, 360) * scrRatio;
            titlebox.Pos = new Vector2(FrpgWindow.DisplaySize.X / 2, FrpgWindow.DisplaySize.Y * (float)0.66 + titlebox.Size.Y / 2) - titlebox.Size / 2;
            titlebox.Color1 = Color.White;
            titlebox.Color2 = Color.White;
            titlebox.IgnoreCulling = true;
            titlebox.TexHandle = Hook.RUInt32(tex + 0x28);
            titlebox.Flags = 0x37;


            titletext.Size = 40;
            titletext.Pos = new Vector2(550, 485) * scrRatio;
            titletext.TextColor = Color.Red;
            titletext.UseTextColor = true;
            titlebox.Flags = 0x37;
            titletext.Txt = "Boss Rush Menu";


            //Hide rite of kindling
            MenuMan.ActionMsgState = 0;

        }

        private void btnCloseMenu_Click(object sender, EventArgs e)
        {
            FrpgSystem.MoveMap.pause = 0;
            if (!(titlebox == null)) titlebox.Pos = offScreen;
            if (!(titletext == null)) titletext.Pos = offScreen;



            /*
            titlebox?.Cleanup();
            titlebox = null;
            Thread.Sleep(66);

            titletext?.Cleanup();
            titletext = null;
            Thread.Sleep(66);
            */

        }

        private void btnWarp_Click(object sender, EventArgs e)
        {
            if (bosses == null) return;
            boss? bossfight = bosses.FirstOrDefault(b => b.Name == cmbBoss.Text);
            if (bossfight == null) return;
            bossfight.Fight();
        }

        private void btnArena_Click(object sender, EventArgs e)
        {
            new Thread(Arena).Start();
        }

        private void Arena()
        {
            ChrDbg.PlayerHide = true;
            ChrDbg.PlayerSilence = true;
            ChrDbg.PlayerExterminate = true;


            //bosses.Add(new boss("Manus", 0x0c010000, 862.82, -538.33, 882.28, -135, new int[] { 11210002 }, new int[] { 11210031 }));
            IngameFuncs.SetEventFlag(11210002, false);


            GameMan.SetMapUid = 0x0c010000;
            GameMan.WarpNextStage = true;


            if (MenuMan.LoadingState == 0)
            {
                while (MenuMan.LoadingState == 0)
                {

                }
            }

            while (!(MenuMan.LoadingState == 0))
            {

            }

            IngameFuncs.SetDisable(1210840, 1);
            GameRend.ColorCorrection = true;

            Player plr = WorldChrMan.LocalPlayer;
            plr.WarpToCoords(858.52, -576.78, 875.28, -135);

            Thread.Sleep(1000);
            IngameFuncs.CamReset(10000, true);
            FadeMan.A = 0f;

        }
    }
}
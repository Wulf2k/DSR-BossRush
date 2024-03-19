using DarkSoulsScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSR_BossRush
{
    internal class boss
    {
        public string Name = "";
        public uint MapUid = 0;

        public float WarpX = 0;
        public float WarpY = 0;
        public float WarpZ = 0;
        public float WarpFace = 0;
        public int[] clearFlags = { };
        public int[] setFlags = { };

        private boss(string name, uint MapUid, double WarpX, double WarpY, double WarpZ, double WarpFace, int[] clearFlags, int[] setFlags)
        {
            this.Name = name;
            this.MapUid = MapUid;
            this.WarpX = (float)WarpX;
            this.WarpY = (float)WarpY;
            this.WarpZ = (float)WarpZ;
            this.WarpFace = (float)WarpFace;
            this.clearFlags = clearFlags;
            this.setFlags = setFlags;
        }
        private boss(string name, uint MapUid, float WarpX, float WarpY, float WarpZ, float WarpFace, int[] clearFlags, int[] setFlags)
        {
            this.Name = name;
            this.MapUid = MapUid;
            this.WarpX = WarpX;
            this.WarpY = WarpY;
            this.WarpZ = WarpZ;
            this.WarpFace = WarpFace;
            this.clearFlags = clearFlags;
            this.setFlags = setFlags;
        }

        public void Fight()
        {
            new Thread(FightThread).Start();
        }
        private void FightThread()
        {
            ChrDbg.PlayerHide = true;
            ChrDbg.PlayerSilence = true;
            ChrDbg.PlayerExterminate = true;

            foreach (int id in clearFlags)
            {
                IngameFuncs.SetEventFlag(id, false);
            }

            GameMan.SetMapUid = MapUid;
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

            Player plr = WorldChrMan.LocalPlayer;
            plr.WarpToCoords(WarpX, WarpY, WarpZ, WarpFace);

            Thread.Sleep(1000);
            IngameFuncs.CamReset(10000, true);
            FadeMan.A = 0f;

        }



        public static List<boss> getBosses()
        {
            List<boss> bosses = new List<boss>();

            //bosses.Add(new boss("Gough", 0x0c010000, 1061.36, -300, 765.70, 90, new int[] { 11210520, 11210052, 11210056, 11210057, 11210070, 11210071 }, new int[] { }));

            //11210001 = Artorias dead
            //11210030 = Artorias cutscene played
            //50001680 = Artorias soul earned
            bosses.Add(new boss("Artorias", 0x0c010000, 1033.11, -330, 810.55, 90, new int[] { 11210001 }, new int[] { 11210030, 50001680 }));

            //Needs Door eventId
            //16 = Asylum Demon dead
            //11810110 = Exit door open
            //bosses.Add(new boss("Asylum Demon", 0x12010000, 3.14, 198.15, -3.7, 180, new int[] { 16, 11810110, 11810312 }, new int[] { 11810315 }));
            bosses.Add(new boss("Asylum Demon", 0x12010000, 3.06, 198.15, -5.97, 180, new int[] { 16, 11810110 }, new int[] { 11810900 }));

            bosses.Add(new boss("Bed of Chaos", 0x0e010000, 453.03, -363.34, 337.08, 60, new int[] { 10 }, new int[] { }));
            bosses.Add(new boss("Capra Demon", 0x0a010000, -72.46, -43.56, -16.9, 320, new int[] { 11010902 }, new int[] { }));
            bosses.Add(new boss("Ceaseless Discharge", 0x0e010000, 248.32, -283.15, 69.88, 50, new int[] { 11410900 }, new int[] { }));
            bosses.Add(new boss("Centipede Demon", 0x0e010000, 166.73, -383.20, 81.19, 140, new int[] { 11410901 }, new int[] { }));
            bosses.Add(new boss("Demon Firesage", 0x0e010000, 149.52, -341.04, 94.02, 320, new int[] { 11410410 }, new int[] { }));
            bosses.Add(new boss("Four Kings", 0x10000000, 81.02, -163.02, -0.84, 60, new int[] { 13 }, new int[] { }));
            //Needs Garg Intro event flag, tail flags
            bosses.Add(new boss("Gargoyles", 0x0a010000, 17.3, 47.78, 73.9, 0, new int[] { 3 }, new int[] { }));
            //Needs Channeler EventID
            bosses.Add(new boss("Gaping Dragon", 0x0a000000, -166.7, -100.05, -12.75, 0, new int[] { 2 }, new int[] { }));
            bosses.Add(new boss("Gwyn", 0x12000000, 420.43, -115.74, 168.28, 300, new int[] { 15 }, new int[] { }));
            //Find better eventflag
            bosses.Add(new boss("Gwyndolin", 0x0f010000, 432.29, 60.20, 254.91, 90, new int[] { 11510900 }, new int[] { }));
            bosses.Add(new boss("Iron Golem", 0x0f000000, 82.51, 82.25, 254.95, 90, new int[] { 11 }, new int[] { }));

            //11210004 = Kalameet dead
            //11210539 = Kalameet grounded
            //50001710 = Calamity Ring earned
            bosses.Add(new boss("Kalameet", 0x0c010000, 877.00, -344.72, 750.99, 220, new int[] { 11210004 }, new int[] { 11210539, 50001710 }));

            //11210002 = Manus dead
            //11210031 = Manus cutscene played
            bosses.Add(new boss("Manus", 0x0c010000, 862.82, -538.33, 882.28, -135, new int[] { 11210002 }, new int[] { 11210031 }));

            //11200900 = Moonlight Butterfly dead
            bosses.Add(new boss("Moonlight Butterfly", 0x0c000000, 180.37, 8.07, 29.02, 300, new int[] { 11200900 }, new int[] { }));
            bosses.Add(new boss("Nito", 0x0D010000, -111.54, -249.11, -33.71, 290, new int[] { 7 }, new int[] { }));
            bosses.Add(new boss("Ornstein and Smough", 0x0f010000, 536.79, 142.60, 254.99, 90, new int[] { 12 }, new int[] { }));
            bosses.Add(new boss("Pinwheel", 0x0d000000, 50.33, -158.45, 190.41, 160, new int[] { 6 }, new int[] { }));

            //4 = Priscilla dead
            //11105396 = Priscilla's tail cut
            bosses.Add(new boss("Priscilla", 0x0b000000, -22.77, 60.71, 715.05, 180, new int[] { 4, 11105396 }, new int[] { }));
            bosses.Add(new boss("Quelagg", 0x0e000000, 13.84, -237.09, 113.06, 90, new int[] { 9 }, new int[] { }));
            bosses.Add(new boss("Sanctuary Guardian", 0x0c010000, 930.41, -318.63, 470.77, 45, new int[] { 11210000 }, new int[] { }));
            bosses.Add(new boss("Seath", 0x11000000, 134.85, 136.45, 828.33, 330, new int[] { 14 }, new int[] { }));
            bosses.Add(new boss("Sif", 0x0c000000, 275.27, -19.20, -264.90, 210, new int[] { 5 }, new int[] { }));
            bosses.Add(new boss("Stray Demon", 0x12010000, 3.36, 182.05, -28.41, 0, new int[] { 11810900 }, new int[] { }));
            bosses.Add(new boss("Taurus Demon", 0x0a010000, 51.91, 17.21, -118.57, -90, new int[] { 11010901 }, new int[] { }));

            return bosses;
        }
    }
}

using System;
using spaar.ModLoader;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace Blocks__Scale_Mod
{
    public class BesiegeModLoader : Mod
    {
        public override string Name { get { return "BlocksScaleMod"; } }
        public override string DisplayName { get { return "Blocks' Scale Mod"; } }
        public override string BesiegeVersion { get { return "v0.2.3"; } }
        public override string Author { get { return "覅是"; } }
        public override Version Version { get { return new Version("0.5"); } }
        public override bool CanBeUnloaded { get { return true; } }

        public GameObject temp;



        public override void OnLoad()
        {
            GameObject temp = new GameObject();
            temp.AddComponent<ScaleMod>();
            GameObject.DontDestroyOnLoad(temp);

        }
        public override void OnUnload()
        {
            GameObject.Destroy(temp);
        }

    }
    public class ScaleMod :MonoBehaviour
    {
        public string CurrentMachineParentName = ""; 
        public string LoadingData = "";
        public string LoadingTitle = "";
        public string NowTitle = "";
        public bool YesWriteIt = false;
        void Start()
        {
            MachineData.Add("Blocks Scale", ReadScaleData, WriteScaleData);
            Commands.RegisterHelpMessage("SimpleIOBlocks commands:\n	IOSpheres [bool]\n	IOPulse [bool]\n	IOTickGUI [bool]");
            Commands.RegisterCommand("OSD", (args, notUses) =>
            {
                if (args.Length != 1) { return "Please enter your file name!"; }
                string scaledata = "";
                bool firsttime = true;
                foreach (Transform block in Game.AddPiece.machineParent)
                {
                    if (!firsttime) { scaledata += "|"; }
                    firsttime = false;
                    scaledata += block.transform.localScale.x + "," + block.transform.localScale.y + "," + block.transform.localScale.z;
                }
                File.WriteAllText("./Besiege_Data/SavedMachines/" + args[0] + ".txt", scaledata);
                return /*scaledata + */"\n Done! please check your " + args[0] + ".txt under /Besiege_Data/SavedMachines/ \n Please Save your current Machine!";
            }, "Output your blocks' scale!");//OSD

            Commands.RegisterCommand("LSD", (filename, notUses) =>
            {
                if (filename.Length != 1)
                {
                    return "ERROR!Please ensure that you have entered your file name!(without format)";
                }
                else
                {
                    string args;
                    if (File.Exists("./Besiege_Data/SavedMachines/" + filename[0] + ".txt"))
                    {
                        args = File.ReadAllText("./Besiege_Data/SavedMachines/" + filename[0] + ".txt");
                    }
                    else { return "Cannot find the file!"; }
                    string[] scaledataS;

                    try
                    {
                        scaledataS = args.Split('|');
                        for (int ii = 0; ii < scaledataS.Length; ii++)
                        {
                            scaledataS[ii] = scaledataS[ii].Trim();
                        }

                    }
                    catch { return "Invalid Scale data!"; }
                    float[] scalex = new float[scaledataS.Length];
                    float[] scaley = new float[scaledataS.Length];
                    float[] scalez = new float[scaledataS.Length];
                    for (int i = 0; i < scaledataS.Length; i++)
                    {
                        scalex[i] = float.Parse(scaledataS[i].Split(',')[0]);
                        scaley[i] = float.Parse(scaledataS[i].Split(',')[1]);
                        scalez[i] = float.Parse(scaledataS[i].Split(',')[2]);
                    }
                    int CountOfBlock = 0;
                    foreach (Transform block in Game.AddPiece.machineParent)
                    {
                        block.localScale = new Vector3(scalex[CountOfBlock], scaley[CountOfBlock], scalez[CountOfBlock]);
                        CountOfBlock += 1;
                    }
                    return "Done!";
                }
            }, "Load your block's scale data's file name here, Do not need to type in .txt");//LSD
        }
        private string ReadScaleData(string title)
        {
            bool firsttime = true;
            string scaledata = "";
            Debug.Log("Save has been called!");
            foreach (Transform block in Game.AddPiece.machineParent)
            {
                if (!firsttime) { scaledata += "|"; }
                firsttime = false;
                scaledata += block.transform.localScale.x + "," + block.transform.localScale.y + "," + block.transform.localScale.z;
            }
            return scaledata;
        }
        void Update()
        {
            if (YesWriteIt /*&& !LoadingTitle.Equals(NowTitle)*/)
            {
                string[] scaledataS;
                NowTitle = LoadingTitle;
                scaledataS = LoadingData.Split('|');
                for (int ii = 0; ii < scaledataS.Length; ii++)
                {
                    scaledataS[ii] = scaledataS[ii].Trim();
                }
                float[] scalex = new float[scaledataS.Length];
                float[] scaley = new float[scaledataS.Length];
                float[] scalez = new float[scaledataS.Length];
                for (int i = 0; i < scaledataS.Length; i++)
                {
                    scalex[i] = float.Parse(scaledataS[i].Split(',')[0]);
                    scaley[i] = float.Parse(scaledataS[i].Split(',')[1]);
                    scalez[i] = float.Parse(scaledataS[i].Split(',')[2]);
                }
                int CountOfBlock = 0 ;
                foreach (Transform block in Game.AddPiece.machineParent)
                {
                    block.localScale = new Vector3(scalex[CountOfBlock], scaley[CountOfBlock], scalez[CountOfBlock]);
                    CountOfBlock += 1;
                }
                if (CountOfBlock <= 1)
                {
                    YesWriteIt = true;
                }
                else { YesWriteIt = false; }
            }
        }
        private void WriteScaleData(string title, string args)
        {
            if (!args.Equals("NOTFOUND" ))
            {
                Debug.Log("Load Has been called!");
                LoadingData = args.Trim();
                LoadingTitle = null;
                YesWriteIt = true;
            }
            else { Debug.Log("NO Scale Data!"); }
        }
    }
}

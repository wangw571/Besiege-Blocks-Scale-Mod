using System;
using spaar.ModLoader;
using UnityEngine;
using System.Collections.Generic;

namespace Blocks__Scale_Mod
{

    // If you need documentation about any of these values or the mod loader
    // in general, take a look at https://spaar.github.io/besiege-modloader.


    public class BesiegeModLoader : Mod
    {
        public override string Name { get { return "BlocksScaleMod"; } }
        public override string DisplayName { get { return "Blocks' Scale Mod"; } }
        public override string BesiegeVersion { get { return "v0.2.3"; } }
        public override string Author { get { return "覅是"; } }
        public override Version Version { get { return new Version("0.01"); } }
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
        void Start()
        {
            Commands.RegisterHelpMessage("SimpleIOBlocks commands:\n	IOSpheres [bool]\n	IOPulse [bool]\n	IOTickGUI [bool]");
            Commands.RegisterCommand("OSD", (args, notUses) =>
            {
                string scaledata = "" ;
                bool firsttime = true;
                foreach (GameObject block in FindObjectsOfType<GameObject>())
                {
                    if (block.transform.parent == GameObject.Find("MACHINE").transform)
                    {
                        if (!firsttime) { scaledata += "|"; }
                        firsttime = false;          
                        scaledata += block.transform.localScale.x + "," + block.transform.localScale.y + "," + block.transform.localScale.z;
                    }
                }
                return scaledata;
            }, "Output your blocks' scale!");//Amount

            Commands.RegisterCommand("LSD", (args, notUses) =>
            {
                if (args.Length != 1)
                {
                    return "ERROR!Please ensure that you have only got one line of scale data!";
                }
                else {
                    string[] scaledataS;
                    try
                    {
                        scaledataS = args[0].Split('|');
                        for(int ii = 0;ii< scaledataS.Length;ii++)
                        {
                            scaledataS[ii] = scaledataS[ii].Trim();
                        }
                        
                    }
                    catch { return "Invalid Scale data!";  }
                    float[] scalex = new float[scaledataS.Length];
                    float[] scaley = new float[scaledataS.Length];
                    float[] scalez = new float[scaledataS.Length];
                    for ( int i = 0; i < scaledataS.Length; i++) {
                        scalex[i] = float.Parse(scaledataS[i].Split(',')[0]);
                        scaley[i] = float.Parse(scaledataS[i].Split(',')[1]);
                        scalez[i] = float.Parse(scaledataS[i].Split(',')[2]);
                    }
                    int CountOfBlock = 0;
                        foreach (GameObject block in FindObjectsOfType<GameObject>())
                        {
                        if (block.transform.parent == GameObject.Find("MACHINE").transform)
                        {
                            block.transform.localScale = new Vector3(scalex[CountOfBlock], scaley[CountOfBlock], scalez[CountOfBlock]);
                            CountOfBlock += 1;
                        }
                        else { }
                        }
                    return "Done!";
                }
            }, "Input your block's scale data here, for example,\n1,1,15|3,3,3|1,1,1");//Amount
        }
    }
}

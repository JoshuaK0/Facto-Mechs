using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModCombiner
{
    public static BulletMod CombineModList(List<BulletMod> mods, BulletMod outputMod)
    {
        foreach(BulletMod mod in mods)
        {
            if(mod != null)
            {
                outputMod.damage += mod.damage;
                outputMod.accuracy += mod.accuracy;
                outputMod.muzzleVelocity += mod.muzzleVelocity;
                outputMod.fireRate += mod.fireRate;
                outputMod.ammo += mod.ammo;
                outputMod.rifling += mod.rifling;
                if(mod.hasCore)
                {
                    outputMod.hasCore = true;
                }

                foreach (Transform b in mod.behaviours)
                {
                    if (b != null)
                    {
                        outputMod.behaviours.Add(b);
                    }
                }
            }
            
        }
        return outputMod;
    }

    public static BulletMod CombineTwoMods(BulletMod mod1, BulletMod mod2)
    {
        if (mod1 != null && mod2 != null)
        {
            mod1.damage += mod2.damage;
            mod1.accuracy += mod2.accuracy;
            mod1.muzzleVelocity += mod2.muzzleVelocity;
            mod1.fireRate += mod2.fireRate;
            mod1.ammo += mod2.ammo;
            mod1.rifling += mod2.rifling;
            if(mod2.hasCore)
            {
                mod1.hasCore = true;
            }
            

            foreach (Transform b in mod2.behaviours)
            {
                if (b != null)
                {
                    mod1.behaviours.Add(b);
                }
            }
            return mod1;
        }
        else
        { return null; }
    }
}

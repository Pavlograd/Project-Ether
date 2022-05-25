using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextureHolder : MonoBehaviour
{
    public Sprite spriteToChange = null;
    public receiverTexture receiverObj = null;

    public void setTexture(Sprite spriteToSet)
    {   if (spriteToSet != null)
        {
            spriteToChange= spriteToSet;
        }
    }

    public void changeOnCLick()
    {
        if (receiverObj != null && spriteToChange != null)
        {
            receiverObj.changeTexture(spriteToChange);
        }
    }
    
    
}

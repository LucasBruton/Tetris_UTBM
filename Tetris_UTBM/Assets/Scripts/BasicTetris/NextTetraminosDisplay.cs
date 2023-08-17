using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// This script is used to show the 4 next moves
public class NextTetraminosDisplay : MonoBehaviour
{
    // Sprites used to show a futur move
    public Sprite spriteI;
    public Sprite spriteJ;
    public Sprite spriteL;
    public Sprite spriteO;
    public Sprite spriteT;
    public Sprite spriteS;
    public Sprite spriteZ;

    // Images used for the 4 next moves
    public Image[] futurTetraminos;

    // Update the images used to show the next 4 moves
    public void UpdateNextMoves(Queue<TetraminoData> dataNextMoves)
    {
        int i = 0;
        foreach(TetraminoData data in dataNextMoves) {
            updateTetraminoSprite(data, futurTetraminos[i++]);
        }
        
    }

    // Update an image to the corresponding move
    private void updateTetraminoSprite(TetraminoData data, Image image)
    {
        Sprite updateSprite = null;
        switch(data.tetramino)
        {
            case Tetramino.I:
                updateSprite = spriteI;
                break;
            case Tetramino.J:
                updateSprite = spriteJ;
                break;
            case Tetramino.L:
                updateSprite = spriteL;
                break;
            case Tetramino.O:
                updateSprite = spriteO;
                break;
            case Tetramino.T:
                updateSprite = spriteT;
                break;
            case Tetramino.S:
                updateSprite = spriteS;
                break;
            case Tetramino.Z:
                updateSprite = spriteZ;
                break;
        }

        image.sprite = updateSprite;
    }
}

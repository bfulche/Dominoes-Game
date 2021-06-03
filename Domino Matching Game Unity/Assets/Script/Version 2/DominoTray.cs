using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoTray : MonoBehaviour
{
    public List<Domino> dominos;

    public Transform trayStartPosition; // location where to start adding dominos

    public Transform handDeck; // the tray reference;

    [SerializeField] private float dominoSpacing = 2.5f;

    public delegate void EmptyTray();
    public EmptyTray OnTrayEmptied;



    /// <summary>
    /// Expecting array of domino prefabs
    /// </summary>
    /// <param name="startingDominos"></param>
    public void CreateDominoHand(Domino[] startingDominos)
    {
        foreach (Domino domino in startingDominos)
        {
            Domino t = Instantiate(domino);
            dominos.Add(t);
        }

        FitTray();
    }

    public void AddDominoToTray(Domino newDomino)
    {
        // in actual gameplay, Tiles would only be added when you move a domino from the field, back to the tray
        if (dominos.Count > 8)
            return;
       // Domino t = Instantiate(dominoToAdd);
        dominos.Add(newDomino);
        FitTray();
    }

 //   public void RemoveFirstTile()
 //   {
 //       if (dominos.Count > 0)
 //       {
 //           Domino t = dominos[0];
 //           dominos.Remove(t);
 //         //  Destroy(t.gameObject);  // in actual gameplay. You'd just remove the reference to the list as the domino would be "on the field"
 //           FitTray();
 //       }
 //   }

    public void RemoveDomino(Domino selectedDomino)
    {
        if (dominos.Contains(selectedDomino))
        {
            dominos.Remove(selectedDomino);
            FitTray();

            if (dominos.Count == 0)
            {
                OnTrayEmptied?.Invoke();
            }
        }
    }

    public void ClearDominoTray()
    {
        dominos.Clear();
    }

    private void FitTray()
    {
        int count = dominos.Count;
        // adjust card positioning to fit properly on tray
        for (int i = count - 1 ; i >= 0; i--)
        {
          //  dominos[i].position = trayStartPosition.position;

            dominos[i].transform.position = new Vector3((trayStartPosition.position.x + ((count - 1 - i) * -dominoSpacing)), trayStartPosition.position.y, trayStartPosition.position.z);
        }
    }
}

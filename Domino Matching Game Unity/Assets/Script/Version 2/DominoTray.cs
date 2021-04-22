using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoTray : MonoBehaviour
{
    public List<Transform> dominos;

    public Transform trayStartPosition; // location where to start adding dominos

    public Transform handDeck; // the tray reference;

    public float howManyAdded; // how many dominos added so far

    public Transform dominoToAdd;

    public Transform[] startingHand;

    /// <summary>
    /// Expecting array of domino prefabs
    /// </summary>
    /// <param name="startingDominos"></param>
    public void CreateDominoHand(Transform[] startingDominos)
    {
        foreach (Transform domino in startingDominos)
        {
            Transform t = Instantiate(domino);
            dominos.Add(t);
        }

        FitTray();
    }

    public void AddNewTile()
    {
        // in actual gameplay, Tiles would only be added when you move a domino from the field, back to the tray
        if (dominos.Count > 8)
            return;
        Transform t = Instantiate(dominoToAdd);
        dominos.Add(t);
        FitTray();
    }

    public void RemoveFirstTile()
    {
        if (dominos.Count > 0)
        {
            Transform t = dominos[0];
            dominos.Remove(t);
            Destroy(t.gameObject);  // in actual gameplay. You'd just remove the reference to the list as the domino would be "on the field"
            FitTray();
        }
    }

    private void FitTray()
    {
        int count = dominos.Count;
        // adjust card positioning to fit properly on tray
        for (int i = count - 1 ; i >= 0; i--)
        {
          //  dominos[i].position = trayStartPosition.position;

            dominos[i].position = new Vector3((trayStartPosition.position.x + ((count - 1 - i) * -1.5f)), trayStartPosition.position.y, trayStartPosition.position.z);
        }
    }

  //  public void FitCards()
  //  {
  //      if (dominos.Count == 0)
  //          return;
  //
  //  //    GameObject obj = dominos[0];
  //
  //      obj.transform.position = trayStartPosition.position;
  //
  //      obj.transform.position += new Vector3((howManyAdded * gapFromOneItemToTheNextOne), 0, 0);
  //
  //      obj.transform.SetParent(handDeck);
  //
  //      dominos.RemoveAt(0);
  //      howManyAdded++;
  //  }
}

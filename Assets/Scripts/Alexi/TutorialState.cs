using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialState : MonoBehaviour
{
    public class TutorialPiece
    {
        [SerializeField] public PhysicsGun.Mode mode;
        [SerializeField] public PhysicsEffect effect;
        [SerializeField] public Door door;

        public TutorialPiece(PhysicsGun.Mode _mode,
                             PhysicsEffect _effect,
                             Door _door)
        {
            mode = _mode;
            effect = _effect;
            door = _door;
        }
    }

    List<TutorialPiece> tutorialPieces;

    void Start()
    {
        tutorialPieces = new List<TutorialPiece>();

        List<Door> doors = new List<Door>(FindObjectsOfType<Door>());

        foreach(KeyValuePair<PhysicsGun.Mode, PhysicsEffect> pair in PhysicsGun.effects)
        {
            TutorialPiece newPiece = new TutorialPiece(pair.Key, pair.Value, 
                    doors.Find(door => door.modeToOpen == pair.Key));
            newPiece.effect.effectAppliedEvent += ApplyEffectAction;
            tutorialPieces.Add(newPiece);
        }
    }

    public void ApplyEffectAction()
    {
        Debug.Log(tutorialPieces.Count + " modes left to try");

        TutorialPiece removing;
        removing = tutorialPieces.Find(piece => piece.mode == PhysicsGun.currentMode);
        if (null != removing)
        {
            tutorialPieces.Remove(removing);
            removing.door.Open();
        }
    }
}

public class Door : MonoBehaviour
{
    public PhysicsGun.Mode modeToOpen;

    public void Open() { }
}

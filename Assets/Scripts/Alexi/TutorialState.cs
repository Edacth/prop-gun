using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialState : MonoBehaviour
{
    public class TutorialPiece
    {
        [SerializeField] public PhysicsGun.Mode mode;
        [SerializeField] public PhysicsEffect effect;
        [SerializeField] public Door door;

        public TutorialPiece(PhysicsGun.Mode _mode,
                             PhysicsEffect   _effect,
                             Door            _door)
        {
            mode = _mode;
            effect = _effect;
            door = _door;
        }
    }

    public List<Door> doors;
    public Door moveDoor;
    public Door jumpDoor;
    public Door grabDoor;

    List<TutorialPiece> tutorialPieces;

    void Start()
    {
        tutorialPieces = new List<TutorialPiece>();

        foreach(KeyValuePair<PhysicsGun.Mode, PhysicsEffect> pair in PhysicsGun.effects)
        {
            TutorialPiece newPiece = new TutorialPiece(pair.Key, pair.Value, 
                    doors.Find(door => door.modeToOpen == pair.Key));
            newPiece.effect.effectAppliedEvent += ApplyEffectAction;
            tutorialPieces.Add(newPiece);
        }

        PhysicsGun.objectGrabbedEvent += GrabFunc;
        PlayerController.jumpEvent += JumpFunc;
    }

    public void ApplyEffectAction()
    {
        // Debug.Log(tutorialPieces.Count + " modes left to try");

        TutorialPiece removing;
        removing = tutorialPieces.Find(piece => piece.mode == PhysicsGun.currentMode);
        if (null != removing)
        {
            tutorialPieces.Remove(removing);
            if(null != removing.door) { removing.door.Open(); }
            else { Debug.LogWarning("No " + removing.mode + " door"); }
        }
    }

    void GrabFunc()
    {
        // grabDoor.Open();
        PhysicsGun.objectGrabbedEvent -= GrabFunc;
        grabDoor.Open();
        StartCoroutine("StartGame");
    }

    void JumpFunc()
    {
        jumpDoor.Open();
        PlayerController.jumpEvent -= JumpFunc;
    }

    void OnTriggerEnter(Collider other)
    {
        moveDoor.Open();
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("BounceCannon");
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// gun that can affect physics properties of objects
/// </summary>
public class PhysicsGun : MonoBehaviour
{
    /// <summary>
    /// how the gun affects physics objects
    /// </summary>
    public enum Mode
    {
        mass,       // increase/decrease mass
        material,   // change physics material
        gravity,    // increase/decrease gravity modifier
        layer,      // change collision layer
        kinematic,  // toggle isKinematic
        force,      // apply pushing force
        magnet,     // apply pulling force
        torque      // apply torque
    };

    public InteractableChecker interactableChecker;
    public PhysicsValues data;

    const int modeCount = 8; // how many modes there are

    public static Mode currentMode;
    public static InteractableObject currentObject;
    InteractableObjectCollectionManager collectionManager;
    static Dictionary<Mode, PhysicsEffect> effects;
    [SerializeField] Transform grabPoint;
    InteractableObject grabedObject;

    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
    };

    // Start is called before the first frame update
    void Start()
    {
        // initalize effects
        if (null == effects)
        {
            effects = new Dictionary<Mode, PhysicsEffect>();
            effects.Add(Mode.mass, new ChangeMass(data.minMass, data.maxMass));
            effects.Add(Mode.material, new ChangeMaterial(data.physMaterials));
            effects.Add(Mode.gravity, new ChangeGravity(data.minGrav, data.maxGrav));
            effects.Add(Mode.layer, new ChangeLayer(data.defaultLayer, data.layer1, data.layer2));
            effects.Add(Mode.kinematic, new ToggleKinematic());
            effects.Add(Mode.force, new ApplyForce(data.force));
            effects.Add(Mode.magnet, new UseMagnet(data.magForce));
            effects.Add(Mode.torque, new ApplyTorque(data.torque));
        }

        currentMode = Mode.mass; // initialize to default
        SwitchMode(currentMode); // initialize effect

        // can be changed to drag and drop for performance later
        collectionManager = FindObjectOfType<InteractableObjectCollectionManager>();
    }

    void Update()
    {
        TakeInput();
    }

    /// <summary>
    /// get mouse and keyboard input
    /// </summary>
    void TakeInput()
    {
        #region Effect Editor
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            PhysicsEffect.current.EnterEditMode();
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            PhysicsEffect.current.RunEditMode();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            PhysicsEffect.current.ExitEditMode();
        }
        else //not in edit mode
        {
            if (Input.mouseScrollDelta.y < 0) { ScrollSelection(1); }
            else if (Input.mouseScrollDelta.y > 0) { ScrollSelection(-1); }
        }
        #endregion

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                int numberPressed = i + 1;
                Mode newMode = (Mode)numberPressed;
                SwitchMode(newMode);
                //Debug.Log(numberPressed);
            }

             
        }
        if (Input.GetMouseButtonDown(1))
        {
            GetGrab();
        }
        if (Input.GetMouseButton(1))
        {
            Grab();
        }
        if (Input.GetMouseButtonUp(1))
        {
            UnGrab();
        }

    }

    /// <summary>
    /// go to next mode
    /// </summary>
    void ScrollSelection(int add)
    {
        int cur = (int)currentMode;
        cur += add;

        if(cur >= modeCount) { cur = 0; } //FIXME this may break when adding more than modeCount, modulo would fix that but would break on negitive numbers aswell, we may want to just make this take a up or down bool anyway
        else if(cur < 0) { cur = modeCount - 1; }

        SwitchMode((Mode)cur);

        
    }

    public void SwitchMode(Mode newMode)
    {
        collectionManager?.SwitchMode(newMode);

        // any kind of animation here

        // switch gun ui

        currentMode = newMode;
        effects.TryGetValue(currentMode, out PhysicsEffect.current);
        Debug.Log(currentMode);
    }

    /// <summary>
    /// fire the gun
    /// </summary>
    public void Fire()
    {
        if(null == currentObject) { return; }


        PhysicsEffect.current.ApplyEffect(currentObject);
    }
    public void Grab()
    {
        if (null == grabedObject) { return; }

        grabedObject.grabTarget = grabPoint;      
        grabedObject.grabUpdate();
    }
    public void GetGrab()
    {
        if (null == currentObject) { return; }
        grabedObject = currentObject;
    }
    public void UnGrab()
    {
        grabedObject = null;
    }
}

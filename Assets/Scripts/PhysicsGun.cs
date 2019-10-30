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
    };

    public InteractableChecker interactableChecker;
    LineRenderer lineRenderer;
    public PhysicsValues data;
    public GlowOutlinePostProcessing outlineEffect;

    int modeCount = 6; // how many modes there are

    public static Mode currentMode;
    public static InteractableObject currentInteractingObject;
    public static InteractableObject currentPointingObject;
    public static Dictionary<Mode, PhysicsEffect> effects { get; private set; }
    [SerializeField] Transform grabPoint;
    InteractableObject grabbedObject;
    [SerializeField]
    AimDownSights aimDownSights;
    float colorTime = -1;
    [SerializeField]
    float colorDelay = 0.1f;
    Color fireColor;

    bool fireEnabled; // can we fire at things?

    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6
    };

    // Start is called before the first frame update
    void Start()
    {
        // initalize effects
        if (null == effects)
        {
            effects = new Dictionary<Mode, PhysicsEffect>();
            effects.Add(Mode.mass,      new ChangeMass());
            effects.Add(Mode.material,  new ChangeMaterial());
            effects.Add(Mode.gravity,   new ChangeGravity());
            effects.Add(Mode.layer,     new ChangeLayer());
            effects.Add(Mode.kinematic, new ToggleKinematic());
            effects.Add(Mode.force,     new ApplyForce(data.force, data.forceStepAmount, data.camera));

            modeCount = effects.Count;
        }

        currentMode = Mode.mass; // initialize to default
        SwitchMode(currentMode); // initialize effect

        // debug for physics effect
        if (null == PhysicsEffect.current)
        {
            Debug.LogWarning("Physics effect not set. Default to mass");
            PhysicsEffect.current = new ChangeMass();
        }

        lineRenderer = GetComponent<LineRenderer>();
        fireColor = new Color(0, 255, 255);
    }

    void Update()
    {
        TakeInput();

        if (colorTime != -1)
        {
            colorTime += Time.deltaTime;
            if (colorTime >= colorDelay)
            {
                colorTime = -1;
                lineRenderer.startColor = Color.red;
            }
        }
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
            aimDownSights.TakeAim();
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            PhysicsEffect.current.RunEditMode();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            PhysicsEffect.current.ExitEditMode();
            aimDownSights.ReleaseAim();
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
                Mode newMode = (Mode)numberPressed - 1;
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
        InteractableObjectCollectionManager.SwitchMode(newMode);

        // any kind of animation here

        if (currentMode != newMode)
        {
            PhysicsEffect.current.OnSwitchedFrom(currentInteractingObject);
        }

        currentMode = newMode;
        effects.TryGetValue(currentMode, out PhysicsEffect.current);

        if (null == PhysicsEffect.current) { Debug.LogWarning("Invalid physics effect: " + newMode); }
        else { PhysicsEffect.current.OnSwitchedTo(currentInteractingObject); }

        interactableChecker.CheckInteractableHelper();

        // Switch gun UI panels
        SwitchUI(newMode);

        StartCoroutine("FinishSwitch");
        outlineEffect?.SwitchOutlineColor(newMode);
    }

    /// <summary>
    /// fire the gun
    /// </summary>
    public void Fire()
    {
        if (null != PhysicsValues.instance.shotPointParticles)
        {
            Instantiate(PhysicsValues.instance.shotPointParticles, interactableChecker.getRaycastHit().point, Quaternion.identity);
        }
        else
        {
            Debug.LogError("shotPointParticles is null. Assign it in physics values to the 'Spark emitter' prefab");
        }

        lineRenderer.startColor = fireColor;
        colorTime = 0.0f;

        if (null == currentInteractingObject) { return; }
        PhysicsEffect.current.ApplyEffect(currentInteractingObject);
        
    }
    public void Grab()
    {
        if(grabPoint == null)
        {
            throw new System.Exception("You guys remeber to set grab point to an empty vaugly infront of the charecter");
        }
        if (null == grabbedObject) { return; }
        grabbedObject.grabTarget = grabPoint;      
        grabbedObject.grabUpdate();
    }
    public void GetGrab()
    {
        grabbedObject = currentPointingObject;
    }
    public void UnGrab()
    {
        grabbedObject = null;
    }

    /// <summary>
    /// Set all ui panels to false except the one we want
    /// </summary>
    private void SwitchUI(Mode newMode)
    {
        data.UIPanels[(int)newMode].SetActive(true);

        int offset = (int)newMode + 1;
        for (int i = 0; i < data.UIPanels.Length - 1; ++i)
        {
            int index = (offset + i) % data.UIPanels.Length;
            data.UIPanels[index].SetActive(false);
            //Debug.Log("Disable " + (Mode)index);
        }
    }

    /// <summary>
    /// disable fire until end of frame, to combat weird object issue
    /// </summary>
    /// 
    /// <returns>
    /// coroutine, end of frame
    /// </returns>
    IEnumerator FinishSwitch()
    {
        fireEnabled = false;
        yield return new WaitForEndOfFrame();
        fireEnabled = true;
    }
}

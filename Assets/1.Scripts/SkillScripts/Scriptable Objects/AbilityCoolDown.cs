using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityCoolDown : MonoBehaviour {

    public string abilityButtonAxisName = "Fire1";
    public Image darkMask;
    public Text coolDownTextDisplay;
    public Text abilityTextDisplay;

    [SerializeField]
    private Ability ability;

    private Image myButtonImage;
    private AudioSource abilitySource;
    private float abilityTimeLeft;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;

    // Use this for initialization
    void Start () {
        Initialize(ability);
	}

    public void Initialize(Ability selectedAbility)
    {
        ability = selectedAbility;
        myButtonImage = GetComponent<Image>();
        abilitySource = GetComponent<AudioSource>();
        myButtonImage.sprite = ability.aSprite;

        darkMask.sprite = ability.aSprite;
        coolDownDuration = ability.aBaseCoolDown;
        abilityTimeLeft = 0.0f;
        ability.Initialize();
        AbilityReady();
    }


    // Update is called once per frame
    void Update () {

        // bool coolDownComplete = (Time.time > nextReadyTime)
        
        bool coolDownComplete = (coolDownTimeLeft <= 0);
        if (coolDownComplete)
        {
            AbilityReady();     // TODO: calling this function every frame is not efficient
            // Check button input and trigger CD
            if (Input.GetButtonDown(abilityButtonAxisName))
                ButtonTriggered();
        }
        else
        {
            CoolDown();         // Update CD timer
            bool abilityComplete = (abilityTimeLeft <= 0);
            if (abilityComplete)
            {
                ability.Deactivate();
                abilityTimeLeft = 0;
            }
        }
	
	}

    // Reset the CD button in a ready state
    private void AbilityReady()
    {
        coolDownTextDisplay.enabled = false;
        darkMask.enabled = false;
    }


    // Update CD timer
    private void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        
        float roundedCD = Mathf.Round(coolDownTimeLeft);
        coolDownTextDisplay.text = roundedCD.ToString();
        darkMask.fillAmount = coolDownTimeLeft / coolDownDuration;

        Debug.Log(abilityTimeLeft.ToString());
        if (abilityTimeLeft > 0)
        {
            abilityTimeLeft -= Time.deltaTime;
            float roundedAbility = Mathf.Round(abilityTimeLeft);
            abilityTextDisplay.text = roundedAbility.ToString();
        }
        if (abilityTimeLeft < 0)
        {
            abilityTimeLeft = 0;
            abilityTextDisplay.text = null;
        }
    }



    // Called from update
    private void ButtonTriggered()
    {
        nextReadyTime = coolDownDuration + Time.time;       // Determine Time when CD can be used again
        coolDownTimeLeft = coolDownDuration;                // Start Cooldown timer

     
        abilityTimeLeft = ability.aDuration;                // Set Ability Timer
        abilityTextDisplay.text = abilityTimeLeft.ToString();
        
        darkMask.enabled = true;
        coolDownTextDisplay.enabled = true;
        coolDownTextDisplay.text = coolDownTimeLeft.ToString();

        //abilitySource.clip = ability.aSound;      // TODO: play sound
        //abilitySource.Play();
        ability.TriggerAbility();

    }
}

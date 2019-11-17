using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LookToCamera))]
public class TeleportController : MonoBehaviour
{

    public Enums.TypeOfTeleport type;

    private GameObject[]    _childs;
    private float           _counter;
    private float           _counterRecharge;
    private Material        _planeMaterial;
    private Image           _backgroundImage;

    private Quaternion      _originalRotation;
    private bool            _isAwake = false;
    public bool            _LookDirection = false;

    private void Awake()
    {
        _originalRotation = transform.rotation;
         
        Init();

        _isAwake = true;

    }
    
    private void Start()
    {
        _planeMaterial = _childs[(int)Enums.TypeOfTeleport.Ring].GetComponent<MeshRenderer>().material;
        _backgroundImage = _childs[(int)Enums.TypeOfTeleport.Tread].transform.GetChild(0).GetComponent<Image>();
        
        _backgroundImage.fillAmount = 1 - _counter;
        _planeMaterial.SetFloat("_Cutoff", _counter);
    }

    // Update is called once per frame
    void Update()
    {
        RechargeTeleport();
        _LookDirection = true;
    }

    private void Init()
    {
        _counter = Constans.MINIM_CUTOFF_VALUE;
        _counterRecharge = Constans.MAX_CUTOFF_VALUE;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        _childs = new GameObject[transform.childCount];

        _childs[(int)Enums.TypeOfTeleport.Ring] = GetComponentInChildren<MeshRenderer>().gameObject;
        _childs[(int)Enums.TypeOfTeleport.Tread] = GetComponentInChildren<Canvas>().gameObject;

        _childs[(int)Enums.TypeOfTeleport.Ring].SetActive(type == Enums.TypeOfTeleport.Ring);
        _childs[(int)Enums.TypeOfTeleport.Tread].SetActive(type == Enums.TypeOfTeleport.Tread);

        GetComponent<LookToCamera>().enabled = (type == Enums.TypeOfTeleport.Ring);
    }

    private void RestoreOrientation()
    {
        if (_isAwake == true)
        {
            transform.rotation = _originalRotation;
        }

    }

    private void OnValidate()
    {
        Init();

        RestoreOrientation();
    }

    public void RestoreValues()
    {
        
        Init();

        transform.rotation = _originalRotation;

        _planeMaterial.SetFloat("_Cutoff", _counter);
        _backgroundImage.fillAmount = 1 - _counter;
    }

    public void RechargeTeleport()
    {
        if (_LookDirection == true)
        {
            _counterRecharge += Time.deltaTime / Constans.TIME_TO_LOAD;

            if (_counterRecharge > 0.99f)
            {
                _counterRecharge = 0.99f;
            }

            switch (type)
            {
                case Enums.TypeOfTeleport.Ring:

                    _planeMaterial.SetFloat("_Cutoff", 1 - _counterRecharge);

                    break;
                case Enums.TypeOfTeleport.Tread:

                    _backgroundImage.fillAmount = 0 + _counterRecharge ;

                    break;
                default:
                    break;
            }

            _counter = 0;
        }
    }

    public void LoadingTeleport(PlayerController player)
    {
        _counter += Time.deltaTime / Constans.TIME_TO_TELEPORT;

        if (_counter > 1)
        {
            _counter = 1;
            player.transform.position = transform.position;
            player.transform.rotation = _originalRotation;

            player.SetCurrentPoint(this);

            gameObject.SetActive(false);
        }

        _LookDirection = false;

        switch (type)
        {
            case Enums.TypeOfTeleport.Ring:

                _planeMaterial.SetFloat("_Cutoff", _counter);

                break;
            case Enums.TypeOfTeleport.Tread:
                
                    _backgroundImage.fillAmount = 1 - _counter;
                
                break;
            default:
                break;
        }

        _counterRecharge = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : ImpBehaviour
{
    [Header("EnergyManager")]
    private static EnergyManager _instance;
    public static EnergyManager Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("Singleton instance has not been created yet!");
            return _instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (_instance == null)
        {
            _instance = this;
            //if (transform.parent == null) DontDestroyOnLoad(gameObject);
            return;
        }

        if (_instance != this)
        {
            Debug.LogWarning("Another instance of EnergyManager already exists!");
            //Destroy(gameObject);
            PoolManager.Instance.ReturnToPool(gameObject, PoolManager.TagType.energy);
            return;
        }
    }

    [SerializeField] protected int energy = 100;
    public int Energy { get => energy; }

    [SerializeField] protected float increaseTime = 1;
    public float IncreaseTime { get => increaseTime; }

    [SerializeField] protected int increaseEnergy = 15;
    public int IncreaseEnergy { get => increaseEnergy; }

    [SerializeField] protected Image energyImg;
    //public Image EnergyImg { get => energyImg; }

    [SerializeField] protected TextMeshProUGUI energyTxt;
    //public Text EnergyTxt { get => energyTxt; }

    [SerializeField] protected TextMeshProUGUI energyNeedTxt;
    public TextMeshProUGUI EnergyNeedTxt { get => energyNeedTxt; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnergyImg();
        this.LoadEnergyTxt();
        this.LoadEnergyNeedTxt();
    }

    protected virtual void LoadEnergyImg()
    {
        if (this.energyImg != null) return;
        this.energyImg = GameObject.Find("EnergyImg").GetComponent<Image>();
        Debug.Log(transform.name + ": LoadEnergyImg", gameObject);
    }

    protected virtual void LoadEnergyTxt()
    {
        if (this.energyTxt != null) return;
        this.energyTxt = GameObject.Find("EnergyTxt").GetComponent<TextMeshProUGUI>();
        this.energyTxt.text = "x" + this.energy;
        Debug.Log(transform.name + ": LoadEnergyTxt", gameObject);
    }

    protected virtual void LoadEnergyNeedTxt()
    {
        if (this.energyNeedTxt != null) return;
        this.energyNeedTxt = GameObject.Find("EnergyNeedTxt").GetComponent<TextMeshProUGUI>();
        Debug.Log(transform.name + ": LoadEnergyNeedTxt", gameObject);
    }

    public void Add(int _energy)
    {
        this.energy += _energy;
        this.UpdateUI();
    }


    protected virtual void Sub(int _energy)
    {
        this.energy -= _energy;
        this.UpdateUI();
    }

    public virtual bool CanUseEnergy(int _energy)
    {
        if (this.energy < _energy) return false;
        this.Sub(_energy);
        return true;
    }

    protected override void Start()
    {
        base.Start();
        this.AutoIncreaseEnergy();
    }

    protected virtual void AutoIncreaseEnergy()
    {
        StartCoroutine(AutoIncreaseCoroutine());
    }

    protected virtual IEnumerator AutoIncreaseCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(increaseTime);
            this.Add(increaseEnergy);
        }
    }

    protected virtual void UpdateUI()
    {
        this.energyTxt.text = "x" + this.energy;
    }
}

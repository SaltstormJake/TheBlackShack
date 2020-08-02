﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blackjackUIScript : MonoBehaviour
{
    GameObject[] UIObjects;
    [SerializeField] Button DealButton;
    [SerializeField] Button HitButton;
    [SerializeField] Button StandButton;
    [SerializeField] Button DoubleDownButton;
    [SerializeField] Button SplitButton;
    [SerializeField] Button InsuranceButton;
    [SerializeField] Slider betSlider;
    [SerializeField] GameObject Insurance;
    [SerializeField] Button insuranceYesButton;
    [SerializeField] Button insuranceNoButton;
    [SerializeField] GameObject betText;
    [SerializeField] Text betTextNumber;
    [SerializeField] GameObject fundsText;
    [SerializeField] Text fundsTextNumber;
    [SerializeField] dealerScript dealer;
    [SerializeField] playerScript player;
    int funds = 500;
    int betAmount;

    public enum Result { PlayerWins, DealerWins, PlayerBlackjack, DealerBlackjack, BothHaveBlackjack, PlayerBust, DealerBust, Player5Cards, Dealer5Cards, Push };

    private void Awake()
    {
        UIObjects = GameObject.FindGameObjectsWithTag("UIOnly");
        DealButton.onClick.AddListener(OnDealClick);
        HitButton.onClick.AddListener(OnHitClick);
        StandButton.onClick.AddListener(OnStandClick);
        DoubleDownButton.onClick.AddListener(OnDoubleDownClick);
        SplitButton.onClick.AddListener(OnSplitClick);
        insuranceYesButton.onClick.AddListener(() => OnInsuranceClick(true));
        insuranceNoButton.onClick.AddListener(() => OnInsuranceClick(false));
    }

    // Start is called before the first frame update
    void Start()
    {
        fundsTextNumber.text = funds.ToString();
        SetUI(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (betTextNumber.IsActive())
        {
            betTextNumber.text = (betSlider.value * 50).ToString();
        }
    }

    public void ClearTable()
    {
        dealer.ClearHand();
        player.ClearHand();
    }


    public void SetDeal(bool enabled)
    {
        DealButton.gameObject.SetActive(enabled);
        betSlider.gameObject.SetActive(enabled);
        betText.SetActive(enabled);
        betTextNumber.gameObject.SetActive(enabled);
        betSlider.maxValue = GetFunds() / 50;

        SetFunds(true);
    }

    public void SetFunds(bool enabled)
    {
        fundsText.SetActive(enabled);
        fundsTextNumber.gameObject.SetActive(enabled);
    }

    public void SetHitAndStand(bool enabled)
    {
        HitButton.gameObject.SetActive(enabled);
        StandButton.gameObject.SetActive(enabled);
    }

    public void SetDoubleDown(bool enabled)
    {
        DoubleDownButton.gameObject.SetActive(enabled);
    }

    public void SetSplit(bool enabled)
    {
        SplitButton.gameObject.SetActive(enabled);
    }

    public void SetInsurance(bool enabled)
    {
        Insurance.SetActive(enabled);
    }

    public void SetUI(bool enabled)
    {
        foreach (GameObject g in UIObjects)
            g.SetActive(enabled);
    }

    public void ChangeFunds(int i)
    {
        funds += i;
        fundsTextNumber.text = funds.ToString();
    }

    public int GetFunds()
    {
        return funds;
    }

    public void OnDealClick()
    {
        ClearTable();
        player.SetBetAmount((int)betSlider.value * 50);
        SetDeal(false);
        StartCoroutine(dealer.Deal());
        player.ToggleTableLean();
        ChangeFunds(-(int)betSlider.value * 50);
    }

    public void OnDealAgainClick(int bet)
    {
        ClearTable();
        player.SetBetAmount(bet);
        StartCoroutine(dealer.Deal());
        player.ToggleTableLean();
        ChangeFunds(-bet);
    }

    private void OnHitClick()
    {
        StartCoroutine(dealer.Hit());
        SetHitAndStand(false);
        SetInsurance(false);
        SetDoubleDown(false);
        SetSplit(false);
    }

    private void OnStandClick()
    {
        StartCoroutine(dealer.Stand());
        SetHitAndStand(false);
        SetInsurance(false);
        SetDoubleDown(false);
        SetSplit(false);
    }

    private void OnDoubleDownClick()
    {
        ChangeFunds(-player.GetBetAmount());
        StartCoroutine(dealer.DoubleDown());
        SetHitAndStand(false);
        SetInsurance(false);
        SetDoubleDown(false);
        SetSplit(false);
    }

    private void OnSplitClick()
    {

    }
    
    private void OnInsuranceClick(bool tookInsurance)
    {
        StartCoroutine(dealer.Insurance(tookInsurance));
        if(tookInsurance)
            ChangeFunds(-(int)(player.GetBetAmount() / 2));
        SetInsurance(false);
    }


    public int GetDealerHandValue()
    {
        return dealer.GetHandValue();
    }

    public int GetPlayerHandValue()
    {
        return player.GetHandValue();
    }

    public bool GetPlayerInsurance()
    {
        return player.GetInsurance();
    }

    public void QuitGame()
    {
        StartCoroutine(player.QuitGame());
        StartCoroutine(dealer.QuitGame());
        SetFunds(false);
    }
}

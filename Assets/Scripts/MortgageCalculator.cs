using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MortgageCalculator : MonoBehaviour
{
    public Text costOfHouse;
    public Text interestRate;
    public Text monthlyPayment;
    public Text output;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) CalculatePressed();
    }

    public void CalculatePressed()
    {
        if (costOfHouse.text == "" || interestRate.text == "" || monthlyPayment.text == "")
        {
            output.text = "Invalid input";
        }
        try
        {
            Calculate(float.Parse(costOfHouse.text), float.Parse(interestRate.text), float.Parse(monthlyPayment.text));
        }
        catch
        {
            output.text = "Cannot parse input values";
        }
    }

    public void Calculate(float houseCost, float percentageInterestRate, float monthlyPayment) //how much money I would need to pay in total if I took a mortgage for a house, and how many years it would take
    {
        if (houseCost <= 0 || percentageInterestRate <= 0 || monthlyPayment <= 0)
        {
            output.text = "Error: An input field is zero.";
            return;
        }
        float currentCost = houseCost;
        float totalCost = 0;
        int years = 0;
        float interestRate = percentageInterestRate / 100;
        float yearlyPayment = monthlyPayment * 12;
        if (yearlyPayment < houseCost * interestRate)
        {
            output.text = "You're not going to pay off the debt with monthly payments of £" + monthlyPayment;
            return;
        }
        do
        {
            currentCost = currentCost * (1 + interestRate);
            if (currentCost > yearlyPayment)
            {
                totalCost += yearlyPayment;
                currentCost -= yearlyPayment;
            }
            else
            {
                totalCost += currentCost;
                currentCost -= currentCost;
            }
            years++;
            if (years > 1000)
            {
                output.text = "It will take more than 1000 years to pay off!";
                return;
            }
        } while (currentCost > 0);
        output.text = "Total Cost: £" + totalCost + " for a £" + houseCost + " house over " + years + " years.";
    }
}

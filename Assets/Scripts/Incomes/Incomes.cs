using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Incomes : MonoBehaviour
{
    [SerializeField] GameObject mob;
    [SerializeField] TMP_Text textMoney;
    [SerializeField] int incomes = 0;
    bool collecting = false;
    float timerCollect = 0.0f;
    int level = 0;

    // Start is called before the first frame update
    void Start()
    {
        incomes = AmountToCollect();

        UpdateText();

        // Calculate date here
    }

    private void Update()
    {
        if (collecting)
        {
            timerCollect += Time.deltaTime;

            if (timerCollect >= 0.05f)
            {
                if (incomes > 100000)
                {
                    incomes -= 10000;
                }
                else if (incomes > 1000)
                {
                    incomes -= 1000;
                }
                else if (incomes >= 100)
                {
                    incomes -= 100;
                }
                else if (incomes >= 10)
                {
                    incomes -= 10;
                }
                else if (incomes >= 1)
                {
                    incomes -= 1;
                }
                else
                {
                    collecting = false;
                }
                timerCollect = 0.0f;
                UpdateText();
            }
        }
    }

    int AmountToCollect()
    {
        API_Incomes lastIncomes = API.GetLastIncomes();

        level = lastIncomes.level;

        if (lastIncomes.passif == null)
        {
            API.PostIncomes(System.DateTime.UtcNow.ToString(), 0);
            lastIncomes = API.GetLastIncomes();
        }

        System.DateTime dateTime = System.DateTime.Parse(lastIncomes.passif);

        System.TimeSpan ts = System.DateTime.UtcNow - dateTime;

        return (int)(ts.TotalSeconds / 10.0f) * level;
    }

    public void UpdateIncomes()
    {
        incomes += 2 * level;
        UpdateText();
        Invoke("Respawn", 1.0f);
    }

    void UpdateText()
    {
        textMoney.text = incomes.ToString();
    }

    public void Collect()
    {
        collecting = true;
        API.PostIncomes(System.DateTime.UtcNow.ToString(), AmountToCollect());
    }

    void Respawn()
    {
        GameObject newObject = Instantiate(mob, new Vector3(2.5f, 1.4f, 0.0f), Quaternion.identity);

        newObject.GetComponent<MobIncome>().incomes = this;
    }
}

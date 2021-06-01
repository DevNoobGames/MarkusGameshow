using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class devNoobPlayer : MonoBehaviour
{
    public float money;
    public TextMeshProUGUI moneyText;
    public GameObject moneyInstructioNText;
    public float health = 100;
    public float totalHealthLost = 0;
    bool canGetHurt;
    public Animation hurtAnim;

    public handGunScr[] gunScripts;

    public int activeGuns = 0;
    public GameObject[] gunObjects;

    public Slider healthSlider;
    public bool bought; //so gun swapping doesnt result in buying twice -_-

    public GameObject deathPanel;
    public GameObject pm;

    private void Start()
    {
        totalHealthLost = 0;
        Time.timeScale = 1;
        bought = false;
        canGetHurt = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pm.activeInHierarchy)
            {
                pm.GetComponent<PauseMenu>().revive();
            }
            else if (!pm.activeInHierarchy)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pm.SetActive(true);
            }
        }
    }

    public void addMoney(float moneyToAdd)
    {
        if (money == 0)
        {
            moneyInstructioNText.SetActive(true);
        }

        money += moneyToAdd;
        setMoneyText();
    }
    public void addHealth(float healthToAdd)
    {
        health += healthToAdd;
        if (health > 100)
        {
            health = 100;
            healthSlider.value = health;
        }
    }


    public void gotHurt(float damage)
    {
        if (canGetHurt)
        {
            totalHealthLost += damage;
            canGetHurt = false;
            hurtAnim.Play();
            health -= damage;
            healthSlider.value = health;
            if (health <= 0)
            {
                Debug.Log("Game over");
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                deathPanel.SetActive(true); 
            }
        }
        StartCoroutine(resetHurt());
    }
    IEnumerator resetHurt()
    {
        yield return new WaitForSeconds(1);
        canGetHurt = true;
    }


    public void setMoneyText()
    {
        moneyText.text = "$" + money.ToString("F0");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("trapSaw"))
        {
            gotHurt(20);
            GetComponent<SC_FPSController>().moveDirection.y = GetComponent<SC_FPSController>().jumpSpeed;
        }
        if (other.CompareTag("pancakeAttack"))
        {
            gotHurt(20);
            GetComponent<SC_FPSController>().moveDirection.y = GetComponent<SC_FPSController>().jumpSpeed;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        gotHurt(5);
        GetComponent<SC_FPSController>().moveDirection.y = GetComponent<SC_FPSController>().jumpSpeed;
    }
}

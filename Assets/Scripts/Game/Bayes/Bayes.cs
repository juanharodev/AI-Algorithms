using System;
using TMPro; 
using UnityEngine;

public class Bayes : MonoBehaviour
{
    [Header("Probabilities")]
    [SerializeField, Range(0.01f,1)] float realHitRate;
    [SerializeField, Range(0.01f,1)] float fakeHitRate;
    [SerializeField] bool isReal;
    float successRate;
    BayesEntry successEntry;
    BayesEntry failureEntry;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI successDisplay;
    [SerializeField] TextMeshProUGUI failureDisplay;
    [SerializeField] TextMeshProUGUI hitDisplay;

    [Header("Visuals")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject hitZone;

    private void Start()
    {
        successEntry = new BayesEntry(0.5f, realHitRate);
        failureEntry = new BayesEntry(0.5f, fakeHitRate);
        successRate = isReal? realHitRate : fakeHitRate;
        UpdateDisplay();
        hitDisplay.text = string.Empty; 
    }

    //Bayes
    public void Next()
    {
        float newHappeningRate = 0;
        //Succes case
        if (UnityEngine.Random.Range(0f, 1f) <= successRate)
        {
            hitDisplay.text = "Hit!";

            newHappeningRate = successEntry.z1 / (successEntry.z1 + failureEntry.z1);

            successEntry.happeningRate = newHappeningRate;
            failureEntry.happeningRate = 1 - newHappeningRate;

            PlaceBullet(true);
        }
        //Failure case
        else
        {
            hitDisplay.text = "Miss!";

            newHappeningRate = failureEntry.z2 / (failureEntry.z2 + successEntry.z2);

            successEntry.happeningRate = 1 - newHappeningRate;
            failureEntry.happeningRate =  newHappeningRate;
            PlaceBullet(false);
        }
        
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        successDisplay.text = (successEntry.happeningRate * 100).ToString("n2") + "% de ser profesional de CS:GO";
        failureDisplay.text = (failureEntry.happeningRate * 100).ToString("n2") + "% de ser un stormtrooper";
    }

    void PlaceBullet(bool hasHit)
    {
        bullet.gameObject.SetActive(hasHit);
        if (!hasHit) {return;}
        
        float radius = UnityEngine.Random.Range(-hitZone.transform.localScale.x + bullet.transform.localScale.x, hitZone.transform.localScale.x - bullet.transform.localScale.x)/2;
        float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

        float x = (radius * Mathf.Cos(angle)) + hitZone.transform.position.x ;
        float y = (radius * Mathf.Sin(angle)) + hitZone.transform.position.y ;

        bullet.transform.position = new Vector3(x,y,0);
    }
}

[Serializable]
    class BayesEntry
    {
        public float happeningRate { get;  set; }
        public float succesRate { get; private set; }
        public float failureRate => 1 - succesRate;
        public float z1 => happeningRate * succesRate;
        public float z2 => happeningRate * failureRate;

        public BayesEntry(float _happeningRate, float _succesRate)
        {
            happeningRate = _happeningRate;
            succesRate = _succesRate;
        }
    }


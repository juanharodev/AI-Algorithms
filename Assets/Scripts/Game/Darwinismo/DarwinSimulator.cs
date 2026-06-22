using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DarwinSimulator : MonoBehaviour
{

    public static DarwinSimulator Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [Header("Sprites")]
    public Sprite square;
    public Sprite triangle;
    public Sprite circle;
    public Sprite hexagon;

    [Header("Generation")]
    public Evolving prefabSubject;
    public RectTransform container;
    public int population;
    [Range(0, 1)] public float mutationPercentage;
    int currentGen = 0;

    [Header("UI")]
    public TMP_InputField populationAmount;
    public TextMeshProUGUI genDisplay;
    public TextMeshProUGUI mutationPercentageDisplay;
    public Slider mutationPercentageSlider;
    public TextMeshProUGUI perfectDisplay;

    List<Evolving> subjects;

    SpecimenValues parent_1;
    SpecimenValues parent_2;

    [SerializeField] Evolving parent_1_display;
    [SerializeField] Evolving parent_2_display;

    Evolving squareParent_1;
    Evolving squareParent_2;

    Evolving triangleParent_1;
    Evolving triangleParent_2;

    Evolving circleParent_1;
    Evolving circleParent_2;

    Evolving hexagonParent_1;
    Evolving hexagonParent_2;

    List<GameObject> perfects;

    bool isAuto = true;

    #region Modify values
    private void Start()
    {
        SetPopulation(population.ToString());
        SetMutationPercentage(mutationPercentage);
        SetCurrentGen(0);
        perfects = new List<GameObject>();
    }

    public void SetPopulation(string population)
    {
        population = population.Trim();
        if (Int32.TryParse(population, out int pop))
        {
            this.population = pop;
        }
        populationAmount.text = this.population.ToString();
    }

    public void SetMutationPercentage(float mutationPercentage)
    {
        this.mutationPercentage = mutationPercentage;
        mutationPercentageDisplay.text = (mutationPercentage * 100).ToString("n2") + "% of mutation";
        mutationPercentageSlider.value = this.mutationPercentage;
    }

    public void SetIsAuto(bool isAuto)
    {
        this.isAuto = isAuto;

    }
    #endregion

    #region Evolution
    public void GeneratePopulation()
    {
        if (population < 2)
        {
            perfectDisplay.text = "Population needs to be bigger than 2";
            return;
        }
        //Clear
        parent_1_display.Clear();
        parent_2_display.Clear();
        subjects?.Clear();
        perfects?.Clear();
        parent_1 = null;
        parent_2 = null;
        squareParent_1 = null;
        squareParent_2 = null;
        triangleParent_1 = null;
        triangleParent_2 = null;
        circleParent_1 = null;
        circleParent_2 = null;
        hexagonParent_1 = null;
        hexagonParent_2 = null;
        SetCurrentGen(0);
        for(int i = container.childCount - 1; i>=0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }
        perfectDisplay.text = string.Empty;

        //Generate new population
        subjects = new List<Evolving>();
        GameObject instance;
        Evolving newSubject;
        for (int i = 0; i < population; i++)
        {
            instance = Instantiate(prefabSubject.gameObject, container);
            newSubject = instance.GetComponent<Evolving>();
            newSubject.name = "Subject# " + i;
            subjects.Add(newSubject);
            RandomMutate(newSubject);
        }
        FindParents();
    }

    public void NextGeneration()
    {
        if(subjects == null || perfects.Count > 0) { return; }

        while (perfects.Count <= 0)
        {
            SetCurrentGen(currentGen + 1);
            for (int i = 0; i < population; i++)
            {
                Evolve(subjects[i], (i < population / 2));
            }
            FindParents();
            if (!isAuto) { break; }
        }

        if (perfects.Count <= 0) { return; }
        string perfectText = "Especimen(es) perfecto(s) encontrado(s) en la generacion #" + currentGen + "\n";
        for (int i = 0; i < perfects.Count; i++) 
        {
            perfectText += (perfects[i].name + " es perfecto\n");
        }
        perfectDisplay.text = perfectText;
    }
    
    void SetCurrentGen(int gen)
    {
        this.currentGen = gen;
        genDisplay.text = "Gen #" + currentGen; 
    }

    void RandomMutate(Evolving _subject)
    {
        _subject.Change(GenerateMutation(), GenerateMutation(), GenerateMutation(), GenerateMutation(), GenerateMutation(), GenerateMutation());
    }

    void Evolve(Evolving _subject, bool flip)
    {
        if (flip)
        {
            _subject.Change(GenerateEvolution(parent_1.headValue),
                GenerateEvolution(parent_1.bodyValue),
                GenerateEvolution(parent_1.lArmValue),
                GenerateEvolution(parent_2.rArmValue),
                GenerateEvolution(parent_2.lLegValue),
                GenerateEvolution(parent_2.rLegValue)
            );
        }
        else
        {
            _subject.Change(GenerateEvolution(parent_2.headValue),
            GenerateEvolution(parent_2.bodyValue),
            GenerateEvolution(parent_2.lArmValue),
            GenerateEvolution(parent_1.rArmValue),
            GenerateEvolution(parent_1.lLegValue),
            GenerateEvolution(parent_1.rLegValue)
            );
        }
        
    }

    int GenerateMutation()
    {
        return UnityEngine.Random.Range(0, 4);
    }

    int GenerateEvolution(int originalValue)
    {
        if(UnityEngine.Random.Range(0f,1f) <= mutationPercentage)
        {
            return GenerateMutation();
        }
        return originalValue;
    }

    public void PerfectFound(GameObject perfect)
    {
        perfects.Add(perfect);
    }

    public void ApplyForParent(Evolving _subject)
    {
        //Square
        if (squareParent_1 == null)
        {
            squareParent_1 = _subject;
        }
        else if (_subject.totalSquares > squareParent_1.totalSquares)
        {
            squareParent_2 = squareParent_1;
            squareParent_1 = _subject;
        }
        else if (squareParent_2 == null) 
        {
            squareParent_2 = _subject;
        }
        else if (_subject.totalSquares > squareParent_2.totalSquares)
        {
            squareParent_2 = _subject;
        }

        //Triangle 
        if (triangleParent_1 == null)
        {
            triangleParent_1 = _subject;
        }
        else if (_subject.totalTriangles > triangleParent_1.totalTriangles)
        {
            triangleParent_2 = triangleParent_1;
            triangleParent_1 = _subject;
        }
        else if (triangleParent_2 == null)
        {
            triangleParent_2 = _subject;
        }
        else if (_subject.totalTriangles > triangleParent_2.totalTriangles)
        {
            triangleParent_2 = _subject;
        }

        //Circles
        if (circleParent_1 == null)
        {
            circleParent_1 = _subject;
        }
        else if (_subject.totalCirlces > circleParent_1.totalCirlces)
        {
            circleParent_2 = circleParent_1;
            circleParent_1 = _subject;
        }
        else if (circleParent_2 == null)
        {
            circleParent_2 = _subject;
        }
        else if (_subject.totalCirlces > circleParent_2.totalCirlces)
        {
            circleParent_2 = _subject;
        }

        //Hexagons
        if (hexagonParent_1 == null)
        {
            hexagonParent_1 = _subject;
        }
        else if (_subject.totalHexagon > hexagonParent_1.totalHexagon)
        {
            hexagonParent_2 = hexagonParent_1;
            hexagonParent_1 = _subject;
        }
        else if (hexagonParent_2 == null)
        {
            hexagonParent_2 = _subject;
        }
        else if (_subject.totalHexagon > hexagonParent_2.totalHexagon)
        {
            hexagonParent_2 = _subject;
        }
    }
    
    void FindParents()
    {
        //We finding squares
        if(squareParent_1.totalSquares >= triangleParent_1.totalTriangles && squareParent_1.totalSquares >= circleParent_1.totalCirlces && squareParent_1.totalSquares >= hexagonParent_1.totalHexagon)
        {
            parent_1 = new SpecimenValues(squareParent_1.values.headValue, squareParent_1.values.bodyValue, squareParent_1.values.lArmValue, squareParent_1.values.rArmValue, squareParent_1.values.lLegValue, squareParent_1.values.rLegValue);
            parent_2 = new SpecimenValues(squareParent_2.values.headValue, squareParent_2.values.bodyValue, squareParent_2.values.lArmValue, squareParent_2.values.rArmValue, squareParent_2.values.lLegValue, squareParent_2.values.rLegValue);
        }

        //We finding triangles 
        else if (triangleParent_1.totalTriangles >= circleParent_1.totalCirlces && triangleParent_1.totalTriangles >= hexagonParent_1.totalHexagon)
        {
            parent_1 = new SpecimenValues(triangleParent_1.values.headValue, triangleParent_1.values.bodyValue, triangleParent_1.values.lArmValue, triangleParent_1.values.rArmValue, triangleParent_1.values.lLegValue, triangleParent_1.values.rLegValue);
            parent_2 = new SpecimenValues(triangleParent_2.values.headValue, triangleParent_2.values.bodyValue, triangleParent_2.values.lArmValue, triangleParent_2.values.rArmValue, triangleParent_2.values.lLegValue, triangleParent_2.values.rLegValue);
        }

        //We finding circles
        else if (circleParent_1.totalCirlces >= hexagonParent_1.totalHexagon)
        {
            parent_1 = new SpecimenValues(circleParent_1.values.headValue, circleParent_1.values.bodyValue, circleParent_1.values.lArmValue, circleParent_1.values.rArmValue, circleParent_1.values.lLegValue, circleParent_1.values.rLegValue);
            parent_2 = new SpecimenValues(circleParent_2.values.headValue, circleParent_2.values.bodyValue, circleParent_2.values.lArmValue, circleParent_2.values.rArmValue, circleParent_2.values.lLegValue, circleParent_2.values.rLegValue);
            return;
        }
        
        //We finding hexagons
        else
        {
            parent_1 = new SpecimenValues(hexagonParent_1.values.headValue, hexagonParent_1.values.bodyValue, hexagonParent_1.values.lArmValue, hexagonParent_1.values.rArmValue, hexagonParent_1.values.lLegValue, hexagonParent_1.values.rLegValue);
            parent_2 = new SpecimenValues(hexagonParent_2.values.headValue, hexagonParent_2.values.bodyValue, hexagonParent_2.values.lArmValue, hexagonParent_2.values.rArmValue, hexagonParent_2.values.lLegValue, hexagonParent_2.values.rLegValue);
            return;
        }
          parent_1_display.Change(parent_1.bodyValue, parent_1.bodyValue,parent_1.lArmValue,parent_1.rArmValue,parent_1.lLegValue,parent_1.rLegValue);
          parent_2_display.Change(parent_2.bodyValue, parent_2.bodyValue,parent_2.lArmValue,parent_2.rArmValue,parent_2.lLegValue,parent_2.rLegValue);
    }
    #endregion
}

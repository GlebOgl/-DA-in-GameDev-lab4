using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Perceptron : MonoBehaviour
{

    public TrainingSet[] Ts;

    public int Value;

    public Material True;
    public Material False;

    public GameObject Cube;

    private class PerceptronClass
    {
        public TrainingSet[] ts;
        double[] weights = { 0, 0 };
        double bias = 0;
        double totalError = 0;

        double DotProductBias(double[] v1, double[] v2)
        {
            if (v1 == null || v2 == null)
                return -1;

            if (v1.Length != v2.Length)
                return -1;

            double d = 0;
            for (int x = 0; x < v1.Length; x++)
            {
                d += v1[x] * v2[x];
            }

            d += bias;

            return d;
        }

        double CalcOutput(int i)
        {
            double dp = DotProductBias(weights, ts[i].input);
            if (dp > 0) return (1);
            return (0);
        }

        void InitialiseWeights()
        {
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = Random.Range(-1.0f, 1.0f);
            }
            bias = Random.Range(-1.0f, 1.0f);
        }

        void UpdateWeights(int j)
        {
            double error = ts[j].output - CalcOutput(j);
            totalError += Mathf.Abs((float)error);
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = weights[i] + error * ts[j].input[i];
            }
            bias += error;
        }

        public double CalcOutput(double i1, double i2)
        {
            double[] inp = new double[] { i1, i2 };
            double dp = DotProductBias(weights, inp);
            if (dp > 0) return (1);
            return (0);
        }

        public void Train(int epochs, TrainingSet[] ts)
        {
            this.ts = ts;
            InitialiseWeights();

            for (int e = 0; e < epochs; e++)
            {
                totalError = 0;
                for (int t = 0; t < ts.Length; t++)
                {
                    UpdateWeights(t);
                    //Debug.Log("W1: " + (weights[0]) + " W2: " + (weights[1]) + " B: " + bias);
                }
                //Debug.Log("TOTAL ERROR: " + totalError);
            }
        }

        public double GetStatistics(int epochs, TrainingSet[] ts, int range)
        {
            var statArray = new double[range];
            for (int i = 0; i < range; i++)
            {
                Train(epochs, ts);
                statArray[i] = totalError;
            }
            var res = 0.0;
            foreach (var item in statArray)
            {
                res += item;
            }
            return res / range;
            
        }
    }

    IEnumerator UpdateCoroutine()
    {
        yield return new WaitForSeconds(2);
        this.transform.position = pos;
        this.gameObject.GetComponent<Renderer>().material = material;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        var Cube = collision.gameObject.GetComponent<Perceptron>();
        if (Cube)
        {
            if (perceptron.CalcOutput(this.Value, Cube.Value) == 1)
            {
                this.gameObject.GetComponent<Renderer>().material = True;
            }
            else
            {
                this.gameObject.GetComponent<Renderer>().material = False;
            }
            StartCoroutine(UpdateCoroutine());
        }
    }

    private PerceptronClass perceptron;
    private Vector3 pos;
    private Material material;

    void Start()
    {
        perceptron = new PerceptronClass();
        perceptron.Train(8, Ts);

        pos = this.transform.position;
        material = this.gameObject.GetComponent<Renderer>().material;
    }

    void Update()
    {

    }
}
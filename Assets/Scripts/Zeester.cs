//import java.util.Random;
using UnityEngine;
using System.Collections.Generic;


public class Zeester
{

    public int NumberOfLegs, LegLength, Amp, Freq, Phase, score, Hue, Sat, Val;
    private double chnc;



    public GameObject GameObject;

    private int GENELENGHT = 25;

    private bool[] gene ;
  //  Random r = new Random();

    //Variabelen:
    // # pootjes                        8 (3 bits)
    // lengte pootjes                   8 (3 bits)
    // gewicht hoofd                    8 (3 bits)
    // snelheid benen? / frequentie     8 (3 bits)
    // fase verschil                    16 (4 bits)
    // hue, saturation, brightness      8 (3 bit) x3
    // Alles samen: gene van 16 bools

    public Zeester()
    {
        gene = new bool[GENELENGHT];
        chance();
                setVariables();
     


    }

    public Zeester(bool[] gene)
    {
       // gene = new bool[GENELENGHT];
        this.gene = gene;
        setVariables();
    }

    public Zeester(bool[] gene, int score)
    {
        this.gene = gene;
        this.score = score;
        setVariables();
    }


    public void chance()
    {
        for (int i = 0; i < gene.Length; i++)
        {
           // gene[i] = r.nextbool();
           gene[i]=  Random.value > 0.5f;
        }
    }

    public void setVariables()
    {
        NumberOfLegs = 1;
        int binary = 1;
        int i = 0;
        while (i < 3)
        {
            if (gene[i])
            {
                NumberOfLegs += binary;
            }
            binary = binary * 2;
            i++;
        }
        LegLength = 1;
        binary = 1;
        while (i < 6)
        {
            if (gene[i])
            {
                LegLength += binary;
            }
            binary = binary * 2;
            i++;
        }
        Amp = 0;
        binary = 1;
        while (i < 9)
        {
            if (gene[i])
            {
                Amp += binary;
            }
            binary = binary * 2;
            i++;
        }

        Freq = 0;
        binary = 1;
        while (i < 12)
        {
            if (gene[i])
            {
                Freq += binary;
            }
            binary = binary * 2;
            i++;
        }
        Phase = 0;
        binary = 1;
        while (i < 16)
        {
            if (gene[i])
            {
                Phase += binary;
            }
            binary = binary * 2;
            i++;
        }
        Hue = 0;
        binary = 1;
        while (i < 19)
        {
            if (gene[i])
            {
                Hue += binary;
            }
            binary = binary * 2;
            i++;
        }
        Sat = 0;
        binary = 1;
        while (i < 22)
        {
            if (gene[i])
            {
                Sat += binary;
            }
            binary = binary * 2;
            i++;
        }
        Val = 0;
        binary = 1;
        while (i < 25)
        {
            if (gene[i])
            {
                Val += binary;
            }
            binary = binary * 2;
            i++;
        }
    }

    public void setScore(int score)
    {
        this.score = score;
    }

    public int getScore()
    {
        return score;
    }

    public bool[] getGene()
    {
        return gene;
    }

    public string getGeneAsString()
    {

        string geneString = "";
        for (int g = 0; g < GENELENGHT; g++)
        {
            geneString += gene[g] ? "1" : "0";
        }
        return geneString;

    }

    public double getChance()
    {
        return chnc;
    }

    public void setChance(double chance)
    {
        this.chnc = chance;
    }

    public int getPootjes()
    {
        return NumberOfLegs;
    }

    public int getLengtePootjes()
    {
        return LegLength;
    }

    public int getSnelheidBenen()
    {
        return Freq;
    }

    public int getGewichtHoofd()
    {
        return Amp;
    }

    public int getFase()
    {
        return Phase;
    }

    public int getHue()
    {
        return Hue;
    }

    public int getBrightness()
    {
        return Val;
    }

    public int getSaturation()
    {
        return Sat;
    }

   // @Override

    public string toString()
    {
        return ("p: " + NumberOfLegs + " lp: " + LegLength + " gh: " + Amp + " sp: " + Freq + " f: " + Phase);
    }

}


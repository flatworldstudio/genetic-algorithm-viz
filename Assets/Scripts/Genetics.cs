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


public class ZeeSterEvolutie
{

    private  int POPULATION = 10;
    private  int GENELENGHT = 25;

    private Zeester[] pop ;
    private int totalValue;
    Random r = new Random();

    public ZeeSterEvolutie()
    {

        pop = new Zeester[POPULATION];
        makePop();

        for (int i = 0; i < POPULATION; i++)
        {
            Debug.Log(pop[i].toString());

            //System.out.println(pop[i]);
        }





    }


    public Zeester[] GetPop()
    {
        return pop;
    }

    public Zeester[] Evolution (int[] score)
    {
        totalValue = 0;
        giveScore(score);

        bubbleSort();

        if (false)
        {
            totalValue = 0;
            for (int i = 9; i >= 0; i--)
            {
                pop[i].setScore(i);
                totalValue += i;
            }
        }


        assignChance();


        //  Zeester[] ster;

        List<Zeester> temp = new List<Zeester>();


        while (temp.Count < pop.Length)
        {
            temp.Add(mate(makeParent(), makeParent()));
        }

        //Empty pop
        pop = new Zeester[POPULATION];

        for (int j = 0; j < POPULATION; j++)
        {
            pop[j] = new Zeester(temp[j].getGene());
        }

        //totalValue = 0;

        return pop;
       





    }

    public void makePop()
    {
        for (int i = 0; i < pop.Length; i++)
        {
            pop[i] = new Zeester();
        }
    }

    public void giveScore(int[] score)
    {
        for (int i = 0; i < pop.Length; i++)
        {
            pop[i].setScore(score[i]);
            totalValue += score[i];
        }
    }

    public void assignChance()
    {
        double totalChance = 0;
        for (int i = 0; i < POPULATION; i++)
        {
            double currentChance = (double)pop[i].getScore() / (double)totalValue;
            pop[i].setChance(totalChance + currentChance);
            totalChance += currentChance;
            //System.out.println(totalChance);
        }
    }

    public void bubbleSort()
    {
        int k;
        for (int m = POPULATION; m >= 0; m--)
        {
            for (int i = 0; i < POPULATION - 1; i++)
            {
                k = i + 1;
                if (pop[i].getScore() < pop[k].getScore())
                {
                    swapNumbers(i, k);
                }
            }
        }
    }

    private void swapNumbers(int i, int j)
    {
        Zeester temp = new Zeester(pop[i].getGene(), pop[i].getScore());
        pop[i] = new Zeester(pop[j].getGene(), pop[j].getScore());
        pop[j] = new Zeester(temp.getGene(), temp.getScore());
    }


    public Zeester makeParent()
    {
        double chance = Random.value;

        Zeester parent = new Zeester();
        for (int i = 0; i < POPULATION; i++)
        {
            if (chance < pop[i].getChance())
            {
                parent = new Zeester(pop[i].getGene());
                break;
            }
        }
        return parent;
    }

    //Mates to knapsacks
    public Zeester mate(Zeester p1, Zeester p2)
    {

        Zeester child;

        child = crossover(p1.getGene(), p2.getGene());

        if (Random.value > 0.8f)
            child = mutation(child.getGene());

        return child;
    }

    //One point crossover between two genes
    public Zeester crossover(bool[] gene1, bool[] gene2)
    {

        bool[] newgene = new bool[GENELENGHT];
        for (int i = 0; i < gene1.Length; i++)
        {
            if (Random.value>0.5f)
            {
                newgene[i] = gene2[i];
            }
            else
                newgene[i] = gene1[i];
        }

        return new Zeester(newgene);

    }

    // Point mutation
    public Zeester mutation(bool[] gene)
    {
        int amount = Random.Range(1,10);
        for (int i = 0; i< amount; i++){
            int point = Random.Range(0,gene.Length);
            gene[point] = !gene[point];
        }
        return new Zeester(gene);
    }




    public int[] battle()
    {
        // pop

        int[] toReturn = { 5, 5, 10, 20, 40, 2, 3, 5, 5, 5 };
        return toReturn;
    }


    public int[] randomEvolution()
    {
        // TO DO
        return new int[10];
    }



    //public static void main(String args[])
    //{
    //    new ZeeSterEvolutie();
    //}




    // make pop method
}

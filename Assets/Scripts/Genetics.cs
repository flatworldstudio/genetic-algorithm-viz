//import java.util.Random;
using UnityEngine;
using System.Collections.Generic;


public class ZeeSterEvolutie
{

    public int POPULATION = 10;
    public int GENELENGHT = 25;


    /**
     * Current and previous generations
     */
    private Zeester[] pop;
    public List<Generation> familyTree = new List<Generation>();



    private int totalValue;
    Random r = new Random();

    public ZeeSterEvolutie()
    {

        pop = new Zeester[POPULATION];
        makePop();

        for (int i = 0; i < POPULATION; i++)
        {
            Debug.Log(pop[i].toString());
        }
    }


    public Zeester[] GetPop()
    {
        return pop;
    }

    public void SaveGeneration(int[] score)
    {
        /**
      * Find Zeester with highest score
      */
        var highestScore = 0;
        Zeester highestScoringZeester = null;
        for (int i = 0; i < POPULATION; i++)
        {
            Zeester z = pop[i];
            int s = score[i];
            if (s > highestScore)
            {
                highestScore = s;
                highestScoringZeester = z;
            }
        }

        // Save!
        var newGeneration = new Generation(pop, highestScoringZeester, score);
        familyTree.Add(newGeneration);
    }

    public Zeester[] Evolution (int[] score)
    {
        totalValue = 0;
        giveScore(score);
        bubbleSort();
        assignChance();
        SaveGeneration(score);

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

}

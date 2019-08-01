
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Generation
{
    public List<Zeester> population;
    public Zeester winner;
    private int[] scores;

    public Generation(Zeester[] population, Zeester winner, int[] scores)
    {
        this.population = population.ToList();
        this.winner = winner;
        this.scores = scores;

    }
}

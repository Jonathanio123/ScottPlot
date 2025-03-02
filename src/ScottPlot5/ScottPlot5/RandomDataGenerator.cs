﻿namespace ScottPlot;

public class RandomDataGenerator
{
    readonly Random Rand;

    /// <summary>
    /// Random seed
    /// </summary>
    public RandomDataGenerator()
    {
        Rand = new();
    }

    /// <summary>
    /// Defined seed
    /// </summary>
    public RandomDataGenerator(int seed = 0)
    {
        Rand = new Random(seed);
    }

    public double[] RandomSin(int count)
    {
        double mult = Math.Pow(2, 1 + Rand.NextDouble() * 10);
        double offset = mult * (Rand.NextDouble() - .5);
        double oscillations = 1 + Rand.NextDouble() * 5;
        double phase = Rand.NextDouble() * Math.PI * 2;
        return Generate.Sin(count, mult, offset, oscillations, phase);
    }

    public double RandomNumber(double min, double max)
    {
        double span = max - min;
        return min + Rand.NextDouble() * span;
    }

    public double[] RandomWalk(int pointCount, double mult = 1, double offset = 0)
    {
        double[] data = new double[pointCount];
        data[0] = offset;
        for (int i = 1; i < data.Length; i++)
            data[i] = data[i - 1] + (Rand.NextDouble() * 2 - 1) * mult;
        return data;
    }

    public IEnumerable<OHLC> RandomOHLCs(int count)
    {
        DateTime[] dates = Generate.DateTime.Weekdays(count);
        TimeSpan span = TimeSpan.FromDays(1);

        double mult = 1;

        OHLC[] ohlcs = new OHLC[count];
        double open = RandomNumber(150, 250);
        for (int i = 0; i < count; i++)
        {
            double close = open + RandomNumber(-mult, mult);
            double high = Math.Max(open, close) + RandomNumber(0, mult);
            double low = Math.Min(open, close) - RandomNumber(0, mult);
            ohlcs[i] = new OHLC(open, high, low, close, dates[i], span);
            open = close + RandomNumber(-mult / 2, mult / 2);
        }

        return ohlcs;
    }
}

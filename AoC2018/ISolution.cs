namespace AoC2018
{
    interface ISolution
    {
        void LoadInput(params string[] files);

        object Part1();

        object Part2();
    }
}

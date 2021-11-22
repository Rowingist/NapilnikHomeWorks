using System;

class Example_Task_2_HW
{
    public static int GetValidated(int a, int b, int c)
    {
        if (a < b)
            return b;
        else if (a > c)
            return c;
        else
            return a;
    }
}

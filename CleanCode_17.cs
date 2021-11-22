using System;

class Example_Task_17_HW
{
    public static void Initialize()
    {
        //Создание объекта на карте
    }

    public static void Randomize()
    {
        _chance = Random.Range(0, 100);
    }

    public static int GetFullSalary(int hoursWorked)
    {
        return _hourlyRate * hoursWorked;
    }
}

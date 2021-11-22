using System;

class Example_Task_10_HW
{
    private int _bullets;
    private int _emptyCage = 0;
    private int _bulletsPerShoot = 1;

    public bool CanShoot() => _bullets > _emptyCage;

    public void Shoot() => _bullets -= _bulletsPerShoot;
}
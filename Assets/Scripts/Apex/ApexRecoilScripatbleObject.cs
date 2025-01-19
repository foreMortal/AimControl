using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecoil", menuName = "ScriptableObjects/ApexRecoil/ApexRecoilScriptableObject", order = 1)]
public class ApexRecoilScripatbleObject : ScriptableObject
{
    [SerializeField] private string weaponName;
    [SerializeField] private float[] s1, s2, s3, s4, s5, s6, s7, s8, s9, s10,
        s11, s12, s13, s14, s15, s16, s17, s18, s19, s20, s21, s22, s23, s24,
        s25, s26, s27, s28, s29, s30, s31, s32, s33, s34, s35, s36, s37, s38,
        s39, s40, s41, s42, s43, s44, s45, s46, s47, s48, s49, s50, s51, s52,
        s53, s54, s55, s56, s57, s58, s59, s60, s61;
    [SerializeField] private int bulletsCount;
    [SerializeField] private float timebetweenShots;
    [SerializeField] private bool haveDelay;
    [SerializeField] private float delay;
    [SerializeField] private bool burst;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private int bulletsInBurst;
    [SerializeField] private float compinsieIn = 0.1f;
    [SerializeField] private bool single;
    [SerializeField] private bool changesTimeBetweenShots;
    [SerializeField] private int weaponIndex;

    public string WeaponName { get { return weaponName; } }
    public int BulletsCount { get { return bulletsCount; } }
    public float TimeBetweenShots { get { return timebetweenShots; } }
    public bool HaveDelay { get { return haveDelay; } }
    public float DelayTime { get { return delay; } }
    public bool Burst { get { return burst; } }
    public float TimeBetweenBursts { get { return timeBetweenBursts; } }
    public int BulletsInBurst { get { return bulletsInBurst; } }
    public float CompinsieIn { get { return compinsieIn; } }
    public bool Single { get { return single; } }
    public bool ChangesTimeBetweenShots { get { return changesTimeBetweenShots; } }
    public int WeaponIndex { get { return weaponIndex; } }
    public float BulletsSpeed;

    public float[][] GetRecoil()
    {
        List<float[]> bullets = new List<float[]>();
        if (bulletsCount >= 1)
            bullets.Add(s1);
        if (bulletsCount >= 2)
            bullets.Add(s2);
        if (bulletsCount >= 3)
            bullets.Add(s3);
        if (bulletsCount >= 4)
            bullets.Add(s4);
        if (bulletsCount >= 5)
            bullets.Add(s5);
        if (bulletsCount >= 6)
            bullets.Add(s6);
        if (bulletsCount >= 7)
            bullets.Add(s7);
        if (bulletsCount >= 8)
            bullets.Add(s8);
        if (bulletsCount >= 9)
            bullets.Add(s9);
        if (bulletsCount >= 10)
            bullets.Add(s10);
        if (bulletsCount >= 11)
            bullets.Add(s11);
        if (bulletsCount >= 12)
            bullets.Add(s12);
        if (bulletsCount >= 13)
            bullets.Add(s13);
        if (bulletsCount >= 14)
            bullets.Add(s14);
        if (bulletsCount >= 15)
            bullets.Add(s15);
        if (bulletsCount >= 16)
            bullets.Add(s16);
        if (bulletsCount >= 17)
            bullets.Add(s17);
        if (bulletsCount >= 18)
            bullets.Add(s18);
        if (bulletsCount >= 19)
            bullets.Add(s19);
        if (bulletsCount >= 20)
            bullets.Add(s20);
        if (bulletsCount >= 21)
            bullets.Add(s21);
        if (bulletsCount >= 22)
            bullets.Add(s22);
        if (bulletsCount >= 23)
            bullets.Add(s23);
        if (bulletsCount >= 24)
            bullets.Add(s24);
        if (bulletsCount >= 25)
            bullets.Add(s25);
        if (bulletsCount >= 26)
            bullets.Add(s26);
        if (bulletsCount >= 27)
            bullets.Add(s27);
        if (bulletsCount >= 28)
            bullets.Add(s28);
        if (bulletsCount >= 29)
            bullets.Add(s29);
        if (bulletsCount >= 30)
            bullets.Add(s30);
        if (bulletsCount >= 31)
            bullets.Add(s31);
        if (bulletsCount >= 32)
            bullets.Add(s32);
        if (bulletsCount >= 33)
            bullets.Add(s33);
        if (bulletsCount >= 34)
            bullets.Add(s34);
        if (bulletsCount >= 35)
            bullets.Add(s35);
        if (bulletsCount >= 36)
            bullets.Add(s36);
        if (bulletsCount >= 37)
            bullets.Add(s37);
        if (bulletsCount >= 38)
            bullets.Add(s38);
        if (bulletsCount >= 39)
            bullets.Add(s39);
        if (bulletsCount >= 40)
            bullets.Add(s40);
        if (bulletsCount >= 41)
            bullets.Add(s41);
        if (bulletsCount >= 42)
            bullets.Add(s42);
        if (bulletsCount >= 43)
            bullets.Add(s43);
        if (bulletsCount >= 44)
            bullets.Add(s44);
        if (bulletsCount >= 45)
            bullets.Add(s45);
        if (bulletsCount >= 46)
            bullets.Add(s46);
        if (bulletsCount >= 47)
            bullets.Add(s47);
        if (bulletsCount >= 48)
            bullets.Add(s48);
        if (bulletsCount >= 49)
            bullets.Add(s49);
        if (bulletsCount >= 50)
            bullets.Add(s50);
        if (bulletsCount >= 51)
            bullets.Add(s51);
        if (bulletsCount >= 52)
            bullets.Add(s52);
        if (bulletsCount >= 53)
            bullets.Add(s53);
        if (bulletsCount >= 54)
            bullets.Add(s54);
        if (bulletsCount >= 55)
            bullets.Add(s55);
        if (bulletsCount >= 56)
            bullets.Add(s56);
        if (bulletsCount >= 57)
            bullets.Add(s57);
        if (bulletsCount >= 58)
            bullets.Add(s58);
        if (bulletsCount >= 59)
            bullets.Add(s59);
        if (bulletsCount >= 60)
            bullets.Add(s60);
        return bullets.ToArray();
    }
    public float[][] GetRecoil(int count)
    {
        List<float[]> bullets = new List<float[]>();
        if (count >= 1)
            bullets.Add(s1);
        if (count >= 2)
            bullets.Add(s2);
        if (count >= 3)
            bullets.Add(s3);
        if (count >= 4)
            bullets.Add(s4);
        if (count >= 5)
            bullets.Add(s5);
        if (count >= 6)
            bullets.Add(s6);
        if (count >= 7)
            bullets.Add(s7);
        if (count >= 8)
            bullets.Add(s8);
        if (count >= 9)
            bullets.Add(s9);
        if (count >= 10)
            bullets.Add(s10);
        if (count >= 11)
            bullets.Add(s11);
        if (count >= 12)
            bullets.Add(s12);
        if (count >= 13)
            bullets.Add(s13);
        if (count >= 14)
            bullets.Add(s14);
        if (count >= 15)
            bullets.Add(s15);
        if (count >= 16)
            bullets.Add(s16);
        if (count >= 17)
            bullets.Add(s17);
        if (count >= 18)
            bullets.Add(s18);
        if (count >= 19)
            bullets.Add(s19);
        if (count >= 20)
            bullets.Add(s20);
        if (count >= 21)
            bullets.Add(s21);
        if (count >= 22)
            bullets.Add(s22);
        if (count >= 23)
            bullets.Add(s23);
        if (count >= 24)
            bullets.Add(s24);
        if (count >= 25)
            bullets.Add(s25);
        if (count >= 26)
            bullets.Add(s26);
        if (count >= 27)
            bullets.Add(s27);
        if (count >= 28)
            bullets.Add(s28);
        if (count >= 29)
            bullets.Add(s29);
        if (count >= 30)
            bullets.Add(s30);
        if (count >= 31)
            bullets.Add(s31);
        if (count >= 32)
            bullets.Add(s32);
        if (count >= 33)
            bullets.Add(s33);
        if (count >= 34)
            bullets.Add(s34);
        if (count >= 35)
            bullets.Add(s35);
        if (count >= 36)
            bullets.Add(s36);
        if (count >= 37)
            bullets.Add(s37);
        if (count >= 38)
            bullets.Add(s38);
        if (count >= 39)
            bullets.Add(s39);
        if (count >= 40)
            bullets.Add(s40);
        if (count >= 41)
            bullets.Add(s41);
        if (count >= 42)
            bullets.Add(s42);
        if (count >= 43)
            bullets.Add(s43);
        if (count >= 44)
            bullets.Add(s44);
        if (count >= 45)
            bullets.Add(s45);
        if (count >= 46)
            bullets.Add(s46);
        if (count >= 47)
            bullets.Add(s47);
        if (count >= 48)
            bullets.Add(s48);
        if (count >= 49)
            bullets.Add(s49);
        if (count >= 50)
            bullets.Add(s50);
        if (count >= 51)
            bullets.Add(s51);
        if (count >= 52)
            bullets.Add(s52);
        if (count >= 53)
            bullets.Add(s53);
        if (count >= 54)
            bullets.Add(s54);
        if (count >= 55)
            bullets.Add(s55);
        if (count >= 56)
            bullets.Add(s56);
        if (count >= 57)
            bullets.Add(s57);
        if (count >= 58)
            bullets.Add(s58);
        if (count >= 59)
            bullets.Add(s59);
        if (count >= 60)
            bullets.Add(s60);
        return bullets.ToArray();
    }
}

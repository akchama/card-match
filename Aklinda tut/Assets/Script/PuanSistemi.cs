using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuanSistemi
{
    private static PuanSistemi instance = null;

    public static PuanSistemi GetInstance()
    {
        if (instance == null)
            instance = new PuanSistemi();
        return instance;
    }

    public int GetPuan()
    {
        int puan = PlayerPrefs.GetInt("puan", 0);
        return puan;
    }

    public void SetPuan(int _puan)
    {
        PlayerPrefs.SetInt("puan", _puan);
    }

    public void PuanArttir()
    {
        PlayerPrefs.SetInt("puan", PlayerPrefs.GetInt("puan", 0) + 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gondr;

public class RadarTest : MonoBehaviour
{
    [SerializeField] private StatRadarChart _statRadarChart = null;

    private void Start()
    {
        Stats stats = new Stats(10, 12);
        _statRadarChart.SetStats(stats);

        GondrDebug.CreateButton(new Vector2(700, 400), "Atk++", new Vector2(50, 20), () =>
        {
            stats.IncStatAmount(Stats.Type.Attack);
        });

        GondrDebug.CreateButton(new Vector2(770, 400), "Atk--", new Vector2(50, 20), () =>
        {
            stats.DecStatAmount(Stats.Type.Attack);
        });

        GondrDebug.CreateButton(new Vector2(700, 360), "Def++", new Vector2(50, 20), () =>
        {
            stats.IncStatAmount(Stats.Type.Defence);
        });

        GondrDebug.CreateButton(new Vector2(770, 360), "Def--", new Vector2(50, 20), () =>
        {
            stats.DecStatAmount(Stats.Type.Defence);
        });
    }

    

}

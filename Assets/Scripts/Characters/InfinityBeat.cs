using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfinityBeat : BeatFormation
{
    [SerializeField]
    private Formation formation;

    public override List<FormationInfos> GetBeat()
    {
        List<FormationInfos> ret = new List<FormationInfos>();
        FormationInfos infos = new FormationInfos();

        infos.color = (ColorPicker)Random.Range(1, 5);
        infos.formation = formation;

        ret.Add(infos);
        return ret;
    }

    public override List<FormationInfos> GetHalfBeat()
    {
        return null;
    }

    public override bool End()
    {
        return false;
    }
}

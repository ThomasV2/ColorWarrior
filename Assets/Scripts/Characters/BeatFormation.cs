using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Beat
{
    BEAT,
    HALF_BEAT
};

public enum ColorPicker
{
    RANDOM,
    LEFT,
    MLEFT,
    MRIGHT,
    RIGHT,
    CONST_RAND_1,
    CONST_RAND_2,
    CONST_RAND_3,
    CONST_RAND_4,
};

public class BeatFormation : MonoBehaviour
{
    [System.Serializable]
    protected class BeatInfo
    {
        public Beat beat = Beat.BEAT;
        public List<FormationInfos> formations = new List<FormationInfos>();
    }

    [System.Serializable]
    public class FormationInfos
    {
        public Formation formation;
        public ColorPicker color = ColorPicker.RANDOM;
    }

    [SerializeField]
    protected List<BeatInfo> formations;
    protected int idx;

    public void Init()
    {
        idx = 0;
    }

    public virtual List<FormationInfos> GetBeat()
    {
        if (idx < formations.Count && formations[idx].beat == Beat.BEAT)
        {
            return formations[idx++].formations;
        }
        return null;
    }

    public virtual List<FormationInfos> GetHalfBeat()
    {
        if (idx < formations.Count && formations[idx].beat == Beat.HALF_BEAT)
        {
            return formations[idx++].formations;
        }
        return null;
    }

    public virtual bool End()
    {
        return (idx >= formations.Count);
    }
}
